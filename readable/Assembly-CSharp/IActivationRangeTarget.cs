using System;

// Token: 0x02000E10 RID: 3600
public interface IActivationRangeTarget
{
	// Token: 0x170007DB RID: 2011
	// (get) Token: 0x06007207 RID: 29191
	// (set) Token: 0x06007208 RID: 29192
	float ActivateValue { get; set; }

	// Token: 0x170007DC RID: 2012
	// (get) Token: 0x06007209 RID: 29193
	// (set) Token: 0x0600720A RID: 29194
	float DeactivateValue { get; set; }

	// Token: 0x170007DD RID: 2013
	// (get) Token: 0x0600720B RID: 29195
	float MinValue { get; }

	// Token: 0x170007DE RID: 2014
	// (get) Token: 0x0600720C RID: 29196
	float MaxValue { get; }

	// Token: 0x170007DF RID: 2015
	// (get) Token: 0x0600720D RID: 29197
	bool UseWholeNumbers { get; }

	// Token: 0x170007E0 RID: 2016
	// (get) Token: 0x0600720E RID: 29198
	string ActivationRangeTitleText { get; }

	// Token: 0x170007E1 RID: 2017
	// (get) Token: 0x0600720F RID: 29199
	string ActivateSliderLabelText { get; }

	// Token: 0x170007E2 RID: 2018
	// (get) Token: 0x06007210 RID: 29200
	string DeactivateSliderLabelText { get; }

	// Token: 0x170007E3 RID: 2019
	// (get) Token: 0x06007211 RID: 29201
	string ActivateTooltip { get; }

	// Token: 0x170007E4 RID: 2020
	// (get) Token: 0x06007212 RID: 29202
	string DeactivateTooltip { get; }
}
