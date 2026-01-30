using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000274 RID: 628
public class FarmStationToolsConfig : IEntityConfig
{
	// Token: 0x06000CC0 RID: 3264 RVA: 0x0004C6A4 File Offset: 0x0004A8A4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("FarmStationTools", ITEMS.INDUSTRIAL_PRODUCTS.FARM_STATION_TOOLS.NAME, ITEMS.INDUSTRIAL_PRODUCTS.FARM_STATION_TOOLS.DESC, 5f, true, Assets.GetAnim("kit_planttender_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.6f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.MiscPickupable
		});
		gameObject.AddOrGet<EntitySplitter>();
		return gameObject;
	}

	// Token: 0x06000CC1 RID: 3265 RVA: 0x0004C714 File Offset: 0x0004A914
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000CC2 RID: 3266 RVA: 0x0004C716 File Offset: 0x0004A916
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040008D5 RID: 2261
	public const string ID = "FarmStationTools";

	// Token: 0x040008D6 RID: 2262
	public static readonly Tag tag = TagManager.Create("FarmStationTools");

	// Token: 0x040008D7 RID: 2263
	public const float MASS = 5f;
}
