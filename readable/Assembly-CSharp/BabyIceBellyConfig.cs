using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000135 RID: 309
[EntityConfigOrder(3)]
public class BabyIceBellyConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060005DA RID: 1498 RVA: 0x0002CC8F File Offset: 0x0002AE8F
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x060005DB RID: 1499 RVA: 0x0002CC96 File Offset: 0x0002AE96
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060005DC RID: 1500 RVA: 0x0002CC9C File Offset: 0x0002AE9C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = IceBellyConfig.CreateIceBelly("IceBellyBaby", CREATURES.SPECIES.ICEBELLY.BABY.NAME, CREATURES.SPECIES.ICEBELLY.BABY.DESC, "baby_icebelly_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "IceBelly", null, false, 5f).AddOrGetDef<BabyMonitor.Def>().configureAdultOnMaturation = delegate(GameObject go)
		{
			AmountInstance amountInstance = Db.Get().Amounts.ScaleGrowth.Lookup(go);
			amountInstance.value = amountInstance.GetMax() * IceBellyConfig.SCALE_INITIAL_GROWTH_PCT;
		};
		return gameObject;
	}

	// Token: 0x060005DD RID: 1501 RVA: 0x0002CD0D File Offset: 0x0002AF0D
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060005DE RID: 1502 RVA: 0x0002CD0F File Offset: 0x0002AF0F
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400046E RID: 1134
	public const string ID = "IceBellyBaby";
}
