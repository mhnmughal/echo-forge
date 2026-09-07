using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using EchoForge.Chamber;
using EchoForge.Menu;
using EchoForge.Level;
using EchoForge.Audio;
using B = EchoForge.EditorTools.EchoBuild;

namespace EchoForge.EditorTools
{
    /// <summary>Builds the three Echo Forge scenes + their support assets, at edit time.
    /// Re-runnable: overwrites the scene files and re-points the build settings.</summary>
    public static class EchoBuildScenes
    {
        [MenuItem("Echo Forge/Build/Everything", priority = 0)]
        public static void BuildAll()
        {
            BuildAssets();
            BuildBootstrap();
            BuildHall();
            BuildChamber();
            EditorBuildSettings.scenes = new[]
            {
                new EditorBuildSettingsScene($"{B.SceneDir}/Bootstrap.unity", true),
                new EditorBuildSettingsScene($"{B.SceneDir}/ResonanceHall.unity", true),
                new EditorBuildSettingsScene($"{B.SceneDir}/EchoChamber.unity", true),
            };
            AssetDatabase.SaveAssets();
            Debug.Log("<b>Echo Forge: built assets + 3 scenes, build settings set.</b>");
        }

        // ---------------------------------------------------------------- assets

        [MenuItem("Echo Forge/Build/1 - Assets")]
        public static void BuildAssets()
        {
            B.EnsureDirs();
            Mats();

            var catalog = LoadOrCreate<LevelCatalog>($"{B.ResDir}/LevelCatalog.asset");
            catalog.levels = new List<EchoLevelDefinition>();
            foreach (var guid in AssetDatabase.FindAssets("t:EchoLevelDefinition"))
                catalog.levels.Add(AssetDatabase.LoadAssetAtPath<EchoLevelDefinition>(AssetDatabase.GUIDToAssetPath(guid)));
            catalog.levels.Sort((a, b) => a.levelId.CompareTo(b.levelId));
            EditorUtility.SetDirty(catalog);

            var bank = LoadOrCreate<SoundBank>($"{B.ResDir}/SoundBank.asset");
            if (bank.clips == null || bank.clips.Length != 19) bank.clips = new AudioClip[19];
            EditorUtility.SetDirty(bank);

            AssetDatabase.SaveAssets();
            Debug.Log($"Echo Forge: assets built (catalog has {catalog.levels.Count} levels).");
        }

        static Dictionary<string, Material> Mats()
        {
            var m = new Dictionary<string, Material>
            {
                ["FloorEnv"] = B.Mat("Env_Floor", B.Graphite, 0.1f, 0.15f),
                ["WallEnv"] = B.Mat("Env_Wall", B.MidnightNavy, 0f, 0.05f),
                ["FabricEnv"] = B.Mat("Env_Fabric", B.Hex("241C24"), 0f, 0.03f),
                ["BronzeEnv"] = B.Mat("Env_Bronze", B.Bronze, 0.85f, 0.55f),
                ["Emitter"] = B.Mat("Comp_Emitter", B.Bronze, 0.85f, 0.55f, B.Amber * 1.15f),
                ["Reflector"] = B.Mat("Comp_Reflector", B.Bronze * 1.1f, 1f, 0.9f),
                ["Absorber"] = B.Mat("Comp_Absorber", B.Hex("191A20"), 0.15f, 0.42f),
                ["Splitter"] = B.Mat("Comp_Splitter", B.BronzeDark * 1.15f, 0.85f, 0.65f, B.PaleViolet * 0.5f),
                ["Delay"] = B.Mat("Comp_Delay", B.Graphite, 0.3f, 0.45f, B.PaleViolet * 0.85f),
                ["Receiver"] = B.Mat("Comp_Receiver", B.Hex("2A2320"), 0.25f, 0.55f, B.Amber * 0.7f),
                ["Resonator"] = B.Mat("Comp_Resonator", B.Bronze, 0.75f, 0.65f, B.Amber * 1.25f),
                ["Forbidden"] = B.Mat("Comp_Forbidden", B.Hex("2A1517"), 0.25f, 0.45f, B.Crimson * 0.9f),
                ["Wall"] = B.Mat("Comp_Wall", B.Graphite, 0.1f, 0.1f),
                ["Socket"] = B.Mat("Comp_Socket", B.MidnightNavy, 0.2f, 0.3f, B.PaleViolet * 0.15f),
            };
            return m;
        }

        static T LoadOrCreate<T>(string path) where T : ScriptableObject
        {
            var a = AssetDatabase.LoadAssetAtPath<T>(path);
            if (a == null)
            {
                a = ScriptableObject.CreateInstance<T>();
                AssetDatabase.CreateAsset(a, path);
            }
            return a;
        }

        static void SetRef(Object comp, string field, Object value)
        {
            var so = new SerializedObject(comp);
            var p = so.FindProperty(field);
            if (p == null) { Debug.LogWarning($"field '{field}' not found on {comp.GetType().Name}"); return; }
            p.objectReferenceValue = value;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        static void SetFloat(Object comp, string field, float value)
        {
            var so = new SerializedObject(comp);
            var p = so.FindProperty(field);
            if (p != null) { p.floatValue = value; so.ApplyModifiedPropertiesWithoutUndo(); }
        }

        static Scene NewScene()
        {
            var s = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            B.SetAmbient();
            return s;
        }

        static void Save(Scene s, string name)
        {
            B.EnsureDirs();
            EditorSceneManager.MarkSceneDirty(s);
            EditorSceneManager.SaveScene(s, $"{B.SceneDir}/{name}.unity");
        }

        static Camera MakeCamera(Vector3 pos, Vector3 euler, float fov, Color bg)
        {
            var go = new GameObject("ChamberCamera", typeof(Camera));
            go.tag = "MainCamera";
            go.transform.SetPositionAndRotation(pos, Quaternion.Euler(euler));
            var cam = go.GetComponent<Camera>();
            cam.fieldOfView = fov;
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = bg;
            cam.nearClipPlane = 0.1f;
            cam.farClipPlane = 200f;
            go.AddComponent<AudioListener>();
            return cam;
        }

        static GameObject AudioRig()
        {
            var go = new GameObject("Audio", typeof(AudioController));
            var amb = new GameObject("Ambience", typeof(AudioSource));
            amb.transform.SetParent(go.transform, false);
            var src = amb.GetComponent<AudioSource>();
            src.playOnAwake = false; src.loop = true; src.spatialBlend = 0f;
            var ac = go.GetComponent<AudioController>();
            SetRef(ac, "ambienceSource", src);
            SetRef(ac, "bank", AssetDatabase.LoadAssetAtPath<SoundBank>($"{B.ResDir}/SoundBank.asset"));

            var mixer = AssetDatabase.LoadAssetAtPath<UnityEngine.Audio.AudioMixer>(EchoPolish.MixerPath);
            if (mixer != null)
            {
                SetRef(ac, "mixer", mixer);
                var sfxG = mixer.FindMatchingGroups("SFX");
                var ambG = mixer.FindMatchingGroups("Ambience");
                if (sfxG.Length > 0) SetRef(ac, "sfxGroup", sfxG[0]);
                if (ambG.Length > 0) { SetRef(ac, "ambienceGroup", ambG[0]); src.outputAudioMixerGroup = ambG[0]; }
            }
            return go;
        }

        // ---------------------------------------------------------------- bootstrap

        [MenuItem("Echo Forge/Build/2 - Bootstrap Scene")]
        public static void BuildBootstrap()
        {
            var s = NewScene();
            var bcam = MakeCamera(new Vector3(0, 1, -10), Vector3.zero, 40f, B.MidnightNavy);
            B.AddPostVolume(bcam);
            AudioRig();
            B.EventSystem();

            var (_, safe) = B.Canvas("Canvas", false);
            B.Text(safe, "Title", "ECHO FORGE", 92, TextAlignmentOptions.Center, B.Ivory,
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0, 40), new Vector2(1200, 160));
            B.Text(safe, "Sub", "tune the room, not the character", 30, TextAlignmentOptions.Center,
                new Color(B.Bronze.r, B.Bronze.g, B.Bronze.b, 0.85f),
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0, -50), new Vector2(1200, 60));

            new GameObject("Systems", typeof(BootstrapController));
            Save(s, "Bootstrap");
            Debug.Log("Echo Forge: Bootstrap scene built.");
        }

        // ---------------------------------------------------------------- hall

        [MenuItem("Echo Forge/Build/3 - Resonance Hall Scene")]
        public static void BuildHall()
        {
            var s = NewScene();
            var m = Mats();
            var cam = MakeCamera(new Vector3(0, 7.4f, -11.2f), new Vector3(28, 0, 0), 44f, B.MidnightNavy);
            B.AddPostVolume(cam);
            AudioRig();
            B.EventSystem();

            // --- environment: circular acoustic room ---
            var env = new GameObject("HallEnvironment").transform;
            B.Primitive(PrimitiveType.Cylinder, "Floor", env, new Vector3(0, -0.2f, 2f), Vector3.zero,
                new Vector3(16, 0.2f, 16), m["FloorEnv"]);
            for (int i = 0; i < 3; i++)
                B.Primitive(PrimitiveType.Cylinder, $"Groove{i}", env, new Vector3(0, -0.06f, 2f), Vector3.zero,
                    new Vector3(5 + i * 3.4f, 0.12f, 5 + i * 3.4f), m["WallEnv"]);
            B.Primitive(PrimitiveType.Cylinder, "ListeningPlatform", env, new Vector3(0, -0.05f, 2f), Vector3.zero,
                new Vector3(4.4f, 0.22f, 4.4f), m["BronzeEnv"]);

            int walls = 14;
            for (int i = 0; i < walls; i++)
            {
                float a = i / (float)walls * Mathf.PI * 2f;
                var p = new Vector3(Mathf.Sin(a) * 8.6f, 2.4f, 2f + Mathf.Cos(a) * 8.6f);
                B.Primitive(PrimitiveType.Cube, $"WallPanel{i}", env, p,
                    new Vector3(-8, -a * Mathf.Rad2Deg, 0), new Vector3(2.6f, 5.6f, 0.3f), m["FabricEnv"]);
            }

            // suspended resonance ring — real torus mesh, standing vertical, facing the camera
            var ringGO = new GameObject("ResonanceRing", typeof(MeshFilter), typeof(MeshRenderer));
            ringGO.transform.SetParent(env, false);
            ringGO.transform.localPosition = new Vector3(0, 3.6f, 3.4f);
            ringGO.transform.localEulerAngles = new Vector3(90, 0, 0);
            ringGO.transform.localScale = new Vector3(1.7f, 1.7f, 1.7f);
            ringGO.GetComponent<MeshFilter>().sharedMesh =
                AssetDatabase.LoadAssetAtPath<Mesh>(EchoPolish.TorusPath) ?? EchoPolish.BuildTorus();
            ringGO.GetComponent<MeshRenderer>().sharedMaterial = m["BronzeEnv"];
            var ring = ringGO;
            var ringCol = ringGO.AddComponent<BoxCollider>();
            ringCol.size = new Vector3(2.2f, 2.2f, 0.5f); // tap target across the whole ring

            for (int i = 0; i < 6; i++)
            {
                float a = (i + 0.5f) / 6f * Mathf.PI * 2f;
                B.Primitive(PrimitiveType.Cylinder, $"Resonator{i}", env,
                    new Vector3(Mathf.Sin(a) * 6.4f, 0.9f, 2f + Mathf.Cos(a) * 6.4f), Vector3.zero,
                    new Vector3(0.35f, 0.9f, 0.35f), m["BronzeEnv"]);
            }

            var lights = new GameObject("Lighting").transform;
            B.AddLight("KeyLight", lights, LightType.Directional, B.Hex("FFE6C4"), 1.35f, new Vector3(0, 8, 0), new Vector3(50, 24, 0));
            var rimLight = B.AddLight("RimAmber", lights, LightType.Point, B.Amber, 7.5f, new Vector3(0, 2.6f, 2f), Vector3.zero);
            rimLight.range = 17f;
            var hallFill = B.AddLight("DomeFill", lights, LightType.Point, B.Hex("6A5CA8"), 2.2f, new Vector3(0, 7.5f, 2f), Vector3.zero);
            hallFill.range = 20f;

            // --- UI ---
            var (_, safe) = B.Canvas("HallCanvas", false);

            var hallGO = new GameObject("HallSystems", typeof(ResonanceHallController),
                typeof(ResonanceDialController), typeof(TuningStripController), typeof(SoundGuidePanel));
            var hall = hallGO.GetComponent<ResonanceHallController>();
            var dial = hallGO.GetComponent<ResonanceDialController>();
            var strip = hallGO.GetComponent<TuningStripController>();
            var guide = hallGO.GetComponent<SoundGuidePanel>();

            var primary = B.Button(safe, "PrimaryAction", "START SESSION", 34,
                new Vector2(0.5f, 0), new Vector2(0.5f, 0), new Vector2(0, 150), new Vector2(460, 108));
            B.Wire(primary, hall, nameof(ResonanceHallController.OnPrimaryAction));
            var primaryLabel = primary.GetComponentInChildren<TMP_Text>();

            var soundCheck = B.Button(safe, "SoundCheck", "SOUND CHECK", 24,
                new Vector2(0.5f, 0), new Vector2(0.5f, 0), new Vector2(-260, 90), new Vector2(230, 68));
            B.Wire(soundCheck, hall, nameof(ResonanceHallController.OnSoundCheck));
            var dialBtn = B.Button(safe, "OpenDial", "RESONANCE DIAL", 24,
                new Vector2(0.5f, 0), new Vector2(0.5f, 0), new Vector2(260, 90), new Vector2(230, 68));
            B.Wire(dialBtn, hall, nameof(ResonanceHallController.OnOpenDial));
            var setBtn = B.Button(safe, "OpenSettings", "TUNING", 22,
                new Vector2(1, 1), new Vector2(1, 1), new Vector2(-110, -70), new Vector2(160, 68));
            B.Wire(setBtn, hall, nameof(ResonanceHallController.OnOpenSettings));
            var guideBtn = B.Button(safe, "OpenGuide", "SOUND GUIDE", 22,
                new Vector2(0, 1), new Vector2(0, 1), new Vector2(150, -70), new Vector2(200, 68));
            B.Wire(guideBtn, hall, nameof(ResonanceHallController.OnOpenSoundGuide));

            var emblem = B.Text(safe, "ChamberInTuneEmblem", "CHAMBER IN TUNE", 28, TextAlignmentOptions.Center,
                B.Amber, new Vector2(0.5f, 1), new Vector2(0.5f, 1), new Vector2(0, -70), new Vector2(600, 60));
            emblem.gameObject.SetActive(false);

            // dial panel
            var dialPanel = B.Panel(safe, "DialPanel", new Vector2(0, 0), new Vector2(1, 1), Vector2.zero, Vector2.zero,
                new Color(0.03f, 0.035f, 0.05f, 0.96f));
            var ringRT = B.Rect(dialPanel.gameObject, "DialRing", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0, 0), new Vector2(720, 720));
            var nodes = new DialNode[12];
            for (int i = 0; i < 12; i++)
            {
                float a = i / 12f * Mathf.PI * 2f;
                var nrt = B.Rect(ringRT.gameObject, $"Node{i + 1}", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                    new Vector2(Mathf.Sin(a) * 300f, Mathf.Cos(a) * 300f), new Vector2(96, 96));
                var nimg = nrt.gameObject.AddComponent<Image>();
                nimg.color = new Color(B.Bronze.r, B.Bronze.g, B.Bronze.b, 0.18f);
                var node = nrt.gameObject.AddComponent<DialNode>();
                node.chamberId = i + 1;
                var num = B.Text(nrt.gameObject, "Num", $"{i + 1:00}", 26, TextAlignmentOptions.Center, B.Ivory);
                B.Stretch((RectTransform)num.transform);
                var lockT = B.Text(nrt.gameObject, "Lock", "LOCKED", 12, TextAlignmentOptions.Center, B.Crimson);
                B.Stretch((RectTransform)lockT.transform);
                var focusGlow = B.Panel(nrt.gameObject, "Focus", new Vector2(0, 0), new Vector2(1, 1), Vector2.zero,
                    new Vector2(18, 18), new Color(B.Amber.r, B.Amber.g, B.Amber.b, 0.28f));
                var mark = B.Text(nrt.gameObject, "Mark", "~", 18, TextAlignmentOptions.BottomRight, B.Amber);
                B.Stretch((RectTransform)mark.transform);
                SetRef(node, "numberLabel", num);
                SetRef(node, "lockIcon", lockT.gameObject);
                SetRef(node, "focusGlow", focusGlow.gameObject);
                SetRef(node, "completeMark", mark.gameObject);
                nodes[i] = node;
            }
            var fNum = B.Text(dialPanel.gameObject, "FocusNum", "01", 64, TextAlignmentOptions.Center, B.Ivory,
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0, 40), new Vector2(400, 90));
            var fTitle = B.Text(dialPanel.gameObject, "FocusTitle", "", 30, TextAlignmentOptions.Center, B.Bronze,
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0, -20), new Vector2(600, 50));
            var fStatus = B.Text(dialPanel.gameObject, "FocusStatus", "OPEN", 22, TextAlignmentOptions.Center, B.Ivory,
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0, -66), new Vector2(400, 40));
            var prevB = B.Button(dialPanel.gameObject, "Prev", "<", 40, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(-430, 0), new Vector2(90, 90));
            B.Wire(prevB, dial, nameof(ResonanceDialController.Prev));
            var nextB = B.Button(dialPanel.gameObject, "NextC", ">", 40, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(430, 0), new Vector2(90, 90));
            B.Wire(nextB, dial, nameof(ResonanceDialController.Next));
            var launchB = B.Button(dialPanel.gameObject, "Launch", "ENTER CHAMBER", 26,
                new Vector2(0.5f, 0), new Vector2(0.5f, 0), new Vector2(0, 150), new Vector2(420, 96));
            B.Wire(launchB, dial, nameof(ResonanceDialController.Launch));
            var dialClose = B.Button(dialPanel.gameObject, "DialClose", "BACK", 22,
                new Vector2(0, 1), new Vector2(0, 1), new Vector2(120, -70), new Vector2(160, 64));
            B.Wire(dialClose, dial, nameof(ResonanceDialController.Close));
            dialPanel.gameObject.SetActive(false);

            SetRef(dial, "panel", dialPanel.gameObject);
            SetRef(dial, "ring", ringRT);
            SetRef(dial, "focusNumber", fNum);
            SetRef(dial, "focusTitle", fTitle);
            SetRef(dial, "focusStatus", fStatus);
            SetRef(dial, "launchButton", launchB);
            {
                var so = new SerializedObject(dial);
                var arr = so.FindProperty("nodes");
                arr.arraySize = 12;
                for (int i = 0; i < 12; i++) arr.GetArrayElementAtIndex(i).objectReferenceValue = nodes[i];
                so.ApplyModifiedPropertiesWithoutUndo();
            }

            // tuning strip
            var stripPanel = B.Panel(safe, "TuningStrip", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                Vector2.zero, new Vector2(760, 520), new Color(0.04f, 0.045f, 0.06f, 0.98f));
            B.Text(stripPanel.gameObject, "SettingsTitle", "TUNING STRIP", 30, TextAlignmentOptions.Center, B.Ivory,
                new Vector2(0.5f, 1), new Vector2(0.5f, 1), new Vector2(0, -44), new Vector2(500, 44));
            var musicSlider = MakeSlider(stripPanel.gameObject, "MusicFader", "MUSIC / AMBIENCE", 120);
            var fxSlider = MakeSlider(stripPanel.gameObject, "EffectsFader", "EFFECTS", 20);
            var hapticsToggle = MakeToggle(stripPanel.gameObject, "HapticsToggle", "HAPTICS", -70);
            var resetBtn = B.Button(stripPanel.gameObject, "ResetProgress", "RESET RESONANCE HISTORY", 20,
                new Vector2(0.5f, 0), new Vector2(0.5f, 0), new Vector2(0, 120), new Vector2(460, 60),
                new Color(B.Crimson.r, B.Crimson.g, B.Crimson.b, 0.25f));
            B.Wire(resetBtn, strip, nameof(TuningStripController.OnResetRequested));
            var stripClose = B.Button(stripPanel.gameObject, "StripClose", "DONE", 22,
                new Vector2(0.5f, 0), new Vector2(0.5f, 0), new Vector2(0, 48), new Vector2(200, 56));
            B.Wire(stripClose, strip, nameof(TuningStripController.Close));
            var creditsBtn = B.Button(stripPanel.gameObject, "Credits", "CREDITS", 16,
                new Vector2(1, 1), new Vector2(1, 1), new Vector2(-70, -30), new Vector2(120, 44));
            B.Wire(creditsBtn, strip, nameof(TuningStripController.OnCreditsToggle));
            var creditsPanel = B.Panel(stripPanel.gameObject, "CreditsPanel", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                Vector2.zero, new Vector2(520, 280));
            B.Text(creditsPanel.gameObject, "CreditsText",
                "ECHO FORGE\nDesign, code, audio, art: project-generated original\nBuilt offline. No trackers.", 22,
                TextAlignmentOptions.Center, B.Ivory);
            creditsPanel.gameObject.SetActive(false);

            var resetConfirm = B.Panel(stripPanel.gameObject, "ResetConfirm", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                Vector2.zero, new Vector2(560, 260), new Color(0.05f, 0.03f, 0.035f, 0.99f));
            B.Text(resetConfirm.gameObject, "ConfirmText",
                "RESET RESONANCE HISTORY?\nAll completed chambers and ratings will be cleared.", 22,
                TextAlignmentOptions.Center, B.Ivory, new Vector2(0, 0.4f), new Vector2(1, 1), Vector2.zero, new Vector2(-40, -20));
            var doReset = B.Button(resetConfirm.gameObject, "DoReset", "RESET", 22, new Vector2(0.5f, 0), new Vector2(0.5f, 0),
                new Vector2(-120, 40), new Vector2(180, 60), new Color(B.Crimson.r, B.Crimson.g, B.Crimson.b, 0.4f));
            B.Wire(doReset, strip, nameof(TuningStripController.OnResetConfirmed));
            var cancelReset = B.Button(resetConfirm.gameObject, "CancelReset", "CANCEL", 22, new Vector2(0.5f, 0), new Vector2(0.5f, 0),
                new Vector2(120, 40), new Vector2(180, 60));
            B.Wire(cancelReset, strip, nameof(TuningStripController.OnResetCancelled));
            resetConfirm.gameObject.SetActive(false);
            stripPanel.gameObject.SetActive(false);

            B.WireFloat(musicSlider, strip, nameof(TuningStripController.OnMusicChanged));
            B.WireFloat(fxSlider, strip, nameof(TuningStripController.OnEffectsChanged));
            B.WireBool(hapticsToggle, strip, nameof(TuningStripController.OnHapticsChanged));
            SetRef(strip, "panel", stripPanel.gameObject);
            SetRef(strip, "musicFader", musicSlider);
            SetRef(strip, "effectsFader", fxSlider);
            SetRef(strip, "hapticsToggle", hapticsToggle);
            SetRef(strip, "resetConfirm", resetConfirm.gameObject);
            SetRef(strip, "creditsPanel", creditsPanel.gameObject);

            // sound guide
            var guidePanel = B.Panel(safe, "SoundGuidePanel", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                Vector2.zero, new Vector2(900, 640), new Color(0.04f, 0.045f, 0.06f, 0.98f));
            B.Text(guidePanel.gameObject, "GuideTitle", "SOUND GUIDE", 30, TextAlignmentOptions.Center, B.Ivory,
                new Vector2(0.5f, 1), new Vector2(0.5f, 1), new Vector2(0, -44), new Vector2(500, 44));
            B.Text(guidePanel.gameObject, "GuideBody",
                "EMITTER  releases one visible pulse.\n" +
                "REFLECTOR  turns the pulse in 45 steps.\n" +
                "ABSORBER  stops a pulse.\n" +
                "SPLITTER  forks one pulse into two.\n" +
                "DELAY PLATE  holds a pulse a set number of beats.\n" +
                "RECEIVER  needs a pulse. Forbidden receivers must stay silent.\n" +
                "RESONATOR  needs two pulses in the right relationship.", 24,
                TextAlignmentOptions.TopLeft, B.Ivory,
                new Vector2(0, 0), new Vector2(1, 1), Vector2.zero, new Vector2(-80, -120));
            var guideClose = B.Button(guidePanel.gameObject, "GuideClose", "BACK", 22,
                new Vector2(0.5f, 0), new Vector2(0.5f, 0), new Vector2(0, 40), new Vector2(200, 56));
            B.Wire(guideClose, guide, nameof(SoundGuidePanel.Close));
            guidePanel.gameObject.SetActive(false);
            SetRef(guide, "panel", guidePanel.gameObject);

            // hall refs
            SetRef(hall, "dial", dial);
            SetRef(hall, "tuningStrip", strip);
            SetRef(hall, "soundGuide", guide);
            SetRef(hall, "primaryActionLabel", primaryLabel);
            SetRef(hall, "chamberInTuneEmblem", emblem.gameObject);
            SetRef(hall, "resonanceRing", ring.transform);
            SetRef(hall, "rimLight", rimLight);

            // ring tap -> hall.OnRingTapped via a physical raycast helper on the ring
            var tap = ring.AddComponent<RingTapRelay>();
            SetRef(tap, "hall", hall);

            Save(s, "ResonanceHall");
            Debug.Log("Echo Forge: Resonance Hall scene built.");
        }

        static Slider MakeSlider(GameObject parent, string name, string label, float y)
        {
            B.Text(parent, name + "Label", label, 20, TextAlignmentOptions.Left, B.Ivory,
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(-250, y + 34), new Vector2(500, 30));
            var rt = B.Rect(parent, name, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0, y), new Vector2(520, 26));
            var s = rt.gameObject.AddComponent<Slider>();
            var bg = new GameObject("BG", typeof(Image)); bg.transform.SetParent(rt, false);
            var bgrt = (RectTransform)bg.transform; B.Stretch(bgrt);
            bg.GetComponent<Image>().color = new Color(1, 1, 1, 0.08f);
            var fillArea = new GameObject("Fill Area", typeof(RectTransform)); fillArea.transform.SetParent(rt, false);
            var fart = (RectTransform)fillArea.transform; B.Stretch(fart);
            var fill = new GameObject("Fill", typeof(Image)); fill.transform.SetParent(fillArea.transform, false);
            var frt = (RectTransform)fill.transform; B.Stretch(frt);
            fill.GetComponent<Image>().color = new Color(B.Bronze.r, B.Bronze.g, B.Bronze.b, 0.8f);
            var handle = new GameObject("Handle", typeof(Image)); handle.transform.SetParent(rt, false);
            var hrt = (RectTransform)handle.transform; hrt.sizeDelta = new Vector2(22, 34);
            handle.GetComponent<Image>().color = B.Ivory;
            s.fillRect = frt; s.handleRect = hrt; s.targetGraphic = handle.GetComponent<Image>();
            s.minValue = 0; s.maxValue = 1; s.value = 0.8f;
            return s;
        }

        static Toggle MakeToggle(GameObject parent, string name, string label, float y)
        {
            B.Text(parent, name + "Label", label, 20, TextAlignmentOptions.Left, B.Ivory,
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(-160, y), new Vector2(300, 30));
            var rt = B.Rect(parent, name, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(150, y), new Vector2(64, 34));
            var t = rt.gameObject.AddComponent<Toggle>();
            var bg = new GameObject("BG", typeof(Image)); bg.transform.SetParent(rt, false);
            var bgrt = (RectTransform)bg.transform; B.Stretch(bgrt);
            bg.GetComponent<Image>().color = new Color(1, 1, 1, 0.1f);
            var check = new GameObject("Check", typeof(Image)); check.transform.SetParent(rt, false);
            var crt = (RectTransform)check.transform; B.Stretch(crt);
            check.GetComponent<Image>().color = new Color(B.Amber.r, B.Amber.g, B.Amber.b, 0.9f);
            t.targetGraphic = bg.GetComponent<Image>();
            t.graphic = check.GetComponent<Image>();
            t.isOn = true;
            return t;
        }

        // ---------------------------------------------------------------- chamber

        [MenuItem("Echo Forge/Build/4 - Echo Chamber Scene")]
        public static void BuildChamber()
        {
            var s = NewScene();
            var m = Mats();
            var cam = MakeCamera(new Vector3(0, 10.5f, -9.6f), new Vector3(45, 0, 0), 43f, B.MidnightNavy);
            B.AddPostVolume(cam);
            AudioRig();
            B.EventSystem();

            var env = new GameObject("ChamberEnvironment").transform;
            B.Primitive(PrimitiveType.Cube, "AcousticFloor", env, new Vector3(0, -0.25f, 0), Vector3.zero,
                new Vector3(18, 0.5f, 14), m["FloorEnv"]);
            B.Primitive(PrimitiveType.Cube, "BackWall", env, new Vector3(0, 2.4f, 6.6f), Vector3.zero,
                new Vector3(18, 5.4f, 0.5f), m["WallEnv"]);
            B.Primitive(PrimitiveType.Cube, "LeftWall", env, new Vector3(-8.6f, 1.6f, 0), Vector3.zero,
                new Vector3(0.5f, 3.6f, 13.5f), m["FabricEnv"]);
            B.Primitive(PrimitiveType.Cube, "RightWall", env, new Vector3(8.6f, 1.6f, 0), Vector3.zero,
                new Vector3(0.5f, 3.6f, 13.5f), m["FabricEnv"]);
            var frame = new GameObject("ResonanceFrame").transform; frame.SetParent(env, false);
            B.Primitive(PrimitiveType.Cube, "FrameFront", frame, new Vector3(0, 0.15f, -5.6f), Vector3.zero, new Vector3(15, 0.3f, 0.4f), m["BronzeEnv"]);
            B.Primitive(PrimitiveType.Cube, "FrameBack", frame, new Vector3(0, 0.15f, 5.6f), Vector3.zero, new Vector3(15, 0.3f, 0.4f), m["BronzeEnv"]);
            B.Primitive(PrimitiveType.Cube, "FrameLeft", frame, new Vector3(-7.4f, 0.15f, 0), Vector3.zero, new Vector3(0.4f, 0.3f, 11.6f), m["BronzeEnv"]);
            B.Primitive(PrimitiveType.Cube, "FrameRight", frame, new Vector3(7.4f, 0.15f, 0), Vector3.zero, new Vector3(0.4f, 0.3f, 11.6f), m["BronzeEnv"]);
            // ambient resonator posts with a warm emissive cap (well outside the play area)
            for (int i = -1; i <= 1; i += 2)
            {
                B.Primitive(PrimitiveType.Cylinder, $"AmbientResonator{i}", env, new Vector3(i * 7.6f, 1.1f, 5.6f), Vector3.zero,
                    new Vector3(0.42f, 1.1f, 0.42f), m["BronzeEnv"]);
                B.Primitive(PrimitiveType.Sphere, $"ResonatorCap{i}", env, new Vector3(i * 7.6f, 2.3f, 5.6f), Vector3.zero,
                    new Vector3(0.42f, 0.42f, 0.42f), m["Resonator"]);
            }

            var lights = new GameObject("Lighting").transform;
            B.AddLight("KeyLight", lights, LightType.Directional, B.Hex("FFEAD0"), 1.55f, new Vector3(0, 9, -2), new Vector3(52, 18, 0));
            var fillPoint = B.AddLight("BoardGlow", lights, LightType.Point, B.Hex("FFD9A6"), 3f, new Vector3(-2f, 4.5f, -1.5f), Vector3.zero);
            fillPoint.range = 13f;
            var backGlow = B.AddLight("BackGlow", lights, LightType.Point, B.PaleViolet, 2.4f, new Vector3(1.5f, 3f, 6f), Vector3.zero);
            backGlow.range = 13f;

            var board = new GameObject("PuzzleBoard").transform;
            board.position = new Vector3(0, 0.02f, 0);

            var vfxGO = new GameObject("PulseVFX", typeof(PulseVisualizer));
            var visualizer = vfxGO.GetComponent<PulseVisualizer>();

            var sysGO = new GameObject("Systems", typeof(ChamberInput), typeof(LevelController));
            var input = sysGO.GetComponent<ChamberInput>();
            var level = sysGO.GetComponent<LevelController>();

            // ---- UI ----
            var (_, safe) = B.Canvas("ChamberCanvas", false);
            var uiGO = new GameObject("ChamberUI", typeof(ChamberUI));
            uiGO.transform.SetParent(safe.transform, false);
            var ui = uiGO.GetComponent<ChamberUI>();

            // context (lower-left)
            var ctx = B.Panel(safe, "ContextInfo", new Vector2(0, 0), new Vector2(0, 0), new Vector2(250, 120), new Vector2(460, 150));
            var ctxGroup = ctx.gameObject.AddComponent<CanvasGroup>();
            var ctxTitle = B.Text(ctx.gameObject, "Title", "REFLECTOR", 30, TextAlignmentOptions.TopLeft, B.Bronze,
                new Vector2(0, 0), new Vector2(1, 1), Vector2.zero, new Vector2(-40, -20));
            var ctxDetail = B.Text(ctx.gameObject, "Detail", "ANGLE 45", 22, TextAlignmentOptions.BottomLeft, B.Ivory,
                new Vector2(0, 0), new Vector2(1, 1), Vector2.zero, new Vector2(-40, -20));

            // counter (top-left, engraved)
            var tunes = B.Text(safe, "AdjustmentCounter", "TUNES 00", 26, TextAlignmentOptions.TopLeft, B.Bronze,
                new Vector2(0, 1), new Vector2(0, 1), new Vector2(180, -66), new Vector2(300, 44));

            // PULSE actuator (lower-center) - a physical circular strike pad, not a rectangle
            var pulseBtn = B.Button(safe, "PulseActuator", "PULSE", 30, new Vector2(0.5f, 0), new Vector2(0.5f, 0),
                new Vector2(0, 150), new Vector2(188, 188), new Color(0.42f, 0.28f, 0.12f, 0.92f));
            var pulseImg = pulseBtn.GetComponent<Image>();
            pulseImg.sprite = B.Disc(false);
            pulseImg.type = Image.Type.Simple;
            var pulseRing = B.Rect(pulseBtn.gameObject, "Ring", new Vector2(0, 0), new Vector2(1, 1), Vector2.zero, new Vector2(26, 26));
            var pulseRingImg = pulseRing.gameObject.AddComponent<Image>();
            pulseRingImg.sprite = B.Disc(true);
            pulseRingImg.color = new Color(B.Amber.r, B.Amber.g, B.Amber.b, 0.9f);
            pulseRingImg.raycastTarget = false;
            B.Wire(pulseBtn, level, nameof(LevelController.OnPulsePressed));

            // pause button (small circular)
            var pauseBtn = B.Button(safe, "PauseButton", "II", 26, new Vector2(1, 1), new Vector2(1, 1),
                new Vector2(-84, -64), new Vector2(84, 84), new Color(0.09f, 0.10f, 0.13f, 0.85f));
            var pauseImg = pauseBtn.GetComponent<Image>();
            pauseImg.sprite = B.Disc(false);
            pauseImg.type = Image.Type.Simple;
            B.Wire(pauseBtn, level, nameof(LevelController.OnPausePressed));

            // tutorial layer - a restrained lower-center caption, not a full-width bar
            var tut = B.Panel(safe, "TutorialLayer", new Vector2(0.5f, 0), new Vector2(0.5f, 0), new Vector2(0, 330),
                new Vector2(600, 64), new Color(0.05f, 0.055f, 0.075f, 0.55f));
            var tutAccent = B.Panel(tut.gameObject, "Accent", new Vector2(0, 0), new Vector2(0, 1), new Vector2(6, 0),
                new Vector2(4, -16), new Color(B.Bronze.r, B.Bronze.g, B.Bronze.b, 0.9f));
            tutAccent.rectTransform.pivot = new Vector2(0, 0.5f);
            var tutGroup = tut.gameObject.AddComponent<CanvasGroup>();
            var tutText = B.Text(tut.gameObject, "Text", "", 24, TextAlignmentOptions.Center, B.Ivory);
            B.Stretch((RectTransform)tutText.transform);

            // result layer
            var result = B.Panel(safe, "ResultLayer", new Vector2(0, 0), new Vector2(1, 1), Vector2.zero, Vector2.zero,
                new Color(0.02f, 0.025f, 0.04f, 0.88f));
            var rTitle = B.Text(result.gameObject, "ResultTitle", "RESONANCE ACHIEVED", 52, TextAlignmentOptions.Center, B.Ivory,
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0, 120), new Vector2(1000, 90));
            var rReason = B.Text(result.gameObject, "ResultReason", "", 26, TextAlignmentOptions.Center, B.Crimson,
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0, 50), new Vector2(800, 44));
            var rRating = B.Text(result.gameObject, "ResultRating", "", 30, TextAlignmentOptions.Center, B.Amber,
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0, 4), new Vector2(800, 44));
            var nextBtn = B.Button(result.gameObject, "NextChamber", "NEXT CHAMBER", 24, new Vector2(0.5f, 0), new Vector2(0.5f, 0),
                new Vector2(0, 210), new Vector2(360, 84));
            B.Wire(nextBtn, level, nameof(LevelController.OnNextPressed));
            var retuneBtn = B.Button(result.gameObject, "Retune", "RETUNE", 24, new Vector2(0.5f, 0), new Vector2(0.5f, 0),
                new Vector2(-230, 110), new Vector2(300, 76));
            B.Wire(retuneBtn, level, nameof(LevelController.OnRetunePressed));
            var toDial = B.Button(result.gameObject, "ToDial", "RESONANCE DIAL", 24, new Vector2(0.5f, 0), new Vector2(0.5f, 0),
                new Vector2(230, 110), new Vector2(300, 76));
            B.Wire(toDial, level, nameof(LevelController.OnDialPressed));
            result.gameObject.SetActive(false);

            // pause layer
            var pause = B.Panel(safe, "PauseLayer", new Vector2(0, 0), new Vector2(1, 1), Vector2.zero, Vector2.zero,
                new Color(0.02f, 0.025f, 0.035f, 0.9f));
            B.Text(pause.gameObject, "PauseTitle", "SOUND DAMPING", 44, TextAlignmentOptions.Center, B.Ivory,
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0, 150), new Vector2(800, 80));
            var resumeBtn = B.Button(pause.gameObject, "Resume", "RESUME", 26, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0, 40), new Vector2(360, 76));
            B.Wire(resumeBtn, level, nameof(LevelController.OnResumePressed));
            var restartBtn = B.Button(pause.gameObject, "Restart", "RESTART CHAMBER", 26, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0, -50), new Vector2(360, 76));
            B.Wire(restartBtn, level, nameof(LevelController.OnRestartPressed));
            var pguideBtn = B.Button(pause.gameObject, "PauseGuide", "SOUND GUIDE", 26, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0, -140), new Vector2(360, 76));
            B.Wire(pguideBtn, level, nameof(LevelController.OnSoundGuideOpen));
            var exitBtn = B.Button(pause.gameObject, "ExitSession", "EXIT SESSION", 26, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0, -230), new Vector2(360, 76));
            B.Wire(exitBtn, level, nameof(LevelController.OnExitSessionPressed));
            pause.gameObject.SetActive(false);

            // dialog + sound guide layers
            var dialog = B.Panel(safe, "DialogLayer", new Vector2(0, 0), new Vector2(1, 1), Vector2.zero, Vector2.zero,
                new Color(0.02f, 0.025f, 0.035f, 0.9f));
            var dialogText = B.Text(dialog.gameObject, "DialogText", "", 28, TextAlignmentOptions.Center, B.Ivory,
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0, 60), new Vector2(800, 120));
            dialog.gameObject.SetActive(false);

            var guideL = B.Panel(safe, "SoundGuideLayer", new Vector2(0, 0), new Vector2(1, 1), Vector2.zero, Vector2.zero,
                new Color(0.03f, 0.035f, 0.05f, 0.96f));
            B.Text(guideL.gameObject, "GuideBody",
                "EMITTER releases a pulse. REFLECTOR turns it 45 deg. ABSORBER stops it.\n" +
                "SPLITTER forks it. DELAY PLATE holds it. RECEIVER needs a pulse.\n" +
                "RESONATOR needs two. Forbidden receivers must stay silent.", 26,
                TextAlignmentOptions.Center, B.Ivory,
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0, 40), new Vector2(1100, 300));
            var guideLClose = B.Button(guideL.gameObject, "GuideLClose", "BACK", 24, new Vector2(0.5f, 0), new Vector2(0.5f, 0),
                new Vector2(0, 120), new Vector2(220, 66));
            B.Wire(guideLClose, level, nameof(LevelController.OnSoundGuideClose));
            guideL.gameObject.SetActive(false);

            // ---- wire ChamberUI ----
            SetRef(ui, "contextGroup", ctxGroup);
            SetRef(ui, "contextTitle", ctxTitle);
            SetRef(ui, "contextDetail", ctxDetail);
            SetRef(ui, "tunesLabel", tunes);
            SetRef(ui, "tutorialGroup", tutGroup);
            SetRef(ui, "tutorialLabel", tutText);
            SetRef(ui, "resultLayer", result.gameObject);
            SetRef(ui, "resultTitle", rTitle);
            SetRef(ui, "resultReason", rReason);
            SetRef(ui, "resultRating", rRating);
            SetRef(ui, "nextButton", nextBtn.gameObject);
            SetRef(ui, "pauseLayer", pause.gameObject);
            SetRef(ui, "dialogLayer", dialog.gameObject);
            SetRef(ui, "dialogLabel", dialogText);
            SetRef(ui, "soundGuideLayer", guideL.gameObject);

            // ---- wire LevelController ----
            SetRef(level, "cam", cam);
            SetRef(level, "boardRoot", board);
            SetRef(level, "input", input);
            SetRef(level, "ui", ui);
            SetRef(level, "visualizer", visualizer);
            SetRef(level, "catalog", AssetDatabase.LoadAssetAtPath<LevelCatalog>($"{B.ResDir}/LevelCatalog.asset"));
            SetRef(level, "matEmitter", m["Emitter"]);
            SetRef(level, "matReflector", m["Reflector"]);
            SetRef(level, "matAbsorber", m["Absorber"]);
            SetRef(level, "matSplitter", m["Splitter"]);
            SetRef(level, "matDelay", m["Delay"]);
            SetRef(level, "matReceiver", m["Receiver"]);
            SetRef(level, "matResonator", m["Resonator"]);
            SetRef(level, "matForbidden", m["Forbidden"]);
            SetRef(level, "matWall", m["Wall"]);
            SetRef(level, "matSocket", m["Socket"]);
            SetFloat(level, "cellSize", 1.15f);
            SetRef(input, "cam", cam);
            SetRef(visualizer, "boardRoot", board);

            Save(s, "EchoChamber");
            Debug.Log("Echo Forge: Echo Chamber scene built.");
        }
    }
}
