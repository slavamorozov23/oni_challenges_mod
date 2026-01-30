using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200003A RID: 58
public class CanvasConfig : IBuildingConfig
{
	// Token: 0x06000115 RID: 277 RVA: 0x00008A24 File Offset: 0x00006C24
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Canvas";
		int width = 2;
		int height = 2;
		string anim = "painting_off_kanim";
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
			amount = 10,
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

	// Token: 0x06000116 RID: 278 RVA: 0x00008B24 File Offset: 0x00006D24
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<BuildingComplete>().isArtable = true;
		go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration, false);
	}

	// Token: 0x06000117 RID: 279 RVA: 0x00008B43 File Offset: 0x00006D43
	public override void DoPostConfigureComplete(GameObject go)
	{
		SymbolOverrideControllerUtil.AddToPrefab(go);
		go.AddComponent<Painting>().defaultAnimName = "off";
	}

	// Token: 0x040000AF RID: 175
	public const string ID = "Canvas";
}
