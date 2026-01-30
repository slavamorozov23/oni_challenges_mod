using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000E1D RID: 3613
public class BionicSideScreenUpgradeSlot : KMonoBehaviour
{
	// Token: 0x170007E7 RID: 2023
	// (get) Token: 0x0600729A RID: 29338 RVA: 0x002BC5B9 File Offset: 0x002BA7B9
	// (set) Token: 0x06007299 RID: 29337 RVA: 0x002BC5B0 File Offset: 0x002BA7B0
	public BionicUpgradesMonitor.UpgradeComponentSlot upgradeSlot { get; private set; }

	// Token: 0x0600729B RID: 29339 RVA: 0x002BC5C1 File Offset: 0x002BA7C1
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MultiToggle multiToggle = this.toggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.OnSlotClicked));
	}

	// Token: 0x0600729C RID: 29340 RVA: 0x002BC5F0 File Offset: 0x002BA7F0
	public void Setup(BionicUpgradesMonitor.UpgradeComponentSlot upgradeSlot)
	{
		if (this.upgradeSlot != null)
		{
			BionicUpgradesMonitor.UpgradeComponentSlot upgradeSlot2 = this.upgradeSlot;
			upgradeSlot2.OnAssignedUpgradeChanged = (Action<BionicUpgradesMonitor.UpgradeComponentSlot>)Delegate.Remove(upgradeSlot2.OnAssignedUpgradeChanged, new Action<BionicUpgradesMonitor.UpgradeComponentSlot>(this.OnAssignedUpgradeChanged));
		}
		this.upgradeSlot = upgradeSlot;
		if (upgradeSlot != null)
		{
			upgradeSlot.OnAssignedUpgradeChanged = (Action<BionicUpgradesMonitor.UpgradeComponentSlot>)Delegate.Combine(upgradeSlot.OnAssignedUpgradeChanged, new Action<BionicUpgradesMonitor.UpgradeComponentSlot>(this.OnAssignedUpgradeChanged));
		}
		this.Refresh();
	}

	// Token: 0x0600729D RID: 29341 RVA: 0x002BC65E File Offset: 0x002BA85E
	private void OnAssignedUpgradeChanged(BionicUpgradesMonitor.UpgradeComponentSlot slot)
	{
		this.Refresh();
	}

	// Token: 0x0600729E RID: 29342 RVA: 0x002BC668 File Offset: 0x002BA868
	public void Refresh()
	{
		this.label.color = this.standardColor;
		BionicSideScreenUpgradeSlot.State state = this.upgradeSlot.IsLocked ? BionicSideScreenUpgradeSlot.State.Locked : BionicSideScreenUpgradeSlot.State.Empty;
		if (state == BionicSideScreenUpgradeSlot.State.Empty && this.upgradeSlot.HasUpgradeInstalled)
		{
			state = BionicSideScreenUpgradeSlot.State.Installed;
		}
		else if (state == BionicSideScreenUpgradeSlot.State.Empty && this.upgradeSlot.HasUpgradeComponentAssigned && !this.upgradeSlot.GetAssignableSlotInstance().IsUnassigning())
		{
			state = BionicSideScreenUpgradeSlot.State.Assigned;
		}
		switch (state)
		{
		case BionicSideScreenUpgradeSlot.State.Locked:
			this.tooltip.SizingSetting = ToolTip.ToolTipSizeSetting.DynamicWidthNoWrap;
			this.tooltip.SetSimpleTooltip(BionicSideScreenUpgradeSlot.TEXT_TOOLTIP_BLOCKED);
			this.label.SetText(BionicSideScreenUpgradeSlot.TEXT_BLOCKED_SLOT);
			this.label.Opacity(0.5f);
			this.icon.gameObject.SetActive(false);
			break;
		case BionicSideScreenUpgradeSlot.State.Empty:
			this.tooltip.SizingSetting = ToolTip.ToolTipSizeSetting.DynamicWidthNoWrap;
			this.tooltip.SetSimpleTooltip(BionicSideScreenUpgradeSlot.TEXT_TOOLTIP_EMPTY);
			this.label.SetText(BionicSideScreenUpgradeSlot.TEXT_NO_UPGRADE_INSTALLED);
			this.label.Opacity(1f);
			this.icon.gameObject.SetActive(false);
			break;
		case BionicSideScreenUpgradeSlot.State.Assigned:
			this.icon.sprite = Def.GetUISprite(this.upgradeSlot.assignedUpgradeComponent.gameObject, "ui", false).first;
			this.icon.Opacity(0.5f);
			this.icon.gameObject.SetActive(true);
			this.label.SetText(BionicSideScreenUpgradeSlot.TEXT_UPGRADE_ASSIGNED_NOT_INSTALLED);
			this.label.Opacity(1f);
			this.tooltip.SizingSetting = ToolTip.ToolTipSizeSetting.MaxWidthWrapContent;
			this.tooltip.SetSimpleTooltip(string.Format(BionicSideScreenUpgradeSlot.TEXT_TOOLTIP_ASSIGNED, this.upgradeSlot.assignedUpgradeComponent.GetProperName()));
			break;
		case BionicSideScreenUpgradeSlot.State.Installed:
			this.icon.sprite = Def.GetUISprite(this.upgradeSlot.installedUpgradeComponent.gameObject, "ui", false).first;
			this.icon.Opacity(1f);
			this.icon.gameObject.SetActive(true);
			this.label.SetText(BionicSideScreenUpgradeSlot.TEXT_UPGRADE_INSTALLED);
			this.label.Opacity(1f);
			this.tooltip.SizingSetting = ToolTip.ToolTipSizeSetting.MaxWidthWrapContent;
			this.tooltip.SetSimpleTooltip(string.Format(BionicSideScreenUpgradeSlot.TEXT_TOOLTIP_INSTALLED, BionicUpgradeComponentConfig.GenerateTooltipForBooster(this.upgradeSlot.installedUpgradeComponent)));
			break;
		}
		this.SetSelected(this._isSelected);
	}

	// Token: 0x0600729F RID: 29343 RVA: 0x002BC8DC File Offset: 0x002BAADC
	private void OnSlotClicked()
	{
		Action<BionicSideScreenUpgradeSlot> onClick = this.OnClick;
		if (onClick == null)
		{
			return;
		}
		onClick(this);
	}

	// Token: 0x060072A0 RID: 29344 RVA: 0x002BC8F0 File Offset: 0x002BAAF0
	public void SetSelected(bool isSelected)
	{
		this._isSelected = isSelected;
		bool flag = this.upgradeSlot == null || this.upgradeSlot.IsLocked;
		bool flag2 = this.upgradeSlot != null && this.upgradeSlot.HasUpgradeComponentAssigned && !this.upgradeSlot.GetAssignableSlotInstance().IsUnassigning();
		bool flag3 = flag2 && this.upgradeSlot.assignedUpgradeComponent.Booster == BionicUpgradeComponentConfig.BoosterType.Basic;
		this.toggle.ChangeState((flag ? 0 : 2) + (flag2 ? 2 : 0) + ((flag2 && flag3) ? 2 : 0) + (isSelected ? 1 : 0));
	}

	// Token: 0x04004F32 RID: 20274
	public static string TEXT_BLOCKED_SLOT = UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.UPGRADE_SLOT_LOCKED;

	// Token: 0x04004F33 RID: 20275
	public static string TEXT_NO_UPGRADE_INSTALLED = UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.UPGRADE_SLOT_EMPTY;

	// Token: 0x04004F34 RID: 20276
	public static string TEXT_UPGRADE_ASSIGNED_NOT_INSTALLED = UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.UPGRADE_SLOT_ASSIGNED;

	// Token: 0x04004F35 RID: 20277
	public static string TEXT_UPGRADE_INSTALLED = UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.UPGRADE_SLOT_INSTALLED;

	// Token: 0x04004F36 RID: 20278
	public static string TEXT_TOOLTIP_BLOCKED = UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.TOOLTIP.SLOT_LOCKED;

	// Token: 0x04004F37 RID: 20279
	public static string TEXT_TOOLTIP_EMPTY = UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.TOOLTIP.SLOT_EMPTY;

	// Token: 0x04004F38 RID: 20280
	public static string TEXT_TOOLTIP_ASSIGNED = UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.TOOLTIP.SLOT_ASSIGNED;

	// Token: 0x04004F39 RID: 20281
	public static string TEXT_TOOLTIP_INSTALLED = UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.TOOLTIP.SLOT_INSTALLED;

	// Token: 0x04004F3A RID: 20282
	public MultiToggle toggle;

	// Token: 0x04004F3B RID: 20283
	public KImage icon;

	// Token: 0x04004F3C RID: 20284
	public LocText label;

	// Token: 0x04004F3D RID: 20285
	public ToolTip tooltip;

	// Token: 0x04004F3E RID: 20286
	[Header("Effects settings")]
	public float inUseAnimationDuration = 0.5f;

	// Token: 0x04004F3F RID: 20287
	public Color standardColor = Color.black;

	// Token: 0x04004F40 RID: 20288
	public Color activeColor = Color.blue;

	// Token: 0x04004F41 RID: 20289
	public Color activeColorTooltip = Color.blue;

	// Token: 0x04004F42 RID: 20290
	public Action<BionicSideScreenUpgradeSlot> OnClick;

	// Token: 0x04004F44 RID: 20292
	private bool _isSelected;

	// Token: 0x020020A3 RID: 8355
	public enum State
	{
		// Token: 0x040096C3 RID: 38595
		Locked,
		// Token: 0x040096C4 RID: 38596
		Empty,
		// Token: 0x040096C5 RID: 38597
		Assigned,
		// Token: 0x040096C6 RID: 38598
		Installed
	}
}
