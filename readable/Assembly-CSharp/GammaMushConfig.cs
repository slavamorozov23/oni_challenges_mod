using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001EA RID: 490
public class GammaMushConfig : IEntityConfig
{
	// Token: 0x060009ED RID: 2541 RVA: 0x0003FB08 File Offset: 0x0003DD08
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("GammaMush", STRINGS.ITEMS.FOOD.GAMMAMUSH.NAME, STRINGS.ITEMS.FOOD.GAMMAMUSH.DESC, 1f, false, Assets.GetAnim("mushbarfried_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.GAMMAMUSH);
	}

	// Token: 0x060009EE RID: 2542 RVA: 0x0003FB6C File Offset: 0x0003DD6C
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060009EF RID: 2543 RVA: 0x0003FB6E File Offset: 0x0003DD6E
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000731 RID: 1841
	public const string ID = "GammaMush";

	// Token: 0x04000732 RID: 1842
	public static ComplexRecipe recipe;
}
