using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001D5 RID: 469
public class ButterflyFoodConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000981 RID: 2433 RVA: 0x0003F022 File Offset: 0x0003D222
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC4;
	}

	// Token: 0x06000982 RID: 2434 RVA: 0x0003F029 File Offset: 0x0003D229
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000983 RID: 2435 RVA: 0x0003F02C File Offset: 0x0003D22C
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("ButterflyFood", STRINGS.ITEMS.FOOD.BUTTERFLYFOOD.NAME, STRINGS.ITEMS.FOOD.BUTTERFLYFOOD.DESC, 1f, false, Assets.GetAnim("fried_mimillet_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.85f, 0.75f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.BUTTERFLYFOOD);
	}

	// Token: 0x06000984 RID: 2436 RVA: 0x0003F090 File Offset: 0x0003D290
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000985 RID: 2437 RVA: 0x0003F092 File Offset: 0x0003D292
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000708 RID: 1800
	public const string ID = "ButterflyFood";

	// Token: 0x04000709 RID: 1801
	public static ComplexRecipe recipe;
}
