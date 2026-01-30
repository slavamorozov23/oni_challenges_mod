using System;

// Token: 0x020009E4 RID: 2532
public class ExecutableSpecificString
{
	// Token: 0x060049B8 RID: 18872 RVA: 0x001AB867 File Offset: 0x001A9A67
	public ExecutableSpecificString(string baseStr, string soStr)
	{
		this.baseString = baseStr;
		this.soString = soStr;
	}

	// Token: 0x060049B9 RID: 18873 RVA: 0x001AB87D File Offset: 0x001A9A7D
	public static implicit operator string(ExecutableSpecificString dualString)
	{
		if (!DlcManager.IsExpansion1Active())
		{
			return dualString.baseString;
		}
		return dualString.soString;
	}

	// Token: 0x060049BA RID: 18874 RVA: 0x001AB893 File Offset: 0x001A9A93
	public static implicit operator LocString(ExecutableSpecificString dualString)
	{
		return new LocString(dualString);
	}

	// Token: 0x04003106 RID: 12550
	private string baseString;

	// Token: 0x04003107 RID: 12551
	private string soString;
}
