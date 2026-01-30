using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200011C RID: 284
[EntityConfigOrder(2)]
public class CrabWoodConfig : IEntityConfig
{
	// Token: 0x06000547 RID: 1351 RVA: 0x0002A468 File Offset: 0x00028668
	public static GameObject CreateCrabWood(string id, string name, string desc, string anim_file, bool is_baby, string deathDropID = "CrabWoodShell", float deathDropCount = 5f)
	{
		GameObject prefab = EntityTemplates.ExtendEntityToWildCreature(BaseCrabConfig.BaseCrab(id, name, desc, anim_file, "CrabWoodBaseTrait", is_baby, CrabWoodConfig.animPrefix, deathDropID, deathDropCount), CrabTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("CrabWoodBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, CrabTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -CrabTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		List<Diet.Info> diet_infos = BaseCrabConfig.DietWithSlime(SimHashes.Sand.CreateTag(), CrabWoodConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.BAD_1, null, 0f);
		return BaseCrabConfig.SetupDiet(prefab, diet_infos, CrabWoodConfig.CALORIES_PER_KG_OF_ORE, CrabWoodConfig.MIN_POOP_SIZE_IN_KG);
	}

	// Token: 0x06000548 RID: 1352 RVA: 0x0002A5A4 File Offset: 0x000287A4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = CrabWoodConfig.CreateCrabWood("CrabWood", STRINGS.CREATURES.SPECIES.CRAB.VARIANT_WOOD.NAME, STRINGS.CREATURES.SPECIES.CRAB.VARIANT_WOOD.DESC, "pincher_kanim", false, "CrabWoodShell", 500f);
		gameObject = EntityTemplates.ExtendEntityToFertileCreature(gameObject, this as IHasDlcRestrictions, "CrabWoodEgg", STRINGS.CREATURES.SPECIES.CRAB.VARIANT_WOOD.EGG_NAME, STRINGS.CREATURES.SPECIES.CRAB.VARIANT_WOOD.DESC, "egg_pincher_kanim", CrabTuning.EGG_MASS, "CrabWoodBaby", 60.000004f, 20f, CrabTuning.EGG_CHANCES_WOOD, CrabWoodConfig.EGG_SORT_ORDER, true, false, 1f, false);
		EggProtectionMonitor.Def def = gameObject.AddOrGetDef<EggProtectionMonitor.Def>();
		def.allyTags = new Tag[]
		{
			GameTags.Creatures.CrabFriend
		};
		def.animPrefix = CrabWoodConfig.animPrefix;
		MoltDropperMonitor.Def def2 = gameObject.AddOrGetDef<MoltDropperMonitor.Def>();
		def2.onGrowDropID = "CrabWoodShell";
		def2.massToDrop = 100f;
		def2.isReadyToMolt = new Func<MoltDropperMonitor.Instance, bool>(CrabTuning.IsReadyToMolt);
		return gameObject;
	}

	// Token: 0x06000549 RID: 1353 RVA: 0x0002A689 File Offset: 0x00028889
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600054A RID: 1354 RVA: 0x0002A68B File Offset: 0x0002888B
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040003D2 RID: 978
	public const string ID = "CrabWood";

	// Token: 0x040003D3 RID: 979
	public const string BASE_TRAIT_ID = "CrabWoodBaseTrait";

	// Token: 0x040003D4 RID: 980
	public const string EGG_ID = "CrabWoodEgg";

	// Token: 0x040003D5 RID: 981
	private const SimHashes EMIT_ELEMENT = SimHashes.Sand;

	// Token: 0x040003D6 RID: 982
	private static float KG_ORE_EATEN_PER_CYCLE = 70f;

	// Token: 0x040003D7 RID: 983
	private static float CALORIES_PER_KG_OF_ORE = CrabTuning.STANDARD_CALORIES_PER_CYCLE / CrabWoodConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x040003D8 RID: 984
	private static float MIN_POOP_SIZE_IN_KG = 25f;

	// Token: 0x040003D9 RID: 985
	public static int EGG_SORT_ORDER = 0;

	// Token: 0x040003DA RID: 986
	private static string animPrefix = "wood_";
}
