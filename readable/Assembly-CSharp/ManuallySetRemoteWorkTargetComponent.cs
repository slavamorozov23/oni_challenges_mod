using System;

// Token: 0x02000AD1 RID: 2769
public class ManuallySetRemoteWorkTargetComponent : RemoteDockWorkTargetComponent
{
	// Token: 0x17000583 RID: 1411
	// (get) Token: 0x0600508A RID: 20618 RVA: 0x001D37D6 File Offset: 0x001D19D6
	public override Chore RemoteDockChore
	{
		get
		{
			return this.chore;
		}
	}

	// Token: 0x0600508B RID: 20619 RVA: 0x001D37DE File Offset: 0x001D19DE
	public void SetChore(Chore chore_)
	{
		this.chore = chore_;
	}

	// Token: 0x040035BA RID: 13754
	private Chore chore;
}
