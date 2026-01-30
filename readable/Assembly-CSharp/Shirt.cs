using System;

// Token: 0x02000B46 RID: 2886
public class Shirt : Resource
{
	// Token: 0x060054F2 RID: 21746 RVA: 0x001EF9B3 File Offset: 0x001EDBB3
	public Shirt(string id) : base(id, null, null)
	{
		this.hash = new HashedString(id);
	}

	// Token: 0x0400395E RID: 14686
	public HashedString hash;
}
