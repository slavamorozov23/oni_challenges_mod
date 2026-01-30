using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003E1 RID: 993
public class ResearchCenterConfig : IBuildingConfig
{
	// Token: 0x06001465 RID: 5221 RVA: 0x00073D4C File Offset: 0x00071F4C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "ResearchCenter";
		int width = 2;
		int height = 2;
		string anim = "research_center_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 60f;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.ExhaustKilowattsWhenActive = 0.125f;
		buildingDef.SelfHeatKilowattsWhenActive = 1f;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		buildingDef.AddSearchTerms(SEARCH_TERMS.RESEARCH);
		return buildingDef;
	}

	// Token: 0x06001466 RID: 5222 RVA: 0x00073DEC File Offset: 0x00071FEC
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
		manualDeliveryKG.RequestedItemTag = ResearchCenterConfig.INPUT_MATERIAL;
		manualDeliveryKG.refillMass = 150f;
		manualDeliveryKG.capacity = 750f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.ResearchFetch.IdHash;
		ResearchCenter researchCenter = go.AddOrGet<ResearchCenter>();
		researchCenter.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_research_center_kanim")
		};
		researchCenter.research_point_type_id = "basic";
		researchCenter.inputMaterial = ResearchCenterConfig.INPUT_MATERIAL;
		researchCenter.mass_per_point = 50f;
		ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
		{
			new ElementConverter.ConsumedElement(ResearchCenterConfig.INPUT_MATERIAL, 1.1111112f, true)
		};
		elementConverter.showDescriptors = false;
		go.AddOrGetDef<PoweredController.Def>();
	}

	// Token: 0x06001467 RID: 5223 RVA: 0x00073EFA File Offset: 0x000720FA
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000C53 RID: 3155
	public const float BASE_SECONDS_PER_POINT = 45f;

	// Token: 0x04000C54 RID: 3156
	public const float MASS_PER_POINT = 50f;

	// Token: 0x04000C55 RID: 3157
	public const float BASE_MASS_PER_SECOND = 1.1111112f;

	// Token: 0x04000C56 RID: 3158
	public static readonly Tag INPUT_MATERIAL = GameTags.Dirt;

	// Token: 0x04000C57 RID: 3159
	public const float CAPACITY = 750f;

	// Token: 0x04000C58 RID: 3160
	public const string ID = "ResearchCenter";
}
