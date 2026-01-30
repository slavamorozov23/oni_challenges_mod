using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001FB RID: 507
public class RawEggConfig : IEntityConfig
{
	// Token: 0x06000A40 RID: 2624 RVA: 0x0004054C File Offset: 0x0003E74C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("RawEgg", STRINGS.ITEMS.FOOD.RAWEGG.NAME, STRINGS.ITEMS.FOOD.RAWEGG.DESC, 1f, false, Assets.GetAnim("rawegg_kanim"), "object", Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.RAWEGG);
		TemperatureCookable temperatureCookable = gameObject.AddOrGet<TemperatureCookable>();
		temperatureCookable.cookTemperature = 344.15f;
		temperatureCookable.cookedID = "CookedEgg";
		return gameObject;
	}

	// Token: 0x06000A41 RID: 2625 RVA: 0x000405CD File Offset: 0x0003E7CD
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000A42 RID: 2626 RVA: 0x000405CF File Offset: 0x0003E7CF
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000757 RID: 1879
	public const string ID = "RawEgg";
}
