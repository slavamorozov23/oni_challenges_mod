using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001F4 RID: 500
public class PemmicanConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000A1B RID: 2587 RVA: 0x000400D0 File Offset: 0x0003E2D0
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x06000A1C RID: 2588 RVA: 0x000400D7 File Offset: 0x0003E2D7
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000A1D RID: 2589 RVA: 0x000400DC File Offset: 0x0003E2DC
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("Pemmican", STRINGS.ITEMS.FOOD.PEMMICAN.NAME, STRINGS.ITEMS.FOOD.PEMMICAN.DESC, 1f, false, Assets.GetAnim("pemmican_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.PEMMICAN);
	}

	// Token: 0x06000A1E RID: 2590 RVA: 0x00040140 File Offset: 0x0003E340
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000A1F RID: 2591 RVA: 0x00040142 File Offset: 0x0003E342
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000747 RID: 1863
	public const string ID = "Pemmican";

	// Token: 0x04000748 RID: 1864
	public const string ANIM = "pemmican_kanim";

	// Token: 0x04000749 RID: 1865
	public static ComplexRecipe recipe;
}
