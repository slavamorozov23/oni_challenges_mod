using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003A9 RID: 937
public class PropFacilityChairFlipConfig : IEntityConfig
{
	// Token: 0x0600135A RID: 4954 RVA: 0x0006FC00 File Offset: 0x0006DE00
	public GameObject CreatePrefab()
	{
		string id = "PropFacilityChairFlip";
		string name = STRINGS.BUILDINGS.PREFABS.PROPFACILITYCHAIR.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPFACILITYCHAIR.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_chairFlip_kanim"), "off", Grid.SceneLayer.Building, 2, 2, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
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

	// Token: 0x0600135B RID: 4955 RVA: 0x0006FCAC File Offset: 0x0006DEAC
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600135C RID: 4956 RVA: 0x0006FCB0 File Offset: 0x0006DEB0
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
