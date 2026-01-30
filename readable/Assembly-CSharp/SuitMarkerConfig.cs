using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000445 RID: 1093
public class SuitMarkerConfig : IBuildingConfig
{
	// Token: 0x060016AE RID: 5806 RVA: 0x0008153C File Offset: 0x0007F73C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SuitMarker";
		int width = 1;
		int height = 3;
		string anim = "changingarea_arrow_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] construction_materials = refined_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, construction_materials, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.PreventIdleTraversalPastBuilding = true;
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SuitIDs, "SuitMarker");
		buildingDef.AddSearchTerms(SEARCH_TERMS.ATMOSUIT);
		return buildingDef;
	}

	// Token: 0x060016AF RID: 5807 RVA: 0x000815C4 File Offset: 0x0007F7C4
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		SuitMarker suitMarker = go.AddOrGet<SuitMarker>();
		suitMarker.LockerTags = new Tag[]
		{
			new Tag("SuitLocker")
		};
		suitMarker.PathFlag = PathFinder.PotentialPath.Flags.HasAtmoSuit;
		go.AddOrGet<AnimTileable>().tags = new Tag[]
		{
			new Tag("SuitMarker"),
			new Tag("SuitLocker")
		};
		go.AddTag(GameTags.JetSuitBlocker);
	}

	// Token: 0x060016B0 RID: 5808 RVA: 0x0008163A File Offset: 0x0007F83A
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
	}

	// Token: 0x04000D73 RID: 3443
	public const string ID = "SuitMarker";
}
