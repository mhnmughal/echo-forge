using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using EchoForge.Chamber;
using EchoForge.Game;

namespace EchoForge.PlayTests
{
    /// <summary>Automated controller QA over the real EchoChamber scene: drives select ->
    /// rotate -> PULSE through LevelController and asserts the deterministic outcome,
    /// progression save, and that input never stays locked. This is NOT human touch QA.</summary>
    public class ChamberFlowTests
    {
        const BindingFlags F = BindingFlags.NonPublic | BindingFlags.Instance;

        static object Priv(object o, string field) => o.GetType().GetField(field, F).GetValue(o);
        static void Tap(LevelController lc, ComponentView v) =>
            typeof(LevelController).GetMethod("OnComponentTapped", F).Invoke(lc, new object[] { v });

        [SetUp]
        public void Reset() => SaveSystem.ResetProgress();

        IEnumerator LoadChamber(int levelId)
        {
            GameSession.CurrentLevelId = levelId;
            yield return SceneManager.LoadSceneAsync("EchoChamber");
            yield return null;
            yield return null;
        }

        static LevelController Controller() => Object.FindFirstObjectByType<LevelController>();

        IEnumerator WaitUntilNotBusy(LevelController lc, float timeout = 12f)
        {
            float t = 0f;
            while ((bool)Priv(lc, "_busy") && t < timeout) { t += Time.unscaledDeltaTime; yield return null; }
        }

        [UnityTest]
        public IEnumerator Level0_RotateThenPulse_Solves_And_UnlocksLevel1()
        {
            yield return LoadChamber(0);
            var lc = Controller();
            Assert.IsNotNull(lc, "LevelController missing from EchoChamber");

            var reflector = GameObject.Find("Reflector_5_3").GetComponent<ComponentView>();
            Tap(lc, reflector);  // select
            Tap(lc, reflector);  // rotate 3 -> 0 ("/")

            var chamber = Priv(lc, "_chamber");
            Assert.AreEqual(1, (int)chamber.GetType().GetProperty("Tunes").GetValue(chamber));

            lc.OnPulsePressed();
            yield return WaitUntilNotBusy(lc);

            Assert.IsTrue((bool)Priv(lc, "_solved"), "Level 0 should be solved after one correct rotation");
            Assert.IsTrue(SaveSystem.Data.tutorialComplete, "tutorial completion must persist");
            Assert.GreaterOrEqual(SaveSystem.Data.highestUnlocked, 1, "Level 1 should unlock");
        }

        [UnityTest]
        public IEnumerator Level0_PulseWithoutTuning_Fails_ThenRetuneReenablesInput()
        {
            yield return LoadChamber(0);
            var lc = Controller();

            lc.OnPulsePressed();
            yield return WaitUntilNotBusy(lc);

            Assert.IsFalse((bool)Priv(lc, "_solved"), "untuned pulse must not solve");
            var resultLayer = GameObject.Find("ResultLayer");
            Assert.IsTrue(resultLayer != null && resultLayer.activeInHierarchy, "failure result should show");

            lc.OnRetunePressed();
            var input = (ChamberInput)Priv(lc, "input");
            Assert.IsTrue(input.InputEnabled, "Retune must re-enable input - never leave the game stuck");
        }

        [UnityTest]
        public IEnumerator Level1_FirstEcho_Solvable_ViaSingleRotation()
        {
            yield return LoadChamber(1);
            var lc = Controller();

            var reflector = GameObject.Find("Reflector_6_1").GetComponent<ComponentView>();
            Tap(lc, reflector);  // select
            Tap(lc, reflector);  // rotate 3 -> 0

            lc.OnPulsePressed();
            yield return WaitUntilNotBusy(lc);

            Assert.IsTrue((bool)Priv(lc, "_solved"), "Level 1 optimal solution should solve in play");
            Assert.GreaterOrEqual(SaveSystem.Data.highestUnlocked, 2, "Level 2 should unlock");
        }

        [UnityTest]
        public IEnumerator Pause_Then_Resume_RestoresTimeAndInput()
        {
            yield return LoadChamber(1);
            var lc = Controller();

            lc.OnPausePressed();
            Assert.AreEqual(0f, Time.timeScale);
            var pauseLayer = GameObject.Find("PauseLayer");
            Assert.IsTrue(pauseLayer != null && pauseLayer.activeInHierarchy);

            lc.OnResumePressed();
            Assert.AreEqual(1f, Time.timeScale);
            var input = (ChamberInput)Priv(lc, "input");
            Assert.IsTrue(input.InputEnabled);
        }
    }
}
