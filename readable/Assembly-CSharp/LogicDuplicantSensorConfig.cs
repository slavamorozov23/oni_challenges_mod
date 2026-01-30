using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002B0 RID: 688
public class LogicDuplicantSensorConfig : IBuildingConfig
{
	// Token: 0x06000DED RID: 3565 RVA: 0x000527B4 File Offset: 0x000509B4
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("LogicDuplicantSensor", 1, 1, "presence_sensor_kanim", 30, 30f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0, MATERIALS.REFINED_METALS, 1600f, BuildLocationRule.OnFoundationRotatable, TUNING.BUILDINGS.DECOR.PENALTY.TIER0, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.Entombable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.ViewMode = OverlayModes.Logic.ID;
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.AlwaysOperational = true;
		buildingDef.LogicOutputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort(LogicSwitch.PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.LOGICDUPLICANTSENSOR.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.LOGICDUPLICANTSENSOR.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.LOGICDUPLICANTSENSOR.LOGIC_PORT_INACTIVE, true, false)
		};
		GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, "LogicDuplicantSensor");
		buildingDef.AddSearchTerms(SEARCH_TERMS.AUTOMATION);
		return buildingDef;
	}

	// Token: 0x06000DEE RID: 3566 RVA: 0x0005289B File Offset: 0x00050A9B
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		LogicDuplicantSensorConfig.AddVisualizer(go, true);
	}

	// Token: 0x06000DEF RID: 3567 RVA: 0x000528A4 File Offset: 0x00050AA4
	public override void DoPostConfigureComplete(GameObject go)
	{
		LogicDuplicantSensor logicDuplicantSensor = go.AddOrGet<LogicDuplicantSensor>();
		logicDuplicantSensor.defaultState = false;
		logicDuplicantSensor.manuallyControlled = false;
		logicDuplicantSensor.pickupRange = 4;
		LogicDuplicantSensorConfig.AddVisualizer(go, false);
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits, false);
	}

	// Token: 0x06000DF0 RID: 3568 RVA: 0x000528D8 File Offset: 0x00050AD8
	private static void AddVisualizer(GameObject prefab, bool movable)
	{
		RangeVisualizer rangeVisualizer = prefab.AddOrGet<RangeVisualizer>();
		rangeVisualizer.OriginOffset = new Vector2I(0, 0);
		rangeVisualizer.RangeMin.x = -2;
		rangeVisualizer.RangeMin.y = 0;
		rangeVisualizer.RangeMax.x = 2;
		rangeVisualizer.RangeMax.y = 4;
		rangeVisualizer.BlockingTileVisible = true;
	}

	// Token: 0x04000951 RID: 2385
	public const string ID = "LogicDuplicantSensor";

	// Token: 0x04000952 RID: 2386
	private const int RANGE = 4;
}
