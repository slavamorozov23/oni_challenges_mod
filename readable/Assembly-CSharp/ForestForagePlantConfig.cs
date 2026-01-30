using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000198 RID: 408
public class ForestForagePlantConfig : IEntityConfig
{
	// Token: 0x060007F1 RID: 2033 RVA: 0x00036058 File Offset: 0x00034258
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("ForestForagePlant", STRINGS.ITEMS.FOOD.FORESTFORAGEPLANT.NAME, STRINGS.ITEMS.FOOD.FORESTFORAGEPLANT.DESC, 1f, false, Assets.GetAnim("podmelon_fruit_kanim"), "object", Grid.SceneLayer.BuildingBack, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.FORESTFORAGEPLANT);
	}

	// Token: 0x060007F2 RID: 2034 RVA: 0x000360BC File Offset: 0x000342BC
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060007F3 RID: 2035 RVA: 0x000360BE File Offset: 0x000342BE
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000602 RID: 1538
	public const string ID = "ForestForagePlant";
}
