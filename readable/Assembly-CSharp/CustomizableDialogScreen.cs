using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000CEC RID: 3308
public class CustomizableDialogScreen : KModalScreen
{
	// Token: 0x0600661F RID: 26143 RVA: 0x00266DDB File Offset: 0x00264FDB
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.gameObject.SetActive(false);
		this.buttons = new List<CustomizableDialogScreen.Button>();
	}

	// Token: 0x06006620 RID: 26144 RVA: 0x00266DFA File Offset: 0x00264FFA
	public override bool IsModal()
	{
		return true;
	}

	// Token: 0x06006621 RID: 26145 RVA: 0x00266E00 File Offset: 0x00265000
	public void AddOption(string text, System.Action action)
	{
		GameObject gameObject = Util.KInstantiateUI(this.buttonPrefab, this.buttonPanel, true);
		this.buttons.Add(new CustomizableDialogScreen.Button
		{
			label = text,
			action = action,
			gameObject = gameObject
		});
	}

	// Token: 0x06006622 RID: 26146 RVA: 0x00266E4C File Offset: 0x0026504C
	public void PopupConfirmDialog(string text, string title_text = null, Sprite image_sprite = null)
	{
		foreach (CustomizableDialogScreen.Button button in this.buttons)
		{
			button.gameObject.GetComponentInChildren<LocText>().text = button.label;
			button.gameObject.GetComponent<KButton>().onClick += button.action;
		}
		if (image_sprite != null)
		{
			this.image.sprite = image_sprite;
			this.image.gameObject.SetActive(true);
		}
		if (title_text != null)
		{
			this.titleText.text = title_text;
		}
		this.popupMessage.text = text;
	}

	// Token: 0x06006623 RID: 26147 RVA: 0x00266F08 File Offset: 0x00265108
	protected override void OnDeactivate()
	{
		if (this.onDeactivateCB != null)
		{
			this.onDeactivateCB();
		}
		base.OnDeactivate();
	}

	// Token: 0x040045A6 RID: 17830
	public System.Action onDeactivateCB;

	// Token: 0x040045A7 RID: 17831
	[SerializeField]
	private GameObject buttonPrefab;

	// Token: 0x040045A8 RID: 17832
	[SerializeField]
	private GameObject buttonPanel;

	// Token: 0x040045A9 RID: 17833
	[SerializeField]
	private LocText titleText;

	// Token: 0x040045AA RID: 17834
	[SerializeField]
	private LocText popupMessage;

	// Token: 0x040045AB RID: 17835
	[SerializeField]
	private Image image;

	// Token: 0x040045AC RID: 17836
	private List<CustomizableDialogScreen.Button> buttons;

	// Token: 0x02001F1D RID: 7965
	private struct Button
	{
		// Token: 0x04009195 RID: 37269
		public System.Action action;

		// Token: 0x04009196 RID: 37270
		public GameObject gameObject;

		// Token: 0x04009197 RID: 37271
		public string label;
	}
}
