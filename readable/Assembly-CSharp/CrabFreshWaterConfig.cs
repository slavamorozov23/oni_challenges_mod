using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200011A RID: 282
[EntityConfigOrder(2)]
public class CrabFreshWaterConfig : IEntityConfig
{
	// Token: 0x0600053D RID: 1341 RVA: 0x0002A0A8 File Offset: 0x000282A8
	public static GameObject CreateCrabFreshWater(string id, string name, string desc, string anim_file, bool is_baby, string deathDropID = null, int deathDropCount = 0)
	{
		GameObject prefab = EntityTemplates.ExtendEntityToWildCreature(BaseCrabConfig.BaseCrab(id, name, desc, anim_file, "CrabFreshWaterBaseTrait", is_baby, CrabFreshWaterConfig.animPrefix, deathDropID, (float)deathDropCount), CrabTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("CrabFreshWaterBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, CrabTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -CrabTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		List<Diet.Info> diet_infos = BaseCrabConfig.DietWithSlime(SimHashes.Sand.CreateTag(), CrabFreshWaterConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.NORMAL, null, 0f);
		return BaseCrabConfig.SetupDiet(prefab, diet_infos, CrabFreshWaterConfig.CALORIES_PER_KG_OF_ORE, CrabFreshWaterConfig.MIN_POOP_SIZE_IN_KG);
	}

	// Token: 0x0600053E RID: 1342 RVA: 0x0002A1E4 File Offset: 0x000283E4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = CrabFreshWaterConfig.CreateCrabFreshWater("CrabFreshWater", STRINGS.CREATURES.SPECIES.CRAB.VARIANT_FRESH_WATER.NAME, STRINGS.CREATURES.SPECIES.CRAB.VARIANT_FRESH_WATER.DESC, "pincher_kanim", false, "ShellfishMeat", 4);
		gameObject = EntityTemplates.ExtendEntityToFertileCreature(gameObject, this as IHasDlcRestrictions, "CrabFreshWaterEgg", STRINGS.CREATURES.SPECIES.CRAB.VARIANT_FRESH_WATER.EGG_NAME, STRINGS.CREATURES.SPECIES.CRAB.VARIANT_FRESH_WATER.DESC, "egg_pincher_kanim", CrabTuning.EGG_MASS, "CrabFreshWaterBaby", 60.000004f, 20f, CrabTuning.EGG_CHANCES_FRESH, CrabFreshWaterConfig.EGG_SORT_ORDER, true, false, 1f, false);
		EggProtectionMonitor.Def def = gameObject.AddOrGetDef<EggProtectionMonitor.Def>();
		def.allyTags = new Tag[]
		{
			GameTags.Creatures.CrabFriend
		};
		def.animPrefix = CrabFreshWaterConfig.animPrefix;
		DiseaseEmitter diseaseEmitter = gameObject.AddComponent<DiseaseEmitter>();
		List<Disease> list = new List<Disease>
		{
			Db.Get().Diseases.FoodGerms,
			Db.Get().Diseases.PollenGerms,
			Db.Get().Diseases.SlimeGerms,
			Db.Get().Diseases.ZombieSpores
		};
		if (DlcManager.IsExpansion1Active())
		{
			list.Add(Db.Get().Diseases.RadiationPoisoning);
		}
		diseaseEmitter.SetDiseases(list);
		diseaseEmitter.emitRange = 2;
		diseaseEmitter.emitCount = -1 * Mathf.RoundToInt((float)DUPLICANTSTATS.STANDARD.Secretions.DISEASE_PER_PEE / 600f * 6f * 2f * 4f / 9f);
		CleaningMonitor.Def def2 = gameObject.AddOrGetDef<CleaningMonitor.Def>();
		def2.elementState = Element.State.Liquid;
		def2.cellOffsets = new CellOffset[]
		{
			new CellOffset(1, 0),
			new CellOffset(-1, 0),
			new CellOffset(0, 1),
			new CellOffset(-1, 1),
			new CellOffset(1, 1)
		};
		def2.coolDown = 30f;
		return gameObject;
	}

	// Token: 0x0600053F RID: 1343 RVA: 0x0002A3C7 File Offset: 0x000285C7
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000540 RID: 1344 RVA: 0x0002A3C9 File Offset: 0x000285C9
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040003C8 RID: 968
	public const string ID = "CrabFreshWater";

	// Token: 0x040003C9 RID: 969
	public const string BASE_TRAIT_ID = "CrabFreshWaterBaseTrait";

	// Token: 0x040003CA RID: 970
	public const string EGG_ID = "CrabFreshWaterEgg";

	// Token: 0x040003CB RID: 971
	private const SimHashes EMIT_ELEMENT = SimHashes.Sand;

	// Token: 0x040003CC RID: 972
	private static float KG_ORE_EATEN_PER_CYCLE = 70f;

	// Token: 0x040003CD RID: 973
	private static float CALORIES_PER_KG_OF_ORE = CrabTuning.STANDARD_CALORIES_PER_CYCLE / CrabFreshWaterConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x040003CE RID: 974
	private static float MIN_POOP_SIZE_IN_KG = 25f;

	// Token: 0x040003CF RID: 975
	public static int EGG_SORT_ORDER = 0;

	// Token: 0x040003D0 RID: 976
	private static string animPrefix = "fresh_";
}
