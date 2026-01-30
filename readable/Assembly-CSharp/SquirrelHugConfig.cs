using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200016C RID: 364
[EntityConfigOrder(2)]
public class SquirrelHugConfig : IEntityConfig
{
	// Token: 0x06000700 RID: 1792 RVA: 0x0003154C File Offset: 0x0002F74C
	public static GameObject CreateSquirrelHug(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = BaseSquirrelConfig.BaseSquirrel(id, name, desc, anim_file, "SquirrelHugBaseTrait", is_baby, "hug_", true);
		gameObject = EntityTemplates.ExtendEntityToWildCreature(gameObject, SquirrelTuning.PEN_SIZE_PER_CREATURE_HUG);
		gameObject.AddOrGet<DecorProvider>().SetValues(DECOR.BONUS.TIER3);
		Trait trait = Db.Get().CreateTrait("SquirrelHugBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, SquirrelTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -SquirrelTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		Diet.Info[] diet_infos = BaseSquirrelConfig.BasicDiet(SimHashes.Dirt.CreateTag(), SquirrelHugConfig.CALORIES_PER_DAY_OF_PLANT_EATEN, SquirrelHugConfig.KG_POOP_PER_DAY_OF_PLANT, null, 0f);
		gameObject = BaseSquirrelConfig.SetupDiet(gameObject, diet_infos, SquirrelHugConfig.MIN_POOP_SIZE_KG);
		if (!is_baby)
		{
			gameObject.AddOrGetDef<HugMonitor.Def>();
		}
		return gameObject;
	}

	// Token: 0x06000701 RID: 1793 RVA: 0x000316A0 File Offset: 0x0002F8A0
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(SquirrelHugConfig.CreateSquirrelHug("SquirrelHug", STRINGS.CREATURES.SPECIES.SQUIRREL.VARIANT_HUG.NAME, STRINGS.CREATURES.SPECIES.SQUIRREL.VARIANT_HUG.DESC, "squirrel_kanim", false), this as IHasDlcRestrictions, "SquirrelHugEgg", STRINGS.CREATURES.SPECIES.SQUIRREL.VARIANT_HUG.EGG_NAME, STRINGS.CREATURES.SPECIES.SQUIRREL.VARIANT_HUG.DESC, "egg_squirrel_kanim", SquirrelTuning.EGG_MASS, "SquirrelHugBaby", 60.000004f, 20f, SquirrelTuning.EGG_CHANCES_HUG, SquirrelHugConfig.EGG_SORT_ORDER, true, false, 1f, false);
	}

	// Token: 0x06000702 RID: 1794 RVA: 0x00031720 File Offset: 0x0002F920
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000703 RID: 1795 RVA: 0x00031722 File Offset: 0x0002F922
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000555 RID: 1365
	public const string ID = "SquirrelHug";

	// Token: 0x04000556 RID: 1366
	public const string BASE_TRAIT_ID = "SquirrelHugBaseTrait";

	// Token: 0x04000557 RID: 1367
	public const string EGG_ID = "SquirrelHugEgg";

	// Token: 0x04000558 RID: 1368
	public const float OXYGEN_RATE = 0.023437504f;

	// Token: 0x04000559 RID: 1369
	public const float BABY_OXYGEN_RATE = 0.011718752f;

	// Token: 0x0400055A RID: 1370
	private const SimHashes EMIT_ELEMENT = SimHashes.Dirt;

	// Token: 0x0400055B RID: 1371
	public static float DAYS_PLANT_GROWTH_EATEN_PER_CYCLE = 0.5f;

	// Token: 0x0400055C RID: 1372
	private static float CALORIES_PER_DAY_OF_PLANT_EATEN = SquirrelTuning.STANDARD_CALORIES_PER_CYCLE / SquirrelHugConfig.DAYS_PLANT_GROWTH_EATEN_PER_CYCLE;

	// Token: 0x0400055D RID: 1373
	private static float KG_POOP_PER_DAY_OF_PLANT = 25f;

	// Token: 0x0400055E RID: 1374
	private static float MIN_POOP_SIZE_KG = 40f;

	// Token: 0x0400055F RID: 1375
	public static int EGG_SORT_ORDER = 0;
}
