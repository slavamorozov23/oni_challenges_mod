using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200016E RID: 366
public class StaterpillarConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x0600070A RID: 1802 RVA: 0x000317AC File Offset: 0x0002F9AC
	public static GameObject CreateStaterpillar(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = EntityTemplates.ExtendEntityToWildCreature(BaseStaterpillarConfig.BaseStaterpillar(id, name, desc, anim_file, "StaterpillarBaseTrait", is_baby, ObjectLayer.Wire, StaterpillarGeneratorConfig.ID, Tag.Invalid, null, 283.15f, 313.15f, 173.15f, 373.15f, null), TUNING.CREATURES.SPACE_REQUIREMENTS.TIER3);
		Trait trait = Db.Get().CreateTrait("StaterpillarBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, StaterpillarTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -StaterpillarTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		List<Diet.Info> list = new List<Diet.Info>();
		list.AddRange(BaseStaterpillarConfig.RawMetalDiet(SimHashes.Hydrogen.CreateTag(), StaterpillarConfig.CALORIES_PER_KG_OF_ORE, StaterpillarTuning.POOP_CONVERSTION_RATE, null, 0f));
		list.AddRange(BaseStaterpillarConfig.RefinedMetalDiet(SimHashes.Hydrogen.CreateTag(), StaterpillarConfig.CALORIES_PER_KG_OF_ORE, StaterpillarTuning.POOP_CONVERSTION_RATE, null, 0f));
		GameObject gameObject = BaseStaterpillarConfig.SetupDiet(prefab, list);
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x0600070B RID: 1803 RVA: 0x0003192F File Offset: 0x0002FB2F
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x0600070C RID: 1804 RVA: 0x00031936 File Offset: 0x0002FB36
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x0600070D RID: 1805 RVA: 0x0003193C File Offset: 0x0002FB3C
	public virtual GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(StaterpillarConfig.CreateStaterpillar("Staterpillar", STRINGS.CREATURES.SPECIES.STATERPILLAR.NAME, STRINGS.CREATURES.SPECIES.STATERPILLAR.DESC, "caterpillar_kanim", false), this, "StaterpillarEgg", STRINGS.CREATURES.SPECIES.STATERPILLAR.EGG_NAME, STRINGS.CREATURES.SPECIES.STATERPILLAR.DESC, "egg_caterpillar_kanim", StaterpillarTuning.EGG_MASS, "StaterpillarBaby", 60.000004f, 20f, StaterpillarTuning.EGG_CHANCES_BASE, 0, true, false, 1f, false);
	}

	// Token: 0x0600070E RID: 1806 RVA: 0x000319B3 File Offset: 0x0002FBB3
	public void OnPrefabInit(GameObject prefab)
	{
		prefab.GetComponent<KBatchedAnimController>().SetSymbolVisiblity("gulp", false);
	}

	// Token: 0x0600070F RID: 1807 RVA: 0x000319CB File Offset: 0x0002FBCB
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000561 RID: 1377
	public const string ID = "Staterpillar";

	// Token: 0x04000562 RID: 1378
	public const string BASE_TRAIT_ID = "StaterpillarBaseTrait";

	// Token: 0x04000563 RID: 1379
	public const string EGG_ID = "StaterpillarEgg";

	// Token: 0x04000564 RID: 1380
	public const int EGG_SORT_ORDER = 0;

	// Token: 0x04000565 RID: 1381
	private static float KG_ORE_EATEN_PER_CYCLE = 60f;

	// Token: 0x04000566 RID: 1382
	private static float CALORIES_PER_KG_OF_ORE = StaterpillarTuning.STANDARD_CALORIES_PER_CYCLE / StaterpillarConfig.KG_ORE_EATEN_PER_CYCLE;
}
