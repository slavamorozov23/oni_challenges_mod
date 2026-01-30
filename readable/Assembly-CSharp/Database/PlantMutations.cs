using System;
using System.Collections.Generic;
using System.Linq;
using Klei.AI;

namespace Database
{
	// Token: 0x02000F4F RID: 3919
	public class PlantMutations : ResourceSet<PlantMutation>
	{
		// Token: 0x06007CB2 RID: 31922 RVA: 0x00315628 File Offset: 0x00313828
		public PlantMutation AddPlantMutation(string id)
		{
			StringEntry entry = Strings.Get(new StringKey("STRINGS.CREATURES.PLANT_MUTATIONS." + id.ToUpper() + ".NAME"));
			StringEntry entry2 = Strings.Get(new StringKey("STRINGS.CREATURES.PLANT_MUTATIONS." + id.ToUpper() + ".DESCRIPTION"));
			PlantMutation plantMutation = new PlantMutation(id, entry, entry2);
			base.Add(plantMutation);
			return plantMutation;
		}

		// Token: 0x06007CB3 RID: 31923 RVA: 0x00315694 File Offset: 0x00313894
		public PlantMutations(ResourceSet parent) : base("PlantMutations", parent)
		{
			this.moderatelyLoose = this.AddPlantMutation("moderatelyLoose").AttributeModifier(Db.Get().PlantAttributes.MinRadiationThreshold, 250f, false).AttributeModifier(Db.Get().PlantAttributes.WiltTempRangeMod, 0.5f, true).AttributeModifier(Db.Get().PlantAttributes.YieldAmount, -0.25f, true).AttributeModifier(Db.Get().PlantAttributes.FertilizerUsageMod, -0.5f, true).VisualTint(-0.4f, -0.4f, -0.4f);
			this.moderatelyTight = this.AddPlantMutation("moderatelyTight").AttributeModifier(Db.Get().PlantAttributes.MinRadiationThreshold, 250f, false).AttributeModifier(Db.Get().PlantAttributes.WiltTempRangeMod, -0.5f, true).AttributeModifier(Db.Get().PlantAttributes.YieldAmount, 0.5f, true).VisualTint(0.2f, 0.2f, 0.2f);
			this.extremelyTight = this.AddPlantMutation("extremelyTight").AttributeModifier(Db.Get().PlantAttributes.MinRadiationThreshold, 250f, false).AttributeModifier(Db.Get().PlantAttributes.WiltTempRangeMod, -0.8f, true).AttributeModifier(Db.Get().PlantAttributes.YieldAmount, 1f, true).VisualTint(0.3f, 0.3f, 0.3f).VisualBGFX("mutate_glow_fx_kanim");
			this.bonusLice = this.AddPlantMutation("bonusLice").AttributeModifier(Db.Get().PlantAttributes.MinRadiationThreshold, 250f, false).AttributeModifier(Db.Get().PlantAttributes.FertilizerUsageMod, 0.25f, true).BonusCrop("BasicPlantFood", 1f).VisualSymbolOverride("snapTo_mutate1", "mutate_snaps_kanim", "meal_lice_mutate1").VisualSymbolOverride("snapTo_mutate2", "mutate_snaps_kanim", "meal_lice_mutate2").AddSoundEvent(GlobalAssets.GetSound("Plant_mutation_MealLice", false));
			this.sunnySpeed = this.AddPlantMutation("sunnySpeed").AttributeModifier(Db.Get().PlantAttributes.MinRadiationThreshold, 250f, false).AttributeModifier(Db.Get().PlantAttributes.MinLightLux, 1000f, false).AttributeModifier(Db.Get().Amounts.Maturity.maxAttribute, -0.5f, true).AttributeModifier(Db.Get().PlantAttributes.FertilizerUsageMod, 0.25f, true).VisualSymbolOverride("snapTo_mutate1", "mutate_snaps_kanim", "leaf_mutate1").VisualSymbolOverride("snapTo_mutate2", "mutate_snaps_kanim", "leaf_mutate2").AddSoundEvent(GlobalAssets.GetSound("Plant_mutation_Leaf", false));
			this.slowBurn = this.AddPlantMutation("slowBurn").AttributeModifier(Db.Get().PlantAttributes.MinRadiationThreshold, 250f, false).AttributeModifier(Db.Get().PlantAttributes.FertilizerUsageMod, -0.9f, true).AttributeModifier(Db.Get().Amounts.Maturity.maxAttribute, 3.5f, true).VisualTint(-0.3f, -0.3f, -0.5f);
			this.blooms = this.AddPlantMutation("blooms").AttributeModifier(Db.Get().PlantAttributes.MinRadiationThreshold, 250f, false).AttributeModifier(Db.Get().BuildingAttributes.Decor, 20f, false).VisualSymbolOverride("snapTo_mutate1", "mutate_snaps_kanim", "blossom_mutate1").VisualSymbolOverride("snapTo_mutate2", "mutate_snaps_kanim", "blossom_mutate2").AddSoundEvent(GlobalAssets.GetSound("Plant_mutation_PrickleFlower", false));
			this.loadedWithFruit = this.AddPlantMutation("loadedWithFruit").AttributeModifier(Db.Get().PlantAttributes.MinRadiationThreshold, 250f, false).AttributeModifier(Db.Get().PlantAttributes.YieldAmount, 1f, true).AttributeModifier(Db.Get().PlantAttributes.HarvestTime, 4f, true).AttributeModifier(Db.Get().PlantAttributes.MinLightLux, 200f, false).AttributeModifier(Db.Get().PlantAttributes.FertilizerUsageMod, 0.2f, true).VisualSymbolScale("swap_crop01", 1.3f).VisualSymbolScale("swap_crop02", 1.3f);
			this.rottenHeaps = this.AddPlantMutation("rottenHeaps").AttributeModifier(Db.Get().PlantAttributes.MinRadiationThreshold, 250f, false).AttributeModifier(Db.Get().Amounts.Maturity.maxAttribute, -0.75f, true).AttributeModifier(Db.Get().PlantAttributes.FertilizerUsageMod, 0.5f, true).BonusCrop(RotPileConfig.ID, 4f).AddDiseaseToHarvest(Db.Get().Diseases.GetIndex(Db.Get().Diseases.FoodGerms.Id), 10000).ForcePrefersDarkness().VisualFGFX("mutate_stink_fx_kanim").VisualSymbolTint("swap_crop01", -0.2f, -0.1f, -0.5f).VisualSymbolTint("swap_crop02", -0.2f, -0.1f, -0.5f);
			this.heavyFruit = this.AddPlantMutation("heavyFruit").AttributeModifier(Db.Get().PlantAttributes.MinRadiationThreshold, 250f, false).AttributeModifier(Db.Get().PlantAttributes.FertilizerUsageMod, 0.25f, true).ForceSelfHarvestOnGrown().VisualSymbolTint("swap_crop01", -0.1f, -0.5f, -0.5f).VisualSymbolTint("swap_crop02", -0.1f, -0.5f, -0.5f);
		}

		// Token: 0x06007CB4 RID: 31924 RVA: 0x00315C80 File Offset: 0x00313E80
		public List<string> GetNamesForMutations(List<string> mutationIDs)
		{
			List<string> list = new List<string>(mutationIDs.Count);
			foreach (string id in mutationIDs)
			{
				list.Add(base.Get(id).Name);
			}
			return list;
		}

		// Token: 0x06007CB5 RID: 31925 RVA: 0x00315CE8 File Offset: 0x00313EE8
		public PlantMutation GetRandomMutation(string targetPlantPrefabID)
		{
			return (from m in this.resources
			where !m.originalMutation && !m.restrictedPrefabIDs.Contains(targetPlantPrefabID) && (m.requiredPrefabIDs.Count == 0 || m.requiredPrefabIDs.Contains(targetPlantPrefabID))
			select m).ToList<PlantMutation>().GetRandom<PlantMutation>();
		}

		// Token: 0x04005B20 RID: 23328
		public PlantMutation moderatelyLoose;

		// Token: 0x04005B21 RID: 23329
		public PlantMutation moderatelyTight;

		// Token: 0x04005B22 RID: 23330
		public PlantMutation extremelyTight;

		// Token: 0x04005B23 RID: 23331
		public PlantMutation bonusLice;

		// Token: 0x04005B24 RID: 23332
		public PlantMutation sunnySpeed;

		// Token: 0x04005B25 RID: 23333
		public PlantMutation slowBurn;

		// Token: 0x04005B26 RID: 23334
		public PlantMutation blooms;

		// Token: 0x04005B27 RID: 23335
		public PlantMutation loadedWithFruit;

		// Token: 0x04005B28 RID: 23336
		public PlantMutation heavyFruit;

		// Token: 0x04005B29 RID: 23337
		public PlantMutation rottenHeaps;
	}
}
