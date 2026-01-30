using System;

// Token: 0x02000E1E RID: 3614
public interface ISidescreenButtonControl
{
	// Token: 0x170007E8 RID: 2024
	// (get) Token: 0x060072A3 RID: 29347
	string SidescreenButtonText { get; }

	// Token: 0x170007E9 RID: 2025
	// (get) Token: 0x060072A4 RID: 29348
	string SidescreenButtonTooltip { get; }

	// Token: 0x060072A5 RID: 29349
	void SetButtonTextOverride(ButtonMenuTextOverride textOverride);

	// Token: 0x060072A6 RID: 29350
	bool SidescreenEnabled();

	// Token: 0x060072A7 RID: 29351
	bool SidescreenButtonInteractable();

	// Token: 0x060072A8 RID: 29352
	void OnSidescreenButtonPressed();

	// Token: 0x060072A9 RID: 29353
	int HorizontalGroupID();

	// Token: 0x060072AA RID: 29354
	int ButtonSideScreenSortOrder();
}
