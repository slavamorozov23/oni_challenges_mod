using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000EA1 RID: 3745
public class SpriteListDialogScreen : KModalScreen
{
	// Token: 0x060077C9 RID: 30665 RVA: 0x002DEDDB File Offset: 0x002DCFDB
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.gameObject.SetActive(false);
		this.buttons = new List<SpriteListDialogScreen.Button>();
	}

	// Token: 0x060077CA RID: 30666 RVA: 0x002DEDFA File Offset: 0x002DCFFA
	public override bool IsModal()
	{
		return true;
	}

	// Token: 0x060077CB RID: 30667 RVA: 0x002DEDFD File Offset: 0x002DCFFD
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape))
		{
			this.Deactivate();
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x060077CC RID: 30668 RVA: 0x002DEE18 File Offset: 0x002DD018
	public void AddOption(string text, System.Action action)
	{
		GameObject gameObject = Util.KInstantiateUI(this.buttonPrefab, this.buttonPanel, true);
		this.buttons.Add(new SpriteListDialogScreen.Button
		{
			label = text,
			action = action,
			gameObject = gameObject
		});
	}

	// Token: 0x060077CD RID: 30669 RVA: 0x002DEE64 File Offset: 0x002DD064
	public void AddListRow(Sprite sprite, string text, float width = -1f, float height = -1f)
	{
		GameObject gameObject = Util.KInstantiateUI(this.listPrefab, this.listPanel, true);
		gameObject.GetComponentInChildren<LocText>().text = text;
		Image componentInChildren = gameObject.GetComponentInChildren<Image>();
		componentInChildren.sprite = sprite;
		if (sprite == null)
		{
			Color color = componentInChildren.color;
			color.a = 0f;
			componentInChildren.color = color;
		}
		if (width >= 0f || height >= 0f)
		{
			componentInChildren.GetComponent<AspectRatioFitter>().enabled = false;
			LayoutElement component = componentInChildren.GetComponent<LayoutElement>();
			component.minWidth = width;
			component.preferredWidth = width;
			component.minHeight = height;
			component.preferredHeight = height;
			return;
		}
		AspectRatioFitter component2 = componentInChildren.GetComponent<AspectRatioFitter>();
		float aspectRatio = (sprite == null) ? 1f : (sprite.rect.width / sprite.rect.height);
		component2.aspectRatio = aspectRatio;
	}

	// Token: 0x060077CE RID: 30670 RVA: 0x002DEF3C File Offset: 0x002DD13C
	public void PopupConfirmDialog(string text, string title_text = null)
	{
		foreach (SpriteListDialogScreen.Button button in this.buttons)
		{
			button.gameObject.GetComponentInChildren<LocText>().text = button.label;
			button.gameObject.GetComponent<KButton>().onClick += button.action;
		}
		if (title_text != null)
		{
			this.titleText.text = title_text;
		}
		this.popupMessage.text = text;
	}

	// Token: 0x060077CF RID: 30671 RVA: 0x002DEFD0 File Offset: 0x002DD1D0
	protected override void OnDeactivate()
	{
		if (this.onDeactivateCB != null)
		{
			this.onDeactivateCB();
		}
		base.OnDeactivate();
	}

	// Token: 0x04005346 RID: 21318
	public System.Action onDeactivateCB;

	// Token: 0x04005347 RID: 21319
	[SerializeField]
	private GameObject buttonPrefab;

	// Token: 0x04005348 RID: 21320
	[SerializeField]
	private GameObject buttonPanel;

	// Token: 0x04005349 RID: 21321
	[SerializeField]
	private LocText titleText;

	// Token: 0x0400534A RID: 21322
	[SerializeField]
	private LocText popupMessage;

	// Token: 0x0400534B RID: 21323
	[SerializeField]
	private GameObject listPanel;

	// Token: 0x0400534C RID: 21324
	[SerializeField]
	private GameObject listPrefab;

	// Token: 0x0400534D RID: 21325
	private List<SpriteListDialogScreen.Button> buttons;

	// Token: 0x0200210B RID: 8459
	private struct Button
	{
		// Token: 0x040097E7 RID: 38887
		public System.Action action;

		// Token: 0x040097E8 RID: 38888
		public GameObject gameObject;

		// Token: 0x040097E9 RID: 38889
		public string label;
	}
}
