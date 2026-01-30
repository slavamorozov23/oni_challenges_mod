using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003AB RID: 939
public class PropFacilityCouchConfig : IEntityConfig
{
	// Token: 0x06001362 RID: 4962 RVA: 0x0006FDF8 File Offset: 0x0006DFF8
	public GameObject CreatePrefab()
	{
		string id = "PropFacilityCouch";
		string name = STRINGS.BUILDINGS.PREFABS.PROPFACILITYCOUCH.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPFACILITYCOUCH.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_couch_kanim"), "off", Grid.SceneLayer.Building, 4, 2, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		gameObject.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		return gameObject;
	}

	// Token: 0x06001363 RID: 4963 RVA: 0x0006FEA4 File Offset: 0x0006E0A4
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001364 RID: 4964 RVA: 0x0006FEA8 File Offset: 0x0006E0A8
	public void OnSpawn(GameObject inst)
	{
		OccupyArea component = inst.GetComponent<OccupyArea>();
		int cell = Grid.PosToCell(inst);
		foreach (CellOffset offset in component.OccupiedCellsOffsets)
		{
			Grid.GravitasFacility[Grid.OffsetCell(cell, offset)] = true;
		}
	}
}
