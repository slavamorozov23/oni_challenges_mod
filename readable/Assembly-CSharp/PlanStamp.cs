using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DDB RID: 3547
[AddComponentMenu("KMonoBehaviour/scripts/PlanStamp")]
public class PlanStamp : KMonoBehaviour
{
	// Token: 0x06006F86 RID: 28550 RVA: 0x002A5C18 File Offset: 0x002A3E18
	public void SetStamp(Sprite sprite, string Text)
	{
		this.StampImage.sprite = sprite;
		this.StampText.text = Text.ToUpper();
	}

	// Token: 0x04004C4B RID: 19531
	public PlanStamp.StampArt stampSprites;

	// Token: 0x04004C4C RID: 19532
	[SerializeField]
	private Image StampImage;

	// Token: 0x04004C4D RID: 19533
	[SerializeField]
	private Text StampText;

	// Token: 0x0200204A RID: 8266
	[Serializable]
	public struct StampArt
	{
		// Token: 0x04009594 RID: 38292
		public Sprite UnderConstruction;

		// Token: 0x04009595 RID: 38293
		public Sprite NeedsResearch;

		// Token: 0x04009596 RID: 38294
		public Sprite SelectResource;

		// Token: 0x04009597 RID: 38295
		public Sprite NeedsRepair;

		// Token: 0x04009598 RID: 38296
		public Sprite NeedsPower;

		// Token: 0x04009599 RID: 38297
		public Sprite NeedsResource;

		// Token: 0x0400959A RID: 38298
		public Sprite NeedsGasPipe;

		// Token: 0x0400959B RID: 38299
		public Sprite NeedsLiquidPipe;
	}
}
