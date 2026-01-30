using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001FF RID: 511
public class ShellfishMeatConfig : IEntityConfig
{
	// Token: 0x06000A52 RID: 2642 RVA: 0x000407A8 File Offset: 0x0003E9A8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("ShellfishMeat", STRINGS.ITEMS.FOOD.SHELLFISHMEAT.NAME, STRINGS.ITEMS.FOOD.SHELLFISHMEAT.DESC, 1f, false, Assets.GetAnim("shellfish_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.SHELLFISH_MEAT);
		return gameObject;
	}

	// Token: 0x06000A53 RID: 2643 RVA: 0x0004080E File Offset: 0x0003EA0E
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000A54 RID: 2644 RVA: 0x00040810 File Offset: 0x0003EA10
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400075F RID: 1887
	public const string ID = "ShellfishMeat";
}
