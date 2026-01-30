using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000054 RID: 84
public class GasConduitTemperatureSensorConfig : ConduitSensorConfig
{
	// Token: 0x17000011 RID: 17
	// (get) Token: 0x060001A7 RID: 423 RVA: 0x0000BAD4 File Offset: 0x00009CD4
	protected override ConduitType ConduitType
	{
		get
		{
			return ConduitType.Gas;
		}
	}

	// Token: 0x060001A8 RID: 424 RVA: 0x0000BAD8 File Offset: 0x00009CD8
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef result = base.CreateBuildingDef(GasConduitTemperatureSensorConfig.ID, "gas_temperature_sensor_kanim", TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0, MATERIALS.REFINED_METALS, new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort(LogicSwitch.PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.GASCONDUITTEMPERATURESENSOR.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.GASCONDUITTEMPERATURESENSOR.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.GASCONDUITTEMPERATURESENSOR.LOGIC_PORT_INACTIVE, true, false)
		});
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.GasVentIDs, GasConduitTemperatureSensorConfig.ID);
		return result;
	}

	// Token: 0x060001A9 RID: 425 RVA: 0x0000BB4C File Offset: 0x00009D4C
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
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits, false);
	}

	// Token: 0x040000FC RID: 252
	public static string ID = "GasConduitTemperatureSensor";
}
