using System;

// Token: 0x020005F7 RID: 1527
public class KSelectableProgressBar : KSelectable
{
	// Token: 0x06002374 RID: 9076 RVA: 0x000CD018 File Offset: 0x000CB218
	public override string GetName()
	{
		int num = (int)(this.progressBar.PercentFull * (float)this.scaleAmount);
		return string.Format("{0} {1}/{2}", this.entityName, num, this.scaleAmount);
	}

	// Token: 0x040014A9 RID: 5289
	[MyCmpGet]
	private ProgressBar progressBar;

	// Token: 0x040014AA RID: 5290
	private int scaleAmount = 100;
}
