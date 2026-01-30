using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000127 RID: 295
[EntityConfigOrder(2)]
public class GlassDeerConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000589 RID: 1417 RVA: 0x0002B600 File Offset: 0x00029800
	public static GameObject CreateGlassDeer(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = EntityTemplates.ExtendEntityToWildCreature(BaseDeerConfig.BaseDeer(id, name, desc, anim_file, "GlassDeerBaseTrait", is_baby, "gla_"), DeerTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("GlassDeerBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, 1000000f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -166.66667f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		GameObject gameObject = BaseDeerConfig.SetupDiet(prefab, new List<Diet.Info>
		{
			BaseDeerConfig.CreateDietInfo("HardSkinBerryPlant", SimHashes.Dirt.CreateTag(), GlassDeerConfig.HARD_SKIN_CALORIES_PER_KG, 8.333334f, null, 0f),
			new Diet.Info(new HashSet<Tag>
			{
				"HardSkinBerry"
			}, SimHashes.Dirt.CreateTag(), GlassDeerConfig.CONSUMABLE_PLANT_MATURITY_LEVELS * GlassDeerConfig.HARD_SKIN_CALORIES_PER_KG / 1f, 25.000002f, null, 0f, false, Diet.Info.FoodType.EatSolid, false, null),
			BaseDeerConfig.CreateDietInfo("PrickleFlower", SimHashes.Dirt.CreateTag(), GlassDeerConfig.BRISTLE_CALORIES_PER_KG / 2f, 8.333334f, null, 0f),
			new Diet.Info(new HashSet<Tag>
			{
				PrickleFruitConfig.ID
			}, SimHashes.Dirt.CreateTag(), GlassDeerConfig.CONSUMABLE_PLANT_MATURITY_LEVELS * GlassDeerConfig.BRISTLE_CALORIES_PER_KG / 1f, 50.000004f, null, 0f, false, Diet.Info.FoodType.EatSolid, false, null),
			new Diet.Info(new HashSet<Tag>
			{
				SimHashes.Katairite.CreateTag()
			}, SimHashes.Dirt.CreateTag(), 5000f, 0.5f, null, 0f, false, Diet.Info.FoodType.EatSolid, false, null)
		}.ToArray(), 1f);
		WellFedShearable.Def def = gameObject.AddOrGetDef<WellFedShearable.Def>();
		def.effectId = "GlassDeerWellFed";
		def.caloriesPerCycle = 100000f;
		def.growthDurationCycles = 6f;
		def.dropMass = 60f;
		def.requiredDiet = SimHashes.Katairite.CreateTag();
		def.itemDroppedOnShear = SimHashes.Glass.CreateTag();
		def.levelCount = 6;
		return gameObject;
	}

	// Token: 0x0600058A RID: 1418 RVA: 0x0002B8A5 File Offset: 0x00029AA5
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x0600058B RID: 1419 RVA: 0x0002B8AC File Offset: 0x00029AAC
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x0600058C RID: 1420 RVA: 0x0002B8B0 File Offset: 0x00029AB0
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(GlassDeerConfig.CreateGlassDeer("GlassDeer", STRINGS.CREATURES.SPECIES.GLASSDEER.NAME, STRINGS.CREATURES.SPECIES.GLASSDEER.DESC, "ice_floof_kanim", false), this, "GlassDeerEgg", STRINGS.CREATURES.SPECIES.GLASSDEER.EGG_NAME, STRINGS.CREATURES.SPECIES.GLASSDEER.DESC, "egg_ice_floof_kanim", DeerTuning.EGG_MASS, "GlassDeerBaby", 60.000004f, 20f, DeerTuning.EGG_CHANCES_GLASS, GlassDeerConfig.EGG_SORT_ORDER, true, false, 1f, false);
	}

	// Token: 0x0600058D RID: 1421 RVA: 0x0002B92B File Offset: 0x00029B2B
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600058E RID: 1422 RVA: 0x0002B92D File Offset: 0x00029B2D
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000417 RID: 1047
	public const string ID = "GlassDeer";

	// Token: 0x04000418 RID: 1048
	public const string BASE_TRAIT_ID = "GlassDeerBaseTrait";

	// Token: 0x04000419 RID: 1049
	public const string EGG_ID = "GlassDeerEgg";

	// Token: 0x0400041A RID: 1050
	public static int EGG_SORT_ORDER = 0;

	// Token: 0x0400041B RID: 1051
	public const SimHashes CONSUMED_ELEMENT = SimHashes.Katairite;

	// Token: 0x0400041C RID: 1052
	public const SimHashes POOP_ELEMENT = SimHashes.Dirt;

	// Token: 0x0400041D RID: 1053
	public const SimHashes SHEAR_ELEMENT = SimHashes.Glass;

	// Token: 0x0400041E RID: 1054
	public const float ANTLER_GROWTH_TIME_IN_CYCLES = 6f;

	// Token: 0x0400041F RID: 1055
	public const float ANTLER_STARTING_GROWTH_PCT = 0.5f;

	// Token: 0x04000420 RID: 1056
	public const float ANTLER_MATERIAL_MASS_PER_CYCLE = 10f;

	// Token: 0x04000421 RID: 1057
	public const float ANTLER_MASS_PER_ANTLER = 60f;

	// Token: 0x04000422 RID: 1058
	public const float CALORIES_PER_PLANT_BITE = 100000f;

	// Token: 0x04000423 RID: 1059
	public const float DAYS_PLANT_GROWTH_EATEN_PER_CYCLE = 0.2f;

	// Token: 0x04000424 RID: 1060
	public static float CONSUMABLE_PLANT_MATURITY_LEVELS = CROPS.CROP_TYPES.Find((Crop.CropVal m) => m.cropId == "HardSkinBerry").cropDuration / 600f;

	// Token: 0x04000425 RID: 1061
	public static float KG_PLANT_EATEN_A_DAY = 0.2f * GlassDeerConfig.CONSUMABLE_PLANT_MATURITY_LEVELS;

	// Token: 0x04000426 RID: 1062
	public static float HARD_SKIN_CALORIES_PER_KG = 100000f / GlassDeerConfig.KG_PLANT_EATEN_A_DAY;

	// Token: 0x04000427 RID: 1063
	public static float BRISTLE_CALORIES_PER_KG = GlassDeerConfig.HARD_SKIN_CALORIES_PER_KG * 2f;

	// Token: 0x04000428 RID: 1064
	public const float CALORIES_PER_BITE = 100000f;

	// Token: 0x04000429 RID: 1065
	public const float KG_SOLIDS_EATEN_A_DAY = 20f;

	// Token: 0x0400042A RID: 1066
	public const float CALORIES_PER_SOLID_KG = 5000f;

	// Token: 0x0400042B RID: 1067
	public const float MIN_KG_CONSUMED_BEFORE_POOPING = 1f;

	// Token: 0x0400042C RID: 1068
	public const float POOP_MASS_CONVERSION_MULTIPLIER = 0.5f;

	// Token: 0x0400042D RID: 1069
	public const float POOP_MASS_PLANT_CONVERSION_MULTIPLIER = 8.333334f;
}
