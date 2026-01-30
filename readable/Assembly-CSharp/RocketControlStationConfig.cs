using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003EE RID: 1006
public class RocketControlStationConfig : IBuildingConfig
{
	// Token: 0x060014B5 RID: 5301 RVA: 0x000772D3 File Offset: 0x000754D3
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060014B6 RID: 5302 RVA: 0x000772DC File Offset: 0x000754DC
	public override BuildingDef CreateBuildingDef()
	{
		string id = RocketControlStationConfig.ID;
		int width = 2;
		int height = 2;
		string anim = "rocket_control_station_kanim";
		int hitpoints = 30;
		float construction_time = 60f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER3;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER2, tier2, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.Repairable = false;
		buildingDef.Floodable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		buildingDef.DefaultAnimState = "off";
		buildingDef.OnePerWorld = true;
		buildingDef.RequiredSkillPerkID = Db.Get().SkillPerks.CanUseRocketControlStation.Id;
		buildingDef.LogicInputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.InputPort(RocketControlStation.PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.ROCKETCONTROLSTATION.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.ROCKETCONTROLSTATION.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.ROCKETCONTROLSTATION.LOGIC_PORT_INACTIVE, false, false)
		};
		buildingDef.AddSearchTerms(SEARCH_TERMS.ROCKET);
		return buildingDef;
	}

	// Token: 0x060014B7 RID: 5303 RVA: 0x000773CB File Offset: 0x000755CB
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		KPrefabID component = go.GetComponent<KPrefabID>();
		component.AddTag(GameTags.RocketInteriorBuilding, false);
		component.AddTag(GameTags.UniquePerWorld, false);
	}

	// Token: 0x060014B8 RID: 5304 RVA: 0x000773EC File Offset: 0x000755EC
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		go.AddOrGet<RocketControlStationIdleWorkable>().workLayer = Grid.SceneLayer.BuildingUse;
		go.AddOrGet<RocketControlStationLaunchWorkable>().workLayer = Grid.SceneLayer.BuildingUse;
		go.AddOrGet<RocketControlStation>();
		go.AddOrGetDef<PoweredController.Def>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RocketInterior, false);
	}

	// Token: 0x04000C95 RID: 3221
	public static string ID = "RocketControlStation";

	// Token: 0x04000C96 RID: 3222
	public const float CONSOLE_WORK_TIME = 30f;

	// Token: 0x04000C97 RID: 3223
	public const float CONSOLE_IDLE_TIME = 120f;

	// Token: 0x04000C98 RID: 3224
	public const float WARNING_COOLDOWN = 30f;

	// Token: 0x04000C99 RID: 3225
	public const float DEFAULT_SPEED = 1f;

	// Token: 0x04000C9A RID: 3226
	public const float SLOW_SPEED = 0.5f;

	// Token: 0x04000C9B RID: 3227
	public const float SUPER_SPEED = 1.5f;

	// Token: 0x04000C9C RID: 3228
	public const float DEFAULT_PILOT_MODIFIER = 1f;
}
