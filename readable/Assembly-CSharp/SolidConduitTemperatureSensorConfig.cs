using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000428 RID: 1064
public class SolidConduitTemperatureSensorConfig : ConduitSensorConfig
{
	// Token: 0x17000062 RID: 98
	// (get) Token: 0x060015F4 RID: 5620 RVA: 0x0007D4E6 File Offset: 0x0007B6E6
	protected override ConduitType ConduitType
	{
		get
		{
			return ConduitType.Solid;
		}
	}

	// Token: 0x060015F5 RID: 5621 RVA: 0x0007D4EC File Offset: 0x0007B6EC
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef result = base.CreateBuildingDef(SolidConduitTemperatureSensorConfig.ID, "conveyor_temperature_sensor_kanim", TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0, MATERIALS.REFINED_METALS, new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort(LogicSwitch.PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.SOLIDCONDUITTEMPERATURESENSOR.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.SOLIDCONDUITTEMPERATURESENSOR.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.SOLIDCONDUITTEMPERATURESENSOR.LOGIC_PORT_INACTIVE, true, false)
		});
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SolidConveyorIDs, SolidConduitTemperatureSensorConfig.ID);
		return result;
	}

	// Token: 0x060015F6 RID: 5622 RVA: 0x0007D560 File Offset: 0x0007B760
	public override void DoPostConfigureComplete(GameObject go)
	{
		base.DoPostConfigureComplete(go);
		ConduitTemperatureSensor conduitTemperatureSensor = go.AddComponent<ConduitTemperatureSensor>();
		conduitTemperatureSensor.conduitType = this.ConduitType;
		conduitTemperatureSensor.Threshold = 280f;
		conduitTemperatureSensor.ActivateAboveThreshold = true;
		conduitTemperatureSensor.manuallyControlled = false;
		conduitTemperatureSensor.rangeMin = 0f;
		conduitTemperatureSensor.rangeMax = 9999f;
		conduitTemperatureSensor.defaultState = false;
	}

	// Token: 0x04000D0A RID: 3338
	public static string ID = "SolidConduitTemperatureSensor";
}
