using System;
using KSerialization;

// Token: 0x02000A88 RID: 2696
[SerializationConfig(MemberSerialization.OptIn)]
public class Ownables : Assignables
{
	// Token: 0x06004E4A RID: 20042 RVA: 0x001C7ACB File Offset: 0x001C5CCB
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06004E4B RID: 20043 RVA: 0x001C7AD4 File Offset: 0x001C5CD4
	public void UnassignAll()
	{
		foreach (AssignableSlotInstance assignableSlotInstance in this.slots)
		{
			if (assignableSlotInstance.assignable != null)
			{
				assignableSlotInstance.assignable.Unassign();
			}
		}
	}
}
