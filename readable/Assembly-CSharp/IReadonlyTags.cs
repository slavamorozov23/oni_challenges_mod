using System;

// Token: 0x02000BEF RID: 3055
public interface IReadonlyTags
{
	// Token: 0x06005BB7 RID: 23479
	bool HasTag(string tag);

	// Token: 0x06005BB8 RID: 23480
	bool HasTag(int hashtag);

	// Token: 0x06005BB9 RID: 23481
	bool HasTags(int[] tags);
}
