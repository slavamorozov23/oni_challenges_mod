using System;

// Token: 0x02000664 RID: 1636
public struct DataPoint
{
	// Token: 0x060027A4 RID: 10148 RVA: 0x000E3716 File Offset: 0x000E1916
	public DataPoint(float start, float end, float value)
	{
		this.periodStart = start;
		this.periodEnd = end;
		this.periodValue = value;
	}

	// Token: 0x04001748 RID: 5960
	public float periodStart;

	// Token: 0x04001749 RID: 5961
	public float periodEnd;

	// Token: 0x0400174A RID: 5962
	public float periodValue;
}
