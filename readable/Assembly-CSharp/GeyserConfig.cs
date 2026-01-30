using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001A3 RID: 419
public class GeyserConfig : IEntityConfig
{
	// Token: 0x0600082C RID: 2092 RVA: 0x0003725C File Offset: 0x0003545C
	public GameObject CreatePrefab()
	{
		string id = "Geyser";
		string name = STRINGS.CREATURES.SPECIES.GEYSER.NAME;
		string desc = STRINGS.CREATURES.SPECIES.GEYSER.DESC;
		float mass = 2000f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER1;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER6;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("geyser_side_steam_kanim"), "inactive", Grid.SceneLayer.BuildingBack, 4, 2, tier, tier2, SimHashes.Creature, null, 293f);
		gameObject.GetComponent<KPrefabID>().AddTag(GameTags.DeprecatedContent, false);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.IgneousRock, true);
		component.Temperature = 372.15f;
		gameObject.AddOrGet<Geyser>().outputOffset = new Vector2I(0, 1);
		gameObject.AddOrGet<UserNameable>();
		GeyserConfigurator geyserConfigurator = gameObject.AddOrGet<GeyserConfigurator>();
		geyserConfigurator.presetType = "steam";
		geyserConfigurator.presetMin = 0.5f;
		geyserConfigurator.presetMax = 0.75f;
		Studyable studyable = gameObject.AddOrGet<Studyable>();
		studyable.meterTrackerSymbol = "geotracker_target";
		studyable.meterAnim = "tracker";
		gameObject.AddOrGet<LoopingSounds>();
		SoundEventVolumeCache.instance.AddVolume("geyser_side_steam_kanim", "Geyser_shake_LP", NOISE_POLLUTION.NOISY.TIER5);
		SoundEventVolumeCache.instance.AddVolume("geyser_side_steam_kanim", "Geyser_erupt_LP", NOISE_POLLUTION.NOISY.TIER6);
		return gameObject;
	}

	// Token: 0x0600082D RID: 2093 RVA: 0x00037382 File Offset: 0x00035582
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600082E RID: 2094 RVA: 0x00037384 File Offset: 0x00035584
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400061B RID: 1563
	public const int GEOTUNERS_REQUIRED_FOR_MAJOR_TRACKER_ANIMATION = 5;

	// Token: 0x020011B5 RID: 4533
	public enum TrackerMeterAnimNames
	{
		// Token: 0x04006565 RID: 25957
		tracker,
		// Token: 0x04006566 RID: 25958
		geotracker,
		// Token: 0x04006567 RID: 25959
		geotracker_minor,
		// Token: 0x04006568 RID: 25960
		geotracker_major
	}
}
