using System;
using Database;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D5C RID: 3420
public class KleiPermitDioramaVis_Wallpaper : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x060069C4 RID: 27076 RVA: 0x002805EF File Offset: 0x0027E7EF
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x060069C5 RID: 27077 RVA: 0x002805F7 File Offset: 0x0027E7F7
	public void ConfigureSetup()
	{
	}

	// Token: 0x060069C6 RID: 27078 RVA: 0x002805FC File Offset: 0x0027E7FC
	public void ConfigureWith(PermitResource permit)
	{
		PermitPresentationInfo permitPresentationInfo = permit.GetPermitPresentationInfo();
		this.itemSprite.rectTransform().sizeDelta = Vector2.one * 176f;
		this.itemSprite.sprite = permitPresentationInfo.sprite;
		if (!this.itemSpriteDidInit)
		{
			this.itemSpriteDidInit = true;
			this.itemSpritePosStart = this.itemSprite.rectTransform.anchoredPosition + new Vector2(0f, 16f);
			this.itemSpritePosEnd = this.itemSprite.rectTransform.anchoredPosition;
		}
		this.itemSprite.StartCoroutine(Updater.Parallel(new Updater[]
		{
			Updater.Ease(delegate(float alpha)
			{
				this.itemSprite.color = new Color(1f, 1f, 1f, alpha);
			}, 0f, 1f, 0.2f, Easing.SmoothStep, 0.1f),
			Updater.Ease(delegate(Vector2 position)
			{
				this.itemSprite.rectTransform.anchoredPosition = position;
			}, this.itemSpritePosStart, this.itemSpritePosEnd, 0.2f, Easing.SmoothStep, 0.1f)
		}));
	}

	// Token: 0x040048BA RID: 18618
	[SerializeField]
	private Image itemSprite;

	// Token: 0x040048BB RID: 18619
	private bool itemSpriteDidInit;

	// Token: 0x040048BC RID: 18620
	private Vector2 itemSpritePosStart;

	// Token: 0x040048BD RID: 18621
	private Vector2 itemSpritePosEnd;
}
