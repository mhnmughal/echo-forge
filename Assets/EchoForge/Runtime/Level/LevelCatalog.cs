using System.Collections.Generic;
using UnityEngine;

namespace EchoForge.Level
{
    /// <summary>The ordered set of all 13 stage definitions. Assigned in-scene / loaded from
    /// Resources so no gameplay code hard-references individual level assets.</summary>
    [CreateAssetMenu(menuName = "Echo Forge/Level Catalog", fileName = "LevelCatalog")]
    public sealed class LevelCatalog : ScriptableObject
    {
        public List<EchoLevelDefinition> levels = new();

        public int Count => levels.Count;

        public EchoLevelDefinition ById(int id)
        {
            foreach (var l in levels) if (l != null && l.levelId == id) return l;
            return null;
        }

        public IEnumerable<EchoLevelDefinition> Campaign()
        {
            foreach (var l in levels) if (l != null && !l.isTutorial) yield return l;
        }

        static LevelCatalog _instance;
        public static LevelCatalog Instance =>
            _instance != null ? _instance : (_instance = Resources.Load<LevelCatalog>("LevelCatalog"));
    }
}
