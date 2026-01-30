using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003D5 RID: 981
public class PropTallPlantConfig : IEntityConfig
{
	// Token: 0x06001425 RID: 5157 RVA: 0x000726E4 File Offset: 0x000708E4
	public GameObject CreatePrefab()
	{
		string id = "PropTallPlant";
		string name = STRINGS.BUILDINGS.PREFABS.PROPFACILITYTALLPLANT.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPFACILITYTALLPLANT.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_tall_plant_kanim"), "off", Grid.SceneLayer.Building, 1, 3, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Polypropylene, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06001426 RID: 5158 RVA: 0x00072779 File Offset: 0x00070979
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06001427 RID: 5159 RVA: 0x00072790 File Offset: 0x00070990
	public void OnSpawn(GameObject inst)
	{
	}
}
