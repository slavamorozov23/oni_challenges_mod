using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000E3E RID: 3646
public class GeneShufflerSideScreen : SideScreenContent
{
	// Token: 0x0600739F RID: 29599 RVA: 0x002C2619 File Offset: 0x002C0819
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.button.onClick += this.OnButtonClick;
		this.Refresh();
	}

	// Token: 0x060073A0 RID: 29600 RVA: 0x002C263E File Offset: 0x002C083E
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<GeneShuffler>() != null;
	}

	// Token: 0x060073A1 RID: 29601 RVA: 0x002C264C File Offset: 0x002C084C
	public override void SetTarget(GameObject target)
	{
		GeneShuffler component = target.GetComponent<GeneShuffler>();
		if (component == null)
		{
			global::Debug.LogError("Target doesn't have a GeneShuffler associated with it.");
			return;
		}
		this.target = component;
		this.Refresh();
	}

	// Token: 0x060073A2 RID: 29602 RVA: 0x002C2684 File Offset: 0x002C0884
	private void OnButtonClick()
	{
		if (this.target.WorkComplete)
		{
			this.target.SetWorkTime(0f);
			return;
		}
		if (this.target.IsConsumed)
		{
			this.target.RequestRecharge(!this.target.RechargeRequested);
			this.Refresh();
		}
	}

	// Token: 0x060073A3 RID: 29603 RVA: 0x002C26DC File Offset: 0x002C08DC
	private void Refresh()
	{
		if (!(this.target != null))
		{
			this.contents.SetActive(false);
			return;
		}
		if (this.target.WorkComplete)
		{
			this.contents.SetActive(true);
			this.label.text = UI.UISIDESCREENS.GENESHUFFLERSIDESREEN.COMPLETE;
			this.button.gameObject.SetActive(true);
			this.buttonLabel.text = UI.UISIDESCREENS.GENESHUFFLERSIDESREEN.BUTTON;
			return;
		}
		if (this.target.IsConsumed)
		{
			this.contents.SetActive(true);
			this.button.gameObject.SetActive(true);
			if (this.target.RechargeRequested)
			{
				this.label.text = UI.UISIDESCREENS.GENESHUFFLERSIDESREEN.CONSUMED_WAITING;
				this.buttonLabel.text = UI.UISIDESCREENS.GENESHUFFLERSIDESREEN.BUTTON_RECHARGE_CANCEL;
				return;
			}
			this.label.text = UI.UISIDESCREENS.GENESHUFFLERSIDESREEN.CONSUMED;
			this.buttonLabel.text = UI.UISIDESCREENS.GENESHUFFLERSIDESREEN.BUTTON_RECHARGE;
			return;
		}
		else
		{
			if (this.target.IsWorking)
			{
				this.contents.SetActive(true);
				this.label.text = UI.UISIDESCREENS.GENESHUFFLERSIDESREEN.UNDERWAY;
				this.button.gameObject.SetActive(false);
				return;
			}
			this.contents.SetActive(false);
			return;
		}
	}

	// Token: 0x04004FF4 RID: 20468
	[SerializeField]
	private LocText label;

	// Token: 0x04004FF5 RID: 20469
	[SerializeField]
	private KButton button;

	// Token: 0x04004FF6 RID: 20470
	[SerializeField]
	private LocText buttonLabel;

	// Token: 0x04004FF7 RID: 20471
	[SerializeField]
	private GeneShuffler target;

	// Token: 0x04004FF8 RID: 20472
	[SerializeField]
	private GameObject contents;
}
