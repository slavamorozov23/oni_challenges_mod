using System;
using System.Collections.Generic;
using Klei;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x020009D5 RID: 2517
[AddComponentMenu("KMonoBehaviour/Workable/JuicerWorkable")]
public class JuicerWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x06004911 RID: 18705 RVA: 0x001A6C3A File Offset: 0x001A4E3A
	private JuicerWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x06004912 RID: 18706 RVA: 0x001A6C58 File Offset: 0x001A4E58
	public override Workable.AnimInfo GetAnim(WorkerBase worker)
	{
		KAnimFile[] overrideAnims = null;
		if (this.workerTypeOverrideAnims.TryGetValue(worker.PrefabID(), out overrideAnims))
		{
			this.overrideAnims = overrideAnims;
		}
		return base.GetAnim(worker);
	}

	// Token: 0x06004913 RID: 18707 RVA: 0x001A6C8C File Offset: 0x001A4E8C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_juicer_kanim")
		};
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		this.synchronizeAnims = false;
		base.SetWorkTime(30f);
		this.juicer = base.GetComponent<Juicer>();
	}

	// Token: 0x06004914 RID: 18708 RVA: 0x001A6CE9 File Offset: 0x001A4EE9
	protected override void OnStartWork(WorkerBase worker)
	{
		this.operational.SetActive(true, false);
	}

	// Token: 0x06004915 RID: 18709 RVA: 0x001A6CF8 File Offset: 0x001A4EF8
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Storage component = base.GetComponent<Storage>();
		float num;
		SimUtil.DiseaseInfo diseaseInfo;
		float num2;
		component.ConsumeAndGetDisease(GameTags.Water, this.juicer.waterMassPerUse, out num, out diseaseInfo, out num2);
		GermExposureMonitor.Instance smi = worker.GetSMI<GermExposureMonitor.Instance>();
		for (int i = 0; i < this.juicer.ingredientTags.Length; i++)
		{
			SimUtil.DiseaseInfo diseaseInfo2;
			component.ConsumeAndGetDisease(this.juicer.ingredientTags[i], this.juicer.ingredientMassesPerUse[i], out num, out diseaseInfo2, out num2);
			if (smi != null)
			{
				smi.TryInjectDisease(diseaseInfo2.idx, diseaseInfo2.count, this.juicer.ingredientTags[i], Sickness.InfectionVector.Digestion);
			}
		}
		if (smi != null)
		{
			smi.TryInjectDisease(diseaseInfo.idx, diseaseInfo.count, GameTags.Water, Sickness.InfectionVector.Digestion);
		}
		Effects component2 = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.juicer.specificEffect))
		{
			component2.Add(this.juicer.specificEffect, true);
		}
		if (!string.IsNullOrEmpty(this.juicer.trackingEffect))
		{
			component2.Add(this.juicer.trackingEffect, true);
		}
	}

	// Token: 0x06004916 RID: 18710 RVA: 0x001A6E15 File Offset: 0x001A5015
	protected override void OnStopWork(WorkerBase worker)
	{
		this.operational.SetActive(false, false);
	}

	// Token: 0x06004917 RID: 18711 RVA: 0x001A6E24 File Offset: 0x001A5024
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.juicer.trackingEffect) && component.HasEffect(this.juicer.trackingEffect))
		{
			priority = 0;
			return false;
		}
		if (!string.IsNullOrEmpty(this.juicer.specificEffect) && component.HasEffect(this.juicer.specificEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x04003093 RID: 12435
	public Dictionary<Tag, KAnimFile[]> workerTypeOverrideAnims = new Dictionary<Tag, KAnimFile[]>();

	// Token: 0x04003094 RID: 12436
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04003095 RID: 12437
	public int basePriority;

	// Token: 0x04003096 RID: 12438
	private Juicer juicer;
}
