using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200013C RID: 316
[EntityConfigOrder(2)]
public class LightBugCrystalConfig : IEntityConfig
{
	// Token: 0x060005FE RID: 1534 RVA: 0x0002D570 File Offset: 0x0002B770
	public static GameObject CreateLightBug(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = BaseLightBugConfig.BaseLightBug(id, name, desc, anim_file, "LightBugCrystalBaseTrait", LIGHT2D.LIGHTBUG_COLOR_CRYSTAL, DECOR.BONUS.TIER8, is_baby, "cry_", "", 0f);
		EntityTemplates.ExtendEntityToWildCreature(gameObject, LightBugTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("LightBugCrystalBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, LightBugTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -LightBugTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 5f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 75f, name, false, false, true));
		gameObject = BaseLightBugConfig.SetupDiet(gameObject, new HashSet<Tag>
		{
			TagManager.Create("CookedMeat"),
			SimHashes.Diamond.CreateTag()
		}, Tag.Invalid, LightBugCrystalConfig.CALORIES_PER_KG_OF_ORE);
		gameObject.AddOrGetDef<LureableMonitor.Def>().lures = new Tag[]
		{
			SimHashes.Diamond.CreateTag(),
			GameTags.Creatures.FlyersLure
		};
		return gameObject;
	}

	// Token: 0x060005FF RID: 1535 RVA: 0x0002D6F8 File Offset: 0x0002B8F8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = LightBugCrystalConfig.CreateLightBug("LightBugCrystal", STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_CRYSTAL.NAME, STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_CRYSTAL.DESC, "lightbug_kanim", false);
		EntityTemplates.ExtendEntityToFertileCreature(gameObject, this as IHasDlcRestrictions, "LightBugCrystalEgg", STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_CRYSTAL.EGG_NAME, STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_CRYSTAL.DESC, "egg_lightbug_kanim", LightBugTuning.EGG_MASS, "LightBugCrystalBaby", 45f, 15f, LightBugTuning.EGG_CHANCES_CRYSTAL, LightBugCrystalConfig.EGG_SORT_ORDER, true, false, 1f, false, false, LightBugTuning.EGG_MASS * 0.2f);
		return gameObject;
	}

	// Token: 0x06000600 RID: 1536 RVA: 0x0002D786 File Offset: 0x0002B986
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000601 RID: 1537 RVA: 0x0002D788 File Offset: 0x0002B988
	public void OnSpawn(GameObject inst)
	{
		BaseLightBugConfig.SetupLoopingSounds(inst);
	}

	// Token: 0x04000484 RID: 1156
	public const string ID = "LightBugCrystal";

	// Token: 0x04000485 RID: 1157
	public const string BASE_TRAIT_ID = "LightBugCrystalBaseTrait";

	// Token: 0x04000486 RID: 1158
	public const string EGG_ID = "LightBugCrystalEgg";

	// Token: 0x04000487 RID: 1159
	private static float KG_ORE_EATEN_PER_CYCLE = 1f;

	// Token: 0x04000488 RID: 1160
	private static float CALORIES_PER_KG_OF_ORE = LightBugTuning.STANDARD_CALORIES_PER_CYCLE / LightBugCrystalConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x04000489 RID: 1161
	public static int EGG_SORT_ORDER = LightBugConfig.EGG_SORT_ORDER + 7;
}
