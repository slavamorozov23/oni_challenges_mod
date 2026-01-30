using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001DC RID: 476
public class CurryConfig : IEntityConfig
{
	// Token: 0x060009A5 RID: 2469 RVA: 0x0003F46C File Offset: 0x0003D66C
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("Curry", STRINGS.ITEMS.FOOD.CURRY.NAME, STRINGS.ITEMS.FOOD.CURRY.DESC, 1f, false, Assets.GetAnim("curried_beans_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.9f, 0.5f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.CURRY);
	}

	// Token: 0x060009A6 RID: 2470 RVA: 0x0003F4D0 File Offset: 0x0003D6D0
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060009A7 RID: 2471 RVA: 0x0003F4D2 File Offset: 0x0003D6D2
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000717 RID: 1815
	public const string ID = "Curry";
}
