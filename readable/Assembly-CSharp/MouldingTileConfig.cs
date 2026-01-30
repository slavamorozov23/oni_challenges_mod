using System;
using TUNING;
using UnityEngine;

// Token: 0x02000367 RID: 871
public class MouldingTileConfig : IBuildingConfig
{
	// Token: 0x0600122A RID: 4650 RVA: 0x00069D78 File Offset: 0x00067F78
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MouldingTile";
		int width = 1;
		int height = 1;
		string anim = "floor_moulding_kanim";
		int hitpoints = 100;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Tile;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, new EffectorValues
		{
			amount = 10,
			radius = 1
		}, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.Entombable = false;
		buildingDef.UseStructureTemperature = false;
		buildingDef.IsFoundation = true;
		buildingDef.TileLayer = ObjectLayer.FoundationTile;
		buildingDef.ReplacementLayer = ObjectLayer.ReplacementTile;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.SceneLayer = Grid.SceneLayer.TileMain;
		buildingDef.ConstructionOffsetFilter = BuildingDef.ConstructionOffsetFilter_OneDown;
		buildingDef.isKAnimTile = true;
		buildingDef.BlockTileAtlas = Assets.GetTextureAtlas("tiles_moulding");
		buildingDef.BlockTilePlaceAtlas = Assets.GetTextureAtlas("tiles_moulding_place");
		buildingDef.BlockTileMaterial = Assets.GetMaterial("tiles_solid");
		buildingDef.DecorBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_bunker_tops_decor_info");
		buildingDef.DecorPlaceBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_bunker_tops_decor_place_info");
		buildingDef.Deprecated = true;
		return buildingDef;
	}

	// Token: 0x0600122B RID: 4651 RVA: 0x00069E98 File Offset: 0x00068098
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<SimCellOccupier>().doReplaceElement = true;
		go.AddOrGet<TileTemperature>();
		go.AddOrGet<KAnimGridTileVisualizer>().blockTileConnectorID = MouldingTileConfig.BlockTileConnectorID;
		go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
	}

	// Token: 0x0600122C RID: 4652 RVA: 0x00069EEF File Offset: 0x000680EF
	public override void DoPostConfigureComplete(GameObject go)
	{
		GeneratedBuildings.RemoveLoopingSounds(go);
	}

	// Token: 0x0600122D RID: 4653 RVA: 0x00069EF7 File Offset: 0x000680F7
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		go.AddOrGet<KAnimGridTileVisualizer>();
	}

	// Token: 0x04000B7A RID: 2938
	public const string ID = "MouldingTile";

	// Token: 0x04000B7B RID: 2939
	public static readonly int BlockTileConnectorID = Hash.SDBMLower("tiles_bunker_tops");
}
