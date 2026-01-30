using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000CE1 RID: 3297
public class ConfirmDialogScreen : KModalScreen
{
	// Token: 0x060065D3 RID: 26067 RVA: 0x00265855 File Offset: 0x00263A55
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.gameObject.SetActive(false);
	}

	// Token: 0x060065D4 RID: 26068 RVA: 0x00265869 File Offset: 0x00263A69
	public override bool IsModal()
	{
		return true;
	}

	// Token: 0x060065D5 RID: 26069 RVA: 0x0026586C File Offset: 0x00263A6C
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape))
		{
			this.OnSelect_CANCEL();
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x060065D6 RID: 26070 RVA: 0x00265888 File Offset: 0x00263A88
	public void PopupConfirmDialog(string text, System.Action on_confirm, System.Action on_cancel, string configurable_text = null, System.Action on_configurable_clicked = null, string title_text = null, string confirm_text = null, string cancel_text = null, Sprite image_sprite = null)
	{
		while (base.transform.parent.GetComponent<Canvas>() == null && base.transform.parent.parent != null)
		{
			base.transform.SetParent(base.transform.parent.parent);
		}
		base.transform.SetAsLastSibling();
		this.confirmAction = on_confirm;
		this.cancelAction = on_cancel;
		this.configurableAction = on_configurable_clicked;
		int num = 0;
		if (this.confirmAction != null)
		{
			num++;
		}
		if (this.cancelAction != null)
		{
			num++;
		}
		if (this.configurableAction != null)
		{
			num++;
		}
		this.confirmButton.GetComponentInChildren<LocText>().text = ((confirm_text == null) ? UI.CONFIRMDIALOG.OK.text : confirm_text);
		this.cancelButton.GetComponentInChildren<LocText>().text = ((cancel_text == null) ? UI.CONFIRMDIALOG.CANCEL.text : cancel_text);
		this.confirmButton.GetComponent<KButton>().onClick += this.OnSelect_OK;
		this.cancelButton.GetComponent<KButton>().onClick += this.OnSelect_CANCEL;
		this.configurableButton.GetComponent<KButton>().onClick += this.OnSelect_third;
		this.cancelButton.SetActive(on_cancel != null);
		if (this.configurableButton != null)
		{
			this.configurableButton.SetActive(this.configurableAction != null);
			if (configurable_text != null)
			{
				this.configurableButton.GetComponentInChildren<LocText>().text = configurable_text;
			}
		}
		if (image_sprite != null)
		{
			this.image.sprite = image_sprite;
			this.image.gameObject.SetActive(true);
		}
		if (title_text != null)
		{
			this.titleText.key = "";
			this.titleText.text = title_text;
		}
		this.popupMessage.text = text;
	}

	// Token: 0x060065D7 RID: 26071 RVA: 0x00265A5D File Offset: 0x00263C5D
	public void OnSelect_OK()
	{
		if (this.deactivateOnConfirmAction)
		{
			this.Deactivate();
		}
		if (this.confirmAction != null)
		{
			this.confirmAction();
		}
	}

	// Token: 0x060065D8 RID: 26072 RVA: 0x00265A80 File Offset: 0x00263C80
	public void OnSelect_CANCEL()
	{
		if (this.deactivateOnCancelAction)
		{
			this.Deactivate();
		}
		if (this.cancelAction != null)
		{
			this.cancelAction();
		}
	}

	// Token: 0x060065D9 RID: 26073 RVA: 0x00265AA3 File Offset: 0x00263CA3
	public void OnSelect_third()
	{
		if (this.deactivateOnConfigurableAction)
		{
			this.Deactivate();
		}
		if (this.configurableAction != null)
		{
			this.configurableAction();
		}
	}

	// Token: 0x060065DA RID: 26074 RVA: 0x00265AC6 File Offset: 0x00263CC6
	protected override void OnDeactivate()
	{
		if (this.onDeactivateCB != null)
		{
			this.onDeactivateCB();
		}
		base.OnDeactivate();
	}

	// Token: 0x0400455F RID: 17759
	private System.Action confirmAction;

	// Token: 0x04004560 RID: 17760
	private System.Action cancelAction;

	// Token: 0x04004561 RID: 17761
	private System.Action configurableAction;

	// Token: 0x04004562 RID: 17762
	public bool deactivateOnConfigurableAction = true;

	// Token: 0x04004563 RID: 17763
	public bool deactivateOnConfirmAction = true;

	// Token: 0x04004564 RID: 17764
	public bool deactivateOnCancelAction = true;

	// Token: 0x04004565 RID: 17765
	public System.Action onDeactivateCB;

	// Token: 0x04004566 RID: 17766
	[SerializeField]
	private GameObject confirmButton;

	// Token: 0x04004567 RID: 17767
	[SerializeField]
	private GameObject cancelButton;

	// Token: 0x04004568 RID: 17768
	[SerializeField]
	private GameObject configurableButton;

	// Token: 0x04004569 RID: 17769
	[SerializeField]
	private LocText titleText;

	// Token: 0x0400456A RID: 17770
	[SerializeField]
	private LocText popupMessage;

	// Token: 0x0400456B RID: 17771
	[SerializeField]
	private Image image;
}
