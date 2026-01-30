using System;
using STRINGS;

// Token: 0x020002BA RID: 698
public class LogicGateDemultiplexerConfig : LogicGateBaseConfig
{
	// Token: 0x06000E2F RID: 3631 RVA: 0x00053181 File Offset: 0x00051381
	protected override LogicGateBase.Op GetLogicOp()
	{
		return LogicGateBase.Op.Demultiplexer;
	}

	// Token: 0x1700003B RID: 59
	// (get) Token: 0x06000E30 RID: 3632 RVA: 0x00053184 File Offset: 0x00051384
	protected override CellOffset[] InputPortOffsets
	{
		get
		{
			return new CellOffset[]
			{
				new CellOffset(-1, 3)
			};
		}
	}

	// Token: 0x1700003C RID: 60
	// (get) Token: 0x06000E31 RID: 3633 RVA: 0x0005319A File Offset: 0x0005139A
	protected override CellOffset[] OutputPortOffsets
	{
		get
		{
			return new CellOffset[]
			{
				new CellOffset(1, 3),
				new CellOffset(1, 2),
				new CellOffset(1, 1),
				new CellOffset(1, 0)
			};
		}
	}

	// Token: 0x1700003D RID: 61
	// (get) Token: 0x06000E32 RID: 3634 RVA: 0x000531DA File Offset: 0x000513DA
	protected override CellOffset[] ControlPortOffsets
	{
		get
		{
			return new CellOffset[]
			{
				new CellOffset(-1, 0),
				new CellOffset(0, 0)
			};
		}
	}

	// Token: 0x06000E33 RID: 3635 RVA: 0x00053200 File Offset: 0x00051400
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

	// Token: 0x06000E34 RID: 3636 RVA: 0x0005324D File Offset: 0x0005144D
	public override BuildingDef CreateBuildingDef()
	{
		return base.CreateBuildingDef("LogicGateDemultiplexer", "logic_demultiplexer_kanim", 3, 4);
	}

	// Token: 0x0400095C RID: 2396
	public const string ID = "LogicGateDemultiplexer";
}
