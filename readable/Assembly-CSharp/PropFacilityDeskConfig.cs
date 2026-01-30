using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003AC RID: 940
public class PropFacilityDeskConfig : IEntityConfig
{
	// Token: 0x06001366 RID: 4966 RVA: 0x0006FEF4 File Offset: 0x0006E0F4
	public GameObject CreatePrefab()
	{
		string id = "PropFacilityDesk";
		string name = STRINGS.BUILDINGS.PREFABS.PROPFACILITYDESK.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPFACILITYDESK.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_desk_kanim"), "off", Grid.SceneLayer.Building, 4, 2, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		LoreBearerUtil.AddLoreTo(gameObject, LoreBearerUtil.UnlockSpecificEntry("journal_magazine", UI.USERMENUACTIONS.READLORE.SEARCH_STERNSDESK, false));
		gameObject.AddOrGet<Demolishable>();
		gameObject.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		return gameObject;
	}

	// Token: 0x06001367 RID: 4967 RVA: 0x0006FFBB File Offset: 0x0006E1BB
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001368 RID: 4968 RVA: 0x0006FFC0 File Offset: 0x0006E1C0
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
