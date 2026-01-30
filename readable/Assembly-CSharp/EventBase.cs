using System;

// Token: 0x02000943 RID: 2371
public class EventBase : Resource
{
	// Token: 0x06004229 RID: 16937 RVA: 0x0017562B File Offset: 0x0017382B
	public EventBase(string id) : base(id, id)
	{
		this.hash = Hash.SDBMLower(id);
	}

	// Token: 0x0600422A RID: 16938 RVA: 0x00175641 File Offset: 0x00173841
	public virtual string GetDescription(EventInstanceBase ev)
	{
		return "";
	}

	// Token: 0x04002985 RID: 10629
	public int hash;
}
