using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001FA RID: 506
public class DehydratedQuicheConfig : IEntityConfig
{
	// Token: 0x06000A3B RID: 2619 RVA: 0x000404C0 File Offset: 0x0003E6C0
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000A3C RID: 2620 RVA: 0x000404C2 File Offset: 0x0003E6C2
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x06000A3D RID: 2621 RVA: 0x000404C4 File Offset: 0x0003E6C4
	public GameObject CreatePrefab()
	{
		KAnimFile anim = Assets.GetAnim("dehydrated_food_quiche_kanim");
		GameObject gameObject = EntityTemplates.CreateLooseEntity(DehydratedQuicheConfig.ID.Name, STRINGS.ITEMS.FOOD.QUICHE.DEHYDRATED.NAME, STRINGS.ITEMS.FOOD.QUICHE.DEHYDRATED.DESC, 1f, true, anim, "idle", Grid.SceneLayer.BuildingFront, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, 0, SimHashes.Polypropylene, null);
		EntityTemplates.ExtendEntityToDehydratedFoodPackage(gameObject, FOOD.FOOD_TYPES.QUICHE);
		return gameObject;
	}

	// Token: 0x04000753 RID: 1875
	public static Tag ID = new Tag("DehydratedQuiche");

	// Token: 0x04000754 RID: 1876
	public const float MASS = 1f;

	// Token: 0x04000755 RID: 1877
	public const string ANIM_FILE = "dehydrated_food_quiche_kanim";

	// Token: 0x04000756 RID: 1878
	public const string INITIAL_ANIM = "idle";
}
