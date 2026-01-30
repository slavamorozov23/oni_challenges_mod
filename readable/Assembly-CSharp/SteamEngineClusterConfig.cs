using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000439 RID: 1081
public class SteamEngineClusterConfig : IBuildingConfig
{
	// Token: 0x0600166C RID: 5740 RVA: 0x0007F765 File Offset: 0x0007D965
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x0600166D RID: 5741 RVA: 0x0007F76C File Offset: 0x0007D96C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SteamEngineCluster";
		int width = 7;
		int height = 5;
		string anim = "rocket_cluster_steam_engine_kanim";
		int hitpoints = 1000;
		float construction_time = 60f;
		float[] dense_TIER = TUNING.BUILDINGS.ROCKETRY_MASS_KG.DENSE_TIER0;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, dense_TIER, refined_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, tier, 0.2f);
		BuildingTemplates.CreateRocketBuildingDef(buildingDef);
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.AttachmentSlotTag = GameTags.Rocket;
		buildingDef.attachablePosition = new CellOffset(0, 0);
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.UtilityInputOffset = new CellOffset(2, 3);
		buildingDef.InputConduitType = ConduitType.Gas;
		buildingDef.GeneratorWattageRating = 600f;
		buildingDef.GeneratorBaseCapacity = buildingDef.GeneratorWattageRating;
		buildingDef.RequiresPowerInput = false;
		buildingDef.RequiresPowerOutput = false;
		buildingDef.CanMove = true;
		buildingDef.Cancellable = false;
		buildingDef.ShowInBuildMenu = false;
		buildingDef.AddSearchTerms(SEARCH_TERMS.STEAM);
		return buildingDef;
	}

	// Token: 0x0600166E RID: 5742 RVA: 0x0007F854 File Offset: 0x0007DA54
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

	// Token: 0x0600166F RID: 5743 RVA: 0x0007F8B8 File Offset: 0x0007DAB8
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
	}

	// Token: 0x06001670 RID: 5744 RVA: 0x0007F8BA File Offset: 0x0007DABA
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
	}

	// Token: 0x06001671 RID: 5745 RVA: 0x0007F8BC File Offset: 0x0007DABC
	public override void DoPostConfigureComplete(GameObject go)
	{
		RocketEngineCluster rocketEngineCluster = go.AddOrGet<RocketEngineCluster>();
		rocketEngineCluster.maxModules = 6;
		rocketEngineCluster.maxHeight = ROCKETRY.ROCKET_HEIGHT.TALL;
		rocketEngineCluster.fuelTag = ElementLoader.FindElementByHash(SimHashes.Steam).tag;
		rocketEngineCluster.efficiency = ROCKETRY.ENGINE_EFFICIENCY.WEAK;
		rocketEngineCluster.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
		rocketEngineCluster.requireOxidizer = false;
		rocketEngineCluster.exhaustElement = SimHashes.Steam;
		rocketEngineCluster.exhaustTemperature = ElementLoader.FindElementByHash(SimHashes.Steam).lowTemp + 50f;
		go.AddOrGet<ModuleGenerator>();
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = TUNING.BUILDINGS.ROCKETRY_MASS_KG.FUEL_TANK_WET_MASS_GAS_LARGE[0];
		storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Hide,
			Storage.StoredItemModifier.Seal,
			Storage.StoredItemModifier.Insulate
		});
		FuelTank fuelTank = go.AddOrGet<FuelTank>();
		fuelTank.consumeFuelOnLand = false;
		fuelTank.storage = storage;
		fuelTank.FuelType = ElementLoader.FindElementByHash(SimHashes.Steam).tag;
		fuelTank.targetFillMass = storage.capacityKg;
		fuelTank.physicalFuelCapacity = storage.capacityKg;
		go.AddOrGet<CopyBuildingSettings>();
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Gas;
		conduitConsumer.consumptionRate = 10f;
		conduitConsumer.capacityTag = fuelTank.FuelType;
		conduitConsumer.capacityKG = storage.capacityKg;
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		BuildingTemplates.ExtendBuildingToRocketModuleCluster(go, null, ROCKETRY.BURDEN.MONUMENTAL, (float)ROCKETRY.ENGINE_POWER.MID_WEAK, ROCKETRY.FUEL_COST_PER_DISTANCE.GAS_VERY_LOW);
		go.GetComponent<KPrefabID>().prefabInitFn += delegate(GameObject inst)
		{
		};
	}

	// Token: 0x04000D56 RID: 3414
	public const string ID = "SteamEngineCluster";
}
