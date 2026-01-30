using System;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using ModStrings = SlavaMorozov.NoPollutionMod.STRINGS;

namespace SlavaMorozov.NoPollutionMod
{
    [HarmonyPatch(typeof(TopLeftControlScreen), "OnActivate")]
    internal static partial class TopLeftControlScreen_OnActivate_Patch
    {
        private const string ChallengeIconName = "NoPollutionChallengeTopLeftIcon";
        private const string WinButtonName = "NoPollutionChallengeWinButton";
        private const string AchievementsButtonName = "NoPollutionChallengeAchievementsButton";
        private const string AllAchievementsProgressTileName = "NoPollutionChallengeAllAchievementsProgress";
        private static readonly AccessTools.FieldRef<TopLeftControlScreen, RectTransform> SecondaryRowRef =
            AccessTools.FieldRefAccess<TopLeftControlScreen, RectTransform>("secondaryRow");
        private static readonly AccessTools.FieldRef<TopLeftControlScreen, MultiToggle> SandboxToggleRef =
            AccessTools.FieldRefAccess<TopLeftControlScreen, MultiToggle>("sandboxToggle");

        private static void Postfix(TopLeftControlScreen __instance)
        {
            if (__instance == null)
            {
                return;
            }

            var row = SecondaryRowRef(__instance);
            if (row == null)
            {
                return;
            }

            var existing = row.Find(ChallengeIconName);
            if (existing != null)
            {
                UnityEngine.Object.DestroyImmediate(existing.gameObject);
            }

            var existingWin = row.Find(WinButtonName);
            if (existingWin != null)
            {
                UnityEngine.Object.DestroyImmediate(existingWin.gameObject);
            }

            var existingAchievements = row.Find(AchievementsButtonName);
            if (existingAchievements != null)
            {
                UnityEngine.Object.DestroyImmediate(existingAchievements.gameObject);
            }

            var existingProgressTile = row.Find(AllAchievementsProgressTileName);
            if (existingProgressTile != null)
            {
                UnityEngine.Object.DestroyImmediate(existingProgressTile.gameObject);
            }

            if (!ChallengeSettings.IsChallengeActive())
            {
                ModLog.Info("TopLeftControlScreen.OnActivate: challenge inactive");
                return;
            }

            ModLog.Info("TopLeftControlScreen.OnActivate: challenge icon injected");

            var module = ChallengeModuleManager.GetActiveModule();
            if (module == null)
            {
                return;
            }

            var sprite = ModAssets.GetChallengeMedalSprite(module.Id) ?? ModAssets.LogoSprite;
            if (sprite == null)
            {
                return;
            }

            var icon = new GameObject(
                ChallengeIconName,
                typeof(RectTransform),
                typeof(Image),
                typeof(ToolTip),
                typeof(LayoutElement)
            );
            icon.transform.SetParent(row, false);
            icon.transform.SetAsLastSibling();

            var rect = icon.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(28f, 28f);

            var layout = icon.GetComponent<LayoutElement>();
            layout.preferredWidth = 28f;
            layout.preferredHeight = 28f;
            layout.minWidth = 28f;
            layout.minHeight = 28f;

            var image = icon.GetComponent<Image>();
            image.sprite = sprite;
            image.preserveAspect = true;

            var tooltip = icon.GetComponent<ToolTip>();
            UiHelpers.ConfigureTooltip(tooltip, ChallengeSettings.GetChallengeName(module.Id));

            TryAddWinButton(__instance, row);
            TryAddAchievementsButton(__instance, row);
            TryAddAllAchievementsProgressTile(__instance, row);
        }

        private static void TryAddWinButton(TopLeftControlScreen screen, RectTransform row)
        {
            if (screen == null || row == null)
            {
                return;
            }

            if (!ChallengeSettings.IsChallengeActive() || !DebugOptionsScreen.IsDebugEnabled())
            {
                return;
            }

            var sandboxToggle = SandboxToggleRef(screen);
            if (sandboxToggle == null)
            {
                return;
            }

            var winToggle = Util.KInstantiateUI<MultiToggle>(sandboxToggle.gameObject, row.gameObject, true);
            winToggle.name = WinButtonName;
            winToggle.ChangeState(1);
            winToggle.onClick = null;
            winToggle.onClick += DebugOptionsScreen.TryGrantVictory;
            DisableSandboxIcon(winToggle);

            var label = winToggle.transform.Find("Label");
            if (label != null)
            {
                var labelText = label.GetComponent<LocText>();
                if (labelText != null)
                {
                    labelText.text = ModStrings.UI.DEBUG_WIN_BUTTON;
                }
            }

            var tooltip = winToggle.GetComponent<ToolTip>();
            if (tooltip != null)
            {
                tooltip.SetSimpleTooltip(ModStrings.UI.DEBUG_WIN_BUTTON);
            }

        }

        private static void TryAddAchievementsButton(TopLeftControlScreen screen, RectTransform row)
        {
            if (screen == null || row == null)
            {
                return;
            }

            if (!ChallengeSettings.IsChallengeActive() || !DebugOptionsScreen.IsDebugEnabled())
            {
                return;
            }

            var sandboxToggle = SandboxToggleRef(screen);
            if (sandboxToggle == null)
            {
                return;
            }

            var achievementsToggle = Util.KInstantiateUI<MultiToggle>(sandboxToggle.gameObject, row.gameObject, true);
            achievementsToggle.name = AchievementsButtonName;
            achievementsToggle.ChangeState(1);
            achievementsToggle.onClick = null;
            achievementsToggle.onClick += DebugOptionsScreen.TryGrantAllAchievements;
            DisableSandboxIcon(achievementsToggle);

            var label = achievementsToggle.transform.Find("Label");
            if (label != null)
            {
                var labelText = label.GetComponent<LocText>();
                if (labelText != null)
                {
                    labelText.text = ModStrings.UI.DEBUG_ACHIEVEMENTS_BUTTON;
                }
            }

            var tooltip = achievementsToggle.GetComponent<ToolTip>();
            if (tooltip != null)
            {
                tooltip.SetSimpleTooltip(ModStrings.UI.DEBUG_ACHIEVEMENTS_BUTTON);
            }

        }

        private static void DisableSandboxIcon(MultiToggle toggle)
        {
            if (toggle == null)
            {
                return;
            }

            for (int i = 0; i < toggle.transform.childCount; i++)
            {
                var child = toggle.transform.GetChild(i);
                if (child == null)
                {
                    continue;
                }

                if (child.name.IndexOf("icon", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    var iconImage = child.GetComponent<Image>();
                    if (iconImage != null)
                    {
                        iconImage.enabled = false;
                    }
                }
            }

            var fg = toggle.transform.Find("FG");
            if (fg != null)
            {
                var fgImage = fg.GetComponent<Image>();
                if (fgImage != null)
                {
                    fgImage.enabled = false;
                }
            }
        }
    }
}
