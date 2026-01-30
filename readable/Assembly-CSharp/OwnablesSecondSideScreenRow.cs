using System;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000E5B RID: 3675
public class OwnablesSecondSideScreenRow : KMonoBehaviour
{
	// Token: 0x17000813 RID: 2067
	// (get) Token: 0x06007486 RID: 29830 RVA: 0x002C76F5 File Offset: 0x002C58F5
	// (set) Token: 0x06007485 RID: 29829 RVA: 0x002C76EC File Offset: 0x002C58EC
	public AssignableSlotInstance minionSlotInstance { get; private set; }

	// Token: 0x17000814 RID: 2068
	// (get) Token: 0x06007488 RID: 29832 RVA: 0x002C7706 File Offset: 0x002C5906
	// (set) Token: 0x06007487 RID: 29831 RVA: 0x002C76FD File Offset: 0x002C58FD
	public Assignable item { get; private set; }

	// Token: 0x06007489 RID: 29833 RVA: 0x002C7710 File Offset: 0x002C5910
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.toggle = base.GetComponent<MultiToggle>();
		MultiToggle multiToggle = this.toggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.OnMultitoggleClicked));
		this.eyeButton.onClick.AddListener(new UnityAction(this.FocusCameraOnAssignedItem));
	}

	// Token: 0x0600748A RID: 29834 RVA: 0x002C7774 File Offset: 0x002C5974
	public void SetData(AssignableSlotInstance minion, Assignable item_assignable)
	{
		this.minionSlotInstance = minion;
		this.item = item_assignable;
		this.changeAssignmentListenerIDX = this.item.Subscribe(684616645, new Action<object>(this._OnItemAssignationChanged));
		this.destroyListenerIDX = this.item.Subscribe(1969584890, new Action<object>(this._OnRowItemDestroyed));
		this.customTooltipFunc = this.item.customAssignmentUITooltipFunc;
		this.Refresh();
	}

	// Token: 0x0600748B RID: 29835 RVA: 0x002C77EC File Offset: 0x002C59EC
	public void Refresh()
	{
		if (this.item != null)
		{
			this.item.PrefabID();
			string properName = this.item.GetProperName();
			this.nameLabel.text = properName;
			this.icon.sprite = Def.GetUISprite(this.item.gameObject, "ui", false).first;
			bool flag = this.item.IsAssigned() && !this.minionSlotInstance.IsUnassigning() && this.minionSlotInstance.assignable != this.item;
			if (this.item.IsAssigned())
			{
				this.statusLabel.SetText(string.Format(flag ? OwnablesSecondSideScreenRow.ASSIGNED_TO_OTHER : OwnablesSecondSideScreenRow.ASSIGNED_TO_SELF, this.item.assignee.GetProperName()));
			}
			else
			{
				this.statusLabel.SetText(OwnablesSecondSideScreenRow.NOT_ASSIGNED);
			}
			if (this.customTooltipFunc == null)
			{
				InfoDescription component = this.item.gameObject.GetComponent<InfoDescription>();
				bool flag2 = component != null && !string.IsNullOrEmpty(component.description);
				string simpleTooltip = flag2 ? component.description : properName;
				this.tooltip.SizingSetting = (flag2 ? ToolTip.ToolTipSizeSetting.MaxWidthWrapContent : ToolTip.ToolTipSizeSetting.DynamicWidthNoWrap);
				this.tooltip.SetSimpleTooltip(simpleTooltip);
			}
			else
			{
				this.tooltip.SizingSetting = ToolTip.ToolTipSizeSetting.MaxWidthWrapContent;
				this.tooltip.SetSimpleTooltip(this.customTooltipFunc(this.minionSlotInstance.assignables));
			}
		}
		else
		{
			this.nameLabel.text = OwnablesSecondSideScreenRow.NO_DATA_MESSAGE;
			this.tooltip.SetSimpleTooltip(null);
		}
		bool flag3 = this.item != null && this.minionSlotInstance != null && !this.minionSlotInstance.IsUnassigning() && this.minionSlotInstance.assignable == this.item;
		this.toggle.ChangeState(flag3 ? 1 : 0);
		this.emptyIcon.gameObject.SetActive(this.item == null);
		this.icon.gameObject.SetActive(this.item != null);
		this.eyeButton.gameObject.SetActive(this.item != null);
		this.statusLabel.gameObject.SetActive(this.item != null);
	}

	// Token: 0x0600748C RID: 29836 RVA: 0x002C7A48 File Offset: 0x002C5C48
	public void ClearData()
	{
		if (this.item != null)
		{
			if (this.destroyListenerIDX != -1)
			{
				this.item.Unsubscribe(this.destroyListenerIDX);
			}
			if (this.changeAssignmentListenerIDX != -1)
			{
				this.item.Unsubscribe(this.changeAssignmentListenerIDX);
			}
		}
		this.minionSlotInstance = null;
		this.item = null;
		this.destroyListenerIDX = -1;
		this.changeAssignmentListenerIDX = -1;
		this.Refresh();
	}

	// Token: 0x0600748D RID: 29837 RVA: 0x002C7AB9 File Offset: 0x002C5CB9
	private void _OnItemAssignationChanged(object o)
	{
		Action<OwnablesSecondSideScreenRow> onRowItemAssigneeChanged = this.OnRowItemAssigneeChanged;
		if (onRowItemAssigneeChanged == null)
		{
			return;
		}
		onRowItemAssigneeChanged(this);
	}

	// Token: 0x0600748E RID: 29838 RVA: 0x002C7ACC File Offset: 0x002C5CCC
	private void _OnRowItemDestroyed(object o)
	{
		Action<OwnablesSecondSideScreenRow> onRowItemDestroyed = this.OnRowItemDestroyed;
		if (onRowItemDestroyed == null)
		{
			return;
		}
		onRowItemDestroyed(this);
	}

	// Token: 0x0600748F RID: 29839 RVA: 0x002C7ADF File Offset: 0x002C5CDF
	private void OnMultitoggleClicked()
	{
		Action<OwnablesSecondSideScreenRow> onRowClicked = this.OnRowClicked;
		if (onRowClicked == null)
		{
			return;
		}
		onRowClicked(this);
	}

	// Token: 0x06007490 RID: 29840 RVA: 0x002C7AF4 File Offset: 0x002C5CF4
	private void FocusCameraOnAssignedItem()
	{
		if (this.item != null)
		{
			GameObject gameObject = this.item.gameObject;
			if (this.item.HasTag(GameTags.Equipped))
			{
				gameObject = this.item.assignee.GetOwners()[0].GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
			}
			GameUtil.FocusCamera(gameObject.transform, false, true);
		}
	}

	// Token: 0x04005085 RID: 20613
	public static string NO_DATA_MESSAGE = UI.UISIDESCREENS.OWNABLESSIDESCREEN.NO_ITEM_FOUND;

	// Token: 0x04005086 RID: 20614
	public static string NOT_ASSIGNED = UI.UISIDESCREENS.OWNABLESSECONDSIDESCREEN.NOT_ASSIGNED;

	// Token: 0x04005087 RID: 20615
	public static string ASSIGNED_TO_SELF = UI.UISIDESCREENS.OWNABLESSECONDSIDESCREEN.ASSIGNED_TO_SELF_STATUS;

	// Token: 0x04005088 RID: 20616
	public static string ASSIGNED_TO_OTHER = UI.UISIDESCREENS.OWNABLESSECONDSIDESCREEN.ASSIGNED_TO_OTHER_STATUS;

	// Token: 0x04005089 RID: 20617
	public KImage icon;

	// Token: 0x0400508A RID: 20618
	public KImage emptyIcon;

	// Token: 0x0400508B RID: 20619
	public LocText nameLabel;

	// Token: 0x0400508C RID: 20620
	public LocText statusLabel;

	// Token: 0x0400508D RID: 20621
	public Button eyeButton;

	// Token: 0x0400508E RID: 20622
	public ToolTip tooltip;

	// Token: 0x0400508F RID: 20623
	public Action<OwnablesSecondSideScreenRow> OnRowItemAssigneeChanged;

	// Token: 0x04005090 RID: 20624
	public Action<OwnablesSecondSideScreenRow> OnRowItemDestroyed;

	// Token: 0x04005091 RID: 20625
	public Action<OwnablesSecondSideScreenRow> OnRowClicked;

	// Token: 0x04005092 RID: 20626
	public Func<Assignables, string> customTooltipFunc;

	// Token: 0x04005095 RID: 20629
	private MultiToggle toggle;

	// Token: 0x04005096 RID: 20630
	private int changeAssignmentListenerIDX = -1;

	// Token: 0x04005097 RID: 20631
	private int destroyListenerIDX = -1;
}
