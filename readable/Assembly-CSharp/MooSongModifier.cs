using System;

// Token: 0x020004EE RID: 1262
public class MooSongModifier : Resource
{
	// Token: 0x06001B3E RID: 6974 RVA: 0x00097183 File Offset: 0x00095383
	public MooSongModifier(string id, Tag targetTag, string name, string description, Func<string, string> tooltipCB, MooSongModifier.MooSongModFn applyFunction) : base(id, name)
	{
		this.Description = description;
		this.TargetTag = targetTag;
		this.TooltipCB = tooltipCB;
		this.ApplyFunction = applyFunction;
	}

	// Token: 0x06001B3F RID: 6975 RVA: 0x000971AC File Offset: 0x000953AC
	public string GetTooltip()
	{
		if (this.TooltipCB != null)
		{
			return this.TooltipCB(this.Description);
		}
		return this.Description;
	}

	// Token: 0x04000FC3 RID: 4035
	public string Description;

	// Token: 0x04000FC4 RID: 4036
	public Tag TargetTag;

	// Token: 0x04000FC5 RID: 4037
	public Func<string, string> TooltipCB;

	// Token: 0x04000FC6 RID: 4038
	public MooSongModifier.MooSongModFn ApplyFunction;

	// Token: 0x02001387 RID: 4999
	// (Invoke) Token: 0x06008C3B RID: 35899
	public delegate void MooSongModFn(BeckoningMonitor.Instance inst, Tag meteorTag);
}
