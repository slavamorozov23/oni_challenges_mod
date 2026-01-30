using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002D3 RID: 723
public class LuxuryBedConfig : IBuildingConfig
{
	// Token: 0x06000EB1 RID: 3761 RVA: 0x000557B4 File Offset: 0x000539B4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "LuxuryBed";
		int width = 4;
		int height = 2;
		string anim = "elegantbed_kanim";
		int hitpoints = 10;
		float construction_time = 10f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] plastics = MATERIALS.PLASTICS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, plastics, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER2, none, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AddSearchTerms(SEARCH_TERMS.BED);
		buildingDef.AddSearchTerms(SEARCH_TERMS.MORALE);
		return buildingDef;
	}

	// Token: 0x06000EB2 RID: 3762 RVA: 0x0005582C File Offset: 0x00053A2C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.BedType, false);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.LuxuryBedType, false);
	}

	// Token: 0x06000EB3 RID: 3763 RVA: 0x00055858 File Offset: 0x00053A58
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<KAnimControllerBase>().initialAnim = "off";
		Bed bed = go.AddOrGet<Bed>();
		bed.effects = new string[]
		{
			"LuxuryBedStamina",
			"BedHealth"
		};
		bed.workLayer = Grid.SceneLayer.BuildingFront;
		Sleepable sleepable = go.AddOrGet<Sleepable>();
		sleepable.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_sleep_bed_kanim")
		};
		sleepable.workLayer = Grid.SceneLayer.BuildingFront;
		if (DlcManager.IsContentSubscribed("DLC3_ID"))
		{
			DefragmentationZone defragmentationZone = go.AddOrGet<DefragmentationZone>();
			defragmentationZone.overrideAnims = new KAnimFile[]
			{
				Assets.GetAnim("anim_bionic_kanim")
			};
			defragmentationZone.workLayer = Grid.SceneLayer.BuildingFront;
		}
		go.AddOrGet<Ownable>().slotID = Db.Get().AssignableSlots.Bed.Id;
		go.AddOrGetDef<RocketUsageRestriction.Def>();
	}

	// Token: 0x0400098D RID: 2445
	public const string ID = "LuxuryBed";
}
