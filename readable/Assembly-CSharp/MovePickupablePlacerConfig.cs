using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000395 RID: 917
public class MovePickupablePlacerConfig : CommonPlacerConfig, IEntityConfig
{
	// Token: 0x060012F8 RID: 4856 RVA: 0x0006E460 File Offset: 0x0006C660
	public GameObject CreatePrefab()
	{
		GameObject gameObject = base.CreatePrefab(MovePickupablePlacerConfig.ID, MISC.PLACERS.MOVEPICKUPABLEPLACER.NAME, Assets.instance.movePickupToPlacerAssets.material);
		gameObject.AddOrGet<CancellableMove>();
		Storage storage = gameObject.AddOrGet<Storage>();
		storage.showInUI = false;
		storage.showUnreachableStatus = true;
		gameObject.AddOrGet<Approachable>();
		gameObject.AddOrGet<Prioritizable>();
		gameObject.AddTag(GameTags.NotConversationTopic);
		return gameObject;
	}

	// Token: 0x060012F9 RID: 4857 RVA: 0x0006E4C4 File Offset: 0x0006C6C4
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x060012FA RID: 4858 RVA: 0x0006E4C6 File Offset: 0x0006C6C6
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000BFD RID: 3069
	public static string ID = "MovePickupablePlacer";

	// Token: 0x02001253 RID: 4691
	[Serializable]
	public class MovePickupablePlacerAssets
	{
		// Token: 0x0400678E RID: 26510
		public Material material;
	}
}
