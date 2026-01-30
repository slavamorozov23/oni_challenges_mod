using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000014 RID: 20
public class AdvancedResearchCenterConfig : IBuildingConfig
{
	// Token: 0x0600005A RID: 90 RVA: 0x00004600 File Offset: 0x00002800
	public override BuildingDef CreateBuildingDef()
	{
		string id = "AdvancedResearchCenter";
		int width = 3;
		int height = 3;
		string anim = "research_center2_kanim";
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
		buildingDef.RequiredSkillPerkID = Db.Get().SkillPerks.AllowAdvancedResearch.Id;
		buildingDef.AddSearchTerms(SEARCH_TERMS.RESEARCH);
		return buildingDef;
	}

	// Token: 0x0600005B RID: 91 RVA: 0x000046BC File Offset: 0x000028BC
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.ScienceBuilding, false);
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		Prioritizable.AddRef(go);
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 1000f;
		storage.showInUI = true;
		storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Hide,
			Storage.StoredItemModifier.Insulate
		});
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = AdvancedResearchCenterConfig.INPUT_MATERIAL;
		manualDeliveryKG.refillMass = 150f;
		manualDeliveryKG.capacity = 750f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.ResearchFetch.IdHash;
		ResearchCenter researchCenter = go.AddOrGet<ResearchCenter>();
		researchCenter.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_research2_kanim")
		};
		researchCenter.research_point_type_id = "advanced";
		researchCenter.inputMaterial = AdvancedResearchCenterConfig.INPUT_MATERIAL;
		researchCenter.mass_per_point = 50f;
		researchCenter.requiredSkillPerk = Db.Get().SkillPerks.AllowAdvancedResearch.Id;
		ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
		{
			new ElementConverter.ConsumedElement(AdvancedResearchCenterConfig.INPUT_MATERIAL, 0.8333333f, true)
		};
		elementConverter.showDescriptors = false;
		go.AddOrGetDef<PoweredController.Def>();
	}

	// Token: 0x0600005C RID: 92 RVA: 0x000047FD File Offset: 0x000029FD
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x0400004D RID: 77
	public const string ID = "AdvancedResearchCenter";

	// Token: 0x0400004E RID: 78
	public const float BASE_SECONDS_PER_POINT = 60f;

	// Token: 0x0400004F RID: 79
	public const float MASS_PER_POINT = 50f;

	// Token: 0x04000050 RID: 80
	public const float BASE_MASS_PER_SECOND = 0.8333333f;

	// Token: 0x04000051 RID: 81
	public const float CAPACITY = 750f;

	// Token: 0x04000052 RID: 82
	public static readonly Tag INPUT_MATERIAL = GameTags.Water;
}
