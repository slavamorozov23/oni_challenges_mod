using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001DA RID: 474
public class CookedMeatConfig : IEntityConfig
{
	// Token: 0x0600099B RID: 2459 RVA: 0x0003F380 File Offset: 0x0003D580
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("CookedMeat", STRINGS.ITEMS.FOOD.COOKEDMEAT.NAME, STRINGS.ITEMS.FOOD.COOKEDMEAT.DESC, 1f, false, Assets.GetAnim("barbeque_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.COOKED_MEAT);
	}

	// Token: 0x0600099C RID: 2460 RVA: 0x0003F3E4 File Offset: 0x0003D5E4
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600099D RID: 2461 RVA: 0x0003F3E6 File Offset: 0x0003D5E6
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000713 RID: 1811
	public const string ID = "CookedMeat";

	// Token: 0x04000714 RID: 1812
	public static ComplexRecipe recipe;
}
