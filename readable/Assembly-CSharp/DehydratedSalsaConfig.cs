using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001FE RID: 510
public class DehydratedSalsaConfig : IEntityConfig
{
	// Token: 0x06000A4D RID: 2637 RVA: 0x0004071C File Offset: 0x0003E91C
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000A4E RID: 2638 RVA: 0x0004071E File Offset: 0x0003E91E
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x06000A4F RID: 2639 RVA: 0x00040720 File Offset: 0x0003E920
	public GameObject CreatePrefab()
	{
		KAnimFile anim = Assets.GetAnim("dehydrated_food_salsa_kanim");
		GameObject gameObject = EntityTemplates.CreateLooseEntity(DehydratedSalsaConfig.ID.Name, STRINGS.ITEMS.FOOD.SALSA.DEHYDRATED.NAME, STRINGS.ITEMS.FOOD.SALSA.DEHYDRATED.DESC, 1f, true, anim, "idle", Grid.SceneLayer.BuildingFront, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, 0, SimHashes.Polypropylene, null);
		EntityTemplates.ExtendEntityToDehydratedFoodPackage(gameObject, FOOD.FOOD_TYPES.SALSA);
		return gameObject;
	}

	// Token: 0x0400075B RID: 1883
	public static Tag ID = new Tag("DehydratedSalsa");

	// Token: 0x0400075C RID: 1884
	public const float MASS = 1f;

	// Token: 0x0400075D RID: 1885
	public const string ANIM_FILE = "dehydrated_food_salsa_kanim";

	// Token: 0x0400075E RID: 1886
	public const string INITIAL_ANIM = "idle";
}
