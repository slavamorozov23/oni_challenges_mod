using System;
using UnityEngine;

// Token: 0x02000347 RID: 839
public class SimpleFXConfig : IEntityConfig
{
	// Token: 0x0600115B RID: 4443 RVA: 0x00066A8A File Offset: 0x00064C8A
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(SimpleFXConfig.ID, SimpleFXConfig.ID, false);
		gameObject.AddOrGet<KBatchedAnimController>();
		return gameObject;
	}

	// Token: 0x0600115C RID: 4444 RVA: 0x00066AA3 File Offset: 0x00064CA3
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x0600115D RID: 4445 RVA: 0x00066AA5 File Offset: 0x00064CA5
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000B04 RID: 2820
	public static readonly string ID = "SimpleFX";
}
