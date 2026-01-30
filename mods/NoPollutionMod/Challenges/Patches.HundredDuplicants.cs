using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace SlavaMorozov.NoPollutionMod
{
    internal static class HundredDuplicantsImmigrantScreenGuard
    {
        internal static float SuppressUntil;

        internal static bool ShouldSuppress()
        {
            return Time.unscaledTime < SuppressUntil;
        }

        internal static void ExtendSuppression(float seconds)
        {
            var until = Time.unscaledTime + seconds;
            if (until > SuppressUntil)
            {
                ModLog.Info($"HundredDuplicants: ExtendSuppression {seconds:F2}s (now={Time.unscaledTime:F3}, prevUntil={SuppressUntil:F3}, newUntil={until:F3})");
                SuppressUntil = until;
            }
            else
            {
                ModLog.Info($"HundredDuplicants: ExtendSuppression {seconds:F2}s ignored (now={Time.unscaledTime:F3}, currentUntil={SuppressUntil:F3}, candidateUntil={until:F3})");
            }
        }
    }

    internal sealed class HundredDuplicantsProgressTile : KMonoBehaviour
    {
        private Image fillImage;
        private Image failureImage;
        private ToolTip tooltip;
        private int lastDuplicantCount = -1;
        private bool hasFailed;
        private KButton button;

        internal void Initialize(Image fill, Image failure, ToolTip tooltip)
        {
            this.fillImage = fill;
            this.failureImage = failure;
            this.tooltip = tooltip;
            
            this.button = gameObject.GetComponent<KButton>();
            if (this.button == null)
            {
                this.button = gameObject.AddComponent<KButton>();
                this.button.additionalKImages = new KImage[0];
                if (this.button.soundPlayer == null)
                {
                    this.button.soundPlayer = new ButtonSoundPlayer();
                }
            }
            
            this.button.onClick += OnClick;
            
            RefreshValues();
        }

        private void OnClick()
        {
            if (!DebugOptionsScreen.IsDebugEnabled())
            {
                ModLog.Info("HundredDuplicantsProgressTile: Click ignored (debug disabled)");
                return;
            }

            var challengeId = ChallengeSettings.GetCurrentChallengeLevelId();
            ModLog.Info($"HundredDuplicantsProgressTile: Click (challengeId={challengeId}, failed={ChallengeSettings.IsChallengeFailed(challengeId)}, completed={ChallengeSettings.IsChallengeCompleted(challengeId)})");
            
            if (ChallengeSettings.IsChallengeFailed(challengeId))
            {
                ModLog.Info($"HundredDuplicants: Debug reset failure for challenge {challengeId}");
                ChallengeSettings.ClearChallengeFailed(challengeId);
                hasFailed = false;
                RefreshValues();
                return;
            }

            SkipToNextPrint();
        }

        private void SkipToNextPrint()
        {
            var immigration = Immigration.Instance;
            if (immigration == null)
            {
                ModLog.Info("HundredDuplicants: Immigration instance not found");
                return;
            }

            ModLog.Info($"HundredDuplicants: SkipToNextPrint ENTER (now={Time.unscaledTime:F3})");

            var currentTime = immigration.timeBeforeSpawn;

            ModLog.Info($"HundredDuplicants: SkipToNextPrint current timeBeforeSpawn={currentTime:F2}");

            if (currentTime <= 0.1f)
            {
                ModLog.Info($"HundredDuplicants: Already close to print time ({currentTime:F2} cycles)");
                return;
            }

            immigration.timeBeforeSpawn = 0f;
            
            var bImmigrantAvailableField = AccessTools.Field(typeof(Immigration), "bImmigrantAvailable");
            if (bImmigrantAvailableField != null)
            {
                bImmigrantAvailableField.SetValue(immigration, true);
                ModLog.Info("HundredDuplicants: SkipToNextPrint set bImmigrantAvailable=true");
            }
            else
            {
                ModLog.Info("HundredDuplicants: SkipToNextPrint cannot access bImmigrantAvailable field");
            }

            ModLog.Info($"HundredDuplicants: SkipToNextPrint set print time to 0 (skipped {currentTime:F2} cycles). UI will NOT be opened.");
        }

        internal void RefreshNow()
        {
            RefreshValues();
        }

        private void Update()
        {
            if (fillImage == null)
            {
                return;
            }

            var currentCount = GetLivingDuplicantCount();
            if (currentCount == lastDuplicantCount)
            {
                return;
            }

            RefreshValues();
        }

        private void RefreshValues()
        {
            if (fillImage == null)
            {
                return;
            }

            var currentCount = GetLivingDuplicantCount();
            lastDuplicantCount = currentCount;

            var challengeId = ChallengeSettings.GetCurrentChallengeLevelId();
            if (ChallengeSettings.IsChallengeFailed(challengeId))
            {
                if (!hasFailed)
                {
                    hasFailed = true;
                    if (fillImage != null)
                    {
                        fillImage.gameObject.SetActive(false);
                    }
                    if (failureImage != null)
                    {
                        failureImage.gameObject.SetActive(true);
                    }
                }

                if (tooltip != null)
                {
                    tooltip.SetSimpleTooltip(STRINGS.NO_POLLUTION_CHALLENGE.HUNDRED_DUPLICANTS_FAILED_TOOLTIP.ToString());
                }
                return;
            }

            if (fillImage != null && failureImage != null)
            {
                fillImage.gameObject.SetActive(true);
                failureImage.gameObject.SetActive(false);
            }

            var fillAmount = Mathf.Clamp01(currentCount / (float)HundredDuplicantsChallengeModule.TargetDuplicantCount);
            if (fillImage != null)
            {
                fillImage.fillAmount = fillAmount;
            }

            if (tooltip != null)
            {
                tooltip.SetSimpleTooltip(string.Format(
                    STRINGS.NO_POLLUTION_CHALLENGE.HUNDRED_DUPLICANTS_PROGRESS.ToString(),
                    currentCount,
                    HundredDuplicantsChallengeModule.TargetDuplicantCount
                ));
            }
        }

        private static int GetLivingDuplicantCount()
        {
            if (Components.LiveMinionIdentities == null)
            {
                return 0;
            }

            return Components.LiveMinionIdentities.Count;
        }
    }

    internal static partial class TopLeftControlScreen_OnActivate_Patch
    {
        private const string HundredDuplicantsProgressTileName = "HundredDuplicantsProgressTile";

        private static void TryAddHundredDuplicantsProgressTile(TopLeftControlScreen screen, RectTransform row)
        {
            try
            {
                if (screen == null || row == null)
                {
                    return;
                }

                if (!ChallengeGuards.IsHundredDuplicantsChallengeActive())
                {
                    return;
                }

                var sandboxToggle = SandboxToggleRef(screen);
                if (sandboxToggle == null)
                {
                    return;
                }

                var progressTile = Util.KInstantiateUI<MultiToggle>(sandboxToggle.gameObject, row.gameObject, true);
                progressTile.name = HundredDuplicantsProgressTileName;
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
                fillImage.fillAmount = 0f;
                fillImage.raycastTarget = false;
                fillImage.preserveAspect = false;
                var barColor = ProgressBarsConfig.Instance.GetBarColor("ProgressBar");
                barColor.a = 0.75f;
                fillImage.color = barColor;

                var failureObject = new GameObject("FailureIcon", typeof(RectTransform), typeof(Image), typeof(LayoutElement));
                failureObject.transform.SetParent(progressTile.transform, false);
                failureObject.transform.SetAsLastSibling();
                var failureRect = failureObject.GetComponent<RectTransform>();
                failureRect.anchorMin = Vector2.zero;
                failureRect.anchorMax = Vector2.one;
                failureRect.offsetMin = Vector2.zero;
                failureRect.offsetMax = Vector2.zero;

                var failureLayout = failureObject.GetComponent<LayoutElement>();
                failureLayout.ignoreLayout = true;

                var failureImage = failureObject.GetComponent<Image>();
                failureImage.sprite = Assets.GetSprite("icon_cancel");
                failureImage.preserveAspect = true;
                failureImage.raycastTarget = false;
                failureImage.color = Color.red;
                failureObject.SetActive(false);

                if (label != null)
                {
                    label.SetAsLastSibling();
                }

                var progressBehaviour = progressTile.gameObject.AddComponent<HundredDuplicantsProgressTile>();
                progressBehaviour.Initialize(fillImage, failureImage, tooltip);
            }
            catch (Exception e)
            {
                ModLog.Error($"HundredDuplicantsProgressTile: failed to create progress tile. {e}");
            }
        }
    }

    [HarmonyPatch(typeof(Immigration), "Sim200ms")]
    internal static class Immigration_Sim200ms_Patch
    {
        private static bool Prepare()
        {
            ModLog.Info("HundredDuplicants: Harmony Prepare Immigration.Sim200ms patch");
            return true;
        }

        private static void Postfix(Immigration __instance, float dt)
        {
            if (!ChallengeGuards.IsHundredDuplicantsChallengeActive())
            {
                return;
            }

            var bImmigrantAvailableField = AccessTools.Field(typeof(Immigration), "bImmigrantAvailable");
            if (bImmigrantAvailableField == null)
            {
                return;
            }

            var bImmigrantAvailable = (bool)bImmigrantAvailableField.GetValue(__instance);
            if (!bImmigrantAvailable)
            {
                return;
            }

            HundredDuplicantsImmigrantScreenGuard.ExtendSuppression(5f);

            var challengeId = ChallengeSettings.GetCurrentChallengeLevelId();
            if (ChallengeSettings.IsChallengeCompleted(challengeId) || ChallengeSettings.IsChallengeFailed(challengeId))
            {
                return;
            }

            var currentCount = Components.LiveMinionIdentities != null ? Components.LiveMinionIdentities.Count : 0;
            if (currentCount >= HundredDuplicantsChallengeModule.TargetDuplicantCount)
            {
                return;
            }

            if (Components.Telepads == null || Components.Telepads.Count == 0)
            {
                return;
            }

            ModLog.Info($"HundredDuplicants: Auto-printing duplicant. Current count: {currentCount}");

            bImmigrantAvailableField.SetValue(__instance, false);

            var availableModels = new List<Tag>
            {
                GameTags.Minions.Models.Standard
            };

            if (DlcManager.IsContentSubscribed("DLC3_ID"))
            {
                availableModels.Add(GameTags.Minions.Models.Bionic);
            }

            var stats = new MinionStartingStats(availableModels, is_starter_minion: false, null, null, false);
            var telepad = Components.Telepads[0];
            
            HundredDuplicantsImmigrantScreenGuard.ExtendSuppression(5f);
            ModLog.Info($"HundredDuplicants: Auto-print calling telepad.OnAcceptDelivery (now={Time.unscaledTime:F3}, suppressUntil={HundredDuplicantsImmigrantScreenGuard.SuppressUntil:F3})");
            telepad.OnAcceptDelivery(stats);
            ModLog.Info($"HundredDuplicants: Auto-print finished telepad.OnAcceptDelivery (now={Time.unscaledTime:F3})");
        }
    }

    [HarmonyPatch(typeof(ImmigrantScreen), nameof(ImmigrantScreen.InitializeImmigrantScreen), new[] { typeof(Telepad) })]
    internal static class HundredDuplicants_ImmigrantScreen_InitializeImmigrantScreen_Patch
    {
        private static bool Prepare()
        {
            ModLog.Info("HundredDuplicants: Harmony Prepare ImmigrantScreen.InitializeImmigrantScreen patch");
            return true;
        }

        private static bool Prefix()
        {
            if (!ChallengeGuards.IsHundredDuplicantsChallengeActive())
            {
                return true;
            }

            var challengeId = ChallengeSettings.GetCurrentChallengeLevelId();
            if (ChallengeSettings.IsChallengeCompleted(challengeId) || ChallengeSettings.IsChallengeFailed(challengeId))
            {
                return true;
            }

            ModLog.Info($"HundredDuplicants: BLOCK InitializeImmigrantScreen (now={Time.unscaledTime:F3}, suppress={HundredDuplicantsImmigrantScreenGuard.ShouldSuppress()}, suppressUntil={HundredDuplicantsImmigrantScreenGuard.SuppressUntil:F3})");
            ModLog.Info(Environment.StackTrace);
            return false;
        }
    }

    [HarmonyPatch(typeof(ImmigrantScreen), "OnShow")]
    internal static class HundredDuplicants_ImmigrantScreen_OnShow_Patch
    {
        private static bool Prepare()
        {
            ModLog.Info("HundredDuplicants: Harmony Prepare ImmigrantScreen.OnShow patch");
            return true;
        }

        private static bool Prefix(ImmigrantScreen __instance, bool show)
        {
            ModLog.Info($"HundredDuplicants: ImmigrantScreen.OnShow Prefix (show={show}, now={Time.unscaledTime:F3}, instanceNull={__instance == null}, suppress={HundredDuplicantsImmigrantScreenGuard.ShouldSuppress()}, suppressUntil={HundredDuplicantsImmigrantScreenGuard.SuppressUntil:F3})");

            if (!show)
            {
                return true;
            }

            if (!ChallengeGuards.IsHundredDuplicantsChallengeActive())
            {
                return true;
            }

            var challengeId = ChallengeSettings.GetCurrentChallengeLevelId();
            if (ChallengeSettings.IsChallengeCompleted(challengeId) || ChallengeSettings.IsChallengeFailed(challengeId))
            {
                return true;
            }

            ModLog.Info($"HundredDuplicants: BLOCK ImmigrantScreen.OnShow(true) (now={Time.unscaledTime:F3}, suppress={HundredDuplicantsImmigrantScreenGuard.ShouldSuppress()}, suppressUntil={HundredDuplicantsImmigrantScreenGuard.SuppressUntil:F3})");
            ModLog.Info(Environment.StackTrace);

            if (__instance != null)
            {
                __instance.Show(false);
            }

            return false;
        }
    }

    [HarmonyPatch(typeof(MinionIdentity), "OnSpawn")]
    internal static class HundredDuplicants_MinionIdentity_OnSpawn_DeathSubscribe_Patch
    {
        private static void Postfix(MinionIdentity __instance)
        {
            if (!ChallengeGuards.IsHundredDuplicantsChallengeActive())
            {
                return;
            }

            if (__instance == null || __instance.gameObject == null)
            {
                return;
            }

            ModLog.Info($"HundredDuplicants: Subscribing to death event for {__instance.name}");
            __instance.Subscribe((int)GameHashes.Died, OnDuplicantDied);
        }

        private static void OnDuplicantDied(object data)
        {
            ModLog.Info("HundredDuplicants: OnDuplicantDied event fired!");
            
            if (!ChallengeGuards.IsHundredDuplicantsChallengeActive())
            {
                ModLog.Info("HundredDuplicants: Challenge not active, ignoring death");
                return;
            }

            if (!GameClock.Instance)
            {
                ModLog.Info("HundredDuplicants: GameClock not ready, ignoring death");
                return;
            }

            var cycle = GameClock.Instance.GetCycle();
            var time = GameClock.Instance.GetTime();
            ModLog.Info($"HundredDuplicants: Current cycle: {cycle}, time: {time:F1}s");
            
            if (time < 10f)
            {
                ModLog.Info("HundredDuplicants: Game just started (time < 10s), ignoring death during initialization");
                return;
            }

            var challengeId = ChallengeSettings.GetCurrentChallengeLevelId();
            var isCompleted = ChallengeSettings.IsChallengeCompleted(challengeId);
            var isFailed = ChallengeSettings.IsChallengeFailed(challengeId);
            ModLog.Info($"HundredDuplicants: Challenge state - ID: {challengeId}, Completed: {isCompleted}, Failed: {isFailed}");
            
            if (isCompleted || isFailed)
            {
                ModLog.Info("HundredDuplicants: Challenge already completed or failed, ignoring death");
                return;
            }

            var currentCount = Components.LiveMinionIdentities != null ? Components.LiveMinionIdentities.Count : 0;
            ModLog.Info($"HundredDuplicants: Duplicant died! Remaining count: {currentCount}");

            ChallengeSettings.MarkChallengeFailed(challengeId);
            ModLog.Info($"HundredDuplicants: Challenge marked as failed");
            
            ChallengeFailureDialog.Schedule(challengeId);
            ModLog.Info($"HundredDuplicants: Failure dialog scheduled");
        }
    }

    [HarmonyPatch(typeof(MinionIdentity), "OnSpawn")]
    internal static class HundredDuplicants_MinionIdentity_OnSpawn_Patch
    {
        private static void Postfix()
        {
            if (!ChallengeGuards.IsHundredDuplicantsChallengeActive())
            {
                return;
            }

            var challengeId = ChallengeSettings.GetCurrentChallengeLevelId();
            if (ChallengeSettings.IsChallengeCompleted(challengeId) || ChallengeSettings.IsChallengeFailed(challengeId))
            {
                return;
            }

            var currentCount = Components.LiveMinionIdentities != null ? Components.LiveMinionIdentities.Count : 0;
            ModLog.Info($"HundredDuplicants: Duplicant spawned. Current count: {currentCount}");

            if (currentCount >= HundredDuplicantsChallengeModule.TargetDuplicantCount)
            {
                ModLog.Info($"HundredDuplicants: Challenge completed! Total duplicants: {currentCount}");
                ChallengeSettings.MarkChallengeCompleted(challengeId);
                ChallengeVictoryDialog.Schedule(challengeId, HundredDuplicantsChallengeModule.ModuleId);
            }
        }
    }
}
