using System.Collections.Generic;
using UnityEngine;
using EchoForge.Sim;

namespace EchoForge.Level
{
    /// <summary>Data-driven definition of one Echo Forge stage (0-12). No gameplay code
    /// branches on levelId; everything a chamber needs lives here.</summary>
    [CreateAssetMenu(menuName = "Echo Forge/Level Definition", fileName = "Level_00")]
    public sealed class EchoLevelDefinition : ScriptableObject
    {
        [Header("Identity")]
        public int levelId;
        public string title = "Untitled Chamber";
        [Tooltip("A / B / C, or empty for Sound Check")]
        public string series = "";
        public bool isTutorial;
        [TextArea] public string tutorialText = "";
        public int visualVariant;

        [Header("Chamber")]
        public Vector2Int chamberSize = new Vector2Int(9, 7);

        [Header("Sources")]
        public List<EmitterDef> emitters = new();

        [Header("Components")]
        [Tooltip("Fixed geometry the player cannot touch (walls, fixed reflectors, receivers, resonators).")]
        public List<ComponentDef> fixedComponents = new();
        [Tooltip("Player-tunable components in their starting state.")]
        public List<ComponentDef> adjustables = new();
        [Tooltip("Cells a movable component may be dropped into.")]
        public List<Vector2Int> movableSockets = new();

        [Header("Win conditions")]
        public List<TimingConstraint> timing = new();

        [Header("Scoring")]
        public int adjustmentPar = 3;
        public int optimalAdjustments = 1;
        [Tooltip("The optimal chamber configuration. Validator confirms it solves the level.")]
        public List<ComponentDef> optimalComponents = new();

        public AcousticGraph BuildGraph(IEnumerable<ComponentDef> currentAdjustables)
        {
            var g = new AcousticGraph(chamberSize.x, chamberSize.y);
            g.Emitters.AddRange(emitters);
            g.Timing.AddRange(timing);
            foreach (var c in fixedComponents) g.Add(c);
            foreach (var c in currentAdjustables) g.Add(c);
            return g;
        }

        public AcousticGraph BuildInitialGraph() => BuildGraph(adjustables);

        public AcousticGraph BuildOptimalGraph() =>
            BuildGraph(optimalComponents.Count > 0 ? optimalComponents : adjustables);
    }
}
