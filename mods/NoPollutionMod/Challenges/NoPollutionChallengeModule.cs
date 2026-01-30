using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;
using ModStrings = SlavaMorozov.NoPollutionMod.STRINGS;

namespace SlavaMorozov.NoPollutionMod
{
    internal sealed class NoPollutionChallengeModule : IChallengeModule
    {
        internal const string ModuleId = "NoPollution";

        private const string DlcBaseId = "BASE";
        private const string DlcExpansion1Id = "EXPANSION1_ID";
        private const string DlcFrostyId = "DLC2_ID";
        private const string DlcBionicId = "DLC3_ID";
        private const string DlcPrehistoricId = "DLC4_ID";

        public string Id => ModuleId;
        public LocString Name => ModStrings.NO_POLLUTION_CHALLENGE.LEVELS.NO_POLLUTION.NAME;
        public LocString Tooltip => ModStrings.NO_POLLUTION_CHALLENGE.LEVELS.NO_POLLUTION.TOOLTIP;
        public LocString CompletionText => ModStrings.NO_POLLUTION_CHALLENGE.LEVELS.NO_POLLUTION.COMPLETION;
        public LocString StartText => ModStrings.NO_POLLUTION_CHALLENGE.LEVELS.NO_POLLUTION.START;
        public LocString FailureText => string.Empty;
        public string RoomDebuffEffectId => "NoPollution_PollutedSurroundings";

        private static readonly Dictionary<string, HashSet<string>> BlockedBuildingsByDlc =
            new Dictionary<string, HashSet<string>>
            {
                {
                    DlcBaseId,
                    new HashSet<string>
                    {
                        "Generator",
                        "WoodGasGenerator",
                        "MethaneGenerator",
                        "PetroleumGenerator",
                        "EthanolDistillery",
                        "Polymerizer",
                        "GourmetCookingStation",
                        "GlassForge",
                        "FarmStation",
                        "FertilizerMaker",
                        "OilRefinery",
                        "DiamondPress",
                        "Smoker",
                        "CO2Scrubber",
                        "Electrolyzer",
                        "RustDeoxidizer",
                        "MineralDeoxidizer",
                        "SublimationStation",
                        "AirFilter",
                        "MissileFabricator",
                        "JetSuitLocker",
                        "PowerControlStation",
                        "GeneticAnalysisStation"
                    }
                },
                {
                    DlcExpansion1Id,
                    new HashSet<string>
                    {
                        "RadiationLight",
                        "NuclearReactor",
                        "UraniumCentrifuge",
                        "HEPEngine",
                        "KeroseneEngineClusterSmall",
                        "KeroseneEngineCluster",
                        "SolidBooster",
                        "CO2Engine",
                        "NoseconeHarvest",
                        "BiodieselEngineCluster"
                    }
                },
                { DlcFrostyId, new HashSet<string> { "Campfire", "IceKettle" } },
                { DlcBionicId, new HashSet<string>() },
                { DlcPrehistoricId, new HashSet<string> { "PeatGenerator" } }
            };

        public void RegisterStrings()
        {
            // strings handled via STRINGS + .po localization
        }

        public void EnsureEffects()
        {
            var effects = Db.Get().effects;
            if (effects == null || effects.Exists(RoomDebuffEffectId))
            {
                return;
            }

            var name = ModStrings.DUPLICANTS.MODIFIERS.NOPOLLUTION_POLLUTED.NAME;
            var tooltip = ModStrings.DUPLICANTS.MODIFIERS.NOPOLLUTION_POLLUTED.TOOLTIP;
            var effect = new Effect(RoomDebuffEffectId, name, tooltip, 0f, true, true, true, null, -1f, 0f, null, "status_item_exclamation");
            effect.Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, 0.1666667f, name, false, false, true));
            effects.Add(effect);
            ModLog.Info("NoPollutionChallengeModule: effect registered");
        }

        public bool IsBuildingBlocked(BuildingDef def)
        {
            if (def == null || string.IsNullOrEmpty(def.PrefabID))
            {
                return false;
            }

            var prefabId = def.PrefabID;
            return IsBlockedForDlc(DlcBaseId, prefabId) ||
                   (Game.IsDlcActiveForCurrentSave(DlcExpansion1Id) && IsBlockedForDlc(DlcExpansion1Id, prefabId)) ||
                   (Game.IsDlcActiveForCurrentSave(DlcFrostyId) && IsBlockedForDlc(DlcFrostyId, prefabId)) ||
                   (Game.IsDlcActiveForCurrentSave(DlcBionicId) && IsBlockedForDlc(DlcBionicId, prefabId)) ||
                   (Game.IsDlcActiveForCurrentSave(DlcPrehistoricId) && IsBlockedForDlc(DlcPrehistoricId, prefabId));
        }

        public bool ShouldApplyRoomDebuff(Room room)
        {
            if (room == null || room.buildings == null)
            {
                return false;
            }

            for (int i = 0; i < room.buildings.Count; i++)
            {
                var building = room.buildings[i];
                if (building == null)
                {
                    continue;
                }

                var def = building.GetComponent<Building>()?.Def;
                if (IsBuildingBlocked(def))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsBlockedForDlc(string dlcId, string prefabId)
        {
            if (!BlockedBuildingsByDlc.TryGetValue(dlcId, out var set))
            {
                return false;
            }

            return set.Contains(prefabId);
        }
    }
}
