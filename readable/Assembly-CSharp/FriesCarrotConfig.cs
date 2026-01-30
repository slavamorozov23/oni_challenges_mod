using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001E8 RID: 488
public class FriesCarrotConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060009E3 RID: 2531 RVA: 0x0003FA1C File Offset: 0x0003DC1C
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x060009E4 RID: 2532 RVA: 0x0003FA23 File Offset: 0x0003DC23
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060009E5 RID: 2533 RVA: 0x0003FA28 File Offset: 0x0003DC28
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("FriesCarrot", STRINGS.ITEMS.FOOD.FRIESCARROT.NAME, STRINGS.ITEMS.FOOD.FRIESCARROT.DESC, 1f, false, Assets.GetAnim("rootfries_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.6f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.FRIES_CARROT);
	}

	// Token: 0x060009E6 RID: 2534 RVA: 0x0003FA8C File Offset: 0x0003DC8C
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060009E7 RID: 2535 RVA: 0x0003FA8E File Offset: 0x0003DC8E
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400072C RID: 1836
	public const string ID = "FriesCarrot";

	// Token: 0x0400072D RID: 1837
	public static ComplexRecipe recipe;
}
