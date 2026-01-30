using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using STRINGS;
using ModStrings = SlavaMorozov.NoPollutionMod.STRINGS;
using Klei.CustomSettings;

namespace SlavaMorozov.NoPollutionMod
{
    internal sealed class DebugOptionsScreen : KScreen
    {
        private const string DebugPrefKey = "NoPollutionMod_DebugMode";
        private const string DebugSettingId = "NoPollutionModDebugMode";
        private const string DebugLevelOff = "Disabled";
        private const string DebugLevelOn = "Enabled";

        private const string VictoryCheckScheduleId = "NoPollutionVictoryCheck";
        private const float VictoryCheckDelay = 1f;
        private const string GrantAllAchievementsScheduleId = "NoPollutionGrantAllAchievements";
        private const float GrantAllAchievementsDelay = 0.05f;
        private static string pendingVictoryChallengeId;
        private static string pendingVictoryAchievementId;
        private static bool victoryDialogScheduled;
        private static bool grantAllAchievementsScheduled;

        internal static void Show(Transform parent, MultiToggle togglePrefab, LocText labelPrefab)
        {
            if (parent == null)
            {
                return;
            }

            var dialogPrefab = ScreenPrefabs.Instance?.InfoDialogScreen;
            if (dialogPrefab == null)
            {
                return;
            }

            var dialog = GameScreenManager.Instance != null
                ? (InfoDialogScreen)GameScreenManager.Instance.StartScreen(dialogPrefab.gameObject, parent.gameObject)
                : Util.KInstantiateUI<InfoDialogScreen>(dialogPrefab.gameObject, parent.gameObject, true);
            if (dialog == null)
            {
                return;
            }
            dialog.gameObject.SetActive(true);
            dialog.SetHeader(ModStrings.UI.DEBUG_OPTIONS_HEADER);

            var checkboxPrefab = GetCheckboxPrefab();
            CustomGameSettingToggleWidget widget = null;
            if (checkboxPrefab != null)
            {
                var checkboxComponent = checkboxPrefab.GetComponent<CustomGameSettingToggleWidget>();
                if (checkboxComponent != null)
                {
                    dialog.AddUI(checkboxComponent, out widget);
                    widget.gameObject.SetActive(true);
                }
            }

            var offLevel = new SettingLevel(DebugLevelOff, ModStrings.UI.DEBUG_MODE_DISABLED, "", 0L, null);
            var onLevel = new SettingLevel(DebugLevelOn, ModStrings.UI.DEBUG_MODE_ENABLED, "", 1L, null);
            var config = new ToggleSettingConfig(
                DebugSettingId,
                ModStrings.UI.DEBUG_MODE_LABEL,
                ModStrings.UI.DEBUG_MODE_TOOLTIP,
                offLevel,
                onLevel,
                DebugLevelOff,
                DebugLevelOff,
                -1L,
                false,
                false,
                null,
                ""
            );

            var initialValue = GetDebugPreference();
            var pendingValue = initialValue;
            var pendingRef = new[] { pendingValue };

            if (widget != null)
            {
                widget.Initialize(
                    config,
                    _ => pendingRef[0] ? onLevel : offLevel,
                    _ =>
                    {
                        pendingRef[0] = !pendingRef[0];
                        return pendingRef[0] ? onLevel : offLevel;
                    }
                );
                widget.Refresh();
            }
            else
            {
                TryAddFallbackToggle(dialog, togglePrefab, labelPrefab, pendingRef);
            }

            dialog.AddOption(false, out KButton cancelButton, out LocText cancelText);
            cancelText.text = UI.CONFIRMDIALOG.CANCEL.text;
            cancelButton.onClick += () =>
            {
                pendingRef[0] = initialValue;
                dialog.Deactivate();
            };

            dialog.AddOption(true, out KButton okButton, out LocText okText);
            okText.text = UI.CONFIRMDIALOG.OK.text;
            okButton.onClick += () =>
            {
                SaveDebugPreference(pendingRef[0]);
                TryGrantAllAchievements();
                dialog.Deactivate();
            };
            SetDialogEscapeCloses(dialog, true);
        }

        internal static void TryGrantAllAchievements()
        {
            if (grantAllAchievementsScheduled)
            {
                Debug.Log("[NoPollutionMod] Grant all achievements already scheduled.");
                return;
            }

            if (SaveGame.Instance == null || SaveGame.Instance.ColonyAchievementTracker == null)
            {
                Debug.LogWarning("[NoPollutionMod] Cannot grant achievements: no save or tracker.");
                return;
            }

            var tracker = SaveGame.Instance.ColonyAchievementTracker;
            var pending = new List<string>();
            foreach (var achievement in Db.Get().ColonyAchievements.resources)
            {
                if (achievement == null || achievement.Disabled || !achievement.IsValidForSave())
                {
                    continue;
                }

                if (tracker.IsAchievementUnlocked(achievement))
                {
                    continue;
                }

                pending.Add(achievement.Id);
            }

            if (pending.Count == 0)
            {
                Debug.Log("[NoPollutionMod] All achievements already unlocked.");
                grantAllAchievementsScheduled = false;
                if (ChallengeGuards.IsAllAchievementsChallengeActive())
                {
                    var challengeId = ChallengeSettings.GetCurrentChallengeLevelId();
                    if (AllAchievementsChallengeModule.AreAllAchievementsComplete())
                    {
                        ModLog.Info($"VictoryCheck: achievements already complete. challenge={challengeId}");
                        ChallengeSettings.MarkChallengeCompleted(challengeId);
                        ChallengeVictoryDialog.Schedule(challengeId, AllAchievementsChallengeModule.ModuleId);
                    }
                }
                return;
            }

            grantAllAchievementsScheduled = true;
            var indexRef = new[] { 0 };
            Debug.Log($"[NoPollutionMod] Granting {pending.Count} achievements...");

            void ScheduleNext()
            {
                if (indexRef[0] >= pending.Count)
                {
                    grantAllAchievementsScheduled = false;
                    Debug.Log("[NoPollutionMod] Grant all achievements complete.");
                    if (ChallengeGuards.IsAllAchievementsChallengeActive())
                    {
                        var challengeId = ChallengeSettings.GetCurrentChallengeLevelId();
                        if (AllAchievementsChallengeModule.AreAllAchievementsComplete())
                        {
                            ModLog.Info($"VictoryCheck: grant all achievements finished. challenge={challengeId}");
                            ChallengeSettings.MarkChallengeCompleted(challengeId);
                            ChallengeVictoryDialog.Schedule(challengeId, AllAchievementsChallengeModule.ModuleId);
                        }
                    }
                    return;
                }

                var id = pending[indexRef[0]++];
                tracker.DebugTriggerAchievement(id);
                var scheduler = GameScheduler.Instance;
                if (scheduler == null)
                {
                    ScheduleNext();
                    return;
                }

                scheduler.Schedule(GrantAllAchievementsScheduleId, GrantAllAchievementsDelay, _ => ScheduleNext());
            }

            ScheduleNext();
        }

        internal static bool IsDebugEnabled()
        {
            return GetDebugPreference();
        }

        private static void TryAddFallbackToggle(InfoDialogScreen dialog, MultiToggle togglePrefab, LocText labelPrefab, bool[] pendingValue)
        {
            var container = GetDialogContentContainer(dialog);
            if (container == null || togglePrefab == null || labelPrefab == null || pendingValue == null || pendingValue.Length == 0)
            {
                return;
            }

            var row = new GameObject("DebugToggleRow", typeof(RectTransform), typeof(HorizontalLayoutGroup));
            row.transform.SetParent(container.transform, false);
            var layout = row.GetComponent<HorizontalLayoutGroup>();
            layout.childAlignment = TextAnchor.MiddleLeft;
            layout.spacing = 10f;
            layout.childForceExpandWidth = false;
            layout.childForceExpandHeight = false;

            var toggle = Util.KInstantiateUI<MultiToggle>(togglePrefab.gameObject, row, true);
            toggle.ChangeState(pendingValue[0] ? 1 : 0);
            toggle.onClick = null;
            toggle.onClick += () =>
            {
                pendingValue[0] = !pendingValue[0];
                toggle.ChangeState(pendingValue[0] ? 1 : 0);
            };

            var label = Util.KInstantiateUI<LocText>(labelPrefab.gameObject, row, true);
            label.text = ModStrings.UI.DEBUG_MODE_LABEL;
        }

        private static GameObject GetDialogContentContainer(InfoDialogScreen dialog)
        {
            if (dialog == null)
            {
                return null;
            }

            var field = typeof(InfoDialogScreen).GetField("contentContainer", BindingFlags.NonPublic | BindingFlags.Instance);
            return field?.GetValue(dialog) as GameObject;
        }

        private static void SaveDebugPreference(bool value)
        {
            KPlayerPrefs.SetString(DebugPrefKey, value ? "true" : "false");
            KPlayerPrefs.Save();
        }

        private static bool GetDebugPreference()
        {
            return KPlayerPrefs.GetString(DebugPrefKey, "false") == "true";
        }

        private static GameObject GetCheckboxPrefab()
        {
            var screen = ScreenPrefabs.Instance?.ColonyDestinationSelectScreen;
            if (screen == null)
            {
                return null;
            }

            var panel = screen.GetComponentInChildren<NewGameSettingsPanel>(true);
            if (panel == null)
            {
                return null;
            }

            var field = typeof(NewGameSettingsPanel).GetField("prefab_checkbox_setting", BindingFlags.NonPublic | BindingFlags.Instance);
            return field?.GetValue(panel) as GameObject;
        }

        private static void SetDialogEscapeCloses(InfoDialogScreen dialog, bool value)
        {
            if (dialog == null)
            {
                return;
            }

            var field = typeof(InfoDialogScreen).GetField("escapeCloses", BindingFlags.NonPublic | BindingFlags.Instance);
            field?.SetValue(dialog, value);
        }

        internal static void TryGrantVictory()
        {
            if (SaveGame.Instance == null || SaveGame.Instance.ColonyAchievementTracker == null)
            {
                Debug.LogWarning("[NoPollutionMod] Cannot grant victory: no save or tracker.");
                return;
            }

            var achievement = Db.Get().ColonyAchievements.ReachedDistantPlanet;
            if (achievement == null)
            {
                Debug.LogWarning("[NoPollutionMod] ReachedDistantPlanet achievement not found.");
                return;
            }

            pendingVictoryChallengeId = ResolveVictoryChallengeId();
            pendingVictoryAchievementId = achievement.Id;
            victoryDialogScheduled = true;

            Debug.Log($"[NoPollutionMod] Victory requested. Challenge={pendingVictoryChallengeId}, Achievement={pendingVictoryAchievementId}");

            if (!string.IsNullOrEmpty(pendingVictoryChallengeId))
            {
                ChallengeSettings.MarkChallengeCompleted(pendingVictoryChallengeId);
            }

            if (SaveGame.Instance.ColonyAchievementTracker.IsAchievementUnlocked(achievement))
            {
                Debug.Log("[NoPollutionMod] Victory already unlocked.");
                ShowVictoryDialog();
                return;
            }

            SaveGame.Instance.ColonyAchievementTracker.DebugTriggerAchievement(achievement.Id);
            GameScheduler.Instance.Schedule("NoPollutionModForceVictory", 0.1f, _ =>
            {
                Game.Instance.Trigger(395452326, null);
            });
            if (IsVictoryCutsceneActive())
            {
                ScheduleVictoryDialogCheck();
            }
            else
            {
                ShowVictoryDialog();
            }
        }

        private static void ScheduleVictoryDialogCheck()
        {
            if (!victoryDialogScheduled)
            {
                return;
            }

            GameScheduler.Instance.Schedule(VictoryCheckScheduleId, VictoryCheckDelay, _ =>
            {
                if (IsVictoryCutsceneActive())
                {
                    ScheduleVictoryDialogCheck();
                    return;
                }

                ShowVictoryDialog();
            });
        }

        private static bool IsVictoryCutsceneActive()
        {
            var videoScreen = UnityEngine.Object.FindObjectOfType<VideoScreen>(true);
            var storyScreen = UnityEngine.Object.FindObjectOfType<StoryMessageScreen>(true);

            var videoActive = videoScreen != null && videoScreen.gameObject.activeInHierarchy;
            var storyActive = storyScreen != null && storyScreen.gameObject.activeInHierarchy;

            return videoActive || storyActive;
        }

        private static void ShowVictoryDialog()
        {
            if (!victoryDialogScheduled)
            {
                return;
            }

            victoryDialogScheduled = false;
            pendingVictoryChallengeId = ResolveVictoryChallengeId();

            if (!string.IsNullOrEmpty(pendingVictoryChallengeId))
            {
                ChallengeSettings.MarkChallengeCompleted(pendingVictoryChallengeId);
            }

            Debug.Log($"[NoPollutionMod] Victory dialog shown. Challenge={pendingVictoryChallengeId}, Achievement={pendingVictoryAchievementId}");

            var canvas = Global.Instance?.globalCanvas;
            var dialogPrefab = ScreenPrefabs.Instance?.InfoDialogScreen;
            if (canvas == null || dialogPrefab == null)
            {
                return;
            }

            var dialog = Util.KInstantiateUI<InfoDialogScreen>(dialogPrefab.gameObject, canvas.gameObject, true);
            if (dialog == null)
            {
                return;
            }

            var achievementName = pendingVictoryAchievementId != null
                ? Db.Get().ColonyAchievements.Get(pendingVictoryAchievementId)?.Name
                : null;
            var challengeName = ChallengeSettings.GetChallengeName(pendingVictoryChallengeId);
            var header = string.IsNullOrEmpty(achievementName)
                ? ModStrings.UI.VICTORY_HEADER.ToString()
                : achievementName;
            dialog.SetHeader(header);
            var completionText = ChallengeSettings.GetCompletionText(pendingVictoryChallengeId);
            if (string.IsNullOrEmpty(completionText))
            {
                completionText = string.Format(ModStrings.NO_POLLUTION_CHALLENGE.COMPLETION_FALLBACK, challengeName);
            }
            dialog.AddPlainText(completionText);
            dialog.AddDefaultOK(true);
        }

        private static string ResolveVictoryChallengeId()
        {
            var levelId = ChallengeSettings.GetCurrentChallengeLevelId();
            if (!string.IsNullOrEmpty(levelId) && levelId != ChallengeSettings.LevelNone)
            {
                return levelId;
            }

            var loader = SaveLoader.Instance;
            if (loader != null && ChallengeSettings.TryGetChallengeForGameInfo(loader.GameInfo, out var storedId))
            {
                return storedId;
            }

            Debug.Log("[NoPollutionMod] ResolveVictoryChallengeId: challenge not found.");
            return levelId;
        }
    }
}
