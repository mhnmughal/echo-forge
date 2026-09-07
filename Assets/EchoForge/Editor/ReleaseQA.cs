using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using EchoForge.Chamber;
using EchoForge.Game;
using EchoForge.Menu;
using EchoForge.Level;

namespace EchoForge.EditorTools
{
    /// <summary>Editor-only repeatable captures of the actual game UI. Preserves local progress.</summary>
    public static class ReleaseQA
    {
        const BindingFlags Private = BindingFlags.NonPublic | BindingFlags.Instance;
        static readonly string[] States = { "Hall", "Dial", "Settings", "Reset", "Guide", "Finale", "SoundCheck", "Chamber", "Selected", "Pause", "ChamberGuide", "Failure", "Success" };
        static int _state, _aspect, _phase, _frame;
        static double _next;
        static byte[] _save;
        static string _savePath;
        public static string Status { get; private set; } = "idle";

        public static void StartCaptures()
        {
            if (!EditorApplication.isPlaying) throw new InvalidOperationException("Enter Play Mode first.");
            _savePath = Path.Combine(Application.persistentDataPath, "echoforge_save.json");
            _save = File.Exists(_savePath) ? File.ReadAllBytes(_savePath) : null;
            Directory.CreateDirectory("QA/Responsive");
            _state = _aspect = _phase = 0;
            _next = 0;
            Status = "running";
            Application.runInBackground = true;
            EditorApplication.update -= Tick;
            EditorApplication.update += Tick;
        }

        static void Tick()
        {
            if (!EditorApplication.isPlaying) { Finish("stopped"); return; }
            if (EditorApplication.timeSinceStartup < _next) return;
            try
            {
                if (_phase == 0)
                {
                    Time.timeScale = 1;
                    if (_state <= 5) SceneFlow.LoadHall();
                    else SceneFlow.LoadChamber(_state == 6 ? 0 : 11);
                    _phase = 1;
                    _next = EditorApplication.timeSinceStartup + 0.8;
                }
                else if (_phase == 1)
                {
                    Prepare(States[_state]);
                    _phase = 2;
                    _next = EditorApplication.timeSinceStartup + ((_state == 11 || _state == 12) ? 12 : 0.5);
                }
                else if (_phase == 2)
                {
                    var t = ResponsiveQA.Targets[_aspect];
                    ResponsiveQA.SetGameViewSize(t.w, t.h, "EF_" + t.label);
                    _frame = Time.frameCount;
                    _phase = 3;
                    _next = EditorApplication.timeSinceStartup + 0.5;
                }
                else if (_phase == 3)
                {
                    if (Time.frameCount < _frame + 3) return;
                    var capture = Type.GetType("UnityEngine.ScreenCapture, UnityEngine.ScreenCaptureModule");
                    var path = Path.GetFullPath("QA/Responsive/" + States[_state] + "_" + ResponsiveQA.Targets[_aspect].label + ".png");
                    capture.GetMethod("CaptureScreenshot", new[] { typeof(string), typeof(int) }).Invoke(null, new object[] { path, 1 });
                    Status = States[_state] + " " + ResponsiveQA.Targets[_aspect].label;
                    _phase = 4;
                    _next = EditorApplication.timeSinceStartup + 0.5;
                }
                else
                {
                    if (++_aspect < ResponsiveQA.Targets.Length) _phase = 2;
                    else { _aspect = 0; _phase = 0; if (++_state == States.Length) Finish("complete"); }
                }
            }
            catch (Exception e) { Debug.LogException(e); Finish("failed: " + e.Message); }
        }

        static void Prepare(string state)
        {
            var hall = UnityEngine.Object.FindFirstObjectByType<ResonanceHallController>();
            var lc = UnityEngine.Object.FindFirstObjectByType<LevelController>();
            switch (state)
            {
                case "Dial": hall.OnOpenDial(); break;
                case "Settings": hall.OnOpenSettings(); break;
                case "Reset": hall.OnOpenSettings(); UnityEngine.Object.FindFirstObjectByType<TuningStripController>().OnResetRequested(); break;
                case "Guide": hall.OnOpenSoundGuide(); break;
                case "Finale":
                    // Preview the earned-state UI only; this is not completion QA.
                    SaveSystem.Data.chamberInTune = true;
                    SaveSystem.Data.finaleSeen = true;
                    var emblem = (GameObject)typeof(ResonanceHallController).GetField("chamberInTuneEmblem", Private).GetValue(hall);
                    emblem.SetActive(true); hall.OnRingTapped(); break;
                case "Selected":
                    Tap(lc, UnityEngine.Object.FindObjectsByType<ComponentView>(FindObjectsSortMode.None).First(v => v.IsAdjustable)); break;
                case "Pause": lc.OnPausePressed(); break;
                case "ChamberGuide": lc.OnPausePressed(); lc.OnSoundGuideOpen(); break;
                case "Failure": lc.OnPulsePressed(); break;
                case "Success": Tune(lc); lc.OnPulsePressed(); break;
            }
        }

        static void Tap(LevelController lc, ComponentView view) => typeof(LevelController).GetMethod("OnComponentTapped", Private).Invoke(lc, new object[] { view });

        public static void Tune(LevelController lc)
        {
            var runtime = (ChamberRuntime)typeof(LevelController).GetField("_chamber", Private).GetValue(lc);
            var views = UnityEngine.Object.FindObjectsByType<ComponentView>(FindObjectsSortMode.None).Where(v => v.IsAdjustable).OrderBy(v => v.AdjustableIndex).ToArray();
            for (int i = 0; i < views.Length; i++)
            {
                var target = runtime.Definition.optimalComponents[i];
                Tap(lc, views[i]);
                if (runtime.Adjustables[i].cell != target.cell)
                    typeof(LevelController).GetMethod("OnSocketTapped", Private).Invoke(lc, new object[] { target.cell });
                for (int n = 0; runtime.Adjustables[i].orient != target.orient && n < 4; n++) Tap(lc, views[i]);
            }
        }

        static void Finish(string status)
        {
            EditorApplication.update -= Tick;
            Time.timeScale = 1;
            if (_savePath != null)
            {
                if (_save == null) File.Delete(_savePath); else File.WriteAllBytes(_savePath, _save);
                SaveSystem.Load();
            }
            Status = status;
            Debug.Log("Echo Forge responsive capture: " + status);
        }
    }
}
