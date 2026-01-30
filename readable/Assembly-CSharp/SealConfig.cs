using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000167 RID: 359
public class SealConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060006E4 RID: 1764 RVA: 0x00030F1C File Offset: 0x0002F11C
	public static GameObject CreateSeal(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = BaseSealConfig.BaseSeal(id, name, desc, anim_file, "SealBaseTrait", is_baby, null);
		gameObject = EntityTemplates.ExtendEntityToWildCreature(gameObject, SealTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("SealBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, SquirrelTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -SquirrelTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		gameObject = BaseSealConfig.SetupDiet(gameObject, new List<Diet.Info>
		{
			new Diet.Info(new HashSet<Tag>
			{
				"SpaceTree"
			}, SimHashes.Ethanol.CreateTag(), 2500f, TUNING.CREATURES.CONVERSION_EFFICIENCY.GOOD_3, null, 0f, false, Diet.Info.FoodType.EatPlantStorage, false, null),
			new Diet.Info(new HashSet<Tag>
			{
				SimHashes.Sucrose.CreateTag()
			}, SimHashes.Ethanol.CreateTag(), 3246.7532f, 1.2987013f, null, 0f, false, Diet.Info.FoodType.EatSolid, false, new string[]
			{
				"eat_ore_pre",
				"eat_ore_loop",
				"eat_ore_pst"
			})
		}, 2500f, SealConfig.MIN_POOP_SIZE_IN_KG);
		gameObject.AddOrGetDef<CreaturePoopLoot.Def>().Loot = new CreaturePoopLoot.LootData[]
		{
			new CreaturePoopLoot.LootData
			{
				tag = "SpaceTreeSeed",
				probability = 0.2f
			}
		};
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x060006E5 RID: 1765 RVA: 0x0003111D File Offset: 0x0002F31D
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x060006E6 RID: 1766 RVA: 0x00031124 File Offset: 0x0002F324
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060006E7 RID: 1767 RVA: 0x00031128 File Offset: 0x0002F328
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(SealConfig.CreateSeal("Seal", STRINGS.CREATURES.SPECIES.SEAL.NAME, STRINGS.CREATURES.SPECIES.SEAL.DESC, "seal_kanim", false), this, "SealEgg", STRINGS.CREATURES.SPECIES.SEAL.EGG_NAME, STRINGS.CREATURES.SPECIES.SEAL.DESC, "egg_seal_kanim", SealTuning.EGG_MASS, "SealBaby", 60.000004f, 20f, SealTuning.EGG_CHANCES_BASE, SealConfig.EGG_SORT_ORDER, true, false, 1f, false);
	}

	// Token: 0x060006E8 RID: 1768 RVA: 0x000311A3 File Offset: 0x0002F3A3
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060006E9 RID: 1769 RVA: 0x000311A5 File Offset: 0x0002F3A5
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400053F RID: 1343
	public const string ID = "Seal";

	// Token: 0x04000540 RID: 1344
	public const string BASE_TRAIT_ID = "SealBaseTrait";

	// Token: 0x04000541 RID: 1345
	public const string EGG_ID = "SealEgg";

	// Token: 0x04000542 RID: 1346
	public const float SUGAR_TREE_SEED_PROBABILITY_ON_POOP = 0.2f;

	// Token: 0x04000543 RID: 1347
	public const float SUGAR_WATER_KG_CONSUMED_PER_DAY = 40f;

	// Token: 0x04000544 RID: 1348
	public const float CALORIES_PER_1KG_OF_SUGAR_WATER = 2500f;

	// Token: 0x04000545 RID: 1349
	private static float MIN_POOP_SIZE_IN_KG = 10f;

	// Token: 0x04000546 RID: 1350
	public static int EGG_SORT_ORDER = 0;
}
