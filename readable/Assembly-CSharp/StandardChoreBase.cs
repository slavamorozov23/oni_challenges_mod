using System;
using System.Collections.Generic;

// Token: 0x020004CE RID: 1230
public abstract class StandardChoreBase : Chore
{
	// Token: 0x17000096 RID: 150
	// (get) Token: 0x06001A06 RID: 6662 RVA: 0x00090A31 File Offset: 0x0008EC31
	// (set) Token: 0x06001A07 RID: 6663 RVA: 0x00090A39 File Offset: 0x0008EC39
	public override int id { get; protected set; }

	// Token: 0x17000097 RID: 151
	// (get) Token: 0x06001A08 RID: 6664 RVA: 0x00090A42 File Offset: 0x0008EC42
	// (set) Token: 0x06001A09 RID: 6665 RVA: 0x00090A4A File Offset: 0x0008EC4A
	public override int priorityMod { get; protected set; }

	// Token: 0x17000098 RID: 152
	// (get) Token: 0x06001A0A RID: 6666 RVA: 0x00090A53 File Offset: 0x0008EC53
	// (set) Token: 0x06001A0B RID: 6667 RVA: 0x00090A5B File Offset: 0x0008EC5B
	public override ChoreType choreType { get; protected set; }

	// Token: 0x17000099 RID: 153
	// (get) Token: 0x06001A0C RID: 6668 RVA: 0x00090A64 File Offset: 0x0008EC64
	// (set) Token: 0x06001A0D RID: 6669 RVA: 0x00090A6C File Offset: 0x0008EC6C
	public override ChoreDriver driver { get; protected set; }

	// Token: 0x1700009A RID: 154
	// (get) Token: 0x06001A0E RID: 6670 RVA: 0x00090A75 File Offset: 0x0008EC75
	// (set) Token: 0x06001A0F RID: 6671 RVA: 0x00090A7D File Offset: 0x0008EC7D
	public override ChoreDriver lastDriver { get; protected set; }

	// Token: 0x06001A10 RID: 6672 RVA: 0x00090A86 File Offset: 0x0008EC86
	public override bool SatisfiesUrge(Urge urge)
	{
		return urge == this.choreType.urge;
	}

	// Token: 0x06001A11 RID: 6673 RVA: 0x00090A96 File Offset: 0x0008EC96
	public override bool IsValid()
	{
		return this.provider != null && this.gameObject.GetMyWorldId() != -1;
	}

	// Token: 0x1700009B RID: 155
	// (get) Token: 0x06001A12 RID: 6674 RVA: 0x00090AB9 File Offset: 0x0008ECB9
	// (set) Token: 0x06001A13 RID: 6675 RVA: 0x00090AC1 File Offset: 0x0008ECC1
	public override IStateMachineTarget target { get; protected set; }

	// Token: 0x1700009C RID: 156
	// (get) Token: 0x06001A14 RID: 6676 RVA: 0x00090ACA File Offset: 0x0008ECCA
	// (set) Token: 0x06001A15 RID: 6677 RVA: 0x00090AD2 File Offset: 0x0008ECD2
	public override bool isComplete { get; protected set; }

	// Token: 0x1700009D RID: 157
	// (get) Token: 0x06001A16 RID: 6678 RVA: 0x00090ADB File Offset: 0x0008ECDB
	// (set) Token: 0x06001A17 RID: 6679 RVA: 0x00090AE3 File Offset: 0x0008ECE3
	public override bool IsPreemptable { get; protected set; }

	// Token: 0x1700009E RID: 158
	// (get) Token: 0x06001A18 RID: 6680 RVA: 0x00090AEC File Offset: 0x0008ECEC
	// (set) Token: 0x06001A19 RID: 6681 RVA: 0x00090AF4 File Offset: 0x0008ECF4
	public override ChoreConsumer overrideTarget { get; protected set; }

	// Token: 0x1700009F RID: 159
	// (get) Token: 0x06001A1A RID: 6682 RVA: 0x00090AFD File Offset: 0x0008ECFD
	// (set) Token: 0x06001A1B RID: 6683 RVA: 0x00090B05 File Offset: 0x0008ED05
	public override Prioritizable prioritizable { get; protected set; }

	// Token: 0x170000A0 RID: 160
	// (get) Token: 0x06001A1C RID: 6684 RVA: 0x00090B0E File Offset: 0x0008ED0E
	// (set) Token: 0x06001A1D RID: 6685 RVA: 0x00090B16 File Offset: 0x0008ED16
	public override ChoreProvider provider { get; set; }

	// Token: 0x170000A1 RID: 161
	// (get) Token: 0x06001A1E RID: 6686 RVA: 0x00090B1F File Offset: 0x0008ED1F
	// (set) Token: 0x06001A1F RID: 6687 RVA: 0x00090B27 File Offset: 0x0008ED27
	public override bool runUntilComplete { get; set; }

	// Token: 0x170000A2 RID: 162
	// (get) Token: 0x06001A20 RID: 6688 RVA: 0x00090B30 File Offset: 0x0008ED30
	// (set) Token: 0x06001A21 RID: 6689 RVA: 0x00090B38 File Offset: 0x0008ED38
	public override bool isExpanded { get; protected set; }

	// Token: 0x06001A22 RID: 6690 RVA: 0x00090B41 File Offset: 0x0008ED41
	public override bool CanPreempt(Chore.Precondition.Context context)
	{
		return this.IsPreemptable;
	}

	// Token: 0x06001A23 RID: 6691 RVA: 0x00090B49 File Offset: 0x0008ED49
	public override void PrepareChore(ref Chore.Precondition.Context context)
	{
	}

	// Token: 0x06001A24 RID: 6692 RVA: 0x00090B4B File Offset: 0x0008ED4B
	public override string GetReportName(string context = null)
	{
		if (context == null || this.choreType.reportName == null)
		{
			return this.choreType.Name;
		}
		return string.Format(this.choreType.reportName, context);
	}

	// Token: 0x06001A25 RID: 6693 RVA: 0x00090B7C File Offset: 0x0008ED7C
	public override void Cancel(string reason)
	{
		if (!this.RemoveFromProvider())
		{
			return;
		}
		if (this.addToDailyReport)
		{
			ReportManager.Instance.ReportValue(ReportManager.ReportType.ChoreStatus, -1f, this.choreType.Name, GameUtil.GetChoreName(this, null));
			SaveGame.Instance.ColonyAchievementTracker.LogSuitChore((this.driver != null) ? this.driver : this.lastDriver);
		}
		this.End(reason);
		this.Cleanup();
	}

	// Token: 0x06001A26 RID: 6694 RVA: 0x00090BF4 File Offset: 0x0008EDF4
	public override void Cleanup()
	{
		this.ClearPrioritizable();
	}

	// Token: 0x06001A27 RID: 6695 RVA: 0x00090BFC File Offset: 0x0008EDFC
	public override ReportManager.ReportType GetReportType()
	{
		return this.reportType;
	}

	// Token: 0x06001A28 RID: 6696 RVA: 0x00090C04 File Offset: 0x0008EE04
	public override void AddPrecondition(Chore.Precondition precondition, object data = null)
	{
		this.arePreconditionsDirty = true;
		this.preconditions.Add(new Chore.PreconditionInstance
		{
			condition = precondition,
			data = data
		});
	}

	// Token: 0x06001A29 RID: 6697 RVA: 0x00090C3C File Offset: 0x0008EE3C
	public override void CollectChores(ChoreConsumerState consumer_state, List<Chore.Precondition.Context> succeeded_contexts, List<Chore.Precondition.Context> incomplete_contexts, List<Chore.Precondition.Context> failed_contexts, bool is_attempting_override)
	{
		Chore.Precondition.Context item = new Chore.Precondition.Context(this, consumer_state, is_attempting_override, null);
		item.RunPreconditions();
		if (!item.IsComplete())
		{
			incomplete_contexts.Add(item);
			return;
		}
		if (item.IsSuccess())
		{
			succeeded_contexts.Add(item);
			return;
		}
		failed_contexts.Add(item);
	}

	// Token: 0x06001A2A RID: 6698 RVA: 0x00090C86 File Offset: 0x0008EE86
	public override void Fail(string reason)
	{
		if (this.provider == null)
		{
			return;
		}
		if (this.driver == null)
		{
			return;
		}
		if (!this.runUntilComplete)
		{
			this.Cancel(reason);
			return;
		}
		this.End(reason);
	}

	// Token: 0x06001A2B RID: 6699 RVA: 0x00090CC0 File Offset: 0x0008EEC0
	public override void Reserve(ChoreDriver reserver)
	{
		if (this.driver != null && this.driver != reserver && reserver != null)
		{
			Debug.LogErrorFormat("Chore.Reserve: driver already set {0} {1} {2}, provider {3}, driver {4} -> {5}", new object[]
			{
				this.id,
				base.GetType(),
				this.choreType.Id,
				this.provider,
				this.driver,
				reserver
			});
		}
		this.driver = reserver;
	}

	// Token: 0x06001A2C RID: 6700 RVA: 0x00090D44 File Offset: 0x0008EF44
	public override void Begin(Chore.Precondition.Context context)
	{
		if (this.driver != null && this.driver != context.consumerState.choreDriver)
		{
			Debug.LogErrorFormat("Chore.Begin driver already set {0} {1} {2}, provider {3}, driver {4} -> {5}", new object[]
			{
				this.id,
				base.GetType(),
				this.choreType.Id,
				this.provider,
				this.driver,
				context.consumerState.choreDriver
			});
		}
		if (this.provider == null)
		{
			Debug.LogErrorFormat("Chore.Begin provider is null {0} {1} {2}, provider {3}, driver {4}", new object[]
			{
				this.id,
				base.GetType(),
				this.choreType.Id,
				this.provider,
				this.driver
			});
		}
		this.driver = context.consumerState.choreDriver;
		StateMachine.Instance smi = this.GetSMI();
		smi.OnStop = (Action<string, StateMachine.Status>)Delegate.Combine(smi.OnStop, new Action<string, StateMachine.Status>(this.OnStateMachineStop));
		KSelectable component = this.driver.GetComponent<KSelectable>();
		if (component != null)
		{
			component.SetStatusItem(Db.Get().StatusItemCategories.Main, this.GetStatusItem(), this);
		}
		smi.StartSM();
		if (this.onBegin != null)
		{
			this.onBegin(this);
		}
	}

	// Token: 0x06001A2D RID: 6701 RVA: 0x00090EA6 File Offset: 0x0008F0A6
	public override bool InProgress()
	{
		return this.driver != null;
	}

	// Token: 0x06001A2E RID: 6702
	protected abstract StateMachine.Instance GetSMI();

	// Token: 0x06001A2F RID: 6703 RVA: 0x00090EB4 File Offset: 0x0008F0B4
	public StandardChoreBase(ChoreType chore_type, IStateMachineTarget target, ChoreProvider chore_provider, bool run_until_complete, Action<Chore> on_complete, Action<Chore> on_begin, Action<Chore> on_end, PriorityScreen.PriorityClass priority_class, int priority_value, bool is_preemptable, bool allow_in_context_menu, int priority_mod, bool add_to_daily_report, ReportManager.ReportType report_type)
	{
		this.target = target;
		if (priority_value == 2147483647)
		{
			priority_class = PriorityScreen.PriorityClass.topPriority;
			priority_value = 2;
		}
		if (priority_value < 1 || priority_value > 9)
		{
			Debug.LogErrorFormat("Priority Value Out Of Range: {0}", new object[]
			{
				priority_value
			});
		}
		this.masterPriority = new PrioritySetting(priority_class, priority_value);
		this.priorityMod = priority_mod;
		this.id = Chore.GetNextChoreID();
		if (chore_provider == null)
		{
			chore_provider = GlobalChoreProvider.Instance;
			DebugUtil.Assert(chore_provider != null);
		}
		this.choreType = chore_type;
		this.runUntilComplete = run_until_complete;
		this.onComplete = on_complete;
		this.onEnd = on_end;
		this.onBegin = on_begin;
		this.IsPreemptable = is_preemptable;
		this.AddPrecondition(ChorePreconditions.instance.IsValid, null);
		this.AddPrecondition(ChorePreconditions.instance.IsPermitted, null);
		this.AddPrecondition(ChorePreconditions.instance.IsPreemptable, null);
		this.AddPrecondition(ChorePreconditions.instance.HasUrge, null);
		this.AddPrecondition(ChorePreconditions.instance.IsMoreSatisfyingEarly, null);
		this.AddPrecondition(ChorePreconditions.instance.IsMoreSatisfyingLate, null);
		this.AddPrecondition(ChorePreconditions.instance.IsOverrideTargetNullOrMe, null);
		chore_provider.AddChore(this);
	}

	// Token: 0x06001A30 RID: 6704 RVA: 0x00090FF8 File Offset: 0x0008F1F8
	public virtual void SetPriorityMod(int priorityMod)
	{
		this.priorityMod = priorityMod;
	}

	// Token: 0x06001A31 RID: 6705 RVA: 0x00091004 File Offset: 0x0008F204
	public override List<Chore.PreconditionInstance> GetPreconditions()
	{
		if (this.arePreconditionsDirty)
		{
			List<Chore.PreconditionInstance> obj = this.preconditions;
			lock (obj)
			{
				if (this.arePreconditionsDirty)
				{
					this.preconditions.Sort((Chore.PreconditionInstance x, Chore.PreconditionInstance y) => x.condition.sortOrder.CompareTo(y.condition.sortOrder));
					this.arePreconditionsDirty = false;
				}
			}
		}
		return this.preconditions;
	}

	// Token: 0x06001A32 RID: 6706 RVA: 0x00091088 File Offset: 0x0008F288
	protected void SetPrioritizable(Prioritizable prioritizable)
	{
		if (prioritizable != null && prioritizable.IsPrioritizable())
		{
			this.prioritizable = prioritizable;
			this.masterPriority = prioritizable.GetMasterPriority();
			prioritizable.onPriorityChanged = (Action<PrioritySetting>)Delegate.Combine(prioritizable.onPriorityChanged, new Action<PrioritySetting>(this.OnMasterPriorityChanged));
		}
	}

	// Token: 0x06001A33 RID: 6707 RVA: 0x000910DB File Offset: 0x0008F2DB
	private void ClearPrioritizable()
	{
		if (this.prioritizable != null)
		{
			Prioritizable prioritizable = this.prioritizable;
			prioritizable.onPriorityChanged = (Action<PrioritySetting>)Delegate.Remove(prioritizable.onPriorityChanged, new Action<PrioritySetting>(this.OnMasterPriorityChanged));
		}
	}

	// Token: 0x06001A34 RID: 6708 RVA: 0x00091112 File Offset: 0x0008F312
	private void OnMasterPriorityChanged(PrioritySetting priority)
	{
		this.masterPriority = priority;
	}

	// Token: 0x06001A35 RID: 6709 RVA: 0x0009111B File Offset: 0x0008F31B
	public void SetOverrideTarget(ChoreConsumer chore_consumer)
	{
		this.overrideTarget = chore_consumer;
		this.Fail("New override target");
	}

	// Token: 0x06001A36 RID: 6710 RVA: 0x00091130 File Offset: 0x0008F330
	protected virtual void End(string reason)
	{
		if (this.driver != null)
		{
			KSelectable component = this.driver.GetComponent<KSelectable>();
			if (component != null)
			{
				component.SetStatusItem(Db.Get().StatusItemCategories.Main, null, null);
			}
		}
		StateMachine.Instance smi = this.GetSMI();
		smi.OnStop = (Action<string, StateMachine.Status>)Delegate.Remove(smi.OnStop, new Action<string, StateMachine.Status>(this.OnStateMachineStop));
		smi.StopSM(reason);
		if (this.driver == null)
		{
			return;
		}
		this.lastDriver = this.driver;
		this.driver = null;
		if (this.onEnd != null)
		{
			this.onEnd(this);
		}
		if (this.onExit != null)
		{
			this.onExit(this);
		}
		this.driver = null;
	}

	// Token: 0x06001A37 RID: 6711 RVA: 0x000911F8 File Offset: 0x0008F3F8
	protected void Succeed(string reason)
	{
		if (!this.RemoveFromProvider())
		{
			return;
		}
		this.isComplete = true;
		if (this.onComplete != null)
		{
			this.onComplete(this);
		}
		if (this.addToDailyReport)
		{
			ReportManager.Instance.ReportValue(ReportManager.ReportType.ChoreStatus, -1f, this.choreType.Name, GameUtil.GetChoreName(this, null));
			SaveGame.Instance.ColonyAchievementTracker.LogSuitChore((this.driver != null) ? this.driver : this.lastDriver);
		}
		this.End(reason);
		this.Cleanup();
	}

	// Token: 0x06001A38 RID: 6712 RVA: 0x0009128B File Offset: 0x0008F48B
	protected virtual StatusItem GetStatusItem()
	{
		return this.choreType.statusItem;
	}

	// Token: 0x06001A39 RID: 6713 RVA: 0x00091298 File Offset: 0x0008F498
	protected virtual void OnStateMachineStop(string reason, StateMachine.Status status)
	{
		if (status == StateMachine.Status.Success)
		{
			this.Succeed(reason);
			return;
		}
		this.Fail(reason);
	}

	// Token: 0x06001A3A RID: 6714 RVA: 0x000912AD File Offset: 0x0008F4AD
	private bool RemoveFromProvider()
	{
		if (this.provider != null)
		{
			this.provider.RemoveChore(this);
			return true;
		}
		return false;
	}

	// Token: 0x04000F0F RID: 3855
	private Action<Chore> onBegin;

	// Token: 0x04000F10 RID: 3856
	private Action<Chore> onEnd;

	// Token: 0x04000F11 RID: 3857
	public Action<Chore> onCleanup;

	// Token: 0x04000F12 RID: 3858
	private List<Chore.PreconditionInstance> preconditions = new List<Chore.PreconditionInstance>();

	// Token: 0x04000F13 RID: 3859
	private bool arePreconditionsDirty;

	// Token: 0x04000F14 RID: 3860
	public bool addToDailyReport;

	// Token: 0x04000F15 RID: 3861
	public ReportManager.ReportType reportType;
}
