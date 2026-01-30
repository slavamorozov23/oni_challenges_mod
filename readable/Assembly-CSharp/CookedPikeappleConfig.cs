using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001DB RID: 475
public class CookedPikeappleConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x0600099F RID: 2463 RVA: 0x0003F3F0 File Offset: 0x0003D5F0
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x060009A0 RID: 2464 RVA: 0x0003F3F7 File Offset: 0x0003D5F7
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060009A1 RID: 2465 RVA: 0x0003F3FC File Offset: 0x0003D5FC
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("CookedPikeapple", STRINGS.ITEMS.FOOD.COOKEDPIKEAPPLE.NAME, STRINGS.ITEMS.FOOD.COOKEDPIKEAPPLE.DESC, 1f, false, Assets.GetAnim("iceberry_cooked_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.6f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.COOKED_PIKEAPPLE);
	}

	// Token: 0x060009A2 RID: 2466 RVA: 0x0003F460 File Offset: 0x0003D660
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060009A3 RID: 2467 RVA: 0x0003F462 File Offset: 0x0003D662
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000715 RID: 1813
	public const string ID = "CookedPikeapple";

	// Token: 0x04000716 RID: 1814
	public static ComplexRecipe recipe;
}
