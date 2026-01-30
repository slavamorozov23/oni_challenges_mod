using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000132 RID: 306
[EntityConfigOrder(2)]
public class HatchVeggieConfig : IEntityConfig
{
	// Token: 0x060005C8 RID: 1480 RVA: 0x0002C7C4 File Offset: 0x0002A9C4
	public static GameObject CreateHatch(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = EntityTemplates.ExtendEntityToWildCreature(BaseHatchConfig.BaseHatch(id, name, desc, anim_file, "HatchVeggieBaseTrait", is_baby, "veg_"), HatchTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("HatchVeggieBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, HatchTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -HatchTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		List<Diet.Info> list = BaseHatchConfig.VeggieDiet(SimHashes.Carbon.CreateTag(), HatchVeggieConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.GOOD_3, null, 0f);
		list.AddRange(BaseHatchConfig.FoodDiet(SimHashes.Carbon.CreateTag(), HatchVeggieConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.GOOD_3, null, 0f));
		return BaseHatchConfig.SetupDiet(prefab, list, HatchVeggieConfig.CALORIES_PER_KG_OF_ORE, HatchVeggieConfig.MIN_POOP_SIZE_IN_KG);
	}

	// Token: 0x060005C9 RID: 1481 RVA: 0x0002C920 File Offset: 0x0002AB20
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(HatchVeggieConfig.CreateHatch("HatchVeggie", STRINGS.CREATURES.SPECIES.HATCH.VARIANT_VEGGIE.NAME, STRINGS.CREATURES.SPECIES.HATCH.VARIANT_VEGGIE.DESC, "hatch_kanim", false), this as IHasDlcRestrictions, "HatchVeggieEgg", STRINGS.CREATURES.SPECIES.HATCH.VARIANT_VEGGIE.EGG_NAME, STRINGS.CREATURES.SPECIES.HATCH.VARIANT_VEGGIE.DESC, "egg_hatch_kanim", HatchTuning.EGG_MASS, "HatchVeggieBaby", 60.000004f, 20f, HatchTuning.EGG_CHANCES_VEGGIE, HatchVeggieConfig.EGG_SORT_ORDER, true, false, 1f, false);
	}

	// Token: 0x060005CA RID: 1482 RVA: 0x0002C9A0 File Offset: 0x0002ABA0
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060005CB RID: 1483 RVA: 0x0002C9A2 File Offset: 0x0002ABA2
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400045D RID: 1117
	public const string ID = "HatchVeggie";

	// Token: 0x0400045E RID: 1118
	public const string BASE_TRAIT_ID = "HatchVeggieBaseTrait";

	// Token: 0x0400045F RID: 1119
	public const string EGG_ID = "HatchVeggieEgg";

	// Token: 0x04000460 RID: 1120
	private const SimHashes EMIT_ELEMENT = SimHashes.Carbon;

	// Token: 0x04000461 RID: 1121
	private static float KG_ORE_EATEN_PER_CYCLE = 140f;

	// Token: 0x04000462 RID: 1122
	private static float CALORIES_PER_KG_OF_ORE = HatchTuning.STANDARD_CALORIES_PER_CYCLE / HatchVeggieConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x04000463 RID: 1123
	private static float MIN_POOP_SIZE_IN_KG = 50f;

	// Token: 0x04000464 RID: 1124
	public static int EGG_SORT_ORDER = HatchConfig.EGG_SORT_ORDER + 1;
}
