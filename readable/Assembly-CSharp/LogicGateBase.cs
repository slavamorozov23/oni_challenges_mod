using System;
using UnityEngine;

// Token: 0x02000796 RID: 1942
[AddComponentMenu("KMonoBehaviour/scripts/LogicGateBase")]
public class LogicGateBase : KMonoBehaviour
{
	// Token: 0x0600320B RID: 12811 RVA: 0x0011FCD8 File Offset: 0x0011DED8
	private int GetActualCell(CellOffset offset)
	{
		Rotatable component = base.GetComponent<Rotatable>();
		if (component != null)
		{
			offset = component.GetRotatedCellOffset(offset);
		}
		return Grid.OffsetCell(Grid.PosToCell(base.transform.GetPosition()), offset);
	}

	// Token: 0x170002BA RID: 698
	// (get) Token: 0x0600320C RID: 12812 RVA: 0x0011FD14 File Offset: 0x0011DF14
	public int InputCellOne
	{
		get
		{
			return this.GetActualCell(this.inputPortOffsets[0]);
		}
	}

	// Token: 0x170002BB RID: 699
	// (get) Token: 0x0600320D RID: 12813 RVA: 0x0011FD28 File Offset: 0x0011DF28
	public int InputCellTwo
	{
		get
		{
			return this.GetActualCell(this.inputPortOffsets[1]);
		}
	}

	// Token: 0x170002BC RID: 700
	// (get) Token: 0x0600320E RID: 12814 RVA: 0x0011FD3C File Offset: 0x0011DF3C
	public int InputCellThree
	{
		get
		{
			return this.GetActualCell(this.inputPortOffsets[2]);
		}
	}

	// Token: 0x170002BD RID: 701
	// (get) Token: 0x0600320F RID: 12815 RVA: 0x0011FD50 File Offset: 0x0011DF50
	public int InputCellFour
	{
		get
		{
			return this.GetActualCell(this.inputPortOffsets[3]);
		}
	}

	// Token: 0x170002BE RID: 702
	// (get) Token: 0x06003210 RID: 12816 RVA: 0x0011FD64 File Offset: 0x0011DF64
	public int OutputCellOne
	{
		get
		{
			return this.GetActualCell(this.outputPortOffsets[0]);
		}
	}

	// Token: 0x170002BF RID: 703
	// (get) Token: 0x06003211 RID: 12817 RVA: 0x0011FD78 File Offset: 0x0011DF78
	public int OutputCellTwo
	{
		get
		{
			return this.GetActualCell(this.outputPortOffsets[1]);
		}
	}

	// Token: 0x170002C0 RID: 704
	// (get) Token: 0x06003212 RID: 12818 RVA: 0x0011FD8C File Offset: 0x0011DF8C
	public int OutputCellThree
	{
		get
		{
			return this.GetActualCell(this.outputPortOffsets[2]);
		}
	}

	// Token: 0x170002C1 RID: 705
	// (get) Token: 0x06003213 RID: 12819 RVA: 0x0011FDA0 File Offset: 0x0011DFA0
	public int OutputCellFour
	{
		get
		{
			return this.GetActualCell(this.outputPortOffsets[3]);
		}
	}

	// Token: 0x170002C2 RID: 706
	// (get) Token: 0x06003214 RID: 12820 RVA: 0x0011FDB4 File Offset: 0x0011DFB4
	public int ControlCellOne
	{
		get
		{
			return this.GetActualCell(this.controlPortOffsets[0]);
		}
	}

	// Token: 0x170002C3 RID: 707
	// (get) Token: 0x06003215 RID: 12821 RVA: 0x0011FDC8 File Offset: 0x0011DFC8
	public int ControlCellTwo
	{
		get
		{
			return this.GetActualCell(this.controlPortOffsets[1]);
		}
	}

	// Token: 0x06003216 RID: 12822 RVA: 0x0011FDDC File Offset: 0x0011DFDC
	public int PortCell(LogicGateBase.PortId port)
	{
		switch (port)
		{
		case LogicGateBase.PortId.InputOne:
			return this.InputCellOne;
		case LogicGateBase.PortId.InputTwo:
			return this.InputCellTwo;
		case LogicGateBase.PortId.InputThree:
			return this.InputCellThree;
		case LogicGateBase.PortId.InputFour:
			return this.InputCellFour;
		case LogicGateBase.PortId.OutputOne:
			return this.OutputCellOne;
		case LogicGateBase.PortId.OutputTwo:
			return this.OutputCellTwo;
		case LogicGateBase.PortId.OutputThree:
			return this.OutputCellThree;
		case LogicGateBase.PortId.OutputFour:
			return this.OutputCellFour;
		case LogicGateBase.PortId.ControlOne:
			return this.ControlCellOne;
		case LogicGateBase.PortId.ControlTwo:
			return this.ControlCellTwo;
		default:
			return this.OutputCellOne;
		}
	}

	// Token: 0x06003217 RID: 12823 RVA: 0x0011FE68 File Offset: 0x0011E068
	public bool TryGetPortAtCell(int cell, out LogicGateBase.PortId port)
	{
		if (cell == this.InputCellOne)
		{
			port = LogicGateBase.PortId.InputOne;
			return true;
		}
		if ((this.RequiresTwoInputs || this.RequiresFourInputs) && cell == this.InputCellTwo)
		{
			port = LogicGateBase.PortId.InputTwo;
			return true;
		}
		if (this.RequiresFourInputs && cell == this.InputCellThree)
		{
			port = LogicGateBase.PortId.InputThree;
			return true;
		}
		if (this.RequiresFourInputs && cell == this.InputCellFour)
		{
			port = LogicGateBase.PortId.InputFour;
			return true;
		}
		if (cell == this.OutputCellOne)
		{
			port = LogicGateBase.PortId.OutputOne;
			return true;
		}
		if (this.RequiresFourOutputs && cell == this.OutputCellTwo)
		{
			port = LogicGateBase.PortId.OutputTwo;
			return true;
		}
		if (this.RequiresFourOutputs && cell == this.OutputCellThree)
		{
			port = LogicGateBase.PortId.OutputThree;
			return true;
		}
		if (this.RequiresFourOutputs && cell == this.OutputCellFour)
		{
			port = LogicGateBase.PortId.OutputFour;
			return true;
		}
		if (this.RequiresControlInputs && cell == this.ControlCellOne)
		{
			port = LogicGateBase.PortId.ControlOne;
			return true;
		}
		if (this.RequiresControlInputs && cell == this.ControlCellTwo)
		{
			port = LogicGateBase.PortId.ControlTwo;
			return true;
		}
		port = LogicGateBase.PortId.InputOne;
		return false;
	}

	// Token: 0x170002C4 RID: 708
	// (get) Token: 0x06003218 RID: 12824 RVA: 0x0011FF4E File Offset: 0x0011E14E
	public bool RequiresTwoInputs
	{
		get
		{
			return LogicGateBase.OpRequiresTwoInputs(this.op);
		}
	}

	// Token: 0x170002C5 RID: 709
	// (get) Token: 0x06003219 RID: 12825 RVA: 0x0011FF5B File Offset: 0x0011E15B
	public bool RequiresFourInputs
	{
		get
		{
			return LogicGateBase.OpRequiresFourInputs(this.op);
		}
	}

	// Token: 0x170002C6 RID: 710
	// (get) Token: 0x0600321A RID: 12826 RVA: 0x0011FF68 File Offset: 0x0011E168
	public bool RequiresFourOutputs
	{
		get
		{
			return LogicGateBase.OpRequiresFourOutputs(this.op);
		}
	}

	// Token: 0x170002C7 RID: 711
	// (get) Token: 0x0600321B RID: 12827 RVA: 0x0011FF75 File Offset: 0x0011E175
	public bool RequiresControlInputs
	{
		get
		{
			return LogicGateBase.OpRequiresControlInputs(this.op);
		}
	}

	// Token: 0x0600321C RID: 12828 RVA: 0x0011FF82 File Offset: 0x0011E182
	public static bool OpRequiresTwoInputs(LogicGateBase.Op op)
	{
		return op != LogicGateBase.Op.Not && op - LogicGateBase.Op.CustomSingle > 2;
	}

	// Token: 0x0600321D RID: 12829 RVA: 0x0011FF91 File Offset: 0x0011E191
	public static bool OpRequiresFourInputs(LogicGateBase.Op op)
	{
		return op == LogicGateBase.Op.Multiplexer;
	}

	// Token: 0x0600321E RID: 12830 RVA: 0x0011FF9A File Offset: 0x0011E19A
	public static bool OpRequiresFourOutputs(LogicGateBase.Op op)
	{
		return op == LogicGateBase.Op.Demultiplexer;
	}

	// Token: 0x0600321F RID: 12831 RVA: 0x0011FFA3 File Offset: 0x0011E1A3
	public static bool OpRequiresControlInputs(LogicGateBase.Op op)
	{
		return op - LogicGateBase.Op.Multiplexer <= 1;
	}

	// Token: 0x04001E19 RID: 7705
	public static LogicModeUI uiSrcData;

	// Token: 0x04001E1A RID: 7706
	public static readonly HashedString OUTPUT_TWO_PORT_ID = new HashedString("LogicGateOutputTwo");

	// Token: 0x04001E1B RID: 7707
	public static readonly HashedString OUTPUT_THREE_PORT_ID = new HashedString("LogicGateOutputThree");

	// Token: 0x04001E1C RID: 7708
	public static readonly HashedString OUTPUT_FOUR_PORT_ID = new HashedString("LogicGateOutputFour");

	// Token: 0x04001E1D RID: 7709
	[SerializeField]
	public LogicGateBase.Op op;

	// Token: 0x04001E1E RID: 7710
	public static CellOffset[] portOffsets = new CellOffset[]
	{
		CellOffset.none,
		new CellOffset(0, 1),
		new CellOffset(1, 0)
	};

	// Token: 0x04001E1F RID: 7711
	public CellOffset[] inputPortOffsets;

	// Token: 0x04001E20 RID: 7712
	public CellOffset[] outputPortOffsets;

	// Token: 0x04001E21 RID: 7713
	public CellOffset[] controlPortOffsets;

	// Token: 0x020016BA RID: 5818
	public enum PortId
	{
		// Token: 0x040075AE RID: 30126
		InputOne,
		// Token: 0x040075AF RID: 30127
		InputTwo,
		// Token: 0x040075B0 RID: 30128
		InputThree,
		// Token: 0x040075B1 RID: 30129
		InputFour,
		// Token: 0x040075B2 RID: 30130
		OutputOne,
		// Token: 0x040075B3 RID: 30131
		OutputTwo,
		// Token: 0x040075B4 RID: 30132
		OutputThree,
		// Token: 0x040075B5 RID: 30133
		OutputFour,
		// Token: 0x040075B6 RID: 30134
		ControlOne,
		// Token: 0x040075B7 RID: 30135
		ControlTwo
	}

	// Token: 0x020016BB RID: 5819
	public enum Op
	{
		// Token: 0x040075B9 RID: 30137
		And,
		// Token: 0x040075BA RID: 30138
		Or,
		// Token: 0x040075BB RID: 30139
		Not,
		// Token: 0x040075BC RID: 30140
		Xor,
		// Token: 0x040075BD RID: 30141
		CustomSingle,
		// Token: 0x040075BE RID: 30142
		Multiplexer,
		// Token: 0x040075BF RID: 30143
		Demultiplexer
	}
}
