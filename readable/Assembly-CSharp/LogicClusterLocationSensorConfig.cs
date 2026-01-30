using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002AC RID: 684
public class LogicClusterLocationSensorConfig : IBuildingConfig
{
	// Token: 0x06000DDB RID: 3547 RVA: 0x00052191 File Offset: 0x00050391
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000DDC RID: 3548 RVA: 0x00052198 File Offset: 0x00050398
	public override BuildingDef CreateBuildingDef()
	{
		string id = LogicClusterLocationSensorConfig.ID;
		int width = 1;
		int height = 1;
		string anim = "logic_location_kanim";
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
		buildingDef.DefaultAnimState = "off_asteroid";
		buildingDef.ViewMode = OverlayModes.Logic.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.AlwaysOperational = true;
		buildingDef.LogicOutputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort(LogicSwitch.PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.LOGICCLUSTERLOCATIONSENSOR.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.LOGICCLUSTERLOCATIONSENSOR.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.LOGICCLUSTERLOCATIONSENSOR.LOGIC_PORT_INACTIVE, true, false)
		};
		SoundEventVolumeCache.instance.AddVolume("switchgaspressure_kanim", "PowerSwitch_on", NOISE_POLLUTION.NOISY.TIER3);
		SoundEventVolumeCache.instance.AddVolume("switchgaspressure_kanim", "PowerSwitch_off", NOISE_POLLUTION.NOISY.TIER3);
		GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, LogicClusterLocationSensorConfig.ID);
		buildingDef.AddSearchTerms(SEARCH_TERMS.AUTOMATION);
		return buildingDef;
	}

	// Token: 0x06000DDD RID: 3549 RVA: 0x000522B6 File Offset: 0x000504B6
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		base.ConfigureBuildingTemplate(go, prefab_tag);
		go.GetComponent<KPrefabID>().AddTag(GameTags.RocketInteriorBuilding, false);
	}

	// Token: 0x06000DDE RID: 3550 RVA: 0x000522D1 File Offset: 0x000504D1
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicClusterLocationSensor>();
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits, false);
	}

	// Token: 0x0400094D RID: 2381
	public static string ID = "LogicClusterLocationSensor";
}
