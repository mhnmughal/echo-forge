using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using TMPro;

namespace EchoForge.EditorTools
{
    /// <summary>Shared palette, material factory and UI factory for the scene builders.
    /// Scenes are assembled at edit time and serialized into .unity files - the runtime
    /// assemblies never construct primary UI.</summary>
    public static class EchoBuild
    {
        public const string ArtDir = "Assets/EchoForge/Art";
        public const string MatDir = ArtDir + "/Materials";
        public const string ResDir = "Assets/EchoForge/Resources";
        public const string SceneDir = "Assets/EchoForge/Scenes";

        // ---- palette (master prompt s54) ----
        public static readonly Color MidnightNavy = Hex("10131E");
        public static readonly Color Graphite = Hex("1D2028");
        public static readonly Color MatteBlack = Hex("0A0B0E");
        public static readonly Color Bronze = Hex("B0762E");
        public static readonly Color BronzeDark = Hex("6E4A22");
        public static readonly Color Ivory = Hex("EDE6D6");
        public static readonly Color PaleViolet = Hex("B7A9F2");
        public static readonly Color Amber = Hex("E0A83A");
        public static readonly Color Crimson = Hex("8C1E28");
        public static readonly Color PanelDark = new Color(0.055f, 0.062f, 0.09f, 0.92f);

        public static Color Hex(string h)
        {
            ColorUtility.TryParseHtmlString("#" + h, out var c);
            return c;
        }

        public static void EnsureDirs()
        {
            foreach (var d in new[] { ArtDir, MatDir, ResDir, SceneDir })
            {
                if (!AssetDatabase.IsValidFolder(d))
                {
                    var parent = Path.GetDirectoryName(d).Replace('\\', '/');
                    AssetDatabase.CreateFolder(parent, Path.GetFileName(d));
                }
            }
        }

        public static Material Mat(string name, Color baseColor, float metallic, float smoothness, Color emission = default)
        {
            EnsureDirs();
            string path = $"{MatDir}/{name}.mat";
            var mat = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (mat == null)
            {
                mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                AssetDatabase.CreateAsset(mat, path);
            }
            mat.SetColor("_BaseColor", baseColor);
            mat.SetFloat("_Metallic", metallic);
            mat.SetFloat("_Smoothness", smoothness);
            if (emission != default && emission != Color.clear)
            {
                mat.EnableKeyword("_EMISSION");
                mat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
                mat.SetColor("_EmissionColor", emission);
            }
            EditorUtility.SetDirty(mat);
            return mat;
        }

        // ---------- generated UI sprites ----------

        /// <summary>Soft-edged disc (filled) or ring, as a reusable sprite asset. Used for the
        /// circular PULSE actuator and radial indicators so no UI element is a bare rectangle.</summary>
        public static Sprite Disc(bool ring)
        {
            EnsureDirs();
            string name = ring ? "ui_ring" : "ui_disc";
            string path = $"{ArtDir}/{name}.png";

            if (!File.Exists(path))
            {
                const int S = 256;
                var tex = new Texture2D(S, S, TextureFormat.RGBA32, false);
                var px = new Color[S * S];
                Vector2 c = new Vector2(S / 2f, S / 2f);
                for (int y = 0; y < S; y++)
                for (int x = 0; x < S; x++)
                {
                    float d = (new Vector2(x, y) - c).magnitude / (S / 2f - 2f);
                    float a = ring
                        ? Mathf.SmoothStep(0.5f, 0f, Mathf.Abs(d - 0.88f) / 0.10f)
                        : Mathf.SmoothStep(1.0f, 0.94f, d) * (d <= 1f ? 1f : 0f);
                    px[y * S + x] = new Color(1f, 1f, 1f, Mathf.Clamp01(a));
                }
                tex.SetPixels(px);
                tex.Apply();
                File.WriteAllBytes(path, tex.EncodeToPNG());
                Object.DestroyImmediate(tex);
            }

            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceSynchronousImport);
            var imp = (TextureImporter)AssetImporter.GetAtPath(path);
            if (imp != null)
            {
                imp.textureType = TextureImporterType.Sprite;
                imp.spriteImportMode = SpriteImportMode.Single;
                imp.mipmapEnabled = false;
                imp.alphaIsTransparency = true;
                imp.wrapMode = TextureWrapMode.Clamp;
                imp.SaveAndReimport();
            }
            var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            if (sprite == null) Debug.LogWarning($"EchoBuild.Disc: sprite still null at {path}");
            return sprite;
        }

        // ---------- UI factory ----------

        public static TMP_FontAsset Font =>
            TMP_Settings.defaultFontAsset != null
                ? TMP_Settings.defaultFontAsset
                : AssetDatabase.LoadAssetAtPath<TMP_FontAsset>("Assets/TextMesh Pro/Resources/Fonts & Materials/LiberationSans SDF.asset");

        public static (GameObject canvas, GameObject safeArea) Canvas(string name, bool camera, Camera cam = null)
        {
            var go = new GameObject(name, typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            var c = go.GetComponent<Canvas>();
            if (camera && cam != null)
            {
                c.renderMode = RenderMode.ScreenSpaceCamera;
                c.worldCamera = cam;
                c.planeDistance = 1f;
            }
            else c.renderMode = RenderMode.ScreenSpaceOverlay;

            var scaler = go.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 1f;

            var safe = new GameObject("SafeArea", typeof(RectTransform), typeof(EchoForge.Chamber.SafeAreaFitter));
            var rt = (RectTransform)safe.transform;
            rt.SetParent(go.transform, false);
            Stretch(rt);
            return (go, safe);
        }

        public static GameObject EventSystem()
        {
            return new GameObject("EventSystem",
                typeof(UnityEngine.EventSystems.EventSystem),
                typeof(UnityEngine.InputSystem.UI.InputSystemUIInputModule));
        }

        public static void Stretch(RectTransform rt)
        {
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }

        public static RectTransform Rect(GameObject parent, string name, Vector2 anchorMin, Vector2 anchorMax,
            Vector2 pos, Vector2 size)
        {
            var go = new GameObject(name, typeof(RectTransform));
            var rt = go.GetComponent<RectTransform>();
            rt.SetParent(parent.transform, false);
            rt.anchorMin = anchorMin;
            rt.anchorMax = anchorMax;
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = pos;
            rt.sizeDelta = size;
            return rt;
        }

        public static Image Panel(GameObject parent, string name, Vector2 anchorMin, Vector2 anchorMax,
            Vector2 pos, Vector2 size, Color? color = null)
        {
            var rt = Rect(parent, name, anchorMin, anchorMax, pos, size);
            var img = rt.gameObject.AddComponent<Image>();
            img.color = color ?? PanelDark;
            return img;
        }

        public static TMP_Text Text(GameObject parent, string name, string content, float size,
            TextAlignmentOptions align, Color? color = null,
            Vector2? anchorMin = null, Vector2? anchorMax = null, Vector2? pos = null, Vector2? sizeDelta = null)
        {
            var rt = Rect(parent, name, anchorMin ?? Vector2.zero, anchorMax ?? Vector2.one,
                pos ?? Vector2.zero, sizeDelta ?? new Vector2(-24, -12));
            var t = rt.gameObject.AddComponent<TextMeshProUGUI>();
            t.font = Font;
            t.text = content;
            t.fontSize = size;
            t.alignment = align;
            t.color = color ?? Ivory;
            t.textWrappingMode = TextWrappingModes.Normal;
            t.raycastTarget = false;
            return t;
        }

        public static Button Button(GameObject parent, string name, string label, float labelSize,
            Vector2 anchorMin, Vector2 anchorMax, Vector2 pos, Vector2 size, Color? bg = null, Color? fg = null)
        {
            var rt = Rect(parent, name, anchorMin, anchorMax, pos, size);
            var img = rt.gameObject.AddComponent<Image>();
            img.color = bg ?? new Color(Bronze.r, Bronze.g, Bronze.b, 0.16f);
            var btn = rt.gameObject.AddComponent<Button>();
            btn.targetGraphic = img;
            var colors = btn.colors;
            colors.highlightedColor = new Color(1, 1, 1, 1.0f);
            colors.pressedColor = new Color(0.8f, 0.8f, 0.8f, 1f);
            colors.fadeDuration = 0.08f;
            btn.colors = colors;

            var t = Text(rt.gameObject, "Label", label, labelSize, TextAlignmentOptions.Center, fg ?? Ivory);
            Stretch((RectTransform)t.transform);
            return btn;
        }

        public static void Wire(Button b, UnityEngine.Object target, string method)
        {
            var action = (UnityEngine.Events.UnityAction)System.Delegate.CreateDelegate(
                typeof(UnityEngine.Events.UnityAction), target, method);
            UnityEditor.Events.UnityEventTools.AddPersistentListener(b.onClick, action);
        }

        public static void WireFloat(Slider s, UnityEngine.Object target, string method)
        {
            var action = (UnityEngine.Events.UnityAction<float>)System.Delegate.CreateDelegate(
                typeof(UnityEngine.Events.UnityAction<float>), target, method);
            UnityEditor.Events.UnityEventTools.AddPersistentListener(s.onValueChanged, action);
        }

        public static void WireBool(Toggle t, UnityEngine.Object target, string method)
        {
            var action = (UnityEngine.Events.UnityAction<bool>)System.Delegate.CreateDelegate(
                typeof(UnityEngine.Events.UnityAction<bool>), target, method);
            UnityEditor.Events.UnityEventTools.AddPersistentListener(t.onValueChanged, action);
        }

        public static GameObject Primitive(PrimitiveType type, string name, Transform parent, Vector3 pos,
            Vector3 euler, Vector3 scale, Material mat, bool collider = false)
        {
            var go = GameObject.CreatePrimitive(type);
            go.name = name;
            if (!collider)
            {
                var col = go.GetComponent<Collider>();
                if (col != null) Object.DestroyImmediate(col);
            }
            go.transform.SetParent(parent, false);
            go.transform.localPosition = pos;
            go.transform.localEulerAngles = euler;
            go.transform.localScale = scale;
            if (mat != null) go.GetComponent<Renderer>().sharedMaterial = mat;
            return go;
        }

        public static Light AddLight(string name, Transform parent, LightType type, Color color,
            float intensity, Vector3 pos, Vector3 euler)
        {
            var go = new GameObject(name, typeof(Light));
            go.transform.SetParent(parent, false);
            go.transform.localPosition = pos;
            go.transform.localEulerAngles = euler;
            var l = go.GetComponent<Light>();
            l.type = type;
            l.color = color;
            l.intensity = intensity;
            l.shadows = type == LightType.Directional ? LightShadows.Soft : LightShadows.None;
            return l;
        }

        public static void SetAmbient()
        {
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.085f, 0.092f, 0.13f);
            RenderSettings.fog = false;
        }

        // ---------- post-processing ----------

        public static VolumeProfile PostProfile()
        {
            EnsureDirs();
            const string path = ArtDir + "/EchoForge_Post.asset";
            var profile = AssetDatabase.LoadAssetAtPath<VolumeProfile>(path);
            if (profile == null)
            {
                profile = ScriptableObject.CreateInstance<VolumeProfile>();
                AssetDatabase.CreateAsset(profile, path);
            }
            foreach (var c in profile.components.ToArray()) profile.Remove(c.GetType());

            var bloom = profile.Add<Bloom>(true);
            bloom.threshold.Override(0.95f);
            bloom.intensity.Override(0.7f);
            bloom.scatter.Override(0.65f);

            var tone = profile.Add<Tonemapping>(true);
            tone.mode.Override(TonemappingMode.ACES);

            var color = profile.Add<ColorAdjustments>(true);
            color.postExposure.Override(0f);
            color.contrast.Override(8f);
            color.saturation.Override(3f);
            color.colorFilter.Override(new Color(1f, 0.98f, 0.95f));

            var vig = profile.Add<Vignette>(true);
            vig.intensity.Override(0.28f);
            vig.smoothness.Override(0.55f);
            vig.color.Override(new Color(0.02f, 0.02f, 0.035f));

            EditorUtility.SetDirty(profile);
            return profile;
        }

        public static void AddPostVolume(Camera cam)
        {
            var go = new GameObject("PostVolume", typeof(Volume));
            var v = go.GetComponent<Volume>();
            v.isGlobal = true;
            v.priority = 0;
            v.sharedProfile = PostProfile();

            var data = cam.GetUniversalAdditionalCameraData();
            if (data != null)
            {
                data.renderPostProcessing = true;
                data.antialiasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing;
                data.antialiasingQuality = AntialiasingQuality.Medium;
            }
        }
    }
}
