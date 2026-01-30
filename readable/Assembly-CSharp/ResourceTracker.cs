using System;

// Token: 0x02000656 RID: 1622
public class ResourceTracker : WorldTracker
{
	// Token: 0x170001C9 RID: 457
	// (get) Token: 0x06002770 RID: 10096 RVA: 0x000E2B6C File Offset: 0x000E0D6C
	// (set) Token: 0x06002771 RID: 10097 RVA: 0x000E2B74 File Offset: 0x000E0D74
	public Tag tag { get; private set; }

	// Token: 0x06002772 RID: 10098 RVA: 0x000E2B7D File Offset: 0x000E0D7D
	public ResourceTracker(int worldID, Tag materialCategoryTag) : base(worldID)
	{
		this.tag = materialCategoryTag;
	}

	// Token: 0x06002773 RID: 10099 RVA: 0x000E2B90 File Offset: 0x000E0D90
	public override void UpdateData()
	{
		if (ClusterManager.Instance.GetWorld(base.WorldID).worldInventory == null)
		{
			return;
		}
		base.AddPoint(ClusterManager.Instance.GetWorld(base.WorldID).worldInventory.GetAmount(this.tag, false));
	}

	// Token: 0x06002774 RID: 10100 RVA: 0x000E2BE2 File Offset: 0x000E0DE2
	public override string FormatValueString(float value)
	{
		return GameUtil.GetFormattedMass(value, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
	}
}
