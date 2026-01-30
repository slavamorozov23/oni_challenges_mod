using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002CF RID: 719
public class LogicWireBridgeConfig : IBuildingConfig
{
	// Token: 0x06000E9B RID: 3739 RVA: 0x00054F18 File Offset: 0x00053118
	public override BuildingDef CreateBuildingDef()
	{
		string id = "LogicWireBridge";
		int width = 3;
		int height = 1;
		string anim = "logic_bridge_kanim";
		int hitpoints = 30;
		float construction_time = 3f;
		float[] tier_TINY = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER_TINY;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.LogicBridge;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier_TINY, refined_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
		buildingDef.ViewMode = OverlayModes.Logic.ID;
		buildingDef.ObjectLayer = ObjectLayer.LogicGate;
		buildingDef.SceneLayer = Grid.SceneLayer.LogicGates;
		buildingDef.Overheatable = false;
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(0, 2);
		buildingDef.AlwaysOperational = true;
		buildingDef.LogicInputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.InputPort(LogicWireBridgeConfig.BRIDGE_LOGIC_IO_ID, new CellOffset(-1, 0), STRINGS.BUILDINGS.PREFABS.LOGICWIREBRIDGE.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.LOGICWIREBRIDGE.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.LOGICWIREBRIDGE.LOGIC_PORT_INACTIVE, false, false),
			LogicPorts.Port.InputPort(LogicWireBridgeConfig.BRIDGE_LOGIC_IO_ID, new CellOffset(1, 0), STRINGS.BUILDINGS.PREFABS.LOGICWIREBRIDGE.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.LOGICWIREBRIDGE.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.LOGICWIREBRIDGE.LOGIC_PORT_INACTIVE, false, false)
		};
		GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, "LogicWireBridge");
		buildingDef.AddSearchTerms(SEARCH_TERMS.AUTOMATION);
		return buildingDef;
	}

	// Token: 0x06000E9C RID: 3740 RVA: 0x00055070 File Offset: 0x00053270
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
	}

	// Token: 0x06000E9D RID: 3741 RVA: 0x00055087 File Offset: 0x00053287
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		base.DoPostConfigurePreview(def, go);
		this.AddNetworkLink(go).visualizeOnly = true;
		go.AddOrGet<BuildingCellVisualizer>();
	}

	// Token: 0x06000E9E RID: 3742 RVA: 0x000550A5 File Offset: 0x000532A5
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		this.AddNetworkLink(go).visualizeOnly = true;
		go.AddOrGet<BuildingCellVisualizer>();
	}

	// Token: 0x06000E9F RID: 3743 RVA: 0x000550C2 File Offset: 0x000532C2
	public override void DoPostConfigureComplete(GameObject go)
	{
		this.AddNetworkLink(go).visualizeOnly = false;
		go.AddOrGet<BuildingCellVisualizer>();
	}

	// Token: 0x06000EA0 RID: 3744 RVA: 0x000550D8 File Offset: 0x000532D8
	private LogicUtilityNetworkLink AddNetworkLink(GameObject go)
	{
		LogicUtilityNetworkLink logicUtilityNetworkLink = go.AddOrGet<LogicUtilityNetworkLink>();
		logicUtilityNetworkLink.bitDepth = LogicWire.BitDepth.OneBit;
		logicUtilityNetworkLink.link1 = new CellOffset(-1, 0);
		logicUtilityNetworkLink.link2 = new CellOffset(1, 0);
		return logicUtilityNetworkLink;
	}

	// Token: 0x04000975 RID: 2421
	public const string ID = "LogicWireBridge";

	// Token: 0x04000976 RID: 2422
	public static readonly HashedString BRIDGE_LOGIC_IO_ID = new HashedString("BRIDGE_LOGIC_IO");
}
