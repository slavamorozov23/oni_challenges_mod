using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001AD RID: 429
public class MethaneGeyserConfig : IEntityConfig
{
	// Token: 0x06000865 RID: 2149 RVA: 0x00038B78 File Offset: 0x00036D78
	public GameObject CreatePrefab()
	{
		string id = "MethaneGeyser";
		string name = STRINGS.CREATURES.SPECIES.METHANEGEYSER.NAME;
		string desc = STRINGS.CREATURES.SPECIES.METHANEGEYSER.DESC;
		float mass = 2000f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER1;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("geyser_side_methane_kanim"), "inactive", Grid.SceneLayer.BuildingBack, 4, 2, tier, tier2, SimHashes.Creature, null, 293f);
		gameObject.GetComponent<KPrefabID>().AddTag(GameTags.DeprecatedContent, false);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.IgneousRock, true);
		component.Temperature = 372.15f;
		gameObject.AddOrGet<Geyser>().outputOffset = new Vector2I(0, 1);
		GeyserConfigurator geyserConfigurator = gameObject.AddOrGet<GeyserConfigurator>();
		geyserConfigurator.presetType = "methane";
		geyserConfigurator.presetMin = 0.35f;
		geyserConfigurator.presetMax = 0.65f;
		Studyable studyable = gameObject.AddOrGet<Studyable>();
		studyable.meterTrackerSymbol = "geotracker_target";
		studyable.meterAnim = "tracker";
		gameObject.AddOrGet<LoopingSounds>();
		SoundEventVolumeCache.instance.AddVolume("geyser_side_methane_kanim", "GeyserMethane_shake_LP", NOISE_POLLUTION.NOISY.TIER5);
		SoundEventVolumeCache.instance.AddVolume("geyser_side_methane_kanim", "GeyserMethane_erupt_LP", NOISE_POLLUTION.NOISY.TIER6);
		return gameObject;
	}

	// Token: 0x06000866 RID: 2150 RVA: 0x00038C97 File Offset: 0x00036E97
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000867 RID: 2151 RVA: 0x00038C99 File Offset: 0x00036E99
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000656 RID: 1622
	public const string ID = "MethaneGeyser";
}
