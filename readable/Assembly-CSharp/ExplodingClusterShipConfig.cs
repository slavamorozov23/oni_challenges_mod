using System;
using UnityEngine;

// Token: 0x02000323 RID: 803
public class ExplodingClusterShipConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06001094 RID: 4244 RVA: 0x00062CCE File Offset: 0x00060ECE
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001095 RID: 4245 RVA: 0x00062CD5 File Offset: 0x00060ED5
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06001096 RID: 4246 RVA: 0x00062CD8 File Offset: 0x00060ED8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity("ExplodingClusterShip", "ExplodingClusterShip", false);
		ClusterFXEntity clusterFXEntity = gameObject.AddOrGet<ClusterFXEntity>();
		clusterFXEntity.kAnimName = "rocket_self_destruct_kanim";
		clusterFXEntity.animName = "explode";
		return gameObject;
	}

	// Token: 0x06001097 RID: 4247 RVA: 0x00062D05 File Offset: 0x00060F05
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001098 RID: 4248 RVA: 0x00062D07 File Offset: 0x00060F07
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000A94 RID: 2708
	public const string ID = "ExplodingClusterShip";
}
