using System;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace SlavaMorozov.NoPollutionMod
{
    internal sealed class AllAchievementsProgressTile : KMonoBehaviour
    {
        private const int SkipCycleStep = 200;
        private static readonly AccessTools.FieldRef<GameClock, int> CycleRef =
            AccessTools.FieldRefAccess<GameClock, int>("cycle");
        private static readonly AccessTools.FieldRef<GameClock, float> TimeSinceStartRef =
            AccessTools.FieldRefAccess<GameClock, float>("timeSinceStartOfCycle");
        private Image fillImage;
        private ToolTip tooltip;
        private int lastCycle = -1;

        internal void Initialize(Image fill, ToolTip tooltip)
        {
            this.fillImage = fill;
            this.tooltip = tooltip;
            RefreshValues();
        }

        internal void RefreshNow()
        {
            RefreshValues();
        }

        internal static void ApplyCycleSkip()
        {
            if (GameClock.Instance == null)
            {
                return;
            }

            var gameClock = GameClock.Instance;
            var currentCycle = GameUtil.GetCurrentCycle();
            if (currentCycle >= AllAchievementsChallengeModule.CycleLimit)
            {
                if (CycleRef != null)
                {
                    CycleRef(gameClock) = 0;
                }

                if (TimeSinceStartRef != null)
                {
                    TimeSinceStartRef(gameClock) = 0f;
                }
            }
            else
            {
                var targetCycle = currentCycle + SkipCycleStep;
                if (targetCycle > AllAchievementsChallengeModule.CycleLimit)
                {
                    targetCycle = AllAchievementsChallengeModule.CycleLimit;
                }

                var targetTime = (targetCycle - 1) * 600f + gameClock.GetTimeSinceStartOfCycle();
                gameClock.SetTime(targetTime);
            }
        }

        private void Update()
        {
            if (fillImage == null || GameClock.Instance == null)
            {
                return;
            }

            var currentCycle = GameUtil.GetCurrentCycle();
            if (currentCycle == lastCycle)
            {
                return;
            }

            RefreshValues();
        }

        private void RefreshValues()
        {
            if (fillImage == null || GameClock.Instance == null)
            {
                return;
            }

            var currentCycle = GameUtil.GetCurrentCycle();
            lastCycle = currentCycle;
            var remainingCycles = Mathf.Max(0, AllAchievementsChallengeModule.CycleLimit - currentCycle + 1);
            var fillAmount = Mathf.Clamp01(remainingCycles / (float)AllAchievementsChallengeModule.CycleLimit);
            fillImage.fillAmount = fillAmount;
            if (tooltip != null)
            {
                tooltip.SetSimpleTooltip(string.Format(
                    STRINGS.NO_POLLUTION_CHALLENGE.ALL_ACHIEVEMENTS_REMAINING_CYCLES.ToString(),
                    remainingCycles
                ));
            }
        }
    }

    internal static partial class TopLeftControlScreen_OnActivate_Patch
    {
        private static void TryAddAllAchievementsProgressTile(TopLeftControlScreen screen, RectTransform row)
        {
            try
            {
                if (screen == null || row == null)
                {
                    return;
                }

                if (!ChallengeGuards.IsAllAchievementsChallengeActive())
                {
                    return;
                }

                var sandboxToggle = SandboxToggleRef(screen);
                if (sandboxToggle == null)
                {
                    return;
                }

                var progressTile = Util.KInstantiateUI<MultiToggle>(sandboxToggle.gameObject, row.gameObject, true);
                progressTile.name = AllAchievementsProgressTileName;
                progressTile.ChangeState(1);
                progressTile.onClick = null;
                progressTile.play_sound_on_click = false;

                var placeholderSprite = ModAssets.PlaceholderSprite ?? ModAssets.LogoSprite;
                if (progressTile.toggle_image != null)
                {
                    progressTile.toggle_image.enabled = false;
                }

                var layoutGroup = progressTile.GetComponent<HorizontalLayoutGroup>();
                if (layoutGroup != null)
                {
                    layoutGroup.padding = new RectOffset(0, 0, 0, 0);
                    layoutGroup.spacing = 0f;
                }

                for (int i = 0; i < progressTile.transform.childCount; i++)
                {
                    var child = progressTile.transform.GetChild(i);
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

                var fg = progressTile.transform.Find("FG");
                if (fg != null)
                {
                    var fgImage = fg.GetComponent<Image>();
                    if (fgImage != null)
                    {
                        fgImage.sprite = null;
                        fgImage.enabled = false;
                    }
                }

                var label = progressTile.transform.Find("Label");
                if (label != null)
                {
                    var labelText = label.GetComponent<LocText>();
                    if (labelText != null)
                    {
                        labelText.text = string.Empty;
                    }
                }

                var tooltip = progressTile.GetComponent<ToolTip>() ?? progressTile.gameObject.AddComponent<ToolTip>();

                if (placeholderSprite != null)
                {
                    var placeholderObject = new GameObject("PlaceholderBg", typeof(RectTransform), typeof(Image), typeof(LayoutElement));
                    placeholderObject.transform.SetParent(progressTile.transform, false);
                    placeholderObject.transform.SetAsFirstSibling();
                    var placeholderRect = placeholderObject.GetComponent<RectTransform>();
                    placeholderRect.anchorMin = Vector2.zero;
                    placeholderRect.anchorMax = Vector2.one;
                    placeholderRect.offsetMin = Vector2.zero;
                    placeholderRect.offsetMax = Vector2.zero;

                    var placeholderLayout = placeholderObject.GetComponent<LayoutElement>();
                    placeholderLayout.ignoreLayout = true;

                    var placeholderImage = placeholderObject.GetComponent<Image>();
                    placeholderImage.sprite = placeholderSprite;
                    placeholderImage.preserveAspect = true;
                    placeholderImage.raycastTarget = false;
                }

                var fillObject = new GameObject("ProgressFill", typeof(RectTransform), typeof(Image), typeof(LayoutElement));
                fillObject.transform.SetParent(progressTile.transform, false);
                fillObject.transform.SetAsLastSibling();
                var fillRect = fillObject.GetComponent<RectTransform>();
                fillRect.anchorMin = Vector2.zero;
                fillRect.anchorMax = Vector2.one;
                fillRect.offsetMin = Vector2.zero;
                fillRect.offsetMax = Vector2.zero;

                var fillLayout = fillObject.GetComponent<LayoutElement>();
                fillLayout.ignoreLayout = true;

                var fillImage = fillObject.GetComponent<Image>();
                fillImage.sprite = progressTile.toggle_image != null && progressTile.toggle_image.sprite != null
                    ? progressTile.toggle_image.sprite
                    : ModAssets.LogoSprite;
                fillImage.type = Image.Type.Filled;
                fillImage.fillMethod = Image.FillMethod.Horizontal;
                fillImage.fillOrigin = 0;
                fillImage.fillAmount = 1f;
                fillImage.raycastTarget = false;
                fillImage.preserveAspect = false;
                var barColor = ProgressBarsConfig.Instance.GetBarColor("ProgressBar");
                barColor.a = 0.75f;
                fillImage.color = barColor;

                if (label != null)
                {
                    label.SetAsLastSibling();
                }

                var progressBehaviour = progressTile.gameObject.AddComponent<AllAchievementsProgressTile>();
                progressBehaviour.Initialize(fillImage, tooltip);

                progressTile.onClick += () =>
                {
                    if (DebugOptionsScreen.IsDebugEnabled())
                    {
                        AllAchievementsProgressTile.ApplyCycleSkip();
                        progressBehaviour.RefreshNow();
                    }
                };
            }
            catch (Exception e)
            {
                ModLog.Error($"AllAchievementsProgressTile: failed to create progress tile. {e}");
                return;
            }
        }

    }

    [HarmonyPatch(typeof(ColonyAchievementTracker), "RenderEveryTick")]
    internal static class ColonyAchievementTracker_RenderEveryTick_Patch
    {
        private const float CheckIntervalSeconds = 5f;
        private static float timeToCheck;

        private static void Postfix(float dt)
        {
            if (!ChallengeGuards.IsAllAchievementsChallengeActive())
            {
                timeToCheck = CheckIntervalSeconds;
                return;
            }

            timeToCheck -= dt;
            if (timeToCheck > 0f)
            {
                return;
            }

            timeToCheck = CheckIntervalSeconds;
            var challengeId = ChallengeSettings.GetCurrentChallengeLevelId();
            if (ChallengeSettings.IsChallengeCompleted(challengeId))
            {
                return;
            }

            if (GameUtil.GetCurrentCycle() > AllAchievementsChallengeModule.CycleLimit &&
                !ChallengeSettings.IsChallengeFailed(challengeId))
            {
                ModLog.Info($"VictoryCheck: all achievements failed. challenge={challengeId}");
                ChallengeSettings.MarkChallengeFailed(challengeId);
                ChallengeFailureDialog.Schedule(challengeId);
                return;
            }

            if (AllAchievementsChallengeModule.AreAllAchievementsComplete())
            {
                ModLog.Info($"VictoryCheck: periodic all achievements complete. challenge={challengeId}");
                ChallengeSettings.MarkChallengeCompleted(challengeId);
                ChallengeVictoryDialog.Schedule(challengeId, AllAchievementsChallengeModule.ModuleId);
            }
        }
    }

    [HarmonyPatch(typeof(ColonyAchievementTracker), "TriggerNewAchievementCompleted")]
    internal static class ColonyAchievementTracker_TriggerNewAchievementCompleted_Patch
    {
        private static void Postfix(string achievement)
        {
            if (!ChallengeSettings.IsChallengeActive())
            {
                ModLog.Info("VictoryCheck: challenge inactive.");
                return;
            }

            ModLog.Info($"VictoryCheck: achievement completed {achievement}");

            if (ChallengeGuards.IsAllAchievementsChallengeActive())
            {
                var challengeId = ChallengeSettings.GetCurrentChallengeLevelId();
                if (!ChallengeSettings.IsChallengeCompleted(challengeId) && AllAchievementsChallengeModule.AreAllAchievementsComplete())
                {
                    ModLog.Info($"VictoryCheck: all achievements complete. challenge={challengeId}");
                    ChallengeSettings.MarkChallengeCompleted(challengeId);
                    ChallengeVictoryDialog.Schedule(challengeId, achievement);
                }
                return;
            }

            if (IsVictoryAchievement(achievement))
            {
                var challengeId = ChallengeSettings.GetCurrentChallengeLevelId();
                ModLog.Info($"VictoryCheck: victory achievement matched. challenge={challengeId}");
                ChallengeSettings.MarkChallengeCompleted(challengeId);
                ChallengeVictoryDialog.Schedule(challengeId, achievement);
            }
            else
            {
                ModLog.Info("VictoryCheck: achievement is not a victory condition.");
            }
        }

        private static bool IsVictoryAchievement(string achievement)
        {
            var achievements = Db.Get().ColonyAchievements;
            return (achievements.Thriving != null && achievement == achievements.Thriving.Id) ||
                   (achievements.ReachedDistantPlanet != null && achievement == achievements.ReachedDistantPlanet.Id) ||
                   (achievements.CollectedArtifacts != null && achievement == achievements.CollectedArtifacts.Id) ||
                   (achievements.ActivateGeothermalPlant != null && achievement == achievements.ActivateGeothermalPlant.Id) ||
                   (achievements.AsteroidDestroyed != null && achievement == achievements.AsteroidDestroyed.Id);
        }
    }
}
