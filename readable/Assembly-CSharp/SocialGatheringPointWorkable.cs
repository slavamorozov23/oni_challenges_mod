using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x02000B57 RID: 2903
[AddComponentMenu("KMonoBehaviour/Workable/SocialGatheringPointWorkable")]
public class SocialGatheringPointWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x060055C0 RID: 21952 RVA: 0x001F4235 File Offset: 0x001F2435
	private SocialGatheringPointWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x060055C1 RID: 21953 RVA: 0x001F4248 File Offset: 0x001F2448
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_generic_convo_kanim")
		};
		this.workAnims = new HashedString[]
		{
			"idle"
		};
		this.faceTargetWhenWorking = true;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Socializing;
		this.synchronizeAnims = false;
		this.showProgressBar = false;
		this.resetProgressOnStop = true;
		this.lightEfficiencyBonus = false;
	}

	// Token: 0x060055C2 RID: 21954 RVA: 0x001F42CE File Offset: 0x001F24CE
	public override Vector3 GetFacingTarget()
	{
		if (this.lastTalker != null)
		{
			return this.lastTalker.transform.GetPosition();
		}
		return base.GetFacingTarget();
	}

	// Token: 0x060055C3 RID: 21955 RVA: 0x001F42F8 File Offset: 0x001F24F8
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (!worker.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Recreation))
		{
			Effects component = worker.GetComponent<Effects>();
			if (string.IsNullOrEmpty(this.specificEffect) || component.HasEffect(this.specificEffect))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060055C4 RID: 21956 RVA: 0x001F4348 File Offset: 0x001F2548
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		worker.GetComponent<KPrefabID>().AddTag(GameTags.AlwaysConverse, false);
		worker.Subscribe(-594200555, new Action<object>(this.OnStartedTalking));
		worker.Subscribe(25860745, new Action<object>(this.OnStoppedTalking));
		this.timesConversed = 0;
	}

	// Token: 0x060055C5 RID: 21957 RVA: 0x001F43A4 File Offset: 0x001F25A4
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		worker.GetComponent<KPrefabID>().RemoveTag(GameTags.AlwaysConverse);
		worker.Unsubscribe(-594200555, new Action<object>(this.OnStartedTalking));
		worker.Unsubscribe(25860745, new Action<object>(this.OnStoppedTalking));
	}

	// Token: 0x060055C6 RID: 21958 RVA: 0x001F43F8 File Offset: 0x001F25F8
	protected override void OnCompleteWork(WorkerBase worker)
	{
		if (this.timesConversed > 0)
		{
			Effects component = worker.GetComponent<Effects>();
			if (!string.IsNullOrEmpty(this.specificEffect))
			{
				component.Add(this.specificEffect, true);
			}
		}
	}

	// Token: 0x060055C7 RID: 21959 RVA: 0x001F4430 File Offset: 0x001F2630
	private void OnStartedTalking(object data)
	{
		ConversationManager.StartedTalkingEvent startedTalkingEvent = data as ConversationManager.StartedTalkingEvent;
		if (startedTalkingEvent == null)
		{
			return;
		}
		GameObject talker = startedTalkingEvent.talker;
		if (talker == base.worker.gameObject)
		{
			KBatchedAnimController component = base.worker.GetComponent<KBatchedAnimController>();
			string text = startedTalkingEvent.anim;
			text += UnityEngine.Random.Range(1, 9).ToString();
			component.Play(text, KAnim.PlayMode.Once, 1f, 0f);
			component.Queue("idle", KAnim.PlayMode.Loop, 1f, 0f);
		}
		else
		{
			base.worker.GetComponent<Facing>().Face(talker.transform.GetPosition());
			this.lastTalker = talker;
		}
		this.timesConversed++;
	}

	// Token: 0x060055C8 RID: 21960 RVA: 0x001F44EE File Offset: 0x001F26EE
	private void OnStoppedTalking(object data)
	{
	}

	// Token: 0x060055C9 RID: 21961 RVA: 0x001F44F0 File Offset: 0x001F26F0
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		if (!string.IsNullOrEmpty(this.specificEffect) && worker.GetComponent<Effects>().HasEffect(this.specificEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x040039E5 RID: 14821
	private GameObject lastTalker;

	// Token: 0x040039E6 RID: 14822
	public int basePriority;

	// Token: 0x040039E7 RID: 14823
	public string specificEffect;

	// Token: 0x040039E8 RID: 14824
	public int timesConversed;
}
