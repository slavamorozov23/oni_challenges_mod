using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002C6 RID: 710
public class LogicRibbonBridgeConfig : IBuildingConfig
{
	// Token: 0x06000E73 RID: 3699 RVA: 0x000542E4 File Offset: 0x000524E4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "LogicRibbonBridge";
		int width = 3;
		int height = 1;
		string anim = "logic_ribbon_bridge_kanim";
		int hitpoints = 30;
		float construction_time = 3f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.LogicBridge;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
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
			LogicPorts.Port.RibbonInputPort(LogicRibbonBridgeConfig.BRIDGE_LOGIC_RIBBON_IO_ID, new CellOffset(-1, 0), STRINGS.BUILDINGS.PREFABS.LOGICRIBBONBRIDGE.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.LOGICRIBBONBRIDGE.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.LOGICRIBBONBRIDGE.LOGIC_PORT_INACTIVE, false, false),
			LogicPorts.Port.RibbonInputPort(LogicRibbonBridgeConfig.BRIDGE_LOGIC_RIBBON_IO_ID, new CellOffset(1, 0), STRINGS.BUILDINGS.PREFABS.LOGICRIBBONBRIDGE.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.LOGICRIBBONBRIDGE.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.LOGICRIBBONBRIDGE.LOGIC_PORT_INACTIVE, false, false)
		};
		GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, "LogicRibbonBridge");
		buildingDef.AddSearchTerms(SEARCH_TERMS.AUTOMATION);
		return buildingDef;
	}

	// Token: 0x06000E74 RID: 3700 RVA: 0x0005443C File Offset: 0x0005263C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
	}

	// Token: 0x06000E75 RID: 3701 RVA: 0x00054453 File Offset: 0x00052653
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		base.DoPostConfigurePreview(def, go);
		this.AddNetworkLink(go).visualizeOnly = true;
		go.AddOrGet<BuildingCellVisualizer>();
	}

	// Token: 0x06000E76 RID: 3702 RVA: 0x00054471 File Offset: 0x00052671
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		this.AddNetworkLink(go).visualizeOnly = true;
		go.AddOrGet<BuildingCellVisualizer>();
	}

	// Token: 0x06000E77 RID: 3703 RVA: 0x0005448E File Offset: 0x0005268E
	public override void DoPostConfigureComplete(GameObject go)
	{
		this.AddNetworkLink(go).visualizeOnly = false;
		go.AddOrGet<BuildingCellVisualizer>();
		go.AddOrGet<LogicRibbonBridge>();
	}

	// Token: 0x06000E78 RID: 3704 RVA: 0x000544AB File Offset: 0x000526AB
	private LogicUtilityNetworkLink AddNetworkLink(GameObject go)
	{
		LogicUtilityNetworkLink logicUtilityNetworkLink = go.AddOrGet<LogicUtilityNetworkLink>();
		logicUtilityNetworkLink.bitDepth = LogicWire.BitDepth.FourBit;
		logicUtilityNetworkLink.link1 = new CellOffset(-1, 0);
		logicUtilityNetworkLink.link2 = new CellOffset(1, 0);
		return logicUtilityNetworkLink;
	}

	// Token: 0x0400096A RID: 2410
	public const string ID = "LogicRibbonBridge";

	// Token: 0x0400096B RID: 2411
	public static readonly HashedString BRIDGE_LOGIC_RIBBON_IO_ID = new HashedString("BRIDGE_LOGIC_RIBBON_IO");
}
