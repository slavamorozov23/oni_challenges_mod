using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000161 RID: 353
[EntityConfigOrder(2)]
public class PuftOxyliteConfig : IEntityConfig
{
	// Token: 0x060006BE RID: 1726 RVA: 0x00030330 File Offset: 0x0002E530
	public static GameObject CreatePuftOxylite(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = BasePuftConfig.BasePuft(id, name, desc, "PuftOxyliteBaseTrait", anim_file, is_baby, "com_", 273.15f, 333.15f, 223.15f, 373.15f);
		gameObject = EntityTemplates.ExtendEntityToWildCreature(gameObject, PuftTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("PuftOxyliteBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, PuftTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -PuftTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 75f, name, false, false, true));
		gameObject = BasePuftConfig.SetupDiet(gameObject, SimHashes.Oxygen.CreateTag(), SimHashes.OxyRock.CreateTag(), PuftOxyliteConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.GOOD_2, null, 0f, PuftOxyliteConfig.MIN_POOP_SIZE_IN_KG);
		gameObject.AddOrGetDef<LureableMonitor.Def>().lures = new Tag[]
		{
			SimHashes.OxyRock.CreateTag(),
			GameTags.Creatures.FlyersLure
		};
		return gameObject;
	}

	// Token: 0x060006BF RID: 1727 RVA: 0x000304AC File Offset: 0x0002E6AC
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(PuftOxyliteConfig.CreatePuftOxylite("PuftOxylite", STRINGS.CREATURES.SPECIES.PUFT.VARIANT_OXYLITE.NAME, STRINGS.CREATURES.SPECIES.PUFT.VARIANT_OXYLITE.DESC, "puft_kanim", false), this as IHasDlcRestrictions, "PuftOxyliteEgg", STRINGS.CREATURES.SPECIES.PUFT.VARIANT_OXYLITE.EGG_NAME, STRINGS.CREATURES.SPECIES.PUFT.VARIANT_OXYLITE.DESC, "egg_puft_kanim", PuftTuning.EGG_MASS, "PuftOxyliteBaby", 45f, 15f, PuftTuning.EGG_CHANCES_OXYLITE, PuftOxyliteConfig.EGG_SORT_ORDER, true, false, 1f, false);
	}

	// Token: 0x060006C0 RID: 1728 RVA: 0x0003052C File Offset: 0x0002E72C
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060006C1 RID: 1729 RVA: 0x0003052E File Offset: 0x0002E72E
	public void OnSpawn(GameObject inst)
	{
		BasePuftConfig.OnSpawn(inst);
	}

	// Token: 0x0400051E RID: 1310
	public const string ID = "PuftOxylite";

	// Token: 0x0400051F RID: 1311
	public const string BASE_TRAIT_ID = "PuftOxyliteBaseTrait";

	// Token: 0x04000520 RID: 1312
	public const string EGG_ID = "PuftOxyliteEgg";

	// Token: 0x04000521 RID: 1313
	public const SimHashes CONSUME_ELEMENT = SimHashes.Oxygen;

	// Token: 0x04000522 RID: 1314
	public const SimHashes EMIT_ELEMENT = SimHashes.OxyRock;

	// Token: 0x04000523 RID: 1315
	private static float KG_ORE_EATEN_PER_CYCLE = 50f;

	// Token: 0x04000524 RID: 1316
	private static float CALORIES_PER_KG_OF_ORE = PuftTuning.STANDARD_CALORIES_PER_CYCLE / PuftOxyliteConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x04000525 RID: 1317
	private static float MIN_POOP_SIZE_IN_KG = 25f;

	// Token: 0x04000526 RID: 1318
	public static int EGG_SORT_ORDER = PuftConfig.EGG_SORT_ORDER + 2;
}
