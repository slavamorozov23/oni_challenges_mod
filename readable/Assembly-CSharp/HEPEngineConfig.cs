using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200025D RID: 605
public class HEPEngineConfig : IBuildingConfig
{
	// Token: 0x06000C48 RID: 3144 RVA: 0x00049B12 File Offset: 0x00047D12
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000C49 RID: 3145 RVA: 0x00049B1C File Offset: 0x00047D1C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "HEPEngine";
		int width = 5;
		int height = 5;
		string anim = "rocket_hep_engine_kanim";
		int hitpoints = 1000;
		float construction_time = 60f;
		float[] engine_MASS_LARGE = TUNING.BUILDINGS.ROCKETRY_MASS_KG.ENGINE_MASS_LARGE;
		string[] construction_materials = new string[]
		{
			SimHashes.Steel.ToString()
		};
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, engine_MASS_LARGE, construction_materials, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, tier, 0.2f);
		BuildingTemplates.CreateRocketBuildingDef(buildingDef);
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.AttachmentSlotTag = GameTags.Rocket;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.attachablePosition = new CellOffset(0, 0);
		buildingDef.RequiresPowerInput = false;
		buildingDef.RequiresPowerOutput = false;
		buildingDef.CanMove = true;
		buildingDef.Cancellable = false;
		buildingDef.ShowInBuildMenu = false;
		buildingDef.UseHighEnergyParticleInputPort = true;
		buildingDef.HighEnergyParticleInputOffset = new CellOffset(0, 3);
		buildingDef.DiseaseCellVisName = "RadiationSickness";
		buildingDef.UtilityOutputOffset = buildingDef.HighEnergyParticleInputOffset;
		buildingDef.LogicOutputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort("HEP_STORAGE", new CellOffset(0, 2), STRINGS.BUILDINGS.PREFABS.HEPENGINE.LOGIC_PORT_STORAGE, STRINGS.BUILDINGS.PREFABS.HEPENGINE.LOGIC_PORT_STORAGE_ACTIVE, STRINGS.BUILDINGS.PREFABS.HEPENGINE.LOGIC_PORT_STORAGE_INACTIVE, false, false)
		};
		return buildingDef;
	}

	// Token: 0x06000C4A RID: 3146 RVA: 0x00049C50 File Offset: 0x00047E50
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

	// Token: 0x06000C4B RID: 3147 RVA: 0x00049CB4 File Offset: 0x00047EB4
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
	}

	// Token: 0x06000C4C RID: 3148 RVA: 0x00049CB6 File Offset: 0x00047EB6
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
	}

	// Token: 0x06000C4D RID: 3149 RVA: 0x00049CB8 File Offset: 0x00047EB8
	public override void DoPostConfigureComplete(GameObject go)
	{
		RadiationEmitter radiationEmitter = go.AddOrGet<RadiationEmitter>();
		radiationEmitter.emitType = RadiationEmitter.RadiationEmitterType.Constant;
		radiationEmitter.emitRadiusX = 10;
		radiationEmitter.emitRadiusY = 10;
		radiationEmitter.emitRads = 8400f / ((float)radiationEmitter.emitRadiusX / 6f);
		radiationEmitter.emissionOffset = new Vector3(0f, 3f, 0f);
		HighEnergyParticleStorage highEnergyParticleStorage = go.AddOrGet<HighEnergyParticleStorage>();
		highEnergyParticleStorage.capacity = 4000f;
		highEnergyParticleStorage.autoStore = true;
		highEnergyParticleStorage.PORT_ID = "HEP_STORAGE";
		highEnergyParticleStorage.showCapacityStatusItem = true;
		go.AddOrGet<HEPFuelTank>().physicalFuelCapacity = 4000f;
		RocketEngineCluster rocketEngineCluster = go.AddOrGet<RocketEngineCluster>();
		rocketEngineCluster.maxModules = 4;
		rocketEngineCluster.maxHeight = ROCKETRY.ROCKET_HEIGHT.MEDIUM;
		rocketEngineCluster.efficiency = ROCKETRY.ENGINE_EFFICIENCY.STRONG;
		rocketEngineCluster.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
		rocketEngineCluster.requireOxidizer = false;
		rocketEngineCluster.exhaustElement = SimHashes.Fallout;
		rocketEngineCluster.exhaustTemperature = 873.15f;
		rocketEngineCluster.exhaustEmitRate = 25f;
		rocketEngineCluster.exhaustDiseaseIdx = Db.Get().Diseases.GetIndex(Db.Get().Diseases.RadiationPoisoning.Id);
		rocketEngineCluster.exhaustDiseaseCount = 100000;
		rocketEngineCluster.emitRadiation = true;
		rocketEngineCluster.fuelTag = GameTags.HighEnergyParticle;
		BuildingTemplates.ExtendBuildingToRocketModuleCluster(go, null, ROCKETRY.BURDEN.MODERATE_PLUS, (float)ROCKETRY.ENGINE_POWER.LATE_STRONG, ROCKETRY.FUEL_COST_PER_DISTANCE.PARTICLES);
		go.GetComponent<KPrefabID>().prefabInitFn += delegate(GameObject inst)
		{
			inst.GetComponent<RadiationEmitter>().SetEmitting(false);
		};
	}

	// Token: 0x0400086E RID: 2158
	private const int PARTICLES_PER_HEX = 200;

	// Token: 0x0400086F RID: 2159
	private const int RANGE = 20;

	// Token: 0x04000870 RID: 2160
	private const int PARTICLE_STORAGE_CAPACITY = 4000;

	// Token: 0x04000871 RID: 2161
	private const int PORT_OFFSET_Y = 3;

	// Token: 0x04000872 RID: 2162
	public const string ID = "HEPEngine";

	// Token: 0x04000873 RID: 2163
	public const string PORT_ID = "HEP_STORAGE";
}
