using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200028D RID: 653
public class LargeElectrobankDischargerConfig : IBuildingConfig
{
	// Token: 0x06000D45 RID: 3397 RVA: 0x0004E9EC File Offset: 0x0004CBEC
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC3;
	}

	// Token: 0x06000D46 RID: 3398 RVA: 0x0004E9F4 File Offset: 0x0004CBF4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "LargeElectrobankDischarger";
		int width = 2;
		int height = 2;
		string anim = "electrobank_discharger_large_kanim";
		int hitpoints = 30;
		float construction_time = 60f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 2400f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, tier2, 0.2f);
		buildingDef.GeneratorWattageRating = 480f;
		buildingDef.GeneratorBaseCapacity = 480f;
		buildingDef.ExhaustKilowattsWhenActive = 0.25f;
		buildingDef.SelfHeatKilowattsWhenActive = 1f;
		buildingDef.RequiresPowerOutput = true;
		buildingDef.PowerOutputOffset = new CellOffset(0, 0);
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.AudioSize = "large";
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		buildingDef.AddSearchTerms(SEARCH_TERMS.BATTERY);
		return buildingDef;
	}

	// Token: 0x06000D47 RID: 3399 RVA: 0x0004EAC0 File Offset: 0x0004CCC0
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		Prioritizable.AddRef(go);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		Storage storage = go.AddOrGet<Storage>();
		storage.showInUI = true;
		storage.capacityKg = 20f;
		storage.storageFilters = STORAGEFILTERS.POWER_BANKS;
		go.AddOrGet<TreeFilterable>().allResourceFilterLabelString = UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.ALLBUTTON;
		go.AddOrGet<ElectrobankDischarger>().wattageRating = 480f;
	}

	// Token: 0x06000D48 RID: 3400 RVA: 0x0004EB32 File Offset: 0x0004CD32
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		SymbolOverrideControllerUtil.AddToPrefab(go);
	}

	// Token: 0x0400090C RID: 2316
	public const string ID = "LargeElectrobankDischarger";

	// Token: 0x0400090D RID: 2317
	public const float DISCHARGE_RATE = 480f;
}
