using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;
using B = EchoForge.EditorTools.EchoBuild;

namespace EchoForge.EditorTools
{
    /// <summary>Procedurally renders the Echo Forge production app icon: a bronze resonance
    /// ring struck by a visible curved acoustic pulse converging on an amber resonant point,
    /// on a deep midnight ground. Original raster art, no external source. 1024 master + QA
    /// downscales, then assigned to Player Settings.</summary>
    public static class IconGen
    {
        const int Master = 1024;
        const int SS = 2; // supersample factor
        static readonly string IconDir = $"{B.ArtDir}/Icon";

        [MenuItem("Echo Forge/Build/6 - Generate Icon")]
        public static void Generate()
        {
            Directory.CreateDirectory(IconDir);
            var master = Render(Master * SS);
            var down = Downscale(master, Master);
            Object.DestroyImmediate(master);

            string masterPath = $"{IconDir}/EchoForge_Icon_1024.png";
            File.WriteAllBytes(masterPath, down.EncodeToPNG());

            foreach (int size in new[] { 180, 120, 60 })
            {
                var small = Downscale(down, size);
                File.WriteAllBytes($"{IconDir}/EchoForge_Icon_{size}.png", small.EncodeToPNG());
                Object.DestroyImmediate(small);
            }
            Object.DestroyImmediate(down);

            AssetDatabase.Refresh();

            var imp = (TextureImporter)AssetImporter.GetAtPath(masterPath);
            if (imp != null)
            {
                imp.textureType = TextureImporterType.Default;
                imp.mipmapEnabled = false;
                imp.alphaIsTransparency = false;
                imp.sRGBTexture = true;
                imp.maxTextureSize = 1024;
                imp.SaveAndReimport();
            }

            AssignToPlayerSettings(masterPath);
            Debug.Log($"<b>Echo Forge: icon generated -> {masterPath} and assigned to Player Settings.</b>");
        }

        static void AssignToPlayerSettings(string masterPath)
        {
            var tex = AssetDatabase.LoadAssetAtPath<Texture2D>(masterPath);
            if (tex == null) return;

            foreach (var nbt in new[] { NamedBuildTarget.Standalone, NamedBuildTarget.iOS, NamedBuildTarget.Android })
            {
                try
                {
                    var sizes = PlayerSettings.GetIconSizes(nbt, IconKind.Application);
                    if (sizes == null || sizes.Length == 0) continue;
                    var arr = new Texture2D[sizes.Length];
                    for (int i = 0; i < arr.Length; i++) arr[i] = tex;
                    PlayerSettings.SetIcons(nbt, arr, IconKind.Application);
                }
                catch { /* platform module not installed - fine */ }
            }
        }

        // ---------------- render ----------------

        static Texture2D Render(int N)
        {
            var tex = new Texture2D(N, N, TextureFormat.RGBA32, false);
            var px = new Color[N * N];

            Vector2 c = new Vector2(0.485f, 0.50f);          // ring centre
            float rMid = 0.335f, ringHalf = 0.080f;
            Vector2 point = new Vector2(0.645f, 0.38f);       // amber resonant point (on the ring, lower-right)
            Vector2 lightDir = new Vector2(-0.55f, 0.83f).normalized;

            Color navy = B.MidnightNavy, graphite = B.Graphite;
            Color bronze = B.Bronze, bronzeDark = B.BronzeDark;
            Color violet = B.PaleViolet, ivory = B.Ivory, amber = B.Amber;

            for (int y = 0; y < N; y++)
            for (int x = 0; x < N; x++)
            {
                Vector2 uv = new Vector2(x / (float)(N - 1), y / (float)(N - 1));
                Vector2 d = uv - c;
                float r = d.magnitude;
                float ang = Mathf.Atan2(d.y, d.x);

                float edgeK = Mathf.Clamp01((uv - new Vector2(0.5f, 0.5f)).magnitude * 1.4f);

                // background: warm dark glow behind the ring, near-black at the corners
                Color col = Color.Lerp(navy * 0.85f, new Color(0.02f, 0.02f, 0.03f), Mathf.SmoothStep(0f, 1f, edgeK));
                col += bronze * 0.10f * Mathf.Exp(-Mathf.Pow(r / 0.42f, 2f));   // soft backlight

                // ---- curved acoustic pulse: three restrained concentric wavefronts ----
                // sweeping in from the lower-left, an angular window opening toward the point
                float toPointAng = Mathf.Atan2((point - c).y, (point - c).x);
                float incomingAng = toPointAng + Mathf.PI;   // opposite side = where the pulse enters
                float dA = Mathf.Abs(Mathf.DeltaAngle(ang * Mathf.Rad2Deg, incomingAng * Mathf.Rad2Deg)) / 90f;
                float wedge = Mathf.SmoothStep(1.15f, 0.15f, dA);
                for (int band = 0; band < 3; band++)
                {
                    float br = 0.30f + band * 0.085f;
                    float w = 0.012f + band * 0.004f;
                    float bandv = Mathf.Exp(-Mathf.Pow((r - br) / w, 2f)) * wedge * (1f - band * 0.16f);
                    Color pc = Color.Lerp(ivory, violet, 0.25f + band * 0.3f);
                    col += pc * bandv * 1.5f;
                }
                // a short reflected wave-packet curling off the ring toward the point
                Vector2 toPt = (point - uv);
                float ptLine = Mathf.Exp(-Mathf.Pow((toPt.magnitude - 0.10f) / 0.05f, 2f));
                col += violet * ptLine * Mathf.SmoothStep(0.5f, 0f, (uv - point).magnitude / 0.22f) * 0.5f;

                // ---- bronze resonance ring ----
                float ringD = Mathf.Abs(r - rMid);
                if (ringD < ringHalf + 0.015f)
                {
                    float shape = Mathf.SmoothStep(ringHalf + 0.010f, ringHalf - 0.030f, ringD);
                    float lit = Mathf.Clamp01(0.34f + 0.66f * Vector2.Dot(d.normalized, lightDir));
                    // rounded cross-section shading
                    float cross = 1f - Mathf.Pow(ringD / ringHalf, 2f);
                    Color metal = Color.Lerp(bronzeDark * 0.85f, bronze * 1.15f, lit);
                    metal = Color.Lerp(metal * (0.6f + 0.4f * cross), metal, cross);
                    metal += new Color(1f, 0.85f, 0.6f) * Mathf.Pow(lit, 11f) * cross * 0.4f; // warm specular glint
                    metal += bronze * (1f - lit) * 0.30f;                            // warm ambient fill on the dark side
                    metal = Color.Lerp(metal, bronze, 0.28f);                        // keep it saturated bronze
                    col = Color.Lerp(col, metal, shape);

                    // pulse impact flare where the wavefront strikes the ring
                    col += Color.Lerp(violet, ivory, 0.4f) * shape * wedge *
                           Mathf.Exp(-Mathf.Pow((r - rMid) / 0.06f, 2f)) * 0.6f;
                }

                // ---- amber resonant point + bloom ----
                float pd = (uv - point).magnitude;
                col += amber * Mathf.Exp(-Mathf.Pow(pd / 0.028f, 2f)) * 2.2f;
                col += amber * Mathf.Exp(-Mathf.Pow(pd / 0.090f, 2f)) * 0.55f;
                col += ivory * Mathf.Exp(-Mathf.Pow(pd / 0.012f, 2f)) * 1.0f;        // hot core

                // gentle vignette
                col *= 1f - 0.18f * edgeK;

                col.r = Mathf.Clamp01(col.r); col.g = Mathf.Clamp01(col.g); col.b = Mathf.Clamp01(col.b);
                col.a = 1f;
                px[y * N + x] = col;
            }

            tex.SetPixels(px);
            tex.Apply();
            return tex;
        }

        static Texture2D Downscale(Texture2D src, int size)
        {
            var rt = RenderTexture.GetTemporary(size, size, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
            var prev = RenderTexture.active;
            Graphics.Blit(src, rt);
            RenderTexture.active = rt;
            var dst = new Texture2D(size, size, TextureFormat.RGBA32, false);
            dst.ReadPixels(new Rect(0, 0, size, size), 0, 0);
            dst.Apply();
            RenderTexture.active = prev;
            RenderTexture.ReleaseTemporary(rt);
            return dst;
        }
    }
}
