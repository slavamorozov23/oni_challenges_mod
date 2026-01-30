using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003C0 RID: 960
public class PropGravitasHandScannerConfig : IEntityConfig
{
	// Token: 0x060013C2 RID: 5058 RVA: 0x000711B8 File Offset: 0x0006F3B8
	public GameObject CreatePrefab()
	{
		string id = "PropGravitasHandScanner";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASHANDSCANNER.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASHANDSCANNER.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_hand_scanner_kanim"), "off", Grid.SceneLayer.Building, 1, 1, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x060013C3 RID: 5059 RVA: 0x0007124B File Offset: 0x0006F44B
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x060013C4 RID: 5060 RVA: 0x00071262 File Offset: 0x0006F462
	public void OnSpawn(GameObject inst)
	{
	}
}
