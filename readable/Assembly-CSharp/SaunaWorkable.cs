using System;
using Klei;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x02000B26 RID: 2854
[AddComponentMenu("KMonoBehaviour/Workable/SaunaWorkable")]
public class SaunaWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x06005363 RID: 21347 RVA: 0x001E5F85 File Offset: 0x001E4185
	private SaunaWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x06005364 RID: 21348 RVA: 0x001E5F98 File Offset: 0x001E4198
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_sauna_kanim")
		};
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		this.synchronizeAnims = true;
		this.workLayer = Grid.SceneLayer.BuildingUse;
		base.SetWorkTime(30f);
		this.sauna = base.GetComponent<Sauna>();
	}

	// Token: 0x06005365 RID: 21349 RVA: 0x001E5FFD File Offset: 0x001E41FD
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.operational.SetActive(true, false);
		worker.GetComponent<Effects>().Add("SaunaRelaxing", false);
	}

	// Token: 0x06005366 RID: 21350 RVA: 0x001E6028 File Offset: 0x001E4228
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.sauna.specificEffect))
		{
			component.Add(this.sauna.specificEffect, true);
		}
		if (!string.IsNullOrEmpty(this.sauna.trackingEffect))
		{
			component.Add(this.sauna.trackingEffect, true);
		}
		component.Add("WarmTouch", true).timeRemaining = 1800f;
		this.operational.SetActive(false, false);
	}

	// Token: 0x06005367 RID: 21351 RVA: 0x001E60AC File Offset: 0x001E42AC
	protected override void OnStopWork(WorkerBase worker)
	{
		this.operational.SetActive(false, false);
		worker.GetComponent<Effects>().Remove("SaunaRelaxing");
		Storage component = base.GetComponent<Storage>();
		float num;
		SimUtil.DiseaseInfo diseaseInfo;
		float num2;
		component.ConsumeAndGetDisease(SimHashes.Steam.CreateTag(), this.sauna.steamPerUseKG, out num, out diseaseInfo, out num2);
		component.AddLiquid(SimHashes.Water, this.sauna.steamPerUseKG, this.sauna.waterOutputTemp, diseaseInfo.idx, diseaseInfo.count, true, false);
	}

	// Token: 0x06005368 RID: 21352 RVA: 0x001E612C File Offset: 0x001E432C
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.sauna.trackingEffect) && component.HasEffect(this.sauna.trackingEffect))
		{
			priority = 0;
			return false;
		}
		if (!string.IsNullOrEmpty(this.sauna.specificEffect) && component.HasEffect(this.sauna.specificEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x04003873 RID: 14451
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04003874 RID: 14452
	public int basePriority;

	// Token: 0x04003875 RID: 14453
	private Sauna sauna;
}
