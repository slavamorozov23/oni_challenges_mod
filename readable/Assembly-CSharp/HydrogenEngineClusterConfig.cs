using System;
using TUNING;
using UnityEngine;

// Token: 0x02000267 RID: 615
public class HydrogenEngineClusterConfig : IBuildingConfig
{
	// Token: 0x06000C7F RID: 3199 RVA: 0x0004B3CE File Offset: 0x000495CE
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000C80 RID: 3200 RVA: 0x0004B3D8 File Offset: 0x000495D8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "HydrogenEngineCluster";
		int width = 7;
		int height = 5;
		string anim = "rocket_cluster_hydrogen_engine_kanim";
		int hitpoints = 1000;
		float construction_time = 60f;
		float[] engine_MASS_LARGE = BUILDINGS.ROCKETRY_MASS_KG.ENGINE_MASS_LARGE;
		string[] construction_materials = new string[]
		{
			SimHashes.Steel.ToString()
		};
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, engine_MASS_LARGE, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier, 0.2f);
		BuildingTemplates.CreateRocketBuildingDef(buildingDef);
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.AttachmentSlotTag = GameTags.Rocket;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.attachablePosition = new CellOffset(0, 0);
		buildingDef.GeneratorWattageRating = 600f;
		buildingDef.GeneratorBaseCapacity = buildingDef.GeneratorWattageRating;
		buildingDef.RequiresPowerInput = false;
		buildingDef.RequiresPowerOutput = false;
		buildingDef.CanMove = true;
		buildingDef.Cancellable = false;
		buildingDef.ShowInBuildMenu = false;
		return buildingDef;
	}

	// Token: 0x06000C81 RID: 3201 RVA: 0x0004B4B4 File Offset: 0x000496B4
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

	// Token: 0x06000C82 RID: 3202 RVA: 0x0004B518 File Offset: 0x00049718
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
	}

	// Token: 0x06000C83 RID: 3203 RVA: 0x0004B51A File Offset: 0x0004971A
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
	}

	// Token: 0x06000C84 RID: 3204 RVA: 0x0004B51C File Offset: 0x0004971C
	public override void DoPostConfigureComplete(GameObject go)
	{
		RocketEngineCluster rocketEngineCluster = go.AddOrGet<RocketEngineCluster>();
		rocketEngineCluster.maxModules = 7;
		rocketEngineCluster.maxHeight = ROCKETRY.ROCKET_HEIGHT.VERY_TALL;
		rocketEngineCluster.fuelTag = ElementLoader.FindElementByHash(SimHashes.LiquidHydrogen).tag;
		rocketEngineCluster.efficiency = ROCKETRY.ENGINE_EFFICIENCY.STRONG;
		rocketEngineCluster.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
		rocketEngineCluster.exhaustElement = SimHashes.Steam;
		rocketEngineCluster.exhaustTemperature = 2000f;
		go.AddOrGet<ModuleGenerator>();
		BuildingTemplates.ExtendBuildingToRocketModuleCluster(go, null, ROCKETRY.BURDEN.MAJOR_PLUS, (float)ROCKETRY.ENGINE_POWER.LATE_VERY_STRONG, ROCKETRY.FUEL_COST_PER_DISTANCE.HIGH);
		go.GetComponent<KPrefabID>().prefabInitFn += delegate(GameObject inst)
		{
		};
	}

	// Token: 0x0400089F RID: 2207
	public const string ID = "HydrogenEngineCluster";
}
