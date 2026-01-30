using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001D4 RID: 468
public class DehydratedFoodPackageConfig : IEntityConfig
{
	// Token: 0x0600097C RID: 2428 RVA: 0x0003EF98 File Offset: 0x0003D198
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600097D RID: 2429 RVA: 0x0003EF9A File Offset: 0x0003D19A
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0600097E RID: 2430 RVA: 0x0003EF9C File Offset: 0x0003D19C
	public GameObject CreatePrefab()
	{
		KAnimFile anim = Assets.GetAnim("dehydrated_food_burger_kanim");
		GameObject gameObject = EntityTemplates.CreateLooseEntity(DehydratedFoodPackageConfig.ID.Name, STRINGS.ITEMS.FOOD.BURGER.DEHYDRATED.NAME, STRINGS.ITEMS.FOOD.BURGER.DEHYDRATED.DESC, 1f, true, anim, "idle", Grid.SceneLayer.BuildingFront, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, 0, SimHashes.Polypropylene, null);
		EntityTemplates.ExtendEntityToDehydratedFoodPackage(gameObject, FOOD.FOOD_TYPES.BURGER);
		return gameObject;
	}

	// Token: 0x04000704 RID: 1796
	public static Tag ID = new Tag("DehydratedFoodPackage");

	// Token: 0x04000705 RID: 1797
	public const float MASS = 1f;

	// Token: 0x04000706 RID: 1798
	public const string ANIM_FILE = "dehydrated_food_burger_kanim";

	// Token: 0x04000707 RID: 1799
	public const string INITIAL_ANIM = "idle";
}
