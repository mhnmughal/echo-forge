using NUnit.Framework;
using UnityEngine;
using EchoForge.Sim;
using EchoForge.Level;

namespace EchoForge.Tests
{
    public class SimulatorTests
    {
        static AcousticGraph G(int w, int h) => new AcousticGraph(w, h);

        static ComponentDef Comp(CompType t, int x, int y, int orient = 0, int delay = 0,
            int id = 0, bool forbidden = false,
            ResonatorRule rule = ResonatorRule.HitCount, int ruleParam = 1) =>
            new ComponentDef
            {
                type = t, cell = new Vector2Int(x, y), orient = orient, delayBeats = delay,
                receiverId = id, forbidden = forbidden, rule = rule, ruleParam = ruleParam
            };

        static void Emit(AcousticGraph g, int x, int y, Dir d, int startBeat = 0) =>
            g.Emitters.Add(new EmitterDef { cell = new Vector2Int(x, y), dir = d, startBeat = startBeat });

        [Test]
        public void Reflector_Redirects_EastToNorth_ReachesReceiver()
        {
            var g = G(5, 5);
            Emit(g, 0, 0, Dir.E);
            g.Add(Comp(CompType.Reflector, 3, 0, orient: 0));   // "/"
            g.Add(Comp(CompType.Receiver, 3, 3, id: 1));

            var r = PulseSimulator.Run(g);

            Assert.IsTrue(r.Success, "reflected pulse should reach the receiver");
            Assert.IsTrue(r.ReceiverWasHit(1));
        }

        [Test]
        public void Absorber_StopsPulse_ReceiverMissed()
        {
            var g = G(5, 5);
            Emit(g, 0, 0, Dir.E);
            g.Add(Comp(CompType.Absorber, 3, 0, id: 42));
            g.Add(Comp(CompType.Receiver, 3, 3, id: 1));

            var r = PulseSimulator.Run(g);

            Assert.IsFalse(r.Success);
            Assert.AreEqual(FailReason.ReceiverMissed, r.Fail);
            CollectionAssert.Contains(r.AbsorbedAt, 42);
        }

        [Test]
        public void Splitter_ForksIntoTwoBranches_BothReceiversHit()
        {
            var g = G(5, 5);
            Emit(g, 2, 0, Dir.N);
            g.Add(Comp(CompType.Splitter, 2, 2));
            g.Add(Comp(CompType.Receiver, 0, 4, id: 1));
            g.Add(Comp(CompType.Receiver, 4, 4, id: 2));

            var r = PulseSimulator.Run(g);

            Assert.IsTrue(r.Success);
            Assert.AreEqual(3, r.PulsesSpawned);
            Assert.IsTrue(r.ReceiverWasHit(1) && r.ReceiverWasHit(2));
        }

        [Test]
        public void DelayPlate_AddsBeats_BeforeReceiver()
        {
            var g = G(6, 1);
            Emit(g, 0, 0, Dir.E);
            g.Add(Comp(CompType.DelayPlate, 2, 0, delay: 2));
            g.Add(Comp(CompType.Receiver, 4, 0, id: 1));

            var r = PulseSimulator.Run(g);

            Assert.IsTrue(r.Success);
            foreach (var b in r.BeatsFor(1)) Assert.AreEqual(6, b); // 4 direct + 2 delay
        }

        [Test]
        public void MultipleEmitters_TwoReceivers_BothHit()
        {
            var g = G(5, 3);
            Emit(g, 0, 0, Dir.E);
            Emit(g, 0, 2, Dir.E);
            g.Add(Comp(CompType.Receiver, 4, 0, id: 1));
            g.Add(Comp(CompType.Receiver, 4, 2, id: 2));

            var r = PulseSimulator.Run(g);

            Assert.IsTrue(r.Success);
            Assert.AreEqual(2, r.PulsesSpawned);
        }

        [Test]
        public void TimingConstraint_OneBeatLater_Passes_And_WrongOffsetFails()
        {
            AcousticGraph Build()
            {
                var g = G(5, 3);
                Emit(g, 0, 0, Dir.E);
                Emit(g, 0, 2, Dir.E);
                g.Add(Comp(CompType.DelayPlate, 2, 2, delay: 1));
                g.Add(Comp(CompType.Receiver, 4, 0, id: 1));
                g.Add(Comp(CompType.Receiver, 4, 2, id: 2));
                return g;
            }

            var ok = Build();
            ok.Timing.Add(new TimingConstraint { fromReceiverId = 1, toReceiverId = 2, minOffset = 1, maxOffset = 1 });
            Assert.IsTrue(PulseSimulator.Run(ok).Success);

            var bad = Build();
            bad.Timing.Add(new TimingConstraint { fromReceiverId = 1, toReceiverId = 2, minOffset = 2, maxOffset = 2 });
            var rb = PulseSimulator.Run(bad);
            Assert.IsFalse(rb.Success);
            Assert.AreEqual(FailReason.TimingIncorrect, rb.Fail);
        }

        [Test]
        public void ForbiddenReceiver_Triggered_FailsEvenIfOthersHit()
        {
            var g = G(6, 4);
            Emit(g, 0, 0, Dir.E);
            g.Add(Comp(CompType.Receiver, 2, 0, id: 7, forbidden: true));
            g.Add(Comp(CompType.Receiver, 5, 0, id: 1));

            var r = PulseSimulator.Run(g);

            Assert.IsFalse(r.Success);
            Assert.AreEqual(FailReason.ForbiddenReceiver, r.Fail);
        }

        [Test]
        public void Resonator_BeatDelta_SatisfiedByTwoStaggeredPulses()
        {
            var g = G(5, 7);
            Emit(g, 0, 0, Dir.E);   // -> (4,0) at beat 4
            Emit(g, 4, 6, Dir.S);   // -> (4,0) at beat 6
            g.Add(Comp(CompType.Resonator, 4, 0, id: 9, rule: ResonatorRule.BeatDelta, ruleParam: 2));

            var r = PulseSimulator.Run(g);

            Assert.IsTrue(r.Success, "beat delta of 2 should satisfy the resonator");
        }

        [Test]
        public void Resonator_HitCount_NotMet_FailsResonanceIncomplete()
        {
            var g = G(5, 3);
            Emit(g, 0, 0, Dir.E);
            g.Add(Comp(CompType.Resonator, 4, 0, id: 9, rule: ResonatorRule.HitCount, ruleParam: 2));

            var r = PulseSimulator.Run(g);

            Assert.IsFalse(r.Success);
            Assert.AreEqual(FailReason.ResonanceIncomplete, r.Fail);
        }

        [Test]
        public void LoopProtection_BouncingPulse_EndsByCap()
        {
            var g = G(5, 1);
            Emit(g, 2, 0, Dir.E);
            g.Add(Comp(CompType.Reflector, 4, 0, orient: 2)); // "|" reverses E<->W
            g.Add(Comp(CompType.Reflector, 0, 0, orient: 2));

            var r = PulseSimulator.Run(g);

            Assert.IsTrue(r.EndedByCap, "an endless bounce must be caught by loop protection");
        }

        [Test]
        public void Determinism_SameGraph_SameResult()
        {
            AcousticGraph Build()
            {
                var g = G(7, 7);
                Emit(g, 0, 0, Dir.E);
                g.Add(Comp(CompType.Splitter, 3, 0));
                g.Add(Comp(CompType.Reflector, 3, 3, orient: 1));
                g.Add(Comp(CompType.Receiver, 6, 3, id: 1));
                g.Add(Comp(CompType.Receiver, 3, 6, id: 2));
                return g;
            }

            var a = PulseSimulator.Run(Build());
            var b = PulseSimulator.Run(Build());
            Assert.AreEqual(a.Success, b.Success);
            Assert.AreEqual(a.TotalBeats, b.TotalBeats);
            Assert.AreEqual(a.Hits.Count, b.Hits.Count);
        }
    }

    public class RatingAndAdjustmentTests
    {
        [Test]
        public void Rating_Thresholds()
        {
            Assert.AreEqual(Rating.None, RatingEvaluator.Evaluate(false, 0, 3, 1, false));
            Assert.AreEqual(Rating.None, RatingEvaluator.Evaluate(true, 0, 3, 1, tutorial: true));
            Assert.AreEqual(Rating.PureResonance, RatingEvaluator.Evaluate(true, 1, 3, 1, false));
            Assert.AreEqual(Rating.CleanEcho, RatingEvaluator.Evaluate(true, 3, 3, 1, false));
            Assert.AreEqual(Rating.Resolved, RatingEvaluator.Evaluate(true, 9, 3, 1, false));
        }

        [Test]
        public void ChamberRuntime_CountsTunes_AndRotationSolves()
        {
            var def = ScriptableObject.CreateInstance<EchoLevelDefinition>();
            def.levelId = 99;
            def.chamberSize = new Vector2Int(5, 5);
            def.adjustmentPar = 2;
            def.optimalAdjustments = 1;
            def.emitters.Add(new EmitterDef { cell = new Vector2Int(0, 0), dir = Dir.E });
            def.fixedComponents.Add(new ComponentDef
            { type = CompType.Receiver, cell = new Vector2Int(3, 3), receiverId = 1 });
            def.adjustables.Add(new ComponentDef
            { type = CompType.Reflector, cell = new Vector2Int(3, 0), orient = 3 }); // starts wrong

            var chamber = new ChamberRuntime(def);
            Assert.IsFalse(chamber.Pulse().Success, "starts unsolved");

            chamber.Rotate(0, 1); // orient 3 -> 0 ("/") redirects E->N into the receiver
            var solved = chamber.Pulse();

            Assert.IsTrue(solved.Success);
            Assert.AreEqual(1, chamber.Tunes);
            Assert.AreEqual(Rating.PureResonance, chamber.Rate(true));

            Object.DestroyImmediate(def);
        }
    }
}
