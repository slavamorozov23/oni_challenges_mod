using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200015B RID: 347
[EntityConfigOrder(2)]
public class PuftAlphaConfig : IEntityConfig
{
	// Token: 0x060006A0 RID: 1696 RVA: 0x0002FAD0 File Offset: 0x0002DCD0
	public static GameObject CreatePuftAlpha(string id, string name, string desc, string anim_file, bool is_baby)
	{
		string symbol_override_prefix = "alp_";
		GameObject gameObject = BasePuftConfig.BasePuft(id, name, desc, "PuftAlphaBaseTrait", anim_file, is_baby, symbol_override_prefix, 293.15f, 313.15f, 223.15f, 373.15f);
		EntityTemplates.ExtendEntityToWildCreature(gameObject, PuftTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("PuftAlphaBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, PuftTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -PuftTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 75f, name, false, false, true));
		gameObject = BasePuftConfig.SetupDiet(gameObject, new List<Diet.Info>
		{
			new Diet.Info(new HashSet<Tag>(new Tag[]
			{
				SimHashes.ContaminatedOxygen.CreateTag()
			}), SimHashes.SlimeMold.CreateTag(), PuftAlphaConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.BAD_2, "SlimeLung", 0f, false, Diet.Info.FoodType.EatSolid, false, null),
			new Diet.Info(new HashSet<Tag>(new Tag[]
			{
				SimHashes.ChlorineGas.CreateTag()
			}), SimHashes.BleachStone.CreateTag(), PuftAlphaConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.BAD_2, "SlimeLung", 0f, false, Diet.Info.FoodType.EatSolid, false, null),
			new Diet.Info(new HashSet<Tag>(new Tag[]
			{
				SimHashes.Oxygen.CreateTag()
			}), SimHashes.OxyRock.CreateTag(), PuftAlphaConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.BAD_2, "SlimeLung", 0f, false, Diet.Info.FoodType.EatSolid, false, null)
		}.ToArray(), PuftAlphaConfig.CALORIES_PER_KG_OF_ORE, PuftAlphaConfig.MIN_POOP_SIZE_IN_KG);
		gameObject.AddOrGet<DiseaseSourceVisualizer>().alwaysShowDisease = "SlimeLung";
		return gameObject;
	}

	// Token: 0x060006A1 RID: 1697 RVA: 0x0002FCF8 File Offset: 0x0002DEF8
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(PuftAlphaConfig.CreatePuftAlpha("PuftAlpha", STRINGS.CREATURES.SPECIES.PUFT.VARIANT_ALPHA.NAME, STRINGS.CREATURES.SPECIES.PUFT.VARIANT_ALPHA.DESC, "puft_kanim", false), this as IHasDlcRestrictions, "PuftAlphaEgg", STRINGS.CREATURES.SPECIES.PUFT.VARIANT_ALPHA.EGG_NAME, STRINGS.CREATURES.SPECIES.PUFT.VARIANT_ALPHA.DESC, "egg_puft_kanim", PuftTuning.EGG_MASS, "PuftAlphaBaby", 45f, 15f, PuftTuning.EGG_CHANCES_ALPHA, PuftAlphaConfig.EGG_SORT_ORDER, true, false, 1f, false);
	}

	// Token: 0x060006A2 RID: 1698 RVA: 0x0002FD78 File Offset: 0x0002DF78
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<KBatchedAnimController>().animScale *= 1.1f;
	}

	// Token: 0x060006A3 RID: 1699 RVA: 0x0002FD91 File Offset: 0x0002DF91
	public void OnSpawn(GameObject inst)
	{
		BasePuftConfig.OnSpawn(inst);
	}

	// Token: 0x040004FC RID: 1276
	public const string ID = "PuftAlpha";

	// Token: 0x040004FD RID: 1277
	public const string BASE_TRAIT_ID = "PuftAlphaBaseTrait";

	// Token: 0x040004FE RID: 1278
	public const string EGG_ID = "PuftAlphaEgg";

	// Token: 0x040004FF RID: 1279
	public const SimHashes CONSUME_ELEMENT = SimHashes.ContaminatedOxygen;

	// Token: 0x04000500 RID: 1280
	public const SimHashes EMIT_ELEMENT = SimHashes.SlimeMold;

	// Token: 0x04000501 RID: 1281
	public const string EMIT_DISEASE = "SlimeLung";

	// Token: 0x04000502 RID: 1282
	public const float EMIT_DISEASE_PER_KG = 0f;

	// Token: 0x04000503 RID: 1283
	private static float KG_ORE_EATEN_PER_CYCLE = 30f;

	// Token: 0x04000504 RID: 1284
	private static float CALORIES_PER_KG_OF_ORE = PuftTuning.STANDARD_CALORIES_PER_CYCLE / PuftAlphaConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x04000505 RID: 1285
	private static float MIN_POOP_SIZE_IN_KG = 5f;

	// Token: 0x04000506 RID: 1286
	public static int EGG_SORT_ORDER = PuftConfig.EGG_SORT_ORDER + 1;
}
