using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200013A RID: 314
public class LightBugConfig : IEntityConfig
{
	// Token: 0x060005F4 RID: 1524 RVA: 0x0002D2B4 File Offset: 0x0002B4B4
	public static GameObject CreateLightBug(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = BaseLightBugConfig.BaseLightBug(id, name, desc, anim_file, "LightBugBaseTrait", LIGHT2D.LIGHTBUG_COLOR, DECOR.BONUS.TIER4, is_baby, null, "", 0f);
		EntityTemplates.ExtendEntityToWildCreature(prefab, LightBugTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("LightBugBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, LightBugTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -LightBugTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 5f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 25f, name, false, false, true));
		HashSet<Tag> hashSet = new HashSet<Tag>();
		hashSet.Add(TagManager.Create(PrickleFruitConfig.ID));
		hashSet.Add(TagManager.Create("GrilledPrickleFruit"));
		if (DlcManager.IsContentSubscribed("DLC2_ID"))
		{
			hashSet.Add(TagManager.Create("HardSkinBerry"));
			hashSet.Add(TagManager.Create("CookedPikeapple"));
		}
		hashSet.Add(SimHashes.Phosphorite.CreateTag());
		GameObject gameObject = BaseLightBugConfig.SetupDiet(prefab, hashSet, Tag.Invalid, LightBugConfig.CALORIES_PER_KG_OF_ORE);
		gameObject.AddTag(GameTags.OriginalCreature);
		gameObject.GetComponent<KPrefabID>().AddTag(GameTags.LightSource, false);
		return gameObject;
	}

	// Token: 0x060005F5 RID: 1525 RVA: 0x0002D460 File Offset: 0x0002B660
	public GameObject CreatePrefab()
	{
		GameObject gameObject = LightBugConfig.CreateLightBug("LightBug", STRINGS.CREATURES.SPECIES.LIGHTBUG.NAME, STRINGS.CREATURES.SPECIES.LIGHTBUG.DESC, "lightbug_kanim", false);
		EntityTemplates.ExtendEntityToFertileCreature(gameObject, this as IHasDlcRestrictions, "LightBugEgg", STRINGS.CREATURES.SPECIES.LIGHTBUG.EGG_NAME, STRINGS.CREATURES.SPECIES.LIGHTBUG.DESC, "egg_lightbug_kanim", LightBugTuning.EGG_MASS, "LightBugBaby", 15.000001f, 5f, LightBugTuning.EGG_CHANCES_BASE, LightBugConfig.EGG_SORT_ORDER, true, false, 1f, false, false, LightBugTuning.EGG_MASS * 0.2f);
		return gameObject;
	}

	// Token: 0x060005F6 RID: 1526 RVA: 0x0002D4EE File Offset: 0x0002B6EE
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060005F7 RID: 1527 RVA: 0x0002D4F0 File Offset: 0x0002B6F0
	public void OnSpawn(GameObject inst)
	{
		BaseLightBugConfig.SetupLoopingSounds(inst);
	}

	// Token: 0x0400047D RID: 1149
	public const string ID = "LightBug";

	// Token: 0x0400047E RID: 1150
	public const string BASE_TRAIT_ID = "LightBugBaseTrait";

	// Token: 0x0400047F RID: 1151
	public const string EGG_ID = "LightBugEgg";

	// Token: 0x04000480 RID: 1152
	private static float KG_ORE_EATEN_PER_CYCLE = 0.166f;

	// Token: 0x04000481 RID: 1153
	private static float CALORIES_PER_KG_OF_ORE = LightBugTuning.STANDARD_CALORIES_PER_CYCLE / LightBugConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x04000482 RID: 1154
	public static int EGG_SORT_ORDER = 100;
}
