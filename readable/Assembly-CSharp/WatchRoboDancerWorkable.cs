using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x0200082B RID: 2091
public class WatchRoboDancerWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x06003908 RID: 14600 RVA: 0x0013EFA0 File Offset: 0x0013D1A0
	private WatchRoboDancerWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x06003909 RID: 14601 RVA: 0x0013F008 File Offset: 0x0013D208
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.synchronizeAnims = false;
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.WatchRoboDancerWorkable;
		base.SetWorkTime(30f);
		this.showProgressBar = false;
	}

	// Token: 0x0600390A RID: 14602 RVA: 0x0013F058 File Offset: 0x0013D258
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(WatchRoboDancerWorkable.TRACKING_EFFECT))
		{
			component.Add(WatchRoboDancerWorkable.TRACKING_EFFECT, true);
		}
		if (!string.IsNullOrEmpty(WatchRoboDancerWorkable.SPECIFIC_EFFECT))
		{
			component.Add(WatchRoboDancerWorkable.SPECIFIC_EFFECT, true);
		}
	}

	// Token: 0x0600390B RID: 14603 RVA: 0x0013F0A0 File Offset: 0x0013D2A0
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(WatchRoboDancerWorkable.TRACKING_EFFECT) && component.HasEffect(WatchRoboDancerWorkable.TRACKING_EFFECT))
		{
			priority = 0;
			return false;
		}
		if (!string.IsNullOrEmpty(WatchRoboDancerWorkable.SPECIFIC_EFFECT) && component.HasEffect(WatchRoboDancerWorkable.SPECIFIC_EFFECT))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x0600390C RID: 14604 RVA: 0x0013F0FB File Offset: 0x0013D2FB
	protected override void OnStartWork(WorkerBase worker)
	{
		worker.GetComponent<Effects>().Add("Dancing", false);
	}

	// Token: 0x0600390D RID: 14605 RVA: 0x0013F10F File Offset: 0x0013D30F
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		worker.GetComponent<Facing>().Face(this.owner.transform.position.x);
		return base.OnWorkTick(worker, dt);
	}

	// Token: 0x0600390E RID: 14606 RVA: 0x0013F139 File Offset: 0x0013D339
	protected override void OnStopWork(WorkerBase worker)
	{
		worker.GetComponent<Effects>().Remove("Dancing");
		ChoreHelpers.DestroyLocator(base.gameObject);
	}

	// Token: 0x0600390F RID: 14607 RVA: 0x0013F158 File Offset: 0x0013D358
	public override Workable.AnimInfo GetAnim(WorkerBase worker)
	{
		int num = UnityEngine.Random.Range(0, this.workerOverrideAnims.Length);
		this.overrideAnims = this.workerOverrideAnims[num];
		return base.GetAnim(worker);
	}

	// Token: 0x040022D6 RID: 8918
	public GameObject owner;

	// Token: 0x040022D7 RID: 8919
	public int basePriority = RELAXATION.PRIORITY.TIER3;

	// Token: 0x040022D8 RID: 8920
	public static string SPECIFIC_EFFECT = "SawRoboDancer";

	// Token: 0x040022D9 RID: 8921
	public static string TRACKING_EFFECT = "RecentlySawRoboDancer";

	// Token: 0x040022DA RID: 8922
	public KAnimFile[][] workerOverrideAnims = new KAnimFile[][]
	{
		new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_robotdance_kanim")
		},
		new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_robotdance1_kanim")
		}
	};
}
