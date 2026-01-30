using System;
using TUNING;
using UnityEngine;

// Token: 0x020003F4 RID: 1012
public class RocketInteriorLiquidInputConfig : IBuildingConfig
{
	// Token: 0x060014DB RID: 5339 RVA: 0x00077D18 File Offset: 0x00075F18
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060014DC RID: 5340 RVA: 0x00077D20 File Offset: 0x00075F20
	public override BuildingDef CreateBuildingDef()
	{
		string id = "RocketInteriorLiquidInput";
		int width = 1;
		int height = 1;
		string anim = "rocket_floor_plug_liquid_kanim";
		int hitpoints = 30;
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnRocketEnvelope;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER2, none, 0.2f);
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
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
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.LiquidVentIDs, "RocketInteriorLiquidInput");
		return buildingDef;
	}

	// Token: 0x060014DD RID: 5341 RVA: 0x00077DE8 File Offset: 0x00075FE8
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		base.ConfigureBuildingTemplate(go, prefab_tag);
		go.GetComponent<KPrefabID>().AddTag(GameTags.RocketInteriorBuilding, false);
		go.AddComponent<RequireInputs>();
	}

	// Token: 0x060014DE RID: 5342 RVA: 0x00077E0C File Offset: 0x0007600C
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<ActiveController.Def>();
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 10f;
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		RocketConduitStorageAccess rocketConduitStorageAccess = go.AddOrGet<RocketConduitStorageAccess>();
		rocketConduitStorageAccess.storage = storage;
		rocketConduitStorageAccess.cargoType = CargoBay.CargoType.Liquids;
		rocketConduitStorageAccess.targetLevel = 0f;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.ignoreMinMassCheck = true;
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.alwaysConsume = true;
		conduitConsumer.capacityKG = storage.capacityKg;
	}

	// Token: 0x04000CA9 RID: 3241
	private const ConduitType CONDUIT_TYPE = ConduitType.Liquid;

	// Token: 0x04000CAA RID: 3242
	private const CargoBay.CargoType CARGO_TYPE = CargoBay.CargoType.Liquids;

	// Token: 0x04000CAB RID: 3243
	public const string ID = "RocketInteriorLiquidInput";
}
