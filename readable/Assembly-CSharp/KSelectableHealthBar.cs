using System;

// Token: 0x020005F6 RID: 1526
public class KSelectableHealthBar : KSelectable
{
	// Token: 0x06002372 RID: 9074 RVA: 0x000CCFC4 File Offset: 0x000CB1C4
	public override string GetName()
	{
		int num = (int)(this.progressBar.PercentFull * (float)this.scaleAmount);
		return string.Format("{0} {1}/{2}", this.entityName, num, this.scaleAmount);
	}

	// Token: 0x040014A7 RID: 5287
	[MyCmpGet]
	private ProgressBar progressBar;

	// Token: 0x040014A8 RID: 5288
	private int scaleAmount = 100;
}
