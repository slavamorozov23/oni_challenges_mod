using System;
using TUNING;
using UnityEngine;

// Token: 0x02000446 RID: 1094
public class SunLampConfig : IBuildingConfig
{
	// Token: 0x060016B2 RID: 5810 RVA: 0x0008164C File Offset: 0x0007F84C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SunLamp";
		int width = 2;
		int height = 4;
		string anim = "sun_lamp_kanim";
		int hitpoints = 10;
		float construction_time = 60f;
		float[] construction_mass = new float[]
		{
			200f,
			50f
		};
		string[] construction_materials = new string[]
		{
			"RefinedMetal",
			"Glass"
		};
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER3, none, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 960f;
		buildingDef.SelfHeatKilowattsWhenActive = 4f;
		buildingDef.ExhaustKilowattsWhenActive = 1f;
		buildingDef.ViewMode = OverlayModes.Light.ID;
		buildingDef.AudioCategory = "Metal";
		return buildingDef;
	}

	// Token: 0x060016B3 RID: 5811 RVA: 0x000816F4 File Offset: 0x0007F8F4
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		LightShapePreview lightShapePreview = go.AddComponent<LightShapePreview>();
		lightShapePreview.lux = LIGHT2D.SUNLAMP_LUX;
		lightShapePreview.radius = 16f;
		lightShapePreview.shape = global::LightShape.Cone;
		lightShapePreview.offset = new CellOffset((int)LIGHT2D.SUNLAMP_OFFSET.x, (int)LIGHT2D.SUNLAMP_OFFSET.y);
	}

	// Token: 0x060016B4 RID: 5812 RVA: 0x00081744 File Offset: 0x0007F944
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(GameTags.LightSource, false);
	}

	// Token: 0x060016B5 RID: 5813 RVA: 0x00081758 File Offset: 0x0007F958
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<EnergyConsumer>();
		go.AddOrGet<LoopingSounds>();
		Light2D light2D = go.AddOrGet<Light2D>();
		light2D.Lux = LIGHT2D.SUNLAMP_LUX;
		light2D.overlayColour = LIGHT2D.SUNLAMP_OVERLAYCOLOR;
		light2D.Color = LIGHT2D.SUNLAMP_COLOR;
		light2D.Range = 16f;
		light2D.Angle = 5.2f;
		light2D.Direction = LIGHT2D.SUNLAMP_DIRECTION;
		light2D.Offset = LIGHT2D.SUNLAMP_OFFSET;
		light2D.shape = global::LightShape.Cone;
		light2D.drawOverlay = true;
		go.AddOrGetDef<LightController.Def>();
	}

	// Token: 0x04000D74 RID: 3444
	public const string ID = "SunLamp";
}
