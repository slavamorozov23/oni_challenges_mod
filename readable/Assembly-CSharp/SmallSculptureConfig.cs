using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000418 RID: 1048
public class SmallSculptureConfig : IBuildingConfig
{
	// Token: 0x0600159E RID: 5534 RVA: 0x0007B7AC File Offset: 0x000799AC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SmallSculpture";
		int width = 1;
		int height = 2;
		string anim = "sculpture_1x2_kanim";
		int hitpoints = 10;
		float construction_time = 60f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, new EffectorValues
		{
			amount = 5,
			radius = 4
		}, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.ViewMode = OverlayModes.Decor.ID;
		buildingDef.DefaultAnimState = "slab";
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.RequiredSkillPerkID = Db.Get().SkillPerks.CanArt.Id;
		buildingDef.AddSearchTerms(SEARCH_TERMS.STATUE);
		buildingDef.AddSearchTerms(SEARCH_TERMS.ARTWORK);
		buildingDef.AddSearchTerms(SEARCH_TERMS.MORALE);
		return buildingDef;
	}

	// Token: 0x0600159F RID: 5535 RVA: 0x0007B891 File Offset: 0x00079A91
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<BuildingComplete>().isArtable = true;
		go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration, false);
	}

	// Token: 0x060015A0 RID: 5536 RVA: 0x0007B8B0 File Offset: 0x00079AB0
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddComponent<Sculpture>().defaultAnimName = "slab";
	}

	// Token: 0x04000CE9 RID: 3305
	public const string ID = "SmallSculpture";
}
