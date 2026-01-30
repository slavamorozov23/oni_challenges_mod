using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000140 RID: 320
[EntityConfigOrder(2)]
public class LightBugPinkConfig : IEntityConfig
{
	// Token: 0x06000612 RID: 1554 RVA: 0x0002DAB4 File Offset: 0x0002BCB4
	public static GameObject CreateLightBug(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = BaseLightBugConfig.BaseLightBug(id, name, desc, anim_file, "LightBugPinkBaseTrait", LIGHT2D.LIGHTBUG_COLOR_PINK, DECOR.BONUS.TIER6, is_baby, "pnk_", "", 0f);
		EntityTemplates.ExtendEntityToWildCreature(prefab, LightBugTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("LightBugPinkBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, LightBugTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -LightBugTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 5f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 25f, name, false, false, true));
		HashSet<Tag> hashSet = new HashSet<Tag>();
		hashSet.Add(TagManager.Create("FriedMushroom"));
		hashSet.Add(TagManager.Create("SpiceBread"));
		hashSet.Add(TagManager.Create(PrickleFruitConfig.ID));
		hashSet.Add(TagManager.Create("GrilledPrickleFruit"));
		if (DlcManager.IsContentSubscribed("DLC2_ID"))
		{
			hashSet.Add(TagManager.Create("CookedPikeapple"));
		}
		hashSet.Add(TagManager.Create("Salsa"));
		hashSet.Add(SimHashes.Phosphorite.CreateTag());
		return BaseLightBugConfig.SetupDiet(prefab, hashSet, Tag.Invalid, LightBugPinkConfig.CALORIES_PER_KG_OF_ORE);
	}

	// Token: 0x06000613 RID: 1555 RVA: 0x0002DC68 File Offset: 0x0002BE68
	public GameObject CreatePrefab()
	{
		GameObject gameObject = LightBugPinkConfig.CreateLightBug("LightBugPink", STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_PINK.NAME, STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_PINK.DESC, "lightbug_kanim", false);
		EntityTemplates.ExtendEntityToFertileCreature(gameObject, this as IHasDlcRestrictions, "LightBugPinkEgg", STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_PINK.EGG_NAME, STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_PINK.DESC, "egg_lightbug_kanim", LightBugTuning.EGG_MASS, "LightBugPinkBaby", 15.000001f, 5f, LightBugTuning.EGG_CHANCES_PINK, LightBugPinkConfig.EGG_SORT_ORDER, true, false, 1f, false, false, LightBugTuning.EGG_MASS * 0.2f);
		return gameObject;
	}

	// Token: 0x06000614 RID: 1556 RVA: 0x0002DCF6 File Offset: 0x0002BEF6
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000615 RID: 1557 RVA: 0x0002DCF8 File Offset: 0x0002BEF8
	public void OnSpawn(GameObject inst)
	{
		BaseLightBugConfig.SetupLoopingSounds(inst);
	}

	// Token: 0x04000492 RID: 1170
	public const string ID = "LightBugPink";

	// Token: 0x04000493 RID: 1171
	public const string BASE_TRAIT_ID = "LightBugPinkBaseTrait";

	// Token: 0x04000494 RID: 1172
	public const string EGG_ID = "LightBugPinkEgg";

	// Token: 0x04000495 RID: 1173
	private static float KG_ORE_EATEN_PER_CYCLE = 1f;

	// Token: 0x04000496 RID: 1174
	private static float CALORIES_PER_KG_OF_ORE = LightBugTuning.STANDARD_CALORIES_PER_CYCLE / LightBugPinkConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x04000497 RID: 1175
	public static int EGG_SORT_ORDER = LightBugConfig.EGG_SORT_ORDER + 3;
}
