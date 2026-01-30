using System;
using UnityEngine;

// Token: 0x02000324 RID: 804
public class EyeAnimation : IEntityConfig
{
	// Token: 0x0600109A RID: 4250 RVA: 0x00062D14 File Offset: 0x00060F14
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(EyeAnimation.ID, EyeAnimation.ID, false);
		gameObject.AddOrGet<KBatchedAnimController>().AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("anim_blinks_kanim")
		};
		return gameObject;
	}

	// Token: 0x0600109B RID: 4251 RVA: 0x00062D56 File Offset: 0x00060F56
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x0600109C RID: 4252 RVA: 0x00062D58 File Offset: 0x00060F58
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000A95 RID: 2709
	public static string ID = "EyeAnimation";
}
