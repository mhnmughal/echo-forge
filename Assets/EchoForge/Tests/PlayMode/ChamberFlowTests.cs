using System.Collections;
using System.Reflection;
using System.IO;
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

        byte[] _savedProgress;
        string SavePath => Path.Combine(Application.persistentDataPath, "echoforge_save.json");

        [OneTimeSetUp]
        public void PreserveProgress() => _savedProgress = File.Exists(SavePath) ? File.ReadAllBytes(SavePath) : null;

        [OneTimeTearDown]
        public void RestoreProgress()
        {
            Time.timeScale = 1f;
            if (_savedProgress == null) File.Delete(SavePath);
            else File.WriteAllBytes(SavePath, _savedProgress);
            SaveSystem.Load();
        }

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
        public IEnumerator EveryLevel_OptimalConfig_SolvesThroughTheChamberScene(
            [Values(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12)] int levelId)
        {
            yield return LoadChamber(levelId);
            var lc = Controller();
            Assert.IsNotNull(lc, $"L{levelId:00}: no LevelController");

            var chamber = Priv(lc, "_chamber");
            var def = (EchoForge.Level.EchoLevelDefinition)
                chamber.GetType().GetProperty("Definition").GetValue(chamber);

            var runtime = (EchoForge.Level.ChamberRuntime)chamber;
            var views = (System.Collections.Generic.List<ComponentView>)Priv(lc, "_adjustableViews");
            for (int i = 0; i < def.optimalComponents.Count; i++)
            {
                var goal = def.optimalComponents[i];
                Tap(lc, views[i]);
                if (runtime.Adjustables[i].cell != goal.cell)
                {
                    var socket = Object.FindObjectsByType<SocketMarker>(FindObjectsSortMode.None);
                    Assert.IsTrue(System.Array.Exists(socket, s => s.Cell == goal.cell), "Movable component must expose its destination socket");
                    typeof(LevelController).GetMethod("OnSocketTapped", F).Invoke(lc, new object[] { goal.cell });
                    Assert.AreEqual(goal.cell, views[i].Cell, "Moved view must follow logical position");
                }
                int turns = 0;
                while (runtime.Adjustables[i].orient != goal.orient && turns++ < 4) Tap(lc, views[i]);
                Assert.AreEqual(goal.orient, runtime.Adjustables[i].orient);
            }

            lc.OnPulsePressed();
            yield return WaitUntilNotBusy(lc, 18f);

            Assert.IsTrue((bool)Priv(lc, "_solved"),
                $"L{levelId:00} '{def.title}': optimal config did not solve through the scene pipeline");
            lc.OnRetunePressed();
            lc.OnPulsePressed();
            Assert.IsTrue((bool)Priv(lc, "_busy"), "Retune after success must allow another pulse");
            yield return WaitUntilNotBusy(lc, 18f);
            Assert.IsTrue((bool)Priv(lc, "_solved"));
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

        [UnityTest]
        public IEnumerator PauseDuringPulse_StopsPlayback_AndResumeKeepsManipulationLocked()
        {
            yield return LoadChamber(11);
            var lc = Controller();
            lc.OnPulsePressed();
            lc.OnPausePressed();
            Assert.AreEqual(0f, Time.timeScale);
            var input = (ChamberInput)Priv(lc, "input");
            Assert.IsFalse(input.InputEnabled);
            yield return new WaitForSecondsRealtime(0.1f);
            Assert.IsTrue((bool)Priv(lc, "_busy"));
            lc.OnResumePressed();
            Assert.IsFalse(input.InputEnabled, "Resuming a pulse must not unlock component manipulation");
            yield return WaitUntilNotBusy(lc, 18f);
            lc.OnRetunePressed();
            Assert.IsTrue(input.InputEnabled);
        }

        [UnityTest]
        public IEnumerator AdvancedBoards_StayAboveActuator_AndEmitterFacesItsLogicalDirection()
        {
            foreach (int id in new[] { 11, 12 })
            {
                yield return LoadChamber(id);
                var lc = Controller();
                var runtime = (EchoForge.Level.ChamberRuntime)Priv(lc, "_chamber");
                foreach (var view in Object.FindObjectsByType<ComponentView>(FindObjectsSortMode.None))
                {
                    var p = Camera.main.WorldToViewportPoint(view.transform.position);
                    Assert.That(p.x, Is.InRange(0.07f, 0.93f));
                    Assert.That(p.y, Is.InRange(0.24f, 0.9f), "Component must not be hidden under PULSE");
                    if (view.Type != EchoForge.Sim.CompType.Emitter) continue;
                    var source = runtime.Definition.emitters.Find(e => e.cell == view.Cell);
                    var forward = view.transform.Find("Spinner").forward;
                    var step = EchoForge.Sim.Directions.Step(source.dir);
                    Assert.Greater(Vector3.Dot(forward, new Vector3(step.x, 0, step.y).normalized), 0.99f);
                }
            }
        }
    }
}
