using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x0200016A RID: 362
public class SquirrelConfig : IEntityConfig
{
	// Token: 0x060006F6 RID: 1782 RVA: 0x00031308 File Offset: 0x0002F508
	public static GameObject CreateSquirrel(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = EntityTemplates.ExtendEntityToWildCreature(BaseSquirrelConfig.BaseSquirrel(id, name, desc, anim_file, "SquirrelBaseTrait", is_baby, null, false), SquirrelTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("SquirrelBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, SquirrelTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -SquirrelTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		Diet.Info[] diet_infos = BaseSquirrelConfig.BasicDiet(SimHashes.Dirt.CreateTag(), SquirrelConfig.CALORIES_PER_DAY_OF_PLANT_EATEN, SquirrelConfig.KG_POOP_PER_DAY_OF_PLANT, null, 0f);
		GameObject gameObject = BaseSquirrelConfig.SetupDiet(prefab, diet_infos, SquirrelConfig.MIN_POOP_SIZE_KG);
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x060006F7 RID: 1783 RVA: 0x00031440 File Offset: 0x0002F640
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(SquirrelConfig.CreateSquirrel("Squirrel", CREATURES.SPECIES.SQUIRREL.NAME, CREATURES.SPECIES.SQUIRREL.DESC, "squirrel_kanim", false), this as IHasDlcRestrictions, "SquirrelEgg", CREATURES.SPECIES.SQUIRREL.EGG_NAME, CREATURES.SPECIES.SQUIRREL.DESC, "egg_squirrel_kanim", SquirrelTuning.EGG_MASS, "SquirrelBaby", 60.000004f, 20f, SquirrelTuning.EGG_CHANCES_BASE, SquirrelConfig.EGG_SORT_ORDER, true, false, 1f, false);
	}

	// Token: 0x060006F8 RID: 1784 RVA: 0x000314C0 File Offset: 0x0002F6C0
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060006F9 RID: 1785 RVA: 0x000314C2 File Offset: 0x0002F6C2
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000549 RID: 1353
	public const string ID = "Squirrel";

	// Token: 0x0400054A RID: 1354
	public const string BASE_TRAIT_ID = "SquirrelBaseTrait";

	// Token: 0x0400054B RID: 1355
	public const string EGG_ID = "SquirrelEgg";

	// Token: 0x0400054C RID: 1356
	public const float OXYGEN_RATE = 0.023437504f;

	// Token: 0x0400054D RID: 1357
	public const float BABY_OXYGEN_RATE = 0.011718752f;

	// Token: 0x0400054E RID: 1358
	private const SimHashes EMIT_ELEMENT = SimHashes.Dirt;

	// Token: 0x0400054F RID: 1359
	public static float DAYS_PLANT_GROWTH_EATEN_PER_CYCLE = 0.4f;

	// Token: 0x04000550 RID: 1360
	private static float CALORIES_PER_DAY_OF_PLANT_EATEN = SquirrelTuning.STANDARD_CALORIES_PER_CYCLE / SquirrelConfig.DAYS_PLANT_GROWTH_EATEN_PER_CYCLE;

	// Token: 0x04000551 RID: 1361
	private static float KG_POOP_PER_DAY_OF_PLANT = 50f;

	// Token: 0x04000552 RID: 1362
	private static float MIN_POOP_SIZE_KG = 40f;

	// Token: 0x04000553 RID: 1363
	public static int EGG_SORT_ORDER = 0;
}
