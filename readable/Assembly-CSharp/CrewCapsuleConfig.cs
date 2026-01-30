using System;
using TUNING;
using UnityEngine;

// Token: 0x02000068 RID: 104
public class CrewCapsuleConfig : IBuildingConfig
{
	// Token: 0x060001F4 RID: 500 RVA: 0x0000E264 File Offset: 0x0000C464
	public override BuildingDef CreateBuildingDef()
	{
		string id = "CrewCapsule";
		int width = 5;
		int height = 19;
		string anim = "rocket_small_steam_kanim";
		int hitpoints = 1000;
		float construction_time = 480f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER7;
		string[] construction_materials = new string[]
		{
			SimHashes.Steel.ToString()
		};
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.BuildingAttachPoint;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.AttachmentSlotTag = GameTags.Rocket;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.UtilityInputOffset = new CellOffset(2, 6);
		buildingDef.InputConduitType = ConduitType.Gas;
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 10f;
		buildingDef.Deprecated = true;
		return buildingDef;
	}

	// Token: 0x060001F5 RID: 501 RVA: 0x0000E31F File Offset: 0x0000C51F
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.AddOrGet<LaunchConditionManager>();
		go.AddOrGet<RocketLaunchConditionVisualizer>();
	}

	// Token: 0x060001F6 RID: 502 RVA: 0x0000E338 File Offset: 0x0000C538
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddComponent<Storage>();
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Gas;
		conduitConsumer.consumptionRate = 1f;
		conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Oxygen).tag;
		conduitConsumer.capacityKG = 10f;
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
	}

	// Token: 0x04000138 RID: 312
	public const string ID = "CrewCapsule";
}
