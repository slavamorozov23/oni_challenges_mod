using System;
using TUNING;
using UnityEngine;

// Token: 0x0200071C RID: 1820
[AddComponentMenu("KMonoBehaviour/Workable/DoctorChoreWorkable")]
public class DoctorChoreWorkable : Workable
{
	// Token: 0x06002D74 RID: 11636 RVA: 0x0010767A File Offset: 0x0010587A
	private DoctorChoreWorkable()
	{
		this.synchronizeAnims = false;
	}

	// Token: 0x06002D75 RID: 11637 RVA: 0x0010768C File Offset: 0x0010588C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.attributeConverter = Db.Get().AttributeConverters.DoctorSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.BARELY_EVER_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.MedicalAid.Id;
		this.skillExperienceMultiplier = SKILLS.BARELY_EVER_EXPERIENCE;
	}
}
