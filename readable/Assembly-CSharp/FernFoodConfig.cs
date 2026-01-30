using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001E3 RID: 483
public class FernFoodConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060009CC RID: 2508 RVA: 0x0003F7D2 File Offset: 0x0003D9D2
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC4;
	}

	// Token: 0x060009CD RID: 2509 RVA: 0x0003F7D9 File Offset: 0x0003D9D9
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060009CE RID: 2510 RVA: 0x0003F7DC File Offset: 0x0003D9DC
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity(FernFoodConfig.ID, STRINGS.ITEMS.FOOD.FERNFOOD.NAME, STRINGS.ITEMS.FOOD.FERNFOOD.DESC, 1f, true, Assets.GetAnim("megafrond_grain_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.FERNFOOD);
	}

	// Token: 0x060009CF RID: 2511 RVA: 0x0003F840 File Offset: 0x0003DA40
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060009D0 RID: 2512 RVA: 0x0003F842 File Offset: 0x0003DA42
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000725 RID: 1829
	public static string ID = "FernFood";
}
