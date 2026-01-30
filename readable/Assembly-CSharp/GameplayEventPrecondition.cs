using System;

// Token: 0x020004E3 RID: 1251
public class GameplayEventPrecondition
{
	// Token: 0x04000F8F RID: 3983
	public string description;

	// Token: 0x04000F90 RID: 3984
	public GameplayEventPrecondition.PreconditionFn condition;

	// Token: 0x04000F91 RID: 3985
	public bool required;

	// Token: 0x04000F92 RID: 3986
	public int priorityModifier;

	// Token: 0x0200135D RID: 4957
	// (Invoke) Token: 0x06008BA9 RID: 35753
	public delegate bool PreconditionFn();
}
