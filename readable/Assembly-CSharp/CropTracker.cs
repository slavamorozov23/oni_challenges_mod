using System;

// Token: 0x02000653 RID: 1619
public class CropTracker : WorldTracker
{
	// Token: 0x06002767 RID: 10087 RVA: 0x000E29B3 File Offset: 0x000E0BB3
	public CropTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x06002768 RID: 10088 RVA: 0x000E29BC File Offset: 0x000E0BBC
	public override void UpdateData()
	{
		float num = 0f;
		foreach (PlantablePlot plantablePlot in Components.PlantablePlots.GetItems(base.WorldID))
		{
			if (!(plantablePlot.plant == null) && plantablePlot.HasDepositTag(GameTags.CropSeed) && !plantablePlot.plant.HasTag(GameTags.Wilting))
			{
				num += 1f;
			}
		}
		base.AddPoint(num);
	}

	// Token: 0x06002769 RID: 10089 RVA: 0x000E2A54 File Offset: 0x000E0C54
	public override string FormatValueString(float value)
	{
		return value.ToString() + "%";
	}
}
