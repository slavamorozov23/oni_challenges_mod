using System;
using STRINGS;

// Token: 0x020002B9 RID: 697
public class LogicGateMultiplexerConfig : LogicGateBaseConfig
{
	// Token: 0x06000E28 RID: 3624 RVA: 0x00053099 File Offset: 0x00051299
	protected override LogicGateBase.Op GetLogicOp()
	{
		return LogicGateBase.Op.Multiplexer;
	}

	// Token: 0x17000038 RID: 56
	// (get) Token: 0x06000E29 RID: 3625 RVA: 0x0005309C File Offset: 0x0005129C
	protected override CellOffset[] InputPortOffsets
	{
		get
		{
			return new CellOffset[]
			{
				new CellOffset(-1, 3),
				new CellOffset(-1, 2),
				new CellOffset(-1, 1),
				new CellOffset(-1, 0)
			};
		}
	}

	// Token: 0x17000039 RID: 57
	// (get) Token: 0x06000E2A RID: 3626 RVA: 0x000530DC File Offset: 0x000512DC
	protected override CellOffset[] OutputPortOffsets
	{
		get
		{
			return new CellOffset[]
			{
				new CellOffset(1, 3)
			};
		}
	}

	// Token: 0x1700003A RID: 58
	// (get) Token: 0x06000E2B RID: 3627 RVA: 0x000530F2 File Offset: 0x000512F2
	protected override CellOffset[] ControlPortOffsets
	{
		get
		{
			return new CellOffset[]
			{
				new CellOffset(0, 0),
				new CellOffset(1, 0)
			};
		}
	}

	// Token: 0x06000E2C RID: 3628 RVA: 0x00053118 File Offset: 0x00051318
	protected override LogicGate.LogicGateDescriptions GetDescriptions()
	{
		return new LogicGate.LogicGateDescriptions
		{
			outputOne = new LogicGate.LogicGateDescriptions.Description
			{
				name = BUILDINGS.PREFABS.LOGICGATEMULTIPLEXER.OUTPUT_NAME,
				active = BUILDINGS.PREFABS.LOGICGATEMULTIPLEXER.OUTPUT_ACTIVE,
				inactive = BUILDINGS.PREFABS.LOGICGATEMULTIPLEXER.OUTPUT_INACTIVE
			}
		};
	}

	// Token: 0x06000E2D RID: 3629 RVA: 0x00053165 File Offset: 0x00051365
	public override BuildingDef CreateBuildingDef()
	{
		return base.CreateBuildingDef("LogicGateMultiplexer", "logic_multiplexer_kanim", 3, 4);
	}

	// Token: 0x0400095B RID: 2395
	public const string ID = "LogicGateMultiplexer";
}
