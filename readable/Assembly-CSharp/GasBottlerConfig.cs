using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x02000226 RID: 550
public class GasBottlerConfig : IBuildingConfig
{
	// Token: 0x06000B23 RID: 2851 RVA: 0x0004307C File Offset: 0x0004127C
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("GasBottler", 3, 2, "gas_bottler_kanim", 100, 120f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER4, MATERIALS.ALL_METALS, 800f, BuildLocationRule.OnFloor, BUILDINGS.DECOR.PENALTY.TIER1, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.InputConduitType = ConduitType.Gas;
		buildingDef.Floodable = false;
		buildingDef.ViewMode = OverlayModes.GasConduits.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.GasVentIDs, "GasBottler");
		return buildingDef;
	}

	// Token: 0x06000B24 RID: 2852 RVA: 0x00043100 File Offset: 0x00041300
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Storage storage = BuildingTemplates.CreateDefaultStorage(go, false);
		storage.showDescriptor = true;
		storage.storageFilters = STORAGEFILTERS.GASES;
		storage.capacityKg = 200f;
		storage.SetDefaultStoredItemModifiers(GasBottlerConfig.GasBottlerStoredItemModifiers);
		storage.allowItemRemoval = false;
		go.AddTag(GameTags.GasSource);
		DropAllWorkable dropAllWorkable = go.AddOrGet<DropAllWorkable>();
		dropAllWorkable.removeTags = new List<Tag>
		{
			GameTags.GasSource
		};
		dropAllWorkable.resetTargetWorkableOnCompleteWork = true;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.storage = storage;
		conduitConsumer.conduitType = ConduitType.Gas;
		conduitConsumer.ignoreMinMassCheck = true;
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.alwaysConsume = true;
		conduitConsumer.capacityKG = 200f;
		conduitConsumer.keepZeroMassObject = false;
		Bottler bottler = go.AddOrGet<Bottler>();
		bottler.storage = storage;
		bottler.workTime = 9f;
		bottler.userMaxCapacity = 25f;
		bottler.consumer = conduitConsumer;
	}

	// Token: 0x06000B25 RID: 2853 RVA: 0x000431D6 File Offset: 0x000413D6
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits, false);
	}

	// Token: 0x040007C7 RID: 1991
	public const string ID = "GasBottler";

	// Token: 0x040007C8 RID: 1992
	private const ConduitType CONDUIT_TYPE = ConduitType.Gas;

	// Token: 0x040007C9 RID: 1993
	private const int WIDTH = 3;

	// Token: 0x040007CA RID: 1994
	private const int HEIGHT = 2;

	// Token: 0x040007CB RID: 1995
	private const float DEFAULT_FILL_LEVEL = 25f;

	// Token: 0x040007CC RID: 1996
	private const float CAPACITY = 200f;

	// Token: 0x040007CD RID: 1997
	private static readonly List<Storage.StoredItemModifier> GasBottlerStoredItemModifiers = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Hide
	};
}
