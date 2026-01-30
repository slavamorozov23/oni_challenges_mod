using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000214 RID: 532
public class WormSuperFruitConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000ABE RID: 2750 RVA: 0x00041228 File Offset: 0x0003F428
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000ABF RID: 2751 RVA: 0x0004122F File Offset: 0x0003F42F
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000AC0 RID: 2752 RVA: 0x00041234 File Offset: 0x0003F434
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("WormSuperFruit", STRINGS.ITEMS.FOOD.WORMSUPERFRUIT.NAME, STRINGS.ITEMS.FOOD.WORMSUPERFRUIT.DESC, 1f, false, Assets.GetAnim("wormwood_super_fruits_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.WORMSUPERFRUIT);
	}

	// Token: 0x06000AC1 RID: 2753 RVA: 0x00041298 File Offset: 0x0003F498
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000AC2 RID: 2754 RVA: 0x0004129A File Offset: 0x0003F49A
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400078F RID: 1935
	public const string ID = "WormSuperFruit";
}
