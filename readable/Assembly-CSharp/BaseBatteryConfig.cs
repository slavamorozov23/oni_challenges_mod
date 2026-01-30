using System;
using TUNING;
using UnityEngine;

// Token: 0x02000022 RID: 34
public abstract class BaseBatteryConfig : IBuildingConfig
{
	// Token: 0x0600009B RID: 155 RVA: 0x00006144 File Offset: 0x00004344
	public BuildingDef CreateBuildingDef(string id, int width, int height, int hitpoints, string anim, float construction_time, float[] construction_mass, string[] construction_materials, float melting_point, float exhaust_temperature_active, float self_heat_kilowatts_active, EffectorValues decor, EffectorValues noise)
	{
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER0;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, decor, tier, 0.2f);
		buildingDef.ExhaustKilowattsWhenActive = exhaust_temperature_active;
		buildingDef.SelfHeatKilowattsWhenActive = self_heat_kilowatts_active;
		buildingDef.Entombable = false;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.RequiresPowerOutput = true;
		buildingDef.UseWhitePowerOutputConnectorColour = true;
		return buildingDef;
	}

	// Token: 0x0600009C RID: 156 RVA: 0x000061B1 File Offset: 0x000043B1
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddComponent<RequireInputs>();
	}

	// Token: 0x0600009D RID: 157 RVA: 0x000061BA File Offset: 0x000043BA
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<Battery>().powerSortOrder = 1000;
		go.AddOrGetDef<PoweredActiveController.Def>();
	}
}
