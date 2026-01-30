using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200002B RID: 43
public class BedConfig : IBuildingConfig
{
	// Token: 0x060000C7 RID: 199 RVA: 0x00006DC8 File Offset: 0x00004FC8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Bed";
		int width = 2;
		int height = 2;
		string anim = "bedlg_kanim";
		int hitpoints = 10;
		float construction_time = 10f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] raw_MINERALS_OR_WOOD = MATERIALS.RAW_MINERALS_OR_WOOD;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS_OR_WOOD, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, none, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AddSearchTerms(SEARCH_TERMS.BED);
		return buildingDef;
	}

	// Token: 0x060000C8 RID: 200 RVA: 0x00006E30 File Offset: 0x00005030
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.BedType, false);
	}

	// Token: 0x060000C9 RID: 201 RVA: 0x00006E4C File Offset: 0x0000504C
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<KAnimControllerBase>().initialAnim = "off";
		Bed bed = go.AddOrGet<Bed>();
		bed.effects = new string[]
		{
			"BedStamina",
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

	// Token: 0x0400008D RID: 141
	public const string ID = "Bed";
}
