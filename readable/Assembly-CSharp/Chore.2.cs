using System;
using UnityEngine;

// Token: 0x020004CF RID: 1231
public class Chore<StateMachineInstanceType> : StandardChoreBase, IStateMachineTarget where StateMachineInstanceType : StateMachine.Instance
{
	// Token: 0x170000A3 RID: 163
	// (get) Token: 0x06001A3B RID: 6715 RVA: 0x000912CC File Offset: 0x0008F4CC
	// (set) Token: 0x06001A3C RID: 6716 RVA: 0x000912D4 File Offset: 0x0008F4D4
	public StateMachineInstanceType smi { get; protected set; }

	// Token: 0x06001A3D RID: 6717 RVA: 0x000912DD File Offset: 0x0008F4DD
	protected override StateMachine.Instance GetSMI()
	{
		return this.smi;
	}

	// Token: 0x06001A3E RID: 6718 RVA: 0x000912EA File Offset: 0x0008F4EA
	public int Subscribe(int hash, Action<object> handler)
	{
		return this.GetComponent<KPrefabID>().Subscribe(hash, handler);
	}

	// Token: 0x06001A3F RID: 6719 RVA: 0x000912F9 File Offset: 0x0008F4F9
	public int Subscribe(int hash, Action<object, object> handler, object context)
	{
		return this.GetComponent<KPrefabID>().Subscribe(hash, handler, context);
	}

	// Token: 0x06001A40 RID: 6720 RVA: 0x00091309 File Offset: 0x0008F509
	public void Unsubscribe(int hash, Action<object> handler)
	{
		this.GetComponent<KPrefabID>().Unsubscribe(hash, handler);
	}

	// Token: 0x06001A41 RID: 6721 RVA: 0x00091318 File Offset: 0x0008F518
	public void Unsubscribe(int id)
	{
		this.GetComponent<KPrefabID>().Unsubscribe(id);
	}

	// Token: 0x06001A42 RID: 6722 RVA: 0x00091326 File Offset: 0x0008F526
	public void Unsubscribe(ref int id)
	{
		this.Unsubscribe(id);
		id = -1;
	}

	// Token: 0x06001A43 RID: 6723 RVA: 0x00091333 File Offset: 0x0008F533
	public void Trigger(int hash, object data = null)
	{
		this.GetComponent<KPrefabID>().Trigger(hash, data);
	}

	// Token: 0x06001A44 RID: 6724 RVA: 0x00091342 File Offset: 0x0008F542
	public ComponentType GetComponent<ComponentType>()
	{
		return this.target.GetComponent<ComponentType>();
	}

	// Token: 0x170000A4 RID: 164
	// (get) Token: 0x06001A45 RID: 6725 RVA: 0x0009134F File Offset: 0x0008F54F
	public override GameObject gameObject
	{
		get
		{
			return this.target.gameObject;
		}
	}

	// Token: 0x170000A5 RID: 165
	// (get) Token: 0x06001A46 RID: 6726 RVA: 0x0009135C File Offset: 0x0008F55C
	public Transform transform
	{
		get
		{
			return this.target.gameObject.transform;
		}
	}

	// Token: 0x170000A6 RID: 166
	// (get) Token: 0x06001A47 RID: 6727 RVA: 0x0009136E File Offset: 0x0008F56E
	public string name
	{
		get
		{
			return this.gameObject.name;
		}
	}

	// Token: 0x170000A7 RID: 167
	// (get) Token: 0x06001A48 RID: 6728 RVA: 0x0009137B File Offset: 0x0008F57B
	public override bool isNull
	{
		get
		{
			return this.target.isNull;
		}
	}

	// Token: 0x06001A49 RID: 6729 RVA: 0x00091388 File Offset: 0x0008F588
	public Chore(ChoreType chore_type, IStateMachineTarget target, ChoreProvider chore_provider, bool run_until_complete = true, Action<Chore> on_complete = null, Action<Chore> on_begin = null, Action<Chore> on_end = null, PriorityScreen.PriorityClass master_priority_class = PriorityScreen.PriorityClass.basic, int master_priority_value = 5, bool is_preemptable = false, bool allow_in_context_menu = true, int priority_mod = 0, bool add_to_daily_report = false, ReportManager.ReportType report_type = ReportManager.ReportType.WorkTime) : base(chore_type, target, chore_provider, run_until_complete, on_complete, on_begin, on_end, master_priority_class, master_priority_value, is_preemptable, allow_in_context_menu, priority_mod, add_to_daily_report, report_type)
	{
		this.onTargetDestroyedHandlerID = target.Subscribe(1969584890, new Action<object>(this.OnTargetDestroyed));
		this.reportType = report_type;
		this.addToDailyReport = add_to_daily_report;
		if (this.addToDailyReport)
		{
			ReportManager.Instance.ReportValue(ReportManager.ReportType.ChoreStatus, 1f, chore_type.Name, GameUtil.GetChoreName(this, null));
		}
	}

	// Token: 0x06001A4A RID: 6730 RVA: 0x00091406 File Offset: 0x0008F606
	public override string ResolveString(string str)
	{
		if (!this.target.isNull)
		{
			str = str.Replace("{Target}", this.target.gameObject.GetProperName());
		}
		return base.ResolveString(str);
	}

	// Token: 0x06001A4B RID: 6731 RVA: 0x00091439 File Offset: 0x0008F639
	public override void Cleanup()
	{
		base.Cleanup();
		if (this.target != null)
		{
			this.target.Unsubscribe(ref this.onTargetDestroyedHandlerID);
		}
		if (this.onCleanup != null)
		{
			this.onCleanup(this);
		}
	}

	// Token: 0x06001A4C RID: 6732 RVA: 0x0009146E File Offset: 0x0008F66E
	private void OnTargetDestroyed(object data)
	{
		this.Cancel("Target Destroyed");
	}

	// Token: 0x06001A4D RID: 6733 RVA: 0x0009147B File Offset: 0x0008F67B
	public override bool CanPreempt(Chore.Precondition.Context context)
	{
		return base.CanPreempt(context);
	}

	// Token: 0x04000F17 RID: 3863
	private int onTargetDestroyedHandlerID;
}
