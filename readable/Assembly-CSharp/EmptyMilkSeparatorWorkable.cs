using System;
using TUNING;

// Token: 0x020007BE RID: 1982
public class EmptyMilkSeparatorWorkable : Workable
{
	// Token: 0x06003476 RID: 13430 RVA: 0x001293A4 File Offset: 0x001275A4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workLayer = Grid.SceneLayer.BuildingFront;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Cleaning;
		this.workingStatusItem = Db.Get().MiscStatusItems.Cleaning;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_milk_separator_kanim")
		};
		this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		base.SetWorkTime(15f);
		this.synchronizeAnims = true;
	}

	// Token: 0x06003477 RID: 13431 RVA: 0x00129444 File Offset: 0x00127644
	public override void OnPendingCompleteWork(WorkerBase worker)
	{
		System.Action onWork_PST_Begins = this.OnWork_PST_Begins;
		if (onWork_PST_Begins != null)
		{
			onWork_PST_Begins();
		}
		base.OnPendingCompleteWork(worker);
	}

	// Token: 0x04001FA2 RID: 8098
	public System.Action OnWork_PST_Begins;
}
