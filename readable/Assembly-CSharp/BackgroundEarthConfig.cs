using System;
using UnityEngine;

// Token: 0x020002FE RID: 766
public class BackgroundEarthConfig : IEntityConfig
{
	// Token: 0x06000F95 RID: 3989 RVA: 0x0005BC20 File Offset: 0x00059E20
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(BackgroundEarthConfig.ID, BackgroundEarthConfig.ID, true);
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("earth_kanim")
		};
		kbatchedAnimController.isMovable = true;
		kbatchedAnimController.initialAnim = "idle";
		kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
		kbatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.OffscreenUpdate;
		gameObject.AddOrGet<LoopingSounds>();
		return gameObject;
	}

	// Token: 0x06000F96 RID: 3990 RVA: 0x0005BC8B File Offset: 0x00059E8B
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000F97 RID: 3991 RVA: 0x0005BC8D File Offset: 0x00059E8D
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000A36 RID: 2614
	public static string ID = "BackgroundEarth";
}
