using System;
using TUNING;

// Token: 0x02000703 RID: 1795
public class AdvancedApothecary : ComplexFabricator
{
	// Token: 0x06002C7A RID: 11386 RVA: 0x00102C8D File Offset: 0x00100E8D
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.choreType = Db.Get().ChoreTypes.Compound;
		this.fetchChoreTypeIdHash = Db.Get().ChoreTypes.DoctorFetch.IdHash;
		this.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
	}

	// Token: 0x06002C7B RID: 11387 RVA: 0x00102CCC File Offset: 0x00100ECC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.workable.WorkerStatusItem = Db.Get().DuplicantStatusItems.Fabricating;
		this.workable.AttributeConverter = Db.Get().AttributeConverters.CompoundingSpeed;
		this.workable.SkillExperienceSkillGroup = Db.Get().SkillGroups.MedicalAid.Id;
		this.workable.SkillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.workable.requiredSkillPerk = Db.Get().SkillPerks.CanCompound.Id;
		this.workable.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_medicine_nuclear_kanim")
		};
	}
}
