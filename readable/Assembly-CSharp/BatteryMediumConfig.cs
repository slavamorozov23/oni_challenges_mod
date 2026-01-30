using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000027 RID: 39
public class BatteryMediumConfig : BaseBatteryConfig
{
	// Token: 0x060000B4 RID: 180 RVA: 0x00006900 File Offset: 0x00004B00
	public override BuildingDef CreateBuildingDef()
	{
		string id = "BatteryMedium";
		int width = 2;
		int height = 2;
		int hitpoints = 30;
		string anim = "batterymed_kanim";
		float construction_time = 60f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 800f;
		float exhaust_temperature_active = 0.25f;
		float self_heat_kilowatts_active = 1f;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = base.CreateBuildingDef(id, width, height, hitpoints, anim, construction_time, tier, all_METALS, melting_point, exhaust_temperature_active, self_heat_kilowatts_active, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, tier2);
		SoundEventVolumeCache.instance.AddVolume("batterymed_kanim", "Battery_med_rattle", NOISE_POLLUTION.NOISY.TIER2);
		buildingDef.AddSearchTerms(SEARCH_TERMS.POWER);
		return buildingDef;
	}

	// Token: 0x060000B5 RID: 181 RVA: 0x00006974 File Offset: 0x00004B74
	public override void DoPostConfigureComplete(GameObject go)
	{
		Battery battery = go.AddOrGet<Battery>();
		battery.capacity = 40000f;
		battery.joulesLostPerSecond = 3.3333333f;
		base.DoPostConfigureComplete(go);
	}

	// Token: 0x060000B6 RID: 182 RVA: 0x00006998 File Offset: 0x00004B98
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		base.ConfigureBuildingTemplate(go, prefab_tag);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.PowerBuilding, false);
	}

	// Token: 0x04000084 RID: 132
	public const string ID = "BatteryMedium";
}
