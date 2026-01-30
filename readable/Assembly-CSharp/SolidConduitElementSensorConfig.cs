using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000427 RID: 1063
public class SolidConduitElementSensorConfig : ConduitSensorConfig
{
	// Token: 0x17000061 RID: 97
	// (get) Token: 0x060015EF RID: 5615 RVA: 0x0007D426 File Offset: 0x0007B626
	protected override ConduitType ConduitType
	{
		get
		{
			return ConduitType.Solid;
		}
	}

	// Token: 0x060015F0 RID: 5616 RVA: 0x0007D42C File Offset: 0x0007B62C
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef result = base.CreateBuildingDef(SolidConduitElementSensorConfig.ID, "conveyor_element_sensor_kanim", TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0, MATERIALS.REFINED_METALS, new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort(LogicSwitch.PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.SOLIDCONDUITELEMENTSENSOR.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.SOLIDCONDUITELEMENTSENSOR.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.SOLIDCONDUITELEMENTSENSOR.LOGIC_PORT_INACTIVE, true, false)
		});
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SolidConveyorIDs, SolidConduitElementSensorConfig.ID);
		return result;
	}

	// Token: 0x060015F1 RID: 5617 RVA: 0x0007D49E File Offset: 0x0007B69E
	public override void DoPostConfigureComplete(GameObject go)
	{
		base.DoPostConfigureComplete(go);
		go.AddOrGet<Filterable>().filterElementState = Filterable.ElementState.Solid;
		ConduitElementSensor conduitElementSensor = go.AddOrGet<ConduitElementSensor>();
		conduitElementSensor.manuallyControlled = false;
		conduitElementSensor.conduitType = this.ConduitType;
		conduitElementSensor.defaultState = false;
	}

	// Token: 0x04000D09 RID: 3337
	public static string ID = "SolidConduitElementSensor";
}
