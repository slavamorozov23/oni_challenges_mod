using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000150 RID: 336
[EntityConfigOrder(2)]
public class OilFloaterHighTempConfig : IEntityConfig
{
	// Token: 0x06000669 RID: 1641 RVA: 0x0002F120 File Offset: 0x0002D320
	public static GameObject CreateOilFloater(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = BaseOilFloaterConfig.BaseOilFloater(id, name, desc, anim_file, "OilfloaterHighTempBaseTrait", 373.15f, 473.15f, 323.15f, 573.15f, is_baby, "hot_");
		EntityTemplates.ExtendEntityToWildCreature(prefab, OilFloaterTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("OilfloaterHighTempBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, OilFloaterTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -OilFloaterTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		return BaseOilFloaterConfig.SetupDiet(prefab, SimHashes.CarbonDioxide.CreateTag(), SimHashes.Petroleum.CreateTag(), OilFloaterHighTempConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.NORMAL, null, 0f, OilFloaterHighTempConfig.MIN_POOP_SIZE_IN_KG);
	}

	// Token: 0x0600066A RID: 1642 RVA: 0x0002F26C File Offset: 0x0002D46C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = OilFloaterHighTempConfig.CreateOilFloater("OilfloaterHighTemp", STRINGS.CREATURES.SPECIES.OILFLOATER.VARIANT_HIGHTEMP.NAME, STRINGS.CREATURES.SPECIES.OILFLOATER.VARIANT_HIGHTEMP.DESC, "oilfloater_kanim", false);
		EntityTemplates.ExtendEntityToFertileCreature(gameObject, this as IHasDlcRestrictions, "OilfloaterHighTempEgg", STRINGS.CREATURES.SPECIES.OILFLOATER.VARIANT_HIGHTEMP.EGG_NAME, STRINGS.CREATURES.SPECIES.OILFLOATER.VARIANT_HIGHTEMP.DESC, "egg_oilfloater_kanim", OilFloaterTuning.EGG_MASS, "OilfloaterHighTempBaby", 60.000004f, 20f, OilFloaterTuning.EGG_CHANCES_HIGHTEMP, OilFloaterHighTempConfig.EGG_SORT_ORDER, true, false, 1f, false);
		return gameObject;
	}

	// Token: 0x0600066B RID: 1643 RVA: 0x0002F2EE File Offset: 0x0002D4EE
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600066C RID: 1644 RVA: 0x0002F2F0 File Offset: 0x0002D4F0
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004D8 RID: 1240
	public const string ID = "OilfloaterHighTemp";

	// Token: 0x040004D9 RID: 1241
	public const string BASE_TRAIT_ID = "OilfloaterHighTempBaseTrait";

	// Token: 0x040004DA RID: 1242
	public const string EGG_ID = "OilfloaterHighTempEgg";

	// Token: 0x040004DB RID: 1243
	public const SimHashes CONSUME_ELEMENT = SimHashes.CarbonDioxide;

	// Token: 0x040004DC RID: 1244
	public const SimHashes EMIT_ELEMENT = SimHashes.Petroleum;

	// Token: 0x040004DD RID: 1245
	private static float KG_ORE_EATEN_PER_CYCLE = 20f;

	// Token: 0x040004DE RID: 1246
	private static float CALORIES_PER_KG_OF_ORE = OilFloaterTuning.STANDARD_CALORIES_PER_CYCLE / OilFloaterHighTempConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x040004DF RID: 1247
	private static float MIN_POOP_SIZE_IN_KG = 0.5f;

	// Token: 0x040004E0 RID: 1248
	public static int EGG_SORT_ORDER = OilFloaterConfig.EGG_SORT_ORDER + 1;
}
