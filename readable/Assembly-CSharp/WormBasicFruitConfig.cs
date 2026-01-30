using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000212 RID: 530
public class WormBasicFruitConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000AB2 RID: 2738 RVA: 0x00041130 File Offset: 0x0003F330
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000AB3 RID: 2739 RVA: 0x00041137 File Offset: 0x0003F337
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000AB4 RID: 2740 RVA: 0x0004113C File Offset: 0x0003F33C
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("WormBasicFruit", STRINGS.ITEMS.FOOD.WORMBASICFRUIT.NAME, STRINGS.ITEMS.FOOD.WORMBASICFRUIT.DESC, 1f, false, Assets.GetAnim("wormwood_basic_fruit_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.7f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.WORMBASICFRUIT);
	}

	// Token: 0x06000AB5 RID: 2741 RVA: 0x000411A0 File Offset: 0x0003F3A0
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000AB6 RID: 2742 RVA: 0x000411A2 File Offset: 0x0003F3A2
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400078C RID: 1932
	public const string ID = "WormBasicFruit";
}
