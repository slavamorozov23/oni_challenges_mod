using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001F9 RID: 505
public class QuicheConfig : IEntityConfig
{
	// Token: 0x06000A37 RID: 2615 RVA: 0x00040450 File Offset: 0x0003E650
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("Quiche", STRINGS.ITEMS.FOOD.QUICHE.NAME, STRINGS.ITEMS.FOOD.QUICHE.DESC, 1f, false, Assets.GetAnim("quiche_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.QUICHE);
	}

	// Token: 0x06000A38 RID: 2616 RVA: 0x000404B4 File Offset: 0x0003E6B4
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000A39 RID: 2617 RVA: 0x000404B6 File Offset: 0x0003E6B6
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000751 RID: 1873
	public const string ID = "Quiche";

	// Token: 0x04000752 RID: 1874
	public static ComplexRecipe recipe;
}
