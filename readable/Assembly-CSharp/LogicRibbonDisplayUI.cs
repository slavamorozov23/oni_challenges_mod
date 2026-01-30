using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D66 RID: 3430
[AddComponentMenu("KMonoBehaviour/scripts/LogicRibbonDisplayUI")]
public class LogicRibbonDisplayUI : KMonoBehaviour
{
	// Token: 0x06006A43 RID: 27203 RVA: 0x00282C8C File Offset: 0x00280E8C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.colourOn = GlobalAssets.Instance.colorSet.logicOn;
		this.colourOff = GlobalAssets.Instance.colorSet.logicOff;
		this.colourOn.a = (this.colourOff.a = byte.MaxValue);
		this.wire1.raycastTarget = false;
		this.wire2.raycastTarget = false;
		this.wire3.raycastTarget = false;
		this.wire4.raycastTarget = false;
	}

	// Token: 0x06006A44 RID: 27204 RVA: 0x00282D18 File Offset: 0x00280F18
	public void SetContent(LogicCircuitNetwork network)
	{
		Color32 color = this.colourDisconnected;
		List<Color32> list = new List<Color32>();
		for (int i = 0; i < this.bitDepth; i++)
		{
			list.Add((network == null) ? color : (network.IsBitActive(i) ? this.colourOn : this.colourOff));
		}
		if (this.wire1.color != list[0])
		{
			this.wire1.color = list[0];
		}
		if (this.wire2.color != list[1])
		{
			this.wire2.color = list[1];
		}
		if (this.wire3.color != list[2])
		{
			this.wire3.color = list[2];
		}
		if (this.wire4.color != list[3])
		{
			this.wire4.color = list[3];
		}
	}

	// Token: 0x0400490F RID: 18703
	[SerializeField]
	private Image wire1;

	// Token: 0x04004910 RID: 18704
	[SerializeField]
	private Image wire2;

	// Token: 0x04004911 RID: 18705
	[SerializeField]
	private Image wire3;

	// Token: 0x04004912 RID: 18706
	[SerializeField]
	private Image wire4;

	// Token: 0x04004913 RID: 18707
	[SerializeField]
	private LogicModeUI uiAsset;

	// Token: 0x04004914 RID: 18708
	private Color32 colourOn;

	// Token: 0x04004915 RID: 18709
	private Color32 colourOff;

	// Token: 0x04004916 RID: 18710
	private Color32 colourDisconnected = new Color(255f, 255f, 255f, 255f);

	// Token: 0x04004917 RID: 18711
	private int bitDepth = 4;
}
