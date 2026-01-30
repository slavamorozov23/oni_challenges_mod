using System;
using TUNING;
using UnityEngine;

// Token: 0x02000268 RID: 616
public class HydrogenEngineConfig : IBuildingConfig
{
	// Token: 0x06000C86 RID: 3206 RVA: 0x0004B5D2 File Offset: 0x000497D2
	public override string[] GetForbiddenDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000C87 RID: 3207 RVA: 0x0004B5DC File Offset: 0x000497DC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "HydrogenEngine";
		int width = 7;
		int height = 5;
		string anim = "rocket_hydrogen_engine_kanim";
		int hitpoints = 1000;
		float construction_time = 60f;
		float[] engine_MASS_LARGE = BUILDINGS.ROCKETRY_MASS_KG.ENGINE_MASS_LARGE;
		string[] construction_materials = new string[]
		{
			SimHashes.Steel.ToString()
		};
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, engine_MASS_LARGE, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier, 0.2f);
		BuildingTemplates.CreateRocketBuildingDef(buildingDef);
		buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.AttachmentSlotTag = GameTags.Rocket;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.attachablePosition = new CellOffset(0, 0);
		buildingDef.RequiresPowerInput = false;
		buildingDef.CanMove = true;
		return buildingDef;
	}

	// Token: 0x06000C88 RID: 3208 RVA: 0x0004B68C File Offset: 0x0004988C
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

	// Token: 0x06000C89 RID: 3209 RVA: 0x0004B6F0 File Offset: 0x000498F0
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
	}

	// Token: 0x06000C8A RID: 3210 RVA: 0x0004B6F2 File Offset: 0x000498F2
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
	}

	// Token: 0x06000C8B RID: 3211 RVA: 0x0004B6F4 File Offset: 0x000498F4
	public override void DoPostConfigureComplete(GameObject go)
	{
		RocketEngine rocketEngine = go.AddOrGet<RocketEngine>();
		rocketEngine.fuelTag = ElementLoader.FindElementByHash(SimHashes.LiquidHydrogen).tag;
		rocketEngine.efficiency = ROCKETRY.ENGINE_EFFICIENCY.STRONG;
		rocketEngine.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
		rocketEngine.exhaustElement = SimHashes.Steam;
		rocketEngine.exhaustTemperature = 2000f;
		BuildingTemplates.ExtendBuildingToRocketModule(go, "rocket_hydrogen_engine_bg_kanim", false);
	}

	// Token: 0x040008A0 RID: 2208
	public const string ID = "HydrogenEngine";
}
