using System;
using TUNING;

// Token: 0x02000364 RID: 868
public class MorbRoverMakerWorkable : Workable
{
	// Token: 0x0600121C RID: 4636 RVA: 0x000699FC File Offset: 0x00067BFC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workingStatusItem = Db.Get().BuildingStatusItems.MorbRoverMakerDoctorWorking;
		base.SetWorkerStatusItem(Db.Get().DuplicantStatusItems.MorbRoverMakerDoctorWorking);
		this.requiredSkillPerk = Db.Get().SkillPerks.CanAdvancedMedicine.Id;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_gravitas_morb_tank_kanim")
		};
		this.attributeConverter = Db.Get().AttributeConverters.DoctorSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.BARELY_EVER_EXPERIENCE;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.lightEfficiencyBonus = true;
		this.synchronizeAnims = true;
		this.shouldShowSkillPerkStatusItem = true;
		base.SetWorkTime(90f);
		this.resetProgressOnStop = true;
	}

	// Token: 0x0600121D RID: 4637 RVA: 0x00069AC3 File Offset: 0x00067CC3
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x0600121E RID: 4638 RVA: 0x00069ACB File Offset: 0x00067CCB
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
	}

	// Token: 0x04000B69 RID: 2921
	public const float DOCTOR_WORKING_TIME = 90f;
}
