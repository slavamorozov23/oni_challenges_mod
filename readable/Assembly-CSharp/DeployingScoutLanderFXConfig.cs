using System;
using UnityEngine;

// Token: 0x0200031E RID: 798
public class DeployingScoutLanderFXConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06001074 RID: 4212 RVA: 0x000626D7 File Offset: 0x000608D7
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001075 RID: 4213 RVA: 0x000626DE File Offset: 0x000608DE
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06001076 RID: 4214 RVA: 0x000626E1 File Offset: 0x000608E1
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity("DeployingScoutLanderFXConfig", "DeployingScoutLanderFXConfig", false);
		ClusterFXEntity clusterFXEntity = gameObject.AddOrGet<ClusterFXEntity>();
		clusterFXEntity.kAnimName = "rover01_kanim";
		clusterFXEntity.animName = "landing";
		clusterFXEntity.animPlayMode = KAnim.PlayMode.Loop;
		return gameObject;
	}

	// Token: 0x06001077 RID: 4215 RVA: 0x00062715 File Offset: 0x00060915
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001078 RID: 4216 RVA: 0x00062717 File Offset: 0x00060917
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000A87 RID: 2695
	public const string ID = "DeployingScoutLanderFXConfig";
}
