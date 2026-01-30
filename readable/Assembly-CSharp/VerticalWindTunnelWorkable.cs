using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x02000C15 RID: 3093
[AddComponentMenu("KMonoBehaviour/Workable/VerticalWindTunnelWorkable")]
public class VerticalWindTunnelWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x06005D10 RID: 23824 RVA: 0x0021B0E7 File Offset: 0x002192E7
	private VerticalWindTunnelWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x06005D11 RID: 23825 RVA: 0x0021B0F8 File Offset: 0x002192F8
	public override Workable.AnimInfo GetAnim(WorkerBase worker)
	{
		Workable.AnimInfo anim = base.GetAnim(worker);
		anim.smi = new WindTunnelWorkerStateMachine.StatesInstance(worker, this);
		return anim;
	}

	// Token: 0x06005D12 RID: 23826 RVA: 0x0021B11C File Offset: 0x0021931C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.synchronizeAnims = false;
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		base.SetWorkTime(90f);
	}

	// Token: 0x06005D13 RID: 23827 RVA: 0x0021B144 File Offset: 0x00219344
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		worker.GetComponent<Effects>().Add("VerticalWindTunnelFlying", false);
	}

	// Token: 0x06005D14 RID: 23828 RVA: 0x0021B15F File Offset: 0x0021935F
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		worker.GetComponent<Effects>().Remove("VerticalWindTunnelFlying");
	}

	// Token: 0x06005D15 RID: 23829 RVA: 0x0021B178 File Offset: 0x00219378
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Effects component = worker.GetComponent<Effects>();
		component.Add(this.windTunnel.trackingEffect, true);
		component.Add(this.windTunnel.specificEffect, true);
	}

	// Token: 0x06005D16 RID: 23830 RVA: 0x0021B1A8 File Offset: 0x002193A8
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.windTunnel.basePriority;
		Effects component = worker.GetComponent<Effects>();
		if (component.HasEffect(this.windTunnel.trackingEffect))
		{
			priority = 0;
			return false;
		}
		if (component.HasEffect(this.windTunnel.specificEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x04003DF2 RID: 15858
	public VerticalWindTunnel windTunnel;

	// Token: 0x04003DF3 RID: 15859
	public HashedString overrideAnim;

	// Token: 0x04003DF4 RID: 15860
	public string[] preAnims;

	// Token: 0x04003DF5 RID: 15861
	public string loopAnim;

	// Token: 0x04003DF6 RID: 15862
	public string[] pstAnims;
}
