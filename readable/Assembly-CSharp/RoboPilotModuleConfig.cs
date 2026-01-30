using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003E6 RID: 998
public class RoboPilotModuleConfig : IBuildingConfig
{
	// Token: 0x0600147F RID: 5247 RVA: 0x00074686 File Offset: 0x00072886
	public override string[] GetRequiredDlcIds()
	{
		return new string[]
		{
			"EXPANSION1_ID",
			"DLC3_ID"
		};
	}

	// Token: 0x06001480 RID: 5248 RVA: 0x000746A0 File Offset: 0x000728A0
	public override BuildingDef CreateBuildingDef()
	{
		string id = "RoboPilotModule";
		int width = 3;
		int height = 4;
		string anim = "robot_rocket_control_station_kanim";
		int hitpoints = 1000;
		float construction_time = 30f;
		float[] hollow_TIER = TUNING.BUILDINGS.ROCKETRY_MASS_KG.HOLLOW_TIER1;
		string[] construction_materials = new string[]
		{
			SimHashes.Steel.ToString()
		};
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, hollow_TIER, construction_materials, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, tier, 0.2f);
		BuildingTemplates.CreateRocketBuildingDef(buildingDef);
		buildingDef.DefaultAnimState = "grounded";
		buildingDef.AttachmentSlotTag = GameTags.Rocket;
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.RequiresPowerInput = false;
		buildingDef.CanMove = true;
		buildingDef.Cancellable = false;
		buildingDef.AddSearchTerms(SEARCH_TERMS.ROBOT);
		return buildingDef;
	}

	// Token: 0x06001481 RID: 5249 RVA: 0x00074764 File Offset: 0x00072964
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[]
		{
			new BuildingAttachPoint.HardPoint(new CellOffset(0, 4), GameTags.Rocket, null)
		};
	}

	// Token: 0x06001482 RID: 5250 RVA: 0x000747C8 File Offset: 0x000729C8
	public override void DoPostConfigureComplete(GameObject go)
	{
		Prioritizable.AddRef(go);
		go.GetComponent<KPrefabID>().AddTag(GameTags.LaunchButtonRocketModule, false);
		go.AddOrGet<RoboPilotModule>();
		go.AddOrGet<LaunchableRocketCluster>();
		go.AddOrGet<RobotCommandConditions>();
		go.AddOrGet<RocketProcessConditionDisplayTarget>();
		go.AddOrGet<RocketLaunchConditionVisualizer>();
		Storage storage = go.AddComponent<Storage>();
		storage.showInUI = true;
		storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
		storage.capacityKg = 100f;
		ManualDeliveryKG manualDeliveryKG = go.AddComponent<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
		manualDeliveryKG.capacity = storage.capacityKg;
		manualDeliveryKG.refillMass = 20f;
		manualDeliveryKG.RequestedItemTag = DatabankHelper.TAG;
		manualDeliveryKG.MinimumMass = 1f;
		BuildingTemplates.ExtendBuildingToRocketModuleCluster(go, null, ROCKETRY.BURDEN.MODERATE, 0f, 0f);
		go.GetComponent<ReorderableBuilding>().buildConditions.Add(new LimitOneRoboPilotModule());
	}

	// Token: 0x04000C66 RID: 3174
	public const string ID = "RoboPilotModule";
}
