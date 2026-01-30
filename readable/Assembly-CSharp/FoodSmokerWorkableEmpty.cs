using System;
using TUNING;

// Token: 0x02000761 RID: 1889
public class FoodSmokerWorkableEmpty : Workable
{
	// Token: 0x06002FCE RID: 12238 RVA: 0x0011427C File Offset: 0x0011247C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Emptying;
		this.workAnims = FoodSmokerWorkableEmpty.WORK_ANIMS;
		this.workingPstComplete = FoodSmokerWorkableEmpty.WORK_ANIMS_PST;
		this.workingPstFailed = FoodSmokerWorkableEmpty.WORK_ANIMS_FAIL_PST;
		this.requiredSkillPerk = Db.Get().SkillPerks.CanGasRange.Id;
		this.attributeConverter = Db.Get().AttributeConverters.CookingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.FULL_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Cooking.Id;
		this.skillExperienceMultiplier = SKILLS.FULL_EXPERIENCE;
	}

	// Token: 0x04001C7E RID: 7294
	private static readonly HashedString[] WORK_ANIMS = new HashedString[]
	{
		"empty_pre",
		"empty_loop"
	};

	// Token: 0x04001C7F RID: 7295
	private static readonly HashedString[] WORK_ANIMS_PST = new HashedString[]
	{
		"empty_pst"
	};

	// Token: 0x04001C80 RID: 7296
	private static readonly HashedString[] WORK_ANIMS_FAIL_PST = new HashedString[]
	{
		""
	};
}
