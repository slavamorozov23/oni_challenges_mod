using UnityEngine;

namespace SlavaMorozov.NoPollutionMod
{
    internal static class ModLog
    {
        internal static void Info(string message)
        {
            Debug.Log($"[NoPollutionMod] {message}");
        }

        internal static void Error(string message)
        {
            Debug.LogError($"[NoPollutionMod] {message}");
        }
    }

    internal static class UiHelpers
    {
        internal static void ConfigureTooltip(ToolTip tooltip, string text)
        {
            if (tooltip == null)
            {
                return;
            }

            tooltip.UseFixedStringKey = false;
            tooltip.SetSimpleTooltip(text);
            if (ToolTipScreen.Instance != null)
            {
                ToolTipScreen.Instance.SetToolTip(tooltip);
            }
        }
    }

    internal static class ChallengeGuards
    {
        internal static bool IsOneDuplicantChallengeActive()
        {
            return ChallengeModuleManager.GetActiveModule() is OneDuplicantChallengeModule;
        }

        internal static bool IsAllAchievementsChallengeActive()
        {
            return ChallengeModuleManager.GetActiveModule() is AllAchievementsChallengeModule;
        }

        internal static bool IsNoToiletsChallengeActive()
        {
            return ChallengeModuleManager.GetActiveModule() is NoToiletsChallengeModule;
        }
    }
}
