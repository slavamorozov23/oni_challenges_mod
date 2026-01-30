using System;
using TUNING;
using UnityEngine;

// Token: 0x0200074D RID: 1869
[AddComponentMenu("KMonoBehaviour/Workable/EggIncubatorWorkable")]
public class EggIncubatorWorkable : Workable
{
	// Token: 0x06002F47 RID: 12103 RVA: 0x0011101C File Offset: 0x0010F21C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.synchronizeAnims = false;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_incubator_kanim")
		};
		base.SetWorkTime(15f);
		this.showProgressBar = true;
		this.requiredSkillPerk = Db.Get().SkillPerks.CanWrangleCreatures.Id;
		this.attributeConverter = Db.Get().AttributeConverters.RanchingEffectDuration;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.BARELY_EVER_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Ranching.Id;
		this.skillExperienceMultiplier = SKILLS.BARELY_EVER_EXPERIENCE;
	}

	// Token: 0x06002F48 RID: 12104 RVA: 0x001110C8 File Offset: 0x0010F2C8
	protected override void OnCompleteWork(WorkerBase worker)
	{
		EggIncubator component = base.GetComponent<EggIncubator>();
		if (component && component.Occupant)
		{
			IncubationMonitor.Instance smi = component.Occupant.GetSMI<IncubationMonitor.Instance>();
			if (smi != null)
			{
				smi.ApplySongBuff();
			}
		}
	}
}
