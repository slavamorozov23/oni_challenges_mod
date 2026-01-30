using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000394 RID: 916
public class MopPlacerConfig : CommonPlacerConfig, IEntityConfig
{
	// Token: 0x060012F3 RID: 4851 RVA: 0x0006E3EC File Offset: 0x0006C5EC
	public GameObject CreatePrefab()
	{
		GameObject gameObject = base.CreatePrefab(MopPlacerConfig.ID, MISC.PLACERS.MOPPLACER.NAME, Assets.instance.mopPlacerAssets.material);
		gameObject.AddTag(GameTags.NotConversationTopic);
		Moppable moppable = gameObject.AddOrGet<Moppable>();
		moppable.synchronizeAnims = false;
		moppable.amountMoppedPerTick = 20f;
		gameObject.AddOrGet<Cancellable>();
		return gameObject;
	}

	// Token: 0x060012F4 RID: 4852 RVA: 0x0006E446 File Offset: 0x0006C646
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x060012F5 RID: 4853 RVA: 0x0006E448 File Offset: 0x0006C648
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000BFC RID: 3068
	public static string ID = "MopPlacer";

	// Token: 0x02001252 RID: 4690
	[Serializable]
	public class MopPlacerAssets
	{
		// Token: 0x0400678D RID: 26509
		public Material material;
	}
}
