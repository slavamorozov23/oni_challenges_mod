using System;
using TUNING;
using UnityEngine;

// Token: 0x02000065 RID: 101
public class CreatureDeliveryPointConfig : IBuildingConfig
{
	// Token: 0x060001E3 RID: 483 RVA: 0x0000DD14 File Offset: 0x0000BF14
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("CreatureDeliveryPoint", 1, 3, "relocator_dropoff_kanim", 10, 10f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER1, MATERIALS.RAW_METALS, 1600f, BuildLocationRule.OnFloor, BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.AudioCategory = "Metal";
		buildingDef.ViewMode = OverlayModes.Rooms.ID;
		buildingDef.Deprecated = true;
		return buildingDef;
	}

	// Token: 0x060001E4 RID: 484 RVA: 0x0000DD78 File Offset: 0x0000BF78
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(GameTags.CodexCategories.CreatureRelocator, false);
		Storage storage = go.AddOrGet<Storage>();
		storage.allowItemRemoval = false;
		storage.showDescriptor = true;
		storage.storageFilters = STORAGEFILTERS.BAGABLE_CREATURES;
		storage.workAnims = new HashedString[]
		{
			"place",
			"release"
		};
		storage.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_restrain_creature_kanim")
		};
		storage.workAnimPlayMode = KAnim.PlayMode.Once;
		storage.synchronizeAnims = false;
		storage.useGunForDelivery = false;
		storage.allowSettingOnlyFetchMarkedItems = false;
		go.AddOrGet<CreatureDeliveryPoint>();
		go.AddOrGet<BaggableCritterCapacityTracker>().maximumCreatures = 20;
		go.AddOrGet<FixedCapturePoint.AutoWrangleCapture>();
		go.AddOrGet<TreeFilterable>();
	}

	// Token: 0x060001E5 RID: 485 RVA: 0x0000DE40 File Offset: 0x0000C040
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<FixedCapturePoint.Def>().isAmountStoredOverCapacity = delegate(FixedCapturePoint.Instance smi, FixedCapturableMonitor.Instance capturable)
		{
			TreeFilterable component = smi.GetComponent<TreeFilterable>();
			IUserControlledCapacity component2 = smi.GetComponent<IUserControlledCapacity>();
			float amountStored = component2.AmountStored;
			float userMaxCapacity = component2.UserMaxCapacity;
			return !component.ContainsTag(capturable.PrefabTag) || amountStored > userMaxCapacity;
		};
	}

	// Token: 0x04000133 RID: 307
	public const string ID = "CreatureDeliveryPoint";
}
