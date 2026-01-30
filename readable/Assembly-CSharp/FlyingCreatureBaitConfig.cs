using System;
using TUNING;
using UnityEngine;

// Token: 0x020001CE RID: 462
public class FlyingCreatureBaitConfig : IBuildingConfig
{
	// Token: 0x0600095D RID: 2397 RVA: 0x0003EC3C File Offset: 0x0003CE3C
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("FlyingCreatureBait", 1, 2, "airborne_critter_bait_kanim", 10, 10f, new float[]
		{
			50f,
			10f
		}, new string[]
		{
			"Metal",
			"FlyingCritterEdible"
		}, 1600f, BuildLocationRule.Anywhere, BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.AudioCategory = "Metal";
		buildingDef.Deprecated = true;
		buildingDef.ShowInBuildMenu = false;
		return buildingDef;
	}

	// Token: 0x0600095E RID: 2398 RVA: 0x0003ECBB File Offset: 0x0003CEBB
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<CreatureBait>();
		go.AddTag(GameTags.OneTimeUseLure);
	}

	// Token: 0x0600095F RID: 2399 RVA: 0x0003ECCF File Offset: 0x0003CECF
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
	}

	// Token: 0x06000960 RID: 2400 RVA: 0x0003ECD1 File Offset: 0x0003CED1
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
	}

	// Token: 0x06000961 RID: 2401 RVA: 0x0003ECD4 File Offset: 0x0003CED4
	public override void DoPostConfigureComplete(GameObject go)
	{
		BuildingTemplates.DoPostConfigure(go);
		SymbolOverrideControllerUtil.AddToPrefab(go);
		go.AddOrGet<SymbolOverrideController>().applySymbolOverridesEveryFrame = true;
		Lure.Def def = go.AddOrGetDef<Lure.Def>();
		def.defaultLurePoints = new CellOffset[]
		{
			new CellOffset(0, 0)
		};
		def.radius = 32;
		Prioritizable.AddRef(go);
	}

	// Token: 0x040006F7 RID: 1783
	public const string ID = "FlyingCreatureBait";
}
