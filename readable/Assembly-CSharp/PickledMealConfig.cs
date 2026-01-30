using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001F5 RID: 501
public class PickledMealConfig : IEntityConfig
{
	// Token: 0x06000A21 RID: 2593 RVA: 0x0004014C File Offset: 0x0003E34C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("PickledMeal", STRINGS.ITEMS.FOOD.PICKLEDMEAL.NAME, STRINGS.ITEMS.FOOD.PICKLEDMEAL.DESC, 1f, false, Assets.GetAnim("pickledmeal_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.PICKLEDMEAL);
		gameObject.GetComponent<KPrefabID>().AddTag(GameTags.Pickled, false);
		return gameObject;
	}

	// Token: 0x06000A22 RID: 2594 RVA: 0x000401C1 File Offset: 0x0003E3C1
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000A23 RID: 2595 RVA: 0x000401C3 File Offset: 0x0003E3C3
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400074A RID: 1866
	public const string ID = "PickledMeal";

	// Token: 0x0400074B RID: 1867
	public static ComplexRecipe recipe;
}
