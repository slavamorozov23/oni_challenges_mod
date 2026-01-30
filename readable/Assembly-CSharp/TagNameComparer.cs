using System;
using System.Collections.Generic;

// Token: 0x02000965 RID: 2405
public class TagNameComparer : IComparer<Tag>
{
	// Token: 0x06004381 RID: 17281 RVA: 0x00183781 File Offset: 0x00181981
	public TagNameComparer()
	{
	}

	// Token: 0x06004382 RID: 17282 RVA: 0x00183789 File Offset: 0x00181989
	public TagNameComparer(Tag firstTag)
	{
		this.firstTag = firstTag;
	}

	// Token: 0x06004383 RID: 17283 RVA: 0x00183798 File Offset: 0x00181998
	public int Compare(Tag x, Tag y)
	{
		if (x == y)
		{
			return 0;
		}
		if (this.firstTag.IsValid)
		{
			if (x == this.firstTag && y != this.firstTag)
			{
				return 1;
			}
			if (x != this.firstTag && y == this.firstTag)
			{
				return -1;
			}
		}
		return x.ProperNameStripLink().CompareTo(y.ProperNameStripLink());
	}

	// Token: 0x04002CAD RID: 11437
	private Tag firstTag;
}
