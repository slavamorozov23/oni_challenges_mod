using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003AF RID: 943
public class PropFacilityDisplay : IEntityConfig
{
	// Token: 0x06001372 RID: 4978 RVA: 0x0007023C File Offset: 0x0006E43C
	public GameObject CreatePrefab()
	{
		string id = "PropFacilityDisplay";
		string name = STRINGS.BUILDINGS.PREFABS.PROPFACILITYDISPLAY1.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPFACILITYDISPLAY1.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_display1_kanim"), "off", Grid.SceneLayer.Building, 2, 3, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Steel, true);
		component.Temperature = 294.15f;
		LoreBearerUtil.AddLoreTo(gameObject, LoreBearerUtil.UnlockSpecificEntry("display_prop1", UI.USERMENUACTIONS.READLORE.SEARCH_DISPLAY, false));
		gameObject.AddOrGet<Demolishable>();
		gameObject.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		return gameObject;
	}

	// Token: 0x06001373 RID: 4979 RVA: 0x00070303 File Offset: 0x0006E503
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001374 RID: 4980 RVA: 0x00070308 File Offset: 0x0006E508
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
