using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000029 RID: 41
public class BatterySmartConfig : BaseBatteryConfig
{
	// Token: 0x060000BE RID: 190 RVA: 0x00006B74 File Offset: 0x00004D74
	public override BuildingDef CreateBuildingDef()
	{
		string id = "BatterySmart";
		int width = 2;
		int height = 2;
		int hitpoints = 30;
		string anim = "smartbattery_kanim";
		float construction_time = 60f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 800f;
		float exhaust_temperature_active = 0f;
		float self_heat_kilowatts_active = 0.5f;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = base.CreateBuildingDef(id, width, height, hitpoints, anim, construction_time, tier, refined_METALS, melting_point, exhaust_temperature_active, self_heat_kilowatts_active, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, tier2);
		SoundEventVolumeCache.instance.AddVolume("batterymed_kanim", "Battery_med_rattle", NOISE_POLLUTION.NOISY.TIER2);
		buildingDef.LogicOutputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort(BatterySmart.PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.BATTERYSMART.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.BATTERYSMART.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.BATTERYSMART.LOGIC_PORT_INACTIVE, true, false)
		};
		buildingDef.AddSearchTerms(SEARCH_TERMS.POWER);
		buildingDef.AddSearchTerms(SEARCH_TERMS.BATTERY);
		return buildingDef;
	}

	// Token: 0x060000BF RID: 191 RVA: 0x00006C3A File Offset: 0x00004E3A
	public override void DoPostConfigureComplete(GameObject go)
	{
		BatterySmart batterySmart = go.AddOrGet<BatterySmart>();
		batterySmart.capacity = 20000f;
		batterySmart.joulesLostPerSecond = 0.6666667f;
		batterySmart.powerSortOrder = 1000;
		base.DoPostConfigureComplete(go);
	}

	// Token: 0x060000C0 RID: 192 RVA: 0x00006C69 File Offset: 0x00004E69
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		base.ConfigureBuildingTemplate(go, prefab_tag);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.PowerBuilding, false);
	}

	// Token: 0x04000088 RID: 136
	public const string ID = "BatterySmart";
}
