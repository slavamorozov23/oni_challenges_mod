using System;
using STRINGS;
using UnityEngine;
using ModStrings = SlavaMorozov.NoPollutionMod.STRINGS;

namespace SlavaMorozov.NoPollutionMod
{
    internal static class ChallengeStartDialog
    {
        private const string StartDialogScheduleId = "NoPollutionStartDialog";
        private const float StartDialogDelay = 1f;
        private static bool pending;
        private static string pendingChallengeId;

        internal static void Schedule(string challengeId)
        {
            if (string.IsNullOrEmpty(challengeId) || challengeId == ChallengeSettings.LevelNone)
            {
                return;
            }

            var startText = ChallengeSettings.GetStartText(challengeId);
            if (string.IsNullOrEmpty(startText))
            {
                return;
            }

            pending = true;
            pendingChallengeId = challengeId;
            ScheduleCheck();
        }

        private static void ScheduleCheck()
        {
            if (!pending)
            {
                return;
            }

            GameScheduler.Instance.Schedule(StartDialogScheduleId, StartDialogDelay, _ =>
            {
                if (IsBlockingScreenActive())
                {
                    ScheduleCheck();
                    return;
                }

                ShowDialog();
            });
        }

        private static bool IsBlockingScreenActive()
        {
            var videoScreen = VideoScreen.Instance ?? UnityEngine.Object.FindObjectOfType<VideoScreen>(true);
            var storyScreen = UnityEngine.Object.FindObjectOfType<StoryMessageScreen>(true);
            var retiredScreen = RetiredColonyInfoScreen.Instance ?? UnityEngine.Object.FindObjectOfType<RetiredColonyInfoScreen>(true);
            return (videoScreen != null && videoScreen.gameObject.activeInHierarchy) ||
                   (storyScreen != null && storyScreen.gameObject.activeInHierarchy) ||
                   (retiredScreen != null && retiredScreen.gameObject.activeInHierarchy);
        }

        private static void ShowDialog()
        {
            if (!pending)
            {
                return;
            }

            pending = false;
            var challengeId = pendingChallengeId;
            pendingChallengeId = null;

            var text = ChallengeSettings.GetStartText(challengeId);
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

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

            var header = ChallengeSettings.GetChallengeName(challengeId);
            dialog.SetHeader(header);
            dialog.AddPlainText(text);
            dialog.AddDefaultOK(true);
            ChallengeSettings.MarkStartSeen(challengeId);
        }
    }

    internal static class ChallengeFailureDialog
    {
        private const string FailureDialogScheduleId = "NoPollutionFailureDialog";
        private const float FailureDialogDelay = 1f;
        private static bool pending;
        private static string pendingChallengeId;

        internal static void Schedule(string challengeId)
        {
            if (string.IsNullOrEmpty(challengeId) || challengeId == ChallengeSettings.LevelNone)
            {
                return;
            }

            var failureText = ChallengeSettings.GetFailureText(challengeId);
            if (string.IsNullOrEmpty(failureText))
            {
                return;
            }

            pending = true;
            pendingChallengeId = challengeId;
            ScheduleCheck();
        }

        private static void ScheduleCheck()
        {
            if (!pending)
            {
                return;
            }

            GameScheduler.Instance.Schedule(FailureDialogScheduleId, FailureDialogDelay, _ =>
            {
                if (IsBlockingScreenActive())
                {
                    ScheduleCheck();
                    return;
                }

                ShowDialog();
            });
        }

        private static bool IsBlockingScreenActive()
        {
            var videoScreen = VideoScreen.Instance ?? UnityEngine.Object.FindObjectOfType<VideoScreen>(true);
            var storyScreen = UnityEngine.Object.FindObjectOfType<StoryMessageScreen>(true);
            var retiredScreen = RetiredColonyInfoScreen.Instance ?? UnityEngine.Object.FindObjectOfType<RetiredColonyInfoScreen>(true);
            return (videoScreen != null && videoScreen.gameObject.activeInHierarchy) ||
                   (storyScreen != null && storyScreen.gameObject.activeInHierarchy) ||
                   (retiredScreen != null && retiredScreen.gameObject.activeInHierarchy);
        }

        private static void ShowDialog()
        {
            if (!pending)
            {
                return;
            }

            pending = false;
            var challengeId = pendingChallengeId;
            pendingChallengeId = null;

            var text = ChallengeSettings.GetFailureText(challengeId);
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

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

            var header = ChallengeSettings.GetChallengeName(challengeId);
            dialog.SetHeader(header);
            dialog.AddPlainText(text);
            dialog.AddDefaultOK(true);
        }
    }

    internal static class ChallengeVictoryDialog
    {
        private const string VictoryDialogScheduleId = "NoPollutionVictoryDialog";
        private const float VictoryDialogDelay = 1f;
        private static bool pending;
        private static string pendingChallengeId;
        private static string pendingAchievementId;
        private static bool hasBlockingState;
        private static bool lastVideoActive;
        private static bool lastStoryActive;
        private static bool lastRetiredActive;

        internal static void Schedule(string challengeId, string achievementId)
        {
            if (string.IsNullOrEmpty(challengeId) || challengeId == ChallengeSettings.LevelNone)
            {
                ModLog.Info($"VictoryDialog.Schedule: invalid challengeId {challengeId}");
                return;
            }

            pending = true;
            pendingChallengeId = challengeId;
            pendingAchievementId = achievementId;
            ModLog.Info($"VictoryDialog.Schedule: pending challenge={challengeId}, achievement={achievementId}");
            ScheduleCheck();
        }

        private static void ScheduleCheck()
        {
            if (!pending)
            {
                return;
            }

            GameScheduler.Instance.Schedule(VictoryDialogScheduleId, VictoryDialogDelay, _ =>
            {
                if (IsBlockingScreenActive())
                {
                    ModLog.Info("VictoryDialog.ScheduleCheck: blocking screen active, reschedule.");
                    ScheduleCheck();
                    return;
                }

                ModLog.Info("VictoryDialog.ScheduleCheck: showing dialog.");
                ShowDialog();
            });
        }

        private static bool IsBlockingScreenActive()
        {
            var videoScreen = VideoScreen.Instance ?? UnityEngine.Object.FindObjectOfType<VideoScreen>(true);
            var storyScreen = UnityEngine.Object.FindObjectOfType<StoryMessageScreen>(true);
            var retiredScreen = RetiredColonyInfoScreen.Instance ?? UnityEngine.Object.FindObjectOfType<RetiredColonyInfoScreen>(true);

            var videoActive = videoScreen != null && videoScreen.gameObject.activeInHierarchy;
            var storyActive = storyScreen != null && storyScreen.gameObject.activeInHierarchy;
            var retiredActive = retiredScreen != null && retiredScreen.gameObject.activeInHierarchy;

            if (!hasBlockingState || videoActive != lastVideoActive || storyActive != lastStoryActive || retiredActive != lastRetiredActive)
            {
                ModLog.Info($"VictoryDialog.Blocking: video={videoActive} story={storyActive} retired={retiredActive}");
                hasBlockingState = true;
                lastVideoActive = videoActive;
                lastStoryActive = storyActive;
                lastRetiredActive = retiredActive;
            }

            return videoActive || storyActive || retiredActive;
        }

        private static void ShowDialog()
        {
            if (!pending)
            {
                return;
            }

            pending = false;

            var canvas = Global.Instance?.globalCanvas;
            var dialogPrefab = ScreenPrefabs.Instance?.InfoDialogScreen;
            if (canvas == null || dialogPrefab == null)
            {
                ModLog.Info("VictoryDialog.Show: missing canvas or dialog prefab.");
                return;
            }

            var dialog = Util.KInstantiateUI<InfoDialogScreen>(dialogPrefab.gameObject, canvas.gameObject, true);
            if (dialog == null)
            {
                ModLog.Info("VictoryDialog.Show: dialog instantiate failed.");
                return;
            }

            var challengeName = ChallengeSettings.GetChallengeName(pendingChallengeId);
            var header = string.IsNullOrEmpty(challengeName)
                ? ModStrings.UI.VICTORY_HEADER.ToString()
                : challengeName;
            dialog.SetHeader(header);
            dialog.AddPlainText(ChallengeSettings.GetCompletionText(pendingChallengeId));
            dialog.AddDefaultOK(true);
            ModLog.Info($"VictoryDialog.Show: shown for challenge={pendingChallengeId}, achievement={pendingAchievementId}");
        }
    }
}
