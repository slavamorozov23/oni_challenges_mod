using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000201 RID: 513
public class SmokedFish : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000A5C RID: 2652 RVA: 0x00040894 File Offset: 0x0003EA94
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC4;
	}

	// Token: 0x06000A5D RID: 2653 RVA: 0x0004089B File Offset: 0x0003EA9B
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000A5E RID: 2654 RVA: 0x000408A0 File Offset: 0x0003EAA0
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("SmokedFish", STRINGS.ITEMS.FOOD.SMOKEDFISH.NAME, STRINGS.ITEMS.FOOD.SMOKEDFISH.DESC, 1f, false, Assets.GetAnim("smokedfish_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.SMOKED_FISH);
	}

	// Token: 0x06000A5F RID: 2655 RVA: 0x00040904 File Offset: 0x0003EB04
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000A60 RID: 2656 RVA: 0x00040906 File Offset: 0x0003EB06
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000762 RID: 1890
	public const string ID = "SmokedFish";

	// Token: 0x04000763 RID: 1891
	public static ComplexRecipe recipe;
}
