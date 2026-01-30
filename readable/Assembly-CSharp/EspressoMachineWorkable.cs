using System;
using System.Collections.Generic;
using Klei;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x02000938 RID: 2360
[AddComponentMenu("KMonoBehaviour/Workable/EspressoMachineWorkable")]
public class EspressoMachineWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x06004204 RID: 16900 RVA: 0x001747B8 File Offset: 0x001729B8
	private EspressoMachineWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x06004205 RID: 16901 RVA: 0x001747E0 File Offset: 0x001729E0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_espresso_machine_kanim")
		};
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		this.synchronizeAnims = false;
		base.SetWorkTime(30f);
	}

	// Token: 0x06004206 RID: 16902 RVA: 0x00174834 File Offset: 0x00172A34
	public override Workable.AnimInfo GetAnim(WorkerBase worker)
	{
		KAnimFile[] overrideAnims = null;
		if (this.workerTypeOverrideAnims.TryGetValue(worker.PrefabID(), out overrideAnims))
		{
			this.overrideAnims = overrideAnims;
		}
		return base.GetAnim(worker);
	}

	// Token: 0x06004207 RID: 16903 RVA: 0x00174866 File Offset: 0x00172A66
	protected override void OnStartWork(WorkerBase worker)
	{
		this.operational.SetActive(true, false);
	}

	// Token: 0x06004208 RID: 16904 RVA: 0x00174878 File Offset: 0x00172A78
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Storage component = base.GetComponent<Storage>();
		float num;
		SimUtil.DiseaseInfo diseaseInfo;
		float num2;
		component.ConsumeAndGetDisease(GameTags.Water, EspressoMachine.WATER_MASS_PER_USE, out num, out diseaseInfo, out num2);
		SimUtil.DiseaseInfo diseaseInfo2;
		component.ConsumeAndGetDisease(EspressoMachine.INGREDIENT_TAG, EspressoMachine.INGREDIENT_MASS_PER_USE, out num, out diseaseInfo2, out num2);
		GermExposureMonitor.Instance smi = worker.GetSMI<GermExposureMonitor.Instance>();
		if (smi != null)
		{
			smi.TryInjectDisease(diseaseInfo.idx, diseaseInfo.count, GameTags.Water, Sickness.InfectionVector.Digestion);
			smi.TryInjectDisease(diseaseInfo2.idx, diseaseInfo2.count, EspressoMachine.INGREDIENT_TAG, Sickness.InfectionVector.Digestion);
		}
		Effects component2 = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(EspressoMachineWorkable.specificEffect))
		{
			component2.Add(EspressoMachineWorkable.specificEffect, true);
		}
		if (!string.IsNullOrEmpty(EspressoMachineWorkable.trackingEffect))
		{
			component2.Add(EspressoMachineWorkable.trackingEffect, true);
		}
	}

	// Token: 0x06004209 RID: 16905 RVA: 0x00174930 File Offset: 0x00172B30
	protected override void OnStopWork(WorkerBase worker)
	{
		this.operational.SetActive(false, false);
	}

	// Token: 0x0600420A RID: 16906 RVA: 0x00174940 File Offset: 0x00172B40
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(EspressoMachineWorkable.trackingEffect) && component.HasEffect(EspressoMachineWorkable.trackingEffect))
		{
			priority = 0;
			return false;
		}
		if (!string.IsNullOrEmpty(EspressoMachineWorkable.specificEffect) && component.HasEffect(EspressoMachineWorkable.specificEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x0400293A RID: 10554
	public Dictionary<Tag, KAnimFile[]> workerTypeOverrideAnims = new Dictionary<Tag, KAnimFile[]>();

	// Token: 0x0400293B RID: 10555
	[MyCmpReq]
	private Operational operational;

	// Token: 0x0400293C RID: 10556
	public int basePriority = RELAXATION.PRIORITY.TIER5;

	// Token: 0x0400293D RID: 10557
	private static string specificEffect = "Espresso";

	// Token: 0x0400293E RID: 10558
	private static string trackingEffect = "RecentlyRecDrink";
}
