using System;
using TUNING;

// Token: 0x020001BB RID: 443
public class SpaceTreeSyrupHarvestWorkable : Workable
{
	// Token: 0x060008F4 RID: 2292 RVA: 0x0003C970 File Offset: 0x0003AB70
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetWorkerStatusItem(Db.Get().DuplicantStatusItems.Harvesting);
		this.workAnims = new HashedString[]
		{
			"syrup_harvest_trunk_pre",
			"syrup_harvest_trunk_loop"
		};
		this.workingPstComplete = new HashedString[]
		{
			"syrup_harvest_trunk_pst"
		};
		this.workingPstFailed = new HashedString[]
		{
			"syrup_harvest_trunk_loop"
		};
		this.attributeConverter = Db.Get().AttributeConverters.HarvestSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Farming.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.requiredSkillPerk = Db.Get().SkillPerks.CanFarmTinker.Id;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_syrup_tree_kanim")
		};
		this.synchronizeAnims = true;
		this.shouldShowSkillPerkStatusItem = false;
		base.SetWorkTime(10f);
		this.resetProgressOnStop = true;
	}

	// Token: 0x060008F5 RID: 2293 RVA: 0x0003CA9D File Offset: 0x0003AC9D
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x060008F6 RID: 2294 RVA: 0x0003CAA5 File Offset: 0x0003ACA5
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
	}
}
