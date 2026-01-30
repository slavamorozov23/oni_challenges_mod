using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000213 RID: 531
public class WormSuperFoodConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000AB8 RID: 2744 RVA: 0x000411AC File Offset: 0x0003F3AC
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000AB9 RID: 2745 RVA: 0x000411B3 File Offset: 0x0003F3B3
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000ABA RID: 2746 RVA: 0x000411B8 File Offset: 0x0003F3B8
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("WormSuperFood", STRINGS.ITEMS.FOOD.WORMSUPERFOOD.NAME, STRINGS.ITEMS.FOOD.WORMSUPERFOOD.DESC, 1f, false, Assets.GetAnim("wormwood_preserved_berries_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.7f, 0.6f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.WORMSUPERFOOD);
	}

	// Token: 0x06000ABB RID: 2747 RVA: 0x0004121C File Offset: 0x0003F41C
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000ABC RID: 2748 RVA: 0x0004121E File Offset: 0x0003F41E
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400078D RID: 1933
	public const string ID = "WormSuperFood";

	// Token: 0x0400078E RID: 1934
	public static ComplexRecipe recipe;
}
