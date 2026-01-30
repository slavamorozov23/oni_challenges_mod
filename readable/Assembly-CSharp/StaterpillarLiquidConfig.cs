using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000172 RID: 370
[EntityConfigOrder(2)]
public class StaterpillarLiquidConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000726 RID: 1830 RVA: 0x00031E1C File Offset: 0x0003001C
	public static GameObject CreateStaterpillarLiquid(string id, string name, string desc, string anim_file, bool is_baby)
	{
		InhaleStates.Def inhaleDef = new InhaleStates.Def
		{
			behaviourTag = GameTags.Creatures.WantsToStore,
			inhaleAnimPre = "liquid_consume_pre",
			inhaleAnimLoop = "liquid_consume_loop",
			inhaleAnimPst = "liquid_consume_pst",
			useStorage = true,
			alwaysPlayPstAnim = true,
			inhaleTime = StaterpillarLiquidConfig.INHALE_TIME,
			storageStatusItem = Db.Get().CreatureStatusItems.LookingForLiquid
		};
		GameObject gameObject = BaseStaterpillarConfig.BaseStaterpillar(id, name, desc, anim_file, "StaterpillarLiquidBaseTrait", is_baby, ObjectLayer.LiquidConduit, StaterpillarLiquidConnectorConfig.ID, GameTags.Unbreathable, "wtr_", 263.15f, 313.15f, 173.15f, 373.15f, inhaleDef);
		gameObject = EntityTemplates.ExtendEntityToWildCreature(gameObject, TUNING.CREATURES.SPACE_REQUIREMENTS.TIER3);
		if (!is_baby)
		{
			GasAndLiquidConsumerMonitor.Def def = gameObject.AddOrGetDef<GasAndLiquidConsumerMonitor.Def>();
			def.behaviourTag = GameTags.Creatures.WantsToStore;
			def.consumableElementTag = GameTags.Liquid;
			def.transitionTag = new Tag[]
			{
				GameTags.Creature
			};
			def.minCooldown = StaterpillarLiquidConfig.COOLDOWN_MIN;
			def.maxCooldown = StaterpillarLiquidConfig.COOLDOWN_MAX;
			def.consumptionRate = StaterpillarLiquidConfig.CONSUMPTION_RATE;
		}
		Trait trait = Db.Get().CreateTrait("StaterpillarLiquidBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, StaterpillarTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -StaterpillarTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		List<Diet.Info> list = new List<Diet.Info>();
		list.AddRange(BaseStaterpillarConfig.RawMetalDiet(SimHashes.Hydrogen.CreateTag(), StaterpillarLiquidConfig.CALORIES_PER_KG_OF_ORE, StaterpillarTuning.POOP_CONVERSTION_RATE, null, 0f));
		list.AddRange(BaseStaterpillarConfig.RefinedMetalDiet(SimHashes.Hydrogen.CreateTag(), StaterpillarLiquidConfig.CALORIES_PER_KG_OF_ORE, StaterpillarTuning.POOP_CONVERSTION_RATE, null, 0f));
		gameObject = BaseStaterpillarConfig.SetupDiet(gameObject, list);
		Storage storage = gameObject.AddComponent<Storage>();
		storage.capacityKg = StaterpillarLiquidConfig.STORAGE_CAPACITY;
		storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
		return gameObject;
	}

	// Token: 0x06000727 RID: 1831 RVA: 0x00032073 File Offset: 0x00030273
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000728 RID: 1832 RVA: 0x0003207A File Offset: 0x0003027A
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000729 RID: 1833 RVA: 0x00032080 File Offset: 0x00030280
	public virtual GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(StaterpillarLiquidConfig.CreateStaterpillarLiquid("StaterpillarLiquid", STRINGS.CREATURES.SPECIES.STATERPILLAR.VARIANT_LIQUID.NAME, STRINGS.CREATURES.SPECIES.STATERPILLAR.VARIANT_LIQUID.DESC, "caterpillar_kanim", false), this, "StaterpillarLiquidEgg", STRINGS.CREATURES.SPECIES.STATERPILLAR.VARIANT_LIQUID.EGG_NAME, STRINGS.CREATURES.SPECIES.STATERPILLAR.VARIANT_LIQUID.DESC, "egg_caterpillar_kanim", StaterpillarTuning.EGG_MASS, "StaterpillarLiquidBaby", 60.000004f, 20f, StaterpillarTuning.EGG_CHANCES_LIQUID, 2, true, false, 1f, false);
	}

	// Token: 0x0600072A RID: 1834 RVA: 0x000320F7 File Offset: 0x000302F7
	public void OnPrefabInit(GameObject prefab)
	{
		KBatchedAnimController component = prefab.GetComponent<KBatchedAnimController>();
		component.SetSymbolVisiblity("electric_bolt_c_bloom", false);
		component.SetSymbolVisiblity("gulp", false);
	}

	// Token: 0x0600072B RID: 1835 RVA: 0x00032120 File Offset: 0x00030320
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000574 RID: 1396
	public const string ID = "StaterpillarLiquid";

	// Token: 0x04000575 RID: 1397
	public const string BASE_TRAIT_ID = "StaterpillarLiquidBaseTrait";

	// Token: 0x04000576 RID: 1398
	public const string EGG_ID = "StaterpillarLiquidEgg";

	// Token: 0x04000577 RID: 1399
	public const int EGG_SORT_ORDER = 2;

	// Token: 0x04000578 RID: 1400
	private static float KG_ORE_EATEN_PER_CYCLE = 30f;

	// Token: 0x04000579 RID: 1401
	private static float CALORIES_PER_KG_OF_ORE = StaterpillarTuning.STANDARD_CALORIES_PER_CYCLE / StaterpillarLiquidConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x0400057A RID: 1402
	private static float STORAGE_CAPACITY = 1000f;

	// Token: 0x0400057B RID: 1403
	private static float COOLDOWN_MIN = 20f;

	// Token: 0x0400057C RID: 1404
	private static float COOLDOWN_MAX = 40f;

	// Token: 0x0400057D RID: 1405
	private static float CONSUMPTION_RATE = 10f;

	// Token: 0x0400057E RID: 1406
	private static float INHALE_TIME = 6f;
}
