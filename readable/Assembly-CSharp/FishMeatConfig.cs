using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001E5 RID: 485
public class FishMeatConfig : IEntityConfig
{
	// Token: 0x060009D7 RID: 2519 RVA: 0x0003F8C8 File Offset: 0x0003DAC8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("FishMeat", STRINGS.ITEMS.FOOD.FISHMEAT.NAME, STRINGS.ITEMS.FOOD.FISHMEAT.DESC, 1f, false, Assets.GetAnim("pacufillet_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.FISH_MEAT);
		return gameObject;
	}

	// Token: 0x060009D8 RID: 2520 RVA: 0x0003F92E File Offset: 0x0003DB2E
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060009D9 RID: 2521 RVA: 0x0003F930 File Offset: 0x0003DB30
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000727 RID: 1831
	public const string ID = "FishMeat";
}
