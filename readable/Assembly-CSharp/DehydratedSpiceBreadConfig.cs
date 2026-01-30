using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000204 RID: 516
public class DehydratedSpiceBreadConfig : IEntityConfig
{
	// Token: 0x06000A6C RID: 2668 RVA: 0x000409FC File Offset: 0x0003EBFC
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000A6D RID: 2669 RVA: 0x000409FE File Offset: 0x0003EBFE
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x06000A6E RID: 2670 RVA: 0x00040A00 File Offset: 0x0003EC00
	public GameObject CreatePrefab()
	{
		KAnimFile anim = Assets.GetAnim("dehydrated_food_spicebread_kanim");
		GameObject gameObject = EntityTemplates.CreateLooseEntity(DehydratedSpiceBreadConfig.ID.Name, STRINGS.ITEMS.FOOD.SPICEBREAD.DEHYDRATED.NAME, STRINGS.ITEMS.FOOD.SPICEBREAD.DEHYDRATED.DESC, 1f, true, anim, "idle", Grid.SceneLayer.BuildingFront, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, 0, SimHashes.Polypropylene, null);
		EntityTemplates.ExtendEntityToDehydratedFoodPackage(gameObject, FOOD.FOOD_TYPES.SPICEBREAD);
		return gameObject;
	}

	// Token: 0x04000768 RID: 1896
	public static Tag ID = new Tag("DehydratedSpiceBread");

	// Token: 0x04000769 RID: 1897
	public const float MASS = 1f;

	// Token: 0x0400076A RID: 1898
	public const string ANIM_FILE = "dehydrated_food_spicebread_kanim";

	// Token: 0x0400076B RID: 1899
	public const string INITIAL_ANIM = "idle";
}
