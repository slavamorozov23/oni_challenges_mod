using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001ED RID: 493
public class LettuceConfig : IEntityConfig
{
	// Token: 0x060009FC RID: 2556 RVA: 0x0003FC88 File Offset: 0x0003DE88
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("Lettuce", STRINGS.ITEMS.FOOD.LETTUCE.NAME, STRINGS.ITEMS.FOOD.LETTUCE.DESC, 1f, false, Assets.GetAnim("sea_lettuce_leaves_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.LETTUCE);
	}

	// Token: 0x060009FD RID: 2557 RVA: 0x0003FCEC File Offset: 0x0003DEEC
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060009FE RID: 2558 RVA: 0x0003FCEE File Offset: 0x0003DEEE
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000737 RID: 1847
	public const string ID = "Lettuce";
}
