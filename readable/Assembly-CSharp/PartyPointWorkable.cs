using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x02000A8E RID: 2702
public class PartyPointWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x06004E7C RID: 20092 RVA: 0x001C8BBA File Offset: 0x001C6DBA
	private PartyPointWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x06004E7D RID: 20093 RVA: 0x001C8BCC File Offset: 0x001C6DCC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_generic_convo_kanim")
		};
		this.workAnimPlayMode = KAnim.PlayMode.Loop;
		this.faceTargetWhenWorking = true;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Socializing;
		this.synchronizeAnims = false;
		this.showProgressBar = false;
		this.resetProgressOnStop = true;
		this.lightEfficiencyBonus = false;
		if (UnityEngine.Random.Range(0f, 100f) > 80f)
		{
			this.activity = PartyPointWorkable.ActivityType.Dance;
		}
		else
		{
			this.activity = PartyPointWorkable.ActivityType.Talk;
		}
		PartyPointWorkable.ActivityType activityType = this.activity;
		if (activityType == PartyPointWorkable.ActivityType.Talk)
		{
			this.workAnims = new HashedString[]
			{
				"idle"
			};
			this.workerOverrideAnims = new KAnimFile[][]
			{
				new KAnimFile[]
				{
					Assets.GetAnim("anim_generic_convo_kanim")
				}
			};
			return;
		}
		if (activityType != PartyPointWorkable.ActivityType.Dance)
		{
			return;
		}
		this.workAnims = new HashedString[]
		{
			"working_loop"
		};
		this.workerOverrideAnims = new KAnimFile[][]
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

	// Token: 0x06004E7E RID: 20094 RVA: 0x001C8D30 File Offset: 0x001C6F30
	public override Workable.AnimInfo GetAnim(WorkerBase worker)
	{
		int num = UnityEngine.Random.Range(0, this.workerOverrideAnims.Length);
		this.overrideAnims = this.workerOverrideAnims[num];
		return base.GetAnim(worker);
	}

	// Token: 0x06004E7F RID: 20095 RVA: 0x001C8D61 File Offset: 0x001C6F61
	public override Vector3 GetFacingTarget()
	{
		if (this.lastTalker != null)
		{
			return this.lastTalker.transform.GetPosition();
		}
		return base.GetFacingTarget();
	}

	// Token: 0x06004E80 RID: 20096 RVA: 0x001C8D88 File Offset: 0x001C6F88
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		return false;
	}

	// Token: 0x06004E81 RID: 20097 RVA: 0x001C8D8C File Offset: 0x001C6F8C
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		worker.GetComponent<KPrefabID>().AddTag(GameTags.AlwaysConverse, false);
		worker.Subscribe(-594200555, new Action<object>(this.OnStartedTalking));
		worker.Subscribe(25860745, new Action<object>(this.OnStoppedTalking));
	}

	// Token: 0x06004E82 RID: 20098 RVA: 0x001C8DE4 File Offset: 0x001C6FE4
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		worker.GetComponent<KPrefabID>().RemoveTag(GameTags.AlwaysConverse);
		worker.Unsubscribe(-594200555, new Action<object>(this.OnStartedTalking));
		worker.Unsubscribe(25860745, new Action<object>(this.OnStoppedTalking));
	}

	// Token: 0x06004E83 RID: 20099 RVA: 0x001C8E38 File Offset: 0x001C7038
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.specificEffect))
		{
			component.Add(this.specificEffect, true);
		}
	}

	// Token: 0x06004E84 RID: 20100 RVA: 0x001C8E68 File Offset: 0x001C7068
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
			if (this.activity == PartyPointWorkable.ActivityType.Talk)
			{
				KBatchedAnimController component = base.worker.GetComponent<KBatchedAnimController>();
				string text = startedTalkingEvent.anim;
				text += UnityEngine.Random.Range(1, 9).ToString();
				component.Play(text, KAnim.PlayMode.Once, 1f, 0f);
				component.Queue("idle", KAnim.PlayMode.Loop, 1f, 0f);
				return;
			}
		}
		else
		{
			if (this.activity == PartyPointWorkable.ActivityType.Talk)
			{
				base.worker.GetComponent<Facing>().Face(talker.transform.GetPosition());
			}
			this.lastTalker = talker;
		}
	}

	// Token: 0x06004E85 RID: 20101 RVA: 0x001C8F2A File Offset: 0x001C712A
	private void OnStoppedTalking(object data)
	{
	}

	// Token: 0x06004E86 RID: 20102 RVA: 0x001C8F2C File Offset: 0x001C712C
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		if (!string.IsNullOrEmpty(this.specificEffect) && worker.GetComponent<Effects>().HasEffect(this.specificEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x0400345B RID: 13403
	private GameObject lastTalker;

	// Token: 0x0400345C RID: 13404
	public int basePriority;

	// Token: 0x0400345D RID: 13405
	public string specificEffect;

	// Token: 0x0400345E RID: 13406
	public KAnimFile[][] workerOverrideAnims;

	// Token: 0x0400345F RID: 13407
	private PartyPointWorkable.ActivityType activity;

	// Token: 0x02001BA9 RID: 7081
	private enum ActivityType
	{
		// Token: 0x04008571 RID: 34161
		Talk,
		// Token: 0x04008572 RID: 34162
		Dance,
		// Token: 0x04008573 RID: 34163
		LENGTH
	}
}
