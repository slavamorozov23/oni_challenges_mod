using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200012C RID: 300
[EntityConfigOrder(1)]
public class HatchConfig : IEntityConfig
{
	// Token: 0x060005A9 RID: 1449 RVA: 0x0002C090 File Offset: 0x0002A290
	public static GameObject CreateHatch(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = EntityTemplates.ExtendEntityToWildCreature(BaseHatchConfig.BaseHatch(id, name, desc, anim_file, "HatchBaseTrait", is_baby, null), HatchTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("HatchBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, HatchTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -HatchTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		List<Diet.Info> list = BaseHatchConfig.BasicRockDiet(SimHashes.Carbon.CreateTag(), HatchConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.NORMAL, null, 0f);
		list.AddRange(BaseHatchConfig.FoodDiet(SimHashes.Carbon.CreateTag(), HatchConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.GOOD_1, null, 0f));
		GameObject gameObject = BaseHatchConfig.SetupDiet(prefab, list, HatchConfig.CALORIES_PER_KG_OF_ORE, HatchConfig.MIN_POOP_SIZE_IN_KG);
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x060005AA RID: 1450 RVA: 0x0002C1F4 File Offset: 0x0002A3F4
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(HatchConfig.CreateHatch("Hatch", STRINGS.CREATURES.SPECIES.HATCH.NAME, STRINGS.CREATURES.SPECIES.HATCH.DESC, "hatch_kanim", false), this as IHasDlcRestrictions, "HatchEgg", STRINGS.CREATURES.SPECIES.HATCH.EGG_NAME, STRINGS.CREATURES.SPECIES.HATCH.DESC, "egg_hatch_kanim", HatchTuning.EGG_MASS, "HatchBaby", 60.000004f, 20f, HatchTuning.EGG_CHANCES_BASE, HatchConfig.EGG_SORT_ORDER, true, false, 1f, false);
	}

	// Token: 0x060005AB RID: 1451 RVA: 0x0002C274 File Offset: 0x0002A474
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060005AC RID: 1452 RVA: 0x0002C276 File Offset: 0x0002A476
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000443 RID: 1091
	public const string ID = "Hatch";

	// Token: 0x04000444 RID: 1092
	public const string BASE_TRAIT_ID = "HatchBaseTrait";

	// Token: 0x04000445 RID: 1093
	public const string EGG_ID = "HatchEgg";

	// Token: 0x04000446 RID: 1094
	private const SimHashes EMIT_ELEMENT = SimHashes.Carbon;

	// Token: 0x04000447 RID: 1095
	private static float KG_ORE_EATEN_PER_CYCLE = 140f;

	// Token: 0x04000448 RID: 1096
	private static float CALORIES_PER_KG_OF_ORE = HatchTuning.STANDARD_CALORIES_PER_CYCLE / HatchConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x04000449 RID: 1097
	private static float MIN_POOP_SIZE_IN_KG = 25f;

	// Token: 0x0400044A RID: 1098
	public static int EGG_SORT_ORDER = 0;
}
