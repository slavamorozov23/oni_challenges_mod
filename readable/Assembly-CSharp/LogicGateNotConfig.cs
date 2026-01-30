using System;
using STRINGS;

// Token: 0x020002B6 RID: 694
public class LogicGateNotConfig : LogicGateBaseConfig
{
	// Token: 0x06000E0F RID: 3599 RVA: 0x00052DC5 File Offset: 0x00050FC5
	protected override LogicGateBase.Op GetLogicOp()
	{
		return LogicGateBase.Op.Not;
	}

	// Token: 0x1700002F RID: 47
	// (get) Token: 0x06000E10 RID: 3600 RVA: 0x00052DC8 File Offset: 0x00050FC8
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

	// Token: 0x17000030 RID: 48
	// (get) Token: 0x06000E11 RID: 3601 RVA: 0x00052DDC File Offset: 0x00050FDC
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

	// Token: 0x17000031 RID: 49
	// (get) Token: 0x06000E12 RID: 3602 RVA: 0x00052DF2 File Offset: 0x00050FF2
	protected override CellOffset[] ControlPortOffsets
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06000E13 RID: 3603 RVA: 0x00052DF8 File Offset: 0x00050FF8
	protected override LogicGate.LogicGateDescriptions GetDescriptions()
	{
		return new LogicGate.LogicGateDescriptions
		{
			outputOne = new LogicGate.LogicGateDescriptions.Description
			{
				name = BUILDINGS.PREFABS.LOGICGATENOT.OUTPUT_NAME,
				active = BUILDINGS.PREFABS.LOGICGATENOT.OUTPUT_ACTIVE,
				inactive = BUILDINGS.PREFABS.LOGICGATENOT.OUTPUT_INACTIVE
			}
		};
	}

	// Token: 0x06000E14 RID: 3604 RVA: 0x00052E45 File Offset: 0x00051045
	public override BuildingDef CreateBuildingDef()
	{
		return base.CreateBuildingDef("LogicGateNOT", "logic_not_kanim", 2, 1);
	}

	// Token: 0x04000958 RID: 2392
	public const string ID = "LogicGateNOT";
}
