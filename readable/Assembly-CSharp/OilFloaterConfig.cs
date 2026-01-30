using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200014C RID: 332
public class OilFloaterConfig : IEntityConfig
{
	// Token: 0x06000655 RID: 1621 RVA: 0x0002EC74 File Offset: 0x0002CE74
	public static GameObject CreateOilFloater(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = BaseOilFloaterConfig.BaseOilFloater(id, name, desc, anim_file, "OilfloaterBaseTrait", 323.15f, 413.15f, 273.15f, 473.15f, is_baby, null);
		EntityTemplates.ExtendEntityToWildCreature(prefab, OilFloaterTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("OilfloaterBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, OilFloaterTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -OilFloaterTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		GameObject gameObject = BaseOilFloaterConfig.SetupDiet(prefab, SimHashes.CarbonDioxide.CreateTag(), SimHashes.CrudeOil.CreateTag(), OilFloaterConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.NORMAL, null, 0f, OilFloaterConfig.MIN_POOP_SIZE_IN_KG);
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x06000656 RID: 1622 RVA: 0x0002EDC4 File Offset: 0x0002CFC4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = OilFloaterConfig.CreateOilFloater("Oilfloater", STRINGS.CREATURES.SPECIES.OILFLOATER.NAME, STRINGS.CREATURES.SPECIES.OILFLOATER.DESC, "oilfloater_kanim", false);
		EntityTemplates.ExtendEntityToFertileCreature(gameObject, this as IHasDlcRestrictions, "OilfloaterEgg", STRINGS.CREATURES.SPECIES.OILFLOATER.EGG_NAME, STRINGS.CREATURES.SPECIES.OILFLOATER.DESC, "egg_oilfloater_kanim", OilFloaterTuning.EGG_MASS, "OilfloaterBaby", 60.000004f, 20f, OilFloaterTuning.EGG_CHANCES_BASE, OilFloaterConfig.EGG_SORT_ORDER, true, false, 1f, false);
		return gameObject;
	}

	// Token: 0x06000657 RID: 1623 RVA: 0x0002EE46 File Offset: 0x0002D046
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000658 RID: 1624 RVA: 0x0002EE48 File Offset: 0x0002D048
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004C6 RID: 1222
	public const string ID = "Oilfloater";

	// Token: 0x040004C7 RID: 1223
	public const string BASE_TRAIT_ID = "OilfloaterBaseTrait";

	// Token: 0x040004C8 RID: 1224
	public const string EGG_ID = "OilfloaterEgg";

	// Token: 0x040004C9 RID: 1225
	public const SimHashes CONSUME_ELEMENT = SimHashes.CarbonDioxide;

	// Token: 0x040004CA RID: 1226
	public const SimHashes EMIT_ELEMENT = SimHashes.CrudeOil;

	// Token: 0x040004CB RID: 1227
	private static float KG_ORE_EATEN_PER_CYCLE = 20f;

	// Token: 0x040004CC RID: 1228
	private static float CALORIES_PER_KG_OF_ORE = OilFloaterTuning.STANDARD_CALORIES_PER_CYCLE / OilFloaterConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x040004CD RID: 1229
	private static float MIN_POOP_SIZE_IN_KG = 0.5f;

	// Token: 0x040004CE RID: 1230
	public static int EGG_SORT_ORDER = 400;
}
