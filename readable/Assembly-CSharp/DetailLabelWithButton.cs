using System;
using UnityEngine;

// Token: 0x02000CFC RID: 3324
[AddComponentMenu("KMonoBehaviour/scripts/DetailLabelWithButton")]
public class DetailLabelWithButton : KMonoBehaviour
{
	// Token: 0x060066BD RID: 26301 RVA: 0x0026B13C File Offset: 0x0026933C
	public void RefreshLabelsVisibility()
	{
		if (this.label.gameObject.activeInHierarchy != !string.IsNullOrEmpty(this.label.text))
		{
			this.label.gameObject.SetActive(!string.IsNullOrEmpty(this.label.text));
		}
		if (this.label2.gameObject.activeInHierarchy != !string.IsNullOrEmpty(this.label2.text))
		{
			this.label2.gameObject.SetActive(!string.IsNullOrEmpty(this.label2.text));
		}
		if (this.label3.gameObject.activeInHierarchy != !string.IsNullOrEmpty(this.label3.text))
		{
			this.label3.gameObject.SetActive(!string.IsNullOrEmpty(this.label3.text));
		}
	}

	// Token: 0x0400463E RID: 17982
	public LocText label;

	// Token: 0x0400463F RID: 17983
	public LocText label2;

	// Token: 0x04004640 RID: 17984
	public LocText label3;

	// Token: 0x04004641 RID: 17985
	public ToolTip toolTip;

	// Token: 0x04004642 RID: 17986
	public KButton button;
}
