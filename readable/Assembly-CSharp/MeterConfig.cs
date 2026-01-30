using System;
using UnityEngine;

// Token: 0x02000336 RID: 822
public class MeterConfig : IEntityConfig
{
	// Token: 0x060010FE RID: 4350 RVA: 0x00065503 File Offset: 0x00063703
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(MeterConfig.ID, MeterConfig.ID, false);
		gameObject.AddOrGet<KBatchedAnimController>();
		gameObject.AddOrGet<KBatchedAnimTracker>();
		return gameObject;
	}

	// Token: 0x060010FF RID: 4351 RVA: 0x00065523 File Offset: 0x00063723
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06001100 RID: 4352 RVA: 0x00065525 File Offset: 0x00063725
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000AE6 RID: 2790
	public static readonly string ID = "Meter";
}
