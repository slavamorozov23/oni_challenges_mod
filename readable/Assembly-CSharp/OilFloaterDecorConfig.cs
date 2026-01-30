using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200014E RID: 334
[EntityConfigOrder(2)]
public class OilFloaterDecorConfig : IEntityConfig
{
	// Token: 0x0600065F RID: 1631 RVA: 0x0002EECC File Offset: 0x0002D0CC
	public static GameObject CreateOilFloater(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = BaseOilFloaterConfig.BaseOilFloater(id, name, desc, anim_file, "OilfloaterDecorBaseTrait", 273.15f, 323.15f, 223.15f, 373.15f, is_baby, "oxy_");
		gameObject.AddOrGet<DecorProvider>().SetValues(DECOR.BONUS.TIER6);
		EntityTemplates.ExtendEntityToWildCreature(gameObject, OilFloaterTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("OilfloaterDecorBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, OilFloaterTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -OilFloaterTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 150f, name, false, false, true));
		return BaseOilFloaterConfig.SetupDiet(gameObject, SimHashes.Oxygen.CreateTag(), Tag.Invalid, OilFloaterDecorConfig.CALORIES_PER_KG_OF_ORE, 0f, null, 0f, 0f);
	}

	// Token: 0x06000660 RID: 1632 RVA: 0x0002F020 File Offset: 0x0002D220
	public GameObject CreatePrefab()
	{
		GameObject gameObject = OilFloaterDecorConfig.CreateOilFloater("OilfloaterDecor", STRINGS.CREATURES.SPECIES.OILFLOATER.VARIANT_DECOR.NAME, STRINGS.CREATURES.SPECIES.OILFLOATER.VARIANT_DECOR.DESC, "oilfloater_kanim", false);
		EntityTemplates.ExtendEntityToFertileCreature(gameObject, this as IHasDlcRestrictions, "OilfloaterDecorEgg", STRINGS.CREATURES.SPECIES.OILFLOATER.VARIANT_DECOR.EGG_NAME, STRINGS.CREATURES.SPECIES.OILFLOATER.VARIANT_DECOR.DESC, "egg_oilfloater_kanim", OilFloaterTuning.EGG_MASS, "OilfloaterDecorBaby", 90f, 30f, OilFloaterTuning.EGG_CHANCES_DECOR, OilFloaterDecorConfig.EGG_SORT_ORDER, true, false, 1f, false);
		return gameObject;
	}

	// Token: 0x06000661 RID: 1633 RVA: 0x0002F0A2 File Offset: 0x0002D2A2
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000662 RID: 1634 RVA: 0x0002F0A4 File Offset: 0x0002D2A4
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004D0 RID: 1232
	public const string ID = "OilfloaterDecor";

	// Token: 0x040004D1 RID: 1233
	public const string BASE_TRAIT_ID = "OilfloaterDecorBaseTrait";

	// Token: 0x040004D2 RID: 1234
	public const string EGG_ID = "OilfloaterDecorEgg";

	// Token: 0x040004D3 RID: 1235
	public const SimHashes CONSUME_ELEMENT = SimHashes.Oxygen;

	// Token: 0x040004D4 RID: 1236
	private static float KG_ORE_EATEN_PER_CYCLE = 30f;

	// Token: 0x040004D5 RID: 1237
	private static float CALORIES_PER_KG_OF_ORE = OilFloaterTuning.STANDARD_CALORIES_PER_CYCLE / OilFloaterDecorConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x040004D6 RID: 1238
	public static int EGG_SORT_ORDER = OilFloaterConfig.EGG_SORT_ORDER + 2;
}
