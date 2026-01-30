using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000125 RID: 293
[EntityConfigOrder(2)]
public class DreckoPlasticConfig : IEntityConfig
{
	// Token: 0x0600057F RID: 1407 RVA: 0x0002B2C0 File Offset: 0x000294C0
	public static GameObject CreateDrecko(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = BaseDreckoConfig.BaseDrecko(id, name, desc, anim_file, "DreckoPlasticBaseTrait", is_baby, null, 293.15f, 323.15f, 243.15f, 373.15f);
		gameObject = EntityTemplates.ExtendEntityToWildCreature(gameObject, DreckoTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("DreckoPlasticBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, DreckoTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -DreckoTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 150f, name, false, false, true));
		Diet diet = new Diet(new Diet.Info[]
		{
			new Diet.Info(new HashSet<Tag>
			{
				"BasicSingleHarvestPlant".ToTag(),
				"PrickleFlower".ToTag()
			}, DreckoPlasticConfig.POOP_ELEMENT, DreckoPlasticConfig.CALORIES_PER_DAY_OF_PLANT_EATEN, DreckoPlasticConfig.KG_POOP_PER_DAY_OF_PLANT, null, 0f, false, Diet.Info.FoodType.EatPlantDirectly, false, null)
		});
		CreatureCalorieMonitor.Def def = gameObject.AddOrGetDef<CreatureCalorieMonitor.Def>();
		def.diet = diet;
		def.minConsumedCaloriesBeforePooping = DreckoPlasticConfig.MIN_POOP_SIZE_IN_CALORIES;
		ScaleGrowthMonitor.Def def2 = gameObject.AddOrGetDef<ScaleGrowthMonitor.Def>();
		def2.defaultGrowthRate = 1f / DreckoPlasticConfig.SCALE_GROWTH_TIME_IN_CYCLES / 600f;
		def2.dropMass = DreckoPlasticConfig.PLASTIC_PER_CYCLE * DreckoPlasticConfig.SCALE_GROWTH_TIME_IN_CYCLES;
		def2.itemDroppedOnShear = DreckoPlasticConfig.EMIT_ELEMENT;
		def2.levelCount = 6;
		def2.targetAtmosphere = SimHashes.Hydrogen;
		gameObject.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;
		return gameObject;
	}

	// Token: 0x06000580 RID: 1408 RVA: 0x0002B49C File Offset: 0x0002969C
	public virtual GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(DreckoPlasticConfig.CreateDrecko("DreckoPlastic", CREATURES.SPECIES.DRECKO.VARIANT_PLASTIC.NAME, CREATURES.SPECIES.DRECKO.VARIANT_PLASTIC.DESC, "drecko_kanim", false), this as IHasDlcRestrictions, "DreckoPlasticEgg", CREATURES.SPECIES.DRECKO.VARIANT_PLASTIC.EGG_NAME, CREATURES.SPECIES.DRECKO.VARIANT_PLASTIC.DESC, "egg_drecko_kanim", DreckoTuning.EGG_MASS, "DreckoPlasticBaby", 90f, 30f, DreckoTuning.EGG_CHANCES_PLASTIC, DreckoPlasticConfig.EGG_SORT_ORDER, true, false, 1f, false);
	}

	// Token: 0x06000581 RID: 1409 RVA: 0x0002B51C File Offset: 0x0002971C
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000582 RID: 1410 RVA: 0x0002B51E File Offset: 0x0002971E
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000409 RID: 1033
	public const string ID = "DreckoPlastic";

	// Token: 0x0400040A RID: 1034
	public const string BASE_TRAIT_ID = "DreckoPlasticBaseTrait";

	// Token: 0x0400040B RID: 1035
	public const string EGG_ID = "DreckoPlasticEgg";

	// Token: 0x0400040C RID: 1036
	public static Tag POOP_ELEMENT = SimHashes.Phosphorite.CreateTag();

	// Token: 0x0400040D RID: 1037
	public static Tag EMIT_ELEMENT = SimHashes.Polypropylene.CreateTag();

	// Token: 0x0400040E RID: 1038
	private static float DAYS_PLANT_GROWTH_EATEN_PER_CYCLE = 1f;

	// Token: 0x0400040F RID: 1039
	private static float CALORIES_PER_DAY_OF_PLANT_EATEN = DreckoTuning.STANDARD_CALORIES_PER_CYCLE / DreckoPlasticConfig.DAYS_PLANT_GROWTH_EATEN_PER_CYCLE;

	// Token: 0x04000410 RID: 1040
	private static float KG_POOP_PER_DAY_OF_PLANT = 9f;

	// Token: 0x04000411 RID: 1041
	private static float MIN_POOP_SIZE_IN_KG = 1.5f;

	// Token: 0x04000412 RID: 1042
	private static float MIN_POOP_SIZE_IN_CALORIES = DreckoPlasticConfig.CALORIES_PER_DAY_OF_PLANT_EATEN * DreckoPlasticConfig.MIN_POOP_SIZE_IN_KG / DreckoPlasticConfig.KG_POOP_PER_DAY_OF_PLANT;

	// Token: 0x04000413 RID: 1043
	public static float SCALE_GROWTH_TIME_IN_CYCLES = 3f;

	// Token: 0x04000414 RID: 1044
	public static float PLASTIC_PER_CYCLE = 50f;

	// Token: 0x04000415 RID: 1045
	public static int EGG_SORT_ORDER = 800;
}
