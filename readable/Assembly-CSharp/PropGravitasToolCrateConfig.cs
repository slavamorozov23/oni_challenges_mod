using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003C9 RID: 969
public class PropGravitasToolCrateConfig : IEntityConfig
{
	// Token: 0x060013F1 RID: 5105 RVA: 0x00071974 File Offset: 0x0006FB74
	public GameObject CreatePrefab()
	{
		string id = "PropGravitasToolCrate";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASTOOLCRATE.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASTOOLCRATE.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("poi_1x1_crate_kanim"), "off", Grid.SceneLayer.Building, 1, 1, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x060013F2 RID: 5106 RVA: 0x00071A07 File Offset: 0x0006FC07
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x060013F3 RID: 5107 RVA: 0x00071A1E File Offset: 0x0006FC1E
	public void OnSpawn(GameObject inst)
	{
	}
}
