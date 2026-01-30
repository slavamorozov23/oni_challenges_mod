using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DD9 RID: 3545
public class PlanCategoryNotifications : MonoBehaviour
{
	// Token: 0x06006F2B RID: 28459 RVA: 0x002A25F7 File Offset: 0x002A07F7
	public void ToggleAttention(bool active)
	{
		if (!this.AttentionImage)
		{
			return;
		}
		this.AttentionImage.gameObject.SetActive(active);
	}

	// Token: 0x04004C04 RID: 19460
	public Image AttentionImage;
}
