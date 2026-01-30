using HarmonyLib;
using Klei.CustomSettings;
using UnityEngine;

namespace SlavaMorozov.NoPollutionMod
{
    [HarmonyPatch(typeof(CustomGameSettings), "OnPrefabInit")]
    internal static class CustomGameSettings_OnPrefabInit_Patch
    {
        private static void Postfix(CustomGameSettings __instance)
        {
            ModLog.Info("CustomGameSettings.OnPrefabInit Postfix");
            ChallengeSettings.RegisterStrings();
            ChallengeSettings.EnsureConfig();

            if (__instance.QualitySettings.ContainsKey(ChallengeSettings.SettingId))
            {
                return;
            }

            __instance.AddQualitySettingConfig(ChallengeSettings.Config);
            if (ChallengeSettings.Config.coordinate_range >= 0L)
            {
                __instance.CoordinatedQualitySettings.Add(ChallengeSettings.SettingId);
            }
        }
    }

    [HarmonyPatch(typeof(SaveLoader), "Load", new[] { typeof(string) })]
    internal static class SaveLoader_Load_Patch
    {
        private static void Postfix(SaveLoader __instance)
        {
            if (__instance == null)
            {
                return;
            }

            ModLog.Info("SaveLoader.Load Postfix: apply challenge for save");
            ChallengeSettings.ApplyChallengeForGameInfo(__instance.GameInfo);
        }
    }

    [HarmonyPatch(typeof(SaveLoader), "InitialSave")]
    internal static class SaveLoader_InitialSave_Patch
    {
        private static void Postfix()
        {
            ModLog.Info("SaveLoader.InitialSave Postfix: store challenge");
            ChallengeSettings.StoreChallengeForCurrentSave();
        }
    }

    [HarmonyPatch(typeof(SaveLoader), "Save", new[] { typeof(string), typeof(bool), typeof(bool) })]
    internal static class SaveLoader_Save_Patch
    {
        private static void Postfix()
        {
            ModLog.Info("SaveLoader.Save Postfix: store challenge");
            ChallengeSettings.StoreChallengeForCurrentSave();
        }
    }

    [HarmonyPatch(typeof(Game), "OnSpawn")]
    internal static class Game_OnSpawn_Patch
    {
        private static void Postfix()
        {
            var challengeId = ChallengeSettings.GetCurrentChallengeLevelId();
            if (string.IsNullOrEmpty(challengeId) || challengeId == ChallengeSettings.LevelNone)
            {
                return;
            }

            if (ChallengeSettings.HasSeenStart(challengeId))
            {
                return;
            }

            ChallengeStartDialog.Schedule(challengeId);
        }
    }
}
