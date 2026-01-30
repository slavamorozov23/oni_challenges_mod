using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200011F RID: 287
[EntityConfigOrder(1)]
public class DivergentBeetleConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000559 RID: 1369 RVA: 0x0002A900 File Offset: 0x00028B00
	public static GameObject CreateDivergentBeetle(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = EntityTemplates.ExtendEntityToWildCreature(BaseDivergentConfig.BaseDivergent(id, name, desc, 50f, anim_file, is_baby ? null : "critter_build_kanim", "DivergentBeetleBaseTrait", is_baby, 8f, null, "DivergentCropTended", 1, true, "critter_emotes_kanim"), DivergentTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("DivergentBeetleBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, DivergentTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -DivergentTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 75f, name, false, false, true));
		List<Diet.Info> diet_infos = BaseDivergentConfig.BasicSulfurDiet(SimHashes.Sucrose.CreateTag(), DivergentBeetleConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.NORMAL, null, 0f);
		GameObject gameObject = BaseDivergentConfig.SetupDiet(prefab, diet_infos, DivergentBeetleConfig.CALORIES_PER_KG_OF_ORE, DivergentBeetleConfig.MIN_POOP_SIZE_IN_KG);
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x0600055A RID: 1370 RVA: 0x0002AA5E File Offset: 0x00028C5E
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x0600055B RID: 1371 RVA: 0x0002AA65 File Offset: 0x00028C65
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x0600055C RID: 1372 RVA: 0x0002AA68 File Offset: 0x00028C68
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.ExtendEntityToFertileCreature(DivergentBeetleConfig.CreateDivergentBeetle("DivergentBeetle", STRINGS.CREATURES.SPECIES.DIVERGENT.VARIANT_BEETLE.NAME, STRINGS.CREATURES.SPECIES.DIVERGENT.VARIANT_BEETLE.DESC, "critter_kanim", false), this, "DivergentBeetleEgg", STRINGS.CREATURES.SPECIES.DIVERGENT.VARIANT_BEETLE.EGG_NAME, STRINGS.CREATURES.SPECIES.DIVERGENT.VARIANT_BEETLE.DESC, "egg_critter_kanim", DivergentTuning.EGG_MASS, "DivergentBeetleBaby", 45f, 15f, DivergentTuning.EGG_CHANCES_BEETLE, DivergentBeetleConfig.EGG_SORT_ORDER, true, false, 1f, false);
		gameObject.AddTag(GameTags.Creatures.Pollinator);
		return gameObject;
	}

	// Token: 0x0600055D RID: 1373 RVA: 0x0002AAEE File Offset: 0x00028CEE
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600055E RID: 1374 RVA: 0x0002AAF0 File Offset: 0x00028CF0
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040003E0 RID: 992
	public const string ID = "DivergentBeetle";

	// Token: 0x040003E1 RID: 993
	public const string BASE_TRAIT_ID = "DivergentBeetleBaseTrait";

	// Token: 0x040003E2 RID: 994
	public const string EGG_ID = "DivergentBeetleEgg";

	// Token: 0x040003E3 RID: 995
	private const float LIFESPAN = 75f;

	// Token: 0x040003E4 RID: 996
	private const SimHashes EMIT_ELEMENT = SimHashes.Sucrose;

	// Token: 0x040003E5 RID: 997
	private static float KG_ORE_EATEN_PER_CYCLE = 20f;

	// Token: 0x040003E6 RID: 998
	private static float CALORIES_PER_KG_OF_ORE = DivergentTuning.STANDARD_CALORIES_PER_CYCLE / DivergentBeetleConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x040003E7 RID: 999
	private static float MIN_POOP_SIZE_IN_KG = 4f;

	// Token: 0x040003E8 RID: 1000
	public static int EGG_SORT_ORDER = 0;
}
