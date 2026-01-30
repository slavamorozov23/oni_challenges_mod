using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000416 RID: 1046
public class SmallElectrobankDischargerConfig : IBuildingConfig
{
	// Token: 0x06001594 RID: 5524 RVA: 0x0007B447 File Offset: 0x00079647
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC3;
	}

	// Token: 0x06001595 RID: 5525 RVA: 0x0007B450 File Offset: 0x00079650
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SmallElectrobankDischarger";
		int width = 1;
		int height = 1;
		string anim = "electrobank_discharger_small_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 2400f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFoundationRotatable;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER1, tier2, 0.2f);
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.GeneratorWattageRating = 60f;
		buildingDef.GeneratorBaseCapacity = 60f;
		buildingDef.ExhaustKilowattsWhenActive = 0.125f;
		buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
		buildingDef.RequiresPowerOutput = true;
		buildingDef.PowerOutputOffset = new CellOffset(0, 0);
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.AudioSize = "small";
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		buildingDef.AddSearchTerms(SEARCH_TERMS.BATTERY);
		return buildingDef;
	}

	// Token: 0x06001596 RID: 5526 RVA: 0x0007B524 File Offset: 0x00079724
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Prioritizable.AddRef(go);
		go.AddOrGet<LoopingSounds>();
		Storage storage = go.AddOrGet<Storage>();
		storage.showInUI = true;
		storage.capacityKg = 20f;
		storage.storageFilters = STORAGEFILTERS.POWER_BANKS;
		go.AddOrGet<TreeFilterable>().allResourceFilterLabelString = UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.ALLBUTTON;
		go.AddOrGet<ElectrobankDischarger>().wattageRating = 60f;
	}

	// Token: 0x06001597 RID: 5527 RVA: 0x0007B585 File Offset: 0x00079785
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		SymbolOverrideControllerUtil.AddToPrefab(go);
	}

	// Token: 0x04000CE5 RID: 3301
	public const string ID = "SmallElectrobankDischarger";

	// Token: 0x04000CE6 RID: 3302
	public const float DISCHARGE_RATE = 60f;
}
