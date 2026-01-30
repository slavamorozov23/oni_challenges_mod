using Database;
using STRINGS;
using ModStrings = SlavaMorozov.NoPollutionMod.STRINGS;

namespace SlavaMorozov.NoPollutionMod
{
    internal sealed class AllAchievementsChallengeModule : IChallengeModule
    {
        internal const string ModuleId = "AllAchievements";
        internal const int CycleLimit = 800;

        public string Id => ModuleId;
        public LocString Name => ModStrings.NO_POLLUTION_CHALLENGE.LEVELS.ALL_ACHIEVEMENTS.NAME;
        public LocString Tooltip => ModStrings.NO_POLLUTION_CHALLENGE.LEVELS.ALL_ACHIEVEMENTS.TOOLTIP;
        public LocString CompletionText => ModStrings.NO_POLLUTION_CHALLENGE.LEVELS.ALL_ACHIEVEMENTS.COMPLETION;
        public LocString StartText => ModStrings.NO_POLLUTION_CHALLENGE.LEVELS.ALL_ACHIEVEMENTS.START;
        public LocString FailureText => ModStrings.NO_POLLUTION_CHALLENGE.LEVELS.ALL_ACHIEVEMENTS.FAILURE;
        public string RoomDebuffEffectId => string.Empty;

        public void RegisterStrings()
        {
            // strings handled via STRINGS + .po localization
        }

        public void EnsureEffects()
        {
        }

        internal static bool AreAllAchievementsComplete()
        {
            if (SaveGame.Instance == null || SaveGame.Instance.ColonyAchievementTracker == null)
            {
                return false;
            }

            if (GameUtil.GetCurrentCycle() > CycleLimit)
            {
                return false;
            }

            var tracker = SaveGame.Instance.ColonyAchievementTracker;
            foreach (var achievement in Db.Get().ColonyAchievements.resources)
            {
                if (achievement == null || achievement.Disabled)
                {
                    continue;
                }

                if (!achievement.IsValidForSave())
                {
                    continue;
                }

                if (!DlcManager.IsCorrectDlcSubscribed(achievement.requiredDlcIds, achievement.forbiddenDlcIds))
                {
                    continue;
                }

                if (!tracker.IsAchievementUnlocked(achievement))
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsBuildingBlocked(BuildingDef def)
        {
            return false;
        }

        public bool ShouldApplyRoomDebuff(Room room)
        {
            return false;
        }
    }
}
