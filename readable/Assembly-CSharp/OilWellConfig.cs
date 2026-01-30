using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001B0 RID: 432
public class OilWellConfig : IEntityConfig
{
	// Token: 0x06000873 RID: 2163 RVA: 0x00038F44 File Offset: 0x00037144
	public GameObject CreatePrefab()
	{
		string id = "OilWell";
		string name = STRINGS.CREATURES.SPECIES.OIL_WELL.NAME;
		string desc = STRINGS.CREATURES.SPECIES.OIL_WELL.DESC;
		float mass = 2000f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER1;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("geyser_side_oil_kanim"), "off", Grid.SceneLayer.BuildingBack, 4, 2, tier, tier2, SimHashes.Creature, null, 293f);
		gameObject.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.SedimentaryRock, true);
		component.Temperature = 372.15f;
		gameObject.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[]
		{
			new BuildingAttachPoint.HardPoint(new CellOffset(0, 0), GameTags.OilWell, null)
		};
		SoundEventVolumeCache.instance.AddVolume("geyser_side_methane_kanim", "GeyserMethane_shake_LP", NOISE_POLLUTION.NOISY.TIER5);
		SoundEventVolumeCache.instance.AddVolume("geyser_side_methane_kanim", "GeyserMethane_erupt_LP", NOISE_POLLUTION.NOISY.TIER6);
		return gameObject;
	}

	// Token: 0x06000874 RID: 2164 RVA: 0x00039034 File Offset: 0x00037234
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000875 RID: 2165 RVA: 0x00039036 File Offset: 0x00037236
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400065B RID: 1627
	public const string ID = "OilWell";
}
