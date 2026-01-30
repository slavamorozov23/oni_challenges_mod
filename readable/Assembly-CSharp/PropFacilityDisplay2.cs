using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003AD RID: 941
public class PropFacilityDisplay2 : IEntityConfig
{
	// Token: 0x0600136A RID: 4970 RVA: 0x0007000C File Offset: 0x0006E20C
	public GameObject CreatePrefab()
	{
		string id = "PropFacilityDisplay2";
		string name = STRINGS.BUILDINGS.PREFABS.PROPFACILITYDISPLAY2.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPFACILITYDISPLAY2.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_display2_kanim"), "off", Grid.SceneLayer.Building, 2, 3, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Steel, true);
		component.Temperature = 294.15f;
		LoreBearerUtil.AddLoreTo(gameObject, LoreBearerUtil.UnlockSpecificEntry("display_prop2", UI.USERMENUACTIONS.READLORE.SEARCH_DISPLAY, false));
		gameObject.AddOrGet<Demolishable>();
		gameObject.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		return gameObject;
	}

	// Token: 0x0600136B RID: 4971 RVA: 0x000700D3 File Offset: 0x0006E2D3
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600136C RID: 4972 RVA: 0x000700D8 File Offset: 0x0006E2D8
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
