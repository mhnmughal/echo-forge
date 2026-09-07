using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using EchoForge.Sim;
using EchoForge.Level;

namespace EchoForge.EditorTools
{
    /// <summary>Editor-side validation of every EchoLevelDefinition asset.
    /// Target state: 13 / 13 VALID. Errors name the exact level and reason.</summary>
    public static class LevelValidator
    {
        [MenuItem("Echo Forge/Validate Levels %#v")]
        public static void ValidateMenu()
        {
            var report = Validate(out int valid, out int total);
            Debug.Log(report);
            if (valid == total && total == 13)
                Debug.Log($"<b>Echo Forge: {valid}/{total} VALID</b>");
            else
                Debug.LogWarning($"Echo Forge: {valid}/{total} valid ({total} level assets found; expect 13).");
        }

        public static string Validate(out int validCount, out int total)
        {
            var defs = AssetDatabase.FindAssets("t:EchoLevelDefinition")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<EchoLevelDefinition>)
                .Where(d => d != null)
                .OrderBy(d => d.levelId)
                .ToList();

            total = defs.Count;
            validCount = 0;
            var sb = new StringBuilder("ECHO FORGE LEVEL VALIDATION\n");

            var seenIds = new HashSet<int>();
            foreach (var d in defs)
            {
                var errs = ValidateOne(d, seenIds);
                if (errs.Count == 0)
                {
                    validCount++;
                    sb.AppendLine($"  [VALID]   L{d.levelId:00} {d.title}");
                }
                else
                {
                    sb.AppendLine($"  [INVALID] L{d.levelId:00} {d.title}");
                    foreach (var e in errs) sb.AppendLine($"            - {e}");
                }
            }

            for (int id = 0; id <= 12; id++)
                if (defs.All(d => d.levelId != id))
                    sb.AppendLine($"  [MISSING] no level asset with id {id:00}");

            return sb.ToString();
        }

        static List<string> ValidateOne(EchoLevelDefinition d, HashSet<int> seenIds)
        {
            var errs = new List<string>();

            if (d.levelId < 0 || d.levelId > 12) errs.Add($"levelId {d.levelId} outside 0..12");
            if (!seenIds.Add(d.levelId)) errs.Add($"duplicate levelId {d.levelId}");
            if (string.IsNullOrWhiteSpace(d.title)) errs.Add("missing title");
            if (d.chamberSize.x < 2 || d.chamberSize.y < 2) errs.Add($"chamber too small {d.chamberSize}");
            if (d.emitters.Count == 0) errs.Add("no emitters");
            if ((d.levelId == 0) != d.isTutorial) errs.Add("isTutorial must be true iff levelId == 0");
            if (!d.isTutorial && d.optimalAdjustments > d.adjustmentPar)
                errs.Add($"optimalAdjustments ({d.optimalAdjustments}) > adjustmentPar ({d.adjustmentPar})");

            bool hasTarget = d.fixedComponents.Concat(d.adjustables).Concat(d.optimalComponents)
                .Any(c => c.type == CompType.Receiver || c.type == CompType.Resonator);
            if (!hasTarget) errs.Add("no receiver or resonator anywhere");

            foreach (var c in d.emitters)
                if (!InBounds(d, c.cell)) errs.Add($"emitter out of bounds at {c.cell}");
            foreach (var c in d.fixedComponents.Concat(d.adjustables))
                if (!InBounds(d, c.cell)) errs.Add($"{c.type} out of bounds at {c.cell}");
            foreach (var s in d.movableSockets)
                if (!InBounds(d, s)) errs.Add($"socket out of bounds at {s}");

            // simulation: optimal config must solve; must not run away
            var opt = PulseSimulator.Run(d.BuildOptimalGraph());
            if (opt.EndedByCap) errs.Add("optimal config simulation hit the step cap (possible loop)");
            if (!opt.Success) errs.Add($"optimal config does NOT solve (fail: {opt.Fail})");

            var init = PulseSimulator.Run(d.BuildInitialGraph());
            if (init.EndedByCap) errs.Add("initial config simulation hit the step cap (possible loop)");
            if (!d.isTutorial && init.Success && d.optimalAdjustments > 0)
                errs.Add("initial config already solves with 0 tunes (no puzzle)");

            return errs;
        }

        static bool InBounds(EchoLevelDefinition d, Vector2Int c) =>
            c.x >= 0 && c.y >= 0 && c.x < d.chamberSize.x && c.y < d.chamberSize.y;
    }
}
