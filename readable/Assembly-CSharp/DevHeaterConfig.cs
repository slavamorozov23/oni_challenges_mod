using System;
using TUNING;
using UnityEngine;

// Token: 0x02000074 RID: 116
public class DevHeaterConfig : IBuildingConfig
{
	// Token: 0x06000230 RID: 560 RVA: 0x0000F890 File Offset: 0x0000DA90
	public override BuildingDef CreateBuildingDef()
	{
		string id = "DevHeater";
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
		buildingDef.RequiresPowerInput = false;
		buildingDef.ViewMode = OverlayModes.Light.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.AudioSize = "large";
		buildingDef.Floodable = false;
		buildingDef.DebugOnly = true;
		buildingDef.Overheatable = false;
		SoundEventVolumeCache.instance.AddVolume("dev_lightgenerator_kanim", "PowerSwitch_on", NOISE_POLLUTION.NOISY.TIER3);
		SoundEventVolumeCache.instance.AddVolume("dev_lightgenerator_kanim", "PowerSwitch_off", NOISE_POLLUTION.NOISY.TIER3);
		return buildingDef;
	}

	// Token: 0x06000231 RID: 561 RVA: 0x0000F945 File Offset: 0x0000DB45
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
	}

	// Token: 0x06000232 RID: 562 RVA: 0x0000F947 File Offset: 0x0000DB47
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddTag(GameTags.DevBuilding);
	}

	// Token: 0x06000233 RID: 563 RVA: 0x0000F954 File Offset: 0x0000DB54
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<DirectVolumeHeater>();
	}

	// Token: 0x04000163 RID: 355
	public const string ID = "DevHeater";
}
