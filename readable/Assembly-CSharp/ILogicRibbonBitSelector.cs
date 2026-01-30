using System;

// Token: 0x02000E4B RID: 3659
public interface ILogicRibbonBitSelector
{
	// Token: 0x06007403 RID: 29699
	void SetBitSelection(int bit);

	// Token: 0x06007404 RID: 29700
	int GetBitSelection();

	// Token: 0x06007405 RID: 29701
	int GetBitDepth();

	// Token: 0x170007F9 RID: 2041
	// (get) Token: 0x06007406 RID: 29702
	string SideScreenTitle { get; }

	// Token: 0x170007FA RID: 2042
	// (get) Token: 0x06007407 RID: 29703
	string SideScreenDescription { get; }

	// Token: 0x06007408 RID: 29704
	bool SideScreenDisplayWriterDescription();

	// Token: 0x06007409 RID: 29705
	bool SideScreenDisplayReaderDescription();

	// Token: 0x0600740A RID: 29706
	bool IsBitActive(int bit);

	// Token: 0x0600740B RID: 29707
	int GetOutputValue();

	// Token: 0x0600740C RID: 29708
	int GetInputValue();

	// Token: 0x0600740D RID: 29709
	void UpdateVisuals();
}
