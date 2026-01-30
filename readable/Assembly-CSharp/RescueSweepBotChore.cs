using System;
using STRINGS;
using UnityEngine;

// Token: 0x020004BC RID: 1212
public class RescueSweepBotChore : Chore<RescueSweepBotChore.StatesInstance>
{
	// Token: 0x0600199A RID: 6554 RVA: 0x0008F194 File Offset: 0x0008D394
	public RescueSweepBotChore(IStateMachineTarget master, GameObject sweepBot, GameObject baseStation) : base(Db.Get().ChoreTypes.RescueIncapacitated, master, null, false, null, null, null, PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new RescueSweepBotChore.StatesInstance(this);
		this.runUntilComplete = true;
		this.AddPrecondition(RescueSweepBotChore.CanReachIncapacitated, sweepBot.GetComponent<Storage>());
		this.AddPrecondition(RescueSweepBotChore.CanReachBaseStation, baseStation.GetComponent<Storage>());
	}

	// Token: 0x0600199B RID: 6555 RVA: 0x0008F1FC File Offset: 0x0008D3FC
	public override void Begin(Chore.Precondition.Context context)
	{
		base.smi.sm.rescuer.Set(context.consumerState.gameObject, base.smi, false);
		base.smi.sm.rescueTarget.Set(this.gameObject, base.smi, false);
		base.smi.sm.deliverTarget.Set(this.gameObject.GetSMI<SweepBotTrappedStates.Instance>().sm.GetSweepLocker(this.gameObject.GetSMI<SweepBotTrappedStates.Instance>()).gameObject, base.smi, false);
		base.Begin(context);
	}

	// Token: 0x0600199C RID: 6556 RVA: 0x0008F29D File Offset: 0x0008D49D
	protected override void End(string reason)
	{
		this.DropSweepBot();
		base.End(reason);
	}

	// Token: 0x0600199D RID: 6557 RVA: 0x0008F2AC File Offset: 0x0008D4AC
	private void DropSweepBot()
	{
		if (base.smi.sm.rescuer.Get(base.smi) != null && base.smi.sm.rescueTarget.Get(base.smi) != null)
		{
			base.smi.sm.rescuer.Get(base.smi).GetComponent<Storage>().Drop(base.smi.sm.rescueTarget.Get(base.smi), true);
		}
	}

	// Token: 0x04000EDB RID: 3803
	public static Chore.Precondition CanReachBaseStation = new Chore.Precondition
	{
		id = "CanReachBaseStation",
		description = DUPLICANTS.CHORES.PRECONDITIONS.CAN_MOVE_TO,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			if (context.consumerState.consumer == null)
			{
				return false;
			}
			KMonoBehaviour kmonoBehaviour = (KMonoBehaviour)data;
			return !(kmonoBehaviour == null) && context.consumerState.consumer.navigator.CanReach(Grid.PosToCell(kmonoBehaviour));
		}
	};

	// Token: 0x04000EDC RID: 3804
	public static Chore.Precondition CanReachIncapacitated = new Chore.Precondition
	{
		id = "CanReachIncapacitated",
		description = DUPLICANTS.CHORES.PRECONDITIONS.CAN_MOVE_TO,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			KMonoBehaviour kmonoBehaviour = (KMonoBehaviour)data;
			if (kmonoBehaviour == null)
			{
				return false;
			}
			int navigationCost = context.consumerState.navigator.GetNavigationCost(Grid.PosToCell(kmonoBehaviour.transform.GetPosition()));
			if (-1 != navigationCost)
			{
				context.cost += navigationCost;
				return true;
			}
			return false;
		}
	};

	// Token: 0x02001316 RID: 4886
	public class StatesInstance : GameStateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore, object>.GameInstance
	{
		// Token: 0x06008AD0 RID: 35536 RVA: 0x0035AE76 File Offset: 0x00359076
		public StatesInstance(RescueSweepBotChore master) : base(master)
		{
		}
	}

	// Token: 0x02001317 RID: 4887
	public class States : GameStateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore>
	{
		// Token: 0x06008AD1 RID: 35537 RVA: 0x0035AE80 File Offset: 0x00359080
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.approachSweepBot;
			this.approachSweepBot.InitializeStates(this.rescuer, this.rescueTarget, this.holding.pickup, this.failure, Grid.DefaultOffset, null);
			this.holding.Target(this.rescuer).Enter(delegate(RescueSweepBotChore.StatesInstance smi)
			{
				if (this.rescuer.Get(smi).gameObject.HasTag(GameTags.BaseMinion))
				{
					KAnimFile anim = Assets.GetAnim("anim_incapacitated_carrier_kanim");
					this.rescuer.Get(smi).GetComponent<KAnimControllerBase>().RemoveAnimOverrides(anim);
					this.rescuer.Get(smi).GetComponent<KAnimControllerBase>().AddAnimOverrides(anim, 0f);
				}
			}).Exit(delegate(RescueSweepBotChore.StatesInstance smi)
			{
				if (this.rescuer.Get(smi).gameObject.HasTag(GameTags.BaseMinion))
				{
					KAnimFile anim = Assets.GetAnim("anim_incapacitated_carrier_kanim");
					this.rescuer.Get(smi).GetComponent<KAnimControllerBase>().RemoveAnimOverrides(anim);
				}
			});
			this.holding.pickup.Target(this.rescuer).PlayAnim("pickup").Enter(delegate(RescueSweepBotChore.StatesInstance smi)
			{
			}).Exit(delegate(RescueSweepBotChore.StatesInstance smi)
			{
				this.rescuer.Get(smi).GetComponent<Storage>().Store(this.rescueTarget.Get(smi), false, false, true, false);
				this.rescueTarget.Get(smi).transform.SetLocalPosition(Vector3.zero);
				KBatchedAnimTracker component = this.rescueTarget.Get(smi).GetComponent<KBatchedAnimTracker>();
				if (component != null)
				{
					component.symbol = new HashedString("snapTo_pivot");
					component.offset = new Vector3(0f, 0f, 1f);
				}
			}).EventTransition(GameHashes.AnimQueueComplete, this.holding.delivering, null);
			this.holding.delivering.InitializeStates(this.rescuer, this.deliverTarget, this.holding.deposit, this.holding.ditch, null, null).Update(delegate(RescueSweepBotChore.StatesInstance smi, float dt)
			{
				if (this.deliverTarget.Get(smi) == null)
				{
					smi.GoTo(this.holding.ditch);
				}
			}, UpdateRate.SIM_200ms, false);
			this.holding.deposit.PlayAnim("place").EventHandler(GameHashes.AnimQueueComplete, delegate(RescueSweepBotChore.StatesInstance smi)
			{
				smi.master.DropSweepBot();
				smi.SetStatus(StateMachine.Status.Success);
				smi.StopSM("complete");
			});
			this.holding.ditch.PlayAnim("place").ScheduleGoTo(0.5f, this.failure).Exit(delegate(RescueSweepBotChore.StatesInstance smi)
			{
				smi.master.DropSweepBot();
			});
			this.failure.Enter(delegate(RescueSweepBotChore.StatesInstance smi)
			{
				smi.SetStatus(StateMachine.Status.Failed);
				smi.StopSM("failed");
			});
		}

		// Token: 0x04006A30 RID: 27184
		public GameStateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore, object>.ApproachSubState<Storage> approachSweepBot;

		// Token: 0x04006A31 RID: 27185
		public GameStateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore, object>.State failure;

		// Token: 0x04006A32 RID: 27186
		public RescueSweepBotChore.States.HoldingSweepBot holding;

		// Token: 0x04006A33 RID: 27187
		public StateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore, object>.TargetParameter rescueTarget;

		// Token: 0x04006A34 RID: 27188
		public StateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore, object>.TargetParameter deliverTarget;

		// Token: 0x04006A35 RID: 27189
		public StateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore, object>.TargetParameter rescuer;

		// Token: 0x020027E0 RID: 10208
		public class HoldingSweepBot : GameStateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore, object>.State
		{
			// Token: 0x0400B0BA RID: 45242
			public GameStateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore, object>.State pickup;

			// Token: 0x0400B0BB RID: 45243
			public GameStateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore, object>.ApproachSubState<IApproachable> delivering;

			// Token: 0x0400B0BC RID: 45244
			public GameStateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore, object>.State deposit;

			// Token: 0x0400B0BD RID: 45245
			public GameStateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore, object>.State ditch;
		}
	}
}
