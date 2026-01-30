using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000256 RID: 598
public class GravitasPotPlantDeadConfig : IEntityConfig
{
	// Token: 0x06000C21 RID: 3105 RVA: 0x00049114 File Offset: 0x00047314
	public GameObject CreatePrefab()
	{
		string id = "GravitasPotPlantDead";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASPOTPLANTDEAD.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASPOTPLANTDEAD.DESC;
		float mass = 25f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_tall_plant_dead_kanim"), "off", Grid.SceneLayer.Building, 1, 2, tier, PermittedRotations.Unrotatable, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Ceramic, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06000C22 RID: 3106 RVA: 0x000491A9 File Offset: 0x000473A9
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06000C23 RID: 3107 RVA: 0x000491C0 File Offset: 0x000473C0
	public void OnSpawn(GameObject inst)
	{
	}
}
