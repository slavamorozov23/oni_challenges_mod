using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000130 RID: 304
[EntityConfigOrder(2)]
public class HatchMetalConfig : IEntityConfig
{
	// Token: 0x17000014 RID: 20
	// (get) Token: 0x060005BD RID: 1469 RVA: 0x0002C55C File Offset: 0x0002A75C
	public static HashSet<Tag> METAL_ORE_TAGS
	{
		get
		{
			return new HashSet<Tag>(GameTags.BasicMetalOres)
			{
				SimHashes.GoldAmalgam.CreateTag(),
				SimHashes.Wolframite.CreateTag()
			};
		}
	}

	// Token: 0x060005BE RID: 1470 RVA: 0x0002C58C File Offset: 0x0002A78C
	public static GameObject CreateHatch(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = EntityTemplates.ExtendEntityToWildCreature(BaseHatchConfig.BaseHatch(id, name, desc, anim_file, "HatchMetalBaseTrait", is_baby, "mtl_"), HatchTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("HatchMetalBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, HatchTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -HatchTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 400f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		List<Diet.Info> diet_infos = BaseHatchConfig.MetalDiet(GameTags.Metal, HatchMetalConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.GOOD_1, null, 0f);
		return BaseHatchConfig.SetupDiet(prefab, diet_infos, HatchMetalConfig.CALORIES_PER_KG_OF_ORE, HatchMetalConfig.MIN_POOP_SIZE_IN_KG);
	}

	// Token: 0x060005BF RID: 1471 RVA: 0x0002C6BC File Offset: 0x0002A8BC
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(HatchMetalConfig.CreateHatch("HatchMetal", STRINGS.CREATURES.SPECIES.HATCH.VARIANT_METAL.NAME, STRINGS.CREATURES.SPECIES.HATCH.VARIANT_METAL.DESC, "hatch_kanim", false), this as IHasDlcRestrictions, "HatchMetalEgg", STRINGS.CREATURES.SPECIES.HATCH.VARIANT_METAL.EGG_NAME, STRINGS.CREATURES.SPECIES.HATCH.VARIANT_METAL.DESC, "egg_hatch_kanim", HatchTuning.EGG_MASS, "HatchMetalBaby", 60.000004f, 20f, HatchTuning.EGG_CHANCES_METAL, HatchMetalConfig.EGG_SORT_ORDER, true, false, 1f, false);
	}

	// Token: 0x060005C0 RID: 1472 RVA: 0x0002C73C File Offset: 0x0002A93C
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060005C1 RID: 1473 RVA: 0x0002C73E File Offset: 0x0002A93E
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000455 RID: 1109
	public const string ID = "HatchMetal";

	// Token: 0x04000456 RID: 1110
	public const string BASE_TRAIT_ID = "HatchMetalBaseTrait";

	// Token: 0x04000457 RID: 1111
	public const string EGG_ID = "HatchMetalEgg";

	// Token: 0x04000458 RID: 1112
	private static float KG_ORE_EATEN_PER_CYCLE = 100f;

	// Token: 0x04000459 RID: 1113
	private static float CALORIES_PER_KG_OF_ORE = HatchTuning.STANDARD_CALORIES_PER_CYCLE / HatchMetalConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x0400045A RID: 1114
	private static float MIN_POOP_SIZE_IN_KG = 10f;

	// Token: 0x0400045B RID: 1115
	public static int EGG_SORT_ORDER = HatchConfig.EGG_SORT_ORDER + 3;
}
