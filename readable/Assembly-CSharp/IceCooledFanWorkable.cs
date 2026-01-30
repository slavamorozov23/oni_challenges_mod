using System;
using TUNING;
using UnityEngine;

// Token: 0x0200077D RID: 1917
[AddComponentMenu("KMonoBehaviour/Workable/IceCooledFanWorkable")]
public class IceCooledFanWorkable : Workable
{
	// Token: 0x060030DA RID: 12506 RVA: 0x00119EF3 File Offset: 0x001180F3
	private IceCooledFanWorkable()
	{
		this.showProgressBar = false;
	}

	// Token: 0x060030DB RID: 12507 RVA: 0x00119F04 File Offset: 0x00118104
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.attributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
		this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
		this.workerStatusItem = null;
	}

	// Token: 0x060030DC RID: 12508 RVA: 0x00119F63 File Offset: 0x00118163
	protected override void OnSpawn()
	{
		GameScheduler.Instance.Schedule("InsulationTutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Insulation, true);
		}, null, null);
		base.OnSpawn();
	}

	// Token: 0x060030DD RID: 12509 RVA: 0x00119FA1 File Offset: 0x001181A1
	protected override void OnStartWork(WorkerBase worker)
	{
		this.operational.SetActive(true, false);
	}

	// Token: 0x060030DE RID: 12510 RVA: 0x00119FB0 File Offset: 0x001181B0
	protected override void OnStopWork(WorkerBase worker)
	{
		this.operational.SetActive(false, false);
	}

	// Token: 0x060030DF RID: 12511 RVA: 0x00119FBF File Offset: 0x001181BF
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.operational.SetActive(false, false);
	}

	// Token: 0x04001D38 RID: 7480
	[MyCmpGet]
	private Operational operational;
}
