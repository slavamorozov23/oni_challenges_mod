using System;
using UnityEngine;

// Token: 0x02000273 RID: 627
public class FabricatedWoodConfig : IOreConfig
{
	// Token: 0x17000019 RID: 25
	// (get) Token: 0x06000CBC RID: 3260 RVA: 0x0004C665 File Offset: 0x0004A865
	public SimHashes ElementID
	{
		get
		{
			return SimHashes.FabricatedWood;
		}
	}

	// Token: 0x06000CBD RID: 3261 RVA: 0x0004C66C File Offset: 0x0004A86C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateSolidOreEntity(this.ElementID, null);
		gameObject.GetComponent<KPrefabID>().RemoveTag(GameTags.HideFromSpawnTool);
		return gameObject;
	}

	// Token: 0x040008D3 RID: 2259
	public const string ID = "FabricatedWood";

	// Token: 0x040008D4 RID: 2260
	public static readonly Tag TAG = TagManager.Create("FabricatedWood");
}
