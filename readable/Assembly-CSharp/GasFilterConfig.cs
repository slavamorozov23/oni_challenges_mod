using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200022F RID: 559
public class GasFilterConfig : IBuildingConfig
{
	// Token: 0x06000B53 RID: 2899 RVA: 0x00043DF4 File Offset: 0x00041FF4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "GasFilter";
		int width = 3;
		int height = 1;
		string anim = "filter_gas_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER0, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.SelfHeatKilowattsWhenActive = 0f;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.InputConduitType = ConduitType.Gas;
		buildingDef.OutputConduitType = ConduitType.Gas;
		buildingDef.Floodable = false;
		buildingDef.ViewMode = OverlayModes.GasConduits.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.UtilityInputOffset = new CellOffset(-1, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(1, 0);
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.AddSearchTerms(SEARCH_TERMS.FILTER);
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.GasVentIDs, "GasFilter");
		return buildingDef;
	}

	// Token: 0x06000B54 RID: 2900 RVA: 0x00043ECD File Offset: 0x000420CD
	private void AttachPort(GameObject go)
	{
		go.AddComponent<ConduitSecondaryOutput>().portInfo = this.secondaryPort;
	}

	// Token: 0x06000B55 RID: 2901 RVA: 0x00043EE0 File Offset: 0x000420E0
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		base.DoPostConfigurePreview(def, go);
		this.AttachPort(go);
	}

	// Token: 0x06000B56 RID: 2902 RVA: 0x00043EF1 File Offset: 0x000420F1
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		this.AttachPort(go);
	}

	// Token: 0x06000B57 RID: 2903 RVA: 0x00043F01 File Offset: 0x00042101
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<ElementFilter>().portInfo = this.secondaryPort;
		go.AddOrGet<Filterable>().filterElementState = Filterable.ElementState.Gas;
	}

	// Token: 0x06000B58 RID: 2904 RVA: 0x00043F31 File Offset: 0x00042131
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<PoweredActiveController.Def>().showWorkingStatus = true;
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits, false);
	}

	// Token: 0x040007DD RID: 2013
	public const string ID = "GasFilter";

	// Token: 0x040007DE RID: 2014
	private const ConduitType CONDUIT_TYPE = ConduitType.Gas;

	// Token: 0x040007DF RID: 2015
	private ConduitPortInfo secondaryPort = new ConduitPortInfo(ConduitType.Gas, new CellOffset(0, 0));
}
