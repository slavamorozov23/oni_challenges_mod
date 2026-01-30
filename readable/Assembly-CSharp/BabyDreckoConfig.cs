using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000124 RID: 292
[EntityConfigOrder(3)]
public class BabyDreckoConfig : IEntityConfig
{
	// Token: 0x0600057B RID: 1403 RVA: 0x0002B240 File Offset: 0x00029440
	public GameObject CreatePrefab()
	{
		GameObject gameObject = DreckoConfig.CreateDrecko("DreckoBaby", CREATURES.SPECIES.DRECKO.BABY.NAME, CREATURES.SPECIES.DRECKO.BABY.DESC, "baby_drecko_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Drecko", null, false, 5f).AddOrGetDef<BabyMonitor.Def>().configureAdultOnMaturation = delegate(GameObject go)
		{
			AmountInstance amountInstance = Db.Get().Amounts.ScaleGrowth.Lookup(go);
			amountInstance.value = amountInstance.GetMax() * DreckoConfig.SCALE_INITIAL_GROWTH_PCT;
		};
		return gameObject;
	}

	// Token: 0x0600057C RID: 1404 RVA: 0x0002B2B1 File Offset: 0x000294B1
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600057D RID: 1405 RVA: 0x0002B2B3 File Offset: 0x000294B3
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000408 RID: 1032
	public const string ID = "DreckoBaby";
}
