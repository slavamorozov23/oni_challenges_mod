using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002A7 RID: 679
public class LiquidPumpingStationConfig : IBuildingConfig
{
	// Token: 0x06000DC6 RID: 3526 RVA: 0x00051978 File Offset: 0x0004FB78
	public override BuildingDef CreateBuildingDef()
	{
		string id = "LiquidPumpingStation";
		int width = 2;
		int height = 4;
		string anim = "waterpump_kanim";
		int hitpoints = 100;
		float construction_time = 10f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Entombable = true;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
		buildingDef.DefaultAnimState = "on";
		buildingDef.ShowInBuildMenu = true;
		buildingDef.AddSearchTerms(SEARCH_TERMS.WATER);
		return buildingDef;
	}

	// Token: 0x06000DC7 RID: 3527 RVA: 0x00051A20 File Offset: 0x0004FC20
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		go.AddOrGet<LiquidPumpingStation>().overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_waterpump_kanim")
		};
		Storage storage = go.AddOrGet<Storage>();
		storage.showInUI = false;
		storage.allowItemRemoval = true;
		storage.showDescriptor = true;
		storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
		go.AddTag(GameTags.CorrosionProof);
		go.AddTag(GameTags.LiquidSource);
	}

	// Token: 0x06000DC8 RID: 3528 RVA: 0x00051AA0 File Offset: 0x0004FCA0
	private static void AddGuide(GameObject go, bool occupy_tiles)
	{
		GameObject gameObject = new GameObject();
		gameObject.transform.parent = go.transform;
		gameObject.transform.SetLocalPosition(Vector3.zero);
		KBatchedAnimController kbatchedAnimController = gameObject.AddComponent<KBatchedAnimController>();
		kbatchedAnimController.Offset = go.GetComponent<Building>().Def.GetVisualizerOffset();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim(new HashedString("waterpump_kanim"))
		};
		kbatchedAnimController.initialAnim = "place_guide";
		kbatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.OffscreenUpdate;
		kbatchedAnimController.isMovable = true;
		PumpingStationGuide pumpingStationGuide = gameObject.AddComponent<PumpingStationGuide>();
		pumpingStationGuide.parent = go;
		pumpingStationGuide.occupyTiles = occupy_tiles;
	}

	// Token: 0x06000DC9 RID: 3529 RVA: 0x00051B3C File Offset: 0x0004FD3C
	public override void DoPostConfigureComplete(GameObject go)
	{
		LiquidPumpingStationConfig.AddGuide(go.GetComponent<Building>().Def.BuildingPreview, false);
		LiquidPumpingStationConfig.AddGuide(go.GetComponent<Building>().Def.BuildingUnderConstruction, false);
		go.AddOrGet<FakeFloorAdder>().floorOffsets = new CellOffset[]
		{
			new CellOffset(0, 0),
			new CellOffset(1, 0)
		};
	}

	// Token: 0x04000941 RID: 2369
	public const string ID = "LiquidPumpingStation";

	// Token: 0x04000942 RID: 2370
	public const int TAIL_LENGTH = 4;
}
