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
        Material _waveMaterial;

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
                        var ended = running[i].seg;
                        if (ended.end != SegEnd.Travel && ended.end != SegEnd.Exit)
                            StartCoroutine(Impact(ended, grid));
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

            // A delayed outgoing segment includes the hold in its duration. Stay at
            // the plate, then travel at the same speed as every other wave packet.
            float hold = Mathf.Max(0f, dur - beatSeconds);
            if (hold > 0f)
            {
                float charge = 0f;
                while (charge < hold)
                {
                    charge += Time.deltaTime;
                    tr.localScale = Vector3.one * (0.7f + 0.15f * Mathf.Sin(charge * 24f));
                    yield return null;
                }
                AudioController.Instance?.Play(Sfx.DelayRelease);
                tr.localScale = Vector3.one;
            }
            float travel = Mathf.Min(dur, beatSeconds);
            float e = 0f;
            while (e < travel)
            {
                e += Time.deltaTime;
                float k = Mathf.Clamp01(e / travel);
                tr.localPosition = Vector3.Lerp(a, b, k);
                yield return null;
            }
            tr.localPosition = b;
            go.GetComponent<LineRenderer>().enabled = false;
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
            if (_waveMaterial == null) _waveMaterial = new Material(Shader.Find("Sprites/Default")) { name = "EchoForge Wave Material" };
            tr.sharedMaterial = _waveMaterial;
            var grad = new Gradient();
            grad.SetKeys(
                new[] { new GradientColorKey(pulseColor, 0f), new GradientColorKey(pulseColor * 0.4f, 1f) },
                new[] { new GradientAlphaKey(0.9f, 0f), new GradientAlphaKey(0f, 1f) });
            tr.colorGradient = grad;
            // Curved pressure front in the board plane, perpendicular to travel.
            var front = go.AddComponent<LineRenderer>();
            front.sharedMaterial = _waveMaterial;
            front.useWorldSpace = false;
            front.positionCount = 17;
            front.widthMultiplier = 0.035f;
            front.startColor = front.endColor = pulseColor;
            for (int i = 0; i < front.positionCount; i++)
            {
                float angle = Mathf.Lerp(-1.05f, 1.05f, i / 16f);
                front.SetPosition(i, new Vector3(Mathf.Sin(angle) * 0.35f, 0f, (Mathf.Cos(angle) - 1f) * 0.35f));
            }
            return go;
        }

        IEnumerator Impact(Segment seg, ChamberGrid grid)
        {
            var go = MakePacket();
            _spawned.Add(go);
            go.name = "AcousticResponse_" + seg.end;
            go.GetComponent<TrailRenderer>().emitting = false;
            go.transform.localPosition = grid.Local(seg.to, 0.4f);
            var ring = go.GetComponent<LineRenderer>();
            ring.loop = true;
            ring.positionCount = 32;
            for (int i = 0; i < 32; i++)
            {
                float a = i * Mathf.PI / 16f;
                ring.SetPosition(i, new Vector3(Mathf.Sin(a), 0, Mathf.Cos(a)) * 0.3f);
            }
            Color color = seg.end == SegEnd.Forbidden ? new Color(0.8f, 0.2f, 0.25f) : pulseColor;
            for (float t = 0; t < 0.45f; t += Time.deltaTime)
            {
                float k = t / 0.45f;
                go.transform.localScale = Vector3.one * (seg.end == SegEnd.Absorb ? 1f - k : 1f + k * 1.3f);
                color.a = 1f - k;
                ring.startColor = ring.endColor = color;
                yield return null;
            }
            go.SetActive(false);
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

        void OnDestroy()
        {
            DestroyPackets();
            if (_waveMaterial != null) Destroy(_waveMaterial);
        }
    }
}
