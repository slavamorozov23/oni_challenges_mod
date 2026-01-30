using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001D0 RID: 464
public class BasicPlantFoodConfig : IEntityConfig
{
	// Token: 0x06000967 RID: 2407 RVA: 0x0003EDA4 File Offset: 0x0003CFA4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("BasicPlantFood", STRINGS.ITEMS.FOOD.BASICPLANTFOOD.NAME, STRINGS.ITEMS.FOOD.BASICPLANTFOOD.DESC, 1f, false, Assets.GetAnim("meallicegrain_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.CIRCLE, 0.25f, 0.25f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.BASICPLANTFOOD);
		return gameObject;
	}

	// Token: 0x06000968 RID: 2408 RVA: 0x0003EE0A File Offset: 0x0003D00A
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000969 RID: 2409 RVA: 0x0003EE0C File Offset: 0x0003D00C
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040006FB RID: 1787
	public const string ID = "BasicPlantFood";
}
