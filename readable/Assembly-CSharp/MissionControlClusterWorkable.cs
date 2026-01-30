using System;
using TUNING;
using UnityEngine;

// Token: 0x020007C4 RID: 1988
public class MissionControlClusterWorkable : Workable
{
	// Token: 0x17000346 RID: 838
	// (get) Token: 0x06003493 RID: 13459 RVA: 0x0012A4C5 File Offset: 0x001286C5
	// (set) Token: 0x06003494 RID: 13460 RVA: 0x0012A4CD File Offset: 0x001286CD
	public Clustercraft TargetClustercraft
	{
		get
		{
			return this.targetClustercraft;
		}
		set
		{
			base.WorkTimeRemaining = this.GetWorkTime();
			this.targetClustercraft = value;
		}
	}

	// Token: 0x06003495 RID: 13461 RVA: 0x0012A4E4 File Offset: 0x001286E4
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

	// Token: 0x06003496 RID: 13462 RVA: 0x0012A5A2 File Offset: 0x001287A2
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.MissionControlClusterWorkables.Add(this);
	}

	// Token: 0x06003497 RID: 13463 RVA: 0x0012A5B5 File Offset: 0x001287B5
	protected override void OnCleanUp()
	{
		Components.MissionControlClusterWorkables.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06003498 RID: 13464 RVA: 0x0012A5C8 File Offset: 0x001287C8
	public static bool IsRocketInRange(AxialI worldLocation, AxialI rocketLocation)
	{
		return AxialUtil.GetDistance(worldLocation, rocketLocation) <= 2;
	}

	// Token: 0x06003499 RID: 13465 RVA: 0x0012A5D8 File Offset: 0x001287D8
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.workStatusItem = base.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.MissionControlAssistingRocket, this.TargetClustercraft);
		this.operational.SetActive(true, false);
	}

	// Token: 0x0600349A RID: 13466 RVA: 0x0012A624 File Offset: 0x00128824
	public override float GetEfficiencyMultiplier(WorkerBase worker)
	{
		return base.GetEfficiencyMultiplier(worker) * Mathf.Clamp01(this.GetSMI<SkyVisibilityMonitor.Instance>().PercentClearSky);
	}

	// Token: 0x0600349B RID: 13467 RVA: 0x0012A63E File Offset: 0x0012883E
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (this.TargetClustercraft == null || !MissionControlClusterWorkable.IsRocketInRange(base.gameObject.GetMyWorldLocation(), this.TargetClustercraft.Location))
		{
			worker.StopWork();
			return true;
		}
		return base.OnWorkTick(worker, dt);
	}

	// Token: 0x0600349C RID: 13468 RVA: 0x0012A67B File Offset: 0x0012887B
	protected override void OnCompleteWork(WorkerBase worker)
	{
		global::Debug.Assert(this.TargetClustercraft != null);
		base.gameObject.GetSMI<MissionControlCluster.Instance>().ApplyEffect(this.TargetClustercraft);
		base.OnCompleteWork(worker);
	}

	// Token: 0x0600349D RID: 13469 RVA: 0x0012A6AB File Offset: 0x001288AB
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		base.gameObject.GetComponent<KSelectable>().RemoveStatusItem(this.workStatusItem, false);
		this.TargetClustercraft = null;
		this.operational.SetActive(false, false);
	}

	// Token: 0x04001FC4 RID: 8132
	private Clustercraft targetClustercraft;

	// Token: 0x04001FC5 RID: 8133
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001FC6 RID: 8134
	private Guid workStatusItem = Guid.Empty;
}
