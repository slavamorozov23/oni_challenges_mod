using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D47 RID: 3399
public class KleiItemDropScreen_PermitVis_Fallback : KMonoBehaviour
{
	// Token: 0x0600695A RID: 26970 RVA: 0x0027E749 File Offset: 0x0027C949
	public void ConfigureWith(DropScreenPresentationInfo info)
	{
		this.sprite.sprite = info.Sprite;
	}

	// Token: 0x0400486D RID: 18541
	[SerializeField]
	private Image sprite;
}
