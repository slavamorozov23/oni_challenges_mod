using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000426 RID: 1062
public class SolidConduitDiseaseSensorConfig : ConduitSensorConfig
{
	// Token: 0x17000060 RID: 96
	// (get) Token: 0x060015EA RID: 5610 RVA: 0x0007D33B File Offset: 0x0007B53B
	protected override ConduitType ConduitType
	{
		get
		{
			return ConduitType.Solid;
		}
	}

	// Token: 0x060015EB RID: 5611 RVA: 0x0007D340 File Offset: 0x0007B540
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef result = base.CreateBuildingDef(SolidConduitDiseaseSensorConfig.ID, "conveyor_germs_sensor_kanim", new float[]
		{
			TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0[0],
			TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER1[0]
		}, new string[]
		{
			"RefinedMetal",
			"Plastic"
		}, new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort(LogicSwitch.PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.SOLIDCONDUITDISEASESENSOR.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.SOLIDCONDUITDISEASESENSOR.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.SOLIDCONDUITDISEASESENSOR.LOGIC_PORT_INACTIVE, true, false)
		});
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SolidConveyorIDs, SolidConduitDiseaseSensorConfig.ID);
		return result;
	}

	// Token: 0x060015EC RID: 5612 RVA: 0x0007D3D8 File Offset: 0x0007B5D8
	public override void DoPostConfigureComplete(GameObject go)
	{
		base.DoPostConfigureComplete(go);
		ConduitDiseaseSensor conduitDiseaseSensor = go.AddComponent<ConduitDiseaseSensor>();
		conduitDiseaseSensor.conduitType = this.ConduitType;
		conduitDiseaseSensor.Threshold = 0f;
		conduitDiseaseSensor.ActivateAboveThreshold = true;
		conduitDiseaseSensor.manuallyControlled = false;
		conduitDiseaseSensor.defaultState = false;
	}

	// Token: 0x04000D08 RID: 3336
	public static string ID = "SolidConduitDiseaseSensor";
}
