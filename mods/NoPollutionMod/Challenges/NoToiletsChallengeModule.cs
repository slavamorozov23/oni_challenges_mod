using System.Collections.Generic;
using STRINGS;
using ModStrings = SlavaMorozov.NoPollutionMod.STRINGS;

namespace SlavaMorozov.NoPollutionMod
{
    internal sealed class NoToiletsChallengeModule : IChallengeModule
    {
        internal const string ModuleId = "NoToilets";

        internal static readonly HashSet<string> ToiletBuildings = new HashSet<string>
        {
            "Outhouse",
            "FlushToilet",
            "WallToilet",
            "GravitasBathroomStall",
            "GunkEmptier"
        };

        public string Id => ModuleId;
        public LocString Name => ModStrings.NO_POLLUTION_CHALLENGE.LEVELS.NO_TOILETS.NAME;
        public LocString Tooltip => ModStrings.NO_POLLUTION_CHALLENGE.LEVELS.NO_TOILETS.TOOLTIP;
        public LocString CompletionText => ModStrings.NO_POLLUTION_CHALLENGE.LEVELS.NO_TOILETS.COMPLETION;
        public LocString StartText => ModStrings.NO_POLLUTION_CHALLENGE.LEVELS.NO_TOILETS.START;
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
            if (def == null || string.IsNullOrEmpty(def.PrefabID))
            {
                return false;
            }

            return ToiletBuildings.Contains(def.PrefabID);
        }

        public bool ShouldApplyRoomDebuff(Room room)
        {
            return false;
        }
    }
}
