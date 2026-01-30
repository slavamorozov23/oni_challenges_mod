using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000246 RID: 582
public class GravitasBathroomSinkConfig : IEntityConfig
{
	// Token: 0x06000BCB RID: 3019 RVA: 0x00047EC8 File Offset: 0x000460C8
	public GameObject CreatePrefab()
	{
		string id = "GravitasBathroomSink";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASBATHROOMSINK.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASBATHROOMSINK.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_bathroom_sink_kanim"), "on", Grid.SceneLayer.Building, 1, 1, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06000BCC RID: 3020 RVA: 0x00047F5B File Offset: 0x0004615B
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06000BCD RID: 3021 RVA: 0x00047F72 File Offset: 0x00046172
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400084A RID: 2122
	public const string ID = "GravitasBathroomSink";
}
