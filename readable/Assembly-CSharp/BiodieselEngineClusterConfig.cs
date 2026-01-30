using System;
using TUNING;
using UnityEngine;

// Token: 0x0200002C RID: 44
public class BiodieselEngineClusterConfig : IBuildingConfig
{
	// Token: 0x060000CB RID: 203 RVA: 0x00006F28 File Offset: 0x00005128
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060000CC RID: 204 RVA: 0x00006F30 File Offset: 0x00005130
	public override BuildingDef CreateBuildingDef()
	{
		string id = "BiodieselEngineCluster";
		int width = 7;
		int height = 5;
		string anim = "rocket_biodiesel_engine_kanim";
		int hitpoints = 1000;
		float construction_time = 60f;
		float[] engine_MASS_SMALL = BUILDINGS.ROCKETRY_MASS_KG.ENGINE_MASS_SMALL;
		string[] construction_materials = new string[]
		{
			SimHashes.Steel.ToString()
		};
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, engine_MASS_SMALL, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier, 0.2f);
		BuildingTemplates.CreateRocketBuildingDef(buildingDef);
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.AttachmentSlotTag = GameTags.Rocket;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.attachablePosition = new CellOffset(0, 0);
		buildingDef.GeneratorWattageRating = 640f;
		buildingDef.GeneratorBaseCapacity = buildingDef.GeneratorWattageRating;
		buildingDef.RequiresPowerInput = false;
		buildingDef.RequiresPowerOutput = false;
		buildingDef.CanMove = true;
		buildingDef.Cancellable = false;
		buildingDef.ShowInBuildMenu = false;
		return buildingDef;
	}

	// Token: 0x060000CD RID: 205 RVA: 0x0000700C File Offset: 0x0000520C
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

	// Token: 0x060000CE RID: 206 RVA: 0x00007070 File Offset: 0x00005270
	public override void DoPostConfigureComplete(GameObject go)
	{
		RocketEngineCluster rocketEngineCluster = go.AddOrGet<RocketEngineCluster>();
		rocketEngineCluster.maxModules = 7;
		rocketEngineCluster.maxHeight = ROCKETRY.ROCKET_HEIGHT.VERY_TALL;
		rocketEngineCluster.fuelTag = SimHashes.RefinedLipid.CreateTag();
		rocketEngineCluster.efficiency = ROCKETRY.ENGINE_EFFICIENCY.STRONG;
		rocketEngineCluster.requireOxidizer = true;
		rocketEngineCluster.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
		rocketEngineCluster.exhaustElement = SimHashes.CarbonDioxide;
		rocketEngineCluster.exhaustTemperature = 1263.15f;
		go.AddOrGet<ModuleGenerator>();
		BuildingTemplates.ExtendBuildingToRocketModuleCluster(go, null, ROCKETRY.BURDEN.MAJOR_PLUS, (float)ROCKETRY.ENGINE_POWER.MID_STRONG, ROCKETRY.FUEL_COST_PER_DISTANCE.HIGHER);
		go.GetComponent<KPrefabID>().prefabInitFn += delegate(GameObject inst)
		{
		};
	}

	// Token: 0x0400008E RID: 142
	public const string ID = "BiodieselEngineCluster";

	// Token: 0x0400008F RID: 143
	public const SimHashes FUEL = SimHashes.RefinedLipid;
}
