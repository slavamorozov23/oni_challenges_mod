using System;

// Token: 0x02000E1F RID: 3615
[Serializable]
public struct ButtonMenuTextOverride
{
	// Token: 0x170007EA RID: 2026
	// (get) Token: 0x060072AB RID: 29355 RVA: 0x002BCA49 File Offset: 0x002BAC49
	public bool IsValid
	{
		get
		{
			return !string.IsNullOrEmpty(this.Text) && !string.IsNullOrEmpty(this.ToolTip);
		}
	}

	// Token: 0x170007EB RID: 2027
	// (get) Token: 0x060072AC RID: 29356 RVA: 0x002BCA72 File Offset: 0x002BAC72
	public bool HasCancelText
	{
		get
		{
			return !string.IsNullOrEmpty(this.CancelText) && !string.IsNullOrEmpty(this.CancelToolTip);
		}
	}

	// Token: 0x04004F45 RID: 20293
	public LocString Text;

	// Token: 0x04004F46 RID: 20294
	public LocString CancelText;

	// Token: 0x04004F47 RID: 20295
	public LocString ToolTip;

	// Token: 0x04004F48 RID: 20296
	public LocString CancelToolTip;
}
