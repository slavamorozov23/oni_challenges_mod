using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000205 RID: 517
public class SpiceNutConfig : IEntityConfig
{
	// Token: 0x06000A71 RID: 2673 RVA: 0x00040A88 File Offset: 0x0003EC88
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity(SpiceNutConfig.ID, STRINGS.ITEMS.FOOD.SPICENUT.NAME, STRINGS.ITEMS.FOOD.SPICENUT.DESC, 1f, false, Assets.GetAnim("spicenut_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.SPICENUT);
		SoundEventVolumeCache.instance.AddVolume("vinespicenut_kanim", "VineSpiceNut_grow", NOISE_POLLUTION.CREATURES.TIER3);
		SoundEventVolumeCache.instance.AddVolume("vinespicenut_kanim", "VineSpiceNut_harvest", NOISE_POLLUTION.CREATURES.TIER3);
		return gameObject;
	}

	// Token: 0x06000A72 RID: 2674 RVA: 0x00040B20 File Offset: 0x0003ED20
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000A73 RID: 2675 RVA: 0x00040B22 File Offset: 0x0003ED22
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400076C RID: 1900
	public static float SEEDS_PER_FRUIT = 1f;

	// Token: 0x0400076D RID: 1901
	public static string ID = "SpiceNut";
}
