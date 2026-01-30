using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200020F RID: 527
public class TofuConfig : IEntityConfig
{
	// Token: 0x06000AA1 RID: 2721 RVA: 0x00040FBC File Offset: 0x0003F1BC
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("Tofu", STRINGS.ITEMS.FOOD.TOFU.NAME, STRINGS.ITEMS.FOOD.TOFU.DESC, 1f, false, Assets.GetAnim("loafu_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.9f, 0.6f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.TOFU);
	}

	// Token: 0x06000AA2 RID: 2722 RVA: 0x00041020 File Offset: 0x0003F220
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000AA3 RID: 2723 RVA: 0x00041022 File Offset: 0x0003F222
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000785 RID: 1925
	public const string ID = "Tofu";

	// Token: 0x04000786 RID: 1926
	public const string ANIM = "loafu_kanim";

	// Token: 0x04000787 RID: 1927
	public static ComplexRecipe recipe;
}
