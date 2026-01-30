using System;
using TUNING;

// Token: 0x02000BDA RID: 3034
public class SplatWorkable : Workable
{
	// Token: 0x06005AE8 RID: 23272 RVA: 0x0020F040 File Offset: 0x0020D240
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetOffsetTable(OffsetGroups.InvertedStandardTableWithCorners);
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Mopping;
		this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Basekeeping.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.multitoolContext = "disinfect";
		this.multitoolHitEffectTag = "fx_disinfect_splash";
		this.synchronizeAnims = false;
		Prioritizable.AddRef(base.gameObject);
	}

	// Token: 0x06005AE9 RID: 23273 RVA: 0x0020F0EA File Offset: 0x0020D2EA
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.SetWorkTime(5f);
	}
}
