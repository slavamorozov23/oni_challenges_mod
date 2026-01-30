using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200043A RID: 1082
public class SteamEngineConfig : IBuildingConfig
{
	// Token: 0x06001673 RID: 5747 RVA: 0x0007FA43 File Offset: 0x0007DC43
	public override string[] GetForbiddenDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001674 RID: 5748 RVA: 0x0007FA4C File Offset: 0x0007DC4C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SteamEngine";
		int width = 7;
		int height = 5;
		string anim = "rocket_steam_engine_kanim";
		int hitpoints = 1000;
		float construction_time = 480f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER7;
		string[] construction_materials = new string[]
		{
			SimHashes.Steel.ToString()
		};
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, construction_materials, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, tier2, 0.2f);
		BuildingTemplates.CreateRocketBuildingDef(buildingDef);
		buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.AttachmentSlotTag = GameTags.Rocket;
		buildingDef.attachablePosition = new CellOffset(0, 0);
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.UtilityInputOffset = new CellOffset(2, 3);
		buildingDef.InputConduitType = ConduitType.Gas;
		buildingDef.RequiresPowerInput = false;
		buildingDef.CanMove = true;
		buildingDef.AddSearchTerms(SEARCH_TERMS.STEAM);
		return buildingDef;
	}

	// Token: 0x06001675 RID: 5749 RVA: 0x0007FB20 File Offset: 0x0007DD20
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[]
		{
			new BuildingAttachPoint.HardPoint(new CellOffset(0, 5), GameTags.Rocket, null)
		};
	}

	// Token: 0x06001676 RID: 5750 RVA: 0x0007FB84 File Offset: 0x0007DD84
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
	}

	// Token: 0x06001677 RID: 5751 RVA: 0x0007FB86 File Offset: 0x0007DD86
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
	}

	// Token: 0x06001678 RID: 5752 RVA: 0x0007FB88 File Offset: 0x0007DD88
	public override void DoPostConfigureComplete(GameObject go)
	{
		RocketEngine rocketEngine = go.AddOrGet<RocketEngine>();
		rocketEngine.fuelTag = ElementLoader.FindElementByHash(SimHashes.Steam).tag;
		rocketEngine.efficiency = ROCKETRY.ENGINE_EFFICIENCY.WEAK;
		rocketEngine.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
		rocketEngine.requireOxidizer = false;
		rocketEngine.exhaustElement = SimHashes.Steam;
		rocketEngine.exhaustTemperature = ElementLoader.FindElementByHash(SimHashes.Steam).lowTemp + 50f;
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = TUNING.BUILDINGS.ROCKETRY_MASS_KG.FUEL_TANK_WET_MASS[0];
		storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Hide,
			Storage.StoredItemModifier.Seal,
			Storage.StoredItemModifier.Insulate
		});
		FuelTank fuelTank = go.AddOrGet<FuelTank>();
		fuelTank.consumeFuelOnLand = !DlcManager.FeatureClusterSpaceEnabled();
		fuelTank.storage = storage;
		fuelTank.FuelType = ElementLoader.FindElementByHash(SimHashes.Steam).tag;
		fuelTank.physicalFuelCapacity = storage.capacityKg;
		go.AddOrGet<CopyBuildingSettings>();
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Gas;
		conduitConsumer.consumptionRate = 10f;
		conduitConsumer.capacityTag = fuelTank.FuelType;
		conduitConsumer.capacityKG = storage.capacityKg;
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		BuildingTemplates.ExtendBuildingToRocketModule(go, "rocket_steam_engine_bg_kanim", false);
	}

	// Token: 0x04000D57 RID: 3415
	public const string ID = "SteamEngine";
}
