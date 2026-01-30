using System;
using TUNING;
using UnityEngine;

// Token: 0x02000781 RID: 1921
public class InsulatedDoorConfig : IBuildingConfig
{
	// Token: 0x06003104 RID: 12548 RVA: 0x0011AD78 File Offset: 0x00118F78
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("InsulatedDoor", 1, 2, "door_insulated_kanim", 100, 60f, new float[]
		{
			BUILDINGS.CONSTRUCTION_MASS_KG.TIER5[0],
			2f
		}, new string[]
		{
			"BuildableRaw",
			"BuildingFiber"
		}, 1600f, BuildLocationRule.Tile, DECOR.PENALTY.TIER2, NOISE_POLLUTION.NONE, 0.2f);
		buildingDef.ForegroundLayer = Grid.SceneLayer.InteriorWall;
		buildingDef.SceneLayer = Grid.SceneLayer.TileMain;
		buildingDef.ThermalConductivity = 0.01f;
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
		buildingDef.Overheatable = false;
		buildingDef.Floodable = false;
		buildingDef.Disinfectable = false;
		buildingDef.Entombable = false;
		buildingDef.Invincible = false;
		buildingDef.Repairable = false;
		buildingDef.IsFoundation = true;
		buildingDef.TileLayer = ObjectLayer.FoundationTile;
		buildingDef.AudioCategory = "Metal";
		return buildingDef;
	}

	// Token: 0x06003105 RID: 12549 RVA: 0x0011AEEC File Offset: 0x001190EC
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Door door = go.AddOrGet<Door>();
		door.hasComplexUserControls = true;
		door.unpoweredAnimSpeed = 1f;
		door.doorType = Door.DoorType.ManualPressure;
		door.insulationModifier = 0.01f;
		go.GetComponent<KPrefabID>();
		go.AddOrGet<ZoneTile>();
		go.AddOrGet<AccessControl>();
		go.AddOrGet<KBoxCollider2D>();
		Prioritizable.AddRef(go);
		go.AddOrGet<CopyBuildingSettings>().copyGroupTag = GameTags.Door;
		go.AddOrGet<Workable>().workTime = 5f;
		go.AddOrGet<LoopingSounds>();
		UnityEngine.Object.DestroyImmediate(go.GetComponent<BuildingEnabledButton>());
	}

	// Token: 0x06003106 RID: 12550 RVA: 0x0011AF76 File Offset: 0x00119176
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<AccessControl>().controlEnabled = true;
		go.GetComponent<KBatchedAnimController>().initialAnim = "closed";
	}

	// Token: 0x04001D56 RID: 7510
	public const string ID = "InsulatedDoor";

	// Token: 0x04001D57 RID: 7511
	private const float INSULATION_MODIFIER = 0.01f;
}
