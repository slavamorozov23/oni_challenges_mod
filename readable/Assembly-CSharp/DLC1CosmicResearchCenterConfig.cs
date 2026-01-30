using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200006D RID: 109
public class DLC1CosmicResearchCenterConfig : IBuildingConfig
{
	// Token: 0x0600020A RID: 522 RVA: 0x0000E9A4 File Offset: 0x0000CBA4
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x0600020B RID: 523 RVA: 0x0000E9AC File Offset: 0x0000CBAC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "DLC1CosmicResearchCenter";
		int width = 4;
		int height = 4;
		string anim = "research_space_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.ExhaustKilowattsWhenActive = 0.5f;
		buildingDef.SelfHeatKilowattsWhenActive = 4f;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		buildingDef.RequiredSkillPerkID = Db.Get().SkillPerks.AllowOrbitalResearch.Id;
		buildingDef.AddSearchTerms(SEARCH_TERMS.RESEARCH);
		return buildingDef;
	}

	// Token: 0x0600020C RID: 524 RVA: 0x0000EA68 File Offset: 0x0000CC68
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.ScienceBuilding, false);
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		Prioritizable.AddRef(go);
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 1000f;
		storage.showInUI = true;
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = DLC1CosmicResearchCenterConfig.INPUT_MATERIAL;
		manualDeliveryKG.refillMass = 3f;
		manualDeliveryKG.capacity = 300f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.ResearchFetch.IdHash;
		ResearchCenter researchCenter = go.AddOrGet<ResearchCenter>();
		researchCenter.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_research_space_kanim")
		};
		researchCenter.research_point_type_id = "orbital";
		researchCenter.inputMaterial = DLC1CosmicResearchCenterConfig.INPUT_MATERIAL;
		researchCenter.mass_per_point = 1f;
		researchCenter.requiredSkillPerk = Db.Get().SkillPerks.AllowOrbitalResearch.Id;
		researchCenter.workLayer = Grid.SceneLayer.BuildingFront;
		ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
		{
			new ElementConverter.ConsumedElement(DLC1CosmicResearchCenterConfig.INPUT_MATERIAL, 0.02f, true)
		};
		elementConverter.showDescriptors = false;
		go.AddOrGetDef<PoweredController.Def>();
	}

	// Token: 0x0600020D RID: 525 RVA: 0x0000EB98 File Offset: 0x0000CD98
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000140 RID: 320
	public const string ID = "DLC1CosmicResearchCenter";

	// Token: 0x04000141 RID: 321
	public const float BASE_SECONDS_PER_POINT = 50f;

	// Token: 0x04000142 RID: 322
	public const float MASS_PER_POINT = 1f;

	// Token: 0x04000143 RID: 323
	public const float BASE_MASS_PER_SECOND = 0.02f;

	// Token: 0x04000144 RID: 324
	public const float CAPACITY = 300f;

	// Token: 0x04000145 RID: 325
	public static readonly Tag INPUT_MATERIAL = OrbitalResearchDatabankConfig.TAG;
}
