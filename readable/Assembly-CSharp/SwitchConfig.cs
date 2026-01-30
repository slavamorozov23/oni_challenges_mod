using System;
using TUNING;
using UnityEngine;

// Token: 0x02000449 RID: 1097
public class SwitchConfig : IBuildingConfig
{
	// Token: 0x060016C2 RID: 5826 RVA: 0x00082074 File Offset: 0x00080274
	public override BuildingDef CreateBuildingDef()
	{
		string id = SwitchConfig.ID;
		int width = 1;
		int height = 1;
		string anim = "switchpower_kanim";
		int hitpoints = 10;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.Floodable = false;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		SoundEventVolumeCache.instance.AddVolume("switchpower_kanim", "PowerSwitch_on", NOISE_POLLUTION.NOISY.TIER3);
		SoundEventVolumeCache.instance.AddVolume("switchpower_kanim", "PowerSwitch_off", NOISE_POLLUTION.NOISY.TIER3);
		return buildingDef;
	}

	// Token: 0x060016C3 RID: 5827 RVA: 0x00082118 File Offset: 0x00080318
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<BuildingComplete>().isManuallyOperated = false;
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		CircuitSwitch circuitSwitch = go.AddOrGet<CircuitSwitch>();
		circuitSwitch.objectLayer = ObjectLayer.Wire;
		circuitSwitch.manuallyControlled = false;
	}

	// Token: 0x060016C4 RID: 5828 RVA: 0x00082140 File Offset: 0x00080340
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddComponent<BuildingCellVisualizer>();
	}

	// Token: 0x04000D7C RID: 3452
	public static string ID = "Switch";
}
