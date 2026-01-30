using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000460 RID: 1120
public class WashBasinConfig : IBuildingConfig
{
	// Token: 0x0600176C RID: 5996 RVA: 0x00085084 File Offset: 0x00083284
	public override BuildingDef CreateBuildingDef()
	{
		string id = "WashBasin";
		int width = 2;
		int height = 3;
		string anim = "wash_basin_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		string[] raw_MINERALS_OR_METALS = MATERIALS.RAW_MINERALS_OR_METALS;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
		string[] construction_materials = raw_MINERALS_OR_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, construction_materials, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER1, tier2, 0.2f);
		buildingDef.AddSearchTerms(SEARCH_TERMS.SINK);
		return buildingDef;
	}

	// Token: 0x0600176D RID: 5997 RVA: 0x000850DC File Offset: 0x000832DC
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.WashStation, false);
		HandSanitizer handSanitizer = go.AddOrGet<HandSanitizer>();
		handSanitizer.massConsumedPerUse = 5f;
		handSanitizer.consumedElement = SimHashes.Water;
		handSanitizer.outputElement = SimHashes.DirtyWater;
		handSanitizer.diseaseRemovalCount = WashBasinConfig.DISEASE_REMOVAL_COUNT;
		handSanitizer.maxUses = 40;
		handSanitizer.dumpWhenFull = true;
		go.AddOrGet<DirectionControl>();
		HandSanitizer.Work work = go.AddOrGet<HandSanitizer.Work>();
		KAnimFile[] overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_washbasin_kanim")
		};
		work.overrideAnims = overrideAnims;
		work.workTime = 5f;
		work.trackUses = true;
		Storage storage = go.AddOrGet<Storage>();
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = GameTagExtensions.Create(SimHashes.Water);
		manualDeliveryKG.MinimumMass = 5f;
		manualDeliveryKG.capacity = 200f;
		manualDeliveryKG.refillMass = 40f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().prefabInitFn += this.OnInit;
	}

	// Token: 0x0600176E RID: 5998 RVA: 0x00085204 File Offset: 0x00083404
	private void OnInit(GameObject go)
	{
		HandSanitizer.Work component = go.GetComponent<HandSanitizer.Work>();
		KAnimFile[] value = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_washbasin_kanim")
		};
		component.workerTypeOverrideAnims.Add(MinionConfig.ID, value);
		component.workerTypeOverrideAnims.Add(BionicMinionConfig.ID, new KAnimFile[]
		{
			Assets.GetAnim("anim_bionic_interacts_washbasin_kanim")
		});
	}

	// Token: 0x0600176F RID: 5999 RVA: 0x00085274 File Offset: 0x00083474
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000DC6 RID: 3526
	public const string ID = "WashBasin";

	// Token: 0x04000DC7 RID: 3527
	public static readonly int DISEASE_REMOVAL_COUNT = DUPLICANTSTATS.STANDARD.Secretions.DISEASE_PER_PEE + 20000;

	// Token: 0x04000DC8 RID: 3528
	public const float WATER_PER_USE = 5f;

	// Token: 0x04000DC9 RID: 3529
	public const int USES_PER_FLUSH = 40;

	// Token: 0x04000DCA RID: 3530
	public const float WORK_TIME = 5f;
}
