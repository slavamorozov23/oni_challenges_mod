using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E90 RID: 3728
public class WarpPortalSideScreen : SideScreenContent
{
	// Token: 0x06007704 RID: 30468 RVA: 0x002D5C9C File Offset: 0x002D3E9C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.buttonLabel.SetText(UI.UISIDESCREENS.WARPPORTALSIDESCREEN.BUTTON);
		this.cancelButtonLabel.SetText(UI.UISIDESCREENS.WARPPORTALSIDESCREEN.CANCELBUTTON);
		this.button.onClick += this.OnButtonClick;
		this.cancelButton.onClick += this.OnCancelClick;
		this.Refresh(null);
	}

	// Token: 0x06007705 RID: 30469 RVA: 0x002D5D0E File Offset: 0x002D3F0E
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<WarpPortal>() != null;
	}

	// Token: 0x06007706 RID: 30470 RVA: 0x002D5D1C File Offset: 0x002D3F1C
	public override void SetTarget(GameObject target)
	{
		WarpPortal component = target.GetComponent<WarpPortal>();
		if (component == null)
		{
			global::Debug.LogError("Target doesn't have a WarpPortal associated with it.");
			return;
		}
		this.target = component;
		target.GetComponent<Assignable>().OnAssign += new Action<IAssignableIdentity>(this.Refresh);
		this.Refresh(null);
	}

	// Token: 0x06007707 RID: 30471 RVA: 0x002D5D6C File Offset: 0x002D3F6C
	private void Update()
	{
		if (this.progressBar.activeSelf)
		{
			RectTransform rectTransform = this.progressBar.GetComponentsInChildren<Image>()[1].rectTransform;
			float num = this.target.rechargeProgress / 3000f;
			rectTransform.sizeDelta = new Vector2(rectTransform.transform.parent.GetComponent<LayoutElement>().minWidth * num, 24f);
			this.progressLabel.text = GameUtil.GetFormattedPercent(num * 100f, GameUtil.TimeSlice.None);
		}
	}

	// Token: 0x06007708 RID: 30472 RVA: 0x002D5DE8 File Offset: 0x002D3FE8
	private void OnButtonClick()
	{
		if (this.target.ReadyToWarp)
		{
			this.target.StartWarpSequence();
			this.Refresh(null);
		}
	}

	// Token: 0x06007709 RID: 30473 RVA: 0x002D5E09 File Offset: 0x002D4009
	private void OnCancelClick()
	{
		this.target.CancelAssignment();
		this.Refresh(null);
	}

	// Token: 0x0600770A RID: 30474 RVA: 0x002D5E20 File Offset: 0x002D4020
	private void Refresh(object data = null)
	{
		this.progressBar.SetActive(false);
		this.cancelButton.gameObject.SetActive(false);
		if (!(this.target != null))
		{
			this.label.text = UI.UISIDESCREENS.WARPPORTALSIDESCREEN.IDLE;
			this.button.gameObject.SetActive(false);
			return;
		}
		if (this.target.ReadyToWarp)
		{
			this.label.text = UI.UISIDESCREENS.WARPPORTALSIDESCREEN.WAITING;
			this.button.gameObject.SetActive(true);
			this.cancelButton.gameObject.SetActive(true);
			return;
		}
		if (this.target.IsConsumed)
		{
			this.button.gameObject.SetActive(false);
			this.progressBar.SetActive(true);
			this.label.text = UI.UISIDESCREENS.WARPPORTALSIDESCREEN.CONSUMED;
			return;
		}
		if (this.target.IsWorking)
		{
			this.label.text = UI.UISIDESCREENS.WARPPORTALSIDESCREEN.UNDERWAY;
			this.button.gameObject.SetActive(false);
			this.cancelButton.gameObject.SetActive(true);
			return;
		}
		this.label.text = UI.UISIDESCREENS.WARPPORTALSIDESCREEN.IDLE;
		this.button.gameObject.SetActive(false);
	}

	// Token: 0x04005259 RID: 21081
	[SerializeField]
	private LocText label;

	// Token: 0x0400525A RID: 21082
	[SerializeField]
	private KButton button;

	// Token: 0x0400525B RID: 21083
	[SerializeField]
	private LocText buttonLabel;

	// Token: 0x0400525C RID: 21084
	[SerializeField]
	private KButton cancelButton;

	// Token: 0x0400525D RID: 21085
	[SerializeField]
	private LocText cancelButtonLabel;

	// Token: 0x0400525E RID: 21086
	[SerializeField]
	private WarpPortal target;

	// Token: 0x0400525F RID: 21087
	[SerializeField]
	private GameObject contents;

	// Token: 0x04005260 RID: 21088
	[SerializeField]
	private GameObject progressBar;

	// Token: 0x04005261 RID: 21089
	[SerializeField]
	private LocText progressLabel;
}
