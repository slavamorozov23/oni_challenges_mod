using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200024E RID: 590
public class GravitasDoorConfig : IBuildingConfig
{
	// Token: 0x06000BFF RID: 3071 RVA: 0x000489D0 File Offset: 0x00046BD0
	public override BuildingDef CreateBuildingDef()
	{
		string id = "GravitasDoor";
		int width = 1;
		int height = 3;
		string anim = "gravitas_door_internal_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Tile;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, none, 1f);
		buildingDef.ShowInBuildMenu = false;
		buildingDef.Entombable = false;
		buildingDef.Floodable = false;
		buildingDef.Invincible = true;
		buildingDef.AudioCategory = "Metal";
		buildingDef.PermittedRotations = PermittedRotations.R90;
		buildingDef.ForegroundLayer = Grid.SceneLayer.InteriorWall;
		buildingDef.LogicInputPorts = DoorConfig.CreateSingleInputPortList(new CellOffset(0, 0));
		buildingDef.LogicInputPorts = GravitasDoorConfig.CreateSingleInputPortList(new CellOffset(0, 0));
		SoundEventVolumeCache.instance.AddVolume("gravitas_door_internal_kanim", "GravitasDoorInternal_open", NOISE_POLLUTION.NOISY.TIER2);
		SoundEventVolumeCache.instance.AddVolume("gravitas_door_internal_kanim", "GravitasDoorInternal_close", NOISE_POLLUTION.NOISY.TIER2);
		return buildingDef;
	}

	// Token: 0x06000C00 RID: 3072 RVA: 0x00048AA4 File Offset: 0x00046CA4
	public static List<LogicPorts.Port> CreateSingleInputPortList(CellOffset offset)
	{
		return new List<LogicPorts.Port>
		{
			LogicPorts.Port.InputPort(Door.OPEN_CLOSE_PORT_ID, offset, STRINGS.BUILDINGS.PREFABS.DOOR.LOGIC_OPEN, STRINGS.BUILDINGS.PREFABS.DOOR.LOGIC_OPEN_ACTIVE, STRINGS.BUILDINGS.PREFABS.DOOR.LOGIC_OPEN_INACTIVE, false, false)
		};
	}

	// Token: 0x06000C01 RID: 3073 RVA: 0x00048AE8 File Offset: 0x00046CE8
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddTag(GameTags.Gravitas);
		Door door = go.AddOrGet<Door>();
		door.unpoweredAnimSpeed = 1f;
		door.doorType = Door.DoorType.Internal;
		door.doorOpeningSoundEventName = "GravitasDoorInternal_open";
		door.doorClosingSoundEventName = "GravitasDoorInternal_close";
		go.AddOrGet<ZoneTile>();
		go.AddOrGet<AccessControl>().controlEnabled = true;
		go.AddOrGet<CopyBuildingSettings>().copyGroupTag = GameTags.Door;
		go.AddOrGet<Workable>().workTime = 3f;
		go.AddOrGet<KBoxCollider2D>();
		Prioritizable.AddRef(go);
		UnityEngine.Object.DestroyImmediate(go.GetComponent<BuildingEnabledButton>());
	}

	// Token: 0x06000C02 RID: 3074 RVA: 0x00048B78 File Offset: 0x00046D78
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000859 RID: 2137
	public const string ID = "GravitasDoor";
}
