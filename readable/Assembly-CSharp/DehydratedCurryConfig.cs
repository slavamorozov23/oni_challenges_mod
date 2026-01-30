using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001DD RID: 477
public class DehydratedCurryConfig : IEntityConfig
{
	// Token: 0x060009A9 RID: 2473 RVA: 0x0003F4DC File Offset: 0x0003D6DC
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060009AA RID: 2474 RVA: 0x0003F4DE File Offset: 0x0003D6DE
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x060009AB RID: 2475 RVA: 0x0003F4E0 File Offset: 0x0003D6E0
	public GameObject CreatePrefab()
	{
		KAnimFile anim = Assets.GetAnim("dehydrated_food_curry_kanim");
		GameObject gameObject = EntityTemplates.CreateLooseEntity(DehydratedCurryConfig.ID.Name, STRINGS.ITEMS.FOOD.CURRY.DEHYDRATED.NAME, STRINGS.ITEMS.FOOD.CURRY.DEHYDRATED.DESC, 1f, true, anim, "idle", Grid.SceneLayer.BuildingFront, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, 0, SimHashes.Polypropylene, null);
		EntityTemplates.ExtendEntityToDehydratedFoodPackage(gameObject, FOOD.FOOD_TYPES.CURRY);
		return gameObject;
	}

	// Token: 0x04000718 RID: 1816
	public static Tag ID = new Tag("DehydratedCurry");

	// Token: 0x04000719 RID: 1817
	public const float MASS = 1f;

	// Token: 0x0400071A RID: 1818
	public const string ANIM_FILE = "dehydrated_food_curry_kanim";

	// Token: 0x0400071B RID: 1819
	public const string INITIAL_ANIM = "idle";
}
