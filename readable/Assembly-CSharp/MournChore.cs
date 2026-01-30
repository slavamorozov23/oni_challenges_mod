using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020004AD RID: 1197
public class MournChore : Chore<MournChore.StatesInstance>
{
	// Token: 0x0600196C RID: 6508 RVA: 0x0008DEEC File Offset: 0x0008C0EC
	private static int GetStandableCell(int cell, Navigator navigator)
	{
		foreach (CellOffset offset in MournChore.ValidStandingOffsets)
		{
			if (Grid.IsCellOffsetValid(cell, offset))
			{
				int num = Grid.OffsetCell(cell, offset);
				if (!Grid.Reserved[num] && navigator.NavGrid.NavTable.IsValid(num, NavType.Floor) && navigator.GetNavigationCost(num) != -1)
				{
					return num;
				}
			}
		}
		return -1;
	}

	// Token: 0x0600196D RID: 6509 RVA: 0x0008DF54 File Offset: 0x0008C154
	private static KAnimFile GetAnimFileName(MournChore.StatesInstance smi)
	{
		string s = "anim_react_mourning_kanim";
		GameObject gameObject = smi.sm.mourner.Get(smi);
		if (gameObject == null)
		{
			return Assets.GetAnim(s);
		}
		MinionIdentity component = gameObject.GetComponent<MinionIdentity>();
		if (component == null)
		{
			return Assets.GetAnim(s);
		}
		if (component.model == BionicMinionConfig.MODEL)
		{
			return Assets.GetAnim("anim_bionic_react_mourning_kanim");
		}
		return Assets.GetAnim(s);
	}

	// Token: 0x0600196E RID: 6510 RVA: 0x0008DFD8 File Offset: 0x0008C1D8
	public MournChore(IStateMachineTarget master) : base(Db.Get().ChoreTypes.Mourn, master, master.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.high, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new MournChore.StatesInstance(this);
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		this.AddPrecondition(ChorePreconditions.instance.NoDeadBodies, null);
		this.AddPrecondition(MournChore.HasValidMournLocation, master);
	}

	// Token: 0x0600196F RID: 6511 RVA: 0x0008E048 File Offset: 0x0008C248
	public static Grave FindGraveToMournAt()
	{
		Grave result = null;
		float num = -1f;
		foreach (object obj in Components.Graves)
		{
			Grave grave = (Grave)obj;
			if (grave.burialTime > num)
			{
				num = grave.burialTime;
				result = grave;
			}
		}
		return result;
	}

	// Token: 0x06001970 RID: 6512 RVA: 0x0008E0B8 File Offset: 0x0008C2B8
	public override void Begin(Chore.Precondition.Context context)
	{
		if (context.consumerState.consumer == null)
		{
			global::Debug.LogError("MournChore null context.consumer");
			return;
		}
		if (base.smi == null)
		{
			global::Debug.LogError("MournChore null smi");
			return;
		}
		if (base.smi.sm == null)
		{
			global::Debug.LogError("MournChore null smi.sm");
			return;
		}
		if (MournChore.FindGraveToMournAt() == null)
		{
			global::Debug.LogError("MournChore no grave");
			return;
		}
		base.smi.sm.mourner.Set(context.consumerState.gameObject, base.smi, false);
		base.Begin(context);
	}

	// Token: 0x04000ECE RID: 3790
	private static readonly CellOffset[] ValidStandingOffsets = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(-1, 0),
		new CellOffset(1, 0)
	};

	// Token: 0x04000ECF RID: 3791
	private static readonly Chore.Precondition HasValidMournLocation = new Chore.Precondition
	{
		id = "HasPlaceToStand",
		description = DUPLICANTS.CHORES.PRECONDITIONS.HAS_PLACE_TO_STAND,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			Navigator component = ((IStateMachineTarget)data).GetComponent<Navigator>();
			bool result = false;
			Grave grave = MournChore.FindGraveToMournAt();
			if (grave != null && Grid.IsValidCell(MournChore.GetStandableCell(Grid.PosToCell(grave), component)))
			{
				result = true;
			}
			return result;
		}
	};

	// Token: 0x020012F0 RID: 4848
	public class StatesInstance : GameStateMachine<MournChore.States, MournChore.StatesInstance, MournChore, object>.GameInstance
	{
		// Token: 0x06008A2D RID: 35373 RVA: 0x00357C77 File Offset: 0x00355E77
		public StatesInstance(MournChore master) : base(master)
		{
		}

		// Token: 0x06008A2E RID: 35374 RVA: 0x00357C88 File Offset: 0x00355E88
		public void CreateLocator()
		{
			int cell = Grid.PosToCell(MournChore.FindGraveToMournAt().transform.GetPosition());
			Navigator component = base.master.GetComponent<Navigator>();
			int standableCell = MournChore.GetStandableCell(cell, component);
			if (standableCell < 0)
			{
				base.smi.GoTo(null);
				return;
			}
			Grid.Reserved[standableCell] = true;
			Vector3 pos = Grid.CellToPosCBC(standableCell, Grid.SceneLayer.Move);
			GameObject value = ChoreHelpers.CreateLocator("MournLocator", pos);
			base.smi.sm.locator.Set(value, base.smi, false);
			this.locatorCell = standableCell;
			base.smi.GoTo(base.sm.moveto);
		}

		// Token: 0x06008A2F RID: 35375 RVA: 0x00357D2C File Offset: 0x00355F2C
		public void DestroyLocator()
		{
			if (this.locatorCell >= 0)
			{
				Grid.Reserved[this.locatorCell] = false;
				ChoreHelpers.DestroyLocator(base.sm.locator.Get(this));
				base.sm.locator.Set(null, this);
				this.locatorCell = -1;
			}
		}

		// Token: 0x040069B5 RID: 27061
		private int locatorCell = -1;
	}

	// Token: 0x020012F1 RID: 4849
	public class States : GameStateMachine<MournChore.States, MournChore.StatesInstance, MournChore>
	{
		// Token: 0x06008A30 RID: 35376 RVA: 0x00357D84 File Offset: 0x00355F84
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.findOffset;
			base.Target(this.mourner);
			this.root.ToggleAnims(new Func<MournChore.StatesInstance, KAnimFile>(MournChore.GetAnimFileName)).Exit("DestroyLocator", delegate(MournChore.StatesInstance smi)
			{
				smi.DestroyLocator();
			});
			this.findOffset.Enter("CreateLocator", delegate(MournChore.StatesInstance smi)
			{
				smi.CreateLocator();
			});
			this.moveto.InitializeStates(this.mourner, this.locator, this.mourn, null, null, null);
			this.mourn.PlayAnims((MournChore.StatesInstance smi) => MournChore.States.WORK_ANIMS, KAnim.PlayMode.Loop).ScheduleGoTo(10f, this.completed);
			this.completed.PlayAnim("working_pst").OnAnimQueueComplete(null).Exit(delegate(MournChore.StatesInstance smi)
			{
				this.mourner.Get<Effects>(smi).Remove(Db.Get().effects.Get("Mourning"));
			});
		}

		// Token: 0x040069B6 RID: 27062
		public StateMachine<MournChore.States, MournChore.StatesInstance, MournChore, object>.TargetParameter mourner;

		// Token: 0x040069B7 RID: 27063
		public StateMachine<MournChore.States, MournChore.StatesInstance, MournChore, object>.TargetParameter locator;

		// Token: 0x040069B8 RID: 27064
		public GameStateMachine<MournChore.States, MournChore.StatesInstance, MournChore, object>.State findOffset;

		// Token: 0x040069B9 RID: 27065
		public GameStateMachine<MournChore.States, MournChore.StatesInstance, MournChore, object>.ApproachSubState<IApproachable> moveto;

		// Token: 0x040069BA RID: 27066
		public GameStateMachine<MournChore.States, MournChore.StatesInstance, MournChore, object>.State mourn;

		// Token: 0x040069BB RID: 27067
		public GameStateMachine<MournChore.States, MournChore.StatesInstance, MournChore, object>.State completed;

		// Token: 0x040069BC RID: 27068
		private static readonly HashedString[] WORK_ANIMS = new HashedString[]
		{
			"working_pre",
			"working_loop"
		};
	}
}
