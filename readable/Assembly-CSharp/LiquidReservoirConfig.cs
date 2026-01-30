using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002A8 RID: 680
public class LiquidReservoirConfig : IBuildingConfig
{
	// Token: 0x06000DCB RID: 3531 RVA: 0x00051BAC File Offset: 0x0004FDAC
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("LiquidReservoir", 2, 3, "liquidreservoir_kanim", 100, 120f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4, MATERIALS.ALL_METALS, 800f, BuildLocationRule.OnFloor, TUNING.BUILDINGS.DECOR.PENALTY.TIER1, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.OutputConduitType = ConduitType.Liquid;
		buildingDef.Floodable = false;
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.UtilityInputOffset = new CellOffset(1, 2);
		buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
		buildingDef.LogicOutputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort(SmartReservoir.PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.SMARTRESERVOIR.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.SMARTRESERVOIR.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.SMARTRESERVOIR.LOGIC_PORT_INACTIVE, false, false)
		};
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.LiquidVentIDs, "LiquidReservoir");
		buildingDef.AddSearchTerms(SEARCH_TERMS.STORAGE);
		return buildingDef;
	}

	// Token: 0x06000DCC RID: 3532 RVA: 0x00051C98 File Offset: 0x0004FE98
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<Reservoir>();
		Storage storage = BuildingTemplates.CreateDefaultStorage(go, false);
		storage.showDescriptor = true;
		storage.allowItemRemoval = false;
		storage.storageFilters = STORAGEFILTERS.LIQUIDS;
		storage.capacityKg = 5000f;
		storage.SetDefaultStoredItemModifiers(GasReservoirConfig.ReservoirStoredItemModifiers);
		storage.showCapacityStatusItem = true;
		storage.showCapacityAsMainStatus = true;
		go.AddOrGet<SmartReservoir>();
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.ignoreMinMassCheck = true;
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.alwaysConsume = true;
		conduitConsumer.capacityKG = storage.capacityKg;
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.conduitType = ConduitType.Liquid;
		conduitDispenser.elementFilter = null;
	}

	// Token: 0x06000DCD RID: 3533 RVA: 0x00051D38 File Offset: 0x0004FF38
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<StorageController.Def>();
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits, false);
	}

	// Token: 0x04000943 RID: 2371
	public const string ID = "LiquidReservoir";

	// Token: 0x04000944 RID: 2372
	private const ConduitType CONDUIT_TYPE = ConduitType.Liquid;

	// Token: 0x04000945 RID: 2373
	private const int WIDTH = 2;

	// Token: 0x04000946 RID: 2374
	private const int HEIGHT = 3;
}
