using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200046F RID: 1135
public class WoodTileConfig : IBuildingConfig
{
	// Token: 0x060017B9 RID: 6073 RVA: 0x00086650 File Offset: 0x00084850
	public override BuildingDef CreateBuildingDef()
	{
		string id = "WoodTile";
		int width = 1;
		int height = 1;
		string anim = "floor_wood_kanim";
		int hitpoints = 100;
		float construction_time = 3f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] woods = MATERIALS.WOODS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Tile;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, woods, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
		BuildingTemplates.CreateFoundationTileDef(buildingDef);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.Entombable = false;
		buildingDef.UseStructureTemperature = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.SceneLayer = Grid.SceneLayer.TileMain;
		buildingDef.isKAnimTile = true;
		buildingDef.BlockTileAtlas = Assets.GetTextureAtlas("tiles_wood");
		buildingDef.BlockTilePlaceAtlas = Assets.GetTextureAtlas("tiles_wood_place");
		buildingDef.BlockTileMaterial = Assets.GetMaterial("tiles_solid");
		buildingDef.DecorBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_wood_decor_info");
		buildingDef.DecorPlaceBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_wood_decor_place_info");
		buildingDef.POIUnlockable = true;
		buildingDef.AddSearchTerms(SEARCH_TERMS.TILE);
		buildingDef.AddSearchTerms(SEARCH_TERMS.LUMBER);
		buildingDef.AddSearchTerms(SEARCH_TERMS.MORALE);
		buildingDef.DragBuild = true;
		return buildingDef;
	}

	// Token: 0x060017BA RID: 6074 RVA: 0x00086778 File Offset: 0x00084978
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
		simCellOccupier.doReplaceElement = true;
		simCellOccupier.strengthMultiplier = 1.5f;
		simCellOccupier.movementSpeedMultiplier = DUPLICANTSTATS.MOVEMENT_MODIFIERS.BONUS_2;
		simCellOccupier.notifyOnMelt = true;
		go.AddOrGet<TileTemperature>();
		go.AddOrGet<KAnimGridTileVisualizer>().blockTileConnectorID = WoodTileConfig.BlockTileConnectorID;
		go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
	}

	// Token: 0x060017BB RID: 6075 RVA: 0x000867EC File Offset: 0x000849EC
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x060017BC RID: 6076 RVA: 0x000867F3 File Offset: 0x000849F3
	public override void DoPostConfigureComplete(GameObject go)
	{
		GeneratedBuildings.RemoveLoopingSounds(go);
		go.GetComponent<KPrefabID>().AddTag(GameTags.FloorTiles, false);
	}

	// Token: 0x060017BD RID: 6077 RVA: 0x0008680C File Offset: 0x00084A0C
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		go.AddOrGet<KAnimGridTileVisualizer>();
	}

	// Token: 0x04000DED RID: 3565
	public const string ID = "WoodTile";

	// Token: 0x04000DEE RID: 3566
	public static readonly int BlockTileConnectorID = Hash.SDBMLower("tiles_wood_tops");
}
