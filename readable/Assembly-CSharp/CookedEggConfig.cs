using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001D8 RID: 472
public class CookedEggConfig : IEntityConfig
{
	// Token: 0x06000993 RID: 2451 RVA: 0x0003F2A0 File Offset: 0x0003D4A0
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("CookedEgg", STRINGS.ITEMS.FOOD.COOKEDEGG.NAME, STRINGS.ITEMS.FOOD.COOKEDEGG.DESC, 1f, false, Assets.GetAnim("cookedegg_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.COOKED_EGG);
	}

	// Token: 0x06000994 RID: 2452 RVA: 0x0003F304 File Offset: 0x0003D504
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000995 RID: 2453 RVA: 0x0003F306 File Offset: 0x0003D506
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400070F RID: 1807
	public const string ID = "CookedEgg";

	// Token: 0x04000710 RID: 1808
	public static ComplexRecipe recipe;
}
