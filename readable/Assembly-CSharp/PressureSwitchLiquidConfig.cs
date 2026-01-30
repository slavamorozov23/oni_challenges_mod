using System;
using TUNING;
using UnityEngine;

// Token: 0x0200039E RID: 926
public class PressureSwitchLiquidConfig : IBuildingConfig
{
	// Token: 0x06001321 RID: 4897 RVA: 0x0006F22C File Offset: 0x0006D42C
	public override BuildingDef CreateBuildingDef()
	{
		string id = PressureSwitchLiquidConfig.ID;
		int width = 1;
		int height = 1;
		string anim = "switchliquidpressure_kanim";
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
		buildingDef.Floodable = false;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		SoundEventVolumeCache.instance.AddVolume("switchliquidpressure_kanim", "PowerSwitch_on", NOISE_POLLUTION.NOISY.TIER3);
		SoundEventVolumeCache.instance.AddVolume("switchliquidpressure_kanim", "PowerSwitch_off", NOISE_POLLUTION.NOISY.TIER3);
		return buildingDef;
	}

	// Token: 0x06001322 RID: 4898 RVA: 0x0006F2D8 File Offset: 0x0006D4D8
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		PressureSwitch pressureSwitch = go.AddOrGet<PressureSwitch>();
		pressureSwitch.objectLayer = ObjectLayer.Wire;
		pressureSwitch.rangeMin = 0f;
		pressureSwitch.rangeMax = 2000f;
		pressureSwitch.Threshold = 500f;
		pressureSwitch.ActivateAboveThreshold = false;
		pressureSwitch.manuallyControlled = false;
		pressureSwitch.desiredState = Element.State.Liquid;
	}

	// Token: 0x06001323 RID: 4899 RVA: 0x0006F32E File Offset: 0x0006D52E
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddComponent<BuildingCellVisualizer>();
	}

	// Token: 0x04000C16 RID: 3094
	public static string ID = "PressureSwitchLiquid";
}
