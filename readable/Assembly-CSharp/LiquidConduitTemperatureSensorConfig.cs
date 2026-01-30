using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000055 RID: 85
public class LiquidConduitTemperatureSensorConfig : ConduitSensorConfig
{
	// Token: 0x17000012 RID: 18
	// (get) Token: 0x060001AC RID: 428 RVA: 0x0000BBCC File Offset: 0x00009DCC
	protected override ConduitType ConduitType
	{
		get
		{
			return ConduitType.Liquid;
		}
	}

	// Token: 0x060001AD RID: 429 RVA: 0x0000BBD0 File Offset: 0x00009DD0
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef result = base.CreateBuildingDef(LiquidConduitTemperatureSensorConfig.ID, "liquid_temperature_sensor_kanim", TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0, MATERIALS.REFINED_METALS, new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort(LogicSwitch.PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.LIQUIDCONDUITTEMPERATURESENSOR.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.LIQUIDCONDUITTEMPERATURESENSOR.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.LIQUIDCONDUITTEMPERATURESENSOR.LOGIC_PORT_INACTIVE, true, false)
		});
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.LiquidVentIDs, LiquidConduitTemperatureSensorConfig.ID);
		return result;
	}

	// Token: 0x060001AE RID: 430 RVA: 0x0000BC44 File Offset: 0x00009E44
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

	// Token: 0x040000FD RID: 253
	public static string ID = "LiquidConduitTemperatureSensor";
}
