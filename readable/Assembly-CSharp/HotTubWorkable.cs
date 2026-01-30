using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x0200098F RID: 2447
[AddComponentMenu("KMonoBehaviour/Workable/HotTubWorkable")]
public class HotTubWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x0600466B RID: 18027 RVA: 0x00196631 File Offset: 0x00194831
	private HotTubWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x0600466C RID: 18028 RVA: 0x00196641 File Offset: 0x00194841
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.synchronizeAnims = false;
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		this.faceTargetWhenWorking = true;
		base.SetWorkTime(90f);
	}

	// Token: 0x0600466D RID: 18029 RVA: 0x00196670 File Offset: 0x00194870
	public override Workable.AnimInfo GetAnim(WorkerBase worker)
	{
		Workable.AnimInfo anim = base.GetAnim(worker);
		anim.smi = new HotTubWorkerStateMachine.StatesInstance(worker);
		return anim;
	}

	// Token: 0x0600466E RID: 18030 RVA: 0x00196693 File Offset: 0x00194893
	protected override void OnStartWork(WorkerBase worker)
	{
		this.faceLeft = (UnityEngine.Random.value > 0.5f);
		worker.GetComponent<Effects>().Add("HotTubRelaxing", false);
	}

	// Token: 0x0600466F RID: 18031 RVA: 0x001966BD File Offset: 0x001948BD
	protected override void OnStopWork(WorkerBase worker)
	{
		worker.GetComponent<Effects>().Remove("HotTubRelaxing");
	}

	// Token: 0x06004670 RID: 18032 RVA: 0x001966CF File Offset: 0x001948CF
	public override Vector3 GetFacingTarget()
	{
		return base.transform.GetPosition() + (this.faceLeft ? Vector3.left : Vector3.right);
	}

	// Token: 0x06004671 RID: 18033 RVA: 0x001966F8 File Offset: 0x001948F8
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.hotTub.trackingEffect))
		{
			component.Add(this.hotTub.trackingEffect, true);
		}
		if (!string.IsNullOrEmpty(this.hotTub.specificEffect))
		{
			component.Add(this.hotTub.specificEffect, true);
		}
		component.Add("WarmTouch", true).timeRemaining = 1800f;
	}

	// Token: 0x06004672 RID: 18034 RVA: 0x0019676C File Offset: 0x0019496C
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.hotTub.basePriority;
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.hotTub.trackingEffect) && component.HasEffect(this.hotTub.trackingEffect))
		{
			priority = 0;
			return false;
		}
		if (!string.IsNullOrEmpty(this.hotTub.specificEffect) && component.HasEffect(this.hotTub.specificEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x04002F6B RID: 12139
	public HotTub hotTub;

	// Token: 0x04002F6C RID: 12140
	private bool faceLeft;
}
