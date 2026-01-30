using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000206 RID: 518
public class SpicyTofuConfig : IEntityConfig
{
	// Token: 0x06000A76 RID: 2678 RVA: 0x00040B44 File Offset: 0x0003ED44
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("SpicyTofu", STRINGS.ITEMS.FOOD.SPICYTOFU.NAME, STRINGS.ITEMS.FOOD.SPICYTOFU.DESC, 1f, false, Assets.GetAnim("spicey_tofu_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.9f, 0.6f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.SPICY_TOFU);
	}

	// Token: 0x06000A77 RID: 2679 RVA: 0x00040BA8 File Offset: 0x0003EDA8
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000A78 RID: 2680 RVA: 0x00040BAA File Offset: 0x0003EDAA
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400076E RID: 1902
	public const string ID = "SpicyTofu";

	// Token: 0x0400076F RID: 1903
	public static ComplexRecipe recipe;
}
