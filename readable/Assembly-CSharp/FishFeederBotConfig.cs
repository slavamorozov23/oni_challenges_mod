using System;
using UnityEngine;

// Token: 0x0200017C RID: 380
public class FishFeederBotConfig : IEntityConfig
{
	// Token: 0x06000763 RID: 1891 RVA: 0x00033200 File Offset: 0x00031400
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity("FishFeederBot", "FishFeederBot", true);
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("fishfeeder_kanim")
		};
		kbatchedAnimController.sceneLayer = Grid.SceneLayer.BuildingBack;
		SymbolOverrideControllerUtil.AddToPrefab(kbatchedAnimController.gameObject);
		return gameObject;
	}

	// Token: 0x06000764 RID: 1892 RVA: 0x00033258 File Offset: 0x00031458
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000765 RID: 1893 RVA: 0x0003325A File Offset: 0x0003145A
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005A4 RID: 1444
	public const string ID = "FishFeederBot";
}
