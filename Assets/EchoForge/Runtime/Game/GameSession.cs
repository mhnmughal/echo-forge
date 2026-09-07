using UnityEngine.SceneManagement;

namespace EchoForge.Game
{
    /// <summary>Transient cross-scene state (not saved). Which chamber to load, where to go back to.</summary>
    public static class GameSession
    {
        public static int CurrentLevelId;
        public static bool CameFromDial = true;
    }

    /// <summary>Single place that knows scene names and transitions.</summary>
    public static class SceneFlow
    {
        public const string Bootstrap = "Bootstrap";
        public const string ResonanceHall = "ResonanceHall";
        public const string EchoChamber = "EchoChamber";

        public static void LoadHall() => SceneManager.LoadScene(ResonanceHall);

        public static void LoadChamber(int levelId, bool fromDial = true)
        {
            GameSession.CurrentLevelId = levelId;
            GameSession.CameFromDial = fromDial;
            SceneManager.LoadScene(EchoChamber);
        }

        public static void ReloadChamber() => SceneManager.LoadScene(EchoChamber);

        public static void NextChamber()
        {
            int next = GameSession.CurrentLevelId + 1;
            if (next <= 12 && SaveSystem.IsUnlocked(next)) LoadChamber(next);
            else LoadHall();
        }
    }
}
