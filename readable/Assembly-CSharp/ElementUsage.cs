using System;

// Token: 0x02000CD5 RID: 3285
public class ElementUsage
{
	// Token: 0x06006564 RID: 25956 RVA: 0x00262D48 File Offset: 0x00260F48
	public ElementUsage(Tag tag, float amount, bool continuous) : this(tag, amount, continuous, null)
	{
	}

	// Token: 0x06006565 RID: 25957 RVA: 0x00262D54 File Offset: 0x00260F54
	public ElementUsage(Tag tag, float amount, bool continuous, Func<Tag, float, bool, string> customFormating)
	{
		this.tag = tag;
		this.amount = amount;
		this.continuous = continuous;
		this.customFormating = customFormating;
	}

	// Token: 0x040044B8 RID: 17592
	public Tag tag;

	// Token: 0x040044B9 RID: 17593
	public float amount;

	// Token: 0x040044BA RID: 17594
	public bool continuous;

	// Token: 0x040044BB RID: 17595
	public Func<Tag, float, bool, string> customFormating;
}
