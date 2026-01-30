using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000203 RID: 515
public class SpiceBreadConfig : IEntityConfig
{
	// Token: 0x06000A68 RID: 2664 RVA: 0x0004098C File Offset: 0x0003EB8C
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("SpiceBread", STRINGS.ITEMS.FOOD.SPICEBREAD.NAME, STRINGS.ITEMS.FOOD.SPICEBREAD.DESC, 1f, false, Assets.GetAnim("pepperbread_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.9f, 0.6f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.SPICEBREAD);
	}

	// Token: 0x06000A69 RID: 2665 RVA: 0x000409F0 File Offset: 0x0003EBF0
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000A6A RID: 2666 RVA: 0x000409F2 File Offset: 0x0003EBF2
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000766 RID: 1894
	public const string ID = "SpiceBread";

	// Token: 0x04000767 RID: 1895
	public static ComplexRecipe recipe;
}
