using System;
using UnityEngine;

// Token: 0x0200033F RID: 831
public class MouthAnimation : IEntityConfig
{
	// Token: 0x06001130 RID: 4400 RVA: 0x00066510 File Offset: 0x00064710
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(MouthAnimation.ID, MouthAnimation.ID, false);
		gameObject.AddOrGet<KBatchedAnimController>().AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("anim_mouth_flap_kanim")
		};
		return gameObject;
	}

	// Token: 0x06001131 RID: 4401 RVA: 0x00066552 File Offset: 0x00064752
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06001132 RID: 4402 RVA: 0x00066554 File Offset: 0x00064754
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000AF9 RID: 2809
	public static string ID = "MouthAnimation";
}
