using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001A0 RID: 416
public class GardenForagePlantConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x0600081C RID: 2076 RVA: 0x00036E62 File Offset: 0x00035062
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC4;
	}

	// Token: 0x0600081D RID: 2077 RVA: 0x00036E69 File Offset: 0x00035069
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x0600081E RID: 2078 RVA: 0x00036E6C File Offset: 0x0003506C
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("GardenForagePlant", STRINGS.ITEMS.FOOD.GARDENFORAGEPLANT.NAME, STRINGS.ITEMS.FOOD.GARDENFORAGEPLANT.DESC, 1f, false, Assets.GetAnim("fatplantfood_kanim"), "object", Grid.SceneLayer.BuildingBack, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.GARDENFORAGEPLANT);
	}

	// Token: 0x0600081F RID: 2079 RVA: 0x00036ED0 File Offset: 0x000350D0
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000820 RID: 2080 RVA: 0x00036ED2 File Offset: 0x000350D2
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000614 RID: 1556
	public const string ID = "GardenForagePlant";
}
