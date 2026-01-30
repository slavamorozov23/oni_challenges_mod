using System;
using UnityEngine.Pool;

// Token: 0x020009E8 RID: 2536
public class LogicValueChanged
{
	// Token: 0x0400311B RID: 12571
	public HashedString portID;

	// Token: 0x0400311C RID: 12572
	public int newValue;

	// Token: 0x0400311D RID: 12573
	public int prevValue;

	// Token: 0x0400311E RID: 12574
	public static ObjectPool<LogicValueChanged> Pool = new ObjectPool<LogicValueChanged>(() => new LogicValueChanged(), null, null, null, false, 1, 8);
}
