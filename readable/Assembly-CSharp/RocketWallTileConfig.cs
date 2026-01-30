using System;
using TUNING;
using UnityEngine;

// Token: 0x020003FB RID: 1019
public class RocketWallTileConfig : IBuildingConfig
{
	// Token: 0x06001504 RID: 5380 RVA: 0x000787CF File Offset: 0x000769CF
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001505 RID: 5381 RVA: 0x000787D8 File Offset: 0x000769D8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "RocketWallTile";
		int width = 1;
		int height = 1;
		string anim = "floor_rocket_kanim";
		int hitpoints = 1000;
		float construction_time = 60f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] construction_materials = new string[]
		{
			SimHashes.Steel.ToString()
		};
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.Tile;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER0, none, 0.2f);
		buildingDef.DebugOnly = true;
		BuildingTemplates.CreateFoundationTileDef(buildingDef);
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.Overheatable = false;
		buildingDef.UseStructureTemperature = false;
		buildingDef.Replaceable = false;
		buildingDef.Invincible = true;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.SceneLayer = Grid.SceneLayer.TileMain;
		buildingDef.isKAnimTile = true;
		buildingDef.BlockTileAtlas = Assets.GetTextureAtlas("tiles_rocket_wall_int");
		buildingDef.BlockTilePlaceAtlas = Assets.GetTextureAtlas("tiles_rocket_wall_int_place");
		buildingDef.BlockTileMaterial = Assets.GetMaterial("tiles_solid");
		buildingDef.DecorBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_rocket_wall_ext_decor_info");
		buildingDef.DecorPlaceBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_rocket_wall_ext_place_decor_info");
		buildingDef.ConstructionOffsetFilter = BuildingDef.ConstructionOffsetFilter_OneDown;
		buildingDef.ShowInBuildMenu = false;
		return buildingDef;
	}

	// Token: 0x06001506 RID: 5382 RVA: 0x00078904 File Offset: 0x00076B04
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
		simCellOccupier.strengthMultiplier = 10f;
		simCellOccupier.notifyOnMelt = true;
		go.AddOrGet<TileTemperature>();
		go.AddOrGet<KAnimGridTileVisualizer>().blockTileConnectorID = RocketWallTileConfig.BlockTileConnectorID;
		go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
	}

	// Token: 0x06001507 RID: 5383 RVA: 0x00078968 File Offset: 0x00076B68
	public override void DoPostConfigureComplete(GameObject go)
	{
		GeneratedBuildings.RemoveLoopingSounds(go);
		KPrefabID component = go.GetComponent<KPrefabID>();
		component.AddTag(GameTags.Bunker, false);
		component.AddTag(GameTags.FloorTiles, false);
		component.AddTag(GameTags.RocketEnvelopeTile, false);
		component.AddTag(GameTags.NoRocketRefund, false);
		go.GetComponent<Deconstructable>().allowDeconstruction = false;
	}

	// Token: 0x06001508 RID: 5384 RVA: 0x000789BC File Offset: 0x00076BBC
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		go.AddOrGet<KAnimGridTileVisualizer>();
	}

	// Token: 0x04000CBA RID: 3258
	public const string ID = "RocketWallTile";

	// Token: 0x04000CBB RID: 3259
	public static readonly int BlockTileConnectorID = Hash.SDBMLower("tiles_rocket_wall_int");
}
