using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000252 RID: 594
public class GravitasPaperTowelDispenserConfig : IEntityConfig
{
	// Token: 0x06000C10 RID: 3088 RVA: 0x00048D9C File Offset: 0x00046F9C
	public GameObject CreatePrefab()
	{
		string id = "GravitasPaperTowelDispenser";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASPAPERTOLELDISPENSER.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASPAPERTOLELDISPENSER.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_paper_towell_dispenser_kanim"), "on", Grid.SceneLayer.Building, 1, 1, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06000C11 RID: 3089 RVA: 0x00048E2F File Offset: 0x0004702F
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06000C12 RID: 3090 RVA: 0x00048E46 File Offset: 0x00047046
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400085C RID: 2140
	public const string ID = "GravitasPaperTowelDispenser";
}
