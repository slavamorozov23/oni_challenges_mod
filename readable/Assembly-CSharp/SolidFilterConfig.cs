using System;
using TUNING;
using UnityEngine;

// Token: 0x02000429 RID: 1065
public class SolidFilterConfig : IBuildingConfig
{
	// Token: 0x060015F9 RID: 5625 RVA: 0x0007D5D0 File Offset: 0x0007B7D0
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SolidFilter";
		int width = 3;
		int height = 1;
		string anim = "filter_material_conveyor_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER0, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.SelfHeatKilowattsWhenActive = 0f;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.InputConduitType = ConduitType.Solid;
		buildingDef.OutputConduitType = ConduitType.Solid;
		buildingDef.Floodable = false;
		buildingDef.ViewMode = OverlayModes.SolidConveyor.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.UtilityInputOffset = new CellOffset(-1, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(1, 0);
		buildingDef.PermittedRotations = PermittedRotations.R360;
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SolidConveyorIDs, "SolidFilter");
		return buildingDef;
	}

	// Token: 0x060015FA RID: 5626 RVA: 0x0007D699 File Offset: 0x0007B899
	private void AttachPort(GameObject go)
	{
		go.AddComponent<ConduitSecondaryOutput>().portInfo = this.secondaryPort;
	}

	// Token: 0x060015FB RID: 5627 RVA: 0x0007D6AC File Offset: 0x0007B8AC
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		base.DoPostConfigurePreview(def, go);
		this.AttachPort(go);
	}

	// Token: 0x060015FC RID: 5628 RVA: 0x0007D6BD File Offset: 0x0007B8BD
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		this.AttachPort(go);
	}

	// Token: 0x060015FD RID: 5629 RVA: 0x0007D6CD File Offset: 0x0007B8CD
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<ElementFilter>().portInfo = this.secondaryPort;
		go.AddOrGet<Filterable>().filterElementState = Filterable.ElementState.Solid;
	}

	// Token: 0x060015FE RID: 5630 RVA: 0x0007D6FD File Offset: 0x0007B8FD
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<PoweredActiveController.Def>().showWorkingStatus = true;
	}

	// Token: 0x04000D0B RID: 3339
	public const string ID = "SolidFilter";

	// Token: 0x04000D0C RID: 3340
	private const ConduitType CONDUIT_TYPE = ConduitType.Solid;

	// Token: 0x04000D0D RID: 3341
	private ConduitPortInfo secondaryPort = new ConduitPortInfo(ConduitType.Solid, new CellOffset(0, 0));
}
