using System;
using Database;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D5D RID: 3421
public class KleiPermitDioramaVis_WiresAndAutomation : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x060069CA RID: 27082 RVA: 0x0028074C File Offset: 0x0027E94C
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x060069CB RID: 27083 RVA: 0x00280754 File Offset: 0x0027E954
	public void ConfigureSetup()
	{
	}

	// Token: 0x060069CC RID: 27084 RVA: 0x00280758 File Offset: 0x0027E958
	public void ConfigureWith(PermitResource permit)
	{
		PermitPresentationInfo permitPresentationInfo = permit.GetPermitPresentationInfo();
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

	// Token: 0x040048BE RID: 18622
	[SerializeField]
	private Image itemSprite;

	// Token: 0x040048BF RID: 18623
	private bool itemSpriteDidInit;

	// Token: 0x040048C0 RID: 18624
	private Vector2 itemSpritePosStart;

	// Token: 0x040048C1 RID: 18625
	private Vector2 itemSpritePosEnd;
}
