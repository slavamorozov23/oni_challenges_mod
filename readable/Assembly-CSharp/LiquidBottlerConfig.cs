using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x02000293 RID: 659
public class LiquidBottlerConfig : IBuildingConfig
{
	// Token: 0x06000D67 RID: 3431 RVA: 0x0004FA00 File Offset: 0x0004DC00
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("LiquidBottler", 3, 2, "liquid_bottler_kanim", 100, 120f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER4, MATERIALS.ALL_METALS, 800f, BuildLocationRule.OnFloor, BUILDINGS.DECOR.PENALTY.TIER1, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.Floodable = false;
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.LiquidVentIDs, "LiquidBottler");
		return buildingDef;
	}

	// Token: 0x06000D68 RID: 3432 RVA: 0x0004FA84 File Offset: 0x0004DC84
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Storage storage = BuildingTemplates.CreateDefaultStorage(go, false);
		storage.showDescriptor = true;
		storage.storageFilters = STORAGEFILTERS.LIQUIDS;
		storage.capacityKg = 200f;
		storage.SetDefaultStoredItemModifiers(LiquidBottlerConfig.LiquidBottlerStoredItemModifiers);
		storage.allowItemRemoval = false;
		go.AddTag(GameTags.LiquidSource);
		DropAllWorkable dropAllWorkable = go.AddOrGet<DropAllWorkable>();
		dropAllWorkable.removeTags = new List<Tag>
		{
			GameTags.LiquidSource
		};
		dropAllWorkable.resetTargetWorkableOnCompleteWork = true;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.storage = storage;
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.ignoreMinMassCheck = true;
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.alwaysConsume = true;
		conduitConsumer.capacityKG = 200f;
		conduitConsumer.keepZeroMassObject = false;
		Bottler bottler = go.AddOrGet<Bottler>();
		bottler.storage = storage;
		bottler.workTime = 9f;
		bottler.consumer = conduitConsumer;
		bottler.userMaxCapacity = 200f;
	}

	// Token: 0x06000D69 RID: 3433 RVA: 0x0004FB5A File Offset: 0x0004DD5A
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits, false);
	}

	// Token: 0x04000918 RID: 2328
	public const string ID = "LiquidBottler";

	// Token: 0x04000919 RID: 2329
	private const ConduitType CONDUIT_TYPE = ConduitType.Liquid;

	// Token: 0x0400091A RID: 2330
	private const int WIDTH = 3;

	// Token: 0x0400091B RID: 2331
	private const int HEIGHT = 2;

	// Token: 0x0400091C RID: 2332
	private const float CAPACITY = 200f;

	// Token: 0x0400091D RID: 2333
	private static readonly List<Storage.StoredItemModifier> LiquidBottlerStoredItemModifiers = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Hide,
		Storage.StoredItemModifier.Seal
	};
}
