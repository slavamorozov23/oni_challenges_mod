using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020000D0 RID: 208
[EntityConfigOrder(1)]
public class ButterflyConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060003AA RID: 938 RVA: 0x0001FE30 File Offset: 0x0001E030
	public static GameObject CreateButterfly(string id, string name, string desc, string anim_file)
	{
		GameObject gameObject = BaseButterflyConfig.BaseButterfly(id, name, desc, anim_file, "ButterflyBaseTrait", null);
		gameObject.AddOrGetDef<AgeMonitor.Def>();
		gameObject.AddOrGetDef<FixedCapturableMonitor.Def>();
		Trait trait = Db.Get().CreateTrait("ButterflyBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 5f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 5f, name, false, false, true));
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x060003AB RID: 939 RVA: 0x0001FED4 File Offset: 0x0001E0D4
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC4;
	}

	// Token: 0x060003AC RID: 940 RVA: 0x0001FEDB File Offset: 0x0001E0DB
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060003AD RID: 941 RVA: 0x0001FEDE File Offset: 0x0001E0DE
	public GameObject CreatePrefab()
	{
		GameObject gameObject = ButterflyConfig.CreateButterfly("Butterfly", CREATURES.SPECIES.BUTTERFLY.NAME, CREATURES.SPECIES.BUTTERFLY.DESC, "pollinator_kanim");
		gameObject.AddTag(GameTags.Creatures.Pollinator);
		return gameObject;
	}

	// Token: 0x060003AE RID: 942 RVA: 0x0001FF0E File Offset: 0x0001E10E
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060003AF RID: 943 RVA: 0x0001FF10 File Offset: 0x0001E110
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040002DC RID: 732
	public const string ID = "Butterfly";

	// Token: 0x040002DD RID: 733
	public const string BASE_TRAIT_ID = "ButterflyBaseTrait";
}
