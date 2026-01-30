using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001A8 RID: 424
public class IceCavesForagePlantConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000848 RID: 2120 RVA: 0x0003846A File Offset: 0x0003666A
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x06000849 RID: 2121 RVA: 0x00038471 File Offset: 0x00036671
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x0600084A RID: 2122 RVA: 0x00038474 File Offset: 0x00036674
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("IceCavesForagePlant", STRINGS.ITEMS.FOOD.ICECAVESFORAGEPLANT.NAME, STRINGS.ITEMS.FOOD.ICECAVESFORAGEPLANT.DESC, 1f, false, Assets.GetAnim("frozenberries_fruit_kanim"), "object", Grid.SceneLayer.BuildingBack, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.ICECAVESFORAGEPLANT);
	}

	// Token: 0x0600084B RID: 2123 RVA: 0x000384D8 File Offset: 0x000366D8
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600084C RID: 2124 RVA: 0x000384DA File Offset: 0x000366DA
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000642 RID: 1602
	public const string ID = "IceCavesForagePlant";
}
