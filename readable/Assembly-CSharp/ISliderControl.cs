using System;

// Token: 0x02000E7E RID: 3710
public interface ISliderControl
{
	// Token: 0x1700082F RID: 2095
	// (get) Token: 0x0600761B RID: 30235
	string SliderTitleKey { get; }

	// Token: 0x17000830 RID: 2096
	// (get) Token: 0x0600761C RID: 30236
	string SliderUnits { get; }

	// Token: 0x0600761D RID: 30237
	int SliderDecimalPlaces(int index);

	// Token: 0x0600761E RID: 30238
	float GetSliderMin(int index);

	// Token: 0x0600761F RID: 30239
	float GetSliderMax(int index);

	// Token: 0x06007620 RID: 30240
	float GetSliderValue(int index);

	// Token: 0x06007621 RID: 30241
	void SetSliderValue(float percent, int index);

	// Token: 0x06007622 RID: 30242
	string GetSliderTooltipKey(int index);

	// Token: 0x06007623 RID: 30243
	string GetSliderTooltip(int index);
}
