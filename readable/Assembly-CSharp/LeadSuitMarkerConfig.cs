using System;
using TUNING;
using UnityEngine;

// Token: 0x02000291 RID: 657
public class LeadSuitMarkerConfig : IBuildingConfig
{
	// Token: 0x06000D57 RID: 3415 RVA: 0x0004EFDF File Offset: 0x0004D1DF
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000D58 RID: 3416 RVA: 0x0004EFE8 File Offset: 0x0004D1E8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "LeadSuitMarker";
		int width = 2;
		int height = 4;
		string anim = "changingarea_radiation_arrow_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] construction_materials = refined_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.PreventIdleTraversalPastBuilding = true;
		buildingDef.Deprecated = !Sim.IsRadiationEnabled();
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SuitIDs, "LeadSuitMarker");
		return buildingDef;
	}

	// Token: 0x06000D59 RID: 3417 RVA: 0x0004F070 File Offset: 0x0004D270
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		SuitMarker suitMarker = go.AddOrGet<SuitMarker>();
		suitMarker.LockerTags = new Tag[]
		{
			new Tag("LeadSuitLocker")
		};
		suitMarker.PathFlag = PathFinder.PotentialPath.Flags.HasLeadSuit;
		go.AddOrGet<AnimTileable>().tags = new Tag[]
		{
			new Tag("LeadSuitMarker"),
			new Tag("LeadSuitLocker")
		};
		go.AddTag(GameTags.JetSuitBlocker);
	}

	// Token: 0x06000D5A RID: 3418 RVA: 0x0004F0E7 File Offset: 0x0004D2E7
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
	}

	// Token: 0x04000917 RID: 2327
	public const string ID = "LeadSuitMarker";
}
