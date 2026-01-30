using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000021 RID: 33
public class AutoMinerConfig : IBuildingConfig
{
	// Token: 0x06000094 RID: 148 RVA: 0x00005F78 File Offset: 0x00004178
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("AutoMiner", 2, 2, "auto_miner_kanim", 10, 10f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3, MATERIALS.REFINED_METALS, 1600f, BuildLocationRule.OnFoundationRotatable, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.SelfHeatKilowattsWhenActive = 2f;
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SolidConveyorIDs, "AutoMiner");
		buildingDef.AddSearchTerms(SEARCH_TERMS.ROBOT);
		return buildingDef;
	}

	// Token: 0x06000095 RID: 149 RVA: 0x0000602F File Offset: 0x0000422F
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<Operational>();
		go.AddOrGet<LoopingSounds>();
		go.AddOrGet<MiningSounds>();
	}

	// Token: 0x06000096 RID: 150 RVA: 0x00006057 File Offset: 0x00004257
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		AutoMinerConfig.AddVisualizer(go, true);
	}

	// Token: 0x06000097 RID: 151 RVA: 0x00006060 File Offset: 0x00004260
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		AutoMinerConfig.AddVisualizer(go, false);
	}

	// Token: 0x06000098 RID: 152 RVA: 0x0000606C File Offset: 0x0000426C
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		AutoMiner autoMiner = go.AddOrGet<AutoMiner>();
		autoMiner.x = -7;
		autoMiner.y = 0;
		autoMiner.width = 16;
		autoMiner.height = 9;
		autoMiner.vision_offset = new CellOffset(0, 1);
		AutoMinerConfig.AddVisualizer(go, false);
	}

	// Token: 0x06000099 RID: 153 RVA: 0x000060B8 File Offset: 0x000042B8
	private static void AddVisualizer(GameObject prefab, bool movable)
	{
		RangeVisualizer rangeVisualizer = prefab.AddOrGet<RangeVisualizer>();
		rangeVisualizer.RangeMin.x = -7;
		rangeVisualizer.RangeMin.y = -1;
		rangeVisualizer.RangeMax.x = 8;
		rangeVisualizer.RangeMax.y = 7;
		rangeVisualizer.OriginOffset = new Vector2I(0, 1);
		rangeVisualizer.BlockingTileVisible = false;
		prefab.GetComponent<KPrefabID>().instantiateFn += delegate(GameObject go)
		{
			go.GetComponent<RangeVisualizer>().BlockingCb = AutoMiner.DigBlockingCB;
		};
	}

	// Token: 0x0400007B RID: 123
	public const string ID = "AutoMiner";

	// Token: 0x0400007C RID: 124
	private const int RANGE = 7;

	// Token: 0x0400007D RID: 125
	private const int X = -7;

	// Token: 0x0400007E RID: 126
	private const int Y = 0;

	// Token: 0x0400007F RID: 127
	private const int WIDTH = 16;

	// Token: 0x04000080 RID: 128
	private const int HEIGHT = 9;

	// Token: 0x04000081 RID: 129
	private const int VISION_OFFSET = 1;
}
