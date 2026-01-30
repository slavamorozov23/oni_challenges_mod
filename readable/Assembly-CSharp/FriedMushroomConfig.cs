using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001E7 RID: 487
public class FriedMushroomConfig : IEntityConfig
{
	// Token: 0x060009DF RID: 2527 RVA: 0x0003F9AC File Offset: 0x0003DBAC
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("FriedMushroom", STRINGS.ITEMS.FOOD.FRIEDMUSHROOM.NAME, STRINGS.ITEMS.FOOD.FRIEDMUSHROOM.DESC, 1f, false, Assets.GetAnim("funguscapfried_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.6f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.FRIED_MUSHROOM);
	}

	// Token: 0x060009E0 RID: 2528 RVA: 0x0003FA10 File Offset: 0x0003DC10
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060009E1 RID: 2529 RVA: 0x0003FA12 File Offset: 0x0003DC12
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400072A RID: 1834
	public const string ID = "FriedMushroom";

	// Token: 0x0400072B RID: 1835
	public static ComplexRecipe recipe;
}
