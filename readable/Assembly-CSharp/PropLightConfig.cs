using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003CE RID: 974
public class PropLightConfig : IEntityConfig
{
	// Token: 0x06001405 RID: 5125 RVA: 0x00071E00 File Offset: 0x00070000
	public GameObject CreatePrefab()
	{
		string id = "PropLight";
		string name = STRINGS.BUILDINGS.PREFABS.PROPLIGHT.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPLIGHT.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("setpiece_light_kanim"), "off", Grid.SceneLayer.Building, 1, 1, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Steel, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06001406 RID: 5126 RVA: 0x00071EAC File Offset: 0x000700AC
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001407 RID: 5127 RVA: 0x00071EAE File Offset: 0x000700AE
	public void OnSpawn(GameObject inst)
	{
	}
}
