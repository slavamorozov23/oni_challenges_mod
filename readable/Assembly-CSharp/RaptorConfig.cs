using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000163 RID: 355
[EntityConfigOrder(1)]
public class RaptorConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060006C8 RID: 1736 RVA: 0x000305BC File Offset: 0x0002E7BC
	public static GameObject CreateRaptor(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = BaseRaptorConfig.BaseRaptor(id, name, desc, anim_file, "RaptorBaseTrait", is_baby, null);
		gameObject = EntityTemplates.ExtendEntityToWildCreature(gameObject, TUNING.CREATURES.SPACE_REQUIREMENTS.TIER4);
		Trait trait = Db.Get().CreateTrait("RaptorBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, RaptorTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -RaptorTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 50f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 200f, name, false, false, true));
		gameObject = BaseRaptorConfig.SetupDiet(gameObject, BaseRaptorConfig.StandardDiets());
		WellFedShearable.Def def = gameObject.AddOrGetDef<WellFedShearable.Def>();
		def.effectId = "RaptorWellFed";
		def.scaleGrowthSymbols = new KAnimHashedString[]
		{
			"body_feathers",
			"tail_feather"
		};
		def.caloriesPerCycle = RaptorTuning.STANDARD_CALORIES_PER_CYCLE;
		def.growthDurationCycles = RaptorConfig.SCALE_GROWTH_TIME_IN_CYCLES;
		def.dropMass = RaptorConfig.FIBER_PER_CYCLE * RaptorConfig.SCALE_GROWTH_TIME_IN_CYCLES;
		def.itemDroppedOnShear = RaptorConfig.SCALE_GROWTH_EMIT_ELEMENT;
		def.levelCount = 2;
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x060006C9 RID: 1737 RVA: 0x00030751 File Offset: 0x0002E951
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC4;
	}

	// Token: 0x060006CA RID: 1738 RVA: 0x00030758 File Offset: 0x0002E958
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060006CB RID: 1739 RVA: 0x0003075C File Offset: 0x0002E95C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.ExtendEntityToFertileCreature(RaptorConfig.CreateRaptor("Raptor", STRINGS.CREATURES.SPECIES.RAPTOR.NAME, STRINGS.CREATURES.SPECIES.RAPTOR.DESC, "raptor_kanim", false), this, "RaptorEgg", STRINGS.CREATURES.SPECIES.RAPTOR.EGG_NAME, STRINGS.CREATURES.SPECIES.RAPTOR.DESC, "egg_raptor_kanim", 8f, "RaptorBaby", 120.00001f, 40f, RaptorTuning.EGG_CHANCES_BASE, RaptorConfig.EGG_SORT_ORDER, true, false, 1f, false);
		gameObject.AddTag(GameTags.LargeCreature);
		return gameObject;
	}

	// Token: 0x060006CC RID: 1740 RVA: 0x000307E2 File Offset: 0x0002E9E2
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060006CD RID: 1741 RVA: 0x000307E4 File Offset: 0x0002E9E4
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000528 RID: 1320
	public const string ID = "Raptor";

	// Token: 0x04000529 RID: 1321
	public const string BASE_TRAIT_ID = "RaptorBaseTrait";

	// Token: 0x0400052A RID: 1322
	public const string EGG_ID = "RaptorEgg";

	// Token: 0x0400052B RID: 1323
	public static int EGG_SORT_ORDER = 0;

	// Token: 0x0400052C RID: 1324
	public static float SCALE_GROWTH_TIME_IN_CYCLES = 4f;

	// Token: 0x0400052D RID: 1325
	public static float SCALE_INITIAL_GROWTH_PCT = 0.9f;

	// Token: 0x0400052E RID: 1326
	public static float FIBER_PER_CYCLE = 1f;

	// Token: 0x0400052F RID: 1327
	public static Tag SCALE_GROWTH_EMIT_ELEMENT = FeatherFabricConfig.ID;

	// Token: 0x04000530 RID: 1328
	public static KAnimHashedString[] SCALE_SYMBOLS = new KAnimHashedString[]
	{
		"scale_0",
		"scale_1",
		"scale_2"
	};

	// Token: 0x04000531 RID: 1329
	public List<Emote> RaptorEmotes = new List<Emote>
	{
		Db.Get().Emotes.Critter.Roar,
		Db.Get().Emotes.Critter.RaptorSignal
	};
}
