using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000264 RID: 612
public class HighEnergyParticleSpawnerConfig : IBuildingConfig
{
	// Token: 0x06000C71 RID: 3185 RVA: 0x0004AD50 File Offset: 0x00048F50
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000C72 RID: 3186 RVA: 0x0004AD58 File Offset: 0x00048F58
	public override BuildingDef CreateBuildingDef()
	{
		string id = "HighEnergyParticleSpawner";
		int width = 1;
		int height = 2;
		string anim = "radiation_collector_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.NotInTiles;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = OverlayModes.Radiation.ID;
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.UseHighEnergyParticleOutputPort = true;
		buildingDef.HighEnergyParticleOutputOffset = new CellOffset(0, 1);
		buildingDef.RequiresPowerInput = true;
		buildingDef.PowerInputOffset = new CellOffset(0, 0);
		buildingDef.EnergyConsumptionWhenActive = 480f;
		buildingDef.ExhaustKilowattsWhenActive = 1f;
		buildingDef.SelfHeatKilowattsWhenActive = 4f;
		buildingDef.DiseaseCellVisName = "RadiationSickness";
		buildingDef.UtilityOutputOffset = CellOffset.none;
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.RadiationIDs, "HighEnergyParticleSpawner");
		buildingDef.Deprecated = !Sim.IsRadiationEnabled();
		buildingDef.AddSearchTerms(SEARCH_TERMS.POWER);
		buildingDef.AddSearchTerms(SEARCH_TERMS.GENERATOR);
		return buildingDef;
	}

	// Token: 0x06000C73 RID: 3187 RVA: 0x0004AE68 File Offset: 0x00049068
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		Prioritizable.AddRef(go);
		go.AddOrGet<HighEnergyParticleStorage>().capacity = 500f;
		go.AddOrGet<LoopingSounds>();
		HighEnergyParticleSpawner highEnergyParticleSpawner = go.AddOrGet<HighEnergyParticleSpawner>();
		highEnergyParticleSpawner.minLaunchInterval = 2f;
		highEnergyParticleSpawner.radiationSampleRate = 0.2f;
		highEnergyParticleSpawner.minSlider = 50;
		highEnergyParticleSpawner.maxSlider = 500;
	}

	// Token: 0x06000C74 RID: 3188 RVA: 0x0004AED5 File Offset: 0x000490D5
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000889 RID: 2185
	public const string ID = "HighEnergyParticleSpawner";

	// Token: 0x0400088A RID: 2186
	public const float MIN_LAUNCH_INTERVAL = 2f;

	// Token: 0x0400088B RID: 2187
	public const float RADIATION_SAMPLE_RATE = 0.2f;

	// Token: 0x0400088C RID: 2188
	public const float HEP_PER_RAD = 0.1f;

	// Token: 0x0400088D RID: 2189
	public const int MIN_SLIDER = 50;

	// Token: 0x0400088E RID: 2190
	public const int MAX_SLIDER = 500;

	// Token: 0x0400088F RID: 2191
	public const float DISABLED_CONSUMPTION_RATE = 0.05f;
}
