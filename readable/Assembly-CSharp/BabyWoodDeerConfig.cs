using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000177 RID: 375
[EntityConfigOrder(3)]
public class BabyWoodDeerConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000749 RID: 1865 RVA: 0x000327CF File Offset: 0x000309CF
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x0600074A RID: 1866 RVA: 0x000327D6 File Offset: 0x000309D6
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x0600074B RID: 1867 RVA: 0x000327DC File Offset: 0x000309DC
	public GameObject CreatePrefab()
	{
		GameObject gameObject = WoodDeerConfig.CreateWoodDeer("WoodDeerBaby", CREATURES.SPECIES.WOODDEER.BABY.NAME, CREATURES.SPECIES.WOODDEER.BABY.DESC, "baby_ice_floof_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "WoodDeer", null, false, 5f).AddOrGetDef<BabyMonitor.Def>().configureAdultOnMaturation = delegate(GameObject go)
		{
			AmountInstance amountInstance = Db.Get().Amounts.ScaleGrowth.Lookup(go);
			amountInstance.value = amountInstance.GetMax() * WoodDeerConfig.ANTLER_STARTING_GROWTH_PCT;
		};
		return gameObject;
	}

	// Token: 0x0600074C RID: 1868 RVA: 0x0003284D File Offset: 0x00030A4D
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600074D RID: 1869 RVA: 0x0003284F File Offset: 0x00030A4F
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000597 RID: 1431
	public const string ID = "WoodDeerBaby";
}
