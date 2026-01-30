using System;
using System.Collections.Generic;
using Klei;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x02000B59 RID: 2905
[AddComponentMenu("KMonoBehaviour/Workable/SodaFountainWorkable")]
public class SodaFountainWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x060055CF RID: 21967 RVA: 0x001F4667 File Offset: 0x001F2867
	private SodaFountainWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x060055D0 RID: 21968 RVA: 0x001F4684 File Offset: 0x001F2884
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_sodamaker_kanim")
		};
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		this.synchronizeAnims = false;
		base.SetWorkTime(30f);
		this.sodaFountain = base.GetComponent<SodaFountain>();
	}

	// Token: 0x060055D1 RID: 21969 RVA: 0x001F46E4 File Offset: 0x001F28E4
	public override Workable.AnimInfo GetAnim(WorkerBase worker)
	{
		KAnimFile[] overrideAnims = null;
		if (this.workerTypeOverrideAnims.TryGetValue(worker.PrefabID(), out overrideAnims))
		{
			this.overrideAnims = overrideAnims;
		}
		return base.GetAnim(worker);
	}

	// Token: 0x060055D2 RID: 21970 RVA: 0x001F4716 File Offset: 0x001F2916
	protected override void OnStartWork(WorkerBase worker)
	{
		this.operational.SetActive(true, false);
	}

	// Token: 0x060055D3 RID: 21971 RVA: 0x001F4728 File Offset: 0x001F2928
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Storage component = base.GetComponent<Storage>();
		float num;
		SimUtil.DiseaseInfo diseaseInfo;
		float num2;
		component.ConsumeAndGetDisease(GameTags.Water, this.sodaFountain.waterMassPerUse, out num, out diseaseInfo, out num2);
		SimUtil.DiseaseInfo diseaseInfo2;
		component.ConsumeAndGetDisease(this.sodaFountain.ingredientTag, this.sodaFountain.ingredientMassPerUse, out num, out diseaseInfo2, out num2);
		GermExposureMonitor.Instance smi = worker.GetSMI<GermExposureMonitor.Instance>();
		if (smi != null)
		{
			smi.TryInjectDisease(diseaseInfo.idx, diseaseInfo.count, GameTags.Water, Sickness.InfectionVector.Digestion);
			smi.TryInjectDisease(diseaseInfo2.idx, diseaseInfo2.count, this.sodaFountain.ingredientTag, Sickness.InfectionVector.Digestion);
		}
		Effects component2 = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.sodaFountain.specificEffect))
		{
			component2.Add(this.sodaFountain.specificEffect, true);
		}
		if (!string.IsNullOrEmpty(this.sodaFountain.trackingEffect))
		{
			component2.Add(this.sodaFountain.trackingEffect, true);
		}
	}

	// Token: 0x060055D4 RID: 21972 RVA: 0x001F4810 File Offset: 0x001F2A10
	protected override void OnStopWork(WorkerBase worker)
	{
		this.operational.SetActive(false, false);
	}

	// Token: 0x060055D5 RID: 21973 RVA: 0x001F4820 File Offset: 0x001F2A20
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.sodaFountain.trackingEffect) && component.HasEffect(this.sodaFountain.trackingEffect))
		{
			priority = 0;
			return false;
		}
		if (!string.IsNullOrEmpty(this.sodaFountain.specificEffect) && component.HasEffect(this.sodaFountain.specificEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x040039EE RID: 14830
	public Dictionary<Tag, KAnimFile[]> workerTypeOverrideAnims = new Dictionary<Tag, KAnimFile[]>();

	// Token: 0x040039EF RID: 14831
	[MyCmpReq]
	private Operational operational;

	// Token: 0x040039F0 RID: 14832
	public int basePriority;

	// Token: 0x040039F1 RID: 14833
	private SodaFountain sodaFountain;
}
