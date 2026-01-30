using System;

// Token: 0x02000655 RID: 1621
public class ToiletTracker : WorldTracker
{
	// Token: 0x0600276D RID: 10093 RVA: 0x000E2B53 File Offset: 0x000E0D53
	public ToiletTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x0600276E RID: 10094 RVA: 0x000E2B5C File Offset: 0x000E0D5C
	public override void UpdateData()
	{
		throw new NotImplementedException();
	}

	// Token: 0x0600276F RID: 10095 RVA: 0x000E2B63 File Offset: 0x000E0D63
	public override string FormatValueString(float value)
	{
		return value.ToString();
	}
}
