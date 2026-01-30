using System;
using TUNING;
using UnityEngine;

// Token: 0x02000076 RID: 118
public class DevLightGeneratorConfig : IBuildingConfig
{
	// Token: 0x06000239 RID: 569 RVA: 0x0000FAF0 File Offset: 0x0000DCF0
	public override BuildingDef CreateBuildingDef()
	{
		string id = "DevLightGenerator";
		int width = 1;
		int height = 1;
		string anim = "dev_generator_kanim";
		int hitpoints = 100;
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 2400f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER2, tier2, 0.2f);
		buildingDef.RequiresPowerInput = false;
		buildingDef.ViewMode = OverlayModes.Light.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.AudioSize = "large";
		buildingDef.Floodable = false;
		buildingDef.DebugOnly = true;
		SoundEventVolumeCache.instance.AddVolume("dev_lightgenerator_kanim", "PowerSwitch_on", NOISE_POLLUTION.NOISY.TIER3);
		SoundEventVolumeCache.instance.AddVolume("dev_lightgenerator_kanim", "PowerSwitch_off", NOISE_POLLUTION.NOISY.TIER3);
		return buildingDef;
	}

	// Token: 0x0600023A RID: 570 RVA: 0x0000FB9E File Offset: 0x0000DD9E
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		LightShapePreview lightShapePreview = go.AddComponent<LightShapePreview>();
		lightShapePreview.lux = 1800;
		lightShapePreview.radius = 8f;
		lightShapePreview.shape = global::LightShape.Circle;
	}

	// Token: 0x0600023B RID: 571 RVA: 0x0000FBC2 File Offset: 0x0000DDC2
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddTag(GameTags.DevBuilding);
		go.AddTag(GameTags.LightSource);
	}

	// Token: 0x0600023C RID: 572 RVA: 0x0000FBDC File Offset: 0x0000DDDC
	public override void DoPostConfigureComplete(GameObject go)
	{
		DevLightGenerator devLightGenerator = go.AddOrGet<DevLightGenerator>();
		devLightGenerator.overlayColour = LIGHT2D.CEILINGLIGHT_OVERLAYCOLOR;
		devLightGenerator.Color = LIGHT2D.CEILINGLIGHT_COLOR;
		devLightGenerator.Range = 8f;
		devLightGenerator.Angle = 2.6f;
		devLightGenerator.Direction = LIGHT2D.CEILINGLIGHT_DIRECTION;
		devLightGenerator.Offset = new Vector2(0f, 0.5f);
		devLightGenerator.shape = global::LightShape.Circle;
		devLightGenerator.drawOverlay = true;
		devLightGenerator.Lux = 1800;
		go.AddOrGetDef<LightController.Def>();
	}

	// Token: 0x04000169 RID: 361
	public const string ID = "DevLightGenerator";
}
