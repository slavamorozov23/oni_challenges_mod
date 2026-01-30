using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000210 RID: 528
public class VineFruitConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000AA5 RID: 2725 RVA: 0x0004102C File Offset: 0x0003F22C
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC4;
	}

	// Token: 0x06000AA6 RID: 2726 RVA: 0x00041033 File Offset: 0x0003F233
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000AA7 RID: 2727 RVA: 0x00041038 File Offset: 0x0003F238
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity(VineFruitConfig.ID, STRINGS.ITEMS.FOOD.VINEFRUIT.NAME, STRINGS.ITEMS.FOOD.VINEFRUIT.DESC, 1f, false, Assets.GetAnim("ova_melon_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.VINEFRUIT);
		return gameObject;
	}

	// Token: 0x06000AA8 RID: 2728 RVA: 0x0004109E File Offset: 0x0003F29E
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000AA9 RID: 2729 RVA: 0x000410A0 File Offset: 0x0003F2A0
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000788 RID: 1928
	public static string ID = "VineFruit";

	// Token: 0x04000789 RID: 1929
	public const float KCalPerUnit = 325000f;
}
