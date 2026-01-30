using System;
using STRINGS;
using UnityEngine;

// Token: 0x020002B8 RID: 696
public class LogicGateFilterConfig : LogicGateBaseConfig
{
	// Token: 0x06000E1F RID: 3615 RVA: 0x00052F7D File Offset: 0x0005117D
	protected override LogicGateBase.Op GetLogicOp()
	{
		return LogicGateBase.Op.CustomSingle;
	}

	// Token: 0x17000035 RID: 53
	// (get) Token: 0x06000E20 RID: 3616 RVA: 0x00052F80 File Offset: 0x00051180
	protected override CellOffset[] InputPortOffsets
	{
		get
		{
			return new CellOffset[]
			{
				CellOffset.none
			};
		}
	}

	// Token: 0x17000036 RID: 54
	// (get) Token: 0x06000E21 RID: 3617 RVA: 0x00052F94 File Offset: 0x00051194
	protected override CellOffset[] OutputPortOffsets
	{
		get
		{
			return new CellOffset[]
			{
				new CellOffset(1, 0)
			};
		}
	}

	// Token: 0x17000037 RID: 55
	// (get) Token: 0x06000E22 RID: 3618 RVA: 0x00052FAA File Offset: 0x000511AA
	protected override CellOffset[] ControlPortOffsets
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06000E23 RID: 3619 RVA: 0x00052FB0 File Offset: 0x000511B0
	protected override LogicGate.LogicGateDescriptions GetDescriptions()
	{
		return new LogicGate.LogicGateDescriptions
		{
			outputOne = new LogicGate.LogicGateDescriptions.Description
			{
				name = BUILDINGS.PREFABS.LOGICGATEFILTER.OUTPUT_NAME,
				active = BUILDINGS.PREFABS.LOGICGATEFILTER.OUTPUT_ACTIVE,
				inactive = BUILDINGS.PREFABS.LOGICGATEFILTER.OUTPUT_INACTIVE
			}
		};
	}

	// Token: 0x06000E24 RID: 3620 RVA: 0x00052FFD File Offset: 0x000511FD
	public override BuildingDef CreateBuildingDef()
	{
		return base.CreateBuildingDef("LogicGateFILTER", "logic_filter_kanim", 2, 1);
	}

	// Token: 0x06000E25 RID: 3621 RVA: 0x00053014 File Offset: 0x00051214
	public override void DoPostConfigureComplete(GameObject go)
	{
		LogicGateFilter logicGateFilter = go.AddComponent<LogicGateFilter>();
		logicGateFilter.op = this.GetLogicOp();
		logicGateFilter.inputPortOffsets = this.InputPortOffsets;
		logicGateFilter.outputPortOffsets = this.OutputPortOffsets;
		logicGateFilter.controlPortOffsets = this.ControlPortOffsets;
		go.GetComponent<KPrefabID>().prefabInitFn += delegate(GameObject game_object)
		{
			game_object.GetComponent<LogicGateFilter>().SetPortDescriptions(this.GetDescriptions());
		};
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits, false);
	}

	// Token: 0x0400095A RID: 2394
	public const string ID = "LogicGateFILTER";
}
