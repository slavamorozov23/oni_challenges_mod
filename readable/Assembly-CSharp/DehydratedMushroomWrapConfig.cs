using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001F2 RID: 498
public class DehydratedMushroomWrapConfig : IEntityConfig
{
	// Token: 0x06000A12 RID: 2578 RVA: 0x0003FFD4 File Offset: 0x0003E1D4
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000A13 RID: 2579 RVA: 0x0003FFD6 File Offset: 0x0003E1D6
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x06000A14 RID: 2580 RVA: 0x0003FFD8 File Offset: 0x0003E1D8
	public GameObject CreatePrefab()
	{
		KAnimFile anim = Assets.GetAnim("dehydrated_food_mushroom_wrap_kanim");
		GameObject gameObject = EntityTemplates.CreateLooseEntity(DehydratedMushroomWrapConfig.ID.Name, STRINGS.ITEMS.FOOD.MUSHROOMWRAP.DEHYDRATED.NAME, STRINGS.ITEMS.FOOD.MUSHROOMWRAP.DEHYDRATED.DESC, 1f, true, anim, "idle", Grid.SceneLayer.BuildingFront, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, 0, SimHashes.Polypropylene, null);
		EntityTemplates.ExtendEntityToDehydratedFoodPackage(gameObject, FOOD.FOOD_TYPES.MUSHROOM_WRAP);
		return gameObject;
	}

	// Token: 0x04000741 RID: 1857
	public static Tag ID = new Tag("DehydratedMushroomWrap");

	// Token: 0x04000742 RID: 1858
	public const float MASS = 1f;

	// Token: 0x04000743 RID: 1859
	public const string ANIM_FILE = "dehydrated_food_mushroom_wrap_kanim";

	// Token: 0x04000744 RID: 1860
	public const string INITIAL_ANIM = "idle";
}
