using System;
using TUNING;

// Token: 0x0200021D RID: 541
public abstract class FossilExcavationWorkable : Workable
{
	// Token: 0x06000AF7 RID: 2807
	protected abstract bool IsMarkedForExcavation();

	// Token: 0x06000AF8 RID: 2808 RVA: 0x00042238 File Offset: 0x00040438
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workingStatusItem = Db.Get().BuildingStatusItems.FossilHuntExcavationInProgress;
		base.SetWorkerStatusItem(Db.Get().DuplicantStatusItems.FossilHunt_WorkerExcavating);
		this.requiredSkillPerk = Db.Get().SkillPerks.CanArtGreat.Id;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_fossils_small_kanim")
		};
		this.attributeConverter = Db.Get().AttributeConverters.ArtSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.BARELY_EVER_EXPERIENCE;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.lightEfficiencyBonus = true;
		this.synchronizeAnims = false;
		this.shouldShowSkillPerkStatusItem = false;
	}

	// Token: 0x06000AF9 RID: 2809 RVA: 0x000422F0 File Offset: 0x000404F0
	protected override void UpdateStatusItem(object data = null)
	{
		base.UpdateStatusItem(data);
		KSelectable component = base.GetComponent<KSelectable>();
		if (this.waitingWorkStatusItemHandle != default(Guid))
		{
			component.RemoveStatusItem(this.waitingWorkStatusItemHandle, false);
		}
		if (base.worker == null && this.IsMarkedForExcavation())
		{
			this.waitingWorkStatusItemHandle = component.AddStatusItem(this.waitingForExcavationWorkStatusItem, null);
		}
	}

	// Token: 0x040007B5 RID: 1973
	protected Guid waitingWorkStatusItemHandle;

	// Token: 0x040007B6 RID: 1974
	protected StatusItem waitingForExcavationWorkStatusItem = Db.Get().BuildingStatusItems.FossilHuntExcavationOrdered;
}
