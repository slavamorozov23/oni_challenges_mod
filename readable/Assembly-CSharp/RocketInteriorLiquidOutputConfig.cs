using System;
using TUNING;
using UnityEngine;

// Token: 0x020003F6 RID: 1014
public class RocketInteriorLiquidOutputConfig : IBuildingConfig
{
	// Token: 0x060014E8 RID: 5352 RVA: 0x000780A0 File Offset: 0x000762A0
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060014E9 RID: 5353 RVA: 0x000780A8 File Offset: 0x000762A8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "RocketInteriorLiquidOutput";
		int width = 1;
		int height = 1;
		string anim = "rocket_floor_plug_liquid_out_kanim";
		int hitpoints = 30;
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnRocketEnvelope;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER2, none, 0.2f);
		buildingDef.OutputConduitType = ConduitType.Liquid;
		buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 60f;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
		buildingDef.Overheatable = false;
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.ShowInBuildMenu = true;
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.LiquidVentIDs, "RocketInteriorLiquidOutput");
		return buildingDef;
	}

	// Token: 0x060014EA RID: 5354 RVA: 0x00078198 File Offset: 0x00076398
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		base.ConfigureBuildingTemplate(go, prefab_tag);
		go.GetComponent<KPrefabID>().AddTag(GameTags.RocketInteriorBuilding, false);
		go.AddComponent<RequireInputs>();
	}

	// Token: 0x060014EB RID: 5355 RVA: 0x000781BC File Offset: 0x000763BC
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<PoweredActiveController.Def>();
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 10f;
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		go.AddOrGet<Filterable>().filterElementState = Filterable.ElementState.Liquid;
		RocketConduitStorageAccess rocketConduitStorageAccess = go.AddOrGet<RocketConduitStorageAccess>();
		rocketConduitStorageAccess.storage = storage;
		rocketConduitStorageAccess.cargoType = CargoBay.CargoType.Liquids;
		rocketConduitStorageAccess.targetLevel = 10f;
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.conduitType = ConduitType.Liquid;
		conduitDispenser.alwaysDispense = true;
		conduitDispenser.elementFilter = null;
	}

	// Token: 0x04000CAE RID: 3246
	private const ConduitType CONDUIT_TYPE = ConduitType.Liquid;

	// Token: 0x04000CAF RID: 3247
	private const CargoBay.CargoType CARGO_TYPE = CargoBay.CargoType.Liquids;

	// Token: 0x04000CB0 RID: 3248
	public const string ID = "RocketInteriorLiquidOutput";
}
