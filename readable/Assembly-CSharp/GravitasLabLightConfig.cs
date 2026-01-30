using System;
using TUNING;
using UnityEngine;

// Token: 0x02000251 RID: 593
public class GravitasLabLightConfig : IBuildingConfig
{
	// Token: 0x06000C0C RID: 3084 RVA: 0x00048D18 File Offset: 0x00046F18
	public override BuildingDef CreateBuildingDef()
	{
		string id = "GravitasLabLight";
		int width = 1;
		int height = 1;
		string anim = "gravitas_lab_light_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 2400f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnCeiling;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
		buildingDef.ShowInBuildMenu = false;
		buildingDef.Entombable = false;
		buildingDef.Floodable = false;
		buildingDef.Invincible = true;
		buildingDef.AudioCategory = "Metal";
		return buildingDef;
	}

	// Token: 0x06000C0D RID: 3085 RVA: 0x00048D85 File Offset: 0x00046F85
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddTag(GameTags.Gravitas);
	}

	// Token: 0x06000C0E RID: 3086 RVA: 0x00048D92 File Offset: 0x00046F92
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x0400085B RID: 2139
	public const string ID = "GravitasLabLight";
}
