using System;

// Token: 0x0200094C RID: 2380
public class Face : Resource
{
	// Token: 0x06004250 RID: 16976 RVA: 0x00175F8D File Offset: 0x0017418D
	public Face(string id, string headFXSymbol = null) : base(id, null, null)
	{
		this.hash = new HashedString(id);
		this.headFXHash = headFXSymbol;
	}

	// Token: 0x040029A4 RID: 10660
	public HashedString hash;

	// Token: 0x040029A5 RID: 10661
	public HashedString headFXHash;

	// Token: 0x040029A6 RID: 10662
	private const string SYMBOL_PREFIX = "headfx_";
}
