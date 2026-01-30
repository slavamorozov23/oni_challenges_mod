using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000026 RID: 38
public class BatteryConfig : BaseBatteryConfig
{
	// Token: 0x060000B0 RID: 176 RVA: 0x0000683C File Offset: 0x00004A3C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Battery";
		int width = 1;
		int height = 2;
		int hitpoints = 30;
		string anim = "batterysm_kanim";
		float construction_time = 30f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 800f;
		float exhaust_temperature_active = 0.25f;
		float self_heat_kilowatts_active = 1f;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = base.CreateBuildingDef(id, width, height, hitpoints, anim, construction_time, tier, all_METALS, melting_point, exhaust_temperature_active, self_heat_kilowatts_active, TUNING.BUILDINGS.DECOR.PENALTY.TIER1, none);
		buildingDef.Breakable = true;
		SoundEventVolumeCache.instance.AddVolume("batterysm_kanim", "Battery_rattle", NOISE_POLLUTION.NOISY.TIER1);
		buildingDef.AddSearchTerms(SEARCH_TERMS.POWER);
		return buildingDef;
	}

	// Token: 0x060000B1 RID: 177 RVA: 0x000068B7 File Offset: 0x00004AB7
	public override void DoPostConfigureComplete(GameObject go)
	{
		Battery battery = go.AddOrGet<Battery>();
		battery.capacity = 10000f;
		battery.joulesLostPerSecond = 1.6666666f;
		base.DoPostConfigureComplete(go);
	}

	// Token: 0x060000B2 RID: 178 RVA: 0x000068DB File Offset: 0x00004ADB
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		base.ConfigureBuildingTemplate(go, prefab_tag);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.PowerBuilding, false);
	}

	// Token: 0x04000083 RID: 131
	public const string ID = "Battery";
}
