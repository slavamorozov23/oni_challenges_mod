using System;
using System.Collections.Generic;

// Token: 0x02000672 RID: 1650
public class WarmthProvider : GameStateMachine<WarmthProvider, WarmthProvider.Instance, IStateMachineTarget, WarmthProvider.Def>
{
	// Token: 0x0600280A RID: 10250 RVA: 0x000E68BA File Offset: 0x000E4ABA
	public static bool IsWarmCell(int cell)
	{
		return WarmthProvider.WarmCells.ContainsKey(cell) && WarmthProvider.WarmCells[cell] > 0;
	}

	// Token: 0x0600280B RID: 10251 RVA: 0x000E68D9 File Offset: 0x000E4AD9
	public static int GetWarmthValue(int cell)
	{
		if (!WarmthProvider.WarmCells.ContainsKey(cell))
		{
			return -1;
		}
		return (int)WarmthProvider.WarmCells[cell];
	}

	// Token: 0x0600280C RID: 10252 RVA: 0x000E68F8 File Offset: 0x000E4AF8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.off;
		this.off.EventTransition(GameHashes.ActiveChanged, this.on, (WarmthProvider.Instance smi) => smi.GetComponent<Operational>().IsActive).Enter(new StateMachine<WarmthProvider, WarmthProvider.Instance, IStateMachineTarget, WarmthProvider.Def>.State.Callback(WarmthProvider.RemoveWarmCells));
		this.on.EventTransition(GameHashes.ActiveChanged, this.off, (WarmthProvider.Instance smi) => !smi.GetComponent<Operational>().IsActive).TagTransition(GameTags.Operational, this.off, true).Enter(new StateMachine<WarmthProvider, WarmthProvider.Instance, IStateMachineTarget, WarmthProvider.Def>.State.Callback(WarmthProvider.AddWarmCells));
	}

	// Token: 0x0600280D RID: 10253 RVA: 0x000E69B3 File Offset: 0x000E4BB3
	private static void AddWarmCells(WarmthProvider.Instance smi)
	{
		smi.AddWarmCells();
	}

	// Token: 0x0600280E RID: 10254 RVA: 0x000E69BB File Offset: 0x000E4BBB
	private static void RemoveWarmCells(WarmthProvider.Instance smi)
	{
		smi.RemoveWarmCells();
	}

	// Token: 0x0400178B RID: 6027
	public static Dictionary<int, byte> WarmCells = new Dictionary<int, byte>();

	// Token: 0x0400178C RID: 6028
	public GameStateMachine<WarmthProvider, WarmthProvider.Instance, IStateMachineTarget, WarmthProvider.Def>.State off;

	// Token: 0x0400178D RID: 6029
	public GameStateMachine<WarmthProvider, WarmthProvider.Instance, IStateMachineTarget, WarmthProvider.Def>.State on;

	// Token: 0x02001540 RID: 5440
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04007146 RID: 28998
		public Vector2I OriginOffset;

		// Token: 0x04007147 RID: 28999
		public Vector2I RangeMin;

		// Token: 0x04007148 RID: 29000
		public Vector2I RangeMax;

		// Token: 0x04007149 RID: 29001
		public Func<int, bool> blockingCellCallback = new Func<int, bool>(Grid.IsSolidCell);
	}

	// Token: 0x02001541 RID: 5441
	public new class Instance : GameStateMachine<WarmthProvider, WarmthProvider.Instance, IStateMachineTarget, WarmthProvider.Def>.GameInstance
	{
		// Token: 0x17000A09 RID: 2569
		// (get) Token: 0x060092B7 RID: 37559 RVA: 0x00374390 File Offset: 0x00372590
		public bool IsWarming
		{
			get
			{
				return base.IsInsideState(base.sm.on);
			}
		}

		// Token: 0x060092B8 RID: 37560 RVA: 0x003743A3 File Offset: 0x003725A3
		public Instance(IStateMachineTarget master, WarmthProvider.Def def) : base(master, def)
		{
		}

		// Token: 0x060092B9 RID: 37561 RVA: 0x003743B0 File Offset: 0x003725B0
		public override void StartSM()
		{
			EntityCellVisualizer component = base.GetComponent<EntityCellVisualizer>();
			if (component != null)
			{
				component.AddPort(EntityCellVisualizer.Ports.HeatSource, default(CellOffset));
			}
			this.WorldID = base.gameObject.GetMyWorldId();
			this.SetupRange();
			this.CreateCellListeners();
			base.StartSM();
		}

		// Token: 0x060092BA RID: 37562 RVA: 0x00374404 File Offset: 0x00372604
		private void SetupRange()
		{
			Vector2I u = Grid.PosToXY(base.transform.GetPosition());
			Vector2I vector2I = base.def.OriginOffset;
			this.range_min = base.def.RangeMin;
			this.range_max = base.def.RangeMax;
			Rotatable rotatable;
			if (base.gameObject.TryGetComponent<Rotatable>(out rotatable))
			{
				vector2I = rotatable.GetRotatedOffset(vector2I);
				Vector2I rotatedOffset = rotatable.GetRotatedOffset(this.range_min);
				Vector2I rotatedOffset2 = rotatable.GetRotatedOffset(this.range_max);
				this.range_min.x = ((rotatedOffset.x < rotatedOffset2.x) ? rotatedOffset.x : rotatedOffset2.x);
				this.range_min.y = ((rotatedOffset.y < rotatedOffset2.y) ? rotatedOffset.y : rotatedOffset2.y);
				this.range_max.x = ((rotatedOffset.x > rotatedOffset2.x) ? rotatedOffset.x : rotatedOffset2.x);
				this.range_max.y = ((rotatedOffset.y > rotatedOffset2.y) ? rotatedOffset.y : rotatedOffset2.y);
			}
			this.origin = u + vector2I;
		}

		// Token: 0x060092BB RID: 37563 RVA: 0x00374538 File Offset: 0x00372738
		public bool ContainsCell(int cell)
		{
			if (this.cellsInRange == null)
			{
				return false;
			}
			for (int i = 0; i < this.cellsInRange.Length; i++)
			{
				if (this.cellsInRange[i] == cell)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060092BC RID: 37564 RVA: 0x00374570 File Offset: 0x00372770
		private void UnmarkAllCellsInRange()
		{
			if (this.cellsInRange != null)
			{
				for (int i = 0; i < this.cellsInRange.Length; i++)
				{
					int num = this.cellsInRange[i];
					if (WarmthProvider.WarmCells.ContainsKey(num))
					{
						Dictionary<int, byte> warmCells = WarmthProvider.WarmCells;
						int key = num;
						byte b = warmCells[key];
						warmCells[key] = b - 1;
					}
				}
			}
			this.cellsInRange = null;
		}

		// Token: 0x060092BD RID: 37565 RVA: 0x003745D0 File Offset: 0x003727D0
		private void UpdateCellsInRange()
		{
			this.UnmarkAllCellsInRange();
			Grid.PosToCell(this);
			List<int> list = new List<int>();
			for (int i = 0; i <= this.range_max.y - this.range_min.y; i++)
			{
				int y = this.origin.y + this.range_min.y + i;
				for (int j = 0; j <= this.range_max.x - this.range_min.x; j++)
				{
					int num = Grid.XYToCell(this.origin.x + this.range_min.x + j, y);
					if (Grid.IsValidCellInWorld(num, this.WorldID) && this.IsCellVisible(num))
					{
						list.Add(num);
						if (!WarmthProvider.WarmCells.ContainsKey(num))
						{
							WarmthProvider.WarmCells.Add(num, 0);
						}
						Dictionary<int, byte> warmCells = WarmthProvider.WarmCells;
						int key = num;
						byte b = warmCells[key];
						warmCells[key] = b + 1;
					}
				}
			}
			this.cellsInRange = list.ToArray();
		}

		// Token: 0x060092BE RID: 37566 RVA: 0x003746E2 File Offset: 0x003728E2
		public void AddWarmCells()
		{
			this.UpdateCellsInRange();
		}

		// Token: 0x060092BF RID: 37567 RVA: 0x003746EA File Offset: 0x003728EA
		public void RemoveWarmCells()
		{
			this.UnmarkAllCellsInRange();
		}

		// Token: 0x060092C0 RID: 37568 RVA: 0x003746F2 File Offset: 0x003728F2
		protected override void OnCleanUp()
		{
			this.RemoveWarmCells();
			this.ClearCellListeners();
			base.OnCleanUp();
		}

		// Token: 0x060092C1 RID: 37569 RVA: 0x00374708 File Offset: 0x00372908
		public bool IsCellVisible(int cell)
		{
			Vector2I vector2I = Grid.CellToXY(Grid.PosToCell(this));
			Vector2I vector2I2 = Grid.CellToXY(cell);
			return Grid.TestLineOfSight(vector2I.x, vector2I.y, vector2I2.x, vector2I2.y, base.def.blockingCellCallback, false, false);
		}

		// Token: 0x060092C2 RID: 37570 RVA: 0x00374752 File Offset: 0x00372952
		public void OnSolidCellChanged(object obj)
		{
			if (this.IsWarming)
			{
				this.UpdateCellsInRange();
			}
		}

		// Token: 0x060092C3 RID: 37571 RVA: 0x00374764 File Offset: 0x00372964
		private void CreateCellListeners()
		{
			Grid.PosToCell(this);
			List<HandleVector<int>.Handle> list = new List<HandleVector<int>.Handle>();
			for (int i = 0; i <= this.range_max.y - this.range_min.y; i++)
			{
				int y = this.origin.y + this.range_min.y + i;
				for (int j = 0; j <= this.range_max.x - this.range_min.x; j++)
				{
					int cell = Grid.XYToCell(this.origin.x + this.range_min.x + j, y);
					if (Grid.IsValidCellInWorld(cell, this.WorldID))
					{
						list.Add(GameScenePartitioner.Instance.Add("WarmthProvider Visibility", base.gameObject, cell, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnSolidCellChanged)));
					}
				}
			}
			this.partitionEntries = list.ToArray();
		}

		// Token: 0x060092C4 RID: 37572 RVA: 0x00374854 File Offset: 0x00372A54
		private void ClearCellListeners()
		{
			if (this.partitionEntries != null)
			{
				for (int i = 0; i < this.partitionEntries.Length; i++)
				{
					HandleVector<int>.Handle handle = this.partitionEntries[i];
					GameScenePartitioner.Instance.Free(ref handle);
				}
			}
		}

		// Token: 0x0400714A RID: 29002
		public int WorldID;

		// Token: 0x0400714B RID: 29003
		private int[] cellsInRange;

		// Token: 0x0400714C RID: 29004
		private HandleVector<int>.Handle[] partitionEntries;

		// Token: 0x0400714D RID: 29005
		public Vector2I range_min;

		// Token: 0x0400714E RID: 29006
		public Vector2I range_max;

		// Token: 0x0400714F RID: 29007
		public Vector2I origin;
	}
}
