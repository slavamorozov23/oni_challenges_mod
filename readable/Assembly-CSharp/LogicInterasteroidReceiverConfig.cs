using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002BE RID: 702
public class LogicInterasteroidReceiverConfig : IBuildingConfig
{
	// Token: 0x06000E4B RID: 3659 RVA: 0x00053729 File Offset: 0x00051929
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000E4C RID: 3660 RVA: 0x00053730 File Offset: 0x00051930
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("LogicInterasteroidReceiver", 1, 1, "inter_asteroid_automation_signal_receiver_kanim", 30, 30f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2, MATERIALS.REFINED_METALS, 1600f, BuildLocationRule.OnFloor, TUNING.BUILDINGS.DECOR.PENALTY.TIER0, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.Entombable = true;
		buildingDef.AudioCategory = "Metal";
		buildingDef.ViewMode = OverlayModes.Logic.ID;
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.PermittedRotations = PermittedRotations.Unrotatable;
		buildingDef.AlwaysOperational = false;
		buildingDef.LogicOutputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort("OutputPort", new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.LOGICINTERASTEROIDRECEIVER.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.LOGICINTERASTEROIDRECEIVER.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.LOGICINTERASTEROIDRECEIVER.LOGIC_PORT_INACTIVE, true, false)
		};
		GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, "LogicInterasteroidReceiver");
		buildingDef.AddSearchTerms(SEARCH_TERMS.AUTOMATION);
		return buildingDef;
	}

	// Token: 0x06000E4D RID: 3661 RVA: 0x0005381B File Offset: 0x00051A1B
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		LogicInterasteroidReceiverConfig.AddVisualizer(go);
	}

	// Token: 0x06000E4E RID: 3662 RVA: 0x00053823 File Offset: 0x00051A23
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicBroadcastReceiver>().PORT_ID = "OutputPort";
		LogicInterasteroidReceiverConfig.AddVisualizer(go);
	}

	// Token: 0x06000E4F RID: 3663 RVA: 0x0005383B File Offset: 0x00051A3B
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		LogicInterasteroidReceiverConfig.AddVisualizer(go);
	}

	// Token: 0x06000E50 RID: 3664 RVA: 0x00053843 File Offset: 0x00051A43
	private static void AddVisualizer(GameObject prefab)
	{
		SkyVisibilityVisualizer skyVisibilityVisualizer = prefab.AddOrGet<SkyVisibilityVisualizer>();
		skyVisibilityVisualizer.RangeMin = 0;
		skyVisibilityVisualizer.RangeMax = 0;
		skyVisibilityVisualizer.SkipOnModuleInteriors = true;
	}

	// Token: 0x04000960 RID: 2400
	public const string ID = "LogicInterasteroidReceiver";

	// Token: 0x04000961 RID: 2401
	public const string OUTPUT_PORT_ID = "OutputPort";
}
