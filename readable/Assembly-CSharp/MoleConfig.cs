using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000144 RID: 324
public class MoleConfig : IEntityConfig
{
	// Token: 0x06000626 RID: 1574 RVA: 0x0002E034 File Offset: 0x0002C234
	public static GameObject CreateMole(string id, string name, string desc, string anim_file, bool is_baby = false)
	{
		GameObject gameObject = BaseMoleConfig.BaseMole(id, name, STRINGS.CREATURES.SPECIES.MOLE.DESC, "MoleBaseTrait", anim_file, is_baby, 173.15f, 673.15f, 73.149994f, 773.15f, null, 10);
		gameObject.AddTag(GameTags.Creatures.Digger);
		EntityTemplates.ExtendEntityToWildCreature(gameObject, MoleTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("MoleBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, MoleTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -MoleTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		Diet diet = new Diet(BaseMoleConfig.SimpleOreDiet(new List<Tag>
		{
			SimHashes.Regolith.CreateTag(),
			SimHashes.Dirt.CreateTag(),
			SimHashes.IronOre.CreateTag()
		}, MoleConfig.CALORIES_PER_KG_OF_DIRT, TUNING.CREATURES.CONVERSION_EFFICIENCY.NORMAL).ToArray());
		CreatureCalorieMonitor.Def def = gameObject.AddOrGetDef<CreatureCalorieMonitor.Def>();
		def.diet = diet;
		def.minConsumedCaloriesBeforePooping = MoleConfig.MIN_POOP_SIZE_IN_CALORIES;
		gameObject.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;
		gameObject.AddOrGetDef<OvercrowdingMonitor.Def>().spaceRequiredPerCreature = 0;
		gameObject.AddOrGet<LoopingSounds>();
		foreach (HashedString hash in MoleTuning.GINGER_SYMBOL_NAMES)
		{
			gameObject.GetComponent<KAnimControllerBase>().SetSymbolVisiblity(hash, false);
		}
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x06000627 RID: 1575 RVA: 0x0002E224 File Offset: 0x0002C424
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(MoleConfig.CreateMole("Mole", STRINGS.CREATURES.SPECIES.MOLE.NAME, STRINGS.CREATURES.SPECIES.MOLE.DESC, "driller_kanim", false), this as IHasDlcRestrictions, "MoleEgg", STRINGS.CREATURES.SPECIES.MOLE.EGG_NAME, STRINGS.CREATURES.SPECIES.MOLE.DESC, "egg_driller_kanim", MoleTuning.EGG_MASS, "MoleBaby", 60.000004f, 20f, MoleTuning.EGG_CHANCES_BASE, MoleConfig.EGG_SORT_ORDER, true, false, 1f, false);
	}

	// Token: 0x06000628 RID: 1576 RVA: 0x0002E2A4 File Offset: 0x0002C4A4
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000629 RID: 1577 RVA: 0x0002E2A6 File Offset: 0x0002C4A6
	public void OnSpawn(GameObject inst)
	{
		MoleConfig.SetSpawnNavType(inst);
	}

	// Token: 0x0600062A RID: 1578 RVA: 0x0002E2B0 File Offset: 0x0002C4B0
	public static void SetSpawnNavType(GameObject inst)
	{
		int cell = Grid.PosToCell(inst);
		Navigator component = inst.GetComponent<Navigator>();
		Pickupable component2 = inst.GetComponent<Pickupable>();
		if (component != null && (component2 == null || component2.storage == null))
		{
			if (Grid.IsSolidCell(cell))
			{
				component.SetCurrentNavType(NavType.Solid);
				inst.transform.SetPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.FXFront));
				inst.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.FXFront);
				return;
			}
			inst.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.Creatures);
		}
	}

	// Token: 0x040004A0 RID: 1184
	public const string ID = "Mole";

	// Token: 0x040004A1 RID: 1185
	public const string BASE_TRAIT_ID = "MoleBaseTrait";

	// Token: 0x040004A2 RID: 1186
	public const string EGG_ID = "MoleEgg";

	// Token: 0x040004A3 RID: 1187
	private static float MIN_POOP_SIZE_IN_CALORIES = 2400000f;

	// Token: 0x040004A4 RID: 1188
	private static float CALORIES_PER_KG_OF_DIRT = 1000f;

	// Token: 0x040004A5 RID: 1189
	public static int EGG_SORT_ORDER = 800;
}
