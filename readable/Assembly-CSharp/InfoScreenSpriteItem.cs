using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D30 RID: 3376
[AddComponentMenu("KMonoBehaviour/scripts/InfoScreenSpriteItem")]
public class InfoScreenSpriteItem : KMonoBehaviour
{
	// Token: 0x06006846 RID: 26694 RVA: 0x00275274 File Offset: 0x00273474
	public void SetSprite(Sprite sprite)
	{
		this.image.sprite = sprite;
		float num = sprite.rect.width / sprite.rect.height;
		this.layout.preferredWidth = this.layout.preferredHeight * num;
	}

	// Token: 0x0400479E RID: 18334
	[SerializeField]
	private Image image;

	// Token: 0x0400479F RID: 18335
	[SerializeField]
	private LayoutElement layout;
}
