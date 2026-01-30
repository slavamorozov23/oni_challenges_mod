using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002C1 RID: 705
public class LogicMemoryConfig : IBuildingConfig
{
	// Token: 0x06000E5E RID: 3678 RVA: 0x00053B5C File Offset: 0x00051D5C
	public override BuildingDef CreateBuildingDef()
	{
		string id = LogicMemoryConfig.ID;
		int width = 2;
		int height = 2;
		string anim = "logic_memory_kanim";
		int hitpoints = 10;
		float construction_time = 30f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, none, 0.2f);
		buildingDef.Deprecated = false;
		buildingDef.Overheatable = false;
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.InitialOrientation = Orientation.R90;
		buildingDef.ViewMode = OverlayModes.Logic.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.SceneLayer = Grid.SceneLayer.LogicGates;
		buildingDef.ObjectLayer = ObjectLayer.LogicGate;
		buildingDef.AlwaysOperational = true;
		buildingDef.LogicInputPorts = new List<LogicPorts.Port>
		{
			new LogicPorts.Port(LogicMemory.SET_PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.LOGICMEMORY.SET_PORT, STRINGS.BUILDINGS.PREFABS.LOGICMEMORY.SET_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.LOGICMEMORY.SET_PORT_INACTIVE, true, LogicPortSpriteType.Input, true),
			new LogicPorts.Port(LogicMemory.RESET_PORT_ID, new CellOffset(1, 0), STRINGS.BUILDINGS.PREFABS.LOGICMEMORY.RESET_PORT, STRINGS.BUILDINGS.PREFABS.LOGICMEMORY.RESET_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.LOGICMEMORY.RESET_PORT_INACTIVE, true, LogicPortSpriteType.ResetUpdate, true)
		};
		buildingDef.LogicOutputPorts = new List<LogicPorts.Port>
		{
			new LogicPorts.Port(LogicMemory.READ_PORT_ID, new CellOffset(0, 1), STRINGS.BUILDINGS.PREFABS.LOGICMEMORY.READ_PORT, STRINGS.BUILDINGS.PREFABS.LOGICMEMORY.READ_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.LOGICMEMORY.READ_PORT_INACTIVE, true, LogicPortSpriteType.Output, true)
		};
		SoundEventVolumeCache.instance.AddVolume("logic_memory_kanim", "PowerMemory_on", NOISE_POLLUTION.NOISY.TIER3);
		SoundEventVolumeCache.instance.AddVolume("logic_memory_kanim", "PowerMemory_off", NOISE_POLLUTION.NOISY.TIER3);
		GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, LogicMemoryConfig.ID);
		buildingDef.AddSearchTerms(SEARCH_TERMS.AUTOMATION);
		return buildingDef;
	}

	// Token: 0x06000E5F RID: 3679 RVA: 0x00053D08 File Offset: 0x00051F08
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicMemory>();
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits, false);
	}

	// Token: 0x04000965 RID: 2405
	public static string ID = "LogicMemory";
}
