using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000281 RID: 641
public class JuicerConfig : IBuildingConfig
{
	// Token: 0x06000D08 RID: 3336 RVA: 0x0004D5B8 File Offset: 0x0004B7B8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Juicer";
		int width = 3;
		int height = 4;
		string anim = "juicer_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.Floodable = true;
		buildingDef.AudioCategory = "Metal";
		buildingDef.Overheatable = true;
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.UtilityInputOffset = new CellOffset(1, 1);
		buildingDef.RequiresPowerInput = true;
		buildingDef.PowerInputOffset = new CellOffset(1, 0);
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
		buildingDef.AddSearchTerms(SEARCH_TERMS.MORALE);
		return buildingDef;
	}

	// Token: 0x06000D09 RID: 3337 RVA: 0x0004D670 File Offset: 0x0004B870
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		KPrefabID component = go.GetComponent<KPrefabID>();
		component.AddTag(RoomConstraints.ConstraintTags.RecBuilding, false);
		Storage storage = go.AddOrGet<Storage>();
		storage.SetDefaultStoredItemModifiers(Storage.StandardFabricatorStorage);
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Water).tag;
		conduitConsumer.capacityKG = 2f;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		ManualDeliveryKG manualDeliveryKG = go.AddComponent<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = MushroomConfig.ID.ToTag();
		manualDeliveryKG.capacity = 10f;
		manualDeliveryKG.refillMass = 5f;
		manualDeliveryKG.MinimumMass = 1f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
		ManualDeliveryKG manualDeliveryKG2 = go.AddComponent<ManualDeliveryKG>();
		manualDeliveryKG2.SetStorage(storage);
		manualDeliveryKG2.RequestedItemTag = PrickleFruitConfig.ID.ToTag();
		manualDeliveryKG2.capacity = 10f;
		manualDeliveryKG2.refillMass = 5f;
		manualDeliveryKG2.MinimumMass = 1f;
		manualDeliveryKG2.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
		ManualDeliveryKG manualDeliveryKG3 = go.AddComponent<ManualDeliveryKG>();
		manualDeliveryKG3.SetStorage(storage);
		manualDeliveryKG3.RequestedItemTag = "BasicPlantFood".ToTag();
		manualDeliveryKG3.capacity = 10f;
		manualDeliveryKG3.refillMass = 5f;
		manualDeliveryKG3.MinimumMass = 1f;
		manualDeliveryKG3.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
		go.AddOrGet<JuicerWorkable>().basePriority = RELAXATION.PRIORITY.TIER5;
		EdiblesManager.FoodInfo foodInfo = EdiblesManager.GetFoodInfo(MushroomConfig.ID);
		EdiblesManager.FoodInfo foodInfo2 = EdiblesManager.GetFoodInfo(PrickleFruitConfig.ID);
		EdiblesManager.FoodInfo foodInfo3 = EdiblesManager.GetFoodInfo("BasicPlantFood");
		Juicer juicer = go.AddOrGet<Juicer>();
		juicer.ingredientTags = new Tag[]
		{
			MushroomConfig.ID.ToTag(),
			PrickleFruitConfig.ID.ToTag(),
			"BasicPlantFood".ToTag()
		};
		juicer.ingredientMassesPerUse = new float[]
		{
			300000f / foodInfo.CaloriesPerUnit,
			600000f / foodInfo2.CaloriesPerUnit,
			500000f / foodInfo3.CaloriesPerUnit
		};
		juicer.specificEffect = "Juicer";
		juicer.trackingEffect = "RecentlyRecDrink";
		juicer.waterMassPerUse = 1f;
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.RecRoom.Id;
		roomTracker.requirement = RoomTracker.Requirement.Recommended;
		go.AddOrGetDef<RocketUsageRestriction.Def>();
		component.prefabInitFn += this.OnInit;
	}

	// Token: 0x06000D0A RID: 3338 RVA: 0x0004D8F4 File Offset: 0x0004BAF4
	private void OnInit(GameObject go)
	{
		JuicerWorkable component = go.GetComponent<JuicerWorkable>();
		KAnimFile[] value = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_juicer_kanim")
		};
		component.workerTypeOverrideAnims.Add(MinionConfig.ID, value);
		component.workerTypeOverrideAnims.Add(BionicMinionConfig.ID, new KAnimFile[]
		{
			Assets.GetAnim("anim_bionic_interacts_juicer_kanim")
		});
	}

	// Token: 0x06000D0B RID: 3339 RVA: 0x0004D964 File Offset: 0x0004BB64
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x040008F3 RID: 2291
	public const string ID = "Juicer";

	// Token: 0x040008F4 RID: 2292
	public const float BERRY_CALS = 600000f;

	// Token: 0x040008F5 RID: 2293
	public const float MUSHROOM_CALS = 300000f;

	// Token: 0x040008F6 RID: 2294
	public const float LICE_CALS = 500000f;

	// Token: 0x040008F7 RID: 2295
	public const float WATER_MASS_PER_USE = 1f;
}
