using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001E6 RID: 486
public class FriedMushBarConfig : IEntityConfig
{
	// Token: 0x060009DB RID: 2523 RVA: 0x0003F93C File Offset: 0x0003DB3C
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("FriedMushBar", STRINGS.ITEMS.FOOD.FRIEDMUSHBAR.NAME, STRINGS.ITEMS.FOOD.FRIEDMUSHBAR.DESC, 1f, false, Assets.GetAnim("mushbarfried_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.FRIEDMUSHBAR);
	}

	// Token: 0x060009DC RID: 2524 RVA: 0x0003F9A0 File Offset: 0x0003DBA0
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060009DD RID: 2525 RVA: 0x0003F9A2 File Offset: 0x0003DBA2
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000728 RID: 1832
	public const string ID = "FriedMushBar";

	// Token: 0x04000729 RID: 1833
	public static ComplexRecipe recipe;
}
