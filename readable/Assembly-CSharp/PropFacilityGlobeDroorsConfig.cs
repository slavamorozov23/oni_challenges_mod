using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003B0 RID: 944
public class PropFacilityGlobeDroorsConfig : IEntityConfig
{
	// Token: 0x06001376 RID: 4982 RVA: 0x00070354 File Offset: 0x0006E554
	public GameObject CreatePrefab()
	{
		string id = "PropFacilityGlobeDroors";
		string name = STRINGS.BUILDINGS.PREFABS.PROPFACILITYGLOBEDROORS.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPFACILITYGLOBEDROORS.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_globe_kanim"), "off", Grid.SceneLayer.Building, 1, 3, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		LoreBearerUtil.AddLoreTo(gameObject, LoreBearerUtil.UnlockSpecificEntry("journal_newspaper", UI.USERMENUACTIONS.READLORE.SEARCH_CABINET, false));
		gameObject.AddOrGet<Demolishable>();
		gameObject.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		return gameObject;
	}

	// Token: 0x06001377 RID: 4983 RVA: 0x0007041B File Offset: 0x0006E61B
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001378 RID: 4984 RVA: 0x00070420 File Offset: 0x0006E620
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
