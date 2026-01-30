using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001FD RID: 509
public class SalsaConfig : IEntityConfig
{
	// Token: 0x06000A49 RID: 2633 RVA: 0x000406AC File Offset: 0x0003E8AC
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("Salsa", STRINGS.ITEMS.FOOD.SALSA.NAME, STRINGS.ITEMS.FOOD.SALSA.DESC, 1f, false, Assets.GetAnim("zestysalsa_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.5f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.SALSA);
	}

	// Token: 0x06000A4A RID: 2634 RVA: 0x00040710 File Offset: 0x0003E910
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000A4B RID: 2635 RVA: 0x00040712 File Offset: 0x0003E912
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000759 RID: 1881
	public const string ID = "Salsa";

	// Token: 0x0400075A RID: 1882
	public static ComplexRecipe recipe;
}
