using System;
using TUNING;
using UnityEngine;

// Token: 0x020003E4 RID: 996
public class ResetSkillsStationConfig : IBuildingConfig
{
	// Token: 0x06001474 RID: 5236 RVA: 0x000742D0 File Offset: 0x000724D0
	public override BuildingDef CreateBuildingDef()
	{
		string id = "ResetSkillsStation";
		int width = 3;
		int height = 3;
		string anim = "reSpeccer_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 480f;
		buildingDef.ExhaustKilowattsWhenActive = 0.5f;
		buildingDef.SelfHeatKilowattsWhenActive = 4f;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		return buildingDef;
	}

	// Token: 0x06001475 RID: 5237 RVA: 0x00074360 File Offset: 0x00072560
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddTag(GameTags.NotRoomAssignable);
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		go.AddOrGet<Ownable>().slotID = Db.Get().AssignableSlots.ResetSkillsStation.Id;
		ResetSkillsStation resetSkillsStation = go.AddOrGet<ResetSkillsStation>();
		resetSkillsStation.workTime = 180f;
		resetSkillsStation.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_reSpeccer_kanim")
		};
		resetSkillsStation.workLayer = Grid.SceneLayer.BuildingFront;
	}

	// Token: 0x06001476 RID: 5238 RVA: 0x000743EC File Offset: 0x000725EC
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000C60 RID: 3168
	public const string ID = "ResetSkillsStation";
}
