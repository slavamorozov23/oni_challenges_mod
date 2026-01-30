using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000209 RID: 521
public class DehydratedSurfAndTurfConfig : IEntityConfig
{
	// Token: 0x06000A83 RID: 2691 RVA: 0x00040CB0 File Offset: 0x0003EEB0
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000A84 RID: 2692 RVA: 0x00040CB2 File Offset: 0x0003EEB2
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x06000A85 RID: 2693 RVA: 0x00040CB4 File Offset: 0x0003EEB4
	public GameObject CreatePrefab()
	{
		KAnimFile anim = Assets.GetAnim("dehydrated_food_surf_and_turf_kanim");
		GameObject gameObject = EntityTemplates.CreateLooseEntity(DehydratedSurfAndTurfConfig.ID.Name, STRINGS.ITEMS.FOOD.SURFANDTURF.DEHYDRATED.NAME, STRINGS.ITEMS.FOOD.SURFANDTURF.DEHYDRATED.DESC, 1f, true, anim, "idle", Grid.SceneLayer.BuildingFront, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, 0, SimHashes.Polypropylene, null);
		EntityTemplates.ExtendEntityToDehydratedFoodPackage(gameObject, FOOD.FOOD_TYPES.SURF_AND_TURF);
		return gameObject;
	}

	// Token: 0x04000776 RID: 1910
	public static Tag ID = new Tag("DehydratedSurfAndTurf");

	// Token: 0x04000777 RID: 1911
	public const float MASS = 1f;

	// Token: 0x04000778 RID: 1912
	public const int FABRICATION_TIME_SECONDS = 300;

	// Token: 0x04000779 RID: 1913
	public const string ANIM_FILE = "dehydrated_food_surf_and_turf_kanim";

	// Token: 0x0400077A RID: 1914
	public const string INITIAL_ANIM = "idle";
}
