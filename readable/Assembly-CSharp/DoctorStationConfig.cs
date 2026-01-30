using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200007D RID: 125
public class DoctorStationConfig : IBuildingConfig
{
	// Token: 0x06000257 RID: 599 RVA: 0x00010570 File Offset: 0x0000E770
	public override BuildingDef CreateBuildingDef()
	{
		string id = "DoctorStation";
		int width = 3;
		int height = 2;
		string anim = "treatment_chair_kanim";
		int hitpoints = 10;
		float construction_time = 10f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, none, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.RequiredSkillPerkID = Db.Get().SkillPerks.CanDoctor.Id;
		buildingDef.AddSearchTerms(SEARCH_TERMS.MEDICINE);
		return buildingDef;
	}

	// Token: 0x06000258 RID: 600 RVA: 0x000105F2 File Offset: 0x0000E7F2
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.Clinic, false);
	}

	// Token: 0x06000259 RID: 601 RVA: 0x0001060C File Offset: 0x0000E80C
	public override void DoPostConfigureComplete(GameObject go)
	{
		Storage storage = go.AddOrGet<Storage>();
		storage.showInUI = true;
		Tag supplyTagForStation = MedicineInfo.GetSupplyTagForStation("DoctorStation");
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = supplyTagForStation;
		manualDeliveryKG.capacity = 10f;
		manualDeliveryKG.refillMass = 5f;
		manualDeliveryKG.MinimumMass = 1f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.DoctorFetch.IdHash;
		manualDeliveryKG.operationalRequirement = Operational.State.Functional;
		DoctorStation doctorStation = go.AddOrGet<DoctorStation>();
		doctorStation.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_treatment_chair_sick_kanim")
		};
		doctorStation.workLayer = Grid.SceneLayer.BuildingFront;
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.Hospital.Id;
		roomTracker.requirement = RoomTracker.Requirement.CustomRecommended;
		roomTracker.customStatusItemID = Db.Get().BuildingStatusItems.ClinicOutsideHospital.Id;
		DoctorStationDoctorWorkable doctorStationDoctorWorkable = go.AddOrGet<DoctorStationDoctorWorkable>();
		doctorStationDoctorWorkable.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_treatment_chair_doctor_kanim")
		};
		doctorStationDoctorWorkable.SetWorkTime(40f);
		doctorStationDoctorWorkable.requiredSkillPerk = Db.Get().SkillPerks.CanDoctor.Id;
	}

	// Token: 0x0400017A RID: 378
	public const string ID = "DoctorStation";
}
