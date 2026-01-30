using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002BB RID: 699
public abstract class LogicGateBaseConfig : IBuildingConfig
{
	// Token: 0x06000E36 RID: 3638 RVA: 0x0005326C File Offset: 0x0005146C
	protected BuildingDef CreateBuildingDef(string ID, string anim, int width = 2, int height = 2)
	{
		int hitpoints = 10;
		float construction_time = 3f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(ID, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
		buildingDef.ViewMode = OverlayModes.Logic.ID;
		buildingDef.ObjectLayer = ObjectLayer.LogicGate;
		buildingDef.SceneLayer = Grid.SceneLayer.LogicGates;
		buildingDef.ThermalConductivity = 0.05f;
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.Entombable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.DragBuild = true;
		buildingDef.AddSearchTerms(SEARCH_TERMS.AUTOMATION);
		LogicGateBase.uiSrcData = Assets.instance.logicModeUIData;
		GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, ID);
		return buildingDef;
	}

	// Token: 0x1700003E RID: 62
	// (get) Token: 0x06000E37 RID: 3639
	protected abstract CellOffset[] InputPortOffsets { get; }

	// Token: 0x1700003F RID: 63
	// (get) Token: 0x06000E38 RID: 3640
	protected abstract CellOffset[] OutputPortOffsets { get; }

	// Token: 0x17000040 RID: 64
	// (get) Token: 0x06000E39 RID: 3641
	protected abstract CellOffset[] ControlPortOffsets { get; }

	// Token: 0x06000E3A RID: 3642
	protected abstract LogicGateBase.Op GetLogicOp();

	// Token: 0x06000E3B RID: 3643
	protected abstract LogicGate.LogicGateDescriptions GetDescriptions();

	// Token: 0x06000E3C RID: 3644 RVA: 0x0005333F File Offset: 0x0005153F
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
	}

	// Token: 0x06000E3D RID: 3645 RVA: 0x0005335C File Offset: 0x0005155C
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		base.DoPostConfigurePreview(def, go);
		MoveableLogicGateVisualizer moveableLogicGateVisualizer = go.AddComponent<MoveableLogicGateVisualizer>();
		moveableLogicGateVisualizer.op = this.GetLogicOp();
		moveableLogicGateVisualizer.inputPortOffsets = this.InputPortOffsets;
		moveableLogicGateVisualizer.outputPortOffsets = this.OutputPortOffsets;
		moveableLogicGateVisualizer.controlPortOffsets = this.ControlPortOffsets;
	}

	// Token: 0x06000E3E RID: 3646 RVA: 0x0005339B File Offset: 0x0005159B
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		LogicGateVisualizer logicGateVisualizer = go.AddComponent<LogicGateVisualizer>();
		logicGateVisualizer.op = this.GetLogicOp();
		logicGateVisualizer.inputPortOffsets = this.InputPortOffsets;
		logicGateVisualizer.outputPortOffsets = this.OutputPortOffsets;
		logicGateVisualizer.controlPortOffsets = this.ControlPortOffsets;
	}

	// Token: 0x06000E3F RID: 3647 RVA: 0x000533DC File Offset: 0x000515DC
	public override void DoPostConfigureComplete(GameObject go)
	{
		LogicGate logicGate = go.AddComponent<LogicGate>();
		logicGate.op = this.GetLogicOp();
		logicGate.inputPortOffsets = this.InputPortOffsets;
		logicGate.outputPortOffsets = this.OutputPortOffsets;
		logicGate.controlPortOffsets = this.ControlPortOffsets;
		go.GetComponent<KPrefabID>().prefabInitFn += delegate(GameObject game_object)
		{
			game_object.GetComponent<LogicGate>().SetPortDescriptions(this.GetDescriptions());
		};
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits, false);
	}
}
