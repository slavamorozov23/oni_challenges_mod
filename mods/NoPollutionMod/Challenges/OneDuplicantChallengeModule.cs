using STRINGS;
using ModStrings = SlavaMorozov.NoPollutionMod.STRINGS;

namespace SlavaMorozov.NoPollutionMod
{
    internal sealed class OneDuplicantChallengeModule : IChallengeModule
    {
        internal const string ModuleId = "OneDuplicant";

        public string Id => ModuleId;
        public LocString Name => ModStrings.NO_POLLUTION_CHALLENGE.LEVELS.ONE_DUPLICANT.NAME;
        public LocString Tooltip => ModStrings.NO_POLLUTION_CHALLENGE.LEVELS.ONE_DUPLICANT.TOOLTIP;
        public LocString CompletionText => ModStrings.NO_POLLUTION_CHALLENGE.LEVELS.ONE_DUPLICANT.COMPLETION;
        public LocString StartText => ModStrings.NO_POLLUTION_CHALLENGE.LEVELS.ONE_DUPLICANT.START;
        public LocString FailureText => string.Empty;
        public string RoomDebuffEffectId => string.Empty;

        public void RegisterStrings()
        {
            // strings handled via STRINGS + .po localization
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
