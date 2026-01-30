using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003A7 RID: 935
public class PropElevatorConfig : IEntityConfig
{
	// Token: 0x06001352 RID: 4946 RVA: 0x0006FA08 File Offset: 0x0006DC08
	public GameObject CreatePrefab()
	{
		string id = "PropElevator";
		string name = STRINGS.BUILDINGS.PREFABS.PROPELEVATOR.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPELEVATOR.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_elevator_kanim"), "off", Grid.SceneLayer.Building, 2, 3, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
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

	// Token: 0x06001353 RID: 4947 RVA: 0x0006FAB4 File Offset: 0x0006DCB4
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001354 RID: 4948 RVA: 0x0006FAB8 File Offset: 0x0006DCB8
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
