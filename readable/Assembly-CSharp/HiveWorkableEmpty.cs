using System;
using TUNING;
using UnityEngine;

// Token: 0x0200077C RID: 1916
[AddComponentMenu("KMonoBehaviour/Workable/HiveWorkableEmpty")]
public class HiveWorkableEmpty : Workable
{
	// Token: 0x060030D6 RID: 12502 RVA: 0x00119DE4 File Offset: 0x00117FE4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Emptying;
		this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Basekeeping.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.workAnims = HiveWorkableEmpty.WORK_ANIMS;
		this.workingPstComplete = new HashedString[]
		{
			HiveWorkableEmpty.PST_ANIM
		};
		this.workingPstFailed = new HashedString[]
		{
			HiveWorkableEmpty.PST_ANIM
		};
	}

	// Token: 0x060030D7 RID: 12503 RVA: 0x00119E8C File Offset: 0x0011808C
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		if (!this.wasStung)
		{
			SaveGame.Instance.ColonyAchievementTracker.harvestAHiveWithoutGettingStung = true;
		}
	}

	// Token: 0x04001D35 RID: 7477
	private static readonly HashedString[] WORK_ANIMS = new HashedString[]
	{
		"working_pre",
		"working_loop"
	};

	// Token: 0x04001D36 RID: 7478
	private static readonly HashedString PST_ANIM = new HashedString("working_pst");

	// Token: 0x04001D37 RID: 7479
	public bool wasStung;
}
