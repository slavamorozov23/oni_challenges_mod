using System;
using UnityEngine;

// Token: 0x02000789 RID: 1929
[AddComponentMenu("KMonoBehaviour/Workable/LiquidCooledFanWorkable")]
public class LiquidCooledFanWorkable : Workable
{
	// Token: 0x0600313F RID: 12607 RVA: 0x0011C1FB File Offset: 0x0011A3FB
	private LiquidCooledFanWorkable()
	{
		this.showProgressBar = false;
	}

	// Token: 0x06003140 RID: 12608 RVA: 0x0011C20A File Offset: 0x0011A40A
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = null;
	}

	// Token: 0x06003141 RID: 12609 RVA: 0x0011C219 File Offset: 0x0011A419
	protected override void OnSpawn()
	{
		GameScheduler.Instance.Schedule("InsulationTutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Insulation, true);
		}, null, null);
		base.OnSpawn();
	}

	// Token: 0x06003142 RID: 12610 RVA: 0x0011C257 File Offset: 0x0011A457
	protected override void OnStartWork(WorkerBase worker)
	{
		this.operational.SetActive(true, false);
	}

	// Token: 0x06003143 RID: 12611 RVA: 0x0011C266 File Offset: 0x0011A466
	protected override void OnStopWork(WorkerBase worker)
	{
		this.operational.SetActive(false, false);
	}

	// Token: 0x06003144 RID: 12612 RVA: 0x0011C275 File Offset: 0x0011A475
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.operational.SetActive(false, false);
	}

	// Token: 0x04001D93 RID: 7571
	[MyCmpGet]
	private Operational operational;
}
