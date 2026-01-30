using System;
using TUNING;

// Token: 0x020007DA RID: 2010
public class PartyCakeWorkable : Workable
{
	// Token: 0x0600355D RID: 13661 RVA: 0x0012D6C4 File Offset: 0x0012B8C4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Cooking;
		this.alwaysShowProgressBar = true;
		this.resetProgressOnStop = false;
		this.attributeConverter = Db.Get().AttributeConverters.CookingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_desalinator_kanim")
		};
		this.workAnims = PartyCakeWorkable.WORK_ANIMS;
		this.workingPstComplete = new HashedString[]
		{
			PartyCakeWorkable.PST_ANIM
		};
		this.workingPstFailed = new HashedString[]
		{
			PartyCakeWorkable.PST_ANIM
		};
		this.synchronizeAnims = false;
	}

	// Token: 0x0600355E RID: 13662 RVA: 0x0012D77A File Offset: 0x0012B97A
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		base.OnWorkTick(worker, dt);
		base.GetComponent<KBatchedAnimController>().SetPositionPercent(this.GetPercentComplete());
		return false;
	}

	// Token: 0x04002048 RID: 8264
	private static readonly HashedString[] WORK_ANIMS = new HashedString[]
	{
		"salt_pre",
		"salt_loop"
	};

	// Token: 0x04002049 RID: 8265
	private static readonly HashedString PST_ANIM = new HashedString("salt_pst");
}
