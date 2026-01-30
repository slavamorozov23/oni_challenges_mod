using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x02000A92 RID: 2706
[AddComponentMenu("KMonoBehaviour/Workable/PhonoboxWorkable")]
public class PhonoboxWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x06004E95 RID: 20117 RVA: 0x001C93D8 File Offset: 0x001C75D8
	private PhonoboxWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x06004E96 RID: 20118 RVA: 0x001C9471 File Offset: 0x001C7671
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.synchronizeAnims = false;
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		base.SetWorkTime(15f);
	}

	// Token: 0x06004E97 RID: 20119 RVA: 0x001C949C File Offset: 0x001C769C
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.trackingEffect))
		{
			component.Add(this.trackingEffect, true);
		}
		if (!string.IsNullOrEmpty(this.specificEffect))
		{
			component.Add(this.specificEffect, true);
		}
	}

	// Token: 0x06004E98 RID: 20120 RVA: 0x001C94E8 File Offset: 0x001C76E8
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.trackingEffect) && component.HasEffect(this.trackingEffect))
		{
			priority = 0;
			return false;
		}
		if (!string.IsNullOrEmpty(this.specificEffect) && component.HasEffect(this.specificEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x06004E99 RID: 20121 RVA: 0x001C9547 File Offset: 0x001C7747
	protected override void OnStartWork(WorkerBase worker)
	{
		this.owner.AddWorker(worker);
		worker.GetComponent<Effects>().Add("Dancing", false);
	}

	// Token: 0x06004E9A RID: 20122 RVA: 0x001C9567 File Offset: 0x001C7767
	protected override void OnStopWork(WorkerBase worker)
	{
		this.owner.RemoveWorker(worker);
		worker.GetComponent<Effects>().Remove("Dancing");
	}

	// Token: 0x06004E9B RID: 20123 RVA: 0x001C9588 File Offset: 0x001C7788
	public override Workable.AnimInfo GetAnim(WorkerBase worker)
	{
		int num = UnityEngine.Random.Range(0, this.workerOverrideAnims.Length);
		this.overrideAnims = this.workerOverrideAnims[num];
		return base.GetAnim(worker);
	}

	// Token: 0x0400346A RID: 13418
	public Phonobox owner;

	// Token: 0x0400346B RID: 13419
	public int basePriority = RELAXATION.PRIORITY.TIER3;

	// Token: 0x0400346C RID: 13420
	public string specificEffect = "Danced";

	// Token: 0x0400346D RID: 13421
	public string trackingEffect = "RecentlyDanced";

	// Token: 0x0400346E RID: 13422
	public KAnimFile[][] workerOverrideAnims = new KAnimFile[][]
	{
		new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_phonobox_danceone_kanim")
		},
		new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_phonobox_dancetwo_kanim")
		},
		new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_phonobox_dancethree_kanim")
		}
	};
}
