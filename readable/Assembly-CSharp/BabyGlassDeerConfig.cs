using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000128 RID: 296
[EntityConfigOrder(4)]
public class BabyGlassDeerConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000591 RID: 1425 RVA: 0x0002B9A5 File Offset: 0x00029BA5
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x06000592 RID: 1426 RVA: 0x0002B9AC File Offset: 0x00029BAC
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000593 RID: 1427 RVA: 0x0002B9B0 File Offset: 0x00029BB0
	public GameObject CreatePrefab()
	{
		GameObject gameObject = GlassDeerConfig.CreateGlassDeer("GlassDeerBaby", CREATURES.SPECIES.GLASSDEER.BABY.NAME, CREATURES.SPECIES.GLASSDEER.BABY.DESC, "baby_ice_floof_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "GlassDeer", null, false, 5f).AddOrGetDef<BabyMonitor.Def>().configureAdultOnMaturation = delegate(GameObject go)
		{
			AmountInstance amountInstance = Db.Get().Amounts.ScaleGrowth.Lookup(go);
			amountInstance.value = amountInstance.GetMax() * 0.5f;
		};
		return gameObject;
	}

	// Token: 0x06000594 RID: 1428 RVA: 0x0002BA21 File Offset: 0x00029C21
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000595 RID: 1429 RVA: 0x0002BA23 File Offset: 0x00029C23
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400042E RID: 1070
	public const string ID = "GlassDeerBaby";
}
