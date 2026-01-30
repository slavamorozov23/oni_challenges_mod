using System;
using TUNING;
using UnityEngine;

// Token: 0x020007C5 RID: 1989
public class MissionControlWorkable : Workable
{
	// Token: 0x17000347 RID: 839
	// (get) Token: 0x0600349F RID: 13471 RVA: 0x0012A6F3 File Offset: 0x001288F3
	// (set) Token: 0x060034A0 RID: 13472 RVA: 0x0012A6FB File Offset: 0x001288FB
	public Spacecraft TargetSpacecraft
	{
		get
		{
			return this.targetSpacecraft;
		}
		set
		{
			base.WorkTimeRemaining = this.GetWorkTime();
			this.targetSpacecraft = value;
		}
	}

	// Token: 0x060034A1 RID: 13473 RVA: 0x0012A710 File Offset: 0x00128910
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.requiredSkillPerk = Db.Get().SkillPerks.CanMissionControl.Id;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.MissionControlling;
		this.attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_mission_control_station_kanim")
		};
		base.SetWorkTime(90f);
		this.showProgressBar = true;
		this.lightEfficiencyBonus = true;
	}

	// Token: 0x060034A2 RID: 13474 RVA: 0x0012A7CE File Offset: 0x001289CE
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.MissionControlWorkables.Add(this);
	}

	// Token: 0x060034A3 RID: 13475 RVA: 0x0012A7E1 File Offset: 0x001289E1
	protected override void OnCleanUp()
	{
		Components.MissionControlWorkables.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x060034A4 RID: 13476 RVA: 0x0012A7F4 File Offset: 0x001289F4
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.workStatusItem = base.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.MissionControlAssistingRocket, this.TargetSpacecraft);
		this.operational.SetActive(true, false);
	}

	// Token: 0x060034A5 RID: 13477 RVA: 0x0012A840 File Offset: 0x00128A40
	public override float GetEfficiencyMultiplier(WorkerBase worker)
	{
		return base.GetEfficiencyMultiplier(worker) * Mathf.Clamp01(this.GetSMI<SkyVisibilityMonitor.Instance>().PercentClearSky);
	}

	// Token: 0x060034A6 RID: 13478 RVA: 0x0012A85A File Offset: 0x00128A5A
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (this.TargetSpacecraft == null)
		{
			worker.StopWork();
			return true;
		}
		return base.OnWorkTick(worker, dt);
	}

	// Token: 0x060034A7 RID: 13479 RVA: 0x0012A874 File Offset: 0x00128A74
	protected override void OnCompleteWork(WorkerBase worker)
	{
		global::Debug.Assert(this.TargetSpacecraft != null);
		base.gameObject.GetSMI<MissionControl.Instance>().ApplyEffect(this.TargetSpacecraft);
		base.OnCompleteWork(worker);
	}

	// Token: 0x060034A8 RID: 13480 RVA: 0x0012A8A1 File Offset: 0x00128AA1
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		base.gameObject.GetComponent<KSelectable>().RemoveStatusItem(this.workStatusItem, false);
		this.TargetSpacecraft = null;
		this.operational.SetActive(false, false);
	}

	// Token: 0x04001FC7 RID: 8135
	private Spacecraft targetSpacecraft;

	// Token: 0x04001FC8 RID: 8136
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001FC9 RID: 8137
	private Guid workStatusItem = Guid.Empty;
}
