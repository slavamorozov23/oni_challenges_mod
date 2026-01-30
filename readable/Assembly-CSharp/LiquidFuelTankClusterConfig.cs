using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x0200029F RID: 671
public class LiquidFuelTankClusterConfig : IBuildingConfig
{
	// Token: 0x06000DA8 RID: 3496 RVA: 0x00050CA7 File Offset: 0x0004EEA7
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000DA9 RID: 3497 RVA: 0x00050CB0 File Offset: 0x0004EEB0
	public override BuildingDef CreateBuildingDef()
	{
		string id = "LiquidFuelTankCluster";
		int width = 5;
		int height = 5;
		string anim = "rocket_cluster_liquid_fuel_tank_kanim";
		int hitpoints = 1000;
		float construction_time = 60f;
		float[] fuel_TANK_DRY_MASS = BUILDINGS.ROCKETRY_MASS_KG.FUEL_TANK_DRY_MASS;
		string[] construction_materials = new string[]
		{
			SimHashes.Steel.ToString()
		};
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, fuel_TANK_DRY_MASS, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier, 0.2f);
		BuildingTemplates.CreateRocketBuildingDef(buildingDef);
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.AttachmentSlotTag = GameTags.Rocket;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.UtilityInputOffset = new CellOffset(2, 3);
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.RequiresPowerInput = false;
		buildingDef.attachablePosition = new CellOffset(0, 0);
		buildingDef.CanMove = true;
		buildingDef.Cancellable = false;
		buildingDef.ShowInBuildMenu = false;
		return buildingDef;
	}

	// Token: 0x06000DAA RID: 3498 RVA: 0x00050D80 File Offset: 0x0004EF80
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[]
		{
			new BuildingAttachPoint.HardPoint(new CellOffset(0, 5), GameTags.Rocket, null)
		};
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
	}

	// Token: 0x06000DAB RID: 3499 RVA: 0x00050DE4 File Offset: 0x0004EFE4
	public override void DoPostConfigureComplete(GameObject go)
	{
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = BUILDINGS.ROCKETRY_MASS_KG.FUEL_TANK_WET_MASS[0];
		storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Hide,
			Storage.StoredItemModifier.Seal,
			Storage.StoredItemModifier.Insulate
		});
		FuelTank fuelTank = go.AddOrGet<FuelTank>();
		fuelTank.consumeFuelOnLand = !DlcManager.FeatureClusterSpaceEnabled();
		fuelTank.storage = storage;
		fuelTank.physicalFuelCapacity = storage.capacityKg;
		go.AddOrGet<CopyBuildingSettings>();
		go.AddOrGet<DropToUserCapacity>();
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.refillMass = storage.capacityKg;
		manualDeliveryKG.capacity = storage.capacityKg;
		manualDeliveryKG.operationalRequirement = Operational.State.None;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.consumptionRate = 10f;
		conduitConsumer.capacityTag = GameTags.Liquid;
		conduitConsumer.capacityKG = storage.capacityKg;
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Store;
		BuildingTemplates.ExtendBuildingToRocketModuleCluster(go, null, ROCKETRY.BURDEN.MODERATE_PLUS, 0f, 0f);
		storage.showUnreachableStatus = false;
		go.GetComponent<KPrefabID>().prefabInitFn += delegate(GameObject inst)
		{
			Element element = ElementLoader.FindElementByHash(SimHashes.LiquidOxygen);
			if (!DiscoveredResources.Instance.IsDiscovered(element.tag))
			{
				DiscoveredResources.Instance.Discover(element.tag, element.GetMaterialCategoryTag());
			}
		};
	}

	// Token: 0x04000933 RID: 2355
	public const string ID = "LiquidFuelTankCluster";

	// Token: 0x04000934 RID: 2356
	public const float FuelCapacity = 900f;
}
