using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020006DC RID: 1756
[DebuggerDisplay("{Id}")]
[Serializable]
public class AssignableSlot : Resource
{
	// Token: 0x06002B37 RID: 11063 RVA: 0x000FCA81 File Offset: 0x000FAC81
	public AssignableSlot(string id, string name, bool showInUI = true) : base(id, name)
	{
		this.showInUI = showInUI;
	}

	// Token: 0x06002B38 RID: 11064 RVA: 0x000FCA9C File Offset: 0x000FAC9C
	public AssignableSlotInstance Lookup(GameObject go)
	{
		Assignables component = go.GetComponent<Assignables>();
		if (component != null)
		{
			return component.GetSlot(this);
		}
		return null;
	}

	// Token: 0x040019D8 RID: 6616
	public bool showInUI = true;
}
