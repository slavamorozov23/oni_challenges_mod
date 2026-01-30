using System;
using TUNING;
using UnityEngine;

// Token: 0x02000423 RID: 1059
public class SolidConduitConfig : IBuildingConfig
{
	// Token: 0x060015DC RID: 5596 RVA: 0x0007CF0C File Offset: 0x0007B10C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SolidConduit";
		int width = 1;
		int height = 1;
		string anim = "utilities_conveyor_kanim";
		int hitpoints = 10;
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.Entombable = false;
		buildingDef.ViewMode = OverlayModes.SolidConveyor.ID;
		buildingDef.ObjectLayer = ObjectLayer.SolidConduit;
		buildingDef.TileLayer = ObjectLayer.SolidConduitTile;
		buildingDef.ReplacementLayer = ObjectLayer.ReplacementSolidConduit;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = 0f;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
		buildingDef.SceneLayer = Grid.SceneLayer.SolidConduits;
		buildingDef.isKAnimTile = true;
		buildingDef.isUtility = true;
		buildingDef.DragBuild = true;
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SolidConveyorIDs, "SolidConduit");
		return buildingDef;
	}

	// Token: 0x060015DD RID: 5597 RVA: 0x0007CFF0 File Offset: 0x0007B1F0
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<SolidConduit>();
	}

	// Token: 0x060015DE RID: 5598 RVA: 0x0007D014 File Offset: 0x0007B214
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		KAnimGraphTileVisualizer kanimGraphTileVisualizer = go.AddComponent<KAnimGraphTileVisualizer>();
		kanimGraphTileVisualizer.connectionSource = KAnimGraphTileVisualizer.ConnectionSource.Solid;
		kanimGraphTileVisualizer.isPhysicalBuilding = false;
		go.GetComponent<Constructable>().requiredSkillPerk = Db.Get().SkillPerks.ConveyorBuild.Id;
	}

	// Token: 0x060015DF RID: 5599 RVA: 0x0007D048 File Offset: 0x0007B248
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<Building>().Def.BuildingUnderConstruction.GetComponent<Constructable>().isDiggingRequired = false;
		KAnimGraphTileVisualizer kanimGraphTileVisualizer = go.AddComponent<KAnimGraphTileVisualizer>();
		kanimGraphTileVisualizer.connectionSource = KAnimGraphTileVisualizer.ConnectionSource.Solid;
		kanimGraphTileVisualizer.isPhysicalBuilding = true;
		LiquidConduitConfig.CommonConduitPostConfigureComplete(go);
		go.AddComponent<EmptySolidConduitWorkable>();
	}

	// Token: 0x04000D05 RID: 3333
	public const string ID = "SolidConduit";
}
