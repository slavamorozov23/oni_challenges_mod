using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003CF RID: 975
public class PropReceptionDeskConfig : IEntityConfig
{
	// Token: 0x06001409 RID: 5129 RVA: 0x00071EB8 File Offset: 0x000700B8
	public GameObject CreatePrefab()
	{
		string id = "PropReceptionDesk";
		string name = STRINGS.BUILDINGS.PREFABS.PROPRECEPTIONDESK.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPRECEPTIONDESK.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_reception_kanim"), "off", Grid.SceneLayer.Building, 5, 3, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Steel, true);
		component.Temperature = 294.15f;
		LoreBearerUtil.AddLoreTo(gameObject, LoreBearerUtil.UnlockSpecificEntry("email_pens", UI.USERMENUACTIONS.READLORE.SEARCH_ELLIESDESK, false));
		gameObject.AddOrGet<Demolishable>();
		gameObject.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		return gameObject;
	}

	// Token: 0x0600140A RID: 5130 RVA: 0x00071F7F File Offset: 0x0007017F
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600140B RID: 5131 RVA: 0x00071F84 File Offset: 0x00070184
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
