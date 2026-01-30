using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001F1 RID: 497
public class MushroomWrapConfig : IEntityConfig
{
	// Token: 0x06000A0E RID: 2574 RVA: 0x0003FF64 File Offset: 0x0003E164
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("MushroomWrap", STRINGS.ITEMS.FOOD.MUSHROOMWRAP.NAME, STRINGS.ITEMS.FOOD.MUSHROOMWRAP.DESC, 1f, false, Assets.GetAnim("mushroom_wrap_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.5f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.MUSHROOM_WRAP);
	}

	// Token: 0x06000A0F RID: 2575 RVA: 0x0003FFC8 File Offset: 0x0003E1C8
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000A10 RID: 2576 RVA: 0x0003FFCA File Offset: 0x0003E1CA
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400073F RID: 1855
	public const string ID = "MushroomWrap";

	// Token: 0x04000740 RID: 1856
	public static ComplexRecipe recipe;
}
