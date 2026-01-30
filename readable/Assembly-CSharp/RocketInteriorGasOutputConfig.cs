using System;
using TUNING;
using UnityEngine;

// Token: 0x020003F2 RID: 1010
public class RocketInteriorGasOutputConfig : IBuildingConfig
{
	// Token: 0x060014CE RID: 5326 RVA: 0x000779C0 File Offset: 0x00075BC0
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060014CF RID: 5327 RVA: 0x000779C8 File Offset: 0x00075BC8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "RocketInteriorGasOutput";
		int width = 1;
		int height = 1;
		string anim = "rocket_floor_plug_gas_out_kanim";
		int hitpoints = 30;
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnRocketEnvelope;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER2, none, 0.2f);
		buildingDef.OutputConduitType = ConduitType.Gas;
		buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 60f;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
		buildingDef.Overheatable = false;
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.ViewMode = OverlayModes.GasConduits.ID;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.ShowInBuildMenu = true;
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.GasVentIDs, "RocketInteriorGasOutput");
		return buildingDef;
	}

	// Token: 0x060014D0 RID: 5328 RVA: 0x00077AB8 File Offset: 0x00075CB8
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		base.ConfigureBuildingTemplate(go, prefab_tag);
		go.GetComponent<KPrefabID>().AddTag(GameTags.RocketInteriorBuilding, false);
		go.AddComponent<RequireInputs>();
	}

	// Token: 0x060014D1 RID: 5329 RVA: 0x00077ADC File Offset: 0x00075CDC
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<PoweredActiveController.Def>();
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 1f;
		go.AddOrGet<Filterable>().filterElementState = Filterable.ElementState.Gas;
		RocketConduitStorageAccess rocketConduitStorageAccess = go.AddOrGet<RocketConduitStorageAccess>();
		rocketConduitStorageAccess.storage = storage;
		rocketConduitStorageAccess.cargoType = CargoBay.CargoType.Gasses;
		rocketConduitStorageAccess.targetLevel = 1f;
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.conduitType = ConduitType.Gas;
		conduitDispenser.alwaysDispense = true;
		conduitDispenser.elementFilter = null;
	}

	// Token: 0x04000CA4 RID: 3236
	private const ConduitType CONDUIT_TYPE = ConduitType.Gas;

	// Token: 0x04000CA5 RID: 3237
	private const CargoBay.CargoType CARGO_TYPE = CargoBay.CargoType.Gasses;

	// Token: 0x04000CA6 RID: 3238
	public const string ID = "RocketInteriorGasOutput";
}
