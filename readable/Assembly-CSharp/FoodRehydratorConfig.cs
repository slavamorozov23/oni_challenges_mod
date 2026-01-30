using System;
using FoodRehydrator;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000217 RID: 535
public class FoodRehydratorConfig : IBuildingConfig
{
	// Token: 0x06000ACC RID: 2764 RVA: 0x00041788 File Offset: 0x0003F988
	private static Effect ConstructRehydrationEffect()
	{
		Effect effect = new Effect("RehydratedFoodConsumed", "RehydratedFoodConsumed", STRINGS.ITEMS.DEHYDRATEDFOODPACKAGE.CONSUMED, 600f, false, false, true, null, -1f, 0f, null, "");
		effect.Add(new AttributeModifier(Db.Get().Attributes.QualityOfLife.Id, -1f, STRINGS.ITEMS.DEHYDRATEDFOODPACKAGE.CONSUMED, false, false, true));
		return effect;
	}

	// Token: 0x06000ACD RID: 2765 RVA: 0x000417F8 File Offset: 0x0003F9F8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "FoodRehydrator";
		int width = 1;
		int height = 2;
		string anim = "Rehydrator_kanim";
		int hitpoints = 10;
		float construction_time = 120f;
		float[] construction_mass = new float[]
		{
			TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2[0],
			TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER1[0]
		};
		string[] construction_materials = new string[]
		{
			"RefinedMetal",
			"Plastic"
		};
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER0, none, 0.2f);
		buildingDef.Overheatable = true;
		buildingDef.Floodable = false;
		buildingDef.AudioCategory = "Plastic";
		buildingDef.AudioSize = "small";
		buildingDef.RequiresPowerInput = true;
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.EnergyConsumptionWhenActive = 60f;
		buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AddSearchTerms(SEARCH_TERMS.FOOD);
		return buildingDef;
	}

	// Token: 0x06000ACE RID: 2766 RVA: 0x000418D4 File Offset: 0x0003FAD4
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Storage storage = go.AddComponent<Storage>();
		storage.capacityKg = 5f;
		storage.showInUI = true;
		storage.showDescriptor = false;
		storage.allowItemRemoval = false;
		storage.showCapacityStatusItem = true;
		storage.showCapacityAsMainStatus = true;
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.capacity = 5f;
		manualDeliveryKG.refillMass = 5f;
		manualDeliveryKG.MinimumMass = 1f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.StorageFetch.Id;
		manualDeliveryKG.RequestedItemTag = GameTags.Dehydrated;
		manualDeliveryKG.operationalRequirement = Operational.State.Functional;
		Storage storage2 = go.AddComponent<Storage>();
		storage2.showCapacityStatusItem = true;
		storage2.allowItemRemoval = false;
		storage2.showCapacityStatusItem = true;
		storage2.capacityKg = 20f;
		ConduitConsumer conduitConsumer = go.AddComponent<ConduitConsumer>();
		conduitConsumer.capacityTag = FoodRehydratorConfig.REHYDRATION_TAG;
		conduitConsumer.capacityKG = storage2.capacityKg;
		conduitConsumer.storage = storage2;
		conduitConsumer.alwaysConsume = true;
		Prioritizable.AddRef(go);
		go.AddOrGet<AccessabilityManager>();
		go.AddOrGet<DehydratedManager>();
		go.AddOrGet<ResourceRequirementMonitor>();
		go.AddOrGetDef<FoodRehydratorSM.Def>();
		go.AddOrGet<UserNameable>();
		go.AddOrGetDef<RocketUsageRestriction.Def>();
	}

	// Token: 0x06000ACF RID: 2767 RVA: 0x000419F5 File Offset: 0x0003FBF5
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x0400079F RID: 1951
	public const string ID = "FoodRehydrator";

	// Token: 0x040007A0 RID: 1952
	public static Tag REHYDRATION_TAG = GameTags.Water;

	// Token: 0x040007A1 RID: 1953
	public const float REHYDRATION_COST = 1f;

	// Token: 0x040007A2 RID: 1954
	public const float REHYDRATOR_PACKAGES_CAPACITY = 5f;

	// Token: 0x040007A3 RID: 1955
	public const float REHYDRATION_WORK_TIME = 5f;

	// Token: 0x040007A4 RID: 1956
	public static Effect RehydrationEffect = FoodRehydratorConfig.ConstructRehydrationEffect();

	// Token: 0x040007A5 RID: 1957
	public const string REHYDRATION_DEBUFF_ID = "RehydratedFoodConsumed";

	// Token: 0x040007A6 RID: 1958
	public const string REHDYRATION_DEBUFF_NAME = "RehydratedFoodConsumed";

	// Token: 0x040007A7 RID: 1959
	public const float REHYDRATION_DEBUFF_DURATION = 600f;

	// Token: 0x040007A8 RID: 1960
	public const float REHYDRATION_DEBUFF_EFFECT = -1f;
}
