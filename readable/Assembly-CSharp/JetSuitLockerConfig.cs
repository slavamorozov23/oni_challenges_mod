using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200027F RID: 639
public class JetSuitLockerConfig : IBuildingConfig
{
	// Token: 0x06000CFD RID: 3325 RVA: 0x0004D2B4 File Offset: 0x0004B4B4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "JetSuitLocker";
		int width = 2;
		int height = 4;
		string anim = "changingarea_jetsuit_kanim";
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
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.PreventIdleTraversalPastBuilding = true;
		buildingDef.InputConduitType = ConduitType.Gas;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SuitIDs, "JetSuitLocker");
		buildingDef.AddSearchTerms(SEARCH_TERMS.ATMOSUIT);
		return buildingDef;
	}

	// Token: 0x06000CFE RID: 3326 RVA: 0x0004D353 File Offset: 0x0004B553
	private void AttachPort(GameObject go)
	{
		go.AddComponent<ConduitSecondaryInput>().portInfo = this.secondaryInputPort;
	}

	// Token: 0x06000CFF RID: 3327 RVA: 0x0004D368 File Offset: 0x0004B568
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<SuitLocker>().OutfitTags = new Tag[]
		{
			GameTags.JetSuit
		};
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Gas;
		conduitConsumer.consumptionRate = 1f;
		conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Oxygen).tag;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.capacityKG = 200f;
		go.AddComponent<JetSuitLocker>().portInfo = this.secondaryInputPort;
		go.AddOrGet<AnimTileable>().tags = new Tag[]
		{
			new Tag("JetSuitLocker"),
			new Tag("JetSuitMarker")
		};
		go.AddOrGet<Storage>().capacityKg = 500f;
		Prioritizable.AddRef(go);
	}

	// Token: 0x06000D00 RID: 3328 RVA: 0x0004D431 File Offset: 0x0004B631
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		base.DoPostConfigurePreview(def, go);
		this.AttachPort(go);
	}

	// Token: 0x06000D01 RID: 3329 RVA: 0x0004D442 File Offset: 0x0004B642
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		this.AttachPort(go);
	}

	// Token: 0x06000D02 RID: 3330 RVA: 0x0004D452 File Offset: 0x0004B652
	public override void DoPostConfigureComplete(GameObject go)
	{
		SymbolOverrideControllerUtil.AddToPrefab(go);
	}

	// Token: 0x040008EE RID: 2286
	public const string ID = "JetSuitLocker";

	// Token: 0x040008EF RID: 2287
	public const float O2_CAPACITY = 200f;

	// Token: 0x040008F0 RID: 2288
	public const float SUIT_CAPACITY = 200f;

	// Token: 0x040008F1 RID: 2289
	private ConduitPortInfo secondaryInputPort = new ConduitPortInfo(ConduitType.Liquid, new CellOffset(0, 1));
}
