using System;

// Token: 0x02000D67 RID: 3431
public class Lure : GameStateMachine<Lure, Lure.Instance, IStateMachineTarget, Lure.Def>
{
	// Token: 0x06006A46 RID: 27206 RVA: 0x00282E70 File Offset: 0x00281070
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.off;
		this.off.DoNothing();
		this.on.Enter(new StateMachine<Lure, Lure.Instance, IStateMachineTarget, Lure.Def>.State.Callback(this.AddToScenePartitioner)).Exit(new StateMachine<Lure, Lure.Instance, IStateMachineTarget, Lure.Def>.State.Callback(this.RemoveFromScenePartitioner));
	}

	// Token: 0x06006A47 RID: 27207 RVA: 0x00282EC4 File Offset: 0x002810C4
	private void AddToScenePartitioner(Lure.Instance smi)
	{
		Extents extents = new Extents(smi.cell, smi.def.radius);
		smi.partitionerEntry = GameScenePartitioner.Instance.Add(this.name, smi, extents, GameScenePartitioner.Instance.lure, null);
	}

	// Token: 0x06006A48 RID: 27208 RVA: 0x00282F0C File Offset: 0x0028110C
	private void RemoveFromScenePartitioner(Lure.Instance smi)
	{
		GameScenePartitioner.Instance.Free(ref smi.partitionerEntry);
	}

	// Token: 0x04004918 RID: 18712
	public GameStateMachine<Lure, Lure.Instance, IStateMachineTarget, Lure.Def>.State off;

	// Token: 0x04004919 RID: 18713
	public GameStateMachine<Lure, Lure.Instance, IStateMachineTarget, Lure.Def>.State on;

	// Token: 0x02001F98 RID: 8088
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04009351 RID: 37713
		public CellOffset[] defaultLurePoints = new CellOffset[1];

		// Token: 0x04009352 RID: 37714
		public int radius = 50;

		// Token: 0x04009353 RID: 37715
		public Tag[] initialLures;
	}

	// Token: 0x02001F99 RID: 8089
	public new class Instance : GameStateMachine<Lure, Lure.Instance, IStateMachineTarget, Lure.Def>.GameInstance
	{
		// Token: 0x17000CE2 RID: 3298
		// (get) Token: 0x0600B6D0 RID: 46800 RVA: 0x003F18DC File Offset: 0x003EFADC
		public int cell
		{
			get
			{
				if (this._cell == -1)
				{
					this._cell = Grid.PosToCell(base.transform.GetPosition());
				}
				return this._cell;
			}
		}

		// Token: 0x17000CE3 RID: 3299
		// (get) Token: 0x0600B6D1 RID: 46801 RVA: 0x003F1903 File Offset: 0x003EFB03
		// (set) Token: 0x0600B6D2 RID: 46802 RVA: 0x003F191F File Offset: 0x003EFB1F
		public CellOffset[] LurePoints
		{
			get
			{
				if (this._lurePoints == null)
				{
					return base.def.defaultLurePoints;
				}
				return this._lurePoints;
			}
			set
			{
				this._lurePoints = value;
			}
		}

		// Token: 0x0600B6D3 RID: 46803 RVA: 0x003F1928 File Offset: 0x003EFB28
		public Instance(IStateMachineTarget master, Lure.Def def) : base(master, def)
		{
		}

		// Token: 0x0600B6D4 RID: 46804 RVA: 0x003F1939 File Offset: 0x003EFB39
		public override void StartSM()
		{
			base.StartSM();
			if (base.def.initialLures != null)
			{
				this.SetActiveLures(base.def.initialLures);
			}
		}

		// Token: 0x0600B6D5 RID: 46805 RVA: 0x003F1960 File Offset: 0x003EFB60
		public void ChangeLureCellPosition(int newCell)
		{
			bool flag = base.IsInsideState(base.sm.on);
			if (flag)
			{
				this.GoTo(base.sm.off);
			}
			this.LurePoints = new CellOffset[]
			{
				Grid.GetOffset(Grid.PosToCell(base.smi.transform.GetPosition()), newCell)
			};
			this._cell = newCell;
			if (flag)
			{
				this.GoTo(base.sm.on);
			}
		}

		// Token: 0x0600B6D6 RID: 46806 RVA: 0x003F19DC File Offset: 0x003EFBDC
		public void SetActiveLures(Tag[] lures)
		{
			this.lures = lures;
			if (lures == null || lures.Length == 0)
			{
				this.GoTo(base.sm.off);
				return;
			}
			this.GoTo(base.sm.on);
		}

		// Token: 0x0600B6D7 RID: 46807 RVA: 0x003F1A0F File Offset: 0x003EFC0F
		public bool IsActive()
		{
			return this.GetCurrentState() == base.sm.on;
		}

		// Token: 0x0600B6D8 RID: 46808 RVA: 0x003F1A24 File Offset: 0x003EFC24
		public bool HasAnyLure(Tag[] creature_lures)
		{
			if (this.lures == null || creature_lures == null)
			{
				return false;
			}
			foreach (Tag a in creature_lures)
			{
				foreach (Tag b in this.lures)
				{
					if (a == b)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04009354 RID: 37716
		private int _cell = -1;

		// Token: 0x04009355 RID: 37717
		private Tag[] lures;

		// Token: 0x04009356 RID: 37718
		public HandleVector<int>.Handle partitionerEntry;

		// Token: 0x04009357 RID: 37719
		private CellOffset[] _lurePoints;
	}
}
