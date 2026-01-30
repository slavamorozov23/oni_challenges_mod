using System;

// Token: 0x0200080E RID: 2062
public interface IThresholdSwitch
{
	// Token: 0x170003A6 RID: 934
	// (get) Token: 0x060037C2 RID: 14274
	// (set) Token: 0x060037C3 RID: 14275
	float Threshold { get; set; }

	// Token: 0x170003A7 RID: 935
	// (get) Token: 0x060037C4 RID: 14276
	// (set) Token: 0x060037C5 RID: 14277
	bool ActivateAboveThreshold { get; set; }

	// Token: 0x170003A8 RID: 936
	// (get) Token: 0x060037C6 RID: 14278
	float CurrentValue { get; }

	// Token: 0x170003A9 RID: 937
	// (get) Token: 0x060037C7 RID: 14279
	float RangeMin { get; }

	// Token: 0x170003AA RID: 938
	// (get) Token: 0x060037C8 RID: 14280
	float RangeMax { get; }

	// Token: 0x060037C9 RID: 14281
	float GetRangeMinInputField();

	// Token: 0x060037CA RID: 14282
	float GetRangeMaxInputField();

	// Token: 0x170003AB RID: 939
	// (get) Token: 0x060037CB RID: 14283
	LocString Title { get; }

	// Token: 0x170003AC RID: 940
	// (get) Token: 0x060037CC RID: 14284
	LocString ThresholdValueName { get; }

	// Token: 0x060037CD RID: 14285
	LocString ThresholdValueUnits();

	// Token: 0x060037CE RID: 14286
	string Format(float value, bool units);

	// Token: 0x170003AD RID: 941
	// (get) Token: 0x060037CF RID: 14287
	string AboveToolTip { get; }

	// Token: 0x170003AE RID: 942
	// (get) Token: 0x060037D0 RID: 14288
	string BelowToolTip { get; }

	// Token: 0x060037D1 RID: 14289
	float ProcessedSliderValue(float input);

	// Token: 0x060037D2 RID: 14290
	float ProcessedInputValue(float input);

	// Token: 0x170003AF RID: 943
	// (get) Token: 0x060037D3 RID: 14291
	ThresholdScreenLayoutType LayoutType { get; }

	// Token: 0x170003B0 RID: 944
	// (get) Token: 0x060037D4 RID: 14292
	int IncrementScale { get; }

	// Token: 0x170003B1 RID: 945
	// (get) Token: 0x060037D5 RID: 14293
	NonLinearSlider.Range[] GetRanges { get; }
}
