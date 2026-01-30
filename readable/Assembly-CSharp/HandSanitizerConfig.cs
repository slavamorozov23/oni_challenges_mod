using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000260 RID: 608
public class HandSanitizerConfig : IBuildingConfig
{
	// Token: 0x06000C5F RID: 3167 RVA: 0x0004A5A8 File Offset: 0x000487A8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "HandSanitizer";
		int width = 1;
		int height = 3;
		string anim = "handsanitizer_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		string[] array = new string[]
		{
			"Metal"
		};
		float[] construction_mass = new float[]
		{
			TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2[0]
		};
		string[] construction_materials = array;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
		SoundEventVolumeCache.instance.AddVolume("handsanitizer_kanim", "HandSanitizer_tongue_out", NOISE_POLLUTION.NOISY.TIER0);
		SoundEventVolumeCache.instance.AddVolume("handsanitizer_kanim", "HandSanitizer_tongue_in", NOISE_POLLUTION.NOISY.TIER0);
		SoundEventVolumeCache.instance.AddVolume("handsanitizer_kanim", "HandSanitizer_tongue_slurp", NOISE_POLLUTION.NOISY.TIER0);
		buildingDef.AddSearchTerms(SEARCH_TERMS.MEDICINE);
		return buildingDef;
	}

	// Token: 0x06000C60 RID: 3168 RVA: 0x0004A660 File Offset: 0x00048860
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.WashStation, false);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.AdvancedWashStation, false);
		HandSanitizer handSanitizer = go.AddOrGet<HandSanitizer>();
		handSanitizer.massConsumedPerUse = 0.07f;
		handSanitizer.consumedElement = SimHashes.BleachStone;
		handSanitizer.diseaseRemovalCount = HandSanitizerConfig.DISEASE_REMOVAL_COUNT;
		handSanitizer.hasMeters = false;
		HandSanitizer.Work work = go.AddOrGet<HandSanitizer.Work>();
		work.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_handsanitizer_kanim")
		};
		work.workTime = 1.8f;
		work.trackUses = true;
		Storage storage = go.AddOrGet<Storage>();
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		go.AddOrGet<DirectionControl>();
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = GameTagExtensions.Create(SimHashes.BleachStone);
		manualDeliveryKG.capacity = 15f;
		manualDeliveryKG.refillMass = 3f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
		manualDeliveryKG.operationalRequirement = Operational.State.Functional;
		go.AddOrGetDef<RocketUsageRestriction.Def>();
	}

	// Token: 0x06000C61 RID: 3169 RVA: 0x0004A766 File Offset: 0x00048966
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x0400087E RID: 2174
	public const string ID = "HandSanitizer";

	// Token: 0x0400087F RID: 2175
	private const float STORAGE_SIZE = 15f;

	// Token: 0x04000880 RID: 2176
	private const float MASS_PER_USE = 0.07f;

	// Token: 0x04000881 RID: 2177
	private static readonly int DISEASE_REMOVAL_COUNT = WashBasinConfig.DISEASE_REMOVAL_COUNT * 4;

	// Token: 0x04000882 RID: 2178
	private const float WORK_TIME = 1.8f;

	// Token: 0x04000883 RID: 2179
	private const SimHashes CONSUMED_ELEMENT = SimHashes.BleachStone;
}
