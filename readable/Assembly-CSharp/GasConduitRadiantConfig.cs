using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x0200022E RID: 558
public class GasConduitRadiantConfig : IBuildingConfig
{
	// Token: 0x06000B4E RID: 2894 RVA: 0x00043C5C File Offset: 0x00041E5C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "GasConduitRadiant";
		int width = 1;
		int height = 1;
		string anim = "utilities_gas_radiant_kanim";
		int hitpoints = 10;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
		buildingDef.ThermalConductivity = 2f;
		buildingDef.Overheatable = false;
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.ViewMode = OverlayModes.GasConduits.ID;
		buildingDef.ObjectLayer = ObjectLayer.GasConduit;
		buildingDef.TileLayer = ObjectLayer.GasConduitTile;
		buildingDef.ReplacementLayer = ObjectLayer.ReplacementGasConduit;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = 0f;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
		buildingDef.SceneLayer = Grid.SceneLayer.GasConduits;
		buildingDef.isKAnimTile = true;
		buildingDef.isUtility = true;
		buildingDef.DragBuild = true;
		buildingDef.ReplacementTags = new List<Tag>();
		buildingDef.ReplacementTags.Add(GameTags.Vents);
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.GasVentIDs, "GasConduitRadiant");
		return buildingDef;
	}

	// Token: 0x06000B4F RID: 2895 RVA: 0x00043D66 File Offset: 0x00041F66
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		go.AddOrGet<Conduit>().type = ConduitType.Gas;
	}

	// Token: 0x06000B50 RID: 2896 RVA: 0x00043D7C File Offset: 0x00041F7C
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<Building>().Def.BuildingUnderConstruction.GetComponent<Constructable>().isDiggingRequired = false;
		go.AddComponent<EmptyConduitWorkable>();
		KAnimGraphTileVisualizer kanimGraphTileVisualizer = go.AddComponent<KAnimGraphTileVisualizer>();
		kanimGraphTileVisualizer.connectionSource = KAnimGraphTileVisualizer.ConnectionSource.Gas;
		kanimGraphTileVisualizer.isPhysicalBuilding = true;
		go.GetComponent<KPrefabID>().AddTag(GameTags.Vents, false);
		LiquidConduitConfig.CommonConduitPostConfigureComplete(go);
	}

	// Token: 0x06000B51 RID: 2897 RVA: 0x00043DD5 File Offset: 0x00041FD5
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		KAnimGraphTileVisualizer kanimGraphTileVisualizer = go.AddComponent<KAnimGraphTileVisualizer>();
		kanimGraphTileVisualizer.connectionSource = KAnimGraphTileVisualizer.ConnectionSource.Gas;
		kanimGraphTileVisualizer.isPhysicalBuilding = false;
	}

	// Token: 0x040007DC RID: 2012
	public const string ID = "GasConduitRadiant";
}
