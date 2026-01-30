using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000230 RID: 560
public class GasLimitValveConfig : IBuildingConfig
{
	// Token: 0x06000B5A RID: 2906 RVA: 0x00043F6C File Offset: 0x0004216C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "GasLimitValve";
		int width = 1;
		int height = 2;
		string anim = "limit_valve_gas_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] construction_mass = new float[]
		{
			TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0[0],
			TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER1[0]
		};
		string[] construction_materials = new string[]
		{
			"RefinedMetal",
			"Plastic"
		};
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER0, tier, 0.2f);
		buildingDef.InputConduitType = ConduitType.Gas;
		buildingDef.OutputConduitType = ConduitType.Gas;
		buildingDef.Floodable = false;
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 10f;
		buildingDef.PowerInputOffset = new CellOffset(0, 1);
		buildingDef.ViewMode = OverlayModes.GasConduits.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(0, 1);
		buildingDef.LogicInputPorts = new List<LogicPorts.Port>
		{
			new LogicPorts.Port(LimitValve.RESET_PORT_ID, new CellOffset(0, 1), STRINGS.BUILDINGS.PREFABS.GASLIMITVALVE.LOGIC_PORT_RESET, STRINGS.BUILDINGS.PREFABS.GASLIMITVALVE.RESET_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.GASLIMITVALVE.RESET_PORT_INACTIVE, false, LogicPortSpriteType.ResetUpdate, true)
		};
		buildingDef.LogicOutputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort(LimitValve.OUTPUT_PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.GASLIMITVALVE.LOGIC_PORT_OUTPUT, STRINGS.BUILDINGS.PREFABS.GASLIMITVALVE.OUTPUT_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.GASLIMITVALVE.OUTPUT_PORT_INACTIVE, false, false)
		};
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.GasVentIDs, "GasLimitValve");
		return buildingDef;
	}

	// Token: 0x06000B5B RID: 2907 RVA: 0x000440D8 File Offset: 0x000422D8
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGetDef<PoweredActiveTransitionController.Def>();
		UnityEngine.Object.DestroyImmediate(go.GetComponent<BuildingEnabledButton>());
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<ConduitBridge>().type = ConduitType.Gas;
		LimitValve limitValve = go.AddOrGet<LimitValve>();
		limitValve.conduitType = ConduitType.Gas;
		limitValve.maxLimitKg = 500f;
		limitValve.Limit = 0f;
		limitValve.sliderRanges = LimitValveTuning.GetDefaultSlider();
	}

	// Token: 0x06000B5C RID: 2908 RVA: 0x00044145 File Offset: 0x00042345
	public override void DoPostConfigureComplete(GameObject go)
	{
		UnityEngine.Object.DestroyImmediate(go.GetComponent<ConduitConsumer>());
		UnityEngine.Object.DestroyImmediate(go.GetComponent<ConduitDispenser>());
		go.GetComponent<RequireInputs>().SetRequirements(true, false);
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits, false);
	}

	// Token: 0x040007E0 RID: 2016
	public const string ID = "GasLimitValve";

	// Token: 0x040007E1 RID: 2017
	private const ConduitType CONDUIT_TYPE = ConduitType.Gas;
}
