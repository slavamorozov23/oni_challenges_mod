using System;
using TUNING;
using UnityEngine;

// Token: 0x02000030 RID: 48
public class BottleEmptierConduitLiquidConfig : IBuildingConfig
{
	// Token: 0x060000DC RID: 220 RVA: 0x00007454 File Offset: 0x00005654
	public override BuildingDef CreateBuildingDef()
	{
		string id = "BottleEmptierConduitLiquid";
		int width = 1;
		int height = 2;
		string anim = "bottle_emptier_liquid_conduit_kanim";
		int hitpoints = 30;
		float construction_time = 60f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER2, tier2, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.Overheatable = false;
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.OutputConduitType = ConduitType.Liquid;
		buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.LiquidVentIDs, "BottleEmptierConduitLiquid");
		return buildingDef;
	}

	// Token: 0x060000DD RID: 221 RVA: 0x000074E8 File Offset: 0x000056E8
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Prioritizable.AddRef(go);
		KPrefabID component = go.GetComponent<KPrefabID>();
		component.AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		component.AddTag(GameTags.OverlayBehindConduits, false);
		Storage storage = go.AddOrGet<Storage>();
		storage.storageFilters = STORAGEFILTERS.LIQUIDS;
		storage.showInUI = true;
		storage.showDescriptor = true;
		storage.capacityKg = 200f;
		storage.gunTargetOffset = new Vector2(0f, 1f);
		go.AddOrGet<TreeFilterable>();
		BottleEmptier bottleEmptier = go.AddOrGet<BottleEmptier>();
		bottleEmptier.emit = false;
		bottleEmptier.emptyRate = 5f;
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.conduitType = ConduitType.Liquid;
		conduitDispenser.elementFilter = null;
		conduitDispenser.storage = storage;
		go.AddOrGet<RequireOutputs>().ignoreFullPipe = true;
	}

	// Token: 0x060000DE RID: 222 RVA: 0x0000759C File Offset: 0x0000579C
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000094 RID: 148
	public const string ID = "BottleEmptierConduitLiquid";

	// Token: 0x04000095 RID: 149
	private const int WIDTH = 1;

	// Token: 0x04000096 RID: 150
	private const int HEIGHT = 2;
}
