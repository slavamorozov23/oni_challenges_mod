using System;

// Token: 0x020008A6 RID: 2214
public interface IManageGrowingStates
{
	// Token: 0x06003CF2 RID: 15602
	float TimeUntilNextHarvest();

	// Token: 0x06003CF3 RID: 15603
	float PercentGrown();

	// Token: 0x06003CF4 RID: 15604
	Crop GetCropComponent();

	// Token: 0x06003CF5 RID: 15605
	void OverrideMaturityLevel(float percentage);

	// Token: 0x06003CF6 RID: 15606
	float DomesticGrowthTime();

	// Token: 0x06003CF7 RID: 15607
	float WildGrowthTime();

	// Token: 0x06003CF8 RID: 15608
	bool IsWildPlanted();
}
