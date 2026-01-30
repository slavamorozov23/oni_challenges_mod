using System;
using STRINGS;

// Token: 0x020002B5 RID: 693
public class LogicGateXorConfig : LogicGateBaseConfig
{
	// Token: 0x06000E08 RID: 3592 RVA: 0x00052D1D File Offset: 0x00050F1D
	protected override LogicGateBase.Op GetLogicOp()
	{
		return LogicGateBase.Op.Xor;
	}

	// Token: 0x1700002C RID: 44
	// (get) Token: 0x06000E09 RID: 3593 RVA: 0x00052D20 File Offset: 0x00050F20
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

	// Token: 0x1700002D RID: 45
	// (get) Token: 0x06000E0A RID: 3594 RVA: 0x00052D42 File Offset: 0x00050F42
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

	// Token: 0x1700002E RID: 46
	// (get) Token: 0x06000E0B RID: 3595 RVA: 0x00052D58 File Offset: 0x00050F58
	protected override CellOffset[] ControlPortOffsets
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06000E0C RID: 3596 RVA: 0x00052D5C File Offset: 0x00050F5C
	protected override LogicGate.LogicGateDescriptions GetDescriptions()
	{
		return new LogicGate.LogicGateDescriptions
		{
			outputOne = new LogicGate.LogicGateDescriptions.Description
			{
				name = BUILDINGS.PREFABS.LOGICGATEXOR.OUTPUT_NAME,
				active = BUILDINGS.PREFABS.LOGICGATEXOR.OUTPUT_ACTIVE,
				inactive = BUILDINGS.PREFABS.LOGICGATEXOR.OUTPUT_INACTIVE
			}
		};
	}

	// Token: 0x06000E0D RID: 3597 RVA: 0x00052DA9 File Offset: 0x00050FA9
	public override BuildingDef CreateBuildingDef()
	{
		return base.CreateBuildingDef("LogicGateXOR", "logic_xor_kanim", 2, 2);
	}

	// Token: 0x04000957 RID: 2391
	public const string ID = "LogicGateXOR";
}
