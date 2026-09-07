using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace EchoForge.EditorTools
{
    /// <summary>Drives the Game View to each target device aspect for the responsive/safe-area
    /// QA pass. Landscape only (the game is landscape-locked). Editor tooling - never shipped.</summary>
    public static class ResponsiveQA
    {
        // label -> (w, h). Landscape. Covers 16:9 .. 21:9 phones and 4:3 / ~4.3:3 tablets.
        public static readonly (string label, int w, int h)[] Targets =
        {
            ("16x9_1920",       1920, 1080), // baseline phone / many Android
            ("19_5x9_iPhone",   2340, 1080), // iPhone 12-15 class
            ("21x9_ultrawide",  2520, 1080), // extra-wide phones (Xperia etc.)
            ("4x3_iPad",        1440, 1080), // classic iPad
            ("4_3x3_iPadPro",   1550, 1080), // iPad Pro 11" class (~1.43:1)
        };

        [MenuItem("Echo Forge/QA/Game View - 16x9")]  public static void S0() => Set(Targets[0]);
        [MenuItem("Echo Forge/QA/Game View - 19.5x9")] public static void S1() => Set(Targets[1]);
        [MenuItem("Echo Forge/QA/Game View - 21x9")]  public static void S2() => Set(Targets[2]);
        [MenuItem("Echo Forge/QA/Game View - 4x3")]   public static void S3() => Set(Targets[3]);
        [MenuItem("Echo Forge/QA/Game View - 4.3x3")] public static void S4() => Set(Targets[4]);

        static void Set((string label, int w, int h) t) => SetGameViewSize(t.w, t.h, "EF_" + t.label);

        /// <summary>Adds (once) a fixed-resolution custom Game View size and selects it.</summary>
        public static void SetGameViewSize(int width, int height, string label)
        {
            const BindingFlags BF = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
            var asm = typeof(Editor).Assembly;
            var sizesType = asm.GetType("UnityEditor.GameViewSizes");
            var singleton = typeof(ScriptableSingleton<>).MakeGenericType(sizesType)
                .GetProperty("instance", BF).GetValue(null);
            var group = sizesType.GetProperty("currentGroup", BF).GetValue(singleton);
            var groupType = group.GetType();

            var gvsType = asm.GetType("UnityEditor.GameViewSize");
            var gvsTypeEnum = asm.GetType("UnityEditor.GameViewSizeType");
            var ctor = gvsType.GetConstructor(new[] { gvsTypeEnum, typeof(int), typeof(int), typeof(string) });
            var fixedRes = Enum.Parse(gvsTypeEnum, "FixedResolution");

            int total = (int)groupType.GetMethod("GetTotalCount", BF).Invoke(group, null);
            var getText = gvsType.GetProperty("baseText", BF);
            var getGVS = groupType.GetMethod("GetGameViewSize", BF);

            int found = -1;
            for (int i = 0; i < total; i++)
                if ((string)getText.GetValue(getGVS.Invoke(group, new object[] { i })) == label) { found = i; break; }

            object size;
            if (found < 0)
            {
                size = ctor.Invoke(new[] { fixedRes, width, height, label });
                groupType.GetMethod("AddCustomSize", BF).Invoke(group, new[] { size });
                found = (int)groupType.GetMethod("GetTotalCount", BF).Invoke(group, null) - 1;
            }
            else size = getGVS.Invoke(group, new object[] { found });

            var gameViewType = asm.GetType("UnityEditor.GameView");
            var gv = EditorWindow.GetWindow(gameViewType, false, "Game", true);
            gameViewType.GetMethod("SizeSelectionCallback", BF).Invoke(gv, new object[] { found, size });
            gv.Repaint();
            Debug.Log($"Echo Forge QA: Game View -> {label} ({width}x{height})");
        }
    }
}
