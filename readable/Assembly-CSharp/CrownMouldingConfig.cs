using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200006C RID: 108
public class CrownMouldingConfig : IBuildingConfig
{
	// Token: 0x06000206 RID: 518 RVA: 0x0000E8DC File Offset: 0x0000CADC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "CrownMoulding";
		int width = 1;
		int height = 1;
		string anim = "crown_moulding_kanim";
		int hitpoints = 10;
		float construction_time = 30f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnCeiling;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, new EffectorValues
		{
			amount = 5,
			radius = 3
		}, none, 0.2f);
		buildingDef.DefaultAnimState = "S_U";
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = OverlayModes.Decor.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.AddSearchTerms(SEARCH_TERMS.MORALE);
		return buildingDef;
	}

	// Token: 0x06000207 RID: 519 RVA: 0x0000E980 File Offset: 0x0000CB80
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration, false);
		go.AddOrGet<AnimTileable>();
	}

	// Token: 0x06000208 RID: 520 RVA: 0x0000E99A File Offset: 0x0000CB9A
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x0400013F RID: 319
	public const string ID = "CrownMoulding";
}
