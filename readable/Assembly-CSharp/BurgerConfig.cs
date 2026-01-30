using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001D3 RID: 467
public class BurgerConfig : IEntityConfig
{
	// Token: 0x06000978 RID: 2424 RVA: 0x0003EF28 File Offset: 0x0003D128
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("Burger", STRINGS.ITEMS.FOOD.BURGER.NAME, STRINGS.ITEMS.FOOD.BURGER.DESC, 1f, false, Assets.GetAnim("frost_burger_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.BURGER);
	}

	// Token: 0x06000979 RID: 2425 RVA: 0x0003EF8C File Offset: 0x0003D18C
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600097A RID: 2426 RVA: 0x0003EF8E File Offset: 0x0003D18E
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000702 RID: 1794
	public const string ID = "Burger";

	// Token: 0x04000703 RID: 1795
	public static ComplexRecipe recipe;
}
