using System;
using KSerialization;

// Token: 0x02000681 RID: 1665
[SerializationConfig(MemberSerialization.OptIn)]
public class ScheduleGroupInstance
{
	// Token: 0x170001F9 RID: 505
	// (get) Token: 0x060028F3 RID: 10483 RVA: 0x000E9E94 File Offset: 0x000E8094
	// (set) Token: 0x060028F4 RID: 10484 RVA: 0x000E9EAB File Offset: 0x000E80AB
	public ScheduleGroup scheduleGroup
	{
		get
		{
			return Db.Get().ScheduleGroups.Get(this.scheduleGroupID);
		}
		set
		{
			this.scheduleGroupID = value.Id;
		}
	}

	// Token: 0x060028F5 RID: 10485 RVA: 0x000E9EB9 File Offset: 0x000E80B9
	public ScheduleGroupInstance(ScheduleGroup scheduleGroup)
	{
		this.scheduleGroup = scheduleGroup;
		this.segments = scheduleGroup.defaultSegments;
	}

	// Token: 0x04001835 RID: 6197
	[Serialize]
	private string scheduleGroupID;

	// Token: 0x04001836 RID: 6198
	[Serialize]
	public int segments;
}
