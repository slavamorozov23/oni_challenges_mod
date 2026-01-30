using System;
using TUNING;
using UnityEngine;

// Token: 0x02000290 RID: 656
public class LeadSuitLockerConfig : IBuildingConfig
{
	// Token: 0x06000D52 RID: 3410 RVA: 0x0004EE7D File Offset: 0x0004D07D
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000D53 RID: 3411 RVA: 0x0004EE84 File Offset: 0x0004D084
	public override BuildingDef CreateBuildingDef()
	{
		string id = "LeadSuitLocker";
		int width = 2;
		int height = 4;
		string anim = "changingarea_radiation_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] construction_materials = refined_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.PreventIdleTraversalPastBuilding = true;
		buildingDef.InputConduitType = ConduitType.Gas;
		buildingDef.UtilityInputOffset = new CellOffset(0, 2);
		buildingDef.Deprecated = !Sim.IsRadiationEnabled();
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SuitIDs, "LeadSuitLocker");
		return buildingDef;
	}

	// Token: 0x06000D54 RID: 3412 RVA: 0x0004EF18 File Offset: 0x0004D118
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<SuitLocker>().OutfitTags = new Tag[]
		{
			GameTags.LeadSuit
		};
		go.AddOrGet<LeadSuitLocker>();
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Gas;
		conduitConsumer.consumptionRate = 1f;
		conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Oxygen).tag;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.capacityKG = 80f;
		go.AddOrGet<AnimTileable>().tags = new Tag[]
		{
			new Tag("LeadSuitLocker"),
			new Tag("LeadSuitMarker")
		};
		go.AddOrGet<Storage>();
		Prioritizable.AddRef(go);
	}

	// Token: 0x06000D55 RID: 3413 RVA: 0x0004EFCE File Offset: 0x0004D1CE
	public override void DoPostConfigureComplete(GameObject go)
	{
		SymbolOverrideControllerUtil.AddToPrefab(go);
	}

	// Token: 0x04000916 RID: 2326
	public const string ID = "LeadSuitLocker";
}
