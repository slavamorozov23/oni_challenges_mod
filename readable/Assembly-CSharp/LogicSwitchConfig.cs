using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002CA RID: 714
public class LogicSwitchConfig : IBuildingConfig
{
	// Token: 0x06000E86 RID: 3718 RVA: 0x00054878 File Offset: 0x00052A78
	public override BuildingDef CreateBuildingDef()
	{
		string id = LogicSwitchConfig.ID;
		int width = 1;
		int height = 1;
		string anim = "switchdupecontrolledpower_kanim";
		int hitpoints = 10;
		float construction_time = 30f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, none, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.ViewMode = OverlayModes.Logic.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.AlwaysOperational = true;
		buildingDef.LogicOutputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort(LogicSwitch.PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.LOGICSWITCH.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.LOGICSWITCH.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.LOGICSWITCH.LOGIC_PORT_INACTIVE, true, false)
		};
		SoundEventVolumeCache.instance.AddVolume("switchpower_kanim", "PowerSwitch_on", NOISE_POLLUTION.NOISY.TIER3);
		SoundEventVolumeCache.instance.AddVolume("switchpower_kanim", "PowerSwitch_off", NOISE_POLLUTION.NOISY.TIER3);
		GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, LogicSwitchConfig.ID);
		buildingDef.AddSearchTerms(SEARCH_TERMS.AUTOMATION);
		return buildingDef;
	}

	// Token: 0x06000E87 RID: 3719 RVA: 0x0005498B File Offset: 0x00052B8B
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<BuildingComplete>().isManuallyOperated = false;
	}

	// Token: 0x06000E88 RID: 3720 RVA: 0x00054999 File Offset: 0x00052B99
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicSwitch>().manuallyControlled = false;
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits, false);
	}

	// Token: 0x0400096F RID: 2415
	public static string ID = "LogicSwitch";
}
