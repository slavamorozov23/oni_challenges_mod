using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200041C RID: 1052
public class SolarPanelConfig : IBuildingConfig
{
	// Token: 0x060015B7 RID: 5559 RVA: 0x0007C350 File Offset: 0x0007A550
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SolarPanel";
		int width = 7;
		int height = 3;
		string anim = "solar_panel_kanim";
		int hitpoints = 100;
		float construction_time = 120f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] glasses = MATERIALS.GLASSES;
		float melting_point = 2400f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, glasses, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, tier2, 0.2f);
		buildingDef.GeneratorWattageRating = 380f;
		buildingDef.GeneratorBaseCapacity = buildingDef.GeneratorWattageRating;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.SelfHeatKilowattsWhenActive = 0f;
		buildingDef.BuildLocationRule = BuildLocationRule.Anywhere;
		buildingDef.HitPoints = 10;
		buildingDef.RequiresPowerOutput = true;
		buildingDef.PowerOutputOffset = new CellOffset(0, 0);
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.AudioSize = "large";
		buildingDef.AddSearchTerms(SEARCH_TERMS.POWER);
		return buildingDef;
	}

	// Token: 0x060015B8 RID: 5560 RVA: 0x0007C418 File Offset: 0x0007A618
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.PowerBuilding, false);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.GeneratorType, false);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.HeavyDutyGeneratorType, false);
		go.AddOrGet<LoopingSounds>();
		Prioritizable.AddRef(go);
	}

	// Token: 0x060015B9 RID: 5561 RVA: 0x0007C478 File Offset: 0x0007A678
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<Repairable>().expectedRepairTime = 52.5f;
		go.AddOrGet<SolarPanel>().powerDistributionOrder = 9;
		go.AddOrGetDef<PoweredActiveController.Def>();
		MakeBaseSolid.Def def = go.AddOrGetDef<MakeBaseSolid.Def>();
		def.occupyFoundationLayer = false;
		def.solidOffsets = new CellOffset[7];
		for (int i = 0; i < 7; i++)
		{
			def.solidOffsets[i] = new CellOffset(i - 3, 0);
		}
	}

	// Token: 0x04000CF4 RID: 3316
	public const string ID = "SolarPanel";

	// Token: 0x04000CF5 RID: 3317
	public const float WATTS_PER_LUX = 0.00053f;

	// Token: 0x04000CF6 RID: 3318
	public const float MAX_WATTS = 380f;

	// Token: 0x04000CF7 RID: 3319
	private const int WIDTH = 7;
}
