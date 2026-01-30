using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001EB RID: 491
public class GrilledPrickleFruitConfig : IEntityConfig
{
	// Token: 0x060009F1 RID: 2545 RVA: 0x0003FB78 File Offset: 0x0003DD78
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("GrilledPrickleFruit", STRINGS.ITEMS.FOOD.GRILLEDPRICKLEFRUIT.NAME, STRINGS.ITEMS.FOOD.GRILLEDPRICKLEFRUIT.DESC, 1f, false, Assets.GetAnim("gristleberry_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.7f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.GRILLED_PRICKLEFRUIT);
	}

	// Token: 0x060009F2 RID: 2546 RVA: 0x0003FBDC File Offset: 0x0003DDDC
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060009F3 RID: 2547 RVA: 0x0003FBDE File Offset: 0x0003DDDE
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000733 RID: 1843
	public const string ID = "GrilledPrickleFruit";

	// Token: 0x04000734 RID: 1844
	public static ComplexRecipe recipe;
}
