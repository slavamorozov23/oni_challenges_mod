using System;
using System.Collections.Generic;
using HarmonyLib;
using Klei.AI;
using Klei.CustomSettings;
using STRINGS;
using UnityEngine;
using ModStrings = SlavaMorozov.NoPollutionMod.STRINGS;

namespace SlavaMorozov.NoPollutionMod
{
    internal static class ChallengeSettings
    {
        internal const string SettingId = "NoPollutionChallenge";
        internal const string LevelNone = "None";
        internal const string LevelNoPollution = NoPollutionChallengeModule.ModuleId;
        private const string CompletionKeyPrefix = "ChallengeCompleted_v2_";
        private const string CompletionAsteroidKeyPrefix = "ChallengeCompletedAsteroid_v2_";
        private const string CompletionCycleKeyPrefix = "ChallengeCompletedCycle_v2_";
        private const string FailureKeyPrefix = "ChallengeFailed_v1_";
        private const string StartShownKeyPrefix = "ChallengeStartShown_v1_";
        private const string SaveKeyPrefix = "NoPollutionChallengeForWorld_";

        internal static ListSettingConfig Config { get; private set; }

        internal static void RegisterStrings()
        {
            LogInfo("RegisterStrings called.");
            ChallengeModuleManager.RegisterStrings();
        }

        internal static void EnsureConfig()
        {
            LogInfo("EnsureConfig called.");
            if (Config != null)
            {
                return;
            }

            var levels = new List<SettingLevel>
            {
                new SettingLevel(LevelNone, ModStrings.NO_POLLUTION_CHALLENGE.LEVELS.NONE.NAME, ModStrings.NO_POLLUTION_CHALLENGE.LEVELS.NONE.TOOLTIP, 0L, null)
            };

            long coordinate = 1L;
            foreach (var module in ChallengeModuleManager.GetModules())
            {
                levels.Add(new SettingLevel(module.Id, module.Name, module.Tooltip, coordinate, null));
                coordinate++;
            }

            Config = new ListSettingConfig(
                SettingId,
                ModStrings.NO_POLLUTION_CHALLENGE.NAME,
                ModStrings.NO_POLLUTION_CHALLENGE.TOOLTIP,
                levels,
                LevelNone,
                LevelNone,
                2L,
                false,
                false,
                null,
                "",
                true
            );
        }

        internal static bool IsChallengeActive()
        {
            return GetCurrentChallengeLevelId() != LevelNone;
        }

        internal static string GetChallengeName(string levelId)
        {
            if (levelId == LevelNone)
            {
                return ModStrings.NO_POLLUTION_CHALLENGE.LEVELS.NONE.NAME;
            }

            if (ChallengeModuleManager.TryGetModule(levelId, out var module))
            {
                return module.Name;
            }

            return levelId;
        }

        internal static string GetCurrentChallengeLevelId()
        {
            var settings = CustomGameSettings.Instance;
            if (settings == null)
            {
                return LevelNone;
            }

            try
            {
                var level = settings.GetCurrentQualitySetting(SettingId);
                var levelId = level != null ? level.id : LevelNone;
                return levelId;
            }
            catch
            {
                return LevelNone;
            }
        }

        internal static void StoreChallengeForCurrentSave()
        {
            var loader = SaveLoader.Instance;
            if (loader == null)
            {
                LogInfo("StoreChallengeForCurrentSave: SaveLoader.Instance is null.");
                return;
            }

            var levelId = GetCurrentChallengeLevelId();
            LogInfo($"StoreChallengeForCurrentSave: {levelId}");
            StoreChallengeForGameInfo(loader.GameInfo, levelId);
        }

        internal static void StoreChallengeForGameInfo(SaveGame.GameInfo info, string levelId)
        {
            var key = GetChallengeKey(info);
            KPlayerPrefs.SetString(key, levelId ?? LevelNone);
            KPlayerPrefs.Save();
            LogInfo($"StoreChallengeForGameInfo: {key} -> {levelId}");
        }

        internal static bool TryGetChallengeForGameInfo(SaveGame.GameInfo info, out string levelId)
        {
            var key = GetChallengeKey(info);
            if (!KPlayerPrefs.HasKey(key))
            {
                levelId = LevelNone;
                LogInfo($"TryGetChallengeForGameInfo: missing key {key}");
                return false;
            }

            levelId = KPlayerPrefs.GetString(key, LevelNone);
            LogInfo($"TryGetChallengeForGameInfo: {key} -> {levelId}");
            return true;
        }

        internal static void ApplyChallengeForGameInfo(SaveGame.GameInfo info)
        {
            if (!TryGetChallengeForGameInfo(info, out var levelId))
            {
                LogInfo("ApplyChallengeForGameInfo: no stored challenge.");
                return;
            }

            EnsureConfig();
            var settings = CustomGameSettings.Instance;
            if (settings == null)
            {
                LogInfo("ApplyChallengeForGameInfo: settings is null.");
                return;
            }

            settings.SetQualitySetting(Config, levelId, false);
            LogInfo($"ApplyChallengeForGameInfo: applied {levelId}");
        }

        internal static bool IsBlockedBuilding(BuildingDef def)
        {
            if (def == null)
            {
                return false;
            }

            var challengeId = GetCurrentChallengeLevelId();
            if (!ChallengeModuleManager.TryGetModule(challengeId, out var module))
            {
                return false;
            }

            return module.IsBuildingBlocked(def);
        }

        internal static bool IsVictoryAchieved()
        {
            if (SaveGame.Instance == null)
            {
                return false;
            }

            var tracker = SaveGame.Instance.ColonyAchievementTracker;
            if (tracker == null)
            {
                return false;
            }

            var achievements = Db.Get().ColonyAchievements;
            return (achievements.Thriving != null && tracker.IsAchievementUnlocked(achievements.Thriving)) ||
                   (achievements.ReachedDistantPlanet != null && tracker.IsAchievementUnlocked(achievements.ReachedDistantPlanet)) ||
                   (achievements.CollectedArtifacts != null && tracker.IsAchievementUnlocked(achievements.CollectedArtifacts)) ||
                   (achievements.ActivateGeothermalPlant != null && tracker.IsAchievementUnlocked(achievements.ActivateGeothermalPlant)) ||
                   (achievements.AsteroidDestroyed != null && tracker.IsAchievementUnlocked(achievements.AsteroidDestroyed));
        }

        internal static void MarkChallengeCompleted(string challengeId)
        {
            if (string.IsNullOrEmpty(challengeId) || challengeId == LevelNone)
            {
                return;
            }

            var key = GetCompletionKey(challengeId);
            KPlayerPrefs.SetInt(key, 1);
            var asteroidName = GetCurrentAsteroidName();
            if (!string.IsNullOrEmpty(asteroidName))
            {
                KPlayerPrefs.SetString(GetCompletionAsteroidKey(challengeId), asteroidName);
            }
            KPlayerPrefs.SetInt(GetCompletionCycleKey(challengeId), GameUtil.GetCurrentCycle());
            KPlayerPrefs.Save();
        }

        internal static bool IsChallengeCompleted(string challengeId)
        {
            if (string.IsNullOrEmpty(challengeId) || challengeId == LevelNone)
            {
                return false;
            }

            return KPlayerPrefs.GetInt(GetCompletionKey(challengeId), 0) == 1;
        }

        internal static string GetChallengeTooltip(string challengeId)
        {
            var name = GetChallengeName(challengeId);
            if (!IsChallengeCompleted(challengeId))
            {
                return name;
            }

            var asteroidName = KPlayerPrefs.GetString(GetCompletionAsteroidKey(challengeId), string.Empty);
            var cycle = KPlayerPrefs.GetInt(GetCompletionCycleKey(challengeId), 0);
            var asteroidLine = string.IsNullOrEmpty(asteroidName)
                ? ModStrings.NO_POLLUTION_CHALLENGE.UNKNOWN_ASTEROID.ToString()
                : asteroidName;
            var cycleLine = cycle > 0
                ? string.Format(ModStrings.NO_POLLUTION_CHALLENGE.COMPLETION_CYCLE.ToString(), cycle)
                : ModStrings.NO_POLLUTION_CHALLENGE.COMPLETION_CYCLE_UNKNOWN.ToString();
            return $"{name}\n{asteroidLine}\n{cycleLine}";
        }

        internal static string GetCompletionText(string challengeId)
        {
            if (string.IsNullOrEmpty(challengeId) || challengeId == LevelNone)
            {
                return string.Empty;
            }

            if (ChallengeModuleManager.TryGetModule(challengeId, out var module))
            {
                return module.CompletionText;
            }

            var name = GetChallengeName(challengeId);
            return string.Format(ModStrings.NO_POLLUTION_CHALLENGE.COMPLETION_FALLBACK, name);
        }

        internal static string GetStartText(string challengeId)
        {
            if (string.IsNullOrEmpty(challengeId) || challengeId == LevelNone)
            {
                return string.Empty;
            }

            if (ChallengeModuleManager.TryGetModule(challengeId, out var module))
            {
                return module.StartText;
            }

            return string.Empty;
        }

        internal static string GetFailureText(string challengeId)
        {
            if (string.IsNullOrEmpty(challengeId) || challengeId == LevelNone)
            {
                return string.Empty;
            }

            if (ChallengeModuleManager.TryGetModule(challengeId, out var module))
            {
                return module.FailureText;
            }

            return string.Empty;
        }

        internal static bool HasSeenStart(string challengeId)
        {
            if (string.IsNullOrEmpty(challengeId) || challengeId == LevelNone)
            {
                return true;
            }

            var key = GetStartShownKey(challengeId);
            return KPlayerPrefs.GetInt(key, 0) == 1;
        }

        internal static void MarkStartSeen(string challengeId)
        {
            if (string.IsNullOrEmpty(challengeId) || challengeId == LevelNone)
            {
                return;
            }

            var key = GetStartShownKey(challengeId);
            KPlayerPrefs.SetInt(key, 1);
            KPlayerPrefs.Save();
        }

        internal static bool IsChallengeFailed(string challengeId)
        {
            if (string.IsNullOrEmpty(challengeId) || challengeId == LevelNone)
            {
                return false;
            }

            var key = GetFailureKey(challengeId);
            return KPlayerPrefs.GetInt(key, 0) == 1;
        }

        internal static void MarkChallengeFailed(string challengeId)
        {
            if (string.IsNullOrEmpty(challengeId) || challengeId == LevelNone)
            {
                return;
            }

            var key = GetFailureKey(challengeId);
            KPlayerPrefs.SetInt(key, 1);
            KPlayerPrefs.Save();
        }

        internal static void ClearChallengeFailed(string challengeId)
        {
            if (string.IsNullOrEmpty(challengeId) || challengeId == LevelNone)
            {
                return;
            }

            var key = GetFailureKey(challengeId);
            KPlayerPrefs.SetInt(key, 0);
            KPlayerPrefs.Save();
            LogInfo($"ClearChallengeFailed: {challengeId}");
        }

        internal static bool TryGetChallengeCompletionInfo(string challengeId, out string asteroidName, out int cycle)
        {
            asteroidName = null;
            cycle = 0;
            if (string.IsNullOrEmpty(challengeId) || challengeId == LevelNone)
            {
                return false;
            }

            if (!IsChallengeCompleted(challengeId))
            {
                return false;
            }

            asteroidName = KPlayerPrefs.GetString(GetCompletionAsteroidKey(challengeId), string.Empty);
            cycle = KPlayerPrefs.GetInt(GetCompletionCycleKey(challengeId), 0);
            return !string.IsNullOrEmpty(asteroidName) && cycle > 0;
        }

        internal static void ClearChallengeCompleted(string challengeId)
        {
            if (string.IsNullOrEmpty(challengeId) || challengeId == LevelNone)
            {
                return;
            }

            var key = GetCompletionKey(challengeId);
            if (KPlayerPrefs.HasKey(key))
            {
                KPlayerPrefs.DeleteKey(key);
            }

            var asteroidKey = GetCompletionAsteroidKey(challengeId);
            if (KPlayerPrefs.HasKey(asteroidKey))
            {
                KPlayerPrefs.DeleteKey(asteroidKey);
            }

            var cycleKey = GetCompletionCycleKey(challengeId);
            if (KPlayerPrefs.HasKey(cycleKey))
            {
                KPlayerPrefs.DeleteKey(cycleKey);
            }

            KPlayerPrefs.Save();
        }

        private static string GetChallengeKey(SaveGame.GameInfo info)
        {
            var uniqueId = SaveGame.GetSaveUniqueID(info);
            return SaveKeyPrefix + uniqueId;
        }

        private static string GetStartShownKey(string challengeId)
        {
            return StartShownKeyPrefix + GetSaveScopedId() + "_" + challengeId;
        }

        private static string GetFailureKey(string challengeId)
        {
            return FailureKeyPrefix + GetSaveScopedId() + "_" + challengeId;
        }

        private static string GetSaveScopedId()
        {
            if (SaveLoader.Instance == null)
            {
                return "unknown";
            }

            var info = SaveLoader.Instance.GameInfo;
            return SaveGame.GetSaveUniqueID(info);
        }

        private static string GetCompletionKey(string challengeId)
        {
            return CompletionKeyPrefix + challengeId;
        }

        private static string GetCompletionAsteroidKey(string challengeId)
        {
            return CompletionAsteroidKeyPrefix + challengeId;
        }

        private static string GetCompletionCycleKey(string challengeId)
        {
            return CompletionCycleKeyPrefix + challengeId;
        }

        private static string GetCurrentAsteroidName()
        {
            var cluster = ClusterManager.Instance;
            if (cluster == null)
            {
                return string.Empty;
            }

            var world = cluster.GetStartWorld();
            if (world == null)
            {
                return string.Empty;
            }

            if (!string.IsNullOrEmpty(world.worldType))
            {
                return Strings.Get(world.worldType);
            }

            if (!string.IsNullOrEmpty(world.worldName))
            {
                return Strings.Get(world.worldName);
            }

            return string.Empty;
        }

        private static void LogInfo(string message)
        {
            Debug.Log($"[NoPollutionMod] {message}");
        }
    }
}
