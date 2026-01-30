using System;
using TUNING;
using UnityEngine;

// Token: 0x0200001D RID: 29
public class ArtifactAnalysisStationConfig : IBuildingConfig
{
	// Token: 0x06000082 RID: 130 RVA: 0x00005834 File Offset: 0x00003A34
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000083 RID: 131 RVA: 0x0000583C File Offset: 0x00003A3C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "ArtifactAnalysisStation";
		int width = 4;
		int height = 4;
		string anim = "artifact_analysis_kanim";
		int hitpoints = 30;
		float construction_time = 60f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 2400f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER6;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER2, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 480f;
		buildingDef.SelfHeatKilowattsWhenActive = 1f;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.AudioSize = "large";
		buildingDef.RequiredSkillPerkID = Db.Get().SkillPerks.CanStudyArtifact.Id;
		return buildingDef;
	}

	// Token: 0x06000084 RID: 132 RVA: 0x000058DC File Offset: 0x00003ADC
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<DropAllWorkable>();
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		go.AddOrGetDef<ArtifactAnalysisStation.Def>();
		go.AddOrGet<ArtifactAnalysisStationWorkable>();
		Prioritizable.AddRef(go);
		Storage storage = go.AddOrGet<Storage>();
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
		manualDeliveryKG.RequestedItemTag = GameTags.CharmedArtifact;
		manualDeliveryKG.refillMass = 1f * ArtifactConfig.ARTIFACT_MASS;
		manualDeliveryKG.MinimumMass = 1f * ArtifactConfig.ARTIFACT_MASS;
		manualDeliveryKG.capacity = 1f * ArtifactConfig.ARTIFACT_MASS;
	}

	// Token: 0x06000085 RID: 133 RVA: 0x0000597B File Offset: 0x00003B7B
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x0400006F RID: 111
	public const string ID = "ArtifactAnalysisStation";

	// Token: 0x04000070 RID: 112
	public const float WORK_TIME = 150f;
}
