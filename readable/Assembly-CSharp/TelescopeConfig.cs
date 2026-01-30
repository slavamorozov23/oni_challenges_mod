using System;
using TUNING;
using UnityEngine;

// Token: 0x0200044C RID: 1100
public class TelescopeConfig : IBuildingConfig
{
	// Token: 0x060016D0 RID: 5840 RVA: 0x00082503 File Offset: 0x00080703
	public override string[] GetForbiddenDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060016D1 RID: 5841 RVA: 0x0008250C File Offset: 0x0008070C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Telescope";
		int width = 4;
		int height = 6;
		string anim = "telescope_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.ExhaustKilowattsWhenActive = 0.125f;
		buildingDef.SelfHeatKilowattsWhenActive = 0f;
		buildingDef.InputConduitType = ConduitType.Gas;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		buildingDef.RequiredSkillPerkID = Db.Get().SkillPerks.CanStudyWorldObjects.Id;
		return buildingDef;
	}

	// Token: 0x060016D2 RID: 5842 RVA: 0x000825CC File Offset: 0x000807CC
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.ScienceBuilding, false);
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		Prioritizable.AddRef(go);
		Telescope telescope = go.AddOrGet<Telescope>();
		telescope.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_telescope_kanim")
		};
		telescope.requiredSkillPerk = Db.Get().SkillPerks.CanStudyWorldObjects.Id;
		telescope.workLayer = Grid.SceneLayer.BuildingFront;
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 1000f;
		storage.showInUI = true;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Gas;
		conduitConsumer.consumptionRate = 1f;
		conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Oxygen).tag;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		conduitConsumer.capacityKG = 10f;
		conduitConsumer.forceAlwaysSatisfied = true;
		go.AddOrGetDef<PoweredController.Def>();
	}

	// Token: 0x060016D3 RID: 5843 RVA: 0x000826A6 File Offset: 0x000808A6
	public override void DoPostConfigureComplete(GameObject go)
	{
		TelescopeConfig.AddVisualizer(go);
	}

	// Token: 0x060016D4 RID: 5844 RVA: 0x000826AE File Offset: 0x000808AE
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		TelescopeConfig.AddVisualizer(go);
	}

	// Token: 0x060016D5 RID: 5845 RVA: 0x000826B6 File Offset: 0x000808B6
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		TelescopeConfig.AddVisualizer(go);
	}

	// Token: 0x060016D6 RID: 5846 RVA: 0x000826BE File Offset: 0x000808BE
	private static void AddVisualizer(GameObject prefab)
	{
		SkyVisibilityVisualizer skyVisibilityVisualizer = prefab.AddOrGet<SkyVisibilityVisualizer>();
		skyVisibilityVisualizer.OriginOffset.y = 3;
		skyVisibilityVisualizer.TwoWideOrgin = true;
		skyVisibilityVisualizer.RangeMin = -4;
		skyVisibilityVisualizer.RangeMax = 5;
		skyVisibilityVisualizer.SkipOnModuleInteriors = true;
	}

	// Token: 0x04000D85 RID: 3461
	public const string ID = "Telescope";

	// Token: 0x04000D86 RID: 3462
	public const float POINTS_PER_DAY = 2f;

	// Token: 0x04000D87 RID: 3463
	public const float MASS_PER_POINT = 2f;

	// Token: 0x04000D88 RID: 3464
	public const float CAPACITY = 30f;

	// Token: 0x04000D89 RID: 3465
	public const int SCAN_RADIUS = 4;

	// Token: 0x04000D8A RID: 3466
	public const int VERTICAL_SCAN_OFFSET = 3;

	// Token: 0x04000D8B RID: 3467
	public static readonly SkyVisibilityInfo SKY_VISIBILITY_INFO = new SkyVisibilityInfo(new CellOffset(0, 3), 4, new CellOffset(1, 3), 4, 0);

	// Token: 0x04000D8C RID: 3468
	public static readonly Tag INPUT_MATERIAL = GameTags.Glass;
}
