using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200037D RID: 893
public class OuthouseConfig : IBuildingConfig
{
	// Token: 0x0600128D RID: 4749 RVA: 0x0006BB8C File Offset: 0x00069D8C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Outhouse";
		int width = 2;
		int height = 3;
		string anim = "outhouse_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] raw_MINERALS_OR_WOOD = MATERIALS.RAW_MINERALS_OR_WOOD;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS_OR_WOOD, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER4, none, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.ExhaustKilowattsWhenActive = 0.25f;
		buildingDef.DiseaseCellVisName = DUPLICANTSTATS.STANDARD.Secretions.PEE_DISEASE;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AddSearchTerms(SEARCH_TERMS.TOILET);
		SoundEventVolumeCache.instance.AddVolume("outhouse_kanim", "Latrine_door_open", NOISE_POLLUTION.NOISY.TIER1);
		SoundEventVolumeCache.instance.AddVolume("outhouse_kanim", "Latrine_door_close", NOISE_POLLUTION.NOISY.TIER1);
		return buildingDef;
	}

	// Token: 0x0600128E RID: 4750 RVA: 0x0006BC48 File Offset: 0x00069E48
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		KPrefabID component = go.GetComponent<KPrefabID>();
		go.AddOrGet<LoopingSounds>();
		component.AddTag(RoomConstraints.ConstraintTags.ToiletType, false);
		Toilet toilet = go.AddOrGet<Toilet>();
		toilet.maxFlushes = 15;
		toilet.dirtUsedPerFlush = 13f;
		toilet.solidWastePerUse = new Toilet.SpawnInfo(SimHashes.ToxicSand, DUPLICANTSTATS.STANDARD.Secretions.PEE_PER_TOILET_PEE, 0f);
		toilet.solidWasteTemperature = DUPLICANTSTATS.STANDARD.Temperature.Internal.IDEAL;
		toilet.diseaseId = DUPLICANTSTATS.STANDARD.Secretions.PEE_DISEASE;
		toilet.diseasePerFlush = DUPLICANTSTATS.STANDARD.Secretions.DISEASE_PER_PEE;
		toilet.diseaseOnDupePerFlush = DUPLICANTSTATS.STANDARD.Secretions.DISEASE_PER_PEE;
		go.AddOrGet<ToiletWorkableUse>().workLayer = Grid.SceneLayer.BuildingFront;
		ToiletWorkableClean toiletWorkableClean = go.AddOrGet<ToiletWorkableClean>();
		toiletWorkableClean.workTime = 90f;
		toiletWorkableClean.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_outhouse_kanim")
		};
		toiletWorkableClean.workLayer = Grid.SceneLayer.BuildingFront;
		Prioritizable.AddRef(go);
		toiletWorkableClean.SetIsCloggedByGunk(false);
		Storage storage = go.AddOrGet<Storage>();
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		storage.showInUI = true;
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = new Tag("Dirt");
		manualDeliveryKG.capacity = 200f;
		manualDeliveryKG.refillMass = 0.01f;
		manualDeliveryKG.MinimumMass = 200f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
		manualDeliveryKG.operationalRequirement = Operational.State.Functional;
		manualDeliveryKG.FillToCapacity = true;
		Ownable ownable = go.AddOrGet<Ownable>();
		ownable.slotID = Db.Get().AssignableSlots.Toilet.Id;
		ownable.canBePublic = true;
		go.AddOrGetDef<RocketUsageRestriction.Def>();
		component.prefabInitFn += this.OnInit;
	}

	// Token: 0x0600128F RID: 4751 RVA: 0x0006BE14 File Offset: 0x0006A014
	private void OnInit(GameObject go)
	{
		ToiletWorkableUse component = go.GetComponent<ToiletWorkableUse>();
		KAnimFile[] value = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_outhouse_kanim")
		};
		component.workerTypeOverrideAnims.Add(MinionConfig.ID, value);
		component.workerTypeOverrideAnims.Add(BionicMinionConfig.ID, new KAnimFile[]
		{
			Assets.GetAnim("anim_bionic_interacts_outhouse_kanim")
		});
	}

	// Token: 0x06001290 RID: 4752 RVA: 0x0006BE84 File Offset: 0x0006A084
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000BBF RID: 3007
	public const string ID = "Outhouse";

	// Token: 0x04000BC0 RID: 3008
	private const int USES_PER_REFILL = 15;

	// Token: 0x04000BC1 RID: 3009
	private const float DIRT_PER_REFILL = 200f;

	// Token: 0x04000BC2 RID: 3010
	private const float DIRT_PER_USE = 13f;
}
