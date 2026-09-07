using System.Linq;
using NUnit.Framework;
using UnityEditor;
using EchoForge.Sim;
using EchoForge.Level;

namespace EchoForge.Tests
{
    /// <summary>Regression gate over the actual shipped level assets: every EchoLevelDefinition
    /// must be internally consistent and its optimal configuration must solve via the
    /// deterministic simulator. Mirrors the Editor LevelValidator so CI catches broken data.</summary>
    public class ShippedLevelTests
    {
        static EchoLevelDefinition[] All() =>
            AssetDatabase.FindAssets("t:EchoLevelDefinition")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<EchoLevelDefinition>)
                .Where(d => d != null)
                .OrderBy(d => d.levelId)
                .ToArray();

        [Test]
        public void AtLeastOneLevelExists()
        {
            Assert.IsNotEmpty(All(), "no EchoLevelDefinition assets found");
        }

        [Test]
        public void EveryLevel_OptimalConfig_Solves()
        {
            foreach (var d in All())
            {
                var r = PulseSimulator.Run(d.BuildOptimalGraph());
                Assert.IsFalse(r.EndedByCap, $"L{d.levelId:00} '{d.title}': optimal run hit the step cap");
                Assert.IsTrue(r.Success, $"L{d.levelId:00} '{d.title}': optimal config does not solve ({r.Fail})");
            }
        }

        [Test]
        public void EveryLevel_HasUniqueIdInRange_AndTutorialFlagOnlyOnZero()
        {
            var seen = new System.Collections.Generic.HashSet<int>();
            foreach (var d in All())
            {
                Assert.IsTrue(d.levelId is >= 0 and <= 12, $"L{d.levelId} out of range");
                Assert.IsTrue(seen.Add(d.levelId), $"duplicate level id {d.levelId}");
                Assert.AreEqual(d.levelId == 0, d.isTutorial, $"L{d.levelId:00}: isTutorial must be true iff id == 0");
            }
        }

        [Test]
        public void CampaignLevels_AreNotAlreadySolved_AtStart()
        {
            foreach (var d in All())
            {
                if (d.isTutorial || d.optimalAdjustments <= 0) continue;
                var r = PulseSimulator.Run(d.BuildInitialGraph());
                Assert.IsFalse(r.Success, $"L{d.levelId:00} '{d.title}': initial config already solves with 0 tunes");
            }
        }
    }
}
