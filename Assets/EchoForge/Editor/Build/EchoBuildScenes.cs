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
            EchoPolish.ImproveTouchTargets();
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
            var bTitle = B.Text(safe, "Title", "ECHO FORGE", 88, TextAlignmentOptions.Center, B.Ivory,
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0, 44), new Vector2(1200, 150));
            bTitle.characterSpacing = 16f;
            B.Rule(safe, "TitleRule", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0, -18), new Vector2(150, 3));
            B.Text(safe, "Sub", "TUNE THE ROOM, NOT THE CHARACTER", 22, TextAlignmentOptions.Center,
                new Color(B.Bronze.r, B.Bronze.g, B.Bronze.b, 0.9f),
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0, -58), new Vector2(1200, 50))
                .characterSpacing = 6f;

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

            // suspended resonance ring - real torus mesh, standing vertical, facing the camera
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

            // title block (top-centre)
            var title = B.Text(safe, "HallTitle", "ECHO FORGE", 58, TextAlignmentOptions.Center, B.Ivory,
                new Vector2(0.5f, 1), new Vector2(0.5f, 1), new Vector2(0, -130), new Vector2(1000, 84));
            title.characterSpacing = 12f;
            B.Rule(safe, "TitleRule", new Vector2(0.5f, 1), new Vector2(0.5f, 1), new Vector2(0, -188), new Vector2(140, 3));

            var emblem = B.Text(safe, "ChamberInTuneEmblem", "CHAMBER  IN  TUNE", 20, TextAlignmentOptions.Center,
                B.Amber, new Vector2(0.5f, 1), new Vector2(0.5f, 1), new Vector2(0, -214), new Vector2(600, 36));
            emblem.characterSpacing = 7f;
            emblem.gameObject.SetActive(false);

            // centred menu card - one column, everything flush
            var menu = B.Panel(safe, "MainMenu", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0, -70), new Vector2(524, 462));
            B.Rule(menu.gameObject, "MenuTopRule", new Vector2(0, 1), new Vector2(1, 1),
                new Vector2(0, -1.5f), new Vector2(0, 3), new Color(B.Bronze.r, B.Bronze.g, B.Bronze.b, 0.55f));
            var stack = B.ButtonStack(menu.gameObject, 28f, 432f, B.BtnH, new[]
            {
                ("PrimaryAction", "START SESSION", 30f),
                ("OpenDial", "RESONANCE DIAL", 26f),
                ("SoundCheck", "SOUND CHECK", 26f),
            }, primaryFirst: true);
            B.Wire(stack[0], hall, nameof(ResonanceHallController.OnPrimaryAction));
            B.Wire(stack[1], hall, nameof(ResonanceHallController.OnOpenDial));
            B.Wire(stack[2], hall, nameof(ResonanceHallController.OnSoundCheck));
            var primaryLabel = stack[0].GetComponentInChildren<TMP_Text>();

            B.Rule(menu.gameObject, "MenuDivider", new Vector2(0.5f, 0), new Vector2(0.5f, 0),
                new Vector2(0, 118), new Vector2(432, 2), new Color(1, 1, 1, 0.08f));
            var guideBtn = B.Button(menu.gameObject, "OpenGuide", "SOUND GUIDE", 20,
                new Vector2(0.5f, 0), new Vector2(0.5f, 0), new Vector2(-110, 46), new Vector2(208, 56));
            B.Wire(guideBtn, hall, nameof(ResonanceHallController.OnOpenSoundGuide));
            var setBtn = B.Button(menu.gameObject, "OpenSettings", "TUNING", 20,
                new Vector2(0.5f, 0), new Vector2(0.5f, 0), new Vector2(110, 46), new Vector2(208, 56));
            B.Wire(setBtn, hall, nameof(ResonanceHallController.OnOpenSettings));

            // dial panel
            var dialPanel = B.Panel(safe, "DialPanel", new Vector2(0, 0), new Vector2(1, 1), Vector2.zero, Vector2.zero,
                B.ScrimFill);
            B.Header(dialPanel.gameObject, "SELECT  CHAMBER", 28);
            var ringRT = B.Rect(dialPanel.gameObject, "DialRing", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0, 0), new Vector2(720, 720));
            var nodes = new DialNode[12];
            for (int i = 0; i < 12; i++)
            {
                float a = i / 12f * Mathf.PI * 2f;
                var nrt = B.Rect(ringRT.gameObject, $"Node{i + 1}", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                    new Vector2(Mathf.Sin(a) * 300f, Mathf.Cos(a) * 300f), new Vector2(96, 96));
                var nimg = nrt.gameObject.AddComponent<Image>();
                nimg.sprite = B.Disc(true);
                nimg.color = new Color(B.Bronze.r, B.Bronze.g, B.Bronze.b, 0.18f);
                var node = nrt.gameObject.AddComponent<DialNode>();
                node.chamberId = i + 1;
                var num = B.Text(nrt.gameObject, "Num", $"{i + 1:00}", 26, TextAlignmentOptions.Center, B.Ivory);
                B.Stretch((RectTransform)num.transform);
                var lockT = B.Text(nrt.gameObject, "Lock", "LOCKED", 12, TextAlignmentOptions.Center, B.Crimson);
                B.Stretch((RectTransform)lockT.transform);
                lockT.alignment = TextAlignmentOptions.Bottom;
                lockT.margin = new Vector4(0, 0, 0, 8);
                var focusGlow = B.Panel(nrt.gameObject, "Focus", new Vector2(0, 0), new Vector2(1, 1), Vector2.zero,
                    new Vector2(18, 18), new Color(B.Amber.r, B.Amber.g, B.Amber.b, 0.28f));
                focusGlow.GetComponent<Image>().sprite = B.Disc(true);
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
                new Vector2(0.5f, 0), new Vector2(0.5f, 0), new Vector2(0, 60 + B.BtnHBig * 0.5f), new Vector2(420, B.BtnHBig),
                primary: true);
            B.Wire(launchB, dial, nameof(ResonanceDialController.Launch));
            var dialClose = B.Button(dialPanel.gameObject, "DialClose", "BACK", 20,
                new Vector2(0, 1), new Vector2(0, 1), new Vector2(B.UIEdge + 90, -B.UIEdge - 28), new Vector2(180, 56));
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

            // tuning strip - full-screen scrim + centred card (header · two faders · toggle · divider · reset · done)
            var stripRoot = B.Panel(safe, "TuningStrip", new Vector2(0, 0), new Vector2(1, 1), Vector2.zero, Vector2.zero, B.ScrimFill);
            var stripPanel = B.Panel(stripRoot.gameObject, "Card", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                Vector2.zero, new Vector2(720, 636));
            B.Header(stripPanel.gameObject, "TUNING  STRIP", 28);
            var musicSlider = MakeSlider(stripPanel.gameObject, "MusicFader", "MUSIC / AMBIENCE", 150);
            var fxSlider = MakeSlider(stripPanel.gameObject, "EffectsFader", "EFFECTS", 58);
            var hapticsToggle = MakeToggle(stripPanel.gameObject, "HapticsToggle", "HAPTICS", -24);
            B.Rule(stripPanel.gameObject, "StripDivider", new Vector2(0.5f, 0), new Vector2(0.5f, 0),
                new Vector2(0, 214), new Vector2(560, 2), new Color(1, 1, 1, 0.08f));
            var resetBtn = B.Button(stripPanel.gameObject, "ResetProgress", "RESET RESONANCE HISTORY", 18,
                new Vector2(0.5f, 0), new Vector2(0.5f, 0), new Vector2(0, 172), new Vector2(420, 56),
                new Color(B.Crimson.r * 0.5f, B.Crimson.g * 0.5f, B.Crimson.b * 0.5f, 0.92f));
            B.Wire(resetBtn, strip, nameof(TuningStripController.OnResetRequested));
            var stripClose = B.Button(stripPanel.gameObject, "StripClose", "DONE", 24,
                new Vector2(0.5f, 0), new Vector2(0.5f, 0), new Vector2(0, 80), new Vector2(300, B.BtnHBig),
                primary: true);
            B.Wire(stripClose, strip, nameof(TuningStripController.Close));
            var creditsBtn = B.Button(stripPanel.gameObject, "Credits", "CREDITS", 16,
                new Vector2(1, 1), new Vector2(1, 1), new Vector2(-84, -44), new Vector2(140, 44));
            B.Wire(creditsBtn, strip, nameof(TuningStripController.OnCreditsToggle));
            var creditsPanel = B.Panel(stripPanel.gameObject, "CreditsPanel", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                Vector2.zero, new Vector2(560, 320));
            B.Header(creditsPanel.gameObject, "CREDITS", 26);
            B.Text(creditsPanel.gameObject, "CreditsText",
                "ECHO FORGE\n\nDesign · code · audio · art\nproject-generated original\n\nBuilt offline.  No trackers.", 22,
                TextAlignmentOptions.Center, B.Ivory,
                new Vector2(0, 0), new Vector2(1, 1), new Vector2(0, -10), new Vector2(-64, -140));
            creditsPanel.gameObject.SetActive(false);

            var resetConfirm = B.Panel(stripPanel.gameObject, "ResetConfirm", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                Vector2.zero, new Vector2(580, 300), new Color(0.055f, 0.035f, 0.04f, 0.995f));
            B.Header(resetConfirm.gameObject, "RESET  HISTORY?", 26);
            B.Text(resetConfirm.gameObject, "ConfirmText",
                "All completed chambers and ratings will be cleared. Audio and haptics settings are kept.", 20,
                TextAlignmentOptions.Center, B.Ivory,
                new Vector2(0, 0), new Vector2(1, 1), new Vector2(0, 22), new Vector2(-72, -150));
            var doReset = B.Button(resetConfirm.gameObject, "DoReset", "RESET", 22, new Vector2(0.5f, 0), new Vector2(0.5f, 0),
                new Vector2(-108, 36 + 34), new Vector2(196, 68), new Color(B.Crimson.r, B.Crimson.g, B.Crimson.b, 0.9f));
            B.Wire(doReset, strip, nameof(TuningStripController.OnResetConfirmed));
            var cancelReset = B.Button(resetConfirm.gameObject, "CancelReset", "CANCEL", 22, new Vector2(0.5f, 0), new Vector2(0.5f, 0),
                new Vector2(108, 36 + 34), new Vector2(196, 68));
            B.Wire(cancelReset, strip, nameof(TuningStripController.OnResetCancelled));
            resetConfirm.gameObject.SetActive(false);
            stripRoot.gameObject.SetActive(false);

            B.WireFloat(musicSlider, strip, nameof(TuningStripController.OnMusicChanged));
            B.WireFloat(fxSlider, strip, nameof(TuningStripController.OnEffectsChanged));
            B.WireBool(hapticsToggle, strip, nameof(TuningStripController.OnHapticsChanged));
            SetRef(strip, "panel", stripRoot.gameObject);
            SetRef(strip, "musicFader", musicSlider);
            SetRef(strip, "effectsFader", fxSlider);
            SetRef(strip, "hapticsToggle", hapticsToggle);
            SetRef(strip, "resetConfirm", resetConfirm.gameObject);
            SetRef(strip, "creditsPanel", creditsPanel.gameObject);

            // sound guide - full-screen scrim + centred card
            var guideRoot = B.Panel(safe, "SoundGuidePanel", new Vector2(0, 0), new Vector2(1, 1), Vector2.zero, Vector2.zero, B.ScrimFill);
            var guidePanel = B.Panel(guideRoot.gameObject, "Card", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                Vector2.zero, new Vector2(880, 660));
            B.Header(guidePanel.gameObject, "SOUND  GUIDE", 28);
            B.Text(guidePanel.gameObject, "GuideBody",
                "EMITTER          releases one visible pulse.\n" +
                "REFLECTOR      turns the pulse in 45° steps.\n" +
                "ABSORBER        stops a pulse.\n" +
                "SPLITTER          forks one pulse into two.\n" +
                "DELAY PLATE   holds a pulse a set number of beats.\n" +
                "RECEIVER         needs a pulse.  Forbidden ones must stay silent.\n" +
                "RESONATOR      needs two pulses in the right relationship.", 23,
                TextAlignmentOptions.TopLeft, B.Ivory,
                new Vector2(0, 0), new Vector2(1, 1), new Vector2(0, -16), new Vector2(-104, -150));
            var guideClose = B.Button(guidePanel.gameObject, "GuideClose", "BACK", 22,
                new Vector2(0.5f, 0), new Vector2(0.5f, 0), new Vector2(0, 38 + 30), new Vector2(240, 60));
            B.Wire(guideClose, guide, nameof(SoundGuidePanel.Close));
            guideRoot.gameObject.SetActive(false);
            SetRef(guide, "panel", guideRoot.gameObject);

            // hall refs
            SetRef(hall, "dial", dial);
            SetRef(hall, "tuningStrip", strip);
            SetRef(hall, "soundGuide", guide);
            SetRef(hall, "primaryActionLabel", primaryLabel);
            SetRef(hall, "chamberInTuneEmblem", emblem.gameObject);
            SetRef(hall, "resonanceRing", ring.transform);
            SetRef(hall, "rimLight", rimLight);
            var hallSettings = new SerializedObject(hall);
            var modalRefs = hallSettings.FindProperty("modalPanels");
            var modals = new[] { dialPanel.gameObject, stripRoot.gameObject, guideRoot.gameObject };
            modalRefs.arraySize = modals.Length;
            for (int i = 0; i < modals.Length; i++) modalRefs.GetArrayElementAtIndex(i).objectReferenceValue = modals[i];
            var menuRefs = hallSettings.FindProperty("menuElements");
            var menuObjects = new[] { menu.gameObject, title.gameObject, safe.transform.Find("TitleRule").gameObject, emblem.gameObject };
            menuRefs.arraySize = menuObjects.Length;
            for (int i = 0; i < menuObjects.Length; i++) menuRefs.GetArrayElementAtIndex(i).objectReferenceValue = menuObjects[i];
            hallSettings.ApplyModifiedPropertiesWithoutUndo();

            // ring tap -> hall.OnRingTapped via a physical raycast helper on the ring
            var tap = ring.AddComponent<RingTapRelay>();
            SetRef(tap, "hall", hall);

            EchoPolish.DressEnvironment(true);
            Save(s, "ResonanceHall");
            Debug.Log("Echo Forge: Resonance Hall scene built.");
        }

        // Faders + toggle share one left margin (-280) and a 560-wide control band.
        const float FieldLeft = -280f, FieldW = 560f;

        static Slider MakeSlider(GameObject parent, string name, string label, float y)
        {
            B.Text(parent, name + "Label", label, 19, TextAlignmentOptions.Left, B.Ivory,
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(FieldLeft + 250, y + 32), new Vector2(500, 28))
                .characterSpacing = 3f;
            var rt = B.Rect(parent, name, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0, y), new Vector2(FieldW, 10));
            var s = rt.gameObject.AddComponent<Slider>();
            var bg = new GameObject("BG", typeof(Image)); bg.transform.SetParent(rt, false);
            B.Stretch((RectTransform)bg.transform);
            bg.GetComponent<Image>().color = new Color(1, 1, 1, 0.09f);
            var fillArea = new GameObject("Fill Area", typeof(RectTransform)); fillArea.transform.SetParent(rt, false);
            B.Stretch((RectTransform)fillArea.transform);
            var fill = new GameObject("Fill", typeof(Image)); fill.transform.SetParent(fillArea.transform, false);
            var frt = (RectTransform)fill.transform; B.Stretch(frt);
            fill.GetComponent<Image>().color = new Color(B.Bronze.r, B.Bronze.g, B.Bronze.b, 0.92f);
            var handle = new GameObject("Handle", typeof(Image)); handle.transform.SetParent(rt, false);
            var hrt = (RectTransform)handle.transform; hrt.sizeDelta = new Vector2(26, 26);
            var hi = handle.GetComponent<Image>(); hi.sprite = B.Disc(false); hi.color = B.Ivory;
            s.fillRect = frt; s.handleRect = hrt; s.targetGraphic = hi;
            s.minValue = 0; s.maxValue = 1; s.value = 0.8f;
            return s;
        }

        static Toggle MakeToggle(GameObject parent, string name, string label, float y)
        {
            B.Text(parent, name + "Label", label, 19, TextAlignmentOptions.Left, B.Ivory,
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(FieldLeft + 200, y), new Vector2(400, 30))
                .characterSpacing = 3f;
            var rt = B.Rect(parent, name, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(FieldLeft + FieldW - 36, y), new Vector2(72, 34));
            var t = rt.gameObject.AddComponent<Toggle>();
            var bg = new GameObject("BG", typeof(Image)); bg.transform.SetParent(rt, false);
            B.Stretch((RectTransform)bg.transform);
            bg.GetComponent<Image>().color = new Color(1, 1, 1, 0.11f);
            var check = new GameObject("Check", typeof(Image)); check.transform.SetParent(rt, false);
            var crt = (RectTransform)check.transform;
            crt.anchorMin = new Vector2(0, 0); crt.anchorMax = new Vector2(1, 1);
            crt.offsetMin = new Vector2(5, 5); crt.offsetMax = new Vector2(-5, -5);
            check.GetComponent<Image>().color = new Color(B.Amber.r, B.Amber.g, B.Amber.b, 0.95f);
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

            // --- HUD: top rail (TUNES pill left, pause disc right) on one baseline ---
            var tunesPill = B.Panel(safe, "TunesPill", new Vector2(0, 1), new Vector2(0, 1),
                new Vector2(B.UIEdge + 108, -B.UIEdge - 26), new Vector2(216, 52),
                new Color(0.07f, 0.078f, 0.106f, 0.9f));
            B.Rule(tunesPill.gameObject, "PillTick", new Vector2(0, 0), new Vector2(0, 1),
                new Vector2(2, 0), new Vector2(3, -14), B.RuleBronze);
            var tunes = B.Text(tunesPill.gameObject, "AdjustmentCounter", "TUNES  00", 24, TextAlignmentOptions.Center, B.Bronze,
                new Vector2(0, 0), new Vector2(1, 1), Vector2.zero, new Vector2(-8, -6));
            tunes.characterSpacing = 4f;

            var pauseBtn = B.Button(safe, "PauseButton", "II", 24, new Vector2(1, 1), new Vector2(1, 1),
                new Vector2(-B.UIEdge - 42, -B.UIEdge - 42), new Vector2(84, 84),
                new Color(0.09f, 0.10f, 0.13f, 0.92f), rule: false);
            var pauseImg = pauseBtn.GetComponent<Image>();
            pauseImg.sprite = B.Disc(false);
            pauseImg.type = Image.Type.Simple;
            B.Wire(pauseBtn, level, nameof(LevelController.OnPausePressed));

            // --- HUD: context readout (lower-left, bronze-ticked card) ---
            var ctx = B.Panel(safe, "ContextInfo", new Vector2(0, 0), new Vector2(0, 0),
                new Vector2(B.UIEdge + 240, B.UIEdge + 68), new Vector2(480, 136),
                new Color(0.07f, 0.078f, 0.106f, 0.92f));
            var ctxGroup = ctx.gameObject.AddComponent<CanvasGroup>();
            B.Rule(ctx.gameObject, "CtxTick", new Vector2(0, 0), new Vector2(0, 1),
                new Vector2(3, 0), new Vector2(3, -28), B.RuleBronze);
            var ctxTitle = B.Text(ctx.gameObject, "Title", "REFLECTOR", 28, TextAlignmentOptions.TopLeft, B.Bronze,
                new Vector2(0, 0), new Vector2(1, 1), new Vector2(6, 0), new Vector2(-56, -26));
            ctxTitle.characterSpacing = 4f;
            var ctxDetail = B.Text(ctx.gameObject, "Detail", "ANGLE 45", 21, TextAlignmentOptions.BottomLeft, B.Ivory,
                new Vector2(0, 0), new Vector2(1, 1), new Vector2(6, 0), new Vector2(-56, -26));

            // --- HUD: PULSE actuator (bottom-centre circular strike pad) ---
            var pulseBtn = B.Button(safe, "PulseActuator", "PULSE", 30, new Vector2(0.5f, 0), new Vector2(0.5f, 0),
                new Vector2(0, 156), new Vector2(190, 190), new Color(0.42f, 0.28f, 0.12f, 0.94f), rule: false);
            var pulseImg = pulseBtn.GetComponent<Image>();
            pulseImg.sprite = B.Disc(false);
            pulseImg.type = Image.Type.Simple;
            var pulseRing = B.Rect(pulseBtn.gameObject, "Ring", new Vector2(0, 0), new Vector2(1, 1), Vector2.zero, new Vector2(24, 24));
            var pulseRingImg = pulseRing.gameObject.AddComponent<Image>();
            pulseRingImg.sprite = B.Disc(true);
            pulseRingImg.color = new Color(B.Amber.r, B.Amber.g, B.Amber.b, 0.92f);
            pulseRingImg.raycastTarget = false;
            B.Wire(pulseBtn, level, nameof(LevelController.OnPulsePressed));

            // --- tutorial caption (restrained lower-centre pill above the actuator) ---
            var tut = B.Panel(safe, "TutorialLayer", new Vector2(0.5f, 0), new Vector2(0.5f, 0), new Vector2(0, 300),
                new Vector2(660, 62), new Color(0.06f, 0.066f, 0.09f, 0.82f));
            B.Rule(tut.gameObject, "Accent", new Vector2(0, 0), new Vector2(0, 1),
                new Vector2(3, 0), new Vector2(3, -14), new Color(B.Bronze.r, B.Bronze.g, B.Bronze.b, 0.95f));
            var tutGroup = tut.gameObject.AddComponent<CanvasGroup>();
            var tutText = B.Text(tut.gameObject, "Text", "", 23, TextAlignmentOptions.Center, B.Ivory,
                new Vector2(0, 0), new Vector2(1, 1), new Vector2(6, 0), new Vector2(-40, -8));

            // --- result layer: scrim + centred card ---
            var result = B.Panel(safe, "ResultLayer", new Vector2(0, 0), new Vector2(1, 1), Vector2.zero, Vector2.zero, B.ScrimFill);
            var resultCard = B.Panel(result.gameObject, "Card", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                Vector2.zero, new Vector2(640, 528));
            var rTitle = B.Text(resultCard.gameObject, "ResultTitle", "RESONANCE ACHIEVED", 44, TextAlignmentOptions.Center, B.Ivory,
                new Vector2(0, 1), new Vector2(1, 1), new Vector2(0, -86), new Vector2(-56, 64));
            rTitle.characterSpacing = 6f;
            B.Rule(resultCard.gameObject, "TitleRule", new Vector2(0.5f, 1), new Vector2(0.5f, 1), new Vector2(0, -134), new Vector2(120, 3));
            var rReason = B.Text(resultCard.gameObject, "ResultReason", "", 24, TextAlignmentOptions.Center, B.Crimson,
                new Vector2(0, 1), new Vector2(1, 1), new Vector2(0, -168), new Vector2(-72, 40));
            var rRating = B.Text(resultCard.gameObject, "ResultRating", "", 28, TextAlignmentOptions.Center, B.Amber,
                new Vector2(0, 1), new Vector2(1, 1), new Vector2(0, -212), new Vector2(-72, 44));
            var nextBtn = B.Button(resultCard.gameObject, "NextChamber", "NEXT CHAMBER", 26, new Vector2(0.5f, 0), new Vector2(0.5f, 0),
                new Vector2(0, 40 + B.BtnHBig + B.BtnGap + B.BtnH * 0.5f), new Vector2(468, B.BtnHBig), primary: true);
            B.Wire(nextBtn, level, nameof(LevelController.OnNextPressed));
            var retuneBtn = B.Button(resultCard.gameObject, "Retune", "RETUNE", 22, new Vector2(0.5f, 0), new Vector2(0.5f, 0),
                new Vector2(-118, 40 + B.BtnH * 0.5f), new Vector2(226, B.BtnH));
            B.Wire(retuneBtn, level, nameof(LevelController.OnRetunePressed));
            var toDial = B.Button(resultCard.gameObject, "ToDial", "RESONANCE DIAL", 22, new Vector2(0.5f, 0), new Vector2(0.5f, 0),
                new Vector2(118, 40 + B.BtnH * 0.5f), new Vector2(226, B.BtnH));
            B.Wire(toDial, level, nameof(LevelController.OnDialPressed));
            result.gameObject.SetActive(false);

            // --- pause layer: scrim + centred card, one button column ---
            var pause = B.Panel(safe, "PauseLayer", new Vector2(0, 0), new Vector2(1, 1), Vector2.zero, Vector2.zero, B.ScrimFill);
            var pauseCard = B.Panel(pause.gameObject, "Card", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                Vector2.zero, new Vector2(468, 540));
            B.Header(pauseCard.gameObject, "SOUND  DAMPING", 28);
            var pStack = B.ButtonStack(pauseCard.gameObject, 140f, 384f, B.BtnH, new[]
            {
                ("Resume", "RESUME", 26f),
                ("Restart", "RESTART CHAMBER", 24f),
                ("PauseGuide", "SOUND GUIDE", 24f),
                ("ExitSession", "EXIT SESSION", 24f),
            }, primaryFirst: true);
            B.Wire(pStack[0], level, nameof(LevelController.OnResumePressed));
            B.Wire(pStack[1], level, nameof(LevelController.OnRestartPressed));
            B.Wire(pStack[2], level, nameof(LevelController.OnSoundGuideOpen));
            B.Wire(pStack[3], level, nameof(LevelController.OnExitSessionPressed));
            pause.gameObject.SetActive(false);

            // --- dialog + chamber sound-guide layers ---
            var dialog = B.Panel(safe, "DialogLayer", new Vector2(0, 0), new Vector2(1, 1), Vector2.zero, Vector2.zero, B.ScrimFill);
            var dialogText = B.Text(dialog.gameObject, "DialogText", "", 28, TextAlignmentOptions.Center, B.Ivory,
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0, 40), new Vector2(820, 160));
            dialog.gameObject.SetActive(false);

            var guideL = B.Panel(safe, "SoundGuideLayer", new Vector2(0, 0), new Vector2(1, 1), Vector2.zero, Vector2.zero, B.ScrimFill);
            var guideLCard = B.Panel(guideL.gameObject, "Card", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                Vector2.zero, new Vector2(920, 560));
            B.Header(guideLCard.gameObject, "SOUND  GUIDE", 28);
            B.Text(guideLCard.gameObject, "GuideBody",
                "EMITTER          releases one visible pulse.\n" +
                "REFLECTOR      turns the pulse in 45° steps.\n" +
                "ABSORBER        stops a pulse.\n" +
                "SPLITTER          forks one pulse into two.\n" +
                "DELAY PLATE   holds a pulse a set number of beats.\n" +
                "RECEIVER         needs a pulse.  Forbidden ones must stay silent.\n" +
                "RESONATOR      needs two pulses in the right relationship.", 22,
                TextAlignmentOptions.TopLeft, B.Ivory,
                new Vector2(0, 0), new Vector2(1, 1), new Vector2(0, -6), new Vector2(-120, -150));
            var guideLClose = B.Button(guideLCard.gameObject, "GuideLClose", "BACK", 22, new Vector2(0.5f, 0), new Vector2(0.5f, 0),
                new Vector2(0, 36 + 30), new Vector2(240, 60));
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

            EchoPolish.DressEnvironment(false);
            Save(s, "EchoChamber");
            Debug.Log("Echo Forge: Echo Chamber scene built.");
        }
    }
}
