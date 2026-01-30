using System;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x02000A73 RID: 2675
public class NuclearResearchCenterWorkable : Workable
{
	// Token: 0x06004DC2 RID: 19906 RVA: 0x001C3A9C File Offset: 0x001C1C9C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Researching;
		this.attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.ALL_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
		this.skillExperienceMultiplier = SKILLS.ALL_DAY_EXPERIENCE;
		this.radiationStorage = base.GetComponent<HighEnergyParticleStorage>();
		this.nrc = base.GetComponent<NuclearResearchCenter>();
		this.lightEfficiencyBonus = true;
	}

	// Token: 0x06004DC3 RID: 19907 RVA: 0x001C3B28 File Offset: 0x001C1D28
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.SetWorkTime(float.PositiveInfinity);
	}

	// Token: 0x06004DC4 RID: 19908 RVA: 0x001C3B3C File Offset: 0x001C1D3C
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		float num = dt / this.nrc.timePerPoint;
		if (Game.Instance.FastWorkersModeActive)
		{
			num *= 2f;
		}
		this.radiationStorage.ConsumeAndGet(num * this.nrc.materialPerPoint);
		this.pointsProduced += num;
		if (this.pointsProduced >= 1f)
		{
			int num2 = Mathf.FloorToInt(this.pointsProduced);
			this.pointsProduced -= (float)num2;
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Research, Research.Instance.GetResearchType("nuclear").name, base.transform, 1.5f, false);
			Research.Instance.AddResearchPoints("nuclear", (float)num2);
		}
		TechInstance activeResearch = Research.Instance.GetActiveResearch();
		return this.radiationStorage.IsEmpty() || activeResearch == null || activeResearch.PercentageCompleteResearchType("nuclear") >= 1f;
	}

	// Token: 0x06004DC5 RID: 19909 RVA: 0x001C3C32 File Offset: 0x001C1E32
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorResearching, this.nrc);
	}

	// Token: 0x06004DC6 RID: 19910 RVA: 0x001C3C5C File Offset: 0x001C1E5C
	protected override void OnAbortWork(WorkerBase worker)
	{
		base.OnAbortWork(worker);
	}

	// Token: 0x06004DC7 RID: 19911 RVA: 0x001C3C65 File Offset: 0x001C1E65
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorResearching, this.nrc);
	}

	// Token: 0x06004DC8 RID: 19912 RVA: 0x001C3C94 File Offset: 0x001C1E94
	public override float GetPercentComplete()
	{
		if (Research.Instance.GetActiveResearch() == null)
		{
			return 0f;
		}
		float num = Research.Instance.GetActiveResearch().progressInventory.PointsByTypeID["nuclear"];
		float num2 = 0f;
		if (!Research.Instance.GetActiveResearch().tech.costsByResearchTypeID.TryGetValue("nuclear", out num2))
		{
			return 1f;
		}
		return num / num2;
	}

	// Token: 0x06004DC9 RID: 19913 RVA: 0x001C3D03 File Offset: 0x001C1F03
	public override bool InstantlyFinish(WorkerBase worker)
	{
		return false;
	}

	// Token: 0x040033D1 RID: 13265
	[MyCmpReq]
	private Operational operational;

	// Token: 0x040033D2 RID: 13266
	[Serialize]
	private float pointsProduced;

	// Token: 0x040033D3 RID: 13267
	private NuclearResearchCenter nrc;

	// Token: 0x040033D4 RID: 13268
	private HighEnergyParticleStorage radiationStorage;
}
