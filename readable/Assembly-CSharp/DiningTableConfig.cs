using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200007C RID: 124
public class DiningTableConfig : IBuildingConfig
{
	// Token: 0x06000253 RID: 595 RVA: 0x00010404 File Offset: 0x0000E604
	public override BuildingDef CreateBuildingDef()
	{
		string id = "DiningTable";
		int width = 1;
		int height = 1;
		string anim = "diningtable_kanim";
		int hitpoints = 10;
		float construction_time = 10f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
		buildingDef.WorkTime = 20f;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AddSearchTerms(SEARCH_TERMS.DINING);
		return buildingDef;
	}

	// Token: 0x06000254 RID: 596 RVA: 0x00010477 File Offset: 0x0000E677
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.DiningTableType, false);
		go.AddOrGet<MessStation>();
		go.AddOrGet<AnimTileable>();
		go.AddOrGetDef<RocketUsageRestriction.Def>();
	}

	// Token: 0x06000255 RID: 597 RVA: 0x000104A8 File Offset: 0x0000E6A8
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<KAnimControllerBase>().initialAnim = "off";
		Ownable ownable = go.AddOrGet<Ownable>();
		ownable.slotID = Db.Get().AssignableSlots.MessStation.Id;
		ownable.canBePublic = true;
		ownable.priority = 10;
		Storage storage = BuildingTemplates.CreateDefaultStorage(go, false);
		storage.showInUI = true;
		storage.capacityKg = TableSaltTuning.SALTSHAKERSTORAGEMASS;
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = TableSaltConfig.ID.ToTag();
		manualDeliveryKG.capacity = TableSaltTuning.SALTSHAKERSTORAGEMASS;
		manualDeliveryKG.refillMass = TableSaltTuning.CONSUMABLE_RATE;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.FoodFetch.IdHash;
		manualDeliveryKG.ShowStatusItem = false;
		go.AddOrGet<Reservable>();
	}

	// Token: 0x04000179 RID: 377
	public const string ID = "DiningTable";
}
