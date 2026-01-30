using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000CFE RID: 3326
[AddComponentMenu("KMonoBehaviour/scripts/ScalerMask")]
public class ScalerMask : KMonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x17000781 RID: 1921
	// (get) Token: 0x060066C7 RID: 26311 RVA: 0x0026B60E File Offset: 0x0026980E
	private RectTransform ThisTransform
	{
		get
		{
			if (this._thisTransform == null)
			{
				this._thisTransform = base.GetComponent<RectTransform>();
			}
			return this._thisTransform;
		}
	}

	// Token: 0x17000782 RID: 1922
	// (get) Token: 0x060066C8 RID: 26312 RVA: 0x0026B630 File Offset: 0x00269830
	private LayoutElement ThisLayoutElement
	{
		get
		{
			if (this._thisLayoutElement == null)
			{
				this._thisLayoutElement = base.GetComponent<LayoutElement>();
			}
			return this._thisLayoutElement;
		}
	}

	// Token: 0x060066C9 RID: 26313 RVA: 0x0026B654 File Offset: 0x00269854
	protected override void OnSpawn()
	{
		base.OnSpawn();
		DetailsScreen componentInParent = base.GetComponentInParent<DetailsScreen>();
		if (componentInParent)
		{
			DetailsScreen detailsScreen = componentInParent;
			detailsScreen.pointerEnterActions = (KScreen.PointerEnterActions)Delegate.Combine(detailsScreen.pointerEnterActions, new KScreen.PointerEnterActions(this.OnPointerEnterGrandparent));
			DetailsScreen detailsScreen2 = componentInParent;
			detailsScreen2.pointerExitActions = (KScreen.PointerExitActions)Delegate.Combine(detailsScreen2.pointerExitActions, new KScreen.PointerExitActions(this.OnPointerExitGrandparent));
		}
	}

	// Token: 0x060066CA RID: 26314 RVA: 0x0026B6BC File Offset: 0x002698BC
	protected override void OnCleanUp()
	{
		DetailsScreen componentInParent = base.GetComponentInParent<DetailsScreen>();
		if (componentInParent)
		{
			DetailsScreen detailsScreen = componentInParent;
			detailsScreen.pointerEnterActions = (KScreen.PointerEnterActions)Delegate.Remove(detailsScreen.pointerEnterActions, new KScreen.PointerEnterActions(this.OnPointerEnterGrandparent));
			DetailsScreen detailsScreen2 = componentInParent;
			detailsScreen2.pointerExitActions = (KScreen.PointerExitActions)Delegate.Remove(detailsScreen2.pointerExitActions, new KScreen.PointerExitActions(this.OnPointerExitGrandparent));
		}
		base.OnCleanUp();
	}

	// Token: 0x060066CB RID: 26315 RVA: 0x0026B724 File Offset: 0x00269924
	private void Update()
	{
		if (this.SourceTransform != null)
		{
			this.SourceTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this.ThisTransform.rect.width);
		}
		if (this.SourceTransform != null && (!this.hoverLock || !this.grandparentIsHovered || this.isHovered || this.queuedSizeUpdate))
		{
			this.ThisLayoutElement.minHeight = this.SourceTransform.rect.height + this.topPadding + this.bottomPadding;
			this.SourceTransform.anchoredPosition = new Vector2(0f, -this.topPadding);
			this.queuedSizeUpdate = false;
		}
		if (this.hoverIndicator != null)
		{
			if (this.SourceTransform != null && this.SourceTransform.rect.height > this.ThisTransform.rect.height)
			{
				this.hoverIndicator.SetActive(true);
				return;
			}
			this.hoverIndicator.SetActive(false);
		}
	}

	// Token: 0x060066CC RID: 26316 RVA: 0x0026B838 File Offset: 0x00269A38
	public void UpdateSize()
	{
		this.queuedSizeUpdate = true;
	}

	// Token: 0x060066CD RID: 26317 RVA: 0x0026B841 File Offset: 0x00269A41
	public void OnPointerEnterGrandparent(PointerEventData eventData)
	{
		this.grandparentIsHovered = true;
	}

	// Token: 0x060066CE RID: 26318 RVA: 0x0026B84A File Offset: 0x00269A4A
	public void OnPointerExitGrandparent(PointerEventData eventData)
	{
		this.grandparentIsHovered = false;
	}

	// Token: 0x060066CF RID: 26319 RVA: 0x0026B853 File Offset: 0x00269A53
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.isHovered = true;
	}

	// Token: 0x060066D0 RID: 26320 RVA: 0x0026B85C File Offset: 0x00269A5C
	public void OnPointerExit(PointerEventData eventData)
	{
		this.isHovered = false;
	}

	// Token: 0x04004650 RID: 18000
	public RectTransform SourceTransform;

	// Token: 0x04004651 RID: 18001
	private RectTransform _thisTransform;

	// Token: 0x04004652 RID: 18002
	private LayoutElement _thisLayoutElement;

	// Token: 0x04004653 RID: 18003
	public GameObject hoverIndicator;

	// Token: 0x04004654 RID: 18004
	public bool hoverLock;

	// Token: 0x04004655 RID: 18005
	private bool grandparentIsHovered;

	// Token: 0x04004656 RID: 18006
	private bool isHovered;

	// Token: 0x04004657 RID: 18007
	private bool queuedSizeUpdate = true;

	// Token: 0x04004658 RID: 18008
	public float topPadding;

	// Token: 0x04004659 RID: 18009
	public float bottomPadding;
}
