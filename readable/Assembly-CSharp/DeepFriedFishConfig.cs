using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001DE RID: 478
public class DeepFriedFishConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060009AE RID: 2478 RVA: 0x0003F566 File Offset: 0x0003D766
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x060009AF RID: 2479 RVA: 0x0003F56D File Offset: 0x0003D76D
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060009B0 RID: 2480 RVA: 0x0003F570 File Offset: 0x0003D770
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("DeepFriedFish", STRINGS.ITEMS.FOOD.DEEPFRIEDFISH.NAME, STRINGS.ITEMS.FOOD.DEEPFRIEDFISH.DESC, 1f, false, Assets.GetAnim("deepfried_fish_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.DEEP_FRIED_FISH);
	}

	// Token: 0x060009B1 RID: 2481 RVA: 0x0003F5D4 File Offset: 0x0003D7D4
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060009B2 RID: 2482 RVA: 0x0003F5D6 File Offset: 0x0003D7D6
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400071C RID: 1820
	public const string ID = "DeepFriedFish";

	// Token: 0x0400071D RID: 1821
	public static ComplexRecipe recipe;
}
