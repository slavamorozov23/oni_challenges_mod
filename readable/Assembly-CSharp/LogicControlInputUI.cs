using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D65 RID: 3429
[AddComponentMenu("KMonoBehaviour/scripts/LogicRibbonDisplayUI")]
public class LogicControlInputUI : KMonoBehaviour
{
	// Token: 0x06006A40 RID: 27200 RVA: 0x00282BB0 File Offset: 0x00280DB0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.colourOn = GlobalAssets.Instance.colorSet.logicOn;
		this.colourOff = GlobalAssets.Instance.colorSet.logicOff;
		this.colourOn.a = (this.colourOff.a = byte.MaxValue);
		this.colourDisconnected = GlobalAssets.Instance.colorSet.logicDisconnected;
		this.icon.raycastTarget = false;
		this.border.raycastTarget = false;
	}

	// Token: 0x06006A41 RID: 27201 RVA: 0x00282C38 File Offset: 0x00280E38
	public void SetContent(LogicCircuitNetwork network)
	{
		Color32 c = (network == null) ? GlobalAssets.Instance.colorSet.logicDisconnected : (network.IsBitActive(0) ? this.colourOn : this.colourOff);
		this.icon.color = c;
	}

	// Token: 0x04004909 RID: 18697
	[SerializeField]
	private Image icon;

	// Token: 0x0400490A RID: 18698
	[SerializeField]
	private Image border;

	// Token: 0x0400490B RID: 18699
	[SerializeField]
	private LogicModeUI uiAsset;

	// Token: 0x0400490C RID: 18700
	private Color32 colourOn;

	// Token: 0x0400490D RID: 18701
	private Color32 colourOff;

	// Token: 0x0400490E RID: 18702
	private Color32 colourDisconnected;
}
