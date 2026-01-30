using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200015D RID: 349
[EntityConfigOrder(2)]
public class PuftBleachstoneConfig : IEntityConfig
{
	// Token: 0x060006AA RID: 1706 RVA: 0x0002FE24 File Offset: 0x0002E024
	public static GameObject CreatePuftBleachstone(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = BasePuftConfig.BasePuft(id, name, desc, "PuftBleachstoneBaseTrait", anim_file, is_baby, "anti_", 273.15f, 333.15f, 223.15f, 373.15f);
		gameObject = EntityTemplates.ExtendEntityToWildCreature(gameObject, PuftTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("PuftBleachstoneBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, PuftTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -PuftTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 75f, name, false, false, true));
		gameObject = BasePuftConfig.SetupDiet(gameObject, SimHashes.ChlorineGas.CreateTag(), SimHashes.BleachStone.CreateTag(), PuftBleachstoneConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.GOOD_2, null, 0f, PuftBleachstoneConfig.MIN_POOP_SIZE_IN_KG);
		gameObject.AddOrGetDef<LureableMonitor.Def>().lures = new Tag[]
		{
			SimHashes.BleachStone.CreateTag(),
			GameTags.Creatures.FlyersLure
		};
		return gameObject;
	}

	// Token: 0x060006AB RID: 1707 RVA: 0x0002FFA0 File Offset: 0x0002E1A0
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(PuftBleachstoneConfig.CreatePuftBleachstone("PuftBleachstone", STRINGS.CREATURES.SPECIES.PUFT.VARIANT_BLEACHSTONE.NAME, STRINGS.CREATURES.SPECIES.PUFT.VARIANT_BLEACHSTONE.DESC, "puft_kanim", false), this as IHasDlcRestrictions, "PuftBleachstoneEgg", STRINGS.CREATURES.SPECIES.PUFT.VARIANT_BLEACHSTONE.EGG_NAME, STRINGS.CREATURES.SPECIES.PUFT.VARIANT_BLEACHSTONE.DESC, "egg_puft_kanim", PuftTuning.EGG_MASS, "PuftBleachstoneBaby", 45f, 15f, PuftTuning.EGG_CHANCES_BLEACHSTONE, PuftBleachstoneConfig.EGG_SORT_ORDER, true, false, 1f, false);
	}

	// Token: 0x060006AC RID: 1708 RVA: 0x00030020 File Offset: 0x0002E220
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060006AD RID: 1709 RVA: 0x00030022 File Offset: 0x0002E222
	public void OnSpawn(GameObject inst)
	{
		BasePuftConfig.OnSpawn(inst);
	}

	// Token: 0x04000508 RID: 1288
	public const string ID = "PuftBleachstone";

	// Token: 0x04000509 RID: 1289
	public const string BASE_TRAIT_ID = "PuftBleachstoneBaseTrait";

	// Token: 0x0400050A RID: 1290
	public const string EGG_ID = "PuftBleachstoneEgg";

	// Token: 0x0400050B RID: 1291
	public const SimHashes CONSUME_ELEMENT = SimHashes.ChlorineGas;

	// Token: 0x0400050C RID: 1292
	public const SimHashes EMIT_ELEMENT = SimHashes.BleachStone;

	// Token: 0x0400050D RID: 1293
	private static float KG_ORE_EATEN_PER_CYCLE = 30f;

	// Token: 0x0400050E RID: 1294
	private static float CALORIES_PER_KG_OF_ORE = PuftTuning.STANDARD_CALORIES_PER_CYCLE / PuftBleachstoneConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x0400050F RID: 1295
	private static float MIN_POOP_SIZE_IN_KG = 15f;

	// Token: 0x04000510 RID: 1296
	public static int EGG_SORT_ORDER = PuftConfig.EGG_SORT_ORDER + 3;
}
