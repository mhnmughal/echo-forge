using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using EchoForge.Chamber;
using EchoForge.Game;
using EchoForge.Menu;

namespace EchoForge.EditorTools
{
    /// <summary>App Store Connect screenshot pass. From Play Mode, renders five chosen views at
    /// the two mandatory landscape resolutions (iPhone 6.9 in 2868x1320, iPad 13 in 2752x2064)
    /// with ScreenCapture, into Screenshots/AppStore/. Providing the largest sizes is enough;
    /// App Store Connect scales down for smaller devices. Preserves the local save file.</summary>
    public static class StoreShots
    {
        const BindingFlags Priv = BindingFlags.NonPublic | BindingFlags.Instance;

        static readonly (string label, int w, int h)[] Sizes =
        {
            ("iPhone_6_9", 2868, 1320),
            ("iPad_13",    2752, 2064),
        };

        // id, isChamber, levelId, whether to run a pulse for the wavefront
        static readonly (string id, bool chamber, int level, bool pulse)[] Shots =
        {
            ("01_hall",     false, 0,  false),
            ("02_chamber",  true,  2,  false),
            ("03_pulse",    true,  5,  true),
            ("04_network",  true,  11, false),
            ("05_dial",     false, 0,  false),
        };

        static int _shot, _size, _phase, _frame;
        static double _next;
        static byte[] _save;
        static string _savePath;
        public static string Status { get; private set; } = "idle";

        [MenuItem("Echo Forge/QA/App Store Screenshots (enter Play Mode first)")]
        public static void Start()
        {
            if (!EditorApplication.isPlaying)
            {
                Debug.LogWarning("StoreShots: enter Play Mode, then run this menu item again.");
                return;
            }
            _savePath = Path.Combine(Application.persistentDataPath, "echoforge_save.json");
            _save = File.Exists(_savePath) ? File.ReadAllBytes(_savePath) : null;
            SeedProgress();
            Directory.CreateDirectory("Screenshots/AppStore");
            _shot = _size = _phase = 0;
            _next = 0;
            Status = "running";
            Application.runInBackground = true;
            EditorApplication.update -= Tick;
            EditorApplication.update += Tick;
            Debug.Log("StoreShots: capturing " + (Shots.Length * Sizes.Length) + " frames into Screenshots/AppStore/");
        }

        static void SeedProgress()
        {
            // A lived-in progression so the menu and dial read well: tutorial done, 1-6 cleared.
            SaveSystem.RecordResult(0, 3, 3, true);
            int[] ratings = { 3, 2, 3, 2, 3, 2 };
            int[] tunes = { 3, 5, 4, 7, 5, 6 };
            for (int i = 1; i <= 6; i++) SaveSystem.RecordResult(i, ratings[i - 1], tunes[i - 1], false);
        }

        static void Tick()
        {
            if (!EditorApplication.isPlaying) { Finish("stopped"); return; }
            if (EditorApplication.timeSinceStartup < _next) return;
            try
            {
                var shot = Shots[_shot];
                switch (_phase)
                {
                    case 0: // load the scene for this shot
                        Time.timeScale = 1f;
                        if (shot.chamber) SceneFlow.LoadChamber(shot.level);
                        else SceneFlow.LoadHall();
                        _phase = 1;
                        _next = Now + 1.0;
                        break;

                    case 1: // arrange the static view
                        Prepare(shot);
                        _phase = 2;
                        _next = Now + 0.6;
                        break;

                    case 2: // set the target resolution
                        ResponsiveQA.SetGameViewSize(Sizes[_size].w, Sizes[_size].h, "ASC_" + Sizes[_size].label);
                        _frame = Time.frameCount;
                        _phase = shot.pulse ? 25 : 3;   // pulse shots go through the pulse phase
                        _next = Now + 0.5;
                        break;

                    case 25: // pulse shot: fire a fresh pulse at this resolution, then grab it in flight
                        if (Time.frameCount < _frame + 3) return;
                        var lcp = UnityEngine.Object.FindFirstObjectByType<LevelController>();
                        lcp.OnRetunePressed();
                        ReleaseQA.Tune(lcp);
                        lcp.OnPulsePressed();
                        _frame = Time.frameCount;
                        _phase = 3;
                        _next = Now + 0.30;             // wavefront ~1-2 cells out
                        break;

                    case 3: // capture once the frame has rendered
                        if (Time.frameCount < _frame + (shot.pulse ? 1 : 4)) return;
                        var capture = Type.GetType("UnityEngine.ScreenCapture, UnityEngine.ScreenCaptureModule");
                        var path = Path.GetFullPath($"Screenshots/AppStore/{Sizes[_size].label}_{shot.id}_{Sizes[_size].w}x{Sizes[_size].h}.png");
                        capture.GetMethod("CaptureScreenshot", new[] { typeof(string), typeof(int) })
                               .Invoke(null, new object[] { path, 1 });
                        Status = $"{shot.id} @ {Sizes[_size].label}";
                        _phase = 4;
                        _next = Now + 0.8;
                        break;

                    default: // advance
                        if (++_size < Sizes.Length) { _phase = 2; }
                        else { _size = 0; _phase = 0; if (++_shot == Shots.Length) Finish("complete"); }
                        break;
                }
            }
            catch (Exception e) { Debug.LogException(e); Finish("failed: " + e.Message); }
        }

        static double Now => EditorApplication.timeSinceStartup;

        static void Prepare((string id, bool chamber, int level, bool pulse) shot)
        {
            if (shot.chamber) return; // fresh board; pulse shots fire per-resolution in phase 25
            var hall = UnityEngine.Object.FindFirstObjectByType<ResonanceHallController>();
            if (shot.id == "05_dial" && hall != null) hall.OnOpenDial();
        }

        static void Finish(string status)
        {
            EditorApplication.update -= Tick;
            Time.timeScale = 1f;
            if (_savePath != null)
            {
                if (_save == null) { if (File.Exists(_savePath)) File.Delete(_savePath); }
                else File.WriteAllBytes(_savePath, _save);
                SaveSystem.Load();
            }
            Status = status;
            Debug.Log("StoreShots: " + status + " -> Screenshots/AppStore/");
        }
    }
}
