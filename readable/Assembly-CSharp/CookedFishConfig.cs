using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001D9 RID: 473
public class CookedFishConfig : IEntityConfig
{
	// Token: 0x06000997 RID: 2455 RVA: 0x0003F310 File Offset: 0x0003D510
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("CookedFish", STRINGS.ITEMS.FOOD.COOKEDFISH.NAME, STRINGS.ITEMS.FOOD.COOKEDFISH.DESC, 1f, false, Assets.GetAnim("grilled_pacu_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.COOKED_FISH);
	}

	// Token: 0x06000998 RID: 2456 RVA: 0x0003F374 File Offset: 0x0003D574
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000999 RID: 2457 RVA: 0x0003F376 File Offset: 0x0003D576
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000711 RID: 1809
	public const string ID = "CookedFish";

	// Token: 0x04000712 RID: 1810
	public static ComplexRecipe recipe;
}
