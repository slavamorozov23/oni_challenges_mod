using System;
using TUNING;
using UnityEngine;

// Token: 0x02000470 RID: 1136
public class WoodenDoorConfig : IBuildingConfig
{
	// Token: 0x060017C0 RID: 6080 RVA: 0x00086835 File Offset: 0x00084A35
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x060017C1 RID: 6081 RVA: 0x0008683C File Offset: 0x00084A3C
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("WoodenDoor", 1, 2, "door_wood_kanim", 30, 10f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER2, MATERIALS.WOODS, 1600f, BuildLocationRule.Tile, DECOR.NONE, NOISE_POLLUTION.NONE, 0.2f);
		buildingDef.InputConduitType = ConduitType.None;
		buildingDef.OutputConduitType = ConduitType.None;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
		buildingDef.RequiresPowerInput = false;
		buildingDef.RequiresPowerOutput = false;
		buildingDef.PowerInputOffset = new CellOffset(0, 0);
		buildingDef.PowerOutputOffset = new CellOffset(0, 0);
		buildingDef.UseHighEnergyParticleInputPort = false;
		buildingDef.UseHighEnergyParticleOutputPort = false;
		buildingDef.HighEnergyParticleInputOffset = new CellOffset(0, 0);
		buildingDef.HighEnergyParticleOutputOffset = new CellOffset(0, 0);
		buildingDef.PermittedRotations = PermittedRotations.R90;
		buildingDef.DragBuild = true;
		buildingDef.Replaceable = false;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.SelfHeatKilowattsWhenActive = 0f;
		buildingDef.UseStructureTemperature = true;
		buildingDef.Overheatable = true;
		buildingDef.Floodable = false;
		buildingDef.Disinfectable = false;
		buildingDef.Entombable = true;
		buildingDef.Invincible = false;
		buildingDef.Repairable = false;
		buildingDef.IsFoundation = false;
		buildingDef.ForegroundLayer = Grid.SceneLayer.InteriorWall;
		buildingDef.AudioCategory = "Metal";
		SoundEventVolumeCache.instance.AddVolume("door_internal_kanim", "Open_DoorInternal", NOISE_POLLUTION.NOISY.TIER2);
		SoundEventVolumeCache.instance.AddVolume("door_internal_kanim", "Close_DoorInternal", NOISE_POLLUTION.NOISY.TIER2);
		return buildingDef;
	}

	// Token: 0x060017C2 RID: 6082 RVA: 0x000869A0 File Offset: 0x00084BA0
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>();
		go.AddOrGet<LoopingSounds>();
	}

	// Token: 0x060017C3 RID: 6083 RVA: 0x000869B0 File Offset: 0x00084BB0
	public override void DoPostConfigureComplete(GameObject go)
	{
		Door door = go.AddOrGet<Door>();
		door.unpoweredAnimSpeed = 1f;
		door.doorType = Door.DoorType.Internal;
		go.AddOrGet<AccessControl>().controlEnabled = true;
		go.AddOrGet<CopyBuildingSettings>().copyGroupTag = GameTags.Door;
		go.AddOrGet<Workable>().workTime = 3f;
		go.GetComponent<KBatchedAnimController>().initialAnim = "closed";
		go.AddOrGet<ZoneTile>();
		go.AddOrGet<KBoxCollider2D>();
		Prioritizable.AddRef(go);
		UnityEngine.Object.DestroyImmediate(go.GetComponent<BuildingEnabledButton>());
	}

	// Token: 0x060017C4 RID: 6084 RVA: 0x00086A2F File Offset: 0x00084C2F
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		go.AddTag(GameTags.NoCreatureIdling);
	}

	// Token: 0x04000DEF RID: 3567
	public const string ID = "WoodenDoor";
}
