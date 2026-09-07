using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EchoForge.Sim;
using EchoForge.Audio;

namespace EchoForge.Chamber
{
    /// <summary>Plays back a deterministic SimResult.Trace as visible acoustic wave packets:
    /// short curved pressure ribbons that travel the traced path, never straight laser beams.
    /// Purely cosmetic - it reads the result, it cannot change it.</summary>
    public sealed class PulseVisualizer : MonoBehaviour
    {
        [SerializeField] float beatSeconds = 0.28f;
        [SerializeField] Color pulseColor = new Color(0.72f, 0.66f, 0.95f);
        [SerializeField] Transform boardRoot;

        readonly List<GameObject> _spawned = new();

        public float BeatSeconds => beatSeconds;

        public void Configure(Transform board) => boardRoot = board;

        public void PlayResult(SimResult result, ChamberGrid grid, Action<Segment> onSegmentEnd, Action onComplete)
        {
            StopAllCoroutines();
            DestroyPackets();
            StartCoroutine(Runner(result, grid, onSegmentEnd, onComplete));
        }

        IEnumerator Runner(SimResult result, ChamberGrid grid, Action<Segment> onSegmentEnd, Action onComplete)
        {
            yield return Play(result, grid, onSegmentEnd);
            onComplete?.Invoke();
        }

        IEnumerator Play(SimResult result, ChamberGrid grid, Action<Segment> onSegmentEnd)
        {
            DestroyPackets();
            if (result == null || result.Trace.Count == 0) yield break;

            var segments = new List<Segment>(result.Trace);
            segments.Sort((a, b) => a.startBeat != b.startBeat
                ? a.startBeat.CompareTo(b.startBeat)
                : a.branch.CompareTo(b.branch));

            float t = 0f;
            int idx = 0;
            var running = new List<(Segment seg, float endT)>();

            int lastBeat = 0;
            foreach (var s in segments) lastBeat = Mathf.Max(lastBeat, s.endBeat);
            float totalT = (lastBeat + 1) * beatSeconds;

            while (t < totalT || running.Count > 0)
            {
                while (idx < segments.Count && segments[idx].startBeat * beatSeconds <= t + 0.0001f)
                {
                    var seg = segments[idx++];
                    float dur = Mathf.Max(0.05f, (seg.endBeat - seg.startBeat) * beatSeconds);
                    StartCoroutine(MovePacket(seg, grid, dur));
                    running.Add((seg, t + dur));
                }

                for (int i = running.Count - 1; i >= 0; i--)
                {
                    if (t >= running[i].endT)
                    {
                        onSegmentEnd?.Invoke(running[i].seg);
                        running.RemoveAt(i);
                    }
                }

                t += Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(beatSeconds);
            DestroyPackets();
        }

        IEnumerator MovePacket(Segment seg, ChamberGrid grid, float dur)
        {
            var go = MakePacket();
            _spawned.Add(go);
            Vector3 a = grid.Local(seg.from, 0.35f);
            Vector3 b = grid.Local(seg.to, 0.35f);
            var tr = go.transform;
            tr.localPosition = a;
            tr.localRotation = Quaternion.LookRotation((b - a).sqrMagnitude > 0.0001f ? (b - a) : Vector3.forward);

            float e = 0f;
            while (e < dur)
            {
                e += Time.deltaTime;
                float k = Mathf.Clamp01(e / dur);
                tr.localPosition = Vector3.Lerp(a, b, k);
                yield return null;
            }
            tr.localPosition = b;
            // brief lingering wavefront, then fade with the batch Clear()
        }

        GameObject MakePacket()
        {
            var go = new GameObject("wave_packet");
            go.transform.SetParent(boardRoot != null ? boardRoot : transform, false);

            var tr = go.AddComponent<TrailRenderer>();
            tr.time = beatSeconds * 1.6f;
            tr.startWidth = 0.42f;
            tr.endWidth = 0.02f;
            tr.numCapVertices = 4;
            tr.minVertexDistance = 0.03f;
            var mat = new Material(Shader.Find("Sprites/Default"));
            mat.color = pulseColor;
            tr.material = mat;
            var grad = new Gradient();
            grad.SetKeys(
                new[] { new GradientColorKey(pulseColor, 0f), new GradientColorKey(pulseColor * 0.4f, 1f) },
                new[] { new GradientAlphaKey(0.9f, 0f), new GradientAlphaKey(0f, 1f) });
            tr.colorGradient = grad;
            return go;
        }

        void DestroyPackets()
        {
            foreach (var g in _spawned) if (g != null) Destroy(g);
            _spawned.Clear();
        }

        public void Clear()
        {
            StopAllCoroutines();
            DestroyPackets();
        }
    }
}
