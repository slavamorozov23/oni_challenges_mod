using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x0200004F RID: 79
public abstract class ConduitSensorConfig : IBuildingConfig
{
	// Token: 0x1700000C RID: 12
	// (get) Token: 0x0600018F RID: 399
	protected abstract ConduitType ConduitType { get; }

	// Token: 0x06000190 RID: 400 RVA: 0x0000B654 File Offset: 0x00009854
	protected BuildingDef CreateBuildingDef(string ID, string anim, float[] required_mass, string[] required_materials, List<LogicPorts.Port> output_ports)
	{
		int width = 1;
		int height = 1;
		int hitpoints = 30;
		float construction_time = 30f;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(ID, width, height, anim, hitpoints, construction_time, required_mass, required_materials, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.ViewMode = OverlayModes.Logic.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.AlwaysOperational = true;
		buildingDef.LogicOutputPorts = output_ports;
		SoundEventVolumeCache.instance.AddVolume(anim, "PowerSwitch_on", NOISE_POLLUTION.NOISY.TIER3);
		SoundEventVolumeCache.instance.AddVolume(anim, "PowerSwitch_off", NOISE_POLLUTION.NOISY.TIER3);
		GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, ID);
		return buildingDef;
	}

	// Token: 0x06000191 RID: 401 RVA: 0x0000B702 File Offset: 0x00009902
	public override void DoPostConfigureComplete(GameObject go)
	{
	}
}
