using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;

// Token: 0x02000B51 RID: 2897
[AddComponentMenu("KMonoBehaviour/Workable/Sleepable")]
public class Sleepable : Workable
{
	// Token: 0x0600558B RID: 21899 RVA: 0x001F365C File Offset: 0x001F185C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetReportType(ReportManager.ReportType.PersonalTime);
		this.showProgressBar = false;
		this.workerStatusItem = null;
		this.synchronizeAnims = false;
		this.triggerWorkReactions = false;
		this.lightEfficiencyBonus = false;
		this.approachable = base.GetComponent<IApproachable>();
	}

	// Token: 0x0600558C RID: 21900 RVA: 0x001F369B File Offset: 0x001F189B
	protected override void OnSpawn()
	{
		if (this.isNormalBed)
		{
			Components.NormalBeds.Add(base.gameObject.GetMyWorldId(), this);
		}
		base.SetWorkTime(float.PositiveInfinity);
	}

	// Token: 0x0600558D RID: 21901 RVA: 0x001F36C8 File Offset: 0x001F18C8
	public override HashedString[] GetWorkAnims(WorkerBase worker)
	{
		MinionResume component = worker.GetComponent<MinionResume>();
		if (base.GetComponent<Building>() != null && component != null && component.CurrentHat != null)
		{
			return Sleepable.hatWorkAnims;
		}
		return Sleepable.normalWorkAnims;
	}

	// Token: 0x0600558E RID: 21902 RVA: 0x001F3708 File Offset: 0x001F1908
	public override HashedString[] GetWorkPstAnims(WorkerBase worker, bool successfully_completed)
	{
		MinionResume component = worker.GetComponent<MinionResume>();
		if (base.GetComponent<Building>() != null && component != null && component.CurrentHat != null)
		{
			return Sleepable.hatWorkPstAnim;
		}
		return Sleepable.normalWorkPstAnim;
	}

	// Token: 0x0600558F RID: 21903 RVA: 0x001F3748 File Offset: 0x001F1948
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		KAnimControllerBase animController = this.GetAnimController();
		if (animController != null)
		{
			animController.Play("working_pre", KAnim.PlayMode.Once, 1f, 0f);
			animController.Queue("working_loop", KAnim.PlayMode.Loop, 1f, 0f);
		}
		base.Subscribe(worker.gameObject, -1142962013, new Action<object>(this.PlayPstAnim));
		if (this.operational != null)
		{
			this.operational.SetActive(true, false);
		}
		worker.Trigger(-1283701846, this);
		worker.GetComponent<Effects>().Add(this.effectName, false);
		this.isDoneSleeping = false;
	}

	// Token: 0x06005590 RID: 21904 RVA: 0x001F3804 File Offset: 0x001F1A04
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (this.isDoneSleeping)
		{
			return Time.time > this.wakeTime;
		}
		if (this.Dreamable != null && !this.Dreamable.DreamIsDisturbed)
		{
			this.Dreamable.WorkTick(worker, dt);
		}
		if (worker.GetSMI<StaminaMonitor.Instance>().ShouldExitSleep())
		{
			this.isDoneSleeping = true;
			this.wakeTime = Time.time + UnityEngine.Random.value * 3f;
		}
		return false;
	}

	// Token: 0x06005591 RID: 21905 RVA: 0x001F387C File Offset: 0x001F1A7C
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		if (this.operational != null)
		{
			this.operational.SetActive(false, false);
		}
		base.Unsubscribe(worker.gameObject, -1142962013, new Action<object>(this.PlayPstAnim));
		if (worker != null)
		{
			Effects component = worker.GetComponent<Effects>();
			component.Remove(this.effectName);
			if (this.wakeEffects != null)
			{
				foreach (string effect_id in this.wakeEffects)
				{
					component.Add(effect_id, true);
				}
			}
			if (this.stretchOnWake && UnityEngine.Random.value < 0.33f)
			{
				new EmoteChore(worker.GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteHighPriority, Db.Get().Emotes.Minion.MorningStretch, 1, null);
			}
			if (worker.GetAmounts().Get(Db.Get().Amounts.Stamina).value < worker.GetAmounts().Get(Db.Get().Amounts.Stamina).GetMax())
			{
				worker.Trigger(1338475637, this);
			}
		}
	}

	// Token: 0x06005592 RID: 21906 RVA: 0x001F39C8 File Offset: 0x001F1BC8
	public override bool InstantlyFinish(WorkerBase worker)
	{
		return false;
	}

	// Token: 0x06005593 RID: 21907 RVA: 0x001F39CB File Offset: 0x001F1BCB
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.isNormalBed)
		{
			Components.NormalBeds.Remove(base.gameObject.GetMyWorldId(), this);
		}
	}

	// Token: 0x06005594 RID: 21908 RVA: 0x001F39F4 File Offset: 0x001F1BF4
	private void PlayPstAnim(object data)
	{
		WorkerBase workerBase = (WorkerBase)data;
		if (workerBase != null && workerBase.GetWorkable() != null)
		{
			KAnimControllerBase component = workerBase.GetWorkable().gameObject.GetComponent<KAnimControllerBase>();
			if (component != null)
			{
				component.Play("working_pst", KAnim.PlayMode.Once, 1f, 0f);
			}
		}
	}

	// Token: 0x040039BA RID: 14778
	private const float STRECH_CHANCE = 0.33f;

	// Token: 0x040039BB RID: 14779
	[MyCmpGet]
	public Assignable assignable;

	// Token: 0x040039BC RID: 14780
	public IApproachable approachable;

	// Token: 0x040039BD RID: 14781
	[MyCmpGet]
	private Operational operational;

	// Token: 0x040039BE RID: 14782
	public string effectName = "Sleep";

	// Token: 0x040039BF RID: 14783
	public List<string> wakeEffects;

	// Token: 0x040039C0 RID: 14784
	public bool stretchOnWake = true;

	// Token: 0x040039C1 RID: 14785
	private float wakeTime;

	// Token: 0x040039C2 RID: 14786
	private bool isDoneSleeping;

	// Token: 0x040039C3 RID: 14787
	public bool isNormalBed = true;

	// Token: 0x040039C4 RID: 14788
	public ClinicDreamable Dreamable;

	// Token: 0x040039C5 RID: 14789
	private static readonly HashedString[] normalWorkAnims = new HashedString[]
	{
		"working_pre",
		"working_loop"
	};

	// Token: 0x040039C6 RID: 14790
	private static readonly HashedString[] hatWorkAnims = new HashedString[]
	{
		"hat_pre",
		"working_loop"
	};

	// Token: 0x040039C7 RID: 14791
	private static readonly HashedString[] normalWorkPstAnim = new HashedString[]
	{
		"working_pst"
	};

	// Token: 0x040039C8 RID: 14792
	private static readonly HashedString[] hatWorkPstAnim = new HashedString[]
	{
		"hat_pst"
	};
}
