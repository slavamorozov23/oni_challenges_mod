using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200020B RID: 523
public class SwampFruitConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000A8E RID: 2702 RVA: 0x00040DB4 File Offset: 0x0003EFB4
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000A8F RID: 2703 RVA: 0x00040DBB File Offset: 0x0003EFBB
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000A90 RID: 2704 RVA: 0x00040DC0 File Offset: 0x0003EFC0
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity(SwampFruitConfig.ID, STRINGS.ITEMS.FOOD.SWAMPFRUIT.NAME, STRINGS.ITEMS.FOOD.SWAMPFRUIT.DESC, 1f, false, Assets.GetAnim("swampcrop_fruit_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 1f, 0.72f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.SWAMPFRUIT);
	}

	// Token: 0x06000A91 RID: 2705 RVA: 0x00040E24 File Offset: 0x0003F024
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000A92 RID: 2706 RVA: 0x00040E26 File Offset: 0x0003F026
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400077C RID: 1916
	public static string ID = "SwampFruit";
}
