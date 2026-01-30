using System;
using TUNING;
using UnityEngine;

// Token: 0x0200041D RID: 1053
public class SolarPanelModuleConfig : IBuildingConfig
{
	// Token: 0x060015BB RID: 5563 RVA: 0x0007C4EC File Offset: 0x0007A6EC
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060015BC RID: 5564 RVA: 0x0007C4F4 File Offset: 0x0007A6F4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SolarPanelModule";
		int width = 3;
		int height = 1;
		string anim = "rocket_solar_panel_module_kanim";
		int hitpoints = 1000;
		float construction_time = 30f;
		float[] hollow_TIER = BUILDINGS.ROCKETRY_MASS_KG.HOLLOW_TIER1;
		string[] glasses = MATERIALS.GLASSES;
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, hollow_TIER, glasses, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier, 0.2f);
		BuildingTemplates.CreateRocketBuildingDef(buildingDef);
		buildingDef.DefaultAnimState = "grounded";
		buildingDef.AttachmentSlotTag = GameTags.Rocket;
		buildingDef.GeneratorWattageRating = 60f;
		buildingDef.GeneratorBaseCapacity = buildingDef.GeneratorWattageRating;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.SelfHeatKilowattsWhenActive = 0f;
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.ForegroundLayer = Grid.SceneLayer.Front;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.PowerInputOffset = SolarPanelModuleConfig.PLUG_OFFSET;
		buildingDef.PowerOutputOffset = SolarPanelModuleConfig.PLUG_OFFSET;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.RequiresPowerOutput = true;
		buildingDef.UseWhitePowerOutputConnectorColour = true;
		buildingDef.CanMove = true;
		buildingDef.Cancellable = false;
		return buildingDef;
	}

	// Token: 0x060015BD RID: 5565 RVA: 0x0007C5E4 File Offset: 0x0007A7E4
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddComponent<RequireInputs>();
		go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[]
		{
			new BuildingAttachPoint.HardPoint(new CellOffset(0, 1), GameTags.Rocket, null)
		};
		go.AddComponent<PartialLightBlocking>();
	}

	// Token: 0x060015BE RID: 5566 RVA: 0x0007C656 File Offset: 0x0007A856
	public override void DoPostConfigureComplete(GameObject go)
	{
		Prioritizable.AddRef(go);
		go.AddOrGet<ModuleSolarPanel>().showConnectedConsumerStatusItems = false;
		BuildingTemplates.ExtendBuildingToRocketModuleCluster(go, null, ROCKETRY.BURDEN.INSIGNIFICANT, 0f, 0f);
		go.GetComponent<RocketModule>().operationalLandedRequired = false;
	}

	// Token: 0x04000CF8 RID: 3320
	public const string ID = "SolarPanelModule";

	// Token: 0x04000CF9 RID: 3321
	private static readonly CellOffset PLUG_OFFSET = new CellOffset(-1, 0);

	// Token: 0x04000CFA RID: 3322
	private const float EFFICIENCY_RATIO = 0.75f;

	// Token: 0x04000CFB RID: 3323
	public const float MAX_WATTS = 60f;
}
