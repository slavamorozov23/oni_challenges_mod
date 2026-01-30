using System;
using TUNING;
using UnityEngine;

// Token: 0x02000288 RID: 648
public class LadderConfig : IBuildingConfig
{
	// Token: 0x06000D2A RID: 3370 RVA: 0x0004E51C File Offset: 0x0004C71C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Ladder";
		int width = 1;
		int height = 1;
		string anim = "ladder_kanim";
		int hitpoints = 10;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] raw_MINERALS_OR_WOOD = MATERIALS.RAW_MINERALS_OR_WOOD;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS_OR_WOOD, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
		BuildingTemplates.CreateLadderDef(buildingDef);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.Entombable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.DragBuild = true;
		return buildingDef;
	}

	// Token: 0x06000D2B RID: 3371 RVA: 0x0004E5A5 File Offset: 0x0004C7A5
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		Ladder ladder = go.AddOrGet<Ladder>();
		ladder.upwardsMovementSpeedMultiplier = 1f;
		ladder.downwardsMovementSpeedMultiplier = 1f;
		go.AddOrGet<AnimTileable>();
	}

	// Token: 0x06000D2C RID: 3372 RVA: 0x0004E5CF File Offset: 0x0004C7CF
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000907 RID: 2311
	public const string ID = "Ladder";
}
