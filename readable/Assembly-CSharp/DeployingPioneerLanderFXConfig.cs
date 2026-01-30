using System;
using UnityEngine;

// Token: 0x0200031D RID: 797
public class DeployingPioneerLanderFXConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x0600106E RID: 4206 RVA: 0x0006268D File Offset: 0x0006088D
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x0600106F RID: 4207 RVA: 0x00062694 File Offset: 0x00060894
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06001070 RID: 4208 RVA: 0x00062697 File Offset: 0x00060897
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity("DeployingPioneerLanderFX", "DeployingPioneerLanderFX", false);
		ClusterFXEntity clusterFXEntity = gameObject.AddOrGet<ClusterFXEntity>();
		clusterFXEntity.kAnimName = "pioneer01_kanim";
		clusterFXEntity.animName = "landing";
		clusterFXEntity.animPlayMode = KAnim.PlayMode.Loop;
		return gameObject;
	}

	// Token: 0x06001071 RID: 4209 RVA: 0x000626CB File Offset: 0x000608CB
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001072 RID: 4210 RVA: 0x000626CD File Offset: 0x000608CD
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000A86 RID: 2694
	public const string ID = "DeployingPioneerLanderFX";
}
