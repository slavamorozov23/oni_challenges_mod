using System;
using TUNING;
using UnityEngine;

// Token: 0x020003F0 RID: 1008
public class RocketInteriorGasInputConfig : IBuildingConfig
{
	// Token: 0x060014C1 RID: 5313 RVA: 0x00077645 File Offset: 0x00075845
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060014C2 RID: 5314 RVA: 0x0007764C File Offset: 0x0007584C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "RocketInteriorGasInput";
		int width = 1;
		int height = 1;
		string anim = "rocket_floor_plug_gas_kanim";
		int hitpoints = 30;
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnRocketEnvelope;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER2, none, 0.2f);
		buildingDef.InputConduitType = ConduitType.Gas;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
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
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.GasVentIDs, "RocketInteriorGasInput");
		return buildingDef;
	}

	// Token: 0x060014C3 RID: 5315 RVA: 0x00077714 File Offset: 0x00075914
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		base.ConfigureBuildingTemplate(go, prefab_tag);
		go.GetComponent<KPrefabID>().AddTag(GameTags.RocketInteriorBuilding, false);
		go.AddComponent<RequireInputs>();
	}

	// Token: 0x060014C4 RID: 5316 RVA: 0x00077738 File Offset: 0x00075938
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<ActiveController.Def>();
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 1f;
		RocketConduitStorageAccess rocketConduitStorageAccess = go.AddOrGet<RocketConduitStorageAccess>();
		rocketConduitStorageAccess.storage = storage;
		rocketConduitStorageAccess.cargoType = CargoBay.CargoType.Gasses;
		rocketConduitStorageAccess.targetLevel = 0f;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Gas;
		conduitConsumer.ignoreMinMassCheck = true;
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.alwaysConsume = true;
		conduitConsumer.capacityKG = storage.capacityKg;
	}

	// Token: 0x04000C9F RID: 3231
	private const ConduitType CONDUIT_TYPE = ConduitType.Gas;

	// Token: 0x04000CA0 RID: 3232
	private const CargoBay.CargoType CARGO_TYPE = CargoBay.CargoType.Gasses;

	// Token: 0x04000CA1 RID: 3233
	public const string ID = "RocketInteriorGasInput";
}
