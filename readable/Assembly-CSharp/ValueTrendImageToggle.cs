using System;
using Klei.AI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000EC0 RID: 3776
public class ValueTrendImageToggle : MonoBehaviour
{
	// Token: 0x060078F8 RID: 30968 RVA: 0x002E8234 File Offset: 0x002E6434
	public void SetValue(AmountInstance ainstance)
	{
		float delta = ainstance.GetDelta();
		Sprite sprite = null;
		if (ainstance.paused || delta == 0f)
		{
			this.targetImage.gameObject.SetActive(false);
		}
		else
		{
			this.targetImage.gameObject.SetActive(true);
			if (delta <= -ainstance.amount.visualDeltaThreshold * 2f)
			{
				sprite = this.Down_Three;
			}
			else if (delta <= -ainstance.amount.visualDeltaThreshold)
			{
				sprite = this.Down_Two;
			}
			else if (delta <= 0f)
			{
				sprite = this.Down_One;
			}
			else if (delta > ainstance.amount.visualDeltaThreshold * 2f)
			{
				sprite = this.Up_Three;
			}
			else if (delta > ainstance.amount.visualDeltaThreshold)
			{
				sprite = this.Up_Two;
			}
			else if (delta > 0f)
			{
				sprite = this.Up_One;
			}
		}
		this.targetImage.sprite = sprite;
	}

	// Token: 0x0400544E RID: 21582
	public Image targetImage;

	// Token: 0x0400544F RID: 21583
	public Sprite Up_One;

	// Token: 0x04005450 RID: 21584
	public Sprite Up_Two;

	// Token: 0x04005451 RID: 21585
	public Sprite Up_Three;

	// Token: 0x04005452 RID: 21586
	public Sprite Down_One;

	// Token: 0x04005453 RID: 21587
	public Sprite Down_Two;

	// Token: 0x04005454 RID: 21588
	public Sprite Down_Three;

	// Token: 0x04005455 RID: 21589
	public Sprite Zero;
}
