using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000052 RID: 82
public class GasConduitElementSensorConfig : ConduitSensorConfig
{
	// Token: 0x1700000F RID: 15
	// (get) Token: 0x0600019D RID: 413 RVA: 0x0000B91A File Offset: 0x00009B1A
	protected override ConduitType ConduitType
	{
		get
		{
			return ConduitType.Gas;
		}
	}

	// Token: 0x0600019E RID: 414 RVA: 0x0000B920 File Offset: 0x00009B20
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef result = base.CreateBuildingDef(GasConduitElementSensorConfig.ID, "gas_element_sensor_kanim", TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0, MATERIALS.REFINED_METALS, new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort(LogicSwitch.PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.GASCONDUITELEMENTSENSOR.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.GASCONDUITELEMENTSENSOR.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.GASCONDUITELEMENTSENSOR.LOGIC_PORT_INACTIVE, true, false)
		});
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.GasVentIDs, GasConduitElementSensorConfig.ID);
		return result;
	}

	// Token: 0x0600019F RID: 415 RVA: 0x0000B994 File Offset: 0x00009B94
	public override void DoPostConfigureComplete(GameObject go)
	{
		base.DoPostConfigureComplete(go);
		go.AddOrGet<Filterable>().filterElementState = Filterable.ElementState.Gas;
		ConduitElementSensor conduitElementSensor = go.AddOrGet<ConduitElementSensor>();
		conduitElementSensor.manuallyControlled = false;
		conduitElementSensor.conduitType = this.ConduitType;
		conduitElementSensor.defaultState = false;
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits, false);
	}

	// Token: 0x040000FA RID: 250
	public static string ID = "GasConduitElementSensor";
}
