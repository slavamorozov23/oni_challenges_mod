using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200020E RID: 526
public class TableSaltConfig : IEntityConfig
{
	// Token: 0x06000A9C RID: 2716 RVA: 0x00040F10 File Offset: 0x0003F110
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity(TableSaltConfig.ID, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.TABLE_SALT.NAME, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.TABLE_SALT.DESC, 1f, false, Assets.GetAnim("seed_saltPlant_kanim"), "object", Grid.SceneLayer.BuildingBack, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.45f, true, SORTORDER.BUILDINGELEMENTS + TableSaltTuning.SORTORDER, SimHashes.Salt, new List<Tag>
		{
			GameTags.Other,
			GameTags.Experimental
		});
		gameObject.AddOrGet<EntitySplitter>();
		return gameObject;
	}

	// Token: 0x06000A9D RID: 2717 RVA: 0x00040F95 File Offset: 0x0003F195
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000A9E RID: 2718 RVA: 0x00040F97 File Offset: 0x0003F197
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000783 RID: 1923
	public static string ID = "TableSalt";

	// Token: 0x04000784 RID: 1924
	public static readonly Tag TAG = TableSaltConfig.ID.ToTag();
}
