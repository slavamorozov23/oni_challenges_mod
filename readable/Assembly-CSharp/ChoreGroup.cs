using System;
using System.Collections.Generic;
using System.Diagnostics;
using Klei.AI;

// Token: 0x0200067A RID: 1658
[DebuggerDisplay("{IdHash}")]
public class ChoreGroup : Resource
{
	// Token: 0x170001DC RID: 476
	// (get) Token: 0x060028AB RID: 10411 RVA: 0x000E973C File Offset: 0x000E793C
	public int DefaultPersonalPriority
	{
		get
		{
			return this.defaultPersonalPriority;
		}
	}

	// Token: 0x060028AC RID: 10412 RVA: 0x000E9744 File Offset: 0x000E7944
	public ChoreGroup(string id, string name, Klei.AI.Attribute attribute, string sprite, int default_personal_priority, bool user_prioritizable = true) : base(id, name)
	{
		this.attribute = attribute;
		this.description = Strings.Get("STRINGS.DUPLICANTS.CHOREGROUPS." + id.ToUpper() + ".DESC").String;
		this.sprite = sprite;
		this.defaultPersonalPriority = default_personal_priority;
		this.userPrioritizable = user_prioritizable;
	}

	// Token: 0x04001801 RID: 6145
	public List<ChoreType> choreTypes = new List<ChoreType>();

	// Token: 0x04001802 RID: 6146
	public Klei.AI.Attribute attribute;

	// Token: 0x04001803 RID: 6147
	public string description;

	// Token: 0x04001804 RID: 6148
	public string sprite;

	// Token: 0x04001805 RID: 6149
	private int defaultPersonalPriority;

	// Token: 0x04001806 RID: 6150
	public bool userPrioritizable;
}
