using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000280 RID: 640
public class JetSuitMarkerConfig : IBuildingConfig
{
	// Token: 0x06000D04 RID: 3332 RVA: 0x0004D478 File Offset: 0x0004B678
	public override BuildingDef CreateBuildingDef()
	{
		string id = "JetSuitMarker";
		int width = 2;
		int height = 4;
		string anim = "changingarea_jetsuit_arrow_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float[] construction_mass = new float[]
		{
			TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3[0]
		};
		string[] construction_materials = refined_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.PreventIdleTraversalPastBuilding = true;
		buildingDef.SceneLayer = Grid.SceneLayer.BuildingUse;
		buildingDef.ForegroundLayer = Grid.SceneLayer.TileMain;
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SuitIDs, "JetSuitMarker");
		buildingDef.AddSearchTerms(SEARCH_TERMS.ATMOSUIT);
		return buildingDef;
	}

	// Token: 0x06000D05 RID: 3333 RVA: 0x0004D51C File Offset: 0x0004B71C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		SuitMarker suitMarker = go.AddOrGet<SuitMarker>();
		suitMarker.LockerTags = new Tag[]
		{
			new Tag("JetSuitLocker")
		};
		suitMarker.PathFlag = PathFinder.PotentialPath.Flags.HasJetPack;
		suitMarker.interactAnim = Assets.GetAnim("anim_interacts_changingarea_jetsuit_arrow_kanim");
		go.AddOrGet<AnimTileable>().tags = new Tag[]
		{
			new Tag("JetSuitMarker"),
			new Tag("JetSuitLocker")
		};
		go.AddTag(GameTags.JetSuitBlocker);
	}

	// Token: 0x06000D06 RID: 3334 RVA: 0x0004D5A7 File Offset: 0x0004B7A7
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
	}

	// Token: 0x040008F2 RID: 2290
	public const string ID = "JetSuitMarker";
}
