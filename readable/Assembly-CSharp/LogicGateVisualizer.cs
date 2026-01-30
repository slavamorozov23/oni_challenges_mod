using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000798 RID: 1944
[SkipSaveFileSerialization]
public class LogicGateVisualizer : LogicGateBase
{
	// Token: 0x0600323F RID: 12863 RVA: 0x00122042 File Offset: 0x00120242
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Register();
	}

	// Token: 0x06003240 RID: 12864 RVA: 0x00122050 File Offset: 0x00120250
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.Unregister();
	}

	// Token: 0x06003241 RID: 12865 RVA: 0x00122060 File Offset: 0x00120260
	private void Register()
	{
		this.Unregister();
		this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.OutputCellOne, false));
		if (base.RequiresFourOutputs)
		{
			this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.OutputCellTwo, false));
			this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.OutputCellThree, false));
			this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.OutputCellFour, false));
		}
		this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.InputCellOne, true));
		if (base.RequiresTwoInputs)
		{
			this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.InputCellTwo, true));
		}
		else if (base.RequiresFourInputs)
		{
			this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.InputCellTwo, true));
			this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.InputCellThree, true));
			this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.InputCellFour, true));
		}
		if (base.RequiresControlInputs)
		{
			this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.ControlCellOne, true));
			this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.ControlCellTwo, true));
		}
		LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
		foreach (LogicGateVisualizer.IOVisualizer elem in this.visChildren)
		{
			logicCircuitManager.AddVisElem(elem);
		}
	}

	// Token: 0x06003242 RID: 12866 RVA: 0x001221E4 File Offset: 0x001203E4
	private void Unregister()
	{
		LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
		foreach (LogicGateVisualizer.IOVisualizer elem in this.visChildren)
		{
			logicCircuitManager.RemoveVisElem(elem);
		}
		this.visChildren.Clear();
	}

	// Token: 0x04001E8C RID: 7820
	private List<LogicGateVisualizer.IOVisualizer> visChildren = new List<LogicGateVisualizer.IOVisualizer>();

	// Token: 0x020016BE RID: 5822
	private class IOVisualizer : ILogicUIElement, IUniformGridObject
	{
		// Token: 0x06009856 RID: 38998 RVA: 0x0038804F File Offset: 0x0038624F
		public IOVisualizer(int cell, bool input)
		{
			this.cell = cell;
			this.input = input;
		}

		// Token: 0x06009857 RID: 38999 RVA: 0x00388065 File Offset: 0x00386265
		public int GetLogicUICell()
		{
			return this.cell;
		}

		// Token: 0x06009858 RID: 39000 RVA: 0x0038806D File Offset: 0x0038626D
		public LogicPortSpriteType GetLogicPortSpriteType()
		{
			if (!this.input)
			{
				return LogicPortSpriteType.Output;
			}
			return LogicPortSpriteType.Input;
		}

		// Token: 0x06009859 RID: 39001 RVA: 0x0038807A File Offset: 0x0038627A
		public Vector2 PosMin()
		{
			return Grid.CellToPos2D(this.cell);
		}

		// Token: 0x0600985A RID: 39002 RVA: 0x0038808C File Offset: 0x0038628C
		public Vector2 PosMax()
		{
			return this.PosMin();
		}

		// Token: 0x040075CB RID: 30155
		private int cell;

		// Token: 0x040075CC RID: 30156
		private bool input;
	}
}
