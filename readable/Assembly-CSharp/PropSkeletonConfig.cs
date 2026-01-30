using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003D0 RID: 976
public class PropSkeletonConfig : IEntityConfig
{
	// Token: 0x0600140D RID: 5133 RVA: 0x00071FD0 File Offset: 0x000701D0
	public GameObject CreatePrefab()
	{
		string id = "PropSkeleton";
		string name = STRINGS.BUILDINGS.PREFABS.PROPSKELETON.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPSKELETON.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER5;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("skeleton_poi_kanim"), "off", Grid.SceneLayer.Building, 1, 2, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Creature, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x0600140E RID: 5134 RVA: 0x00072063 File Offset: 0x00070263
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x0600140F RID: 5135 RVA: 0x0007207A File Offset: 0x0007027A
	public void OnSpawn(GameObject inst)
	{
	}
}
