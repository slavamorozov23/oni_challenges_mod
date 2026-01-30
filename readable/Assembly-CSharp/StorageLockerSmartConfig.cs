using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200043F RID: 1087
public class StorageLockerSmartConfig : IBuildingConfig
{
	// Token: 0x06001690 RID: 5776 RVA: 0x00080460 File Offset: 0x0007E660
	public override BuildingDef CreateBuildingDef()
	{
		string id = "StorageLockerSmart";
		int width = 1;
		int height = 2;
		string anim = "smartstoragelocker_kanim";
		int hitpoints = 30;
		float construction_time = 60f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = OverlayModes.Logic.ID;
		buildingDef.RequiresPowerInput = true;
		buildingDef.AddLogicPowerPort = false;
		buildingDef.EnergyConsumptionWhenActive = 60f;
		buildingDef.ExhaustKilowattsWhenActive = 0.125f;
		buildingDef.LogicOutputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort(FilteredStorage.FULL_PORT_ID, new CellOffset(0, 1), STRINGS.BUILDINGS.PREFABS.STORAGELOCKERSMART.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.STORAGELOCKERSMART.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.STORAGELOCKERSMART.LOGIC_PORT_INACTIVE, true, false)
		};
		buildingDef.AddSearchTerms(SEARCH_TERMS.STORAGE);
		return buildingDef;
	}

	// Token: 0x06001691 RID: 5777 RVA: 0x00080540 File Offset: 0x0007E740
	public override void DoPostConfigureComplete(GameObject go)
	{
		SoundEventVolumeCache.instance.AddVolume("storagelocker_kanim", "StorageLocker_Hit_metallic_low", NOISE_POLLUTION.NOISY.TIER1);
		Prioritizable.AddRef(go);
		Storage storage = go.AddOrGet<Storage>();
		storage.showInUI = true;
		storage.allowItemRemoval = true;
		storage.showDescriptor = true;
		storage.storageFilters = STORAGEFILTERS.STORAGE_LOCKERS_STANDARD;
		storage.storageFullMargin = STORAGE.STORAGE_LOCKER_FILLED_MARGIN;
		storage.fetchCategory = Storage.FetchCategory.GeneralStorage;
		storage.showCapacityStatusItem = true;
		storage.showCapacityAsMainStatus = true;
		go.AddOrGet<CopyBuildingSettings>().copyGroupTag = GameTags.StorageLocker;
		go.AddOrGet<StorageLockerSmart>();
		go.AddOrGet<UserNameable>();
		go.AddOrGetDef<StorageController.Def>();
		go.AddOrGetDef<RocketUsageRestriction.Def>();
	}

	// Token: 0x04000D63 RID: 3427
	public const string ID = "StorageLockerSmart";
}
