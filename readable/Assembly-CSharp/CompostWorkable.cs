using System;
using TUNING;
using UnityEngine;

// Token: 0x02000725 RID: 1829
[AddComponentMenu("KMonoBehaviour/Workable/CompostWorkable")]
public class CompostWorkable : Workable
{
	// Token: 0x06002DF7 RID: 11767 RVA: 0x0010B140 File Offset: 0x00109340
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Basekeeping.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
	}

	// Token: 0x06002DF8 RID: 11768 RVA: 0x0010B198 File Offset: 0x00109398
	protected override void OnStartWork(WorkerBase worker)
	{
	}

	// Token: 0x06002DF9 RID: 11769 RVA: 0x0010B19A File Offset: 0x0010939A
	protected override void OnStopWork(WorkerBase worker)
	{
	}
}
