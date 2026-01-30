using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000207 RID: 519
public class DehydratedSpicyTofuConfig : IEntityConfig
{
	// Token: 0x06000A7A RID: 2682 RVA: 0x00040BB4 File Offset: 0x0003EDB4
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000A7B RID: 2683 RVA: 0x00040BB6 File Offset: 0x0003EDB6
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x06000A7C RID: 2684 RVA: 0x00040BB8 File Offset: 0x0003EDB8
	public GameObject CreatePrefab()
	{
		KAnimFile anim = Assets.GetAnim("dehydrated_food_spicy_tofu_kanim");
		GameObject gameObject = EntityTemplates.CreateLooseEntity(DehydratedSpicyTofuConfig.ID.Name, STRINGS.ITEMS.FOOD.SPICYTOFU.DEHYDRATED.NAME, STRINGS.ITEMS.FOOD.SPICYTOFU.DEHYDRATED.DESC, 1f, true, anim, "idle", Grid.SceneLayer.BuildingFront, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, 0, SimHashes.Polypropylene, null);
		EntityTemplates.ExtendEntityToDehydratedFoodPackage(gameObject, FOOD.FOOD_TYPES.SPICY_TOFU);
		return gameObject;
	}

	// Token: 0x04000770 RID: 1904
	public static Tag ID = new Tag("DehydratedSpicyTofu");

	// Token: 0x04000771 RID: 1905
	public const float MASS = 1f;

	// Token: 0x04000772 RID: 1906
	public const string ANIM_FILE = "dehydrated_food_spicy_tofu_kanim";

	// Token: 0x04000773 RID: 1907
	public const string INITIAL_ANIM = "idle";
}
