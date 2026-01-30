using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003B4 RID: 948
public class PropFacilityTableConfig : IEntityConfig
{
	// Token: 0x06001386 RID: 4998 RVA: 0x00070760 File Offset: 0x0006E960
	public GameObject CreatePrefab()
	{
		string id = "PropFacilityTable";
		string name = STRINGS.BUILDINGS.PREFABS.PROPFACILITYTABLE.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPFACILITYTABLE.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_table_kanim"), "off", Grid.SceneLayer.Building, 4, 2, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Steel, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		gameObject.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		return gameObject;
	}

	// Token: 0x06001387 RID: 4999 RVA: 0x0007080C File Offset: 0x0006EA0C
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001388 RID: 5000 RVA: 0x00070810 File Offset: 0x0006EA10
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
