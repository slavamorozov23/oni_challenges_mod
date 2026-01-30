using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200043E RID: 1086
public class StorageLockerConfig : IBuildingConfig
{
	// Token: 0x0600168C RID: 5772 RVA: 0x00080348 File Offset: 0x0007E548
	public override BuildingDef CreateBuildingDef()
	{
		string id = "StorageLocker";
		int width = 1;
		int height = 2;
		string anim = "storagelocker_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] raw_MINERALS_OR_METALS = MATERIALS.RAW_MINERALS_OR_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS_OR_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.Overheatable = false;
		buildingDef.AddSearchTerms(SEARCH_TERMS.STORAGE);
		return buildingDef;
	}

	// Token: 0x0600168D RID: 5773 RVA: 0x000803B8 File Offset: 0x0007E5B8
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
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
		go.AddOrGet<StorageLocker>();
		go.AddOrGet<UserNameable>();
		go.AddOrGetDef<RocketUsageRestriction.Def>();
	}

	// Token: 0x0600168E RID: 5774 RVA: 0x0008044E File Offset: 0x0007E64E
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<StorageController.Def>();
	}

	// Token: 0x04000D62 RID: 3426
	public const string ID = "StorageLocker";
}
