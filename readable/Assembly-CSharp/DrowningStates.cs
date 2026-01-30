using System;
using STRINGS;

// Token: 0x020000F2 RID: 242
public class DrowningStates : GameStateMachine<DrowningStates, DrowningStates.Instance, IStateMachineTarget, DrowningStates.Def>
{
	// Token: 0x06000473 RID: 1139 RVA: 0x00024814 File Offset: 0x00022A14
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.drown;
		GameStateMachine<DrowningStates, DrowningStates.Instance, IStateMachineTarget, DrowningStates.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.DROWNING.NAME;
		string tooltip = CREATURES.STATUSITEMS.DROWNING.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).TagTransition(GameTags.Creatures.Drowning, null, true).ToggleCritterEmotion(Db.Get().CritterEmotions.Suffocating, null);
		this.drown.PlayAnim("drown_pre").QueueAnim("drown_loop", true, null).Transition(this.drown_pst, new StateMachine<DrowningStates, DrowningStates.Instance, IStateMachineTarget, DrowningStates.Def>.Transition.ConditionCallback(this.UpdateSafeCell), UpdateRate.SIM_1000ms);
		this.drown_pst.PlayAnim("drown_pst").OnAnimQueueComplete(this.move_to_safe);
		this.move_to_safe.MoveTo((DrowningStates.Instance smi) => smi.safeCell, null, null, false);
	}

	// Token: 0x06000474 RID: 1140 RVA: 0x00024914 File Offset: 0x00022B14
	public bool UpdateSafeCell(DrowningStates.Instance smi)
	{
		Navigator component = smi.GetComponent<Navigator>();
		DrowningStates.EscapeCellQuery escapeCellQuery = new DrowningStates.EscapeCellQuery(smi.GetComponent<DrowningMonitor>());
		component.RunQuery(escapeCellQuery);
		smi.safeCell = escapeCellQuery.GetResultCell();
		return smi.safeCell != Grid.InvalidCell;
	}

	// Token: 0x0400034C RID: 844
	public GameStateMachine<DrowningStates, DrowningStates.Instance, IStateMachineTarget, DrowningStates.Def>.State drown;

	// Token: 0x0400034D RID: 845
	public GameStateMachine<DrowningStates, DrowningStates.Instance, IStateMachineTarget, DrowningStates.Def>.State drown_pst;

	// Token: 0x0400034E RID: 846
	public GameStateMachine<DrowningStates, DrowningStates.Instance, IStateMachineTarget, DrowningStates.Def>.State move_to_safe;

	// Token: 0x02001133 RID: 4403
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001134 RID: 4404
	public new class Instance : GameStateMachine<DrowningStates, DrowningStates.Instance, IStateMachineTarget, DrowningStates.Def>.GameInstance
	{
		// Token: 0x060083F7 RID: 33783 RVA: 0x00344425 File Offset: 0x00342625
		public Instance(Chore<DrowningStates.Instance> chore, DrowningStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.HasTag, GameTags.Creatures.Drowning);
		}

		// Token: 0x04006431 RID: 25649
		public int safeCell = Grid.InvalidCell;
	}

	// Token: 0x02001135 RID: 4405
	public class EscapeCellQuery : PathFinderQuery
	{
		// Token: 0x060083F8 RID: 33784 RVA: 0x00344454 File Offset: 0x00342654
		public EscapeCellQuery(DrowningMonitor monitor)
		{
			this.monitor = monitor;
		}

		// Token: 0x060083F9 RID: 33785 RVA: 0x00344463 File Offset: 0x00342663
		public override bool IsMatch(int cell, int parent_cell, int cost)
		{
			return this.monitor.IsCellSafe(cell);
		}

		// Token: 0x04006432 RID: 25650
		private DrowningMonitor monitor;
	}
}
