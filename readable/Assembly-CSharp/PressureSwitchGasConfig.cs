using System;
using TUNING;
using UnityEngine;

// Token: 0x0200039D RID: 925
public class PressureSwitchGasConfig : IBuildingConfig
{
	// Token: 0x0600131C RID: 4892 RVA: 0x0006F10C File Offset: 0x0006D30C
	public override BuildingDef CreateBuildingDef()
	{
		string id = PressureSwitchGasConfig.ID;
		int width = 1;
		int height = 1;
		string anim = "switchgaspressure_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
		buildingDef.Deprecated = true;
		buildingDef.Overheatable = false;
		buildingDef.Floodable = true;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		SoundEventVolumeCache.instance.AddVolume("switchgaspressure_kanim", "PowerSwitch_on", NOISE_POLLUTION.NOISY.TIER3);
		SoundEventVolumeCache.instance.AddVolume("switchgaspressure_kanim", "PowerSwitch_off", NOISE_POLLUTION.NOISY.TIER3);
		return buildingDef;
	}

	// Token: 0x0600131D RID: 4893 RVA: 0x0006F1B8 File Offset: 0x0006D3B8
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		PressureSwitch pressureSwitch = go.AddOrGet<PressureSwitch>();
		pressureSwitch.objectLayer = ObjectLayer.Wire;
		pressureSwitch.rangeMin = 0f;
		pressureSwitch.rangeMax = 2f;
		pressureSwitch.Threshold = 1f;
		pressureSwitch.ActivateAboveThreshold = false;
		pressureSwitch.manuallyControlled = false;
		pressureSwitch.desiredState = Element.State.Gas;
	}

	// Token: 0x0600131E RID: 4894 RVA: 0x0006F20E File Offset: 0x0006D40E
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddComponent<BuildingCellVisualizer>();
	}

	// Token: 0x04000C15 RID: 3093
	public static string ID = "PressureSwitchGas";
}
