using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200020C RID: 524
public class SwampLilyFlowerConfig : IEntityConfig
{
	// Token: 0x06000A95 RID: 2709 RVA: 0x00040E3C File Offset: 0x0003F03C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity(SwampLilyFlowerConfig.ID, ITEMS.INGREDIENTS.SWAMPLILYFLOWER.NAME, ITEMS.INGREDIENTS.SWAMPLILYFLOWER.DESC, 1f, false, Assets.GetAnim("swamplilyflower_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.IndustrialIngredient,
			GameTags.PedestalDisplayable
		});
		EntityTemplates.CreateAndRegisterCompostableFromPrefab(gameObject);
		gameObject.AddOrGet<EntitySplitter>();
		return gameObject;
	}

	// Token: 0x06000A96 RID: 2710 RVA: 0x00040EBE File Offset: 0x0003F0BE
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000A97 RID: 2711 RVA: 0x00040EC0 File Offset: 0x0003F0C0
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400077D RID: 1917
	public static float SEEDS_PER_FRUIT = 1f;

	// Token: 0x0400077E RID: 1918
	public static string ID = "SwampLilyFlower";
}
