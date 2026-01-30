using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002F3 RID: 755
public class MilkFeederConfig : IBuildingConfig
{
	// Token: 0x06000F5C RID: 3932 RVA: 0x00059784 File Offset: 0x00057984
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MilkFeeder";
		int width = 3;
		int height = 3;
		string anim = "critter_milk_feeder_kanim";
		int hitpoints = 100;
		float construction_time = 120f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, none, 0.2f);
		buildingDef.AudioCategory = "Metal";
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		buildingDef.AddSearchTerms(SEARCH_TERMS.RANCHING);
		buildingDef.AddSearchTerms(SEARCH_TERMS.CRITTER);
		return buildingDef;
	}

	// Token: 0x06000F5D RID: 3933 RVA: 0x00059822 File Offset: 0x00057A22
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
	}

	// Token: 0x06000F5E RID: 3934 RVA: 0x00059824 File Offset: 0x00057A24
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Prioritizable.AddRef(go);
		go.AddOrGet<LogicOperationalController>();
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.CreaturePen.Id;
		roomTracker.requirement = RoomTracker.Requirement.Required;
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 80f;
		storage.showInUI = true;
		storage.showDescriptor = true;
		storage.allowItemRemoval = false;
		storage.allowSettingOnlyFetchMarkedItems = false;
		storage.showCapacityStatusItem = true;
		storage.showCapacityAsMainStatus = true;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.consumptionRate = 10f;
		conduitConsumer.capacityTag = GameTagExtensions.Create(SimHashes.Milk);
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		conduitConsumer.storage = storage;
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RanchStationType, false);
	}

	// Token: 0x06000F5F RID: 3935 RVA: 0x000598ED File Offset: 0x00057AED
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<MilkFeeder.Def>();
	}

	// Token: 0x06000F60 RID: 3936 RVA: 0x000598F6 File Offset: 0x00057AF6
	public override void ConfigurePost(BuildingDef def)
	{
	}

	// Token: 0x04000A0A RID: 2570
	public const string ID = "MilkFeeder";

	// Token: 0x04000A0B RID: 2571
	public const string HAD_CONSUMED_MILK_RECENTLY_EFFECT_ID = "HadMilk";

	// Token: 0x04000A0C RID: 2572
	public const float EFFECT_DURATION_IN_SECONDS = 600f;

	// Token: 0x04000A0D RID: 2573
	public static readonly CellOffset DRINK_FROM_OFFSET = new CellOffset(1, 0);

	// Token: 0x04000A0E RID: 2574
	public static readonly Tag MILK_TAG = SimHashes.Milk.CreateTag();

	// Token: 0x04000A0F RID: 2575
	public const float UNITS_OF_MILK_CONSUMED_PER_FEEDING = 5f;
}
