using System;
using TUNING;
using UnityEngine;

// Token: 0x0200002F RID: 47
public class BottleEmptierConduitGasConfig : IBuildingConfig
{
	// Token: 0x060000D8 RID: 216 RVA: 0x000072F8 File Offset: 0x000054F8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "BottleEmptierConduitGas";
		int width = 1;
		int height = 2;
		string anim = "bottle_emptier_gas_conduit_kanim";
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
		buildingDef.OutputConduitType = ConduitType.Gas;
		buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
		buildingDef.ViewMode = OverlayModes.GasConduits.ID;
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.GasVentIDs, "BottleEmptierConduitGas");
		return buildingDef;
	}

	// Token: 0x060000D9 RID: 217 RVA: 0x0000738C File Offset: 0x0000558C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Prioritizable.AddRef(go);
		KPrefabID component = go.GetComponent<KPrefabID>();
		component.AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		component.AddTag(GameTags.OverlayBehindConduits, false);
		Storage storage = go.AddOrGet<Storage>();
		storage.storageFilters = STORAGEFILTERS.GASES;
		storage.showInUI = true;
		storage.showDescriptor = true;
		storage.capacityKg = 200f;
		storage.gunTargetOffset = new Vector2(0f, 1f);
		go.AddOrGet<TreeFilterable>();
		BottleEmptier bottleEmptier = go.AddOrGet<BottleEmptier>();
		bottleEmptier.isGasEmptier = true;
		bottleEmptier.emptyRate = 0.25f;
		bottleEmptier.emit = false;
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.conduitType = ConduitType.Gas;
		conduitDispenser.elementFilter = null;
		conduitDispenser.storage = storage;
		go.AddOrGet<RequireOutputs>().ignoreFullPipe = true;
	}

	// Token: 0x060000DA RID: 218 RVA: 0x00007447 File Offset: 0x00005647
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000091 RID: 145
	public const string ID = "BottleEmptierConduitGas";

	// Token: 0x04000092 RID: 146
	private const int WIDTH = 1;

	// Token: 0x04000093 RID: 147
	private const int HEIGHT = 2;
}
