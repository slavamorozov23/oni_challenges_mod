using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200046D RID: 1133
public class WoodSculptureConfig : IBuildingConfig
{
	// Token: 0x060017AF RID: 6063 RVA: 0x000863C0 File Offset: 0x000845C0
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x060017B0 RID: 6064 RVA: 0x000863C8 File Offset: 0x000845C8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "WoodSculpture";
		int width = 1;
		int height = 1;
		string anim = "sculpture_wood_kanim";
		int hitpoints = 10;
		float construction_time = 120f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] woods = MATERIALS.WOODS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, woods, melting_point, build_location_rule, new EffectorValues
		{
			amount = 3,
			radius = 4
		}, none, 0.2f);
		buildingDef.SceneLayer = Grid.SceneLayer.InteriorWall;
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

	// Token: 0x060017B1 RID: 6065 RVA: 0x000864B5 File Offset: 0x000846B5
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<BuildingComplete>().isArtable = true;
		go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration, false);
	}

	// Token: 0x060017B2 RID: 6066 RVA: 0x000864D4 File Offset: 0x000846D4
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddComponent<LongRangeSculpture>().defaultAnimName = "slab";
	}

	// Token: 0x04000DEB RID: 3563
	public const string ID = "WoodSculpture";
}
