using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000118 RID: 280
[EntityConfigOrder(1)]
public class CrabConfig : IEntityConfig
{
	// Token: 0x06000533 RID: 1331 RVA: 0x00029E10 File Offset: 0x00028010
	public static GameObject CreateCrab(string id, string name, string desc, string anim_file, bool is_baby, string deathDropID, float deathDropCount)
	{
		GameObject prefab = EntityTemplates.ExtendEntityToWildCreature(BaseCrabConfig.BaseCrab(id, name, desc, anim_file, "CrabBaseTrait", is_baby, null, deathDropID, deathDropCount), CrabTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("CrabBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, CrabTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -CrabTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		List<Diet.Info> diet_infos = BaseCrabConfig.BasicDiet(SimHashes.Sand.CreateTag(), CrabConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.NORMAL, null, 0f);
		GameObject gameObject = BaseCrabConfig.SetupDiet(prefab, diet_infos, CrabConfig.CALORIES_PER_KG_OF_ORE, CrabConfig.MIN_POOP_SIZE_IN_KG);
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x06000534 RID: 1332 RVA: 0x00029F50 File Offset: 0x00028150
	public GameObject CreatePrefab()
	{
		GameObject gameObject = CrabConfig.CreateCrab("Crab", STRINGS.CREATURES.SPECIES.CRAB.NAME, STRINGS.CREATURES.SPECIES.CRAB.DESC, "pincher_kanim", false, "CrabShell", 10f);
		gameObject = EntityTemplates.ExtendEntityToFertileCreature(gameObject, this as IHasDlcRestrictions, "CrabEgg", STRINGS.CREATURES.SPECIES.CRAB.EGG_NAME, STRINGS.CREATURES.SPECIES.CRAB.DESC, "egg_pincher_kanim", CrabTuning.EGG_MASS, "CrabBaby", 60.000004f, 20f, CrabTuning.EGG_CHANCES_BASE, CrabConfig.EGG_SORT_ORDER, true, false, 1f, false);
		gameObject.AddOrGetDef<EggProtectionMonitor.Def>().allyTags = new Tag[]
		{
			GameTags.Creatures.CrabFriend
		};
		return gameObject;
	}

	// Token: 0x06000535 RID: 1333 RVA: 0x00029FFB File Offset: 0x000281FB
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000536 RID: 1334 RVA: 0x00029FFD File Offset: 0x000281FD
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040003BF RID: 959
	public const string ID = "Crab";

	// Token: 0x040003C0 RID: 960
	public const string BASE_TRAIT_ID = "CrabBaseTrait";

	// Token: 0x040003C1 RID: 961
	public const string EGG_ID = "CrabEgg";

	// Token: 0x040003C2 RID: 962
	private const SimHashes EMIT_ELEMENT = SimHashes.Sand;

	// Token: 0x040003C3 RID: 963
	private static float KG_ORE_EATEN_PER_CYCLE = 70f;

	// Token: 0x040003C4 RID: 964
	private static float CALORIES_PER_KG_OF_ORE = CrabTuning.STANDARD_CALORIES_PER_CYCLE / CrabConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x040003C5 RID: 965
	private static float MIN_POOP_SIZE_IN_KG = 25f;

	// Token: 0x040003C6 RID: 966
	public static int EGG_SORT_ORDER = 0;
}
