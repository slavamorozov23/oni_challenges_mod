using System;
using TUNING;

// Token: 0x02000B0E RID: 2830
public class ArmTrapWorkable : Workable
{
	// Token: 0x06005282 RID: 21122 RVA: 0x001E0000 File Offset: 0x001DE200
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.CanBeArmedAtLongDistance)
		{
			base.SetOffsetTable(OffsetGroups.InvertedStandardTableWithCorners);
			this.faceTargetWhenWorking = true;
			this.multitoolContext = "build";
			this.multitoolHitEffectTag = EffectConfigs.BuildSplashId;
		}
		if (this.initialOffsets != null && this.initialOffsets.Length != 0)
		{
			base.SetOffsets(this.initialOffsets);
		}
		base.SetWorkerStatusItem(Db.Get().DuplicantStatusItems.ArmingTrap);
		this.requiredSkillPerk = Db.Get().SkillPerks.CanWrangleCreatures.Id;
		this.attributeConverter = Db.Get().AttributeConverters.CapturableSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		base.SetWorkTime(5f);
		this.synchronizeAnims = true;
		this.resetProgressOnStop = true;
	}

	// Token: 0x06005283 RID: 21123 RVA: 0x001E00DD File Offset: 0x001DE2DD
	public override void OnPendingCompleteWork(WorkerBase worker)
	{
		base.OnPendingCompleteWork(worker);
		this.WorkInPstAnimation = true;
		base.gameObject.Trigger(-2025798095, null);
	}

	// Token: 0x06005284 RID: 21124 RVA: 0x001E00FE File Offset: 0x001DE2FE
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		this.WorkInPstAnimation = false;
	}

	// Token: 0x040037BD RID: 14269
	public bool WorkInPstAnimation;

	// Token: 0x040037BE RID: 14270
	public bool CanBeArmedAtLongDistance;

	// Token: 0x040037BF RID: 14271
	public CellOffset[] initialOffsets;
}
