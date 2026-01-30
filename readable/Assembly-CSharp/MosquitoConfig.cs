using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x0200014A RID: 330
[EntityConfigOrder(1)]
public class MosquitoConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000647 RID: 1607 RVA: 0x0002EA24 File Offset: 0x0002CC24
	public static GameObject CreateMosquito(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = BaseMosquitoConfig.BaseMosquito(id, name, desc, anim_file, "MosquitoBaseTrait", null, is_baby, 278.15f, 338.15f, 273.15f, 348.15f, "attack_pre", "attack_loop", "attack_pst", "STRINGS.CREATURES.STATUSITEMS.MOSQUITO_GOING_FOR_FOOD", "STRINGS.CREATURES.STATUSITEMS.EATING");
		gameObject.AddOrGetDef<AgeMonitor.Def>();
		Trait trait = Db.Get().CreateTrait("MosquitoBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 5f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 10f, name, false, false, true));
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x06000648 RID: 1608 RVA: 0x0002EAF0 File Offset: 0x0002CCF0
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC4;
	}

	// Token: 0x06000649 RID: 1609 RVA: 0x0002EAF7 File Offset: 0x0002CCF7
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x0600064A RID: 1610 RVA: 0x0002EAFC File Offset: 0x0002CCFC
	public GameObject CreatePrefab()
	{
		CREATURES.SPECIES.MOSQUITO.NAME;
		GameObject gameObject = EntityTemplates.ExtendEntityToFertileCreature(MosquitoConfig.CreateMosquito("Mosquito", CREATURES.SPECIES.MOSQUITO.NAME, CREATURES.SPECIES.MOSQUITO.DESC, "mosquito_kanim", false), this, "MosquitoEgg", CREATURES.SPECIES.MOSQUITO.EGG_NAME, CREATURES.SPECIES.MOSQUITO.DESC, "egg_mosquito_kanim", 1f, "MosquitoBaby", 4.5f, 2f, MosquitoTuning.EGG_CHANCES_BASE, MosquitoConfig.EGG_SORT_ORDER, false, false, 0.75f, false, true, 1f, false);
		gameObject.AddTag(GameTags.OriginalCreature);
		MosquitoHungerMonitor mosquitoHungerMonitor = gameObject.AddOrGet<MosquitoHungerMonitor>();
		mosquitoHungerMonitor.AllowedTargetTags = new List<Tag>
		{
			GameTags.BaseMinion,
			GameTags.Creature
		};
		mosquitoHungerMonitor.ForbiddenTargetTags = new List<Tag>
		{
			"Mosquito",
			GameTags.SwimmingCreature,
			GameTags.Dead,
			GameTags.HasAirtightSuit
		};
		gameObject.AddOrGetDef<AgeMonitor.Def>().minAgePercentOnSpawn = 0.5f;
		return gameObject;
	}

	// Token: 0x0600064B RID: 1611 RVA: 0x0002EC06 File Offset: 0x0002CE06
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600064C RID: 1612 RVA: 0x0002EC08 File Offset: 0x0002CE08
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004BE RID: 1214
	public const string BASE_TRAIT_ID = "MosquitoBaseTrait";

	// Token: 0x040004BF RID: 1215
	public const string ID = "Mosquito";

	// Token: 0x040004C0 RID: 1216
	public const string EGG_ID = "MosquitoEgg";

	// Token: 0x040004C1 RID: 1217
	public static int EGG_SORT_ORDER = 300;

	// Token: 0x040004C2 RID: 1218
	public const int ADULT_LIFESPAN = 5;

	// Token: 0x040004C3 RID: 1219
	public const int BABY_LIFESPAN = 5;

	// Token: 0x040004C4 RID: 1220
	public const int LIFE_SPAN = 10;
}
