using System;

// Token: 0x0200035F RID: 863
public static class MathfExtensions
{
	// Token: 0x06001209 RID: 4617 RVA: 0x000693E9 File Offset: 0x000675E9
	public static long Max(this long a, long b)
	{
		if (a < b)
		{
			return b;
		}
		return a;
	}

	// Token: 0x0600120A RID: 4618 RVA: 0x000693F2 File Offset: 0x000675F2
	public static long Min(this long a, long b)
	{
		if (a > b)
		{
			return b;
		}
		return a;
	}
}
