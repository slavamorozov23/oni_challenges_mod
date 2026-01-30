using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000011 RID: 17
public class AdvancedApothecaryConfig : IBuildingConfig
{
	// Token: 0x0600004B RID: 75 RVA: 0x00003EA0 File Offset: 0x000020A0
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x0600004C RID: 76 RVA: 0x00003EA8 File Offset: 0x000020A8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "AdvancedApothecary";
		int width = 3;
		int height = 3;
		string anim = "medicine_nuclear_kanim";
		int hitpoints = 250;
		float construction_time = 240f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, none, 0.2f);
		buildingDef.ExhaustKilowattsWhenActive = 0.5f;
		buildingDef.SelfHeatKilowattsWhenActive = 2f;
		buildingDef.UseHighEnergyParticleInputPort = true;
		buildingDef.HighEnergyParticleInputOffset = new CellOffset(0, 2);
		buildingDef.ViewMode = OverlayModes.Radiation.ID;
		buildingDef.AudioCategory = "Glass";
		buildingDef.AudioSize = "large";
		buildingDef.AddSearchTerms(SEARCH_TERMS.MEDICINE);
		buildingDef.Deprecated = true;
		return buildingDef;
	}

	// Token: 0x0600004D RID: 77 RVA: 0x00003F54 File Offset: 0x00002154
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Prioritizable.AddRef(go);
		HighEnergyParticleStorage highEnergyParticleStorage = go.AddOrGet<HighEnergyParticleStorage>();
		highEnergyParticleStorage.autoStore = true;
		highEnergyParticleStorage.capacity = 400f;
		highEnergyParticleStorage.showCapacityStatusItem = true;
		go.AddOrGet<HighEnergyParticlePort>().requireOperational = false;
		go.AddOrGet<DropAllWorkable>();
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		AdvancedApothecary fabricator = go.AddOrGet<AdvancedApothecary>();
		BuildingTemplates.CreateComplexFabricatorStorage(go, fabricator);
		go.AddOrGet<ComplexFabricatorWorkable>();
		go.AddOrGet<FabricatorIngredientStatusManager>();
		go.AddOrGet<CopyBuildingSettings>();
		ActiveParticleConsumer.Def def = go.AddOrGetDef<ActiveParticleConsumer.Def>();
		def.activeConsumptionRate = 1f;
		def.minParticlesForOperational = 1f;
		def.meterSymbolName = null;
	}

	// Token: 0x0600004E RID: 78 RVA: 0x00003FE9 File Offset: 0x000021E9
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<PoweredController.Def>();
	}

	// Token: 0x04000049 RID: 73
	public const string ID = "AdvancedApothecary";

	// Token: 0x0400004A RID: 74
	public const float PARTICLE_CAPACITY = 400f;
}
