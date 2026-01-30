using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001DF RID: 479
public class DeepFriedMeatConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060009B4 RID: 2484 RVA: 0x0003F5E0 File Offset: 0x0003D7E0
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x060009B5 RID: 2485 RVA: 0x0003F5E7 File Offset: 0x0003D7E7
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060009B6 RID: 2486 RVA: 0x0003F5EC File Offset: 0x0003D7EC
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("DeepFriedMeat", STRINGS.ITEMS.FOOD.DEEPFRIEDMEAT.NAME, STRINGS.ITEMS.FOOD.DEEPFRIEDMEAT.DESC, 1f, false, Assets.GetAnim("deepfried_meat_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.DEEP_FRIED_MEAT);
	}

	// Token: 0x060009B7 RID: 2487 RVA: 0x0003F650 File Offset: 0x0003D850
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060009B8 RID: 2488 RVA: 0x0003F652 File Offset: 0x0003D852
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400071E RID: 1822
	public const string ID = "DeepFriedMeat";

	// Token: 0x0400071F RID: 1823
	public static ComplexRecipe recipe;
}
