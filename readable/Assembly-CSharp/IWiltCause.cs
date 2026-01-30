using System;

// Token: 0x020008C5 RID: 2245
public interface IWiltCause
{
	// Token: 0x17000446 RID: 1094
	// (get) Token: 0x06003DF3 RID: 15859
	string WiltStateString { get; }

	// Token: 0x17000447 RID: 1095
	// (get) Token: 0x06003DF4 RID: 15860
	WiltCondition.Condition[] Conditions { get; }
}
