using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x02000BF2 RID: 3058
[AddComponentMenu("KMonoBehaviour/Workable/TelephoneWorkable")]
public class TelephoneCallerWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x06005BCC RID: 23500 RVA: 0x00213068 File Offset: 0x00211268
	private TelephoneCallerWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
		this.workingPstComplete = new HashedString[]
		{
			"on_pst"
		};
		this.workAnims = new HashedString[]
		{
			"on_pre",
			"on",
			"on_receiving",
			"on_pre_loop_receiving",
			"on_loop",
			"on_loop_pre"
		};
	}

	// Token: 0x06005BCD RID: 23501 RVA: 0x00213114 File Offset: 0x00211314
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_telephone_kanim")
		};
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		this.synchronizeAnims = true;
		base.SetWorkTime(40f);
		this.telephone = base.GetComponent<Telephone>();
	}

	// Token: 0x06005BCE RID: 23502 RVA: 0x00213171 File Offset: 0x00211371
	protected override void OnStartWork(WorkerBase worker)
	{
		this.operational.SetActive(true, false);
		this.telephone.isInUse = true;
	}

	// Token: 0x06005BCF RID: 23503 RVA: 0x0021318C File Offset: 0x0021138C
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Effects component = worker.GetComponent<Effects>();
		if (this.telephone.HasTag(GameTags.LongDistanceCall))
		{
			if (!string.IsNullOrEmpty(this.telephone.longDistanceEffect))
			{
				component.Add(this.telephone.longDistanceEffect, true);
			}
		}
		else if (this.telephone.wasAnswered)
		{
			if (!string.IsNullOrEmpty(this.telephone.chatEffect))
			{
				component.Add(this.telephone.chatEffect, true);
			}
		}
		else if (!string.IsNullOrEmpty(this.telephone.babbleEffect))
		{
			component.Add(this.telephone.babbleEffect, true);
		}
		if (!string.IsNullOrEmpty(this.telephone.trackingEffect))
		{
			component.Add(this.telephone.trackingEffect, true);
		}
	}

	// Token: 0x06005BD0 RID: 23504 RVA: 0x00213257 File Offset: 0x00211457
	protected override void OnStopWork(WorkerBase worker)
	{
		this.operational.SetActive(false, false);
		this.telephone.HangUp();
	}

	// Token: 0x06005BD1 RID: 23505 RVA: 0x00213274 File Offset: 0x00211474
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.telephone.trackingEffect) && component.HasEffect(this.telephone.trackingEffect))
		{
			priority = 0;
			return false;
		}
		if (!string.IsNullOrEmpty(this.telephone.chatEffect) && component.HasEffect(this.telephone.chatEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		if (!string.IsNullOrEmpty(this.telephone.babbleEffect) && component.HasEffect(this.telephone.babbleEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x04003D16 RID: 15638
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04003D17 RID: 15639
	public int basePriority;

	// Token: 0x04003D18 RID: 15640
	private Telephone telephone;
}
