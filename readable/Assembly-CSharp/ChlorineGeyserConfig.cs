using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200018C RID: 396
public class ChlorineGeyserConfig : IEntityConfig
{
	// Token: 0x060007AB RID: 1963 RVA: 0x000348D8 File Offset: 0x00032AD8
	public GameObject CreatePrefab()
	{
		string id = "ChlorineGeyser";
		string name = STRINGS.CREATURES.SPECIES.CHLORINEGEYSER.NAME;
		string desc = STRINGS.CREATURES.SPECIES.CHLORINEGEYSER.DESC;
		float mass = 2000f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER1;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("geyser_side_chlorine_kanim"), "inactive", Grid.SceneLayer.BuildingBack, 4, 2, tier, tier2, SimHashes.Creature, null, 293f);
		gameObject.GetComponent<KPrefabID>().AddTag(GameTags.DeprecatedContent, false);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.IgneousRock, true);
		component.Temperature = 372.15f;
		gameObject.AddOrGet<Geyser>().outputOffset = new Vector2I(0, 1);
		GeyserConfigurator geyserConfigurator = gameObject.AddOrGet<GeyserConfigurator>();
		geyserConfigurator.presetType = "chlorine_gas";
		geyserConfigurator.presetMin = 0.35f;
		geyserConfigurator.presetMax = 0.65f;
		Studyable studyable = gameObject.AddOrGet<Studyable>();
		studyable.meterTrackerSymbol = "geotracker_target";
		studyable.meterAnim = "tracker";
		gameObject.AddOrGet<LoopingSounds>();
		SoundEventVolumeCache.instance.AddVolume("geyser_methane_kanim", "GeyserMethane_shake_LP", NOISE_POLLUTION.NOISY.TIER5);
		SoundEventVolumeCache.instance.AddVolume("geyser_methane_kanim", "GeyserMethane_shake_LP", NOISE_POLLUTION.NOISY.TIER6);
		return gameObject;
	}

	// Token: 0x060007AC RID: 1964 RVA: 0x000349F7 File Offset: 0x00032BF7
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060007AD RID: 1965 RVA: 0x000349F9 File Offset: 0x00032BF9
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005CF RID: 1487
	public const string ID = "ChlorineGeyser";
}
