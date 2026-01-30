using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000051 RID: 81
public class LiquidConduitDiseaseSensorConfig : ConduitSensorConfig
{
	// Token: 0x1700000E RID: 14
	// (get) Token: 0x06000198 RID: 408 RVA: 0x0000B812 File Offset: 0x00009A12
	protected override ConduitType ConduitType
	{
		get
		{
			return ConduitType.Liquid;
		}
	}

	// Token: 0x06000199 RID: 409 RVA: 0x0000B818 File Offset: 0x00009A18
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef result = base.CreateBuildingDef(LiquidConduitDiseaseSensorConfig.ID, "liquid_germs_sensor_kanim", new float[]
		{
			TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0[0],
			TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER1[0]
		}, new string[]
		{
			"RefinedMetal",
			"Plastic"
		}, new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort(LogicSwitch.PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.LIQUIDCONDUITDISEASESENSOR.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.LIQUIDCONDUITDISEASESENSOR.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.LIQUIDCONDUITDISEASESENSOR.LOGIC_PORT_INACTIVE, true, false)
		});
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.LiquidVentIDs, LiquidConduitDiseaseSensorConfig.ID);
		return result;
	}

	// Token: 0x0600019A RID: 410 RVA: 0x0000B8B0 File Offset: 0x00009AB0
	public override void DoPostConfigureComplete(GameObject go)
	{
		base.DoPostConfigureComplete(go);
		ConduitDiseaseSensor conduitDiseaseSensor = go.AddComponent<ConduitDiseaseSensor>();
		conduitDiseaseSensor.conduitType = this.ConduitType;
		conduitDiseaseSensor.Threshold = 0f;
		conduitDiseaseSensor.ActivateAboveThreshold = true;
		conduitDiseaseSensor.manuallyControlled = false;
		conduitDiseaseSensor.defaultState = false;
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits, false);
	}

	// Token: 0x040000F9 RID: 249
	public static string ID = "LiquidConduitDiseaseSensor";
}
