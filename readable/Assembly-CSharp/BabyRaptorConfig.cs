using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000164 RID: 356
[EntityConfigOrder(3)]
public class BabyRaptorConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060006D0 RID: 1744 RVA: 0x000308BA File Offset: 0x0002EABA
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC4;
	}

	// Token: 0x060006D1 RID: 1745 RVA: 0x000308C1 File Offset: 0x0002EAC1
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060006D2 RID: 1746 RVA: 0x000308C4 File Offset: 0x0002EAC4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = RaptorConfig.CreateRaptor("RaptorBaby", CREATURES.SPECIES.RAPTOR.BABY.NAME, CREATURES.SPECIES.RAPTOR.BABY.DESC, "baby_raptor_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Raptor", null, false, 5f).AddOrGetDef<BabyMonitor.Def>().configureAdultOnMaturation = delegate(GameObject go)
		{
			AmountInstance amountInstance = Db.Get().Amounts.ScaleGrowth.Lookup(go);
			amountInstance.value = amountInstance.GetMax() * RaptorConfig.SCALE_INITIAL_GROWTH_PCT;
		};
		return gameObject;
	}

	// Token: 0x060006D3 RID: 1747 RVA: 0x00030935 File Offset: 0x0002EB35
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060006D4 RID: 1748 RVA: 0x00030937 File Offset: 0x0002EB37
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000532 RID: 1330
	public const string ID = "RaptorBaby";
}
