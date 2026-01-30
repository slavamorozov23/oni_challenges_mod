using System;

// Token: 0x02000662 RID: 1634
public abstract class MinionTracker : Tracker
{
	// Token: 0x06002796 RID: 10134 RVA: 0x000E31AD File Offset: 0x000E13AD
	public MinionTracker(MinionIdentity identity)
	{
		this.identity = identity;
	}

	// Token: 0x04001742 RID: 5954
	public MinionIdentity identity;
}
