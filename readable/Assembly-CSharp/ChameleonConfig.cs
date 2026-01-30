using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020000D1 RID: 209
public class ChameleonConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060003B1 RID: 945 RVA: 0x0001FF1C File Offset: 0x0001E11C
	public static GameObject CreateChameleon(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = BaseChameleonConfig.BaseChameleon(id, name, desc, anim_file, "ChameleonBaseTrait", is_baby, null, 233.15f, 293.15f, 173.15f, 373.15f);
		gameObject = EntityTemplates.ExtendEntityToWildCreature(gameObject, ChameleonTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("ChameleonBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, ChameleonTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -ChameleonTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, (float)ChameleonConfig.LIFESPAN, name, false, false, true));
		Diet diet = new Diet(new Diet.Info[]
		{
			new Diet.Info(new HashSet<Tag>
			{
				DewDripConfig.ID.ToTag()
			}, ChameleonConfig.POOP_ELEMENT, ChameleonConfig.CALORIES_PER_DRIP_EATEN, ChameleonConfig.KG_POOP_PER_DRIP, null, 0f, false, Diet.Info.FoodType.EatSolid, false, null)
		});
		CreatureCalorieMonitor.Def def = gameObject.AddOrGetDef<CreatureCalorieMonitor.Def>();
		def.diet = diet;
		def.minConsumedCaloriesBeforePooping = ChameleonConfig.MIN_POOP_SIZE_IN_CALORIES;
		gameObject.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;
		gameObject.AddOrGetDef<SetNavOrientationOnSpawnMonitor.Def>();
		gameObject.AddTag(GameTags.OriginalCreature);
		EntityTemplates.AddSecondaryExcretion(gameObject, SimHashes.ChlorineGas, 0.005f);
		return gameObject;
	}

	// Token: 0x060003B2 RID: 946 RVA: 0x000200C0 File Offset: 0x0001E2C0
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC4;
	}

	// Token: 0x060003B3 RID: 947 RVA: 0x000200C7 File Offset: 0x0001E2C7
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060003B4 RID: 948 RVA: 0x000200CC File Offset: 0x0001E2CC
	public virtual GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(ChameleonConfig.CreateChameleon("Chameleon", CREATURES.SPECIES.CHAMELEON.NAME, CREATURES.SPECIES.CHAMELEON.DESC, "chameleo_kanim", false), this, "ChameleonEgg", CREATURES.SPECIES.CHAMELEON.EGG_NAME, CREATURES.SPECIES.CHAMELEON.DESC, "egg_chameleo_kanim", ChameleonTuning.EGG_MASS, "ChameleonBaby", 0.6f * (float)ChameleonConfig.LIFESPAN, 0.2f * (float)ChameleonConfig.LIFESPAN, ChameleonTuning.EGG_CHANCES_BASE, ChameleonConfig.EGG_SORT_ORDER, true, false, 1f, false);
	}

	// Token: 0x060003B5 RID: 949 RVA: 0x00020155 File Offset: 0x0001E355
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060003B6 RID: 950 RVA: 0x00020157 File Offset: 0x0001E357
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040002DE RID: 734
	public const string ID = "Chameleon";

	// Token: 0x040002DF RID: 735
	public const string BASE_TRAIT_ID = "ChameleonBaseTrait";

	// Token: 0x040002E0 RID: 736
	public const string EGG_ID = "ChameleonEgg";

	// Token: 0x040002E1 RID: 737
	public static Tag POOP_ELEMENT = SimHashes.BleachStone.CreateTag();

	// Token: 0x040002E2 RID: 738
	private static float DRIPS_EATEN_PER_CYCLE = 1f;

	// Token: 0x040002E3 RID: 739
	private static float CALORIES_PER_DRIP_EATEN = ChameleonTuning.STANDARD_CALORIES_PER_CYCLE / ChameleonConfig.DRIPS_EATEN_PER_CYCLE;

	// Token: 0x040002E4 RID: 740
	private static float KG_POOP_PER_DRIP = 10f;

	// Token: 0x040002E5 RID: 741
	private static float MIN_POOP_SIZE_IN_KG = 10f;

	// Token: 0x040002E6 RID: 742
	private static float MIN_POOP_SIZE_IN_CALORIES = ChameleonConfig.CALORIES_PER_DRIP_EATEN * ChameleonConfig.MIN_POOP_SIZE_IN_KG / ChameleonConfig.KG_POOP_PER_DRIP;

	// Token: 0x040002E7 RID: 743
	private static int LIFESPAN = 50;

	// Token: 0x040002E8 RID: 744
	public static int EGG_SORT_ORDER = 800;
}
