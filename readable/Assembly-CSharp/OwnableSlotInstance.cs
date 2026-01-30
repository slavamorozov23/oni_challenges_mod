using System;
using System.Diagnostics;

// Token: 0x02000A87 RID: 2695
[DebuggerDisplay("{slot.Id}")]
public class OwnableSlotInstance : AssignableSlotInstance
{
	// Token: 0x06004E49 RID: 20041 RVA: 0x001C7AC1 File Offset: 0x001C5CC1
	public OwnableSlotInstance(Assignables assignables, OwnableSlot slot) : base(assignables, slot)
	{
	}
}
