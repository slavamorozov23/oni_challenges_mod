using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000257 RID: 599
public class GravitasToiletPaperHolderConfig : IEntityConfig
{
	// Token: 0x06000C25 RID: 3109 RVA: 0x000491CC File Offset: 0x000473CC
	public GameObject CreatePrefab()
	{
		string id = "GravitasToiletPaperHolder";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASBATHROOMTOILETPAPERHOLDER.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASBATHROOMTOILETPAPERHOLDER.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_toilet_paper_holder_kanim"), "on", Grid.SceneLayer.Building, 1, 1, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06000C26 RID: 3110 RVA: 0x0004925F File Offset: 0x0004745F
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06000C27 RID: 3111 RVA: 0x00049276 File Offset: 0x00047476
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400085E RID: 2142
	public const string ID = "GravitasToiletPaperHolder";
}
