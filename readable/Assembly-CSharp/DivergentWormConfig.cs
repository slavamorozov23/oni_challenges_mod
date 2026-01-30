using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000121 RID: 289
[EntityConfigOrder(1)]
public class DivergentWormConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000567 RID: 1383 RVA: 0x0002AB7C File Offset: 0x00028D7C
	public static GameObject CreateWorm(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = EntityTemplates.ExtendEntityToWildCreature(BaseDivergentConfig.BaseDivergent(id, name, desc, 200f, anim_file, is_baby ? null : "worm_build_kanim", "DivergentWormBaseTrait", is_baby, 8f, null, "DivergentCropTendedWorm", 3, false, "worm_emotes_kanim"), DivergentTuning.PEN_SIZE_PER_CREATURE_WORM);
		Trait trait = Db.Get().CreateTrait("DivergentWormBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, DivergentTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -DivergentTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 150f, name, false, false, true));
		prefab.AddWeapon(2f, 3f, AttackProperties.DamageType.Standard, AttackProperties.TargetType.Single, 1, 0f);
		List<Diet.Info> list = BaseDivergentConfig.BasicSulfurDiet(SimHashes.Mud.CreateTag(), DivergentWormConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.BAD_2, null, 0f);
		list.Add(new Diet.Info(new HashSet<Tag>
		{
			SimHashes.Sucrose.CreateTag()
		}, SimHashes.Mud.CreateTag(), DivergentWormConfig.CALORIES_PER_KG_OF_SUCROSE, 1f, null, 0f, false, Diet.Info.FoodType.EatSolid, false, null));
		GameObject gameObject = BaseDivergentConfig.SetupDiet(prefab, list, DivergentWormConfig.CALORIES_PER_KG_OF_ORE, DivergentWormConfig.MINI_POOP_SIZE_IN_KG);
		SegmentedCreature.Def def = gameObject.AddOrGetDef<SegmentedCreature.Def>();
		def.segmentTrackerSymbol = new HashedString("segmenttracker");
		def.numBodySegments = 5;
		def.midAnim = Assets.GetAnim("worm_torso_kanim");
		def.tailAnim = Assets.GetAnim("worm_tail_kanim");
		def.animFrameOffset = 2;
		def.pathSpacing = 0.2f;
		def.numPathNodes = 15;
		def.minSegmentSpacing = 0.1f;
		def.maxSegmentSpacing = 0.4f;
		def.retractionSegmentSpeed = 1f;
		def.retractionPathSpeed = 2f;
		def.compressedMaxScale = 0.25f;
		def.headOffset = new Vector3(0.12f, 0.4f, 0f);
		return gameObject;
	}

	// Token: 0x06000568 RID: 1384 RVA: 0x0002ADD8 File Offset: 0x00028FD8
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000569 RID: 1385 RVA: 0x0002ADDF File Offset: 0x00028FDF
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x0600056A RID: 1386 RVA: 0x0002ADE4 File Offset: 0x00028FE4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.ExtendEntityToFertileCreature(DivergentWormConfig.CreateWorm("DivergentWorm", STRINGS.CREATURES.SPECIES.DIVERGENT.VARIANT_WORM.NAME, STRINGS.CREATURES.SPECIES.DIVERGENT.VARIANT_WORM.DESC, "worm_head_kanim", false), this, "DivergentWormEgg", STRINGS.CREATURES.SPECIES.DIVERGENT.VARIANT_WORM.EGG_NAME, STRINGS.CREATURES.SPECIES.DIVERGENT.VARIANT_WORM.DESC, "egg_worm_kanim", DivergentTuning.EGG_MASS, "DivergentWormBaby", 90f, 30f, DivergentTuning.EGG_CHANCES_WORM, DivergentWormConfig.EGG_SORT_ORDER, true, false, 1f, false);
		gameObject.AddTag(GameTags.Creatures.Pollinator);
		return gameObject;
	}

	// Token: 0x0600056B RID: 1387 RVA: 0x0002AE6A File Offset: 0x0002906A
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600056C RID: 1388 RVA: 0x0002AE6C File Offset: 0x0002906C
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040003EA RID: 1002
	public const string ID = "DivergentWorm";

	// Token: 0x040003EB RID: 1003
	public const string BASE_TRAIT_ID = "DivergentWormBaseTrait";

	// Token: 0x040003EC RID: 1004
	public const string EGG_ID = "DivergentWormEgg";

	// Token: 0x040003ED RID: 1005
	private const float LIFESPAN = 150f;

	// Token: 0x040003EE RID: 1006
	public const float CROP_TENDED_MULTIPLIER_EFFECT = 0.5f;

	// Token: 0x040003EF RID: 1007
	public const float CROP_TENDED_MULTIPLIER_DURATION = 600f;

	// Token: 0x040003F0 RID: 1008
	private const int NUM_SEGMENTS = 5;

	// Token: 0x040003F1 RID: 1009
	private const SimHashes EMIT_ELEMENT = SimHashes.Mud;

	// Token: 0x040003F2 RID: 1010
	private static float KG_ORE_EATEN_PER_CYCLE = 50f;

	// Token: 0x040003F3 RID: 1011
	private static float KG_SUCROSE_EATEN_PER_CYCLE = 30f;

	// Token: 0x040003F4 RID: 1012
	private static float CALORIES_PER_KG_OF_ORE = DivergentTuning.STANDARD_CALORIES_PER_CYCLE / DivergentWormConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x040003F5 RID: 1013
	private static float CALORIES_PER_KG_OF_SUCROSE = DivergentTuning.STANDARD_CALORIES_PER_CYCLE / DivergentWormConfig.KG_SUCROSE_EATEN_PER_CYCLE;

	// Token: 0x040003F6 RID: 1014
	public static int EGG_SORT_ORDER = 0;

	// Token: 0x040003F7 RID: 1015
	private static float MINI_POOP_SIZE_IN_KG = 4f;

	// Token: 0x040003F8 RID: 1016
	public const string CROP_TENDING_EFFECT = "DivergentCropTendedWorm";
}
