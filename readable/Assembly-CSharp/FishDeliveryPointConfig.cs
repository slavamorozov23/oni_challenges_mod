using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200017A RID: 378
public class FishDeliveryPointConfig : IBuildingConfig
{
	// Token: 0x06000757 RID: 1879 RVA: 0x00032C28 File Offset: 0x00030E28
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("FishDeliveryPoint", 1, 3, "fishrelocator_kanim", 10, 10f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER1, MATERIALS.RAW_METALS, 1600f, BuildLocationRule.Anywhere, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.AudioCategory = "Metal";
		buildingDef.Entombable = true;
		buildingDef.Floodable = true;
		buildingDef.ForegroundLayer = Grid.SceneLayer.TileMain;
		buildingDef.ViewMode = OverlayModes.Rooms.ID;
		buildingDef.AddSearchTerms(SEARCH_TERMS.RANCHING);
		buildingDef.AddSearchTerms(SEARCH_TERMS.CRITTER);
		return buildingDef;
	}

	// Token: 0x06000758 RID: 1880 RVA: 0x00032CB8 File Offset: 0x00030EB8
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(GameTags.CodexCategories.CreatureRelocator, false);
		Storage storage = go.AddOrGet<Storage>();
		storage.allowItemRemoval = false;
		storage.showDescriptor = true;
		storage.storageFilters = STORAGEFILTERS.SWIMMING_CREATURES;
		storage.workAnims = new HashedString[]
		{
			new HashedString("working_pre")
		};
		storage.workAnimPlayMode = KAnim.PlayMode.Once;
		storage.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_fishrelocator_kanim")
		};
		storage.synchronizeAnims = false;
		storage.useGunForDelivery = false;
		storage.allowSettingOnlyFetchMarkedItems = false;
		storage.faceTargetWhenWorking = false;
		CreatureDeliveryPoint creatureDeliveryPoint = go.AddOrGet<CreatureDeliveryPoint>();
		creatureDeliveryPoint.deliveryOffsets = new CellOffset[]
		{
			new CellOffset(0, 1)
		};
		creatureDeliveryPoint.spawnOffset = new CellOffset(0, -1);
		creatureDeliveryPoint.largeCritterSpawnOffset = new CellOffset(0, -2);
		creatureDeliveryPoint.playAnimsOnFetch = true;
		BaggableCritterCapacityTracker baggableCritterCapacityTracker = go.AddOrGet<BaggableCritterCapacityTracker>();
		baggableCritterCapacityTracker.maximumCreatures = 20;
		baggableCritterCapacityTracker.cavityOffset = CellOffset.down;
		baggableCritterCapacityTracker.requireLiquidOffset = true;
		go.AddOrGet<TreeFilterable>();
	}

	// Token: 0x06000759 RID: 1881 RVA: 0x00032DBD File Offset: 0x00030FBD
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<MakeBaseSolid.Def>().solidOffsets = new CellOffset[]
		{
			new CellOffset(0, 0)
		};
	}

	// Token: 0x040005A1 RID: 1441
	public const string ID = "FishDeliveryPoint";
}
