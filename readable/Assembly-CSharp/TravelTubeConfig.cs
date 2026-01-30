using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000453 RID: 1107
public class TravelTubeConfig : IBuildingConfig
{
	// Token: 0x060016F9 RID: 5881 RVA: 0x00082FE8 File Offset: 0x000811E8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "TravelTube";
		int width = 1;
		int height = 1;
		string anim = "travel_tube_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
		string[] plastics = MATERIALS.PLASTICS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.NotInTiles;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, plastics, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER0, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.Entombable = false;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.TileLayer = ObjectLayer.TravelTubeTile;
		buildingDef.ReplacementLayer = ObjectLayer.ReplacementTravelTube;
		buildingDef.AudioCategory = "Plastic";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = 0f;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
		buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
		buildingDef.isKAnimTile = true;
		buildingDef.isUtility = true;
		buildingDef.DragBuild = true;
		buildingDef.AddSearchTerms(SEARCH_TERMS.TRANSPORT);
		return buildingDef;
	}

	// Token: 0x060016FA RID: 5882 RVA: 0x000830C2 File Offset: 0x000812C2
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<TravelTube>();
	}

	// Token: 0x060016FB RID: 5883 RVA: 0x000830E6 File Offset: 0x000812E6
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		KAnimGraphTileVisualizer kanimGraphTileVisualizer = go.AddComponent<KAnimGraphTileVisualizer>();
		kanimGraphTileVisualizer.connectionSource = KAnimGraphTileVisualizer.ConnectionSource.Tube;
		kanimGraphTileVisualizer.isPhysicalBuilding = false;
	}

	// Token: 0x060016FC RID: 5884 RVA: 0x000830FB File Offset: 0x000812FB
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<Building>().Def.BuildingUnderConstruction.GetComponent<Constructable>().isDiggingRequired = false;
		KAnimGraphTileVisualizer kanimGraphTileVisualizer = go.AddComponent<KAnimGraphTileVisualizer>();
		kanimGraphTileVisualizer.connectionSource = KAnimGraphTileVisualizer.ConnectionSource.Tube;
		kanimGraphTileVisualizer.isPhysicalBuilding = true;
	}

	// Token: 0x04000D99 RID: 3481
	public const string ID = "TravelTube";
}
