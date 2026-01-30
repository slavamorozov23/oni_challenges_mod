using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D2D RID: 3373
public class InfoDialogScreen : KModalScreen
{
	// Token: 0x0600682B RID: 26667 RVA: 0x00274EEE File Offset: 0x002730EE
	public InfoScreenPlainText GetSubHeaderPrefab()
	{
		return this.subHeaderTemplate;
	}

	// Token: 0x0600682C RID: 26668 RVA: 0x00274EF6 File Offset: 0x002730F6
	public InfoScreenPlainText GetPlainTextPrefab()
	{
		return this.plainTextTemplate;
	}

	// Token: 0x0600682D RID: 26669 RVA: 0x00274EFE File Offset: 0x002730FE
	public InfoScreenLineItem GetLineItemPrefab()
	{
		return this.lineItemTemplate;
	}

	// Token: 0x0600682E RID: 26670 RVA: 0x00274F06 File Offset: 0x00273106
	public GameObject GetPrimaryButtonPrefab()
	{
		return this.leftButtonPrefab;
	}

	// Token: 0x0600682F RID: 26671 RVA: 0x00274F0E File Offset: 0x0027310E
	public GameObject GetSecondaryButtonPrefab()
	{
		return this.rightButtonPrefab;
	}

	// Token: 0x06006830 RID: 26672 RVA: 0x00274F16 File Offset: 0x00273116
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.gameObject.SetActive(false);
	}

	// Token: 0x06006831 RID: 26673 RVA: 0x00274F2A File Offset: 0x0027312A
	public override bool IsModal()
	{
		return true;
	}

	// Token: 0x06006832 RID: 26674 RVA: 0x00274F30 File Offset: 0x00273130
	public override void OnKeyDown(KButtonEvent e)
	{
		if (!this.escapeCloses)
		{
			e.TryConsume(global::Action.Escape);
			return;
		}
		if (e.TryConsume(global::Action.Escape))
		{
			this.Deactivate();
			return;
		}
		if (PlayerController.Instance != null && PlayerController.Instance.ConsumeIfNotDragging(e, global::Action.MouseRight))
		{
			this.Deactivate();
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06006833 RID: 26675 RVA: 0x00274F87 File Offset: 0x00273187
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (!show && this.onDeactivateFn != null)
		{
			this.onDeactivateFn();
		}
	}

	// Token: 0x06006834 RID: 26676 RVA: 0x00274FA6 File Offset: 0x002731A6
	public InfoDialogScreen AddDefaultOK(bool escapeCloses = false)
	{
		this.AddOption(UI.CONFIRMDIALOG.OK, delegate(InfoDialogScreen d)
		{
			d.Deactivate();
		}, true);
		this.escapeCloses = escapeCloses;
		return this;
	}

	// Token: 0x06006835 RID: 26677 RVA: 0x00274FE1 File Offset: 0x002731E1
	public InfoDialogScreen AddDefaultCancel()
	{
		this.AddOption(UI.CONFIRMDIALOG.CANCEL, delegate(InfoDialogScreen d)
		{
			d.Deactivate();
		}, false);
		this.escapeCloses = true;
		return this;
	}

	// Token: 0x06006836 RID: 26678 RVA: 0x0027501C File Offset: 0x0027321C
	public InfoDialogScreen AddOption(string text, Action<InfoDialogScreen> action, bool rightSide = false)
	{
		GameObject gameObject = Util.KInstantiateUI(rightSide ? this.rightButtonPrefab : this.leftButtonPrefab, rightSide ? this.rightButtonPanel : this.leftButtonPanel, true);
		gameObject.gameObject.GetComponentInChildren<LocText>().text = text;
		gameObject.gameObject.GetComponent<KButton>().onClick += delegate()
		{
			action(this);
		};
		return this;
	}

	// Token: 0x06006837 RID: 26679 RVA: 0x00275094 File Offset: 0x00273294
	public InfoDialogScreen AddOption(bool rightSide, out KButton button, out LocText buttonText)
	{
		GameObject gameObject = Util.KInstantiateUI(rightSide ? this.rightButtonPrefab : this.leftButtonPrefab, rightSide ? this.rightButtonPanel : this.leftButtonPanel, true);
		button = gameObject.GetComponent<KButton>();
		buttonText = gameObject.GetComponentInChildren<LocText>();
		return this;
	}

	// Token: 0x06006838 RID: 26680 RVA: 0x002750DB File Offset: 0x002732DB
	public InfoDialogScreen SetHeader(string header)
	{
		this.header.text = header;
		return this;
	}

	// Token: 0x06006839 RID: 26681 RVA: 0x002750EA File Offset: 0x002732EA
	public InfoDialogScreen AddSprite(Sprite sprite)
	{
		Util.KInstantiateUI<InfoScreenSpriteItem>(this.spriteItemTemplate.gameObject, this.contentContainer, false).SetSprite(sprite);
		return this;
	}

	// Token: 0x0600683A RID: 26682 RVA: 0x0027510A File Offset: 0x0027330A
	public InfoDialogScreen AddPlainText(string text)
	{
		Util.KInstantiateUI<InfoScreenPlainText>(this.plainTextTemplate.gameObject, this.contentContainer, false).SetText(text);
		return this;
	}

	// Token: 0x0600683B RID: 26683 RVA: 0x0027512A File Offset: 0x0027332A
	public InfoDialogScreen AddLineItem(string text, string tooltip)
	{
		InfoScreenLineItem infoScreenLineItem = Util.KInstantiateUI<InfoScreenLineItem>(this.lineItemTemplate.gameObject, this.contentContainer, false);
		infoScreenLineItem.SetText(text);
		infoScreenLineItem.SetTooltip(tooltip);
		return this;
	}

	// Token: 0x0600683C RID: 26684 RVA: 0x00275151 File Offset: 0x00273351
	public InfoDialogScreen AddSubHeader(string text)
	{
		Util.KInstantiateUI<InfoScreenPlainText>(this.subHeaderTemplate.gameObject, this.contentContainer, false).SetText(text);
		return this;
	}

	// Token: 0x0600683D RID: 26685 RVA: 0x00275174 File Offset: 0x00273374
	public InfoDialogScreen AddSpacer(float height)
	{
		GameObject gameObject = new GameObject("spacer");
		gameObject.SetActive(false);
		gameObject.transform.SetParent(this.contentContainer.transform, false);
		LayoutElement layoutElement = gameObject.AddComponent<LayoutElement>();
		layoutElement.minHeight = height;
		layoutElement.preferredHeight = height;
		layoutElement.flexibleHeight = 0f;
		gameObject.SetActive(true);
		return this;
	}

	// Token: 0x0600683E RID: 26686 RVA: 0x002751CE File Offset: 0x002733CE
	public InfoDialogScreen AddUI<T>(T prefab, out T spawn) where T : MonoBehaviour
	{
		spawn = Util.KInstantiateUI<T>(prefab.gameObject, this.contentContainer, true);
		return this;
	}

	// Token: 0x0600683F RID: 26687 RVA: 0x002751F0 File Offset: 0x002733F0
	public InfoDialogScreen AddDescriptors(List<Descriptor> descriptors)
	{
		for (int i = 0; i < descriptors.Count; i++)
		{
			this.AddLineItem(descriptors[i].IndentedText(), descriptors[i].tooltipText);
		}
		return this;
	}

	// Token: 0x0400478D RID: 18317
	[SerializeField]
	private InfoScreenPlainText subHeaderTemplate;

	// Token: 0x0400478E RID: 18318
	[SerializeField]
	private InfoScreenPlainText plainTextTemplate;

	// Token: 0x0400478F RID: 18319
	[SerializeField]
	private InfoScreenLineItem lineItemTemplate;

	// Token: 0x04004790 RID: 18320
	[SerializeField]
	private InfoScreenSpriteItem spriteItemTemplate;

	// Token: 0x04004791 RID: 18321
	[Space(10f)]
	[SerializeField]
	private LocText header;

	// Token: 0x04004792 RID: 18322
	[SerializeField]
	private GameObject contentContainer;

	// Token: 0x04004793 RID: 18323
	[SerializeField]
	private GameObject leftButtonPrefab;

	// Token: 0x04004794 RID: 18324
	[SerializeField]
	private GameObject rightButtonPrefab;

	// Token: 0x04004795 RID: 18325
	[SerializeField]
	private GameObject leftButtonPanel;

	// Token: 0x04004796 RID: 18326
	[SerializeField]
	private GameObject rightButtonPanel;

	// Token: 0x04004797 RID: 18327
	private bool escapeCloses;

	// Token: 0x04004798 RID: 18328
	public System.Action onDeactivateFn;
}
