using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000136 RID: 310
[EntityConfigOrder(2)]
public class LightBugBlackConfig : IEntityConfig
{
	// Token: 0x060005E0 RID: 1504 RVA: 0x0002CD1C File Offset: 0x0002AF1C
	public static GameObject CreateLightBug(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = BaseLightBugConfig.BaseLightBug(id, name, desc, anim_file, "LightBugBlackBaseTrait", Color.black, DECOR.BONUS.TIER7, is_baby, "blk_", "", 0f);
		EntityTemplates.ExtendEntityToWildCreature(gameObject, LightBugTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("LightBugBlackBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, LightBugTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -LightBugTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 5f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 75f, name, false, false, true));
		gameObject = BaseLightBugConfig.SetupDiet(gameObject, new HashSet<Tag>
		{
			TagManager.Create("Salsa"),
			TagManager.Create("Meat"),
			TagManager.Create("CookedMeat"),
			SimHashes.Katairite.CreateTag(),
			SimHashes.Phosphorus.CreateTag()
		}, Tag.Invalid, LightBugBlackConfig.CALORIES_PER_KG_OF_ORE);
		gameObject.AddOrGetDef<LureableMonitor.Def>().lures = new Tag[]
		{
			SimHashes.Phosphorus.CreateTag(),
			GameTags.Creatures.FlyersLure
		};
		return gameObject;
	}

	// Token: 0x060005E1 RID: 1505 RVA: 0x0002CED4 File Offset: 0x0002B0D4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = LightBugBlackConfig.CreateLightBug("LightBugBlack", STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_BLACK.NAME, STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_BLACK.DESC, "lightbug_kanim", false);
		EntityTemplates.ExtendEntityToFertileCreature(gameObject, this as IHasDlcRestrictions, "LightBugBlackEgg", STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_BLACK.EGG_NAME, STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_BLACK.DESC, "egg_lightbug_kanim", LightBugTuning.EGG_MASS, "LightBugBlackBaby", 45f, 15f, LightBugTuning.EGG_CHANCES_BLACK, LightBugBlackConfig.EGG_SORT_ORDER, true, false, 1f, false, false, LightBugTuning.EGG_MASS * 0.2f);
		return gameObject;
	}

	// Token: 0x060005E2 RID: 1506 RVA: 0x0002CF62 File Offset: 0x0002B162
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060005E3 RID: 1507 RVA: 0x0002CF64 File Offset: 0x0002B164
	public void OnSpawn(GameObject inst)
	{
		BaseLightBugConfig.SetupLoopingSounds(inst);
	}

	// Token: 0x0400046F RID: 1135
	public const string ID = "LightBugBlack";

	// Token: 0x04000470 RID: 1136
	public const string BASE_TRAIT_ID = "LightBugBlackBaseTrait";

	// Token: 0x04000471 RID: 1137
	public const string EGG_ID = "LightBugBlackEgg";

	// Token: 0x04000472 RID: 1138
	private static float KG_ORE_EATEN_PER_CYCLE = 1f;

	// Token: 0x04000473 RID: 1139
	private static float CALORIES_PER_KG_OF_ORE = LightBugTuning.STANDARD_CALORIES_PER_CYCLE / LightBugBlackConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x04000474 RID: 1140
	public static int EGG_SORT_ORDER = LightBugConfig.EGG_SORT_ORDER + 5;
}
