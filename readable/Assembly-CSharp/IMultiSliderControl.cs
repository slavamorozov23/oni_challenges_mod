using System;

// Token: 0x02000E55 RID: 3669
public interface IMultiSliderControl
{
	// Token: 0x17000806 RID: 2054
	// (get) Token: 0x0600745A RID: 29786
	string SidescreenTitleKey { get; }

	// Token: 0x0600745B RID: 29787
	bool SidescreenEnabled();

	// Token: 0x17000807 RID: 2055
	// (get) Token: 0x0600745C RID: 29788
	ISliderControl[] sliderControls { get; }
}
