using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000176 RID: 374
public class WoodDeerConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000741 RID: 1857 RVA: 0x00032428 File Offset: 0x00030628
	public static GameObject CreateWoodDeer(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = EntityTemplates.ExtendEntityToWildCreature(BaseDeerConfig.BaseDeer(id, name, desc, anim_file, "WoodDeerBaseTrait", is_baby, null), DeerTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("WoodDeerBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, 1000000f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -166.66667f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		GameObject gameObject = BaseDeerConfig.SetupDiet(prefab, new List<Diet.Info>
		{
			BaseDeerConfig.CreateDietInfo("HardSkinBerryPlant", SimHashes.Dirt.CreateTag(), WoodDeerConfig.HARD_SKIN_CALORIES_PER_KG, WoodDeerConfig.POOP_MASS_CONVERSION_MULTIPLIER, null, 0f),
			new Diet.Info(new HashSet<Tag>
			{
				"HardSkinBerry"
			}, SimHashes.Dirt.CreateTag(), WoodDeerConfig.CONSUMABLE_PLANT_MATURITY_LEVELS * WoodDeerConfig.HARD_SKIN_CALORIES_PER_KG / 1f, WoodDeerConfig.POOP_MASS_CONVERSION_MULTIPLIER * 3f, null, 0f, false, Diet.Info.FoodType.EatSolid, false, null),
			BaseDeerConfig.CreateDietInfo("PrickleFlower", SimHashes.Dirt.CreateTag(), WoodDeerConfig.BRISTLE_CALORIES_PER_KG / 2f, WoodDeerConfig.POOP_MASS_CONVERSION_MULTIPLIER, null, 0f),
			new Diet.Info(new HashSet<Tag>
			{
				PrickleFruitConfig.ID
			}, SimHashes.Dirt.CreateTag(), WoodDeerConfig.CONSUMABLE_PLANT_MATURITY_LEVELS * WoodDeerConfig.BRISTLE_CALORIES_PER_KG / 1f, WoodDeerConfig.POOP_MASS_CONVERSION_MULTIPLIER * 6f, null, 0f, false, Diet.Info.FoodType.EatSolid, false, null)
		}.ToArray(), WoodDeerConfig.MIN_KG_CONSUMED_BEFORE_POOPING);
		gameObject.AddTag(GameTags.OriginalCreature);
		WellFedShearable.Def def = gameObject.AddOrGetDef<WellFedShearable.Def>();
		def.effectId = "WoodDeerWellFed";
		def.caloriesPerCycle = 100000f;
		def.growthDurationCycles = WoodDeerConfig.ANTLER_GROWTH_TIME_IN_CYCLES;
		def.dropMass = WoodDeerConfig.WOOD_MASS_PER_ANTLER;
		def.itemDroppedOnShear = WoodLogConfig.TAG;
		def.levelCount = 6;
		return gameObject;
	}

	// Token: 0x06000742 RID: 1858 RVA: 0x0003268C File Offset: 0x0003088C
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x06000743 RID: 1859 RVA: 0x00032693 File Offset: 0x00030893
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000744 RID: 1860 RVA: 0x00032698 File Offset: 0x00030898
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(WoodDeerConfig.CreateWoodDeer("WoodDeer", STRINGS.CREATURES.SPECIES.WOODDEER.NAME, STRINGS.CREATURES.SPECIES.WOODDEER.DESC, "ice_floof_kanim", false), this, "WoodDeerEgg", STRINGS.CREATURES.SPECIES.WOODDEER.EGG_NAME, STRINGS.CREATURES.SPECIES.WOODDEER.DESC, "egg_ice_floof_kanim", DeerTuning.EGG_MASS, "WoodDeerBaby", 60.000004f, 20f, DeerTuning.EGG_CHANCES_BASE, WoodDeerConfig.EGG_SORT_ORDER, true, false, 1f, false);
	}

	// Token: 0x06000745 RID: 1861 RVA: 0x00032713 File Offset: 0x00030913
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000746 RID: 1862 RVA: 0x00032715 File Offset: 0x00030915
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000586 RID: 1414
	public const string ID = "WoodDeer";

	// Token: 0x04000587 RID: 1415
	public const string BASE_TRAIT_ID = "WoodDeerBaseTrait";

	// Token: 0x04000588 RID: 1416
	public const string EGG_ID = "WoodDeerEgg";

	// Token: 0x04000589 RID: 1417
	private const SimHashes EMIT_ELEMENT = SimHashes.Dirt;

	// Token: 0x0400058A RID: 1418
	public const float CALORIES_PER_PLANT_BITE = 100000f;

	// Token: 0x0400058B RID: 1419
	public const float DAYS_PLANT_GROWTH_EATEN_PER_CYCLE = 0.2f;

	// Token: 0x0400058C RID: 1420
	public static float CONSUMABLE_PLANT_MATURITY_LEVELS = CROPS.CROP_TYPES.Find((Crop.CropVal m) => m.cropId == "HardSkinBerry").cropDuration / 600f;

	// Token: 0x0400058D RID: 1421
	public static float KG_PLANT_EATEN_A_DAY = 0.2f * WoodDeerConfig.CONSUMABLE_PLANT_MATURITY_LEVELS;

	// Token: 0x0400058E RID: 1422
	public static float HARD_SKIN_CALORIES_PER_KG = 100000f / WoodDeerConfig.KG_PLANT_EATEN_A_DAY;

	// Token: 0x0400058F RID: 1423
	public static float BRISTLE_CALORIES_PER_KG = WoodDeerConfig.HARD_SKIN_CALORIES_PER_KG * 2f;

	// Token: 0x04000590 RID: 1424
	public static float ANTLER_GROWTH_TIME_IN_CYCLES = 6f;

	// Token: 0x04000591 RID: 1425
	public static float ANTLER_STARTING_GROWTH_PCT = 0.5f;

	// Token: 0x04000592 RID: 1426
	public static float WOOD_PER_CYCLE = 60f;

	// Token: 0x04000593 RID: 1427
	public static float WOOD_MASS_PER_ANTLER = WoodDeerConfig.WOOD_PER_CYCLE * WoodDeerConfig.ANTLER_GROWTH_TIME_IN_CYCLES;

	// Token: 0x04000594 RID: 1428
	private static float POOP_MASS_CONVERSION_MULTIPLIER = 8.333334f;

	// Token: 0x04000595 RID: 1429
	private static float MIN_KG_CONSUMED_BEFORE_POOPING = 1f;

	// Token: 0x04000596 RID: 1430
	public static int EGG_SORT_ORDER = 0;
}
