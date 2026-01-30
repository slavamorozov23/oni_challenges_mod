using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001BE RID: 446
public class SwampForagePlantConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000903 RID: 2307 RVA: 0x0003CDFC File Offset: 0x0003AFFC
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000904 RID: 2308 RVA: 0x0003CE03 File Offset: 0x0003B003
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000905 RID: 2309 RVA: 0x0003CE08 File Offset: 0x0003B008
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("SwampForagePlant", STRINGS.ITEMS.FOOD.SWAMPFORAGEPLANT.NAME, STRINGS.ITEMS.FOOD.SWAMPFORAGEPLANT.DESC, 1f, false, Assets.GetAnim("swamptuber_vegetable_kanim"), "object", Grid.SceneLayer.BuildingBack, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.SWAMPFORAGEPLANT);
	}

	// Token: 0x06000906 RID: 2310 RVA: 0x0003CE6C File Offset: 0x0003B06C
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000907 RID: 2311 RVA: 0x0003CE6E File Offset: 0x0003B06E
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040006BF RID: 1727
	public const string ID = "SwampForagePlant";
}
