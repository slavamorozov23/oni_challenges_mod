using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000593 RID: 1427
[AddComponentMenu("KMonoBehaviour/Workable/Activatable")]
public class Activatable : Workable, ISidescreenButtonControl
{
	// Token: 0x17000127 RID: 295
	// (get) Token: 0x06001FEE RID: 8174 RVA: 0x000B8D13 File Offset: 0x000B6F13
	public bool IsActivated
	{
		get
		{
			return this.activated;
		}
	}

	// Token: 0x06001FEF RID: 8175 RVA: 0x000B8D1B File Offset: 0x000B6F1B
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06001FF0 RID: 8176 RVA: 0x000B8D23 File Offset: 0x000B6F23
	protected override void OnSpawn()
	{
		this.UpdateFlag();
		if (this.awaitingActivation && this.activateChore == null && (this.activationCondition == null || this.activationCondition()))
		{
			this.CreateChore();
		}
	}

	// Token: 0x06001FF1 RID: 8177 RVA: 0x000B8D56 File Offset: 0x000B6F56
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.activated = true;
		if (this.onActivate != null)
		{
			this.onActivate();
		}
		this.awaitingActivation = false;
		this.UpdateFlag();
		Prioritizable.RemoveRef(base.gameObject);
		base.OnCompleteWork(worker);
	}

	// Token: 0x06001FF2 RID: 8178 RVA: 0x000B8D94 File Offset: 0x000B6F94
	private void UpdateFlag()
	{
		base.GetComponent<Operational>().SetFlag(this.Required ? Activatable.activatedFlagRequirement : Activatable.activatedFlagFunctional, this.activated);
		base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.DuplicantActivationRequired, !this.activated, null);
		base.Trigger(-1909216579, BoxedBools.Box(this.IsActivated));
	}

	// Token: 0x06001FF3 RID: 8179 RVA: 0x000B8E04 File Offset: 0x000B7004
	private void CreateChore()
	{
		if (this.activateChore != null)
		{
			return;
		}
		Prioritizable.AddRef(base.gameObject);
		this.activateChore = new WorkChore<Activatable>(Db.Get().ChoreTypes.Toggle, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		if (!string.IsNullOrEmpty(this.requiredSkillPerk))
		{
			this.shouldShowSkillPerkStatusItem = true;
			this.requireMinionToWork = true;
			this.UpdateStatusItem(null);
		}
	}

	// Token: 0x06001FF4 RID: 8180 RVA: 0x000B8E73 File Offset: 0x000B7073
	public void CancelChore()
	{
		if (this.activateChore == null)
		{
			return;
		}
		this.activateChore.Cancel("User cancelled");
		this.activateChore = null;
	}

	// Token: 0x06001FF5 RID: 8181 RVA: 0x000B8E95 File Offset: 0x000B7095
	public int HorizontalGroupID()
	{
		return -1;
	}

	// Token: 0x17000128 RID: 296
	// (get) Token: 0x06001FF6 RID: 8182 RVA: 0x000B8E98 File Offset: 0x000B7098
	public string SidescreenButtonText
	{
		get
		{
			if (this.activateChore != null)
			{
				return this.textOverride.IsValid ? this.textOverride.CancelText : UI.USERMENUACTIONS.ACTIVATEBUILDING.ACTIVATE_CANCEL;
			}
			return this.textOverride.IsValid ? this.textOverride.Text : UI.USERMENUACTIONS.ACTIVATEBUILDING.ACTIVATE;
		}
	}

	// Token: 0x17000129 RID: 297
	// (get) Token: 0x06001FF7 RID: 8183 RVA: 0x000B8EF8 File Offset: 0x000B70F8
	public string SidescreenButtonTooltip
	{
		get
		{
			if (this.activateChore != null)
			{
				return this.textOverride.IsValid ? this.textOverride.CancelToolTip : UI.USERMENUACTIONS.ACTIVATEBUILDING.TOOLTIP_CANCEL;
			}
			return this.textOverride.IsValid ? this.textOverride.ToolTip : UI.USERMENUACTIONS.ACTIVATEBUILDING.TOOLTIP_ACTIVATE;
		}
	}

	// Token: 0x06001FF8 RID: 8184 RVA: 0x000B8F56 File Offset: 0x000B7156
	public bool SidescreenEnabled()
	{
		return !this.activated;
	}

	// Token: 0x06001FF9 RID: 8185 RVA: 0x000B8F61 File Offset: 0x000B7161
	public void SetButtonTextOverride(ButtonMenuTextOverride text)
	{
		this.textOverride = text;
	}

	// Token: 0x06001FFA RID: 8186 RVA: 0x000B8F6A File Offset: 0x000B716A
	public void OnSidescreenButtonPressed()
	{
		if (this.activateChore == null)
		{
			this.CreateChore();
		}
		else
		{
			this.CancelChore();
		}
		this.awaitingActivation = (this.activateChore != null);
	}

	// Token: 0x06001FFB RID: 8187 RVA: 0x000B8F91 File Offset: 0x000B7191
	public bool SidescreenButtonInteractable()
	{
		return !this.activated && (this.activationCondition == null || this.activationCondition());
	}

	// Token: 0x06001FFC RID: 8188 RVA: 0x000B8FB2 File Offset: 0x000B71B2
	public int ButtonSideScreenSortOrder()
	{
		return 20;
	}

	// Token: 0x04001296 RID: 4758
	public bool Required = true;

	// Token: 0x04001297 RID: 4759
	private static readonly Operational.Flag activatedFlagRequirement = new Operational.Flag("activated", Operational.Flag.Type.Requirement);

	// Token: 0x04001298 RID: 4760
	private static readonly Operational.Flag activatedFlagFunctional = new Operational.Flag("activated", Operational.Flag.Type.Functional);

	// Token: 0x04001299 RID: 4761
	[Serialize]
	private bool activated;

	// Token: 0x0400129A RID: 4762
	[Serialize]
	private bool awaitingActivation;

	// Token: 0x0400129B RID: 4763
	public Func<bool> activationCondition;

	// Token: 0x0400129C RID: 4764
	private Guid statusItem;

	// Token: 0x0400129D RID: 4765
	private Chore activateChore;

	// Token: 0x0400129E RID: 4766
	public System.Action onActivate;

	// Token: 0x0400129F RID: 4767
	[SerializeField]
	private ButtonMenuTextOverride textOverride;
}
