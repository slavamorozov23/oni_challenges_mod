using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000082 RID: 130
public class ElectrobankChargerConfig : IBuildingConfig
{
	// Token: 0x06000275 RID: 629 RVA: 0x0001114B File Offset: 0x0000F34B
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC3;
	}

	// Token: 0x06000276 RID: 630 RVA: 0x00011154 File Offset: 0x0000F354
	public override BuildingDef CreateBuildingDef()
	{
		string id = "ElectrobankCharger";
		int width = 2;
		int height = 2;
		string anim = "electrobank_charger_small_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 2400f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER1, tier2, 0.2f);
		buildingDef.EnergyConsumptionWhenActive = 480f;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.SelfHeatKilowattsWhenActive = 1f;
		buildingDef.RequiresPowerInput = true;
		buildingDef.PowerInputOffset = new CellOffset(0, 0);
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.AudioSize = "small";
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		buildingDef.AddSearchTerms(SEARCH_TERMS.BATTERY);
		return buildingDef;
	}

	// Token: 0x06000277 RID: 631 RVA: 0x00011214 File Offset: 0x0000F414
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 1f;
		go.AddOrGet<LoopingSounds>();
		Prioritizable.AddRef(go);
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = GameTags.EmptyPortableBattery;
		manualDeliveryKG.capacity = storage.capacityKg;
		manualDeliveryKG.refillMass = 20f;
		manualDeliveryKG.MinimumMass = 20f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.PowerFetch.IdHash;
	}

	// Token: 0x06000278 RID: 632 RVA: 0x000112A4 File Offset: 0x0000F4A4
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		go.AddOrGetDef<ElectrobankCharger.Def>();
	}

	// Token: 0x04000188 RID: 392
	public const string ID = "ElectrobankCharger";

	// Token: 0x04000189 RID: 393
	public const float BUILDING_WATTAGE_COST = 480f;

	// Token: 0x0400018A RID: 394
	public const float CHARGE_RATE = 400f;
}
