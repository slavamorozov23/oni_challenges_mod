using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001D7 RID: 471
public class ColdWheatBreadConfig : IEntityConfig
{
	// Token: 0x0600098F RID: 2447 RVA: 0x0003F230 File Offset: 0x0003D430
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("ColdWheatBread", STRINGS.ITEMS.FOOD.COLDWHEATBREAD.NAME, STRINGS.ITEMS.FOOD.COLDWHEATBREAD.DESC, 1f, false, Assets.GetAnim("frostbread_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.COLD_WHEAT_BREAD);
	}

	// Token: 0x06000990 RID: 2448 RVA: 0x0003F294 File Offset: 0x0003D494
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000991 RID: 2449 RVA: 0x0003F296 File Offset: 0x0003D496
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400070D RID: 1805
	public const string ID = "ColdWheatBread";

	// Token: 0x0400070E RID: 1806
	public static ComplexRecipe recipe;
}
