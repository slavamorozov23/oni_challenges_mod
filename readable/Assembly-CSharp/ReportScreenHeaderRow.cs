using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DEE RID: 3566
[AddComponentMenu("KMonoBehaviour/scripts/ReportScreenHeaderRow")]
public class ReportScreenHeaderRow : KMonoBehaviour
{
	// Token: 0x06007043 RID: 28739 RVA: 0x002AA474 File Offset: 0x002A8674
	public void SetLine(ReportManager.ReportGroup reportGroup)
	{
		LayoutElement component = this.name.GetComponent<LayoutElement>();
		component.minWidth = (component.preferredWidth = this.nameWidth);
		this.spacer.minWidth = this.groupSpacerWidth;
		this.name.text = reportGroup.stringKey;
	}

	// Token: 0x04004D12 RID: 19730
	[SerializeField]
	public new LocText name;

	// Token: 0x04004D13 RID: 19731
	[SerializeField]
	private LayoutElement spacer;

	// Token: 0x04004D14 RID: 19732
	[SerializeField]
	private Image bgImage;

	// Token: 0x04004D15 RID: 19733
	public float groupSpacerWidth;

	// Token: 0x04004D16 RID: 19734
	private float nameWidth = 164f;

	// Token: 0x04004D17 RID: 19735
	[SerializeField]
	private Color oddRowColor;
}
