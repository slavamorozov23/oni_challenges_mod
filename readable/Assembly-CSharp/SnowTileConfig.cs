using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200041A RID: 1050
public class SnowTileConfig : IBuildingConfig
{
	// Token: 0x060015A9 RID: 5545 RVA: 0x0007BE14 File Offset: 0x0007A014
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SnowTile";
		int width = 1;
		int height = 1;
		string anim = "floor_snow_kanim";
		int hitpoints = 100;
		float construction_time = 3f;
		float[] construction_mass = new float[]
		{
			30f
		};
		string[] construction_materials = new string[]
		{
			this.CONSTRUCTION_ELEMENT.ToString()
		};
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Tile;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, none, 0.2f);
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
		buildingDef.BlockTileAtlas = Assets.GetTextureAtlas("tiles_snow");
		buildingDef.BlockTilePlaceAtlas = Assets.GetTextureAtlas("tiles_snow_place");
		buildingDef.BlockTileMaterial = Assets.GetMaterial("tiles_solid");
		buildingDef.DecorBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_snow_decor_info");
		buildingDef.DecorPlaceBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_snow_decor_place_info");
		buildingDef.Temperature = 263.15f;
		buildingDef.AddSearchTerms(SEARCH_TERMS.TILE);
		buildingDef.DragBuild = true;
		return buildingDef;
	}

	// Token: 0x060015AA RID: 5546 RVA: 0x0007BF3C File Offset: 0x0007A13C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
		simCellOccupier.doReplaceElement = true;
		simCellOccupier.strengthMultiplier = 1.5f;
		simCellOccupier.notifyOnMelt = true;
		go.AddOrGet<TileTemperature>();
		go.AddOrGet<KAnimGridTileVisualizer>().blockTileConnectorID = SnowTileConfig.BlockTileConnectorID;
		go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
		KPrefabID component = go.GetComponent<KPrefabID>();
		component.prefabInitFn += this.BuildingComplete_OnInit;
		component.prefabSpawnFn += this.BuildingComplete_OnSpawn;
	}

	// Token: 0x060015AB RID: 5547 RVA: 0x0007BFCE File Offset: 0x0007A1CE
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x060015AC RID: 5548 RVA: 0x0007BFD5 File Offset: 0x0007A1D5
	public override void DoPostConfigureComplete(GameObject go)
	{
		GeneratedBuildings.RemoveLoopingSounds(go);
		go.GetComponent<KPrefabID>().AddTag(GameTags.FloorTiles, false);
	}

	// Token: 0x060015AD RID: 5549 RVA: 0x0007BFF0 File Offset: 0x0007A1F0
	private void BuildingComplete_OnInit(GameObject instance)
	{
		PrimaryElement component = instance.GetComponent<PrimaryElement>();
		component.SetElement(this.STABLE_SNOW_ELEMENT, true);
		Element element = component.Element;
		Deconstructable component2 = instance.GetComponent<Deconstructable>();
		if (component2 != null)
		{
			component2.constructionElements = new Tag[]
			{
				this.CONSTRUCTION_ELEMENT.CreateTag()
			};
		}
	}

	// Token: 0x060015AE RID: 5550 RVA: 0x0007C044 File Offset: 0x0007A244
	private void BuildingComplete_OnSpawn(GameObject instance)
	{
		instance.GetComponent<PrimaryElement>().SetElement(this.STABLE_SNOW_ELEMENT, true);
		Deconstructable component = instance.GetComponent<Deconstructable>();
		if (component != null)
		{
			component.constructionElements = new Tag[]
			{
				this.CONSTRUCTION_ELEMENT.CreateTag()
			};
		}
	}

	// Token: 0x060015AF RID: 5551 RVA: 0x0007C091 File Offset: 0x0007A291
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		go.AddOrGet<KAnimGridTileVisualizer>();
	}

	// Token: 0x04000CEF RID: 3311
	public const string ID = "SnowTile";

	// Token: 0x04000CF0 RID: 3312
	public static readonly int BlockTileConnectorID = Hash.SDBMLower("tiles_snow_tops");

	// Token: 0x04000CF1 RID: 3313
	private SimHashes CONSTRUCTION_ELEMENT = SimHashes.Snow;

	// Token: 0x04000CF2 RID: 3314
	private SimHashes STABLE_SNOW_ELEMENT = SimHashes.StableSnow;
}
