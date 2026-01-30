using System;
using STRINGS;

// Token: 0x020002B4 RID: 692
public class LogicGateOrConfig : LogicGateBaseConfig
{
	// Token: 0x06000E01 RID: 3585 RVA: 0x00052C75 File Offset: 0x00050E75
	protected override LogicGateBase.Op GetLogicOp()
	{
		return LogicGateBase.Op.Or;
	}

	// Token: 0x17000029 RID: 41
	// (get) Token: 0x06000E02 RID: 3586 RVA: 0x00052C78 File Offset: 0x00050E78
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

	// Token: 0x1700002A RID: 42
	// (get) Token: 0x06000E03 RID: 3587 RVA: 0x00052C9A File Offset: 0x00050E9A
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

	// Token: 0x1700002B RID: 43
	// (get) Token: 0x06000E04 RID: 3588 RVA: 0x00052CB0 File Offset: 0x00050EB0
	protected override CellOffset[] ControlPortOffsets
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06000E05 RID: 3589 RVA: 0x00052CB4 File Offset: 0x00050EB4
	protected override LogicGate.LogicGateDescriptions GetDescriptions()
	{
		return new LogicGate.LogicGateDescriptions
		{
			outputOne = new LogicGate.LogicGateDescriptions.Description
			{
				name = BUILDINGS.PREFABS.LOGICGATEOR.OUTPUT_NAME,
				active = BUILDINGS.PREFABS.LOGICGATEOR.OUTPUT_ACTIVE,
				inactive = BUILDINGS.PREFABS.LOGICGATEOR.OUTPUT_INACTIVE
			}
		};
	}

	// Token: 0x06000E06 RID: 3590 RVA: 0x00052D01 File Offset: 0x00050F01
	public override BuildingDef CreateBuildingDef()
	{
		return base.CreateBuildingDef("LogicGateOR", "logic_or_kanim", 2, 2);
	}

	// Token: 0x04000956 RID: 2390
	public const string ID = "LogicGateOR";
}
