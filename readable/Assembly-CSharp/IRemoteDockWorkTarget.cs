using System;

// Token: 0x02000ACE RID: 2766
public interface IRemoteDockWorkTarget
{
	// Token: 0x1700057D RID: 1405
	// (get) Token: 0x0600507E RID: 20606
	Chore RemoteDockChore { get; }

	// Token: 0x1700057E RID: 1406
	// (get) Token: 0x0600507F RID: 20607
	IApproachable Approachable { get; }
}
