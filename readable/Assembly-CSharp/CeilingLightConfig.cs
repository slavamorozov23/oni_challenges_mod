using System;
using TUNING;
using UnityEngine;

// Token: 0x02000040 RID: 64
public class CeilingLightConfig : IBuildingConfig
{
	// Token: 0x06000136 RID: 310 RVA: 0x00009580 File Offset: 0x00007780
	public override BuildingDef CreateBuildingDef()
	{
		string id = "CeilingLight";
		int width = 1;
		int height = 1;
		string anim = "ceilinglight_kanim";
		int hitpoints = 10;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnCeiling;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 10f;
		buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
		buildingDef.ViewMode = OverlayModes.Light.ID;
		buildingDef.AudioCategory = "Metal";
		return buildingDef;
	}

	// Token: 0x06000137 RID: 311 RVA: 0x000095F9 File Offset: 0x000077F9
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		LightShapePreview lightShapePreview = go.AddComponent<LightShapePreview>();
		lightShapePreview.lux = 1800;
		lightShapePreview.radius = 8f;
		lightShapePreview.shape = global::LightShape.Cone;
	}

	// Token: 0x06000138 RID: 312 RVA: 0x0000961D File Offset: 0x0000781D
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(GameTags.LightSource, false);
	}

	// Token: 0x06000139 RID: 313 RVA: 0x00009630 File Offset: 0x00007830
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LoopingSounds>();
		Light2D light2D = go.AddOrGet<Light2D>();
		light2D.overlayColour = LIGHT2D.CEILINGLIGHT_OVERLAYCOLOR;
		light2D.Color = LIGHT2D.CEILINGLIGHT_COLOR;
		light2D.Range = 8f;
		light2D.Angle = 2.6f;
		light2D.Direction = LIGHT2D.CEILINGLIGHT_DIRECTION;
		light2D.Offset = LIGHT2D.CEILINGLIGHT_OFFSET;
		light2D.shape = global::LightShape.Cone;
		light2D.drawOverlay = true;
		light2D.Lux = 1800;
		go.AddOrGetDef<LightController.Def>();
	}

	// Token: 0x040000BD RID: 189
	public const string ID = "CeilingLight";
}
