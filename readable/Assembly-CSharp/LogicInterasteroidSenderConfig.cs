using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002BF RID: 703
public class LogicInterasteroidSenderConfig : IBuildingConfig
{
	// Token: 0x06000E52 RID: 3666 RVA: 0x00053867 File Offset: 0x00051A67
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000E53 RID: 3667 RVA: 0x00053870 File Offset: 0x00051A70
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("LogicInterasteroidSender", 1, 1, "inter_asteroid_automation_signal_sender_kanim", 30, 30f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2, MATERIALS.REFINED_METALS, 1600f, BuildLocationRule.OnFloor, TUNING.BUILDINGS.DECOR.PENALTY.TIER0, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.Entombable = true;
		buildingDef.AudioCategory = "Metal";
		buildingDef.ViewMode = OverlayModes.Logic.ID;
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.PermittedRotations = PermittedRotations.Unrotatable;
		buildingDef.AlwaysOperational = false;
		buildingDef.LogicInputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.InputPort("InputPort", new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.LOGICDUPLICANTSENSOR.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.LOGICINTERASTEROIDSENDER.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.LOGICINTERASTEROIDSENDER.LOGIC_PORT_INACTIVE, true, false)
		};
		GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, "LogicInterasteroidSender");
		buildingDef.AddSearchTerms(SEARCH_TERMS.AUTOMATION);
		return buildingDef;
	}

	// Token: 0x06000E54 RID: 3668 RVA: 0x0005395B File Offset: 0x00051B5B
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		base.ConfigureBuildingTemplate(go, prefab_tag);
		go.AddOrGet<UserNameable>().savedName = STRINGS.BUILDINGS.PREFABS.LOGICINTERASTEROIDSENDER.DEFAULTNAME;
		go.AddOrGet<LogicBroadcaster>().PORT_ID = "InputPort";
	}

	// Token: 0x06000E55 RID: 3669 RVA: 0x0005398A File Offset: 0x00051B8A
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		LogicInterasteroidSenderConfig.AddVisualizer(go);
	}

	// Token: 0x06000E56 RID: 3670 RVA: 0x00053992 File Offset: 0x00051B92
	public override void DoPostConfigureComplete(GameObject go)
	{
		LogicInterasteroidSenderConfig.AddVisualizer(go);
	}

	// Token: 0x06000E57 RID: 3671 RVA: 0x0005399A File Offset: 0x00051B9A
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		LogicInterasteroidSenderConfig.AddVisualizer(go);
	}

	// Token: 0x06000E58 RID: 3672 RVA: 0x000539A2 File Offset: 0x00051BA2
	private static void AddVisualizer(GameObject prefab)
	{
		SkyVisibilityVisualizer skyVisibilityVisualizer = prefab.AddOrGet<SkyVisibilityVisualizer>();
		skyVisibilityVisualizer.RangeMin = 0;
		skyVisibilityVisualizer.RangeMax = 0;
		skyVisibilityVisualizer.SkipOnModuleInteriors = true;
	}

	// Token: 0x04000962 RID: 2402
	public const string ID = "LogicInterasteroidSender";

	// Token: 0x04000963 RID: 2403
	public const string INPUT_PORT_ID = "InputPort";
}
