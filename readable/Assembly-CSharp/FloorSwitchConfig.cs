using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200017F RID: 383
public class FloorSwitchConfig : IBuildingConfig
{
	// Token: 0x06000771 RID: 1905 RVA: 0x00033528 File Offset: 0x00031728
	public override BuildingDef CreateBuildingDef()
	{
		string id = "FloorSwitch";
		int width = 1;
		int height = 1;
		string anim = "pressureswitch_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Tile;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
		buildingDef.IsFoundation = true;
		buildingDef.Overheatable = false;
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.ViewMode = OverlayModes.Logic.ID;
		buildingDef.TileLayer = ObjectLayer.FoundationTile;
		buildingDef.ReplacementLayer = ObjectLayer.ReplacementTile;
		buildingDef.SceneLayer = Grid.SceneLayer.TileMain;
		buildingDef.ConstructionOffsetFilter = BuildingDef.ConstructionOffsetFilter_OneDown;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.LogicOutputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort(LogicSwitch.PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.LOGICSWITCH.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.LOGICSWITCH.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.LOGICSWITCH.LOGIC_PORT_INACTIVE, true, false)
		};
		buildingDef.AlwaysOperational = true;
		GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, "FloorSwitch");
		return buildingDef;
	}

	// Token: 0x06000772 RID: 1906 RVA: 0x00033631 File Offset: 0x00031831
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
		simCellOccupier.doReplaceElement = true;
		simCellOccupier.movementSpeedMultiplier = DUPLICANTSTATS.MOVEMENT_MODIFIERS.BONUS_2;
		simCellOccupier.notifyOnMelt = true;
	}

	// Token: 0x06000773 RID: 1907 RVA: 0x00033668 File Offset: 0x00031868
	public override void DoPostConfigureComplete(GameObject go)
	{
		LogicMassSensor logicMassSensor = go.AddOrGet<LogicMassSensor>();
		logicMassSensor.rangeMin = 0f;
		logicMassSensor.rangeMax = 2000f;
		logicMassSensor.Threshold = 10f;
		logicMassSensor.ActivateAboveThreshold = true;
		logicMassSensor.manuallyControlled = false;
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits, false);
	}

	// Token: 0x040005A8 RID: 1448
	public const string ID = "FloorSwitch";
}
