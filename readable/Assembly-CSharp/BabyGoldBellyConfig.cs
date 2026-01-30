using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x0200012B RID: 299
[EntityConfigOrder(4)]
public class BabyGoldBellyConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060005A3 RID: 1443 RVA: 0x0002C004 File Offset: 0x0002A204
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x060005A4 RID: 1444 RVA: 0x0002C00B File Offset: 0x0002A20B
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060005A5 RID: 1445 RVA: 0x0002C010 File Offset: 0x0002A210
	public GameObject CreatePrefab()
	{
		GameObject gameObject = GoldBellyConfig.CreateGoldBelly("GoldBellyBaby", CREATURES.SPECIES.ICEBELLY.VARIANT_GOLD.BABY.NAME, CREATURES.SPECIES.ICEBELLY.VARIANT_GOLD.BABY.DESC, "baby_icebelly_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "GoldBelly", null, false, 5f).AddOrGetDef<BabyMonitor.Def>().configureAdultOnMaturation = delegate(GameObject go)
		{
			AmountInstance amountInstance = Db.Get().Amounts.ScaleGrowth.Lookup(go);
			amountInstance.value = amountInstance.GetMax() * GoldBellyConfig.SCALE_INITIAL_GROWTH_PCT;
		};
		return gameObject;
	}

	// Token: 0x060005A6 RID: 1446 RVA: 0x0002C081 File Offset: 0x0002A281
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060005A7 RID: 1447 RVA: 0x0002C083 File Offset: 0x0002A283
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000442 RID: 1090
	public const string ID = "GoldBellyBaby";
}
