using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200019F RID: 415
public class GardenFoodPlantFoodConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000816 RID: 2070 RVA: 0x00036DE5 File Offset: 0x00034FE5
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC4;
	}

	// Token: 0x06000817 RID: 2071 RVA: 0x00036DEC File Offset: 0x00034FEC
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000818 RID: 2072 RVA: 0x00036DF0 File Offset: 0x00034FF0
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("GardenFoodPlantFood", STRINGS.ITEMS.FOOD.GARDENFOODPLANTFOOD.NAME, STRINGS.ITEMS.FOOD.GARDENFOODPLANTFOOD.DESC, 1f, false, Assets.GetAnim("spikefruit_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.CIRCLE, 0.25f, 0.25f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.GARDENFOODPLANT);
		return gameObject;
	}

	// Token: 0x06000819 RID: 2073 RVA: 0x00036E56 File Offset: 0x00035056
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600081A RID: 2074 RVA: 0x00036E58 File Offset: 0x00035058
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000613 RID: 1555
	public const string ID = "GardenFoodPlantFood";
}
