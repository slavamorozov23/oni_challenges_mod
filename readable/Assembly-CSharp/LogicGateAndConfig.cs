using System;
using STRINGS;

// Token: 0x020002B3 RID: 691
public class LogicGateAndConfig : LogicGateBaseConfig
{
	// Token: 0x06000DFA RID: 3578 RVA: 0x00052BCE File Offset: 0x00050DCE
	protected override LogicGateBase.Op GetLogicOp()
	{
		return LogicGateBase.Op.And;
	}

	// Token: 0x17000026 RID: 38
	// (get) Token: 0x06000DFB RID: 3579 RVA: 0x00052BD1 File Offset: 0x00050DD1
	protected override CellOffset[] InputPortOffsets
	{
		get
		{
			return new CellOffset[]
			{
				CellOffset.none,
				new CellOffset(0, 1)
			};
		}
	}

	// Token: 0x17000027 RID: 39
	// (get) Token: 0x06000DFC RID: 3580 RVA: 0x00052BF3 File Offset: 0x00050DF3
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

	// Token: 0x17000028 RID: 40
	// (get) Token: 0x06000DFD RID: 3581 RVA: 0x00052C09 File Offset: 0x00050E09
	protected override CellOffset[] ControlPortOffsets
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06000DFE RID: 3582 RVA: 0x00052C0C File Offset: 0x00050E0C
	protected override LogicGate.LogicGateDescriptions GetDescriptions()
	{
		return new LogicGate.LogicGateDescriptions
		{
			outputOne = new LogicGate.LogicGateDescriptions.Description
			{
				name = BUILDINGS.PREFABS.LOGICGATEAND.OUTPUT_NAME,
				active = BUILDINGS.PREFABS.LOGICGATEAND.OUTPUT_ACTIVE,
				inactive = BUILDINGS.PREFABS.LOGICGATEAND.OUTPUT_INACTIVE
			}
		};
	}

	// Token: 0x06000DFF RID: 3583 RVA: 0x00052C59 File Offset: 0x00050E59
	public override BuildingDef CreateBuildingDef()
	{
		return base.CreateBuildingDef("LogicGateAND", "logic_and_kanim", 2, 2);
	}

	// Token: 0x04000955 RID: 2389
	public const string ID = "LogicGateAND";
}
