using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001D1 RID: 465
public class BerryPieConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x0600096B RID: 2411 RVA: 0x0003EE16 File Offset: 0x0003D016
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x0600096C RID: 2412 RVA: 0x0003EE1D File Offset: 0x0003D01D
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x0600096D RID: 2413 RVA: 0x0003EE20 File Offset: 0x0003D020
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("BerryPie", STRINGS.ITEMS.FOOD.BERRYPIE.NAME, STRINGS.ITEMS.FOOD.BERRYPIE.DESC, 1f, false, Assets.GetAnim("wormwood_berry_pie_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.55f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.BERRY_PIE);
	}

	// Token: 0x0600096E RID: 2414 RVA: 0x0003EE84 File Offset: 0x0003D084
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600096F RID: 2415 RVA: 0x0003EE86 File Offset: 0x0003D086
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040006FC RID: 1788
	public const string ID = "BerryPie";

	// Token: 0x040006FD RID: 1789
	public static ComplexRecipe recipe;
}
