using System.Collections.Generic;
using UnityEngine;
using EchoForge.Sim;

namespace EchoForge.Level
{
    /// <summary>Holds the mutable state of the acoustic components the player can tune in the
    /// current chamber, counts adjustments ("TUNES"), and runs the deterministic simulation.
    /// Pure logic - no scene objects. Presentation layers subscribe to the result.</summary>
    public sealed class ChamberRuntime
    {
        readonly EchoLevelDefinition _def;
        readonly List<ComponentDef> _adjustables;

        public int Tunes { get; private set; }
        public IReadOnlyList<ComponentDef> Adjustables => _adjustables;
        public EchoLevelDefinition Definition => _def;

        public ChamberRuntime(EchoLevelDefinition def)
        {
            _def = def;
            _adjustables = new List<ComponentDef>(def.adjustables.Count);
            foreach (var c in def.adjustables) _adjustables.Add(c);
        }

        public bool CanRotate(int index) =>
            index >= 0 && index < _adjustables.Count &&
            (_adjustables[index].type == CompType.Reflector || _adjustables[index].type == CompType.Splitter);

        /// <summary>Rotate a reflector/splitter by 45deg steps (delta in orientation states). +1 TUNE.</summary>
        public void Rotate(int index, int steps = 1)
        {
            if (!CanRotate(index)) return;
            var c = _adjustables[index];
            c.orient = ((c.orient + steps) % 4 + 4) % 4;
            _adjustables[index] = c;
            Tunes++;
        }

        public bool CanMove(int index) =>
            index >= 0 && index < _adjustables.Count &&
            _def.movableAdjustableIndices.Contains(index) && _def.movableSockets.Count > 0;

        public bool CanMoveTo(int index, Vector2Int socket)
        {
            if (!CanMove(index)) return false;
            if (!_def.movableSockets.Contains(socket)) return false;
            foreach (var c in _adjustables)
                if (c.cell == socket) return false; // socket occupied
            return true;
        }

        /// <summary>Move a movable component to an empty valid socket. +1 TUNE.</summary>
        public void MoveTo(int index, Vector2Int socket)
        {
            if (!CanMoveTo(index, socket)) return;
            var c = _adjustables[index];
            c.cell = socket;
            _adjustables[index] = c;
            Tunes++;
        }

        public SimResult Pulse() => PulseSimulator.Run(_def.BuildGraph(_adjustables));

        public Rating Rate(bool solved) =>
            RatingEvaluator.Evaluate(solved, Tunes, _def.adjustmentPar, _def.optimalAdjustments, _def.isTutorial);
    }
}
