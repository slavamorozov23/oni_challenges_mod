using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000053 RID: 83
public class LiquidConduitElementSensorConfig : ConduitSensorConfig
{
	// Token: 0x17000010 RID: 16
	// (get) Token: 0x060001A2 RID: 418 RVA: 0x0000B9F8 File Offset: 0x00009BF8
	protected override ConduitType ConduitType
	{
		get
		{
			return ConduitType.Liquid;
		}
	}

	// Token: 0x060001A3 RID: 419 RVA: 0x0000B9FC File Offset: 0x00009BFC
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef result = base.CreateBuildingDef(LiquidConduitElementSensorConfig.ID, "liquid_element_sensor_kanim", TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0, MATERIALS.REFINED_METALS, new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort(LogicSwitch.PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.LIQUIDCONDUITELEMENTSENSOR.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.LIQUIDCONDUITELEMENTSENSOR.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.LIQUIDCONDUITELEMENTSENSOR.LOGIC_PORT_INACTIVE, true, false)
		});
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.LiquidVentIDs, LiquidConduitElementSensorConfig.ID);
		return result;
	}

	// Token: 0x060001A4 RID: 420 RVA: 0x0000BA70 File Offset: 0x00009C70
	public override void DoPostConfigureComplete(GameObject go)
	{
		base.DoPostConfigureComplete(go);
		go.AddOrGet<Filterable>().filterElementState = Filterable.ElementState.Liquid;
		ConduitElementSensor conduitElementSensor = go.AddOrGet<ConduitElementSensor>();
		conduitElementSensor.manuallyControlled = false;
		conduitElementSensor.conduitType = this.ConduitType;
		conduitElementSensor.defaultState = false;
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits, false);
	}

	// Token: 0x040000FB RID: 251
	public static string ID = "LiquidConduitElementSensor";
}
