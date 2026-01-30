using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001E0 RID: 480
public class DeepFriedNoshConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060009BA RID: 2490 RVA: 0x0003F65C File Offset: 0x0003D85C
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x060009BB RID: 2491 RVA: 0x0003F663 File Offset: 0x0003D863
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060009BC RID: 2492 RVA: 0x0003F668 File Offset: 0x0003D868
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("DeepFriedNosh", STRINGS.ITEMS.FOOD.DEEPFRIEDNOSH.NAME, STRINGS.ITEMS.FOOD.DEEPFRIEDNOSH.DESC, 1f, false, Assets.GetAnim("deepfried_nosh_beans_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.DEEP_FRIED_NOSH);
	}

	// Token: 0x060009BD RID: 2493 RVA: 0x0003F6CC File Offset: 0x0003D8CC
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060009BE RID: 2494 RVA: 0x0003F6CE File Offset: 0x0003D8CE
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000720 RID: 1824
	public const string ID = "DeepFriedNosh";

	// Token: 0x04000721 RID: 1825
	public static ComplexRecipe recipe;
}
