using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003DC RID: 988
public class RationBoxConfig : IBuildingConfig
{
	// Token: 0x0600144B RID: 5195 RVA: 0x00073464 File Offset: 0x00071664
	public override BuildingDef CreateBuildingDef()
	{
		string id = "RationBox";
		int width = 2;
		int height = 2;
		string anim = "rationbox_kanim";
		int hitpoints = 10;
		float construction_time = 10f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER0, none, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.Floodable = false;
		buildingDef.AudioCategory = "Metal";
		SoundEventVolumeCache.instance.AddVolume("rationbox_kanim", "RationBox_open", NOISE_POLLUTION.NOISY.TIER1);
		SoundEventVolumeCache.instance.AddVolume("rationbox_kanim", "RationBox_close", NOISE_POLLUTION.NOISY.TIER1);
		return buildingDef;
	}

	// Token: 0x0600144C RID: 5196 RVA: 0x000734F8 File Offset: 0x000716F8
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Prioritizable.AddRef(go);
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 150f;
		storage.showInUI = true;
		storage.showDescriptor = true;
		storage.storageFilters = STORAGEFILTERS.FOOD;
		storage.allowItemRemoval = true;
		storage.storageFullMargin = STORAGE.STORAGE_LOCKER_FILLED_MARGIN;
		storage.fetchCategory = Storage.FetchCategory.GeneralStorage;
		storage.showCapacityStatusItem = true;
		storage.showCapacityAsMainStatus = true;
		go.AddOrGet<TreeFilterable>().allResourceFilterLabelString = UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.ALLBUTTON_EDIBLES;
		go.AddOrGet<RationBox>();
		go.AddOrGet<UserNameable>();
		go.AddOrGetDef<RocketUsageRestriction.Def>();
	}

	// Token: 0x0600144D RID: 5197 RVA: 0x00073585 File Offset: 0x00071785
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<StorageController.Def>();
	}

	// Token: 0x04000C42 RID: 3138
	public const string ID = "RationBox";
}
