using System;

// Token: 0x0200075E RID: 1886
public class FoodDehydratorWorkableEmpty : Workable
{
	// Token: 0x06002FC4 RID: 12228 RVA: 0x00113D61 File Offset: 0x00111F61
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Emptying;
		this.workAnims = FoodDehydratorWorkableEmpty.WORK_ANIMS;
		this.workingPstComplete = FoodDehydratorWorkableEmpty.WORK_ANIMS_PST;
		this.workingPstFailed = FoodDehydratorWorkableEmpty.WORK_ANIMS_FAIL_PST;
	}

	// Token: 0x04001C74 RID: 7284
	private static readonly HashedString[] WORK_ANIMS = new HashedString[]
	{
		"empty_pre",
		"empty_loop"
	};

	// Token: 0x04001C75 RID: 7285
	private static readonly HashedString[] WORK_ANIMS_PST = new HashedString[]
	{
		"empty_pst"
	};

	// Token: 0x04001C76 RID: 7286
	private static readonly HashedString[] WORK_ANIMS_FAIL_PST = new HashedString[]
	{
		""
	};
}
