using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000E5E RID: 3678
public class OwnablesSidescreenItemRow : KMonoBehaviour
{
	// Token: 0x17000816 RID: 2070
	// (get) Token: 0x060074B0 RID: 29872 RVA: 0x002C8300 File Offset: 0x002C6500
	// (set) Token: 0x060074AF RID: 29871 RVA: 0x002C82F7 File Offset: 0x002C64F7
	public bool IsLocked { get; private set; }

	// Token: 0x17000817 RID: 2071
	// (get) Token: 0x060074B1 RID: 29873 RVA: 0x002C8308 File Offset: 0x002C6508
	public bool SlotIsAssigned
	{
		get
		{
			return this.Slot != null && this.SlotInstance != null && !this.SlotInstance.IsUnassigning() && this.SlotInstance.IsAssigned();
		}
	}

	// Token: 0x17000818 RID: 2072
	// (get) Token: 0x060074B3 RID: 29875 RVA: 0x002C833D File Offset: 0x002C653D
	// (set) Token: 0x060074B2 RID: 29874 RVA: 0x002C8334 File Offset: 0x002C6534
	public AssignableSlotInstance SlotInstance { get; private set; }

	// Token: 0x17000819 RID: 2073
	// (get) Token: 0x060074B5 RID: 29877 RVA: 0x002C834E File Offset: 0x002C654E
	// (set) Token: 0x060074B4 RID: 29876 RVA: 0x002C8345 File Offset: 0x002C6545
	public AssignableSlot Slot { get; private set; }

	// Token: 0x1700081A RID: 2074
	// (get) Token: 0x060074B7 RID: 29879 RVA: 0x002C835F File Offset: 0x002C655F
	// (set) Token: 0x060074B6 RID: 29878 RVA: 0x002C8356 File Offset: 0x002C6556
	public Assignables Owner { get; private set; }

	// Token: 0x060074B8 RID: 29880 RVA: 0x002C8367 File Offset: 0x002C6567
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MultiToggle multiToggle = this.toggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.OnRowClicked));
		this.SetSelectedVisualState(false);
	}

	// Token: 0x060074B9 RID: 29881 RVA: 0x002C839D File Offset: 0x002C659D
	private void OnRowClicked()
	{
		Action<OwnablesSidescreenItemRow> onSlotRowClicked = this.OnSlotRowClicked;
		if (onSlotRowClicked == null)
		{
			return;
		}
		onSlotRowClicked(this);
	}

	// Token: 0x060074BA RID: 29882 RVA: 0x002C83B0 File Offset: 0x002C65B0
	public void SetLockState(bool locked)
	{
		this.IsLocked = locked;
		this.Refresh();
	}

	// Token: 0x060074BB RID: 29883 RVA: 0x002C83C0 File Offset: 0x002C65C0
	public void SetData(Assignables owner, AssignableSlot slot, bool IsLocked)
	{
		if (this.Owner != null)
		{
			this.ClearData();
		}
		this.Owner = owner;
		this.Slot = slot;
		this.SlotInstance = owner.GetSlot(slot);
		this.subscribe_IDX = this.Owner.Subscribe(-1585839766, delegate(object o)
		{
			this.Refresh();
		});
		this.SetLockState(IsLocked);
		if (!IsLocked)
		{
			this.Refresh();
		}
	}

	// Token: 0x060074BC RID: 29884 RVA: 0x002C8430 File Offset: 0x002C6630
	public void ClearData()
	{
		if (this.Owner != null && this.subscribe_IDX != -1)
		{
			this.Owner.Unsubscribe(this.subscribe_IDX);
		}
		this.Owner = null;
		this.Slot = null;
		this.SlotInstance = null;
		this.IsLocked = false;
		this.subscribe_IDX = -1;
		this.DisplayAsEmpty();
	}

	// Token: 0x060074BD RID: 29885 RVA: 0x002C848E File Offset: 0x002C668E
	private void Refresh()
	{
		if (this.IsNullOrDestroyed())
		{
			return;
		}
		if (this.IsLocked)
		{
			this.DisplayAsLocked();
			return;
		}
		if (!this.SlotIsAssigned)
		{
			this.DisplayAsEmpty();
			return;
		}
		this.DisplayAsOccupied();
	}

	// Token: 0x060074BE RID: 29886 RVA: 0x002C84C0 File Offset: 0x002C66C0
	public void SetSelectedVisualState(bool shouldDisplayAsSelected)
	{
		int new_state_index = shouldDisplayAsSelected ? 1 : 0;
		this.toggle.ChangeState(new_state_index);
	}

	// Token: 0x060074BF RID: 29887 RVA: 0x002C84E4 File Offset: 0x002C66E4
	private void DisplayAsOccupied()
	{
		Assignable assignable = this.SlotInstance.assignable;
		string properName = assignable.GetProperName();
		string text = this.Slot.Name + ": " + properName;
		this.textLabel.SetText(text);
		this.itemIcon.sprite = Def.GetUISprite(assignable.gameObject, "ui", false).first;
		this.itemIcon.gameObject.SetActive(true);
		this.lockedIcon.gameObject.SetActive(false);
		InfoDescription component = assignable.gameObject.GetComponent<InfoDescription>();
		string simpleTooltip = string.Format(UI.UISIDESCREENS.OWNABLESSIDESCREEN.TOOLTIPS.ITEM_ASSIGNED_GENERIC, properName);
		if (component != null && !string.IsNullOrEmpty(component.description))
		{
			simpleTooltip = string.Format(UI.UISIDESCREENS.OWNABLESSIDESCREEN.TOOLTIPS.ITEM_ASSIGNED, properName, component.description);
		}
		this.tooltip.SetSimpleTooltip(simpleTooltip);
	}

	// Token: 0x060074C0 RID: 29888 RVA: 0x002C85C4 File Offset: 0x002C67C4
	private void DisplayAsEmpty()
	{
		this.textLabel.SetText(((this.Slot != null) ? (this.Slot.Name + ": ") : "") + OwnablesSidescreenItemRow.EMPTY_TEXT);
		this.lockedIcon.gameObject.SetActive(false);
		this.itemIcon.sprite = null;
		this.itemIcon.gameObject.SetActive(false);
		this.tooltip.SetSimpleTooltip((this.Slot != null) ? string.Format(UI.UISIDESCREENS.OWNABLESSIDESCREEN.TOOLTIPS.NO_ITEM_ASSIGNED, this.Slot.Name) : null);
	}

	// Token: 0x060074C1 RID: 29889 RVA: 0x002C8668 File Offset: 0x002C6868
	private void DisplayAsLocked()
	{
		this.lockedIcon.gameObject.SetActive(true);
		this.itemIcon.sprite = null;
		this.itemIcon.gameObject.SetActive(false);
		this.textLabel.SetText(string.Format(UI.UISIDESCREENS.OWNABLESSIDESCREEN.NO_APPLICABLE, this.Slot.Name));
		this.tooltip.SetSimpleTooltip(string.Format(UI.UISIDESCREENS.OWNABLESSIDESCREEN.TOOLTIPS.NO_APPLICABLE, this.Slot.Name));
	}

	// Token: 0x060074C2 RID: 29890 RVA: 0x002C86ED File Offset: 0x002C68ED
	protected override void OnCleanUp()
	{
		this.ClearData();
	}

	// Token: 0x040050AB RID: 20651
	private static string EMPTY_TEXT = UI.UISIDESCREENS.OWNABLESSIDESCREEN.NO_ITEM_ASSIGNED;

	// Token: 0x040050AC RID: 20652
	public KImage lockedIcon;

	// Token: 0x040050AD RID: 20653
	public KImage itemIcon;

	// Token: 0x040050AE RID: 20654
	public LocText textLabel;

	// Token: 0x040050AF RID: 20655
	public ToolTip tooltip;

	// Token: 0x040050B0 RID: 20656
	[Header("Icon settings")]
	public KImage frameOuterBorder;

	// Token: 0x040050B1 RID: 20657
	public Action<OwnablesSidescreenItemRow> OnSlotRowClicked;

	// Token: 0x040050B6 RID: 20662
	public MultiToggle toggle;

	// Token: 0x040050B7 RID: 20663
	private int subscribe_IDX = -1;
}
