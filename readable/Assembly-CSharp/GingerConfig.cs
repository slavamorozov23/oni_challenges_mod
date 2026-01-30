using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001A5 RID: 421
public class GingerConfig : IEntityConfig
{
	// Token: 0x06000837 RID: 2103 RVA: 0x00038154 File Offset: 0x00036354
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity(GingerConfig.ID, STRINGS.ITEMS.INGREDIENTS.GINGER.NAME, STRINGS.ITEMS.INGREDIENTS.GINGER.DESC, 1f, true, Assets.GetAnim("ginger_kanim"), "object", Grid.SceneLayer.BuildingBack, EntityTemplates.CollisionShape.RECTANGLE, 0.45f, 0.4f, true, TUNING.SORTORDER.BUILDINGELEMENTS + GingerConfig.SORTORDER, SimHashes.Creature, new List<Tag>
		{
			GameTags.IndustrialIngredient
		});
		gameObject.AddOrGet<EntitySplitter>();
		return gameObject;
	}

	// Token: 0x06000838 RID: 2104 RVA: 0x000381CE File Offset: 0x000363CE
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000839 RID: 2105 RVA: 0x000381D0 File Offset: 0x000363D0
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000637 RID: 1591
	public static string ID = "GingerConfig";

	// Token: 0x04000638 RID: 1592
	public static int SORTORDER = 1;
}
