using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200003B RID: 59
public class CanvasTallConfig : IBuildingConfig
{
	// Token: 0x06000119 RID: 281 RVA: 0x00008B64 File Offset: 0x00006D64
	public override BuildingDef CreateBuildingDef()
	{
		string id = "CanvasTall";
		int width = 2;
		int height = 3;
		string anim = "painting_tall_off_kanim";
		int hitpoints = 30;
		float construction_time = 120f;
		float[] construction_mass = new float[]
		{
			400f,
			1f
		};
		string[] construction_materials = new string[]
		{
			"Metal",
			"BuildingFiber"
		};
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, new EffectorValues
		{
			amount = 15,
			radius = 6
		}, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.SceneLayer = Grid.SceneLayer.InteriorWall;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.ViewMode = OverlayModes.Decor.ID;
		buildingDef.DefaultAnimState = "off";
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.RequiredSkillPerkID = Db.Get().SkillPerks.CanArt.Id;
		buildingDef.AddSearchTerms(SEARCH_TERMS.MORALE);
		buildingDef.AddSearchTerms(SEARCH_TERMS.ARTWORK);
		return buildingDef;
	}

	// Token: 0x0600011A RID: 282 RVA: 0x00008C64 File Offset: 0x00006E64
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<BuildingComplete>().isArtable = true;
		go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration, false);
	}

	// Token: 0x0600011B RID: 283 RVA: 0x00008C83 File Offset: 0x00006E83
	public override void DoPostConfigureComplete(GameObject go)
	{
		SymbolOverrideControllerUtil.AddToPrefab(go);
		go.AddComponent<Painting>().defaultAnimName = "off";
	}

	// Token: 0x040000B0 RID: 176
	public const string ID = "CanvasTall";
}
