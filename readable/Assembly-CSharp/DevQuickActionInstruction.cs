using System;

// Token: 0x02000689 RID: 1673
public struct DevQuickActionInstruction
{
	// Token: 0x06002937 RID: 10551 RVA: 0x000EAC65 File Offset: 0x000E8E65
	public DevQuickActionInstruction(IDevQuickAction.CommonMenusNames category, string name, System.Action action)
	{
		this = new DevQuickActionInstruction(category.ToString() + "/" + name, action);
	}

	// Token: 0x06002938 RID: 10552 RVA: 0x000EAC86 File Offset: 0x000E8E86
	public DevQuickActionInstruction(string address, System.Action action)
	{
		this.Address = address;
		this.Action = action;
	}

	// Token: 0x04001855 RID: 6229
	public string Address;

	// Token: 0x04001856 RID: 6230
	public System.Action Action;
}
