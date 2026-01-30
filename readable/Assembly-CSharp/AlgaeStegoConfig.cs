using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200009B RID: 155
[EntityConfigOrder(2)]
public class AlgaeStegoConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000315 RID: 789 RVA: 0x00016570 File Offset: 0x00014770
	public static GameObject CreateStego(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = EntityTemplates.ExtendEntityToWildCreature(BaseStegoConfig.BaseStego(id, name, desc, anim_file, "AlgaeStegoBaseTrait", is_baby, "alg_"), StegoTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("AlgaeStegoBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, StegoTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -StegoTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 50f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 200f, name, false, false, true));
		return BaseStegoConfig.SetupDiet(prefab, AlgaeStegoConfig.Diets(), StegoTuning.CALORIES_PER_UNIT_EATEN, 4f);
	}

	// Token: 0x06000316 RID: 790 RVA: 0x00016689 File Offset: 0x00014889
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC4;
	}

	// Token: 0x06000317 RID: 791 RVA: 0x00016690 File Offset: 0x00014890
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000318 RID: 792 RVA: 0x00016694 File Offset: 0x00014894
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.ExtendEntityToFertileCreature(AlgaeStegoConfig.CreateStego("AlgaeStego", STRINGS.CREATURES.SPECIES.ALGAE_STEGO.NAME, STRINGS.CREATURES.SPECIES.ALGAE_STEGO.DESC, "stego_kanim", false), this, "AlgaeStegoEgg", STRINGS.CREATURES.SPECIES.ALGAE_STEGO.EGG_NAME, STRINGS.CREATURES.SPECIES.ALGAE_STEGO.DESC, "egg_stego_kanim", 8f, "AlgaeStegoBaby", 120.00001f, 40f, StegoTuning.EGG_CHANCES_ALGAE, 0, true, false, 1f, false);
		KBatchedAnimController component = gameObject.GetComponent<KBatchedAnimController>();
		component.SetSymbolVisiblity("stego_eye_yellow", false);
		component.SetSymbolVisiblity("stego_scale", false);
		component.SetSymbolVisiblity("stego_pupil", false);
		gameObject.AddTag(GameTags.LargeCreature);
		return gameObject;
	}

	// Token: 0x06000319 RID: 793 RVA: 0x0001674E File Offset: 0x0001494E
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600031A RID: 794 RVA: 0x00016750 File Offset: 0x00014950
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0600031B RID: 795 RVA: 0x00016754 File Offset: 0x00014954
	public static List<Diet.Info> Diets()
	{
		List<Diet.Info> list = new List<Diet.Info>();
		list.Add(new Diet.Info(new HashSet<Tag>
		{
			VineFruitConfig.ID
		}, SimHashes.Algae.CreateTag(), StegoTuning.CALORIES_PER_KG_OF_ORE, 33f, null, 0f, false, Diet.Info.FoodType.EatSolid, false, null));
		float num = FOOD.FOOD_TYPES.PRICKLEFRUIT.CaloriesPerUnit / FOOD.FOOD_TYPES.VINEFRUIT.CaloriesPerUnit;
		list.Add(new Diet.Info(new HashSet<Tag>
		{
			PrickleFruitConfig.ID
		}, SimHashes.Algae.CreateTag(), StegoTuning.CALORIES_PER_KG_OF_ORE * num, 132f / (4f / num), null, 0f, false, Diet.Info.FoodType.EatSolid, false, null));
		if (DlcManager.IsExpansion1Active())
		{
			float num2 = FOOD.FOOD_TYPES.SWAMPFRUIT.CaloriesPerUnit / FOOD.FOOD_TYPES.VINEFRUIT.CaloriesPerUnit;
			float num3 = 1.5f;
			list.Add(new Diet.Info(new HashSet<Tag>
			{
				SwampFruitConfig.ID
			}, SimHashes.Algae.CreateTag(), StegoTuning.CALORIES_PER_KG_OF_ORE * num2, 132f / (4f / num2) * num3, null, 0f, false, Diet.Info.FoodType.EatSolid, false, null));
		}
		return list;
	}

	// Token: 0x040001C6 RID: 454
	public const string ID = "AlgaeStego";

	// Token: 0x040001C7 RID: 455
	public const string BASE_TRAIT_ID = "AlgaeStegoBaseTrait";

	// Token: 0x040001C8 RID: 456
	public const string EGG_ID = "AlgaeStegoEgg";

	// Token: 0x040001C9 RID: 457
	public const int EGG_SORT_ORDER = 0;

	// Token: 0x040001CA RID: 458
	public const float VINE_FOOD_PER_CYCLE = 4f;

	// Token: 0x040001CB RID: 459
	public const float PRODUCT_PRODUCED_PER_CYCLE = 132f;

	// Token: 0x040001CC RID: 460
	public const SimHashes POOP_ELEMENT = SimHashes.Algae;

	// Token: 0x040001CD RID: 461
	public const float MIN_POOP_SIZE_IN_KG = 4f;

	// Token: 0x040001CE RID: 462
	public List<Emote> StegoEmotes = new List<Emote>
	{
		Db.Get().Emotes.Critter.Roar
	};
}
