using System;

// Token: 0x020004E1 RID: 1249
public class GameplayEventMinionFilter
{
	// Token: 0x04000F8C RID: 3980
	public string id;

	// Token: 0x04000F8D RID: 3981
	public GameplayEventMinionFilter.FilterFn filter;

	// Token: 0x02001355 RID: 4949
	// (Invoke) Token: 0x06008B96 RID: 35734
	public delegate bool FilterFn(MinionIdentity minion);
}
