using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002EC RID: 748
public class MeshTileConfig : IBuildingConfig
{
	// Token: 0x06000F38 RID: 3896 RVA: 0x00058338 File Offset: 0x00056538
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MeshTile";
		int width = 1;
		int height = 1;
		string anim = "floor_mesh_kanim";
		int hitpoints = 100;
		float construction_time = 30f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Tile;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
		BuildingTemplates.CreateFoundationTileDef(buildingDef);
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.Overheatable = false;
		buildingDef.UseStructureTemperature = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.SceneLayer = Grid.SceneLayer.TileMain;
		buildingDef.ConstructionOffsetFilter = BuildingDef.ConstructionOffsetFilter_OneDown;
		buildingDef.isKAnimTile = true;
		buildingDef.BlockTileAtlas = Assets.GetTextureAtlas("tiles_mesh");
		buildingDef.BlockTilePlaceAtlas = Assets.GetTextureAtlas("tiles_mesh_place");
		buildingDef.BlockTileShineAtlas = Assets.GetTextureAtlas("tiles_mesh_spec");
		buildingDef.BlockTileMaterial = Assets.GetMaterial("tiles_solid");
		buildingDef.DecorBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_mesh_tops_decor_info");
		buildingDef.DecorPlaceBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_mesh_tops_decor_place_info");
		buildingDef.AddSearchTerms(SEARCH_TERMS.TILE);
		return buildingDef;
	}

	// Token: 0x06000F39 RID: 3897 RVA: 0x0005844C File Offset: 0x0005664C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<SimCellOccupier>().doReplaceElement = false;
		go.AddOrGet<TileTemperature>();
		go.AddOrGet<KAnimGridTileVisualizer>().blockTileConnectorID = MeshTileConfig.BlockTileConnectorID;
		go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
	}

	// Token: 0x06000F3A RID: 3898 RVA: 0x000584A3 File Offset: 0x000566A3
	public override void DoPostConfigureComplete(GameObject go)
	{
		GeneratedBuildings.RemoveLoopingSounds(go);
		go.GetComponent<KPrefabID>().AddTag(GameTags.FloorTiles, false);
		go.AddComponent<SimTemperatureTransfer>();
		go.AddComponent<ZoneTile>();
	}

	// Token: 0x06000F3B RID: 3899 RVA: 0x000584CA File Offset: 0x000566CA
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		go.AddOrGet<KAnimGridTileVisualizer>();
	}

	// Token: 0x040009EE RID: 2542
	public static readonly int BlockTileConnectorID = Hash.SDBMLower("tiles_mesh_tops");

	// Token: 0x040009EF RID: 2543
	public const string ID = "MeshTile";
}
