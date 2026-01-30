using System;

// Token: 0x02000365 RID: 869
public class MorbRoverMakerRevealWorkable : Workable
{
	// Token: 0x06001220 RID: 4640 RVA: 0x00069ADC File Offset: 0x00067CDC
	protected override void OnPrefabInit()
	{
		this.workAnims = new HashedString[]
		{
			"reveal_working_pre",
			"reveal_working_loop"
		};
		this.workingPstComplete = new HashedString[]
		{
			"reveal_working_pst"
		};
		this.workingPstFailed = new HashedString[]
		{
			"reveal_working_pst"
		};
		base.OnPrefabInit();
		this.workingStatusItem = Db.Get().BuildingStatusItems.MorbRoverMakerBuildingRevealed;
		base.SetWorkerStatusItem(Db.Get().DuplicantStatusItems.MorbRoverMakerWorkingOnRevealing);
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_gravitas_morb_tank_kanim")
		};
		this.lightEfficiencyBonus = true;
		this.synchronizeAnims = true;
		base.SetWorkTime(15f);
	}

	// Token: 0x06001221 RID: 4641 RVA: 0x00069BB8 File Offset: 0x00067DB8
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
	}

	// Token: 0x04000B6A RID: 2922
	public const string WORKABLE_PRE_ANIM_NAME = "reveal_working_pre";

	// Token: 0x04000B6B RID: 2923
	public const string WORKABLE_LOOP_ANIM_NAME = "reveal_working_loop";

	// Token: 0x04000B6C RID: 2924
	public const string WORKABLE_PST_ANIM_NAME = "reveal_working_pst";
}
