using System;
using UnityEngine;

// Token: 0x020004CB RID: 1227
public class WorkChore<WorkableType> : Chore<WorkChore<WorkableType>.StatesInstance> where WorkableType : Workable
{
	// Token: 0x17000086 RID: 134
	// (get) Token: 0x060019C7 RID: 6599 RVA: 0x000903CB File Offset: 0x0008E5CB
	// (set) Token: 0x060019C8 RID: 6600 RVA: 0x000903D3 File Offset: 0x0008E5D3
	public bool onlyWhenOperational { get; private set; }

	// Token: 0x060019C9 RID: 6601 RVA: 0x000903DC File Offset: 0x0008E5DC
	public override string ToString()
	{
		return "WorkChore<" + typeof(WorkableType).ToString() + ">";
	}

	// Token: 0x060019CA RID: 6602 RVA: 0x000903FC File Offset: 0x0008E5FC
	public WorkChore(ChoreType chore_type, IStateMachineTarget target, ChoreProvider chore_provider = null, bool run_until_complete = true, Action<Chore> on_complete = null, Action<Chore> on_begin = null, Action<Chore> on_end = null, bool allow_in_red_alert = true, ScheduleBlockType schedule_block = null, bool ignore_schedule_block = false, bool only_when_operational = true, KAnimFile override_anims = null, bool is_preemptable = false, bool allow_in_context_menu = true, bool allow_prioritization = true, PriorityScreen.PriorityClass priority_class = PriorityScreen.PriorityClass.basic, int priority_class_value = 5, bool ignore_building_assignment = false, bool add_to_daily_report = true) : base(chore_type, target, chore_provider, run_until_complete, on_complete, on_begin, on_end, priority_class, priority_class_value, is_preemptable, allow_in_context_menu, 0, add_to_daily_report, ReportManager.ReportType.WorkTime)
	{
		base.smi = new WorkChore<WorkableType>.StatesInstance(this, target.gameObject, override_anims);
		this.onlyWhenOperational = only_when_operational;
		if (allow_prioritization)
		{
			base.SetPrioritizable(target.GetComponent<Prioritizable>());
		}
		this.AddPrecondition(ChorePreconditions.instance.IsNotTransferArm, null);
		if (!allow_in_red_alert)
		{
			this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		}
		if (schedule_block != null)
		{
			this.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, schedule_block);
		}
		else if (!ignore_schedule_block)
		{
			this.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, Db.Get().ScheduleBlockTypes.Work);
		}
		this.AddPrecondition(ChorePreconditions.instance.CanMoveTo, base.smi.sm.workable.Get<WorkableType>(base.smi));
		Operational component = target.GetComponent<Operational>();
		if (only_when_operational && component != null)
		{
			this.AddPrecondition(ChorePreconditions.instance.IsOperational, component);
		}
		if (only_when_operational)
		{
			Deconstructable component2 = target.GetComponent<Deconstructable>();
			if (component2 != null)
			{
				this.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDeconstruction, component2);
			}
			BuildingEnabledButton component3 = target.GetComponent<BuildingEnabledButton>();
			if (component3 != null)
			{
				this.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDisable, component3);
			}
		}
		if (!ignore_building_assignment && base.smi.sm.workable.Get(base.smi).GetComponent<Assignable>() != null)
		{
			this.AddPrecondition(ChorePreconditions.instance.IsAssignedtoMe, base.smi.sm.workable.Get<Assignable>(base.smi));
		}
		if (override_anims != null)
		{
			this.AddPrecondition(ChorePreconditions.instance.IsNotARobot, null);
		}
		WorkableType workableType = target as WorkableType;
		if (workableType != null)
		{
			if (!string.IsNullOrEmpty(workableType.requiredSkillPerk))
			{
				HashedString hashedString = workableType.requiredSkillPerk;
				this.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, hashedString);
			}
			if (workableType.requireMinionToWork)
			{
				this.AddPrecondition(ChorePreconditions.instance.IsMinion, null);
			}
		}
	}

	// Token: 0x060019CB RID: 6603 RVA: 0x0009063E File Offset: 0x0008E83E
	public override void Begin(Chore.Precondition.Context context)
	{
		base.smi.sm.worker.Set(context.consumerState.gameObject, base.smi, false);
		base.Begin(context);
	}

	// Token: 0x060019CC RID: 6604 RVA: 0x00090670 File Offset: 0x0008E870
	public override bool IsValid()
	{
		WorkableType workableType = this.target as WorkableType;
		if (workableType != null)
		{
			return this.provider != null && Grid.IsWorldValidCell(workableType.GetCell());
		}
		return base.IsValid();
	}

	// Token: 0x060019CD RID: 6605 RVA: 0x000906C4 File Offset: 0x0008E8C4
	public bool IsOperationalValid()
	{
		if (this.onlyWhenOperational)
		{
			Operational component = base.smi.master.GetComponent<Operational>();
			if (component != null && !component.IsOperational)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060019CE RID: 6606 RVA: 0x00090700 File Offset: 0x0008E900
	public override bool CanPreempt(Chore.Precondition.Context context)
	{
		if (!base.CanPreempt(context))
		{
			return false;
		}
		if (context.chore.driver == null)
		{
			return false;
		}
		if (context.chore.driver == context.consumerState.choreDriver)
		{
			return false;
		}
		Workable workable = base.smi.sm.workable.Get<WorkableType>(base.smi);
		if (workable == null)
		{
			return false;
		}
		if (workable.worker != null && (workable.worker.GetState() == WorkerBase.State.PendingCompletion || workable.worker.GetState() == WorkerBase.State.Completing))
		{
			return false;
		}
		if (this.preemption_cb != null)
		{
			if (!this.preemption_cb(context))
			{
				return false;
			}
		}
		else
		{
			int num = 4;
			int navigationCost = context.chore.driver.GetComponent<Navigator>().GetNavigationCost(workable);
			if (navigationCost == -1 || navigationCost < num)
			{
				return false;
			}
			if (context.consumerState.navigator.GetNavigationCost(workable) * 2 > navigationCost)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x04000EEB RID: 3819
	public Func<Chore.Precondition.Context, bool> preemption_cb;

	// Token: 0x02001339 RID: 4921
	public class StatesInstance : GameStateMachine<WorkChore<WorkableType>.States, WorkChore<WorkableType>.StatesInstance, WorkChore<WorkableType>, object>.GameInstance
	{
		// Token: 0x06008B4A RID: 35658 RVA: 0x0035EB48 File Offset: 0x0035CD48
		public StatesInstance(WorkChore<WorkableType> master, GameObject workable, KAnimFile override_anims) : base(master)
		{
			this.overrideAnims = override_anims;
			base.sm.workable.Set(workable, base.smi, false);
		}

		// Token: 0x06008B4B RID: 35659 RVA: 0x0035EB71 File Offset: 0x0035CD71
		public void EnableAnimOverrides()
		{
			if (this.overrideAnims != null)
			{
				base.sm.worker.Get<KAnimControllerBase>(base.smi).AddAnimOverrides(this.overrideAnims, 0f);
			}
		}

		// Token: 0x06008B4C RID: 35660 RVA: 0x0035EBA7 File Offset: 0x0035CDA7
		public void DisableAnimOverrides()
		{
			if (this.overrideAnims != null)
			{
				base.sm.worker.Get<KAnimControllerBase>(base.smi).RemoveAnimOverrides(this.overrideAnims);
			}
		}

		// Token: 0x04006AAD RID: 27309
		private KAnimFile overrideAnims;
	}

	// Token: 0x0200133A RID: 4922
	public class States : GameStateMachine<WorkChore<WorkableType>.States, WorkChore<WorkableType>.StatesInstance, WorkChore<WorkableType>>
	{
		// Token: 0x06008B4D RID: 35661 RVA: 0x0035EBD8 File Offset: 0x0035CDD8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.approach;
			base.Target(this.worker);
			this.approach.InitializeStates(this.worker, this.workable, this.work, null, null, null).Update("CheckOperational", delegate(WorkChore<WorkableType>.StatesInstance smi, float dt)
			{
				if (!smi.master.IsOperationalValid())
				{
					smi.StopSM("Building not operational");
				}
			}, UpdateRate.SIM_200ms, false);
			this.work.Enter(delegate(WorkChore<WorkableType>.StatesInstance smi)
			{
				smi.EnableAnimOverrides();
			}).ToggleWork<WorkableType>(this.workable, this.success, null, (WorkChore<WorkableType>.StatesInstance smi) => smi.master.IsOperationalValid()).Exit(delegate(WorkChore<WorkableType>.StatesInstance smi)
			{
				smi.DisableAnimOverrides();
			});
			this.success.ReturnSuccess();
		}

		// Token: 0x04006AAE RID: 27310
		public GameStateMachine<WorkChore<WorkableType>.States, WorkChore<WorkableType>.StatesInstance, WorkChore<WorkableType>, object>.ApproachSubState<WorkableType> approach;

		// Token: 0x04006AAF RID: 27311
		public GameStateMachine<WorkChore<WorkableType>.States, WorkChore<WorkableType>.StatesInstance, WorkChore<WorkableType>, object>.State work;

		// Token: 0x04006AB0 RID: 27312
		public GameStateMachine<WorkChore<WorkableType>.States, WorkChore<WorkableType>.StatesInstance, WorkChore<WorkableType>, object>.State success;

		// Token: 0x04006AB1 RID: 27313
		public StateMachine<WorkChore<WorkableType>.States, WorkChore<WorkableType>.StatesInstance, WorkChore<WorkableType>, object>.TargetParameter workable;

		// Token: 0x04006AB2 RID: 27314
		public StateMachine<WorkChore<WorkableType>.States, WorkChore<WorkableType>.StatesInstance, WorkChore<WorkableType>, object>.TargetParameter worker;
	}
}
