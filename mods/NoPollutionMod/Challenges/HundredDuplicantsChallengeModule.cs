using STRINGS;
using ModStrings = SlavaMorozov.NoPollutionMod.STRINGS;

namespace SlavaMorozov.NoPollutionMod
{
    internal sealed class HundredDuplicantsChallengeModule : IChallengeModule
    {
        internal const string ModuleId = "HundredDuplicants";
        internal const int TargetDuplicantCount = 100;

        public string Id => ModuleId;
        public LocString Name => ModStrings.NO_POLLUTION_CHALLENGE.LEVELS.HUNDRED_DUPLICANTS.NAME;
        public LocString Tooltip => ModStrings.NO_POLLUTION_CHALLENGE.LEVELS.HUNDRED_DUPLICANTS.TOOLTIP;
        public LocString CompletionText => ModStrings.NO_POLLUTION_CHALLENGE.LEVELS.HUNDRED_DUPLICANTS.COMPLETION;
        public LocString StartText => ModStrings.NO_POLLUTION_CHALLENGE.LEVELS.HUNDRED_DUPLICANTS.START;
        public LocString FailureText => ModStrings.NO_POLLUTION_CHALLENGE.LEVELS.HUNDRED_DUPLICANTS.FAILURE;
        public string RoomDebuffEffectId => string.Empty;

        public void RegisterStrings()
        {
        }

        public void EnsureEffects()
        {
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
