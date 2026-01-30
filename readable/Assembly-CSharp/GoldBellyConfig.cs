using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x0200012A RID: 298
[EntityConfigOrder(2)]
public class GoldBellyConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x0600059B RID: 1435 RVA: 0x0002BD38 File Offset: 0x00029F38
	public static GameObject CreateGoldBelly(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = EntityTemplates.ExtendEntityToWildCreature(BaseBellyConfig.BaseBelly(id, name, desc, anim_file, "GoldBellyBaseTrait", is_baby, "king_"), MooTuning.PEN_SIZE_PER_CREATURE);
		gameObject.AddOrGet<WarmBlooded>().BaseGenerationKW = 1.3f;
		Trait trait = Db.Get().CreateTrait("GoldBellyBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, BellyTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -BellyTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 50f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 200f, name, false, false, true));
		string alwaysShowDisease = "PollenGerms";
		gameObject.AddOrGet<DiseaseSourceVisualizer>().alwaysShowDisease = alwaysShowDisease;
		WellFedShearable.Def def = gameObject.AddOrGetDef<WellFedShearable.Def>();
		def.effectId = "GoldBellyWellFed";
		def.caloriesPerCycle = BellyTuning.STANDARD_CALORIES_PER_CYCLE;
		def.growthDurationCycles = 10f;
		def.dropMass = 250f;
		def.itemDroppedOnShear = GoldBellyConfig.SCALE_GROWTH_EMIT_ELEMENT;
		def.requiredDiet = "FriesCarrot";
		def.levelCount = 6;
		def.scaleGrowthSymbols = GoldBellyConfig.SCALE_SYMBOLS;
		return BaseBellyConfig.SetupDiet(gameObject, BaseBellyConfig.StandardDiets(), BellyTuning.CALORIES_PER_UNIT_EATEN, 1f);
	}

	// Token: 0x0600059C RID: 1436 RVA: 0x0002BED1 File Offset: 0x0002A0D1
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x0600059D RID: 1437 RVA: 0x0002BED8 File Offset: 0x0002A0D8
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x0600059E RID: 1438 RVA: 0x0002BEDC File Offset: 0x0002A0DC
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.ExtendEntityToFertileCreature(GoldBellyConfig.CreateGoldBelly("GoldBelly", CREATURES.SPECIES.ICEBELLY.VARIANT_GOLD.NAME, CREATURES.SPECIES.ICEBELLY.VARIANT_GOLD.DESC, "ice_belly_kanim", false), this, "GoldBellyEgg", CREATURES.SPECIES.ICEBELLY.VARIANT_GOLD.EGG_NAME, CREATURES.SPECIES.ICEBELLY.VARIANT_GOLD.DESC, "egg_icebelly_kanim", 8f, "GoldBellyBaby", 120.00001f, 40f, BellyTuning.EGG_CHANCES_GOLD, GoldBellyConfig.EGG_SORT_ORDER, true, false, 1f, false);
		gameObject.AddOrGetDef<OvercrowdingMonitor.Def>();
		gameObject.AddTag(GameTags.LargeCreature);
		return gameObject;
	}

	// Token: 0x0600059F RID: 1439 RVA: 0x0002BF69 File Offset: 0x0002A169
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060005A0 RID: 1440 RVA: 0x0002BF6B File Offset: 0x0002A16B
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000438 RID: 1080
	public const string ID = "GoldBelly";

	// Token: 0x04000439 RID: 1081
	public const string BASE_TRAIT_ID = "GoldBellyBaseTrait";

	// Token: 0x0400043A RID: 1082
	public const string EGG_ID = "GoldBellyEgg";

	// Token: 0x0400043B RID: 1083
	public const int GERMS_EMMITED_PER_KG_POOPED = 1000;

	// Token: 0x0400043C RID: 1084
	public static Tag SCALE_GROWTH_EMIT_ELEMENT = "GoldBellyCrown";

	// Token: 0x0400043D RID: 1085
	public static float SCALE_INITIAL_GROWTH_PCT = 0.25f;

	// Token: 0x0400043E RID: 1086
	public const float SCALE_GROWTH_TIME_IN_CYCLES = 10f;

	// Token: 0x0400043F RID: 1087
	public const float GOLD_PER_CYCLE = 25f;

	// Token: 0x04000440 RID: 1088
	public static int EGG_SORT_ORDER = 0;

	// Token: 0x04000441 RID: 1089
	public static KAnimHashedString[] SCALE_SYMBOLS = new KAnimHashedString[]
	{
		"antler_0",
		"antler_1",
		"antler_2",
		"antler_3",
		"antler_4"
	};
}
