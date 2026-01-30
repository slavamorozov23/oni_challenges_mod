using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000159 RID: 345
[EntityConfigOrder(1)]
public class PrehistoricPacuConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000693 RID: 1683 RVA: 0x0002F8A0 File Offset: 0x0002DAA0
	public static GameObject CreatePrehistoricPacu(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = EntityTemplates.ExtendEntityToWildCreature(BasePrehistoricPacuConfig.CreatePrefab(id, "PrehistoricPacuBaseTrait", name, desc, anim_file, is_baby, null, 273.15f, 333.15f, 253.15f, 373.15f), PrehistoricPacuTuning.PEN_SIZE_PER_CREATURE, false);
		EntityTemplates.CreateAndRegisterBaggedCreature(gameObject, true, true, false);
		Trait trait = Db.Get().CreateTrait("PrehistoricPacuBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, PrehistoricPacuTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -PrehistoricPacuTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 50f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x06000694 RID: 1684 RVA: 0x0002F9CB File Offset: 0x0002DBCB
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC4;
	}

	// Token: 0x06000695 RID: 1685 RVA: 0x0002F9D2 File Offset: 0x0002DBD2
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000696 RID: 1686 RVA: 0x0002F9D8 File Offset: 0x0002DBD8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.ExtendEntityToFertileCreature(PrehistoricPacuConfig.CreatePrehistoricPacu("PrehistoricPacu", CREATURES.SPECIES.PREHISTORICPACU.NAME, CREATURES.SPECIES.PREHISTORICPACU.DESC, "paculacanth_kanim", false), this, "PrehistoricPacuEgg", CREATURES.SPECIES.PREHISTORICPACU.EGG_NAME, CREATURES.SPECIES.PREHISTORICPACU.DESC, "egg_paculacanth_kanim", PrehistoricPacuTuning.EGG_MASS, "PrehistoricPacuBaby", 60.000004f, 20f, PrehistoricPacuTuning.EGG_CHANCES_BASE, 500, false, true, 0.75f, false);
		gameObject.AddTag(GameTags.LargeCreature);
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x06000697 RID: 1687 RVA: 0x0002FA69 File Offset: 0x0002DC69
	public void OnPrefabInit(GameObject prefab)
	{
		prefab.AddOrGet<LoopingSounds>();
	}

	// Token: 0x06000698 RID: 1688 RVA: 0x0002FA72 File Offset: 0x0002DC72
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004F7 RID: 1271
	public const string ID = "PrehistoricPacu";

	// Token: 0x040004F8 RID: 1272
	public const string BASE_TRAIT_ID = "PrehistoricPacuBaseTrait";

	// Token: 0x040004F9 RID: 1273
	public const string EGG_ID = "PrehistoricPacuEgg";

	// Token: 0x040004FA RID: 1274
	public const int EGG_SORT_ORDER = 500;
}
