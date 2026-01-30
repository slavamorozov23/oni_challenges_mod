using System;
using TUNING;
using UnityEngine;

// Token: 0x0200017E RID: 382
public class FloorLampConfig : IBuildingConfig
{
	// Token: 0x0600076C RID: 1900 RVA: 0x000333B0 File Offset: 0x000315B0
	public override BuildingDef CreateBuildingDef()
	{
		string id = "FloorLamp";
		int width = 1;
		int height = 2;
		string anim = "floorlamp_kanim";
		int hitpoints = 10;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 8f;
		buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
		buildingDef.ViewMode = OverlayModes.Light.ID;
		buildingDef.AudioCategory = "Metal";
		return buildingDef;
	}

	// Token: 0x0600076D RID: 1901 RVA: 0x0003342C File Offset: 0x0003162C
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		LightShapePreview lightShapePreview = go.AddComponent<LightShapePreview>();
		lightShapePreview.lux = 1000;
		lightShapePreview.radius = 4f;
		lightShapePreview.shape = global::LightShape.Circle;
		lightShapePreview.offset = new CellOffset((int)def.BuildingComplete.GetComponent<Light2D>().Offset.x, (int)def.BuildingComplete.GetComponent<Light2D>().Offset.y);
	}

	// Token: 0x0600076E RID: 1902 RVA: 0x00033492 File Offset: 0x00031692
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(GameTags.LightSource, false);
	}

	// Token: 0x0600076F RID: 1903 RVA: 0x000334A8 File Offset: 0x000316A8
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<EnergyConsumer>();
		go.AddOrGet<LoopingSounds>();
		Light2D light2D = go.AddOrGet<Light2D>();
		light2D.overlayColour = LIGHT2D.FLOORLAMP_OVERLAYCOLOR;
		light2D.Color = LIGHT2D.FLOORLAMP_COLOR;
		light2D.Range = 4f;
		light2D.Angle = 0f;
		light2D.Direction = LIGHT2D.FLOORLAMP_DIRECTION;
		light2D.Offset = LIGHT2D.FLOORLAMP_OFFSET;
		light2D.shape = global::LightShape.Circle;
		light2D.drawOverlay = true;
		go.AddOrGetDef<LightController.Def>();
	}

	// Token: 0x040005A7 RID: 1447
	public const string ID = "FloorLamp";
}
