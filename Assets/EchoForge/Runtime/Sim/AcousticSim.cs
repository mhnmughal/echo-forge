using System;
using System.Collections.Generic;
using UnityEngine;

namespace EchoForge.Sim
{
    /// <summary>8-way pulse travel direction. Ordered clockwise from North.</summary>
    public enum Dir { N = 0, NE = 1, E = 2, SE = 3, S = 4, SW = 5, W = 6, NW = 7 }

    public enum CompType { Emitter, Reflector, Absorber, Splitter, DelayPlate, Receiver, Resonator, Wall }

    /// <summary>How a resonator's multi-pulse requirement is checked.</summary>
    public enum ResonatorRule
    {
        HitCount,     // needs >= param pulses
        BeatDelta,    // needs exactly two hits whose beat difference == param
        Simultaneous  // needs two hits on the same beat (BeatDelta 0, kept explicit for clarity)
    }

    public enum FailReason { None, ReceiverMissed, ForbiddenReceiver, TimingIncorrect, ResonanceIncomplete }

    public enum Rating { None, Resolved, CleanEcho, PureResonance }

    public static class Directions
    {
        static readonly Vector2Int[] Steps =
        {
            new Vector2Int(0, 1),   // N
            new Vector2Int(1, 1),   // NE
            new Vector2Int(1, 0),   // E
            new Vector2Int(1, -1),  // SE
            new Vector2Int(0, -1),  // S
            new Vector2Int(-1, -1), // SW
            new Vector2Int(-1, 0),  // W
            new Vector2Int(-1, 1),  // NW
        };

        public static Vector2Int Step(Dir d) => Steps[(int)d];

        public static Dir FromVector(Vector2Int v)
        {
            for (int i = 0; i < 8; i++)
                if (Steps[i] == v) return (Dir)i;
            throw new ArgumentException($"Not a unit direction: {v}");
        }

        /// <summary>Reflect a travel direction off a flat panel at the given 45deg orientation state.
        /// orient: 0 = "/" (45deg), 1 = "\" (135deg), 2 = "|" (vertical), 3 = "-" (horizontal).
        /// Returns null when the pulse travels parallel to the panel (passes straight through).</summary>
        public static Dir? Reflect(Dir d, int orient)
        {
            Vector2Int v = Step(d);
            Vector2Int r;
            switch (((orient % 4) + 4) % 4)
            {
                case 0: r = new Vector2Int(v.y, v.x); break;    // mirror across y = x
                case 1: r = new Vector2Int(-v.y, -v.x); break;  // mirror across y = -x
                case 2: r = new Vector2Int(-v.x, v.y); break;   // vertical panel: flip x
                default: r = new Vector2Int(v.x, -v.y); break;  // horizontal panel: flip y
            }
            if (r == v) return null;          // parallel to panel -> pass through
            return FromVector(r);
        }

        /// <summary>Splitter fork: incoming direction yields the two 45deg-adjacent directions.</summary>
        public static (Dir, Dir) Fork(Dir d)
        {
            int i = (int)d;
            return ((Dir)(((i + 7) % 8)), (Dir)((i + 1) % 8));
        }
    }

    [Serializable]
    public struct EmitterDef
    {
        public Vector2Int cell;
        public Dir dir;
        public int startBeat;
    }

    [Serializable]
    public struct ComponentDef
    {
        public CompType type;
        public Vector2Int cell;
        public int orient;        // reflector/splitter orientation state
        public int delayBeats;    // DelayPlate
        public int receiverId;    // Receiver/Resonator identity (>=0)
        public bool forbidden;    // Receiver only
        public ResonatorRule rule;
        public int ruleParam;
    }

    [Serializable]
    public struct TimingConstraint
    {
        public int fromReceiverId;
        public int toReceiverId;
        public int minOffset; // beat(to) - beat(from) must be within [min,max]
        public int maxOffset;
    }

    /// <summary>Immutable snapshot of a chamber: bounds + component at each cell + win conditions.
    /// Built from a level definition merged with the player's current adjustments.</summary>
    public sealed class AcousticGraph
    {
        public readonly int Width, Height;
        public readonly List<EmitterDef> Emitters = new();
        public readonly Dictionary<Vector2Int, ComponentDef> Cells = new();
        public readonly List<TimingConstraint> Timing = new();

        public AcousticGraph(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public bool InBounds(Vector2Int c) => c.x >= 0 && c.y >= 0 && c.x < Width && c.y < Height;

        public bool TryGet(Vector2Int c, out ComponentDef comp) => Cells.TryGetValue(c, out comp);

        public void Add(ComponentDef c) => Cells[c.cell] = c;
    }

    public struct ReceiverHit
    {
        public int receiverId;
        public int beat;
        public bool forbidden;
    }

    public sealed class SimResult
    {
        public bool Success;
        public FailReason Fail = FailReason.None;
        public readonly List<ReceiverHit> Hits = new();
        public readonly List<int> AbsorbedAt = new();
        public int TotalBeats;
        public bool EndedByCap;
        public int PulsesSpawned;

        public bool ReceiverWasHit(int id)
        {
            foreach (var h in Hits) if (h.receiverId == id) return true;
            return false;
        }

        public IEnumerable<int> BeatsFor(int id)
        {
            foreach (var h in Hits) if (h.receiverId == id) yield return h.beat;
        }
    }

    /// <summary>Deterministic discrete-event pulse simulation. No randomness, no Unity physics.
    /// Same graph in -> same result out, always.</summary>
    public static class PulseSimulator
    {
        public const int MaxSteps = 5000;
        // ponytail: per-(cell,dir) cap is just an early-out heuristic; MaxSteps is the real
        // loop guard. Raise if a wide splitter network legitimately reuses a lane many times.
        const int MaxRevisits = 8;

        struct Pulse
        {
            public Vector2Int cell;
            public Dir dir;
            public int beat;
            public int branch;
        }

        public static SimResult Run(AcousticGraph g)
        {
            var result = new SimResult();
            if (g == null) return result;

            var pending = new List<Pulse>();
            int branchSeed = 0;
            foreach (var e in g.Emitters)
            {
                pending.Add(new Pulse { cell = e.cell, dir = e.dir, beat = Mathf.Max(0, e.startBeat), branch = branchSeed++ });
                result.PulsesSpawned++;
            }

            var revisits = new Dictionary<(Vector2Int, Dir), int>();
            int steps = 0;
            int nextBranch = branchSeed;

            while (pending.Count > 0)
            {
                if (++steps > MaxSteps) { result.EndedByCap = true; break; }

                // pop lowest beat, stable tiebreak on branch then dir -> deterministic
                int pick = 0;
                for (int i = 1; i < pending.Count; i++)
                {
                    var a = pending[i]; var b = pending[pick];
                    if (a.beat < b.beat ||
                        (a.beat == b.beat && a.branch < b.branch) ||
                        (a.beat == b.beat && a.branch == b.branch && (int)a.dir < (int)b.dir))
                        pick = i;
                }
                Pulse p = pending[pick];
                pending.RemoveAt(pick);

                var key = (p.cell, p.dir);
                revisits.TryGetValue(key, out int seen);
                if (seen >= MaxRevisits) { result.EndedByCap = true; continue; }
                revisits[key] = seen + 1;

                if (p.beat > result.TotalBeats) result.TotalBeats = p.beat;

                if (!g.InBounds(p.cell)) continue; // left the chamber -> gone

                if (g.TryGet(p.cell, out ComponentDef comp))
                {
                    switch (comp.type)
                    {
                        case CompType.Wall:
                        case CompType.Absorber:
                            if (comp.type == CompType.Absorber) result.AbsorbedAt.Add(comp.receiverId);
                            continue;

                        case CompType.Receiver:
                            result.Hits.Add(new ReceiverHit { receiverId = comp.receiverId, beat = p.beat, forbidden = comp.forbidden });
                            if (comp.forbidden) result.Fail = FailReason.ForbiddenReceiver;
                            continue;

                        case CompType.Resonator:
                            result.Hits.Add(new ReceiverHit { receiverId = comp.receiverId, beat = p.beat, forbidden = false });
                            continue;

                        case CompType.Reflector:
                        {
                            var nd = Directions.Reflect(p.dir, comp.orient);
                            if (nd.HasValue) p.dir = nd.Value;
                            break;
                        }

                        case CompType.Splitter:
                        {
                            var (a, b) = Directions.Fork(p.dir);
                            pending.Add(new Pulse { cell = p.cell + Directions.Step(a), dir = a, beat = p.beat + 1, branch = nextBranch++ });
                            pending.Add(new Pulse { cell = p.cell + Directions.Step(b), dir = b, beat = p.beat + 1, branch = nextBranch++ });
                            result.PulsesSpawned += 2;
                            continue;
                        }

                        case CompType.DelayPlate:
                        {
                            int hold = Mathf.Max(0, comp.delayBeats);
                            pending.Add(new Pulse { cell = p.cell + Directions.Step(p.dir), dir = p.dir, beat = p.beat + 1 + hold, branch = p.branch });
                            continue;
                        }

                        case CompType.Emitter:
                            break; // pulse passes through an emitter cell
                    }
                }

                pending.Add(new Pulse { cell = p.cell + Directions.Step(p.dir), dir = p.dir, beat = p.beat + 1, branch = p.branch });
            }

            Evaluate(g, result);
            return result;
        }

        static void Evaluate(AcousticGraph g, SimResult r)
        {
            // forbidden already flags on hit
            if (r.Fail == FailReason.ForbiddenReceiver) { r.Success = false; return; }

            // every non-forbidden receiver must be hit
            foreach (var kv in g.Cells)
            {
                var c = kv.Value;
                if (c.type == CompType.Receiver && !c.forbidden && !r.ReceiverWasHit(c.receiverId))
                {
                    r.Fail = FailReason.ReceiverMissed;
                    r.Success = false;
                    return;
                }
            }

            // timing constraints
            foreach (var t in g.Timing)
            {
                if (!TryFirstBeat(r, t.fromReceiverId, out int bf) || !TryFirstBeat(r, t.toReceiverId, out int bt))
                {
                    r.Fail = FailReason.TimingIncorrect; r.Success = false; return;
                }
                int off = bt - bf;
                if (off < t.minOffset || off > t.maxOffset)
                {
                    r.Fail = FailReason.TimingIncorrect; r.Success = false; return;
                }
            }

            // resonators
            foreach (var kv in g.Cells)
            {
                var c = kv.Value;
                if (c.type != CompType.Resonator) continue;
                if (!ResonatorSatisfied(r, c))
                {
                    r.Fail = FailReason.ResonanceIncomplete; r.Success = false; return;
                }
            }

            r.Fail = FailReason.None;
            r.Success = true;
        }

        static bool TryFirstBeat(SimResult r, int id, out int beat)
        {
            beat = int.MaxValue;
            bool any = false;
            foreach (var h in r.Hits)
                if (h.receiverId == id) { any = true; if (h.beat < beat) beat = h.beat; }
            return any;
        }

        static bool ResonatorSatisfied(SimResult r, ComponentDef c)
        {
            var beats = new List<int>();
            foreach (var h in r.Hits) if (h.receiverId == c.receiverId) beats.Add(h.beat);

            switch (c.rule)
            {
                case ResonatorRule.HitCount:
                    return beats.Count >= Mathf.Max(1, c.ruleParam);

                case ResonatorRule.Simultaneous:
                    return HasPairWithDelta(beats, 0);

                case ResonatorRule.BeatDelta:
                    return HasPairWithDelta(beats, Mathf.Max(0, c.ruleParam));
            }
            return false;
        }

        static bool HasPairWithDelta(List<int> beats, int delta)
        {
            for (int i = 0; i < beats.Count; i++)
                for (int j = i + 1; j < beats.Count; j++)
                    if (Mathf.Abs(beats[i] - beats[j]) == delta) return true;
            return false;
        }
    }

    public static class RatingEvaluator
    {
        /// <summary>Rate a solved level. adjustments = player TUNES used.
        /// par -> CleanEcho, optimal -> PureResonance. Level 0 (tutorial) passes None.</summary>
        public static Rating Evaluate(bool solved, int adjustments, int par, int optimal, bool tutorial)
        {
            if (!solved) return Rating.None;
            if (tutorial) return Rating.None;
            if (adjustments <= optimal) return Rating.PureResonance;
            if (adjustments <= par) return Rating.CleanEcho;
            return Rating.Resolved;
        }
    }
}
