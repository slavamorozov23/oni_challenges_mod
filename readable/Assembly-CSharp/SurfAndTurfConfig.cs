using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000208 RID: 520
public class SurfAndTurfConfig : IEntityConfig
{
	// Token: 0x06000A7F RID: 2687 RVA: 0x00040C40 File Offset: 0x0003EE40
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("SurfAndTurf", STRINGS.ITEMS.FOOD.SURFANDTURF.NAME, STRINGS.ITEMS.FOOD.SURFANDTURF.DESC, 1f, false, Assets.GetAnim("surfnturf_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.SURF_AND_TURF);
	}

	// Token: 0x06000A80 RID: 2688 RVA: 0x00040CA4 File Offset: 0x0003EEA4
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000A81 RID: 2689 RVA: 0x00040CA6 File Offset: 0x0003EEA6
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000774 RID: 1908
	public const string ID = "SurfAndTurf";

	// Token: 0x04000775 RID: 1909
	public static ComplexRecipe recipe;
}
