using System;
using UnityEngine;

// Token: 0x02000340 RID: 832
public class OrbitalBGConfig : IEntityConfig
{
	// Token: 0x06001135 RID: 4405 RVA: 0x0006656A File Offset: 0x0006476A
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(OrbitalBGConfig.ID, OrbitalBGConfig.ID, false);
		gameObject.AddOrGet<LoopingSounds>();
		gameObject.AddOrGet<OrbitalObject>();
		gameObject.AddOrGet<SaveLoadRoot>();
		return gameObject;
	}

	// Token: 0x06001136 RID: 4406 RVA: 0x00066591 File Offset: 0x00064791
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06001137 RID: 4407 RVA: 0x00066593 File Offset: 0x00064793
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000AFA RID: 2810
	public static string ID = "OrbitalBG";
}
