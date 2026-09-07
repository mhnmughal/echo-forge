using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

namespace EchoForge.EditorTools
{
    /// <summary>Edit-time generators for the presentation-polish assets: a real torus mesh
    /// for the Resonance Ring (was a flat disc) and an AudioMixer with the Master/Ambience/
    /// SFX/UI buses. Both are serialized asset files; the runtime never builds them.</summary>
    public static class EchoPolish
    {
        public const string TorusPath = EchoBuild.ArtDir + "/ResonanceRing.asset";
        public const string MixerPath = EchoBuild.ArtDir + "/EchoForge.mixer";

        /// <summary>Pads any control smaller than the 96x80 minimum touch target with an
        /// invisible child that expands only the short axis. Needs real RectTransform sizes,
        /// so it forces a layout pass first. No-op for controls already large enough.</summary>
        public static void ImproveTouchTargets()
        {
            Canvas.ForceUpdateCanvases();
            foreach (var control in UnityEngine.Object.FindObjectsByType<UnityEngine.UI.Selectable>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                var rt = control.transform as RectTransform;
                if (rt == null || rt.Find("TouchArea") != null) continue;
                float w = rt.rect.width, h = rt.rect.height;
                if (w <= 1f || h <= 1f) continue;                 // degenerate rect: skip rather than balloon it
                float padX = Mathf.Max(0f, 96f - w), padY = Mathf.Max(0f, 80f - h);
                if (padX <= 0f && padY <= 0f) continue;           // already a comfortable target
                var area = new GameObject("TouchArea", typeof(RectTransform), typeof(UnityEngine.UI.Image));
                var touch = (RectTransform)area.transform;
                touch.SetParent(rt, false);
                touch.SetAsFirstSibling();
                touch.anchorMin = Vector2.zero; touch.anchorMax = Vector2.one;
                touch.sizeDelta = new Vector2(padX, padY);
                var img = area.GetComponent<UnityEngine.UI.Image>();
                img.color = Color.clear;
                img.raycastTarget = true;
            }
        }

        public static void DressEnvironment(bool hall)
        {
            var root = GameObject.Find(hall ? "HallEnvironment" : "ChamberEnvironment");
            if (root == null || root.transform.Find("AcousticDetail") != null) return;
            var detail = new GameObject("AcousticDetail").transform;
            detail.SetParent(root.transform, false);
            var bronze = AssetDatabase.LoadAssetAtPath<Material>(EchoBuild.MatDir + "/Env_Bronze.mat");
            var fabric = EchoBuild.Mat("Env_Membrane", EchoBuild.Hex("494454"), 0.15f, 0.35f);
            var ribs = EchoBuild.Mat("Env_Ribs", EchoBuild.Hex("33333D"), 0.3f, 0.3f);
            var torus = AssetDatabase.LoadAssetAtPath<Mesh>(TorusPath) ?? BuildTorus();
            if (hall)
            {
                foreach (Transform child in root.transform)
                    if (child.name.StartsWith("WallPanel") && child.localPosition.z < 1f) child.gameObject.SetActive(false);
            }
            for (int i = -2; i <= 2; i++)
            {
                var position = hall ? new Vector3(i * 2.5f, 2.5f, 8.5f - Mathf.Abs(i) * 0.6f)
                    : new Vector3(i * 3f, 2.1f, 6.25f);
                var ring = new GameObject("MembraneRim" + i, typeof(MeshFilter), typeof(MeshRenderer));
                ring.transform.SetParent(detail, false);
                ring.transform.localPosition = position;
                ring.transform.localRotation = Quaternion.Euler(90, 0, 0);
                ring.transform.localScale = Vector3.one * 0.75f;
                ring.GetComponent<MeshFilter>().sharedMesh = torus;
                ring.GetComponent<MeshRenderer>().sharedMaterial = bronze;
                EchoBuild.Primitive(PrimitiveType.Cylinder, "ResonantMembrane" + i, detail,
                    position + Vector3.forward * 0.04f, new Vector3(90, 0, 0), new Vector3(1.3f, 0.025f, 1.3f), fabric);
                for (int rib = -3; rib <= 3; rib++)
                    EchoBuild.Primitive(PrimitiveType.Cube, "AbsorptionRib", detail,
                        position + new Vector3(rib * 0.20f, -1.25f, 0.07f), Vector3.zero,
                        new Vector3(0.06f, 0.65f, 0.12f), ribs);
            }
            if (hall)
                for (int i = 0; i < 24; i++)
                {
                    float a = i * Mathf.PI / 12f;
                    EchoBuild.Primitive(PrimitiveType.Cube, "RadialInlay" + i, detail,
                        new Vector3(Mathf.Sin(a) * 5.2f, 0.075f, 2f + Mathf.Cos(a) * 5.2f),
                        new Vector3(0, a * Mathf.Rad2Deg, 0), new Vector3(0.028f, 0.025f, 2.8f), bronze);
                }
            else
                for (int i = -6; i <= 6; i++)
                    EchoBuild.Primitive(PrimitiveType.Cylinder, "FrameFastener" + i, detail,
                        new Vector3(i, 0.32f, 5.6f), Vector3.zero, new Vector3(0.10f, 0.025f, 0.10f), ribs);
        }

        [MenuItem("Echo Forge/Build/8 - Ring Mesh + Mixer")]
        public static void BuildAll()
        {
            BuildTorus();
            BuildMixer();
            AssetDatabase.SaveAssets();
        }

        // ---------------------------------------------------------------- torus mesh

        /// <summary>Torus in the XZ plane, major R = 1, tube r = 0.13 - scale the transform to
        /// size it. Replaces the flat scaled-cylinder that used to stand in for the ring.</summary>
        public static Mesh BuildTorus()
        {
            EchoBuild.EnsureDirs();
            const int seg = 96, side = 20;
            const float R = 1f, r = 0.13f;

            var verts = new Vector3[(seg + 1) * (side + 1)];
            var norms = new Vector3[verts.Length];
            var uvs = new Vector2[verts.Length];
            var tris = new int[seg * side * 6];

            int v = 0;
            for (int i = 0; i <= seg; i++)
            {
                float u = i / (float)seg * Mathf.PI * 2f;
                var center = new Vector3(Mathf.Cos(u) * R, 0f, Mathf.Sin(u) * R);
                var radial = new Vector3(Mathf.Cos(u), 0f, Mathf.Sin(u));
                for (int j = 0; j <= side; j++, v++)
                {
                    float t = j / (float)side * Mathf.PI * 2f;
                    var n = radial * Mathf.Cos(t) + Vector3.up * Mathf.Sin(t);
                    verts[v] = center + n * r;
                    norms[v] = n;
                    uvs[v] = new Vector2(i / (float)seg, j / (float)side);
                }
            }

            int ti = 0, stride = side + 1;
            for (int i = 0; i < seg; i++)
            for (int j = 0; j < side; j++)
            {
                int a = i * stride + j, b = (i + 1) * stride + j;
                tris[ti++] = a; tris[ti++] = a + 1; tris[ti++] = b;
                tris[ti++] = b; tris[ti++] = a + 1; tris[ti++] = b + 1;
            }

            var mesh = AssetDatabase.LoadAssetAtPath<Mesh>(TorusPath);
            if (mesh == null) { mesh = new Mesh(); AssetDatabase.CreateAsset(mesh, TorusPath); }
            mesh.Clear();
            mesh.name = "ResonanceRing";
            mesh.vertices = verts;
            mesh.normals = norms;
            mesh.uv = uvs;
            mesh.triangles = tris;
            mesh.RecalculateBounds();
            EditorUtility.SetDirty(mesh);
            Debug.Log($"Echo Forge: torus mesh built ({verts.Length} verts) -> {TorusPath}");
            return mesh;
        }

        // ---------------------------------------------------------------- audio mixer

        /// <summary>AudioMixer asset: Master -> {Ambience, SFX, UI}, exposing MusicVol /
        /// EffectsVol / UIVol. Built through the internal AudioMixerController by reflection
        /// (no public creation API). On any failure it logs and deletes the partial asset -
        /// AudioController already ships a working mixer-optional volume path.</summary>
        public static AudioMixer BuildMixer()
        {
            var existing = AssetDatabase.LoadAssetAtPath<AudioMixer>(MixerPath);
            if (existing != null) { Debug.Log("Echo Forge: mixer already present."); return existing; }

            try
            {
                var asm = typeof(Editor).Assembly; // UnityEditor
                var ctrlT = asm.GetType("UnityEditor.Audio.AudioMixerController");
                var pathT = asm.GetType("UnityEditor.Audio.AudioGroupParameterPath");
                var epT = asm.GetType("UnityEditor.Audio.ExposedAudioParameter");
                if (ctrlT == null || pathT == null || epT == null)
                { Debug.LogWarning("EchoPolish: audio editor internals not found - mixer skipped."); return null; }

                const BindingFlags any = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

                var mixer = ctrlT.GetMethod("CreateMixerControllerAtPath", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
                    .Invoke(null, new object[] { MixerPath });

                var createGroup = ctrlT.GetMethod("CreateNewGroup", any);
                var addChild = ctrlT.GetMethod("AddChildToParent", any);
                var addExposed = ctrlT.GetMethod("AddExposedParameter", any);
                var exposedProp = ctrlT.GetProperty("exposedParameters", any);
                var master = ctrlT.GetProperty("masterGroup", any).GetValue(mixer);
                var groupT = master.GetType();
                var guidT = pathT.GetConstructors()[0].GetParameters()[1].ParameterType;
                var pathCtor = pathT.GetConstructor(new[] { groupT, guidT });
                var epName = epT.GetField("name");

                foreach (var (child, param) in new[]
                         {
                             ("Ambience", "MusicVol"),
                             ("SFX", "EffectsVol"),
                             ("UI", "UIVol"),
                         })
                {
                    var g = createGroup.Invoke(mixer, new object[] { child, false });
                    addChild.Invoke(mixer, new[] { g, master });

                    var guid = groupT.GetMethod("GetGUIDForVolume", any).Invoke(g, null);
                    addExposed.Invoke(mixer, new[] { pathCtor.Invoke(new[] { g, guid }) });

                    // AddExposedParameter names it "Exposed Parameter N" - rename the last entry.
                    var arr = (Array)exposedProp.GetValue(mixer);
                    object box = arr.GetValue(arr.Length - 1);   // boxed struct copy
                    epName.SetValue(box, param);
                    arr.SetValue(box, arr.Length - 1);
                    exposedProp.SetValue(mixer, arr);
                }

                EditorUtility.SetDirty((UnityEngine.Object)mixer);
                AssetDatabase.SaveAssets();
                Debug.Log($"Echo Forge: AudioMixer built (Master/Ambience/SFX/UI; MusicVol/EffectsVol/UIVol) -> {MixerPath}");
                return AssetDatabase.LoadAssetAtPath<AudioMixer>(MixerPath);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"EchoPolish.BuildMixer failed ({e.GetType().Name}: {e.InnerException?.Message ?? e.Message}). " +
                                 "Mixer-optional path in AudioController ships instead.");
                if (AssetDatabase.LoadAssetAtPath<AudioMixer>(MixerPath) == null)
                    AssetDatabase.DeleteAsset(MixerPath);
                return null;
            }
        }
    }
}
