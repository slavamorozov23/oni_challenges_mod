using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003B5 RID: 949
public class PropFacilityWallDegreeConfig : IEntityConfig
{
	// Token: 0x0600138A RID: 5002 RVA: 0x0007085C File Offset: 0x0006EA5C
	public GameObject CreatePrefab()
	{
		string id = "PropFacilityWallDegree";
		string name = STRINGS.BUILDINGS.PREFABS.PROPFACILITYWALLDEGREE.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPFACILITYWALLDEGREE.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_degree_kanim"), "off", Grid.SceneLayer.Building, 2, 2, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
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

	// Token: 0x0600138B RID: 5003 RVA: 0x00070908 File Offset: 0x0006EB08
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600138C RID: 5004 RVA: 0x0007090C File Offset: 0x0006EB0C
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
