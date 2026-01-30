using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003A8 RID: 936
public class PropFacilityChairConfig : IEntityConfig
{
	// Token: 0x06001356 RID: 4950 RVA: 0x0006FB04 File Offset: 0x0006DD04
	public GameObject CreatePrefab()
	{
		string id = "PropFacilityChair";
		string name = STRINGS.BUILDINGS.PREFABS.PROPFACILITYCHAIR.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPFACILITYCHAIR.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_chair_kanim"), "off", Grid.SceneLayer.Building, 2, 2, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
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

	// Token: 0x06001357 RID: 4951 RVA: 0x0006FBB0 File Offset: 0x0006DDB0
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001358 RID: 4952 RVA: 0x0006FBB4 File Offset: 0x0006DDB4
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
