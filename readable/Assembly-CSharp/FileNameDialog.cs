using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000C57 RID: 3159
public class FileNameDialog : KModalScreen
{
	// Token: 0x06005FF3 RID: 24563 RVA: 0x0023343D File Offset: 0x0023163D
	public override float GetSortKey()
	{
		return 150f;
	}

	// Token: 0x06005FF4 RID: 24564 RVA: 0x00233444 File Offset: 0x00231644
	public void SetTextAndSelect(string text)
	{
		if (this.inputField == null)
		{
			return;
		}
		this.inputField.text = text;
		this.inputField.Select();
	}

	// Token: 0x06005FF5 RID: 24565 RVA: 0x0023346C File Offset: 0x0023166C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.confirmButton.onClick += this.OnConfirm;
		this.cancelButton.onClick += this.OnCancel;
		this.closeButton.onClick += this.OnCancel;
		this.inputField.onValueChanged.AddListener(delegate(string <p0>)
		{
			Util.ScrubInputField(this.inputField, false, false);
		});
		this.inputField.onEndEdit.AddListener(new UnityAction<string>(this.OnEndEdit));
	}

	// Token: 0x06005FF6 RID: 24566 RVA: 0x002334FC File Offset: 0x002316FC
	protected override void OnActivate()
	{
		base.OnActivate();
		this.inputField.Select();
		this.inputField.ActivateInputField();
		CameraController.Instance.DisableUserCameraControl = true;
	}

	// Token: 0x06005FF7 RID: 24567 RVA: 0x00233525 File Offset: 0x00231725
	protected override void OnDeactivate()
	{
		CameraController.Instance.DisableUserCameraControl = false;
		base.OnDeactivate();
	}

	// Token: 0x06005FF8 RID: 24568 RVA: 0x00233538 File Offset: 0x00231738
	public void OnConfirm()
	{
		if (this.onConfirm != null && !string.IsNullOrEmpty(this.inputField.text))
		{
			string text = this.inputField.text;
			if (!text.EndsWith(".sav"))
			{
				text += ".sav";
			}
			this.onConfirm(text);
			this.Deactivate();
		}
	}

	// Token: 0x06005FF9 RID: 24569 RVA: 0x00233596 File Offset: 0x00231796
	private void OnEndEdit(string str)
	{
		if (Localization.HasDirtyWords(str))
		{
			this.inputField.text = "";
		}
	}

	// Token: 0x06005FFA RID: 24570 RVA: 0x002335B0 File Offset: 0x002317B0
	public void OnCancel()
	{
		if (this.onCancel != null)
		{
			this.onCancel();
		}
		this.Deactivate();
	}

	// Token: 0x06005FFB RID: 24571 RVA: 0x002335CB File Offset: 0x002317CB
	public override void OnKeyUp(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape))
		{
			this.Deactivate();
		}
		else if (e.TryConsume(global::Action.DialogSubmit))
		{
			this.OnConfirm();
		}
		e.Consumed = true;
	}

	// Token: 0x06005FFC RID: 24572 RVA: 0x002335F8 File Offset: 0x002317F8
	public override void OnKeyDown(KButtonEvent e)
	{
		e.Consumed = true;
	}

	// Token: 0x0400401B RID: 16411
	public Action<string> onConfirm;

	// Token: 0x0400401C RID: 16412
	public System.Action onCancel;

	// Token: 0x0400401D RID: 16413
	[SerializeField]
	private KInputTextField inputField;

	// Token: 0x0400401E RID: 16414
	[SerializeField]
	private KButton confirmButton;

	// Token: 0x0400401F RID: 16415
	[SerializeField]
	private KButton cancelButton;

	// Token: 0x04004020 RID: 16416
	[SerializeField]
	private KButton closeButton;
}
