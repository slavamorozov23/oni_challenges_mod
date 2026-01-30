using System;
using System.Collections.Generic;
using KSerialization;

// Token: 0x02000B33 RID: 2867
[Serializable]
public class ScheduleBlock
{
	// Token: 0x170005E9 RID: 1513
	// (get) Token: 0x0600546A RID: 21610 RVA: 0x001ED473 File Offset: 0x001EB673
	public List<ScheduleBlockType> allowed_types
	{
		get
		{
			Debug.Assert(!string.IsNullOrEmpty(this._groupId));
			return Db.Get().ScheduleGroups.Get(this._groupId).allowedTypes;
		}
	}

	// Token: 0x170005EA RID: 1514
	// (get) Token: 0x0600546C RID: 21612 RVA: 0x001ED4AB File Offset: 0x001EB6AB
	// (set) Token: 0x0600546B RID: 21611 RVA: 0x001ED4A2 File Offset: 0x001EB6A2
	public string GroupId
	{
		get
		{
			return this._groupId;
		}
		set
		{
			this._groupId = value;
		}
	}

	// Token: 0x0600546D RID: 21613 RVA: 0x001ED4B3 File Offset: 0x001EB6B3
	public ScheduleBlock(string name, string groupId)
	{
		this.name = name;
		this._groupId = groupId;
	}

	// Token: 0x0600546E RID: 21614 RVA: 0x001ED4CC File Offset: 0x001EB6CC
	public bool IsAllowed(ScheduleBlockType type)
	{
		if (this.allowed_types != null)
		{
			foreach (ScheduleBlockType scheduleBlockType in this.allowed_types)
			{
				if (type.IdHash == scheduleBlockType.IdHash)
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	// Token: 0x04003907 RID: 14599
	[Serialize]
	public string name;

	// Token: 0x04003908 RID: 14600
	[Serialize]
	private string _groupId;
}
