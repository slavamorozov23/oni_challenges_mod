using System;
using TUNING;
using UnityEngine;

// Token: 0x02000073 RID: 115
public class DevHEPSpawnerConfig : IBuildingConfig
{
	// Token: 0x0600022B RID: 555 RVA: 0x0000F752 File Offset: 0x0000D952
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x0600022C RID: 556 RVA: 0x0000F75C File Offset: 0x0000D95C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "DevHEPSpawner";
		int width = 1;
		int height = 1;
		string anim = "dev_radbolt_generator_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.NotInTiles;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Invincible = true;
		buildingDef.Overheatable = false;
		buildingDef.Entombable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = OverlayModes.Radiation.ID;
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.UseHighEnergyParticleOutputPort = true;
		buildingDef.HighEnergyParticleOutputOffset = new CellOffset(0, 0);
		buildingDef.RequiresPowerInput = false;
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.RadiationIDs, "DevHEPSpawner");
		buildingDef.Deprecated = !Sim.IsRadiationEnabled();
		buildingDef.DebugOnly = true;
		return buildingDef;
	}

	// Token: 0x0600022D RID: 557 RVA: 0x0000F834 File Offset: 0x0000DA34
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddTag(GameTags.DevBuilding);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		Prioritizable.AddRef(go);
		go.AddOrGet<LoopingSounds>();
		go.AddOrGet<DevHEPSpawner>().boltAmount = 50f;
		go.AddOrGet<LogicOperationalController>();
	}

	// Token: 0x0600022E RID: 558 RVA: 0x0000F885 File Offset: 0x0000DA85
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000162 RID: 354
	public const string ID = "DevHEPSpawner";
}
