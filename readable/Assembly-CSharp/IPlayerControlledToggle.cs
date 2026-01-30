using System;

// Token: 0x02000E61 RID: 3681
public interface IPlayerControlledToggle
{
	// Token: 0x060074E3 RID: 29923
	void ToggledByPlayer();

	// Token: 0x060074E4 RID: 29924
	bool ToggledOn();

	// Token: 0x060074E5 RID: 29925
	KSelectable GetSelectable();

	// Token: 0x1700081C RID: 2076
	// (get) Token: 0x060074E6 RID: 29926
	string SideScreenTitleKey { get; }

	// Token: 0x1700081D RID: 2077
	// (get) Token: 0x060074E7 RID: 29927
	// (set) Token: 0x060074E8 RID: 29928
	bool ToggleRequested { get; set; }
}
