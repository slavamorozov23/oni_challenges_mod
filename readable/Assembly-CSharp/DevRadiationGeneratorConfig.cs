using System;
using TUNING;
using UnityEngine;

// Token: 0x0200007A RID: 122
public class DevRadiationGeneratorConfig : IBuildingConfig
{
	// Token: 0x0600024A RID: 586 RVA: 0x00010090 File Offset: 0x0000E290
	public override BuildingDef CreateBuildingDef()
	{
		string id = "DevRadiationGenerator";
		int width = 1;
		int height = 1;
		string anim = "dev_generator_kanim";
		int hitpoints = 100;
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER2, tier2, 0.2f);
		buildingDef.ViewMode = OverlayModes.Radiation.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.AudioSize = "large";
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.DebugOnly = true;
		return buildingDef;
	}

	// Token: 0x0600024B RID: 587 RVA: 0x0001010C File Offset: 0x0000E30C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddTag(GameTags.DevBuilding);
		RadiationEmitter radiationEmitter = go.AddOrGet<RadiationEmitter>();
		radiationEmitter.emitType = RadiationEmitter.RadiationEmitterType.Constant;
		radiationEmitter.radiusProportionalToRads = false;
		radiationEmitter.emitRadiusX = 12;
		radiationEmitter.emitRadiusY = 12;
		radiationEmitter.emitRads = 2400f / ((float)radiationEmitter.emitRadiusX / 6f);
		go.AddOrGet<DevRadiationEmitter>();
	}

	// Token: 0x0600024C RID: 588 RVA: 0x00010169 File Offset: 0x0000E369
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<PoweredActiveController.Def>();
	}

	// Token: 0x04000173 RID: 371
	public const string ID = "DevRadiationGenerator";
}
