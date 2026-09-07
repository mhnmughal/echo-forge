using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace EchoForge.Game
{
    [Serializable]
    public class LevelRecord
    {
        public int id;
        public bool completed;
        public int bestRating;    // (int)EchoForge.Sim.Rating
        public int bestTunes = -1; // lower is better; -1 = none
    }

    [Serializable]
    public class SaveData
    {
        public int version = SaveSystem.CurrentVersion;
        public bool tutorialComplete;
        public int highestUnlocked;      // highest level id the player may enter (0 = only Sound Check)
        public List<LevelRecord> levels = new();
        public float musicVolume = 0.7f;
        public float effectsVolume = 0.9f;
        public bool hapticsEnabled = true;
        public bool chamberInTune;
    }

    /// <summary>Versioned local JSON save. Offline only. Never downgrades an earned rating.</summary>
    public static class SaveSystem
    {
        public const int CurrentVersion = 1;

        static string FilePath => Path.Combine(Application.persistentDataPath, "echoforge_save.json");

        static SaveData _data;
        public static SaveData Data => _data ??= Load();

        public static SaveData Load()
        {
            try
            {
                if (!File.Exists(FilePath)) return _data = new SaveData();

                var json = File.ReadAllText(FilePath);
                var d = JsonUtility.FromJson<SaveData>(json);
                if (d == null) return _data = Recover("unparseable");

                if (d.version > CurrentVersion) return _data = Recover($"newer save v{d.version}");
                if (d.version < CurrentVersion) { d.version = CurrentVersion; } // forward migration point

                d.levels ??= new List<LevelRecord>();
                return _data = d;
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[EchoForge] Save load failed ({e.Message}); starting fresh.");
                return _data = Recover(e.Message);
            }
        }

        static SaveData Recover(string reason)
        {
            try
            {
                if (File.Exists(FilePath))
                    File.Copy(FilePath, FilePath + ".bak-" + DateTime.UtcNow.Ticks, true);
            }
            catch { /* best effort */ }
            Debug.LogWarning($"[EchoForge] Save recovered from: {reason}");
            return new SaveData();
        }

        public static void Save()
        {
            try
            {
                var tmp = FilePath + ".tmp";
                File.WriteAllText(tmp, JsonUtility.ToJson(Data, true));
                if (File.Exists(FilePath)) File.Replace(tmp, FilePath, null);
                else File.Move(tmp, FilePath);
            }
            catch (Exception e)
            {
                Debug.LogError($"[EchoForge] Save write failed: {e.Message}");
            }
        }

        public static LevelRecord RecordFor(int id)
        {
            var r = Data.levels.Find(x => x.id == id);
            if (r == null) { r = new LevelRecord { id = id }; Data.levels.Add(r); }
            return r;
        }

        /// <summary>Record a completed stage. Rating never downgrades; best (lowest) tunes kept.</summary>
        public static void RecordResult(int levelId, int rating, int tunes, bool isTutorial)
        {
            if (isTutorial)
            {
                Data.tutorialComplete = true;
                if (Data.highestUnlocked < 1) Data.highestUnlocked = 1;
                Save();
                return;
            }

            var r = RecordFor(levelId);
            r.completed = true;
            if (rating > r.bestRating) r.bestRating = rating;
            if (tunes >= 0 && (r.bestTunes < 0 || tunes < r.bestTunes)) r.bestTunes = tunes;

            int next = levelId + 1;
            if (next <= 12 && Data.highestUnlocked < next) Data.highestUnlocked = next;
            if (levelId == 12) Data.chamberInTune = true;

            Save();
        }

        public static bool IsUnlocked(int levelId)
        {
            if (levelId <= 0) return true;              // Sound Check always available
            if (!Data.tutorialComplete) return false;   // gate campaign behind tutorial
            return levelId <= Mathf.Max(1, Data.highestUnlocked);
        }

        /// <summary>Reset progression + ratings. Keeps audio/haptic settings. UI must confirm first.</summary>
        public static void ResetProgress()
        {
            var d = new SaveData
            {
                musicVolume = Data.musicVolume,
                effectsVolume = Data.effectsVolume,
                hapticsEnabled = Data.hapticsEnabled,
            };
            _data = d;
            Save();
        }
    }
}
