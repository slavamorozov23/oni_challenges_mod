using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003B1 RID: 945
public class PropFacilityHangingLightConfig : IEntityConfig
{
	// Token: 0x0600137A RID: 4986 RVA: 0x0007046C File Offset: 0x0006E66C
	public GameObject CreatePrefab()
	{
		string id = "PropFacilityHangingLight";
		string name = STRINGS.BUILDINGS.PREFABS.PROPFACILITYLAMP.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPFACILITYLAMP.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_light_kanim"), "off", Grid.SceneLayer.Building, 1, 4, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
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

	// Token: 0x0600137B RID: 4987 RVA: 0x00070518 File Offset: 0x0006E718
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600137C RID: 4988 RVA: 0x0007051C File Offset: 0x0006E71C
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
