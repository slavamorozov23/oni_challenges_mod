using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001E9 RID: 489
public class FruitCakeConfig : IEntityConfig
{
	// Token: 0x060009E9 RID: 2537 RVA: 0x0003FA98 File Offset: 0x0003DC98
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("FruitCake", STRINGS.ITEMS.FOOD.FRUITCAKE.NAME, STRINGS.ITEMS.FOOD.FRUITCAKE.DESC, 1f, false, Assets.GetAnim("fruitcake_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.FRUITCAKE);
	}

	// Token: 0x060009EA RID: 2538 RVA: 0x0003FAFC File Offset: 0x0003DCFC
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060009EB RID: 2539 RVA: 0x0003FAFE File Offset: 0x0003DCFE
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400072E RID: 1838
	public const string ID = "FruitCake";

	// Token: 0x0400072F RID: 1839
	public const string ANIM = "fruitcake_kanim";

	// Token: 0x04000730 RID: 1840
	public static ComplexRecipe recipe;
}
