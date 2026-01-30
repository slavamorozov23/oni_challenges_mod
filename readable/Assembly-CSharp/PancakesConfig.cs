using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001F3 RID: 499
public class PancakesConfig : IEntityConfig
{
	// Token: 0x06000A17 RID: 2583 RVA: 0x00040060 File Offset: 0x0003E260
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("Pancakes", STRINGS.ITEMS.FOOD.PANCAKES.NAME, STRINGS.ITEMS.FOOD.PANCAKES.DESC, 1f, false, Assets.GetAnim("stackedpancakes_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.8f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.PANCAKES);
	}

	// Token: 0x06000A18 RID: 2584 RVA: 0x000400C4 File Offset: 0x0003E2C4
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000A19 RID: 2585 RVA: 0x000400C6 File Offset: 0x0003E2C6
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000745 RID: 1861
	public const string ID = "Pancakes";

	// Token: 0x04000746 RID: 1862
	public static ComplexRecipe recipe;
}
