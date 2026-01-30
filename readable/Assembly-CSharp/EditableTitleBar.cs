using System;
using System.Collections;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000D06 RID: 3334
public class EditableTitleBar : TitleBar
{
	// Token: 0x14000028 RID: 40
	// (add) Token: 0x06006729 RID: 26409 RVA: 0x0026E1B8 File Offset: 0x0026C3B8
	// (remove) Token: 0x0600672A RID: 26410 RVA: 0x0026E1F0 File Offset: 0x0026C3F0
	public event Action<string> OnNameChanged;

	// Token: 0x14000029 RID: 41
	// (add) Token: 0x0600672B RID: 26411 RVA: 0x0026E228 File Offset: 0x0026C428
	// (remove) Token: 0x0600672C RID: 26412 RVA: 0x0026E260 File Offset: 0x0026C460
	public event System.Action OnStartedEditing;

	// Token: 0x0600672D RID: 26413 RVA: 0x0026E298 File Offset: 0x0026C498
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.randomNameButton != null)
		{
			this.randomNameButton.onClick += this.GenerateRandomName;
		}
		if (this.editNameButton != null)
		{
			this.EnableEditButtonClick();
		}
		if (this.inputField != null)
		{
			this.inputField.onEndEdit.AddListener(new UnityAction<string>(this.OnEndEdit));
		}
	}

	// Token: 0x0600672E RID: 26414 RVA: 0x0026E310 File Offset: 0x0026C510
	public void UpdateRenameTooltip(GameObject target)
	{
		if (this.editNameButton != null && target != null)
		{
			if (target.GetComponent<MinionBrain>() != null)
			{
				this.editNameButton.GetComponent<ToolTip>().toolTip = UI.TOOLTIPS.EDITNAME;
			}
			if (target.GetComponent<ClustercraftExteriorDoor>() != null || target.GetComponent<CommandModule>() != null)
			{
				this.editNameButton.GetComponent<ToolTip>().toolTip = UI.TOOLTIPS.EDITNAMEROCKET;
				return;
			}
			this.editNameButton.GetComponent<ToolTip>().toolTip = string.Format(UI.TOOLTIPS.EDITNAMEGENERIC, target.GetProperName());
		}
	}

	// Token: 0x0600672F RID: 26415 RVA: 0x0026E3C0 File Offset: 0x0026C5C0
	private void OnEndEdit(string finalStr)
	{
		finalStr = Localization.FilterDirtyWords(finalStr);
		this.SetEditingState(false);
		if (string.IsNullOrEmpty(finalStr))
		{
			return;
		}
		if (this.OnNameChanged != null)
		{
			this.OnNameChanged(finalStr);
		}
		this.titleText.text = finalStr;
		if (this.postEndEdit != null)
		{
			base.StopCoroutine(this.postEndEdit);
		}
		if (base.gameObject.activeInHierarchy && base.enabled)
		{
			this.postEndEdit = base.StartCoroutine(this.PostOnEndEditRoutine());
		}
	}

	// Token: 0x06006730 RID: 26416 RVA: 0x0026E440 File Offset: 0x0026C640
	private IEnumerator PostOnEndEditRoutine()
	{
		int i = 0;
		while (i < 10)
		{
			int num = i;
			i = num + 1;
			yield return SequenceUtil.WaitForEndOfFrame;
		}
		this.EnableEditButtonClick();
		if (this.randomNameButton != null)
		{
			this.randomNameButton.gameObject.SetActive(false);
		}
		yield break;
	}

	// Token: 0x06006731 RID: 26417 RVA: 0x0026E44F File Offset: 0x0026C64F
	private IEnumerator PreToggleNameEditingRoutine()
	{
		yield return SequenceUtil.WaitForEndOfFrame;
		this.ToggleNameEditing();
		this.preToggleNameEditing = null;
		yield break;
	}

	// Token: 0x06006732 RID: 26418 RVA: 0x0026E45E File Offset: 0x0026C65E
	private void EnableEditButtonClick()
	{
		this.editNameButton.onClick += delegate()
		{
			if (this.preToggleNameEditing != null)
			{
				return;
			}
			this.preToggleNameEditing = base.StartCoroutine(this.PreToggleNameEditingRoutine());
		};
	}

	// Token: 0x06006733 RID: 26419 RVA: 0x0026E478 File Offset: 0x0026C678
	private void GenerateRandomName()
	{
		if (this.postEndEdit != null)
		{
			base.StopCoroutine(this.postEndEdit);
		}
		string text = GameUtil.GenerateRandomDuplicantName();
		if (this.OnNameChanged != null)
		{
			this.OnNameChanged(text);
		}
		this.titleText.text = text;
		this.SetEditingState(true);
	}

	// Token: 0x06006734 RID: 26420 RVA: 0x0026E4C8 File Offset: 0x0026C6C8
	private void ToggleNameEditing()
	{
		this.editNameButton.ClearOnClick();
		bool flag = !this.inputField.gameObject.activeInHierarchy;
		if (this.randomNameButton != null)
		{
			this.randomNameButton.gameObject.SetActive(flag);
		}
		this.SetEditingState(flag);
	}

	// Token: 0x06006735 RID: 26421 RVA: 0x0026E51C File Offset: 0x0026C71C
	private void SetEditingState(bool state)
	{
		this.titleText.gameObject.SetActive(!state);
		if (this.setCameraControllerState)
		{
			CameraController.Instance.DisableUserCameraControl = state;
		}
		if (this.inputField == null)
		{
			return;
		}
		this.inputField.gameObject.SetActive(state);
		if (state)
		{
			this.inputField.text = this.titleText.text;
			this.inputField.Select();
			this.inputField.ActivateInputField();
			if (this.OnStartedEditing != null)
			{
				this.OnStartedEditing();
				return;
			}
		}
		else
		{
			this.inputField.DeactivateInputField();
		}
	}

	// Token: 0x06006736 RID: 26422 RVA: 0x0026E5BE File Offset: 0x0026C7BE
	public void ForceStopEditing()
	{
		if (this.postEndEdit != null)
		{
			base.StopCoroutine(this.postEndEdit);
		}
		this.editNameButton.ClearOnClick();
		this.SetEditingState(false);
		this.EnableEditButtonClick();
	}

	// Token: 0x06006737 RID: 26423 RVA: 0x0026E5EC File Offset: 0x0026C7EC
	public void SetUserEditable(bool editable)
	{
		this.userEditable = editable;
		this.editNameButton.gameObject.SetActive(editable);
		this.editNameButton.ClearOnClick();
		this.EnableEditButtonClick();
	}

	// Token: 0x040046A5 RID: 18085
	public KButton editNameButton;

	// Token: 0x040046A6 RID: 18086
	public KButton randomNameButton;

	// Token: 0x040046A7 RID: 18087
	public KInputTextField inputField;

	// Token: 0x040046AA RID: 18090
	private Coroutine postEndEdit;

	// Token: 0x040046AB RID: 18091
	private Coroutine preToggleNameEditing;
}
