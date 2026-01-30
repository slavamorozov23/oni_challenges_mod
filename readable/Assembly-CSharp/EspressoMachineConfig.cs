using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000093 RID: 147
public class EspressoMachineConfig : IBuildingConfig
{
	// Token: 0x060002ED RID: 749 RVA: 0x00015328 File Offset: 0x00013528
	public override BuildingDef CreateBuildingDef()
	{
		string id = "EspressoMachine";
		int width = 3;
		int height = 3;
		string anim = "espresso_machine_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.Floodable = true;
		buildingDef.AudioCategory = "Metal";
		buildingDef.Overheatable = true;
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.UtilityInputOffset = new CellOffset(1, 2);
		buildingDef.RequiresPowerInput = true;
		buildingDef.PowerInputOffset = new CellOffset(1, 2);
		buildingDef.EnergyConsumptionWhenActive = 480f;
		buildingDef.SelfHeatKilowattsWhenActive = 1f;
		buildingDef.AddSearchTerms(SEARCH_TERMS.MORALE);
		return buildingDef;
	}

	// Token: 0x060002EE RID: 750 RVA: 0x000153E0 File Offset: 0x000135E0
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		KPrefabID component = go.GetComponent<KPrefabID>();
		component.AddTag(RoomConstraints.ConstraintTags.RecBuilding, false);
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 20f;
		storage.SetDefaultStoredItemModifiers(Storage.StandardFabricatorStorage);
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Water).tag;
		conduitConsumer.capacityKG = 2f;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = new Tag("SpiceNut");
		manualDeliveryKG.capacity = 10f;
		manualDeliveryKG.refillMass = 5f;
		manualDeliveryKG.MinimumMass = 1f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
		go.AddOrGet<EspressoMachineWorkable>();
		go.AddOrGet<EspressoMachine>();
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.RecRoom.Id;
		roomTracker.requirement = RoomTracker.Requirement.Recommended;
		component.prefabInitFn += this.OnInit;
	}

	// Token: 0x060002EF RID: 751 RVA: 0x000154EC File Offset: 0x000136EC
	private void OnInit(GameObject go)
	{
		EspressoMachineWorkable component = go.GetComponent<EspressoMachineWorkable>();
		KAnimFile[] value = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_espresso_machine_kanim")
		};
		component.workerTypeOverrideAnims.Add(MinionConfig.ID, value);
		component.workerTypeOverrideAnims.Add(BionicMinionConfig.ID, new KAnimFile[]
		{
			Assets.GetAnim("anim_bionic_interacts_espresso_machine_kanim")
		});
	}

	// Token: 0x060002F0 RID: 752 RVA: 0x0001555C File Offset: 0x0001375C
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x040001B1 RID: 433
	public const string ID = "EspressoMachine";
}
