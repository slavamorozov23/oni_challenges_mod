using System;

// Token: 0x02000E78 RID: 3704
public interface ICheckboxControl
{
	// Token: 0x17000825 RID: 2085
	// (get) Token: 0x060075D7 RID: 30167
	string CheckboxTitleKey { get; }

	// Token: 0x17000826 RID: 2086
	// (get) Token: 0x060075D8 RID: 30168
	string CheckboxLabel { get; }

	// Token: 0x17000827 RID: 2087
	// (get) Token: 0x060075D9 RID: 30169
	string CheckboxTooltip { get; }

	// Token: 0x060075DA RID: 30170
	bool GetCheckboxValue();

	// Token: 0x060075DB RID: 30171
	void SetCheckboxValue(bool value);
}
