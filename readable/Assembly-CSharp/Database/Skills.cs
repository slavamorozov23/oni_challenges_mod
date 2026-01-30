using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x02000FB3 RID: 4019
	public class Skills : ResourceSet<Skill>
	{
		// Token: 0x06007E46 RID: 32326 RVA: 0x0032464C File Offset: 0x0032284C
		public Skills(ResourceSet parent) : base("Skills", parent)
		{
			this.Mining1 = this.AddSkill(new Skill("Mining1", DUPLICANTS.ROLES.JUNIOR_MINER.NAME, DUPLICANTS.ROLES.JUNIOR_MINER.DESCRIPTION, 0, "hat_role_mining1", "skillbadge_role_mining1", Db.Get().SkillGroups.Mining.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseDigSpeedSmall,
				Db.Get().SkillPerks.CanDigVeryFirm
			}, null, "Minion", null, null));
			this.Mining2 = this.AddSkill(new Skill("Mining2", DUPLICANTS.ROLES.MINER.NAME, DUPLICANTS.ROLES.MINER.DESCRIPTION, 1, "hat_role_mining2", "skillbadge_role_mining2", Db.Get().SkillGroups.Mining.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseDigSpeedMedium,
				Db.Get().SkillPerks.CanDigNearlyImpenetrable
			}, new List<string>
			{
				this.Mining1.Id
			}, "Minion", null, null));
			this.Mining3 = this.AddSkill(new Skill("Mining3", DUPLICANTS.ROLES.SENIOR_MINER.NAME, DUPLICANTS.ROLES.SENIOR_MINER.DESCRIPTION, 2, "hat_role_mining3", "skillbadge_role_mining3", Db.Get().SkillGroups.Mining.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseDigSpeedLarge,
				Db.Get().SkillPerks.CanDigSuperDuperHard
			}, new List<string>
			{
				this.Mining2.Id
			}, "Minion", null, null));
			this.Mining4 = this.AddSkill(new Skill("Mining4", DUPLICANTS.ROLES.MASTER_MINER.NAME, DUPLICANTS.ROLES.MASTER_MINER.DESCRIPTION, 3, "hat_role_mining4", "skillbadge_role_mining4", Db.Get().SkillGroups.Mining.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.CanDigRadioactiveMaterials
			}, new List<string>
			{
				this.Mining3.Id
			}, "Minion", DlcManager.EXPANSION1, null));
			this.Building1 = this.AddSkill(new Skill("Building1", DUPLICANTS.ROLES.JUNIOR_BUILDER.NAME, DUPLICANTS.ROLES.JUNIOR_BUILDER.DESCRIPTION, 0, "hat_role_building1", "skillbadge_role_building1", Db.Get().SkillGroups.Building.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseConstructionSmall
			}, null, "Minion", null, null));
			this.Building2 = this.AddSkill(new Skill("Building2", DUPLICANTS.ROLES.BUILDER.NAME, DUPLICANTS.ROLES.BUILDER.DESCRIPTION, 1, "hat_role_building2", "skillbadge_role_building2", Db.Get().SkillGroups.Building.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseConstructionMedium
			}, new List<string>
			{
				this.Building1.Id
			}, "Minion", null, null));
			this.Building3 = this.AddSkill(new Skill("Building3", DUPLICANTS.ROLES.SENIOR_BUILDER.NAME, DUPLICANTS.ROLES.SENIOR_BUILDER.DESCRIPTION, 2, "hat_role_building3", "skillbadge_role_building3", Db.Get().SkillGroups.Building.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseConstructionLarge,
				Db.Get().SkillPerks.CanDemolish
			}, new List<string>
			{
				this.Building2.Id
			}, "Minion", null, null));
			this.Farming1 = this.AddSkill(new Skill("Farming1", DUPLICANTS.ROLES.JUNIOR_FARMER.NAME, DUPLICANTS.ROLES.JUNIOR_FARMER.DESCRIPTION, 0, "hat_role_farming1", "skillbadge_role_farming1", Db.Get().SkillGroups.Farming.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseBotanySmall
			}, null, "Minion", null, null));
			this.Farming2 = this.AddSkill(new Skill("Farming2", DUPLICANTS.ROLES.FARMER.NAME, DUPLICANTS.ROLES.FARMER.DESCRIPTION, 1, "hat_role_farming2", "skillbadge_role_farming2", Db.Get().SkillGroups.Farming.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseBotanyMedium,
				Db.Get().SkillPerks.CanFarmTinker,
				Db.Get().SkillPerks.CanFarmStation
			}, new List<string>
			{
				this.Farming1.Id
			}, "Minion", null, null));
			this.Farming3 = this.AddSkill(new Skill("Farming3", DUPLICANTS.ROLES.SENIOR_FARMER.NAME, DUPLICANTS.ROLES.SENIOR_FARMER.DESCRIPTION, 2, "hat_role_farming3", "skillbadge_role_farming3", Db.Get().SkillGroups.Farming.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseBotanyLarge,
				Db.Get().SkillPerks.CanSalvagePlantFiber
			}, new List<string>
			{
				this.Farming2.Id
			}, "Minion", null, null));
			if (DlcManager.FeaturePlantMutationsEnabled())
			{
				this.Farming3.perks.Add(Db.Get().SkillPerks.CanIdentifyMutantSeeds);
			}
			this.Ranching1 = this.AddSkill(new Skill("Ranching1", DUPLICANTS.ROLES.RANCHER.NAME, DUPLICANTS.ROLES.RANCHER.DESCRIPTION, 1, "hat_role_rancher1", "skillbadge_role_rancher1", Db.Get().SkillGroups.Ranching.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseRanchingSmall,
				Db.Get().SkillPerks.CanUseRanchStation,
				Db.Get().SkillPerks.CanWrangleCreatures
			}, new List<string>
			{
				this.Farming1.Id
			}, "Minion", null, null));
			this.Ranching2 = this.AddSkill(new Skill("Ranching2", DUPLICANTS.ROLES.SENIOR_RANCHER.NAME, DUPLICANTS.ROLES.SENIOR_RANCHER.DESCRIPTION, 2, "hat_role_rancher2", "skillbadge_role_rancher2", Db.Get().SkillGroups.Ranching.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseRanchingMedium,
				Db.Get().SkillPerks.CanUseMilkingStation
			}, new List<string>
			{
				this.Ranching1.Id
			}, "Minion", null, null));
			this.Researching1 = this.AddSkill(new Skill("Researching1", DUPLICANTS.ROLES.JUNIOR_RESEARCHER.NAME, DUPLICANTS.ROLES.JUNIOR_RESEARCHER.DESCRIPTION, 0, "hat_role_research1", "skillbadge_role_research1", Db.Get().SkillGroups.Research.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseLearningSmall,
				Db.Get().SkillPerks.AllowAdvancedResearch,
				Db.Get().SkillPerks.AllowChemistry
			}, null, "Minion", null, null));
			this.Researching2 = this.AddSkill(new Skill("Researching2", DUPLICANTS.ROLES.RESEARCHER.NAME, DUPLICANTS.ROLES.RESEARCHER.DESCRIPTION, 1, "hat_role_research2", "skillbadge_role_research2", Db.Get().SkillGroups.Research.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseLearningMedium,
				Db.Get().SkillPerks.CanStudyWorldObjects,
				Db.Get().SkillPerks.AllowGeyserTuning
			}, new List<string>
			{
				this.Researching1.Id
			}, "Minion", null, null));
			this.AtomicResearch = this.AddSkill(new Skill("AtomicResearch", DUPLICANTS.ROLES.NUCLEAR_RESEARCHER.NAME, DUPLICANTS.ROLES.NUCLEAR_RESEARCHER.DESCRIPTION, 2, "hat_role_research5", "skillbadge_role_research3", Db.Get().SkillGroups.Research.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseLearningLarge,
				Db.Get().SkillPerks.AllowNuclearResearch
			}, new List<string>
			{
				this.Researching2.Id
			}, "Minion", DlcManager.EXPANSION1, null));
			this.Researching4 = this.AddSkill(new Skill("Researching4", DUPLICANTS.ROLES.NUCLEAR_RESEARCHER.NAME, DUPLICANTS.ROLES.NUCLEAR_RESEARCHER.DESCRIPTION, 2, "hat_role_research4", "skillbadge_role_research3", Db.Get().SkillGroups.Research.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseLearningLarge,
				Db.Get().SkillPerks.AllowNuclearResearch
			}, new List<string>
			{
				this.Researching2.Id
			}, "Minion", DlcManager.EXPANSION1, null));
			this.Researching4.deprecated = true;
			this.Researching3 = this.AddSkill(new Skill("Researching3", DUPLICANTS.ROLES.SENIOR_RESEARCHER.NAME, DUPLICANTS.ROLES.SENIOR_RESEARCHER.DESCRIPTION, 2, "hat_role_research3", "skillbadge_role_research3", Db.Get().SkillGroups.Research.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseLearningLarge,
				Db.Get().SkillPerks.AllowInterstellarResearch,
				Db.Get().SkillPerks.CanMissionControl
			}, new List<string>
			{
				this.Researching2.Id
			}, "Minion", null, null));
			this.Researching3.deprecated = DlcManager.IsExpansion1Active();
			this.Astronomy = this.AddSkill(new Skill("Astronomy", DUPLICANTS.ROLES.SENIOR_RESEARCHER.NAME, DUPLICANTS.ROLES.SENIOR_RESEARCHER.DESCRIPTION, 1, "hat_role_research3", "skillbadge_role_research2", Db.Get().SkillGroups.Research.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.CanUseClusterTelescope,
				Db.Get().SkillPerks.CanUseClusterTelescopeEnclosed,
				Db.Get().SkillPerks.CanMissionControl
			}, new List<string>
			{
				this.Researching1.Id
			}, "Minion", DlcManager.EXPANSION1, null));
			this.SpaceResearch = this.AddSkill(new Skill("SpaceResearch", DUPLICANTS.ROLES.SPACE_RESEARCHER.NAME, DUPLICANTS.ROLES.SPACE_RESEARCHER.DESCRIPTION, 2, "hat_role_research4", "skillbadge_role_research3", Db.Get().SkillGroups.Research.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseLearningLargeSpace,
				Db.Get().SkillPerks.AllowOrbitalResearch
			}, new List<string>
			{
				this.Astronomy.Id
			}, "Minion", DlcManager.EXPANSION1, null));
			if (DlcManager.IsExpansion1Active())
			{
				this.RocketPiloting1 = this.AddSkill(new Skill("RocketPiloting1", DUPLICANTS.ROLES.ROCKETPILOT.NAME, DUPLICANTS.ROLES.ROCKETPILOT.DESCRIPTION, 0, "hat_role_astronaut1", "skillbadge_role_rocketry1", Db.Get().SkillGroups.Rocketry.Id, new List<SkillPerk>
				{
					Db.Get().SkillPerks.CanUseRocketControlStation
				}, new List<string>(), "Minion", DlcManager.EXPANSION1, null));
				this.RocketPiloting2 = this.AddSkill(new Skill("RocketPiloting2", DUPLICANTS.ROLES.SENIOR_ROCKETPILOT.NAME, DUPLICANTS.ROLES.SENIOR_ROCKETPILOT.DESCRIPTION, 2, "hat_role_astronaut2", "skillbadge_role_rocketry3", Db.Get().SkillGroups.Rocketry.Id, new List<SkillPerk>
				{
					Db.Get().SkillPerks.IncreaseRocketSpeedSmall
				}, new List<string>
				{
					this.RocketPiloting1.Id,
					this.Astronomy.Id
				}, "Minion", DlcManager.EXPANSION1, null));
			}
			this.Cooking1 = this.AddSkill(new Skill("Cooking1", DUPLICANTS.ROLES.JUNIOR_COOK.NAME, DUPLICANTS.ROLES.JUNIOR_COOK.DESCRIPTION, 0, "hat_role_cooking1", "skillbadge_role_cooking1", Db.Get().SkillGroups.Cooking.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseCookingSmall,
				Db.Get().SkillPerks.CanElectricGrill,
				Db.Get().SkillPerks.CanGasRange,
				Db.Get().SkillPerks.CanDeepFry
			}, null, "Minion", null, null));
			this.Cooking2 = this.AddSkill(new Skill("Cooking2", DUPLICANTS.ROLES.COOK.NAME, DUPLICANTS.ROLES.COOK.DESCRIPTION, 1, "hat_role_cooking2", "skillbadge_role_cooking2", Db.Get().SkillGroups.Cooking.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseCookingMedium,
				Db.Get().SkillPerks.CanSpiceGrinder
			}, new List<string>
			{
				this.Cooking1.Id
			}, "Minion", null, null));
			this.Arting1 = this.AddSkill(new Skill("Arting1", DUPLICANTS.ROLES.JUNIOR_ARTIST.NAME, DUPLICANTS.ROLES.JUNIOR_ARTIST.DESCRIPTION, 0, "hat_role_art1", "skillbadge_role_art1", Db.Get().SkillGroups.Art.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseArtSmall,
				Db.Get().SkillPerks.CanArtUgly,
				Db.Get().SkillPerks.CanArt
			}, null, "Minion", null, null));
			this.Arting2 = this.AddSkill(new Skill("Arting2", DUPLICANTS.ROLES.ARTIST.NAME, DUPLICANTS.ROLES.ARTIST.DESCRIPTION, 1, "hat_role_art2", "skillbadge_role_art2", Db.Get().SkillGroups.Art.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseArtMedium,
				Db.Get().SkillPerks.CanArtOkay,
				Db.Get().SkillPerks.CanClothingAlteration
			}, new List<string>
			{
				this.Arting1.Id
			}, "Minion", null, null));
			if (DlcManager.FeatureClusterSpaceEnabled())
			{
				this.Arting2.perks.Add(Db.Get().SkillPerks.CanStudyArtifact);
			}
			this.Arting3 = this.AddSkill(new Skill("Arting3", DUPLICANTS.ROLES.MASTER_ARTIST.NAME, DUPLICANTS.ROLES.MASTER_ARTIST.DESCRIPTION, 2, "hat_role_art3", "skillbadge_role_art3", Db.Get().SkillGroups.Art.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseArtLarge,
				Db.Get().SkillPerks.CanArtGreat
			}, new List<string>
			{
				this.Arting2.Id
			}, "Minion", null, null));
			this.Hauling1 = this.AddSkill(new Skill("Hauling1", DUPLICANTS.ROLES.HAULER.NAME, DUPLICANTS.ROLES.HAULER.DESCRIPTION, 0, "hat_role_hauling1", "skillbadge_role_hauling1", Db.Get().SkillGroups.Hauling.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseStrengthGofer,
				Db.Get().SkillPerks.IncreaseCarryAmountSmall
			}, null, "Minion", null, null));
			this.Hauling2 = this.AddSkill(new Skill("Hauling2", DUPLICANTS.ROLES.MATERIALS_MANAGER.NAME, DUPLICANTS.ROLES.MATERIALS_MANAGER.DESCRIPTION, 1, "hat_role_hauling2", "skillbadge_role_hauling2", Db.Get().SkillGroups.Hauling.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseStrengthCourier,
				Db.Get().SkillPerks.IncreaseCarryAmountMedium
			}, new List<string>
			{
				this.Hauling1.Id
			}, "Minion", null, null));
			if (DlcManager.IsExpansion1Active())
			{
				this.ThermalSuits = this.AddSkill(new Skill("ThermalSuits", DUPLICANTS.ROLES.SUIT_DURABILITY.NAME, DUPLICANTS.ROLES.SUIT_DURABILITY.DESCRIPTION, 1, "hat_role_suits1", "skillbadge_role_suits2", Db.Get().SkillGroups.Suits.Id, new List<SkillPerk>
				{
					Db.Get().SkillPerks.IncreaseAthleticsLarge,
					Db.Get().SkillPerks.ExosuitDurability
				}, new List<string>
				{
					this.Hauling1.Id,
					this.RocketPiloting1.Id
				}, "Minion", DlcManager.EXPANSION1, null));
			}
			else
			{
				this.ThermalSuits = this.AddSkill(new Skill("ThermalSuits", DUPLICANTS.ROLES.SUIT_DURABILITY.NAME, DUPLICANTS.ROLES.SUIT_DURABILITY.DESCRIPTION, 1, "hat_role_suits1", "skillbadge_role_suits2", Db.Get().SkillGroups.Suits.Id, new List<SkillPerk>
				{
					Db.Get().SkillPerks.IncreaseAthleticsLarge,
					Db.Get().SkillPerks.ExosuitDurability
				}, new List<string>
				{
					this.Hauling1.Id
				}, "Minion", null, null));
			}
			this.Suits1 = this.AddSkill(new Skill("Suits1", DUPLICANTS.ROLES.SUIT_EXPERT.NAME, DUPLICANTS.ROLES.SUIT_EXPERT.DESCRIPTION, 2, "hat_role_suits2", "skillbadge_role_suits3", Db.Get().SkillGroups.Suits.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseAthleticsMedium,
				Db.Get().SkillPerks.ExosuitExpertise
			}, new List<string>
			{
				this.ThermalSuits.Id
			}, "Minion", null, null));
			this.Technicals1 = this.AddSkill(new Skill("Technicals1", DUPLICANTS.ROLES.MACHINE_TECHNICIAN.NAME, DUPLICANTS.ROLES.MACHINE_TECHNICIAN.DESCRIPTION, 0, "hat_role_technicals1", "skillbadge_role_technicals1", Db.Get().SkillGroups.Technicals.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseMachinerySmall
			}, null, "Minion", null, null));
			this.Technicals2 = this.AddSkill(new Skill("Technicals2", DUPLICANTS.ROLES.POWER_TECHNICIAN.NAME, DUPLICANTS.ROLES.POWER_TECHNICIAN.DESCRIPTION, 1, "hat_role_technicals2", "skillbadge_role_technicals2", Db.Get().SkillGroups.Technicals.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseMachineryMedium,
				Db.Get().SkillPerks.CanPowerTinker,
				Db.Get().SkillPerks.CanCraftElectronics
			}, new List<string>
			{
				this.Technicals1.Id
			}, "Minion", null, null));
			this.Engineering1 = this.AddSkill(new Skill("Engineering1", DUPLICANTS.ROLES.MECHATRONIC_ENGINEER.NAME, DUPLICANTS.ROLES.MECHATRONIC_ENGINEER.DESCRIPTION, 2, "hat_role_engineering1", "skillbadge_role_engineering1", Db.Get().SkillGroups.Technicals.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseMachineryLarge,
				Db.Get().SkillPerks.IncreaseConstructionMechatronics,
				Db.Get().SkillPerks.ConveyorBuild
			}, new List<string>
			{
				this.Hauling2.Id,
				this.Technicals2.Id
			}, "Minion", null, null));
			this.Basekeeping1 = this.AddSkill(new Skill("Basekeeping1", DUPLICANTS.ROLES.HANDYMAN.NAME, DUPLICANTS.ROLES.HANDYMAN.DESCRIPTION, 0, "hat_role_basekeeping1", "skillbadge_role_basekeeping1", Db.Get().SkillGroups.Basekeeping.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseStrengthGroundskeeper
			}, null, "Minion", null, null));
			this.Basekeeping2 = this.AddSkill(new Skill("Basekeeping2", DUPLICANTS.ROLES.PLUMBER.NAME, DUPLICANTS.ROLES.PLUMBER.DESCRIPTION, 1, "hat_role_basekeeping2", "skillbadge_role_basekeeping2", Db.Get().SkillGroups.Basekeeping.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseStrengthPlumber,
				Db.Get().SkillPerks.CanDoPlumbing
			}, new List<string>
			{
				this.Basekeeping1.Id
			}, "Minion", null, null));
			this.Pyrotechnics = this.AddSkill(new Skill("Pyrotechnics", DUPLICANTS.ROLES.PYROTECHNIC.NAME, DUPLICANTS.ROLES.PYROTECHNIC.DESCRIPTION, 2, "hat_role_pyrotechnics", "skillbadge_role_basekeeping3", Db.Get().SkillGroups.Basekeeping.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.CanMakeMissiles
			}, new List<string>
			{
				this.Basekeeping2.Id
			}, "Minion", null, null));
			if (DlcManager.IsExpansion1Active())
			{
				this.Astronauting1 = this.AddSkill(new Skill("Astronauting1", DUPLICANTS.ROLES.USELESSSKILL.NAME, DUPLICANTS.ROLES.USELESSSKILL.DESCRIPTION, 3, "hat_role_astronaut1", "skillbadge_role_astronaut1", Db.Get().SkillGroups.Suits.Id, new List<SkillPerk>
				{
					Db.Get().SkillPerks.IncreaseAthleticsMedium
				}, new List<string>
				{
					this.Researching3.Id,
					this.Suits1.Id
				}, "Minion", DlcManager.EXPANSION1, null));
				this.Astronauting1.deprecated = true;
				this.Astronauting2 = this.AddSkill(new Skill("Astronauting2", DUPLICANTS.ROLES.USELESSSKILL.NAME, DUPLICANTS.ROLES.USELESSSKILL.DESCRIPTION, 4, "hat_role_astronaut2", "skillbadge_role_astronaut2", Db.Get().SkillGroups.Suits.Id, new List<SkillPerk>
				{
					Db.Get().SkillPerks.IncreaseAthleticsMedium
				}, new List<string>
				{
					this.Astronauting1.Id
				}, "Minion", DlcManager.EXPANSION1, null));
				this.Astronauting2.deprecated = true;
			}
			else
			{
				this.Astronauting1 = this.AddSkill(new Skill("Astronauting1", DUPLICANTS.ROLES.ASTRONAUTTRAINEE.NAME, DUPLICANTS.ROLES.ASTRONAUTTRAINEE.DESCRIPTION, 3, "hat_role_astronaut1", "skillbadge_role_astronaut1", Db.Get().SkillGroups.Suits.Id, new List<SkillPerk>
				{
					Db.Get().SkillPerks.CanUseRockets
				}, new List<string>
				{
					this.Researching3.Id,
					this.Suits1.Id
				}, "Minion", null, null));
				this.Astronauting2 = this.AddSkill(new Skill("Astronauting2", DUPLICANTS.ROLES.ASTRONAUT.NAME, DUPLICANTS.ROLES.ASTRONAUT.DESCRIPTION, 4, "hat_role_astronaut2", "skillbadge_role_astronaut2", Db.Get().SkillGroups.Suits.Id, new List<SkillPerk>
				{
					Db.Get().SkillPerks.FasterSpaceFlight
				}, new List<string>
				{
					this.Astronauting1.Id
				}, "Minion", null, null));
			}
			this.Medicine1 = this.AddSkill(new Skill("Medicine1", DUPLICANTS.ROLES.JUNIOR_MEDIC.NAME, DUPLICANTS.ROLES.JUNIOR_MEDIC.DESCRIPTION, 0, "hat_role_medicalaid1", "skillbadge_role_medicalaid1", Db.Get().SkillGroups.MedicalAid.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseCaringSmall,
				Db.Get().SkillPerks.CanCompound
			}, null, "Minion", null, null));
			this.Medicine2 = this.AddSkill(new Skill("Medicine2", DUPLICANTS.ROLES.MEDIC.NAME, DUPLICANTS.ROLES.MEDIC.DESCRIPTION, 1, "hat_role_medicalaid2", "skillbadge_role_medicalaid2", Db.Get().SkillGroups.MedicalAid.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseCaringMedium,
				Db.Get().SkillPerks.CanDoctor
			}, new List<string>
			{
				this.Medicine1.Id
			}, "Minion", null, null));
			this.Medicine3 = this.AddSkill(new Skill("Medicine3", DUPLICANTS.ROLES.SENIOR_MEDIC.NAME, DUPLICANTS.ROLES.SENIOR_MEDIC.DESCRIPTION, 2, "hat_role_medicalaid3", "skillbadge_role_medicalaid3", Db.Get().SkillGroups.MedicalAid.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseCaringLarge,
				Db.Get().SkillPerks.CanAdvancedMedicine
			}, new List<string>
			{
				this.Medicine2.Id
			}, "Minion", null, null));
			if (DlcManager.IsContentSubscribed("DLC3_ID"))
			{
				this.BionicsA1 = this.AddSkill(new Skill("BionicsA1", DUPLICANTS.ROLES.BIONICS_A1.NAME, DUPLICANTS.ROLES.BIONICS_A1.DESCRIPTION, 0, "hat_role_gainingboosters1", "skillbadge_bionic_booster1", Db.Get().SkillGroups.BionicSkills.Id, new List<SkillPerk>
				{
					Db.Get().SkillPerks.ExtraBionicBooster1
				}, new List<string>(), GameTags.Minions.Models.Bionic.Name, DlcManager.DLC3, null));
				this.BionicsA2 = this.AddSkill(new Skill("BionicsA2", DUPLICANTS.ROLES.BIONICS_A2.NAME, DUPLICANTS.ROLES.BIONICS_A2.DESCRIPTION, 1, "hat_role_gainingboosters2", "skillbadge_bionic_booster2", Db.Get().SkillGroups.BionicSkills.Id, new List<SkillPerk>
				{
					Db.Get().SkillPerks.ExtraBionicBooster2,
					Db.Get().SkillPerks.IncreaseAthleticsBionicsA2
				}, new List<string>
				{
					this.BionicsA1.Id
				}, GameTags.Minions.Models.Bionic.Name, DlcManager.DLC3, null));
				this.BionicsA3 = this.AddSkill(new Skill("BionicsA3", DUPLICANTS.ROLES.BIONICS_A3.NAME, DUPLICANTS.ROLES.BIONICS_A3.DESCRIPTION, 2, "hat_role_gainingboosters3", "skillbadge_bionic_booster3", Db.Get().SkillGroups.BionicSkills.Id, new List<SkillPerk>
				{
					Db.Get().SkillPerks.ExtraBionicBooster3,
					Db.Get().SkillPerks.ExosuitExpertise
				}, new List<string>
				{
					this.BionicsA2.Id
				}, GameTags.Minions.Models.Bionic.Name, DlcManager.DLC3, null));
				this.BionicsB1 = this.AddSkill(new Skill("BionicsB1", DUPLICANTS.ROLES.BIONICS_B1.NAME, DUPLICANTS.ROLES.BIONICS_B1.DESCRIPTION, 0, "hat_role_innerworkings1", "skillbadge_bionic_gears1", Db.Get().SkillGroups.BionicSkills.Id, new List<SkillPerk>
				{
					Db.Get().SkillPerks.EfficientBionicGears
				}, new List<string>(), GameTags.Minions.Models.Bionic.Name, DlcManager.DLC3, null));
				this.BionicsB2 = this.AddSkill(new Skill("BionicsB2", DUPLICANTS.ROLES.BIONICS_B2.NAME, DUPLICANTS.ROLES.BIONICS_B2.DESCRIPTION, 1, "hat_role_innerworkings2", "skillbadge_bionic_gears2", Db.Get().SkillGroups.BionicSkills.Id, new List<SkillPerk>
				{
					Db.Get().SkillPerks.ExtraBionicBooster4,
					Db.Get().SkillPerks.IncreaseAthleticsBionicsB2
				}, new List<string>
				{
					this.BionicsB1.Id
				}, GameTags.Minions.Models.Bionic.Name, DlcManager.DLC3, null));
				this.BionicsC1 = this.AddSkill(new Skill("BionicsC1", DUPLICANTS.ROLES.BIONICS_C1.NAME, DUPLICANTS.ROLES.BIONICS_C1.DESCRIPTION, 0, "hat_role_craftingboosters1", "skillbadge_bionic_schematics1", Db.Get().SkillGroups.BionicSkills.Id, new List<SkillPerk>
				{
					Db.Get().SkillPerks.CanCraftElectronics,
					Db.Get().SkillPerks.IncreaseAthleticsBionicsC1
				}, new List<string>(), GameTags.Minions.Models.Bionic.Name, DlcManager.DLC3, null));
				this.BionicsC2 = this.AddSkill(new Skill("BionicsC2", DUPLICANTS.ROLES.BIONICS_C2.NAME, DUPLICANTS.ROLES.BIONICS_C2.DESCRIPTION, 1, "hat_role_craftingboosters2", "skillbadge_bionic_schematics2", Db.Get().SkillGroups.BionicSkills.Id, new List<SkillPerk>
				{
					Db.Get().SkillPerks.ExtraBionicBooster6,
					Db.Get().SkillPerks.IncreaseAthleticsBionicsC2
				}, new List<string>
				{
					this.BionicsC1.Id
				}, GameTags.Minions.Models.Bionic.Name, DlcManager.DLC3, null));
				this.BionicsC3 = this.AddSkill(new Skill("BionicsC3", DUPLICANTS.ROLES.BIONICS_C3.NAME, DUPLICANTS.ROLES.BIONICS_C3.DESCRIPTION, 2, "hat_role_innerworkings3", "skillbadge_bionic_gears3", Db.Get().SkillGroups.BionicSkills.Id, new List<SkillPerk>
				{
					Db.Get().SkillPerks.ExtraBionicBatteries
				}, new List<string>
				{
					this.BionicsB2.Id,
					this.BionicsC2.Id
				}, GameTags.Minions.Models.Bionic.Name, DlcManager.DLC3, null));
				this.BionicsD1 = this.AddSkill(new Skill("BionicsD1", DUPLICANTS.ROLES.BIONICS_D1.NAME, DUPLICANTS.ROLES.BIONICS_D1.DESCRIPTION, 0, "hat_role_gainingboosters1", "skillbadge_bionic_booster1", Db.Get().SkillGroups.BionicSkills.Id, new List<SkillPerk>
				{
					Db.Get().SkillPerks.BionicEardrumsDefense,
					Db.Get().SkillPerks.BionicMinorEyeIrritationDefense,
					Db.Get().SkillPerks.BionicMajorEyeIrritationDefense
				}, new List<string>(), GameTags.Minions.Models.Bionic.Name, DlcManager.DLC3, null));
				this.BionicsD2 = this.AddSkill(new Skill("BionicsD2", DUPLICANTS.ROLES.BIONICS_D2.NAME, DUPLICANTS.ROLES.BIONICS_D2.DESCRIPTION, 1, "hat_role_gainingboosters1", "skillbadge_bionic_booster1", Db.Get().SkillGroups.BionicSkills.Id, new List<SkillPerk>
				{
					Db.Get().SkillPerks.BionicToastySurroundingsDefense,
					Db.Get().SkillPerks.BionicChillySurroundingsDefense
				}, new List<string>
				{
					this.BionicsD1.Id
				}, GameTags.Minions.Models.Bionic.Name, DlcManager.DLC3, null));
			}
		}

		// Token: 0x06007E47 RID: 32327 RVA: 0x0032662A File Offset: 0x0032482A
		private Skill AddSkill(Skill skill)
		{
			if (DlcManager.IsCorrectDlcSubscribed(skill))
			{
				return base.Add(skill);
			}
			return skill;
		}

		// Token: 0x06007E48 RID: 32328 RVA: 0x00326640 File Offset: 0x00324840
		public List<Skill> GetSkillsWithPerk(string perk)
		{
			List<Skill> list = new List<Skill>();
			foreach (Skill skill in this.resources)
			{
				if (skill.GivesPerk(perk))
				{
					list.Add(skill);
				}
			}
			return list;
		}

		// Token: 0x06007E49 RID: 32329 RVA: 0x003266A8 File Offset: 0x003248A8
		public List<Skill> GetSkillsWithPerk(SkillPerk perk)
		{
			List<Skill> list = new List<Skill>();
			foreach (Skill skill in this.resources)
			{
				if (skill.GivesPerk(perk))
				{
					list.Add(skill);
				}
			}
			return list;
		}

		// Token: 0x06007E4A RID: 32330 RVA: 0x0032670C File Offset: 0x0032490C
		public List<Skill> GetAllPriorSkills(Skill skill)
		{
			List<Skill> list = new List<Skill>();
			foreach (string id in skill.priorSkills)
			{
				Skill skill2 = base.Get(id);
				list.Add(skill2);
				list.AddRange(this.GetAllPriorSkills(skill2));
			}
			return list;
		}

		// Token: 0x06007E4B RID: 32331 RVA: 0x0032677C File Offset: 0x0032497C
		public List<Skill> GetTerminalSkills()
		{
			List<Skill> list = new List<Skill>();
			foreach (Skill skill in this.resources)
			{
				bool flag = true;
				using (List<Skill>.Enumerator enumerator2 = this.resources.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.priorSkills.Contains(skill.Id))
						{
							flag = false;
							break;
						}
					}
				}
				if (flag)
				{
					list.Add(skill);
				}
			}
			return list;
		}

		// Token: 0x04005D31 RID: 23857
		public Skill Mining1;

		// Token: 0x04005D32 RID: 23858
		public Skill Mining2;

		// Token: 0x04005D33 RID: 23859
		public Skill Mining3;

		// Token: 0x04005D34 RID: 23860
		public Skill Mining4;

		// Token: 0x04005D35 RID: 23861
		public Skill Building1;

		// Token: 0x04005D36 RID: 23862
		public Skill Building2;

		// Token: 0x04005D37 RID: 23863
		public Skill Building3;

		// Token: 0x04005D38 RID: 23864
		public Skill Farming1;

		// Token: 0x04005D39 RID: 23865
		public Skill Farming2;

		// Token: 0x04005D3A RID: 23866
		public Skill Farming3;

		// Token: 0x04005D3B RID: 23867
		public Skill Ranching1;

		// Token: 0x04005D3C RID: 23868
		public Skill Ranching2;

		// Token: 0x04005D3D RID: 23869
		public Skill Researching1;

		// Token: 0x04005D3E RID: 23870
		public Skill Researching2;

		// Token: 0x04005D3F RID: 23871
		public Skill Researching3;

		// Token: 0x04005D40 RID: 23872
		public Skill Researching4;

		// Token: 0x04005D41 RID: 23873
		public Skill AtomicResearch;

		// Token: 0x04005D42 RID: 23874
		public Skill SpaceResearch;

		// Token: 0x04005D43 RID: 23875
		public Skill Astronomy;

		// Token: 0x04005D44 RID: 23876
		public Skill RocketPiloting1;

		// Token: 0x04005D45 RID: 23877
		public Skill RocketPiloting2;

		// Token: 0x04005D46 RID: 23878
		public Skill Cooking1;

		// Token: 0x04005D47 RID: 23879
		public Skill Cooking2;

		// Token: 0x04005D48 RID: 23880
		public Skill Arting1;

		// Token: 0x04005D49 RID: 23881
		public Skill Arting2;

		// Token: 0x04005D4A RID: 23882
		public Skill Arting3;

		// Token: 0x04005D4B RID: 23883
		public Skill Hauling1;

		// Token: 0x04005D4C RID: 23884
		public Skill Hauling2;

		// Token: 0x04005D4D RID: 23885
		public Skill ThermalSuits;

		// Token: 0x04005D4E RID: 23886
		public Skill Suits1;

		// Token: 0x04005D4F RID: 23887
		public Skill Technicals1;

		// Token: 0x04005D50 RID: 23888
		public Skill Technicals2;

		// Token: 0x04005D51 RID: 23889
		public Skill Engineering1;

		// Token: 0x04005D52 RID: 23890
		public Skill Basekeeping1;

		// Token: 0x04005D53 RID: 23891
		public Skill Basekeeping2;

		// Token: 0x04005D54 RID: 23892
		public Skill Pyrotechnics;

		// Token: 0x04005D55 RID: 23893
		public Skill Astronauting1;

		// Token: 0x04005D56 RID: 23894
		public Skill Astronauting2;

		// Token: 0x04005D57 RID: 23895
		public Skill Medicine1;

		// Token: 0x04005D58 RID: 23896
		public Skill Medicine2;

		// Token: 0x04005D59 RID: 23897
		public Skill Medicine3;

		// Token: 0x04005D5A RID: 23898
		public Skill BionicsA1;

		// Token: 0x04005D5B RID: 23899
		public Skill BionicsA2;

		// Token: 0x04005D5C RID: 23900
		public Skill BionicsA3;

		// Token: 0x04005D5D RID: 23901
		public Skill BionicsB1;

		// Token: 0x04005D5E RID: 23902
		public Skill BionicsB2;

		// Token: 0x04005D5F RID: 23903
		public Skill BionicsB3;

		// Token: 0x04005D60 RID: 23904
		public Skill BionicsC1;

		// Token: 0x04005D61 RID: 23905
		public Skill BionicsC2;

		// Token: 0x04005D62 RID: 23906
		public Skill BionicsC3;

		// Token: 0x04005D63 RID: 23907
		public Skill BionicsD1;

		// Token: 0x04005D64 RID: 23908
		public Skill BionicsD2;
	}
}
