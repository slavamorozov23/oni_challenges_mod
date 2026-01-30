using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000202 RID: 514
public class SmokedVegetablesConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000A62 RID: 2658 RVA: 0x00040910 File Offset: 0x0003EB10
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC4;
	}

	// Token: 0x06000A63 RID: 2659 RVA: 0x00040917 File Offset: 0x0003EB17
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000A64 RID: 2660 RVA: 0x0004091C File Offset: 0x0003EB1C
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("SmokedVegetables", STRINGS.ITEMS.FOOD.SMOKEDVEGETABLES.NAME, STRINGS.ITEMS.FOOD.SMOKEDVEGETABLES.DESC, 1f, false, Assets.GetAnim("smokedvegetables_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.7f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.SMOKED_VEGETABLES);
	}

	// Token: 0x06000A65 RID: 2661 RVA: 0x00040980 File Offset: 0x0003EB80
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000A66 RID: 2662 RVA: 0x00040982 File Offset: 0x0003EB82
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000764 RID: 1892
	public const string ID = "SmokedVegetables";

	// Token: 0x04000765 RID: 1893
	public static ComplexRecipe recipe;
}
