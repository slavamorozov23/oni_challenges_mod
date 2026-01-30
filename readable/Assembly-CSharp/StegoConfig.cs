using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000174 RID: 372
[EntityConfigOrder(1)]
public class StegoConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000734 RID: 1844 RVA: 0x000321F0 File Offset: 0x000303F0
	public static GameObject CreateStego(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = EntityTemplates.ExtendEntityToWildCreature(BaseStegoConfig.BaseStego(id, name, desc, anim_file, "StegoBaseTrait", is_baby, null), StegoTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("StegoBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, StegoTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -StegoTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 50f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 200f, name, false, false, true));
		GameObject gameObject = BaseStegoConfig.SetupDiet(prefab, BaseStegoConfig.StandardDiets(), StegoTuning.CALORIES_PER_UNIT_EATEN, StegoTuning.MIN_POOP_SIZE_IN_KG);
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x06000735 RID: 1845 RVA: 0x00032310 File Offset: 0x00030510
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC4;
	}

	// Token: 0x06000736 RID: 1846 RVA: 0x00032317 File Offset: 0x00030517
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000737 RID: 1847 RVA: 0x0003231C File Offset: 0x0003051C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.ExtendEntityToFertileCreature(StegoConfig.CreateStego("Stego", CREATURES.SPECIES.STEGO.NAME, CREATURES.SPECIES.STEGO.DESC, "stego_kanim", false), this, "StegoEgg", CREATURES.SPECIES.STEGO.EGG_NAME, CREATURES.SPECIES.STEGO.DESC, "egg_stego_kanim", 8f, "StegoBaby", 120.00001f, 40f, StegoTuning.EGG_CHANCES_BASE, StegoConfig.EGG_SORT_ORDER, true, false, 1f, false);
		gameObject.AddTag(GameTags.LargeCreature);
		return gameObject;
	}

	// Token: 0x06000738 RID: 1848 RVA: 0x000323A2 File Offset: 0x000305A2
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000739 RID: 1849 RVA: 0x000323A4 File Offset: 0x000305A4
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000580 RID: 1408
	public const string ID = "Stego";

	// Token: 0x04000581 RID: 1409
	public const string BASE_TRAIT_ID = "StegoBaseTrait";

	// Token: 0x04000582 RID: 1410
	public const string EGG_ID = "StegoEgg";

	// Token: 0x04000583 RID: 1411
	public static int EGG_SORT_ORDER;

	// Token: 0x04000584 RID: 1412
	public List<Emote> StegoEmotes = new List<Emote>
	{
		Db.Get().Emotes.Critter.Roar
	};
}
