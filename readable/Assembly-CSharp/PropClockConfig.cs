using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003A2 RID: 930
public class PropClockConfig : IEntityConfig
{
	// Token: 0x06001338 RID: 4920 RVA: 0x0006F598 File Offset: 0x0006D798
	public GameObject CreatePrefab()
	{
		string id = "PropClock";
		string name = STRINGS.BUILDINGS.PREFABS.PROPCLOCK.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPCLOCK.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("clock_poi_kanim"), "off", Grid.SceneLayer.Building, 1, 1, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Unobtanium, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06001339 RID: 4921 RVA: 0x0006F644 File Offset: 0x0006D844
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600133A RID: 4922 RVA: 0x0006F646 File Offset: 0x0006D846
	public void OnSpawn(GameObject inst)
	{
	}
}
