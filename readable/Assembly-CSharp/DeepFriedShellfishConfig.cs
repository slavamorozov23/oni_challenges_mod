using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001E1 RID: 481
public class DeepFriedShellfishConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060009C0 RID: 2496 RVA: 0x0003F6D8 File Offset: 0x0003D8D8
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x060009C1 RID: 2497 RVA: 0x0003F6DF File Offset: 0x0003D8DF
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060009C2 RID: 2498 RVA: 0x0003F6E4 File Offset: 0x0003D8E4
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("DeepFriedShellfish", STRINGS.ITEMS.FOOD.DEEPFRIEDSHELLFISH.NAME, STRINGS.ITEMS.FOOD.DEEPFRIEDSHELLFISH.DESC, 1f, false, Assets.GetAnim("deepfried_shellfish_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.DEEP_FRIED_SHELLFISH);
	}

	// Token: 0x060009C3 RID: 2499 RVA: 0x0003F748 File Offset: 0x0003D948
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060009C4 RID: 2500 RVA: 0x0003F74A File Offset: 0x0003D94A
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000722 RID: 1826
	public const string ID = "DeepFriedShellfish";

	// Token: 0x04000723 RID: 1827
	public static ComplexRecipe recipe;
}
