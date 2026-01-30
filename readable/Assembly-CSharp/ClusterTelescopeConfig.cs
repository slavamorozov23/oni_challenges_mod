using System;
using TUNING;
using UnityEngine;

// Token: 0x02000049 RID: 73
public class ClusterTelescopeConfig : IBuildingConfig
{
	// Token: 0x06000165 RID: 357 RVA: 0x0000AA55 File Offset: 0x00008C55
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000166 RID: 358 RVA: 0x0000AA5C File Offset: 0x00008C5C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "ClusterTelescope";
		int width = 3;
		int height = 3;
		string anim = "telescope_low_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.ExhaustKilowattsWhenActive = 0.125f;
		buildingDef.SelfHeatKilowattsWhenActive = 0f;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		buildingDef.RequiredSkillPerkID = Db.Get().SkillPerks.CanUseClusterTelescope.Id;
		return buildingDef;
	}

	// Token: 0x06000167 RID: 359 RVA: 0x0000AB08 File Offset: 0x00008D08
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.ScienceBuilding, false);
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		Prioritizable.AddRef(go);
		ClusterTelescope.Def def = go.AddOrGetDef<ClusterTelescope.Def>();
		def.clearScanCellRadius = 4;
		def.workableOverrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_telescope_low_kanim")
		};
		def.skyVisibilityInfo = ClusterTelescopeConfig.SKY_VISIBILITY_INFO;
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 1000f;
		storage.showInUI = true;
		go.AddOrGetDef<PoweredController.Def>();
	}

	// Token: 0x06000168 RID: 360 RVA: 0x0000AB8D File Offset: 0x00008D8D
	public override void DoPostConfigureComplete(GameObject go)
	{
		ClusterTelescopeConfig.AddVisualizer(go);
	}

	// Token: 0x06000169 RID: 361 RVA: 0x0000AB95 File Offset: 0x00008D95
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		ClusterTelescopeConfig.AddVisualizer(go);
	}

	// Token: 0x0600016A RID: 362 RVA: 0x0000AB9D File Offset: 0x00008D9D
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		ClusterTelescopeConfig.AddVisualizer(go);
	}

	// Token: 0x0600016B RID: 363 RVA: 0x0000ABA5 File Offset: 0x00008DA5
	private static void AddVisualizer(GameObject prefab)
	{
		SkyVisibilityVisualizer skyVisibilityVisualizer = prefab.AddOrGet<SkyVisibilityVisualizer>();
		skyVisibilityVisualizer.OriginOffset.y = 1;
		skyVisibilityVisualizer.RangeMin = -4;
		skyVisibilityVisualizer.RangeMax = 4;
		skyVisibilityVisualizer.SkipOnModuleInteriors = true;
	}

	// Token: 0x040000DE RID: 222
	public const string ID = "ClusterTelescope";

	// Token: 0x040000DF RID: 223
	public const int SCAN_RADIUS = 4;

	// Token: 0x040000E0 RID: 224
	public const int VERTICAL_SCAN_OFFSET = 1;

	// Token: 0x040000E1 RID: 225
	public static readonly SkyVisibilityInfo SKY_VISIBILITY_INFO = new SkyVisibilityInfo(new CellOffset(0, 1), 4, new CellOffset(0, 1), 4, 0);
}
