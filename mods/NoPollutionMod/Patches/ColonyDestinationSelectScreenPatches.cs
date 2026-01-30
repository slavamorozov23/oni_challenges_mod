using HarmonyLib;
using Klei.CustomSettings;
using UnityEngine;

namespace SlavaMorozov.NoPollutionMod
{
    [HarmonyPatch(typeof(ColonyDestinationSelectScreen), "OnSpawn")]
    internal static class ColonyDestinationSelectScreen_OnSpawn_Patch
    {
        private static readonly AccessTools.FieldRef<ColonyDestinationSelectScreen, StoryContentPanel> StoryPanelRef =
            AccessTools.FieldRefAccess<ColonyDestinationSelectScreen, StoryContentPanel>("storyContentPanel");

        private static readonly AccessTools.FieldRef<ColonyDestinationSelectScreen, NewGameSettingsPanel> SettingsPanelRef =
            AccessTools.FieldRefAccess<ColonyDestinationSelectScreen, NewGameSettingsPanel>("newGameSettingsPanel");

        private static readonly AccessTools.FieldRef<StoryContentPanel, GameObject> StoryRowContainerRef =
            AccessTools.FieldRefAccess<StoryContentPanel, GameObject>("storyRowContainer");

        private static readonly AccessTools.FieldRef<NewGameSettingsPanel, GameObject> CycleSettingPrefabRef =
            AccessTools.FieldRefAccess<NewGameSettingsPanel, GameObject>("prefab_cycle_setting");

        private const string ChallengeRowName = "NoPollutionChallengeSetting";

        private static void Postfix(ColonyDestinationSelectScreen __instance)
        {
            if (__instance == null)
            {
                return;
            }

            var storyPanel = StoryPanelRef(__instance);
            var settingsPanel = SettingsPanelRef(__instance);
            if (storyPanel == null || settingsPanel == null)
            {
                return;
            }

            ChallengeSettings.RegisterStrings();
            ChallengeSettings.EnsureConfig();

            var container = StoryRowContainerRef(storyPanel);
            if (container == null || container.transform == null)
            {
                return;
            }

            if (container.transform.Find(ChallengeRowName) != null)
            {
                return;
            }

            var prefab = CycleSettingPrefabRef(settingsPanel);
            if (prefab == null)
            {
                return;
            }

            var widget = Util.KInstantiateUI<CustomGameSettingListWidget>(prefab, container, false);
            widget.name = ChallengeRowName;
            widget.Initialize(
                ChallengeSettings.Config,
                CustomGameSettings.Instance.GetCurrentQualitySetting,
                CustomGameSettings.Instance.CycleQualitySettingLevel
            );
            widget.gameObject.SetActive(true);
            widget.transform.SetAsFirstSibling();
            widget.onSettingChanged += _ =>
            {
                widget.Refresh();
                __instance.RefreshRowsAndDescriptions();
            };
            widget.Refresh();
        }
    }
}
