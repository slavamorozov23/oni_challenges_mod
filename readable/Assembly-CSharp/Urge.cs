using System;

// Token: 0x020004D9 RID: 1241
public class Urge : Resource
{
	// Token: 0x06001ABD RID: 6845 RVA: 0x000937FC File Offset: 0x000919FC
	public Urge(string id) : base(id, null, null)
	{
	}

	// Token: 0x06001ABE RID: 6846 RVA: 0x00093807 File Offset: 0x00091A07
	public override string ToString()
	{
		return this.Id;
	}
}
