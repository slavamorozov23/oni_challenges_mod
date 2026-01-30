using System;
using TUNING;

// Token: 0x02000708 RID: 1800
public class Apothecary : ComplexFabricator
{
	// Token: 0x06002C96 RID: 11414 RVA: 0x001037F1 File Offset: 0x001019F1
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.choreType = Db.Get().ChoreTypes.Compound;
		this.fetchChoreTypeIdHash = Db.Get().ChoreTypes.DoctorFetch.IdHash;
		this.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
	}

	// Token: 0x06002C97 RID: 11415 RVA: 0x00103830 File Offset: 0x00101A30
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
			Assets.GetAnim("anim_interacts_apothecary_kanim")
		};
	}
}
