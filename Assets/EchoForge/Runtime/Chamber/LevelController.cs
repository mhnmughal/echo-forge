using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EchoForge.Sim;
using EchoForge.Level;
using EchoForge.Game;
using EchoForge.Audio;

namespace EchoForge.Chamber
{
    /// <summary>Drives one Echo Chamber: builds the data-driven board inside the authored
    /// environment, routes touch input, runs the deterministic pulse on PULSE, shows the
    /// result, and persists progression. No level-id branching - all content comes from the
    /// EchoLevelDefinition.</summary>
    [DefaultExecutionOrder(10)]
    public sealed class LevelController : MonoBehaviour
    {
        [Header("Scene refs (authored)")]
        [SerializeField] Camera cam;
        [SerializeField] Transform boardRoot;
        [SerializeField] ChamberInput input;
        [SerializeField] ChamberUI ui;
        [SerializeField] PulseVisualizer visualizer;
        [SerializeField] LevelCatalog catalog;

        [Header("Component materials")]
        [SerializeField] Material matEmitter;
        [SerializeField] Material matReflector;
        [SerializeField] Material matAbsorber;
        [SerializeField] Material matSplitter;
        [SerializeField] Material matDelay;
        [SerializeField] Material matReceiver;
        [SerializeField] Material matResonator;
        [SerializeField] Material matForbidden;
        [SerializeField] Material matWall;
        [SerializeField] Material matSocket;

        [Header("Layout")]
        [SerializeField] float cellSize = 1f;

        EchoLevelDefinition _def;
        ChamberRuntime _chamber;
        ChamberGrid _grid;

        readonly List<GameObject> _spawned = new();
        readonly List<ComponentView> _adjustableViews = new();
        readonly List<GameObject> _socketMarkers = new();
        readonly Dictionary<Vector2Int, ComponentView> _viewByCell = new();

        ComponentView _selected;
        bool _busy;
        bool _solved;
        int _tutorialStep;
        string[] _tutorialLines = System.Array.Empty<string>();
        Coroutine _tutorialRoutine;
        float _cameraAspect;

        void Start()
        {
            if (catalog == null) catalog = LevelCatalog.Instance;
            _def = catalog != null ? catalog.ById(GameSession.CurrentLevelId) : null;
            if (_def == null && catalog != null && catalog.Count > 0) _def = catalog.levels[0];
            if (_def == null)
            {
                Debug.LogError("[EchoForge] No level definition available.");
                enabled = false;
                return;
            }

            _chamber = new ChamberRuntime(_def);
            _grid = new ChamberGrid(_def.chamberSize, cellSize);

            if (cam == null) cam = Camera.main;
            if (visualizer != null) visualizer.Configure(boardRoot);
            if (input != null)
            {
                input.Configure(cam);
                input.ComponentTapped += OnComponentTapped;
                input.SocketTapped += OnSocketTapped;
                input.BackgroundTapped += OnBackgroundTapped;
            }

            BuildBoard();
            FitCamera();
            if (ui != null) ui.SetTunes(0);

            AudioController.Instance?.PlayAmbience();

            if (_def.isTutorial)
            {
                _tutorialLines = _def.tutorialText.Split('\n');
                _tutorialStep = 0;
                _tutorialRoutine = StartCoroutine(TutorialIntro());
            }
            else if (ui != null)
            {
                ui.ShowContext($"CHAMBER {_def.levelId:00}", _def.title);
            }
        }

        void OnDestroy()
        {
            AudioController.Instance?.SetDamped(false);
            if (input == null) return;
            input.ComponentTapped -= OnComponentTapped;
            input.SocketTapped -= OnSocketTapped;
            input.BackgroundTapped -= OnBackgroundTapped;
        }

        void LateUpdate()
        {
            if (cam != null && !Mathf.Approximately(_cameraAspect, cam.aspect)) FitCamera();
        }

        void FitCamera()
        {
            if (cam == null || _def == null) return;
            _cameraAspect = cam.aspect;
            // Keep the data-driven board above the actuator at every landscape aspect.
            var target = boardRoot.position - cam.transform.up * 1.2f;
            float halfW = _def.chamberSize.x * cellSize * 0.5f + 0.5f;
            float halfH = _def.chamberSize.y * cellSize * 0.5f + 0.5f;
            for (float distance = 14f; distance <= 45f; distance += 0.25f)
            {
                cam.transform.position = target - cam.transform.forward * distance;
                bool fits = true;
                for (int i = 0; i < 8; i++)
                {
                    var p = boardRoot.TransformPoint(new Vector3((i & 1) == 0 ? -halfW : halfW,
                        (i & 2) == 0 ? 0f : 1.4f, (i & 4) == 0 ? -halfH : halfH));
                    var v = cam.WorldToViewportPoint(p);
                    if (v.x < 0.07f || v.x > 0.93f || v.y < 0.24f || v.y > 0.9f) fits = false;
                }
                if (fits) break;
            }
        }

        // ---------------- board construction ----------------

        void BuildBoard()
        {
            foreach (var e in _def.emitters)
                Spawn(CompType.Emitter, e.cell, -1, 0, e.dir, 0, false);

            foreach (var c in _def.fixedComponents)
                Spawn(c.type, c.cell, -1, c.orient, Dir.N, c.receiverId, c.forbidden);

            _adjustableViews.Clear();
            for (int i = 0; i < _chamber.Adjustables.Count; i++)
            {
                var c = _chamber.Adjustables[i];
                var v = Spawn(c.type, c.cell, i, c.orient, Dir.N, c.receiverId, c.forbidden);
                _adjustableViews.Add(v);
            }

            foreach (var s in _def.movableSockets)
            {
                var go = MakePrimitive(PrimitiveType.Cylinder, matSocket, new Vector3(0.7f, 0.02f, 0.7f));
                go.name = $"socket_{s.x}_{s.y}";
                go.transform.SetParent(boardRoot, false);
                go.transform.localPosition = _grid.Local(s, 0.01f);
                var col = go.GetComponent<Collider>(); if (col) col.isTrigger = true;
                go.AddComponent<SocketMarker>().Cell = s;
                go.SetActive(false);
                _socketMarkers.Add(go);
                _spawned.Add(go);
            }
        }

        ComponentView Spawn(CompType type, Vector2Int cell, int adjIndex, int orient, Dir dir, int receiverId, bool forbidden)
        {
            var root = new GameObject($"{type}_{cell.x}_{cell.y}");
            root.transform.SetParent(boardRoot, false);
            root.transform.localPosition = _grid.Local(cell, 0f);
            _spawned.Add(root);

            var box = root.AddComponent<BoxCollider>();
            box.size = new Vector3(0.9f, 0.9f, 0.9f);
            box.center = new Vector3(0f, 0.45f, 0f);

            var view = root.AddComponent<ComponentView>();
            view.Init(type, adjIndex, receiverId, forbidden, cell);

            var spinner = new GameObject("Spinner").transform;
            spinner.SetParent(root.transform, false);

            Transform mesh;
            Renderer accent = null;

            switch (type)
            {
                case CompType.Emitter:
                    mesh = MakePrimitive(PrimitiveType.Cylinder, matEmitter, new Vector3(0.55f, 0.28f, 0.55f)).transform;
                    mesh.SetParent(spinner, false); mesh.localPosition = new Vector3(0, 0.28f, 0);
                    var nub = MakePrimitive(PrimitiveType.Cube, matEmitter, new Vector3(0.14f, 0.14f, 0.5f)).transform;
                    nub.SetParent(spinner, false); nub.localPosition = new Vector3(0, 0.28f, 0.35f);
                    spinner.localRotation = Quaternion.Euler(0, (int)dir * 45f, 0);
                    AddGlowLight(root.transform, new Color(0.95f, 0.62f, 0.3f), 1.4f);
                    break;

                case CompType.Reflector:
                    mesh = MakePrimitive(PrimitiveType.Cube, matReflector, new Vector3(1.05f, 0.5f, 0.12f)).transform;
                    mesh.SetParent(spinner, false); mesh.localPosition = new Vector3(0, 0.3f, 0);
                    break;

                case CompType.Absorber:
                    mesh = MakePrimitive(PrimitiveType.Cube, matAbsorber, new Vector3(0.86f, 0.6f, 0.86f)).transform;
                    mesh.SetParent(spinner, false); mesh.localPosition = new Vector3(0, 0.3f, 0);
                    break;

                case CompType.Splitter:
                    var l = MakePrimitive(PrimitiveType.Cube, matSplitter, new Vector3(0.9f, 0.45f, 0.1f)).transform;
                    l.SetParent(spinner, false); l.localPosition = new Vector3(0, 0.3f, 0); l.localRotation = Quaternion.Euler(0, 45, 0);
                    var rr = MakePrimitive(PrimitiveType.Cube, matSplitter, new Vector3(0.9f, 0.45f, 0.1f)).transform;
                    rr.SetParent(spinner, false); rr.localPosition = new Vector3(0, 0.3f, 0); rr.localRotation = Quaternion.Euler(0, -45, 0);
                    mesh = spinner;
                    break;

                case CompType.DelayPlate:
                    mesh = MakePrimitive(PrimitiveType.Cube, matDelay, new Vector3(0.9f, 0.14f, 0.9f)).transform;
                    mesh.SetParent(spinner, false); mesh.localPosition = new Vector3(0, 0.12f, 0);
                    break;

                case CompType.Receiver:
                {
                    var baseM = MakePrimitive(PrimitiveType.Cylinder, forbidden ? matForbidden : matReceiver, new Vector3(0.62f, 0.4f, 0.62f)).transform;
                    baseM.SetParent(spinner, false); baseM.localPosition = new Vector3(0, 0.4f, 0);
                    var glow = MakePrimitive(PrimitiveType.Sphere, forbidden ? matForbidden : matReceiver, new Vector3(0.42f, 0.42f, 0.42f));
                    glow.transform.SetParent(spinner, false); glow.transform.localPosition = new Vector3(0, 0.92f, 0);
                    accent = glow.GetComponent<Renderer>();
                    AddGlowLight(root.transform, forbidden ? new Color(0.7f, 0.12f, 0.16f) : new Color(0.95f, 0.6f, 0.22f), 1.6f);
                    mesh = baseM;
                    break;
                }

                case CompType.Resonator:
                {
                    var baseM = MakePrimitive(PrimitiveType.Cylinder, matResonator, new Vector3(0.64f, 0.55f, 0.64f)).transform;
                    baseM.SetParent(spinner, false); baseM.localPosition = new Vector3(0, 0.55f, 0);
                    var ring = MakePrimitive(PrimitiveType.Sphere, matResonator, new Vector3(0.46f, 0.2f, 0.46f));
                    ring.transform.SetParent(spinner, false); ring.transform.localPosition = new Vector3(0, 1.15f, 0);
                    accent = ring.GetComponent<Renderer>();
                    AddGlowLight(root.transform, new Color(0.95f, 0.62f, 0.24f), 2.0f);
                    mesh = baseM;
                    break;
                }

                default: // Wall
                    mesh = MakePrimitive(PrimitiveType.Cube, matWall, new Vector3(1f, 1f, 1f)).transform;
                    mesh.SetParent(spinner, false); mesh.localPosition = new Vector3(0, 0.5f, 0);
                    break;
            }

            var ring2 = MakePrimitive(PrimitiveType.Cylinder, matSocket, new Vector3(1.05f, 0.015f, 1.05f));
            ring2.name = "SelectionRing";
            ring2.transform.SetParent(root.transform, false);
            ring2.transform.localPosition = new Vector3(0, 0.02f, 0);
            var rc = ring2.GetComponent<Collider>(); if (rc) Destroy(rc);
            ring2.SetActive(false);

            view.BindVisuals(spinner, ring2, accent);
            if (type == CompType.Reflector || type == CompType.Splitter) view.ApplyOrientation(orient);
            _viewByCell[cell] = view;
            return view;
        }

        static void AddGlowLight(Transform parent, Color color, float intensity)
        {
            var go = new GameObject("Glow");
            go.transform.SetParent(parent, false);
            go.transform.localPosition = new Vector3(0, 0.9f, 0);
            var l = go.AddComponent<Light>();
            l.type = LightType.Point;
            l.color = color;
            l.intensity = intensity;
            l.range = 2.7f;
            l.shadows = LightShadows.None;
        }

        GameObject MakePrimitive(PrimitiveType type, Material mat, Vector3 scale)
        {
            var go = GameObject.CreatePrimitive(type);
            go.transform.localScale = scale;
            var r = go.GetComponent<Renderer>();
            if (r != null && mat != null) r.sharedMaterial = mat;
            return go;
        }

        // ---------------- interaction ----------------

        void OnComponentTapped(ComponentView view)
        {
            if (_busy) return;
            ShowContextFor(view);

            if (!view.IsAdjustable) return;
            int idx = view.AdjustableIndex;

            if (_chamber.CanRotate(idx))
            {
                if (_selected == view) RotateSelected();
                else Select(view);
            }
            else
            {
                Select(view);
            }
            SetSocketsVisible(_chamber.CanMove(idx));

            NotifyTutorial(TutEvent.Selected);
        }

        void OnSocketTapped(Vector2Int cell)
        {
            if (_busy || _selected == null || !_selected.IsAdjustable) return;
            int idx = _selected.AdjustableIndex;
            if (!_chamber.CanMoveTo(idx, cell)) return;

            _chamber.MoveTo(idx, cell);
            _viewByCell.Remove(_selected.Cell);
            _selected.SetCell(cell);
            _viewByCell[cell] = _selected;
            _selected.transform.localPosition = _grid.Local(cell, 0f);
            if (ui != null) ui.SetTunes(_chamber.Tunes);
            AudioController.Instance?.Play(Sfx.ComponentSocket);
            Haptics.Play(Haptic.Light);
            SetSocketsVisible(false);
            ShowContextFor(_selected);
            NotifyTutorial(TutEvent.Rotated);
        }

        void OnBackgroundTapped()
        {
            if (_busy) return;
            Deselect();
            if (ui != null && !_def.isTutorial) ui.ShowContext($"CHAMBER {_def.levelId:00}", _def.title);
        }

        void Select(ComponentView view)
        {
            if (_selected == view) return;
            Deselect();
            _selected = view;
            view.SetSelected(true);
            AudioController.Instance?.Play(Sfx.ComponentPickup);
            Haptics.Play(Haptic.Light);
        }

        void Deselect()
        {
            SetSocketsVisible(false);
            if (_selected != null) _selected.SetSelected(false);
            _selected = null;
        }

        void RotateSelected()
        {
            if (_selected == null || !_selected.IsAdjustable) return;
            int idx = _selected.AdjustableIndex;
            _chamber.Rotate(idx, 1);
            _selected.ApplyOrientation(_chamber.Adjustables[idx].orient);
            if (ui != null) ui.SetTunes(_chamber.Tunes);
            AudioController.Instance?.Play(Sfx.ComponentRotate);
            Haptics.Play(Haptic.Light);
            ShowContextFor(_selected);
            NotifyTutorial(TutEvent.Rotated);
        }

        void SetSocketsVisible(bool on)
        {
            foreach (var m in _socketMarkers) if (m != null) m.SetActive(on);
        }

        void ShowContextFor(ComponentView view)
        {
            if (ui == null) return;
            string detail = view.Type switch
            {
                CompType.Reflector => $"ANGLE {ReflectorAngle(CurrentOrient(view))}",
                CompType.Splitter => "FORKS THE PULSE",
                CompType.Absorber => "STOPS THE PULSE",
                CompType.DelayPlate => "HOLDS THE PULSE",
                CompType.Receiver => view.Forbidden ? "MUST STAY SILENT" : "NEEDS A PULSE",
                CompType.Resonator => "NEEDS TWO PULSES",
                CompType.Emitter => "THE SOURCE",
                _ => ""
            };
            ui.ShowContext(view.Type.ToString().ToUpperInvariant(), detail);
        }

        int CurrentOrient(ComponentView view) =>
            view.IsAdjustable ? _chamber.Adjustables[view.AdjustableIndex].orient : 0;

        static string ReflectorAngle(int orient) => orient switch { 0 => "45", 1 => "135", 2 => "90", _ => "0" };

        // ---------------- pulse ----------------

        public void OnPulsePressed()
        {
            if (_busy || _solved) return;
            _busy = true;
            if (input != null) input.InputEnabled = false;
            Deselect();
            if (ui != null) { ui.HideContext(); ui.HideResult(); }

            AudioController.Instance?.Play(Sfx.PulseActuator);
            AudioController.Instance?.Play(Sfx.EmitterPulse, 1f, 0.9f);
            foreach (var emitter in _def.emitters)
                if (_viewByCell.TryGetValue(emitter.cell, out var source)) source.React(SegEnd.Receiver);
            Haptics.Play(Haptic.Medium);

            var result = _chamber.Pulse();
            if (visualizer != null)
                visualizer.PlayResult(result, _grid, OnSegmentEnd, () => OnPulseComplete(result));
            else
                OnPulseComplete(result);
        }

        void OnSegmentEnd(Segment seg)
        {
            if (_viewByCell.TryGetValue(seg.to, out var v))
            {
                v.React(seg.end);
                if (seg.end is SegEnd.Receiver or SegEnd.Resonator or SegEnd.Forbidden)
                    v.SetActivated(true);
            }

            Sfx? cue = seg.end switch
            {
                SegEnd.Reflect => Sfx.ReflectorHit,
                SegEnd.Split => Sfx.SplitterBranch,
                SegEnd.Delay => Sfx.DelayCharge,
                SegEnd.Absorb => Sfx.AbsorberImpact,
                SegEnd.Receiver => Sfx.ReceiverActivate,
                SegEnd.Resonator => Sfx.Resonator,
                SegEnd.Forbidden => Sfx.ForbiddenReceiver,
                _ => (Sfx?)null
            };
            if (cue.HasValue) AudioController.Instance?.Play(cue.Value);
        }

        void OnPulseComplete(SimResult result)
        {
            _busy = false;

            if (result.Success)
            {
                _solved = true;
                var rating = _chamber.Rate(true);
                SaveSystem.RecordResult(_def.levelId, (int)rating, _chamber.Tunes, _def.isTutorial);

                string title = _def.levelId == 12 ? "CHAMBER IN TUNE"
                    : _def.isTutorial ? "SIGNAL LOCKED"
                    : "RESONANCE ACHIEVED";
                bool showNext = _def.levelId < 12;
                if (ui != null) ui.ShowResult(title, _def.isTutorial ? "Enter Chamber 01" : null, RatingText(rating), showNext);

                AudioController.Instance?.Play(_def.levelId == 12 ? Sfx.FinalResonance : Sfx.Success);
                Haptics.Play(Haptic.Heavy);
                NotifyTutorial(TutEvent.Solved);
            }
            else
            {
                string title = result.Fail == FailReason.ForbiddenReceiver ? "SIGNAL LOST" : "OUT OF PHASE";
                if (ui != null) ui.ShowResult(title, FailText(result.Fail), null, false);
                AudioController.Instance?.Play(Sfx.Failure);
                Haptics.Play(Haptic.Medium);
            }
        }

        static string RatingText(Rating r) => r switch
        {
            Rating.PureResonance => "PURE RESONANCE",
            Rating.CleanEcho => "CLEAN ECHO",
            Rating.Resolved => "RESOLVED",
            _ => ""
        };

        static string FailText(FailReason f) => f switch
        {
            FailReason.ReceiverMissed => "Receiver Missed",
            FailReason.ForbiddenReceiver => "Forbidden Receiver Triggered",
            FailReason.TimingIncorrect => "Timing Incorrect",
            FailReason.ResonanceIncomplete => "Resonance Incomplete",
            _ => ""
        };

        // ---------------- UI button callbacks (wired in scene) ----------------

        public void OnRetunePressed()
        {
            _solved = false;
            if (ui != null) ui.HideResult();
            visualizer?.Clear();
            foreach (var v in _viewByCell.Values) v.SetActivated(false);
            _busy = false;
            if (input != null) input.InputEnabled = true;
        }

        public void OnNextPressed() => SceneFlow.NextChamber();
        public void OnDialPressed() => SceneFlow.LoadHall();

        public void OnPausePressed()
        {
            Time.timeScale = 0f;
            if (input != null) input.InputEnabled = false;
            if (ui != null) ui.ShowPause(true);
            AudioController.Instance?.Play(Sfx.PauseDamping);
            AudioController.Instance?.SetDamped(true);
        }

        public void OnResumePressed()
        {
            AudioController.Instance?.SetDamped(false);
            Time.timeScale = 1f;
            if (ui != null) ui.ShowPause(false);
            if (input != null) input.InputEnabled = !_busy && !_solved;
        }

        public void OnRestartPressed() { Time.timeScale = 1f; SceneFlow.ReloadChamber(); }
        public void OnExitSessionPressed() { Time.timeScale = 1f; SceneFlow.LoadHall(); }
        public void OnSoundGuideOpen() { if (ui != null) ui.ShowSoundGuide(true); }
        public void OnSoundGuideClose() { if (ui != null) ui.ShowSoundGuide(false); }

        // ---------------- tutorial ----------------

        enum TutEvent { Selected, Rotated, Solved }

        IEnumerator TutorialIntro()
        {
            if (ui == null) yield break;
            for (int i = 0; i < 2 && i < _tutorialLines.Length; i++)
            {
                _tutorialStep = i;
                ui.ShowTutorial(_tutorialLines[i]);
                yield return WaitRealtime(1.8f);
            }
            _tutorialStep = 2;
            if (_tutorialLines.Length > 2) ui.ShowTutorial(_tutorialLines[2]);
        }

        void NotifyTutorial(TutEvent e)
        {
            if (!_def.isTutorial || ui == null) return;
            if (_tutorialStep == 2 && e == TutEvent.Selected && _tutorialLines.Length > 3)
            {
                _tutorialStep = 3;
                ui.ShowTutorial(_tutorialLines[3]);
            }
            else if (_tutorialStep == 3 && e == TutEvent.Rotated && _tutorialLines.Length > 4)
            {
                _tutorialStep = 4;
                ui.ShowTutorial(_tutorialLines[4]);
            }
            else if (e == TutEvent.Solved)
            {
                if (_tutorialRoutine != null) StopCoroutine(_tutorialRoutine);
                ui.HideTutorial();
            }
        }

        static IEnumerator WaitRealtime(float seconds)
        {
            float t = 0f;
            while (t < seconds) { t += Time.unscaledDeltaTime; yield return null; }
        }
    }
}
