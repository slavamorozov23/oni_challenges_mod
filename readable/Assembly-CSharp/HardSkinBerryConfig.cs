using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001A6 RID: 422
public class HardSkinBerryConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x0600083C RID: 2108 RVA: 0x000381EC File Offset: 0x000363EC
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x0600083D RID: 2109 RVA: 0x000381F3 File Offset: 0x000363F3
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x0600083E RID: 2110 RVA: 0x000381F8 File Offset: 0x000363F8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("HardSkinBerry", STRINGS.ITEMS.FOOD.HARDSKINBERRY.NAME, STRINGS.ITEMS.FOOD.HARDSKINBERRY.DESC, 1f, false, Assets.GetAnim("iceBerry_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.CIRCLE, 0.25f, 0.25f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.HARDSKINBERRY);
		return gameObject;
	}

	// Token: 0x0600083F RID: 2111 RVA: 0x0003825E File Offset: 0x0003645E
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000840 RID: 2112 RVA: 0x00038260 File Offset: 0x00036460
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000639 RID: 1593
	public const string ID = "HardSkinBerry";
}
