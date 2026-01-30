using System.Collections.Generic;
using STRINGS;

namespace SlavaMorozov.NoPollutionMod
{
    internal interface IChallengeModule
    {
        string Id { get; }
        LocString Name { get; }
        LocString Tooltip { get; }
        LocString CompletionText { get; }
        LocString StartText { get; }
        LocString FailureText { get; }
        string RoomDebuffEffectId { get; }

        void RegisterStrings();
        void EnsureEffects();
        bool IsBuildingBlocked(BuildingDef def);
        bool ShouldApplyRoomDebuff(Room room);
    }

    internal static class ChallengeModuleManager
    {
        private static readonly List<IChallengeModule> Modules = new List<IChallengeModule>
        {
            new NoPollutionChallengeModule(),
            new NoToiletsChallengeModule(),
            new OneDuplicantChallengeModule(),
            new AllAchievementsChallengeModule(),
            new HundredDuplicantsChallengeModule()
        };

        internal static IEnumerable<IChallengeModule> GetModules() => Modules;

        internal static bool TryGetModule(string id, out IChallengeModule module)
        {
            for (int i = 0; i < Modules.Count; i++)
            {
                if (Modules[i].Id == id)
                {
                    module = Modules[i];
                    return true;
                }
            }

            module = null;
            return false;
        }

        internal static IChallengeModule GetActiveModule()
        {
            var id = ChallengeSettings.GetCurrentChallengeLevelId();
            return TryGetModule(id, out var module) ? module : null;
        }

        internal static void RegisterStrings()
        {
            for (int i = 0; i < Modules.Count; i++)
            {
                Modules[i].RegisterStrings();
            }
        }
    }

}
