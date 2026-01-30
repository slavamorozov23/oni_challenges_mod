using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003BD RID: 957
public class PropGravitasFireExtinguisherConfig : IEntityConfig
{
	// Token: 0x060013B5 RID: 5045 RVA: 0x00070EF4 File Offset: 0x0006F0F4
	public GameObject CreatePrefab()
	{
		string id = "PropGravitasFireExtinguisher";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASFIREEXTINGUISHER.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASFIREEXTINGUISHER.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("poi_fireextinguisher_kanim"), "off", Grid.SceneLayer.Building, 1, 2, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x060013B6 RID: 5046 RVA: 0x00070F87 File Offset: 0x0006F187
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x060013B7 RID: 5047 RVA: 0x00070F9E File Offset: 0x0006F19E
	public void OnSpawn(GameObject inst)
	{
	}
}
