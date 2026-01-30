using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200020A RID: 522
public class SwampDelightsConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000A88 RID: 2696 RVA: 0x00040D3A File Offset: 0x0003EF3A
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000A89 RID: 2697 RVA: 0x00040D41 File Offset: 0x0003EF41
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000A8A RID: 2698 RVA: 0x00040D44 File Offset: 0x0003EF44
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("SwampDelights", STRINGS.ITEMS.FOOD.SWAMPDELIGHTS.NAME, STRINGS.ITEMS.FOOD.SWAMPDELIGHTS.DESC, 1f, false, Assets.GetAnim("swamp_delights_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.7f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.SWAMP_DELIGHTS);
	}

	// Token: 0x06000A8B RID: 2699 RVA: 0x00040DA8 File Offset: 0x0003EFA8
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000A8C RID: 2700 RVA: 0x00040DAA File Offset: 0x0003EFAA
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400077B RID: 1915
	public const string ID = "SwampDelights";
}
