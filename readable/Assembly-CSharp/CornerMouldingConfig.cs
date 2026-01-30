using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000062 RID: 98
public class CornerMouldingConfig : IBuildingConfig
{
	// Token: 0x060001D3 RID: 467 RVA: 0x0000D54C File Offset: 0x0000B74C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "CornerMoulding";
		int width = 1;
		int height = 1;
		string anim = "corner_tile_kanim";
		int hitpoints = 10;
		float construction_time = 30f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.InCorner;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, new EffectorValues
		{
			amount = 5,
			radius = 3
		}, none, 0.2f);
		buildingDef.DefaultAnimState = "corner";
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = OverlayModes.Decor.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.AddSearchTerms(SEARCH_TERMS.MORALE);
		return buildingDef;
	}

	// Token: 0x060001D4 RID: 468 RVA: 0x0000D5F7 File Offset: 0x0000B7F7
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration, false);
	}

	// Token: 0x060001D5 RID: 469 RVA: 0x0000D60A File Offset: 0x0000B80A
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x0400012B RID: 299
	public const string ID = "CornerMoulding";
}
