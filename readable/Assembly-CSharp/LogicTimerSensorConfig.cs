using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002CD RID: 717
public class LogicTimerSensorConfig : IBuildingConfig
{
	// Token: 0x06000E93 RID: 3731 RVA: 0x00054C78 File Offset: 0x00052E78
	public override BuildingDef CreateBuildingDef()
	{
		string id = LogicTimerSensorConfig.ID;
		int width = 1;
		int height = 1;
		string anim = "time_sensor_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.ViewMode = OverlayModes.Logic.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.AlwaysOperational = true;
		buildingDef.LogicOutputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort(LogicSwitch.PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.LOGICTIMERSENSOR.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.LOGICTIMERSENSOR.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.LOGICTIMERSENSOR.LOGIC_PORT_INACTIVE, true, false)
		};
		SoundEventVolumeCache.instance.AddVolume("switchgaspressure_kanim", "PowerSwitch_on", NOISE_POLLUTION.NOISY.TIER3);
		SoundEventVolumeCache.instance.AddVolume("switchgaspressure_kanim", "PowerSwitch_off", NOISE_POLLUTION.NOISY.TIER3);
		GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, LogicTimerSensorConfig.ID);
		buildingDef.AddSearchTerms(SEARCH_TERMS.AUTOMATION);
		return buildingDef;
	}

	// Token: 0x06000E94 RID: 3732 RVA: 0x00054D8B File Offset: 0x00052F8B
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicTimerSensor>().manuallyControlled = false;
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits, false);
	}

	// Token: 0x04000972 RID: 2418
	public static string ID = "LogicTimerSensor";
}
