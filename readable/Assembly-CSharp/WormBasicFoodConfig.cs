using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000211 RID: 529
public class WormBasicFoodConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000AAC RID: 2732 RVA: 0x000410B6 File Offset: 0x0003F2B6
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000AAD RID: 2733 RVA: 0x000410BD File Offset: 0x0003F2BD
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000AAE RID: 2734 RVA: 0x000410C0 File Offset: 0x0003F2C0
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("WormBasicFood", STRINGS.ITEMS.FOOD.WORMBASICFOOD.NAME, STRINGS.ITEMS.FOOD.WORMBASICFOOD.DESC, 1f, false, Assets.GetAnim("wormwood_roast_nuts_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.7f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.WORMBASICFOOD);
	}

	// Token: 0x06000AAF RID: 2735 RVA: 0x00041124 File Offset: 0x0003F324
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000AB0 RID: 2736 RVA: 0x00041126 File Offset: 0x0003F326
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400078A RID: 1930
	public const string ID = "WormBasicFood";

	// Token: 0x0400078B RID: 1931
	public static ComplexRecipe recipe;
}
