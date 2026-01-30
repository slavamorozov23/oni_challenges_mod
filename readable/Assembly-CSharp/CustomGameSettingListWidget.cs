using System;
using Klei.CustomSettings;
using UnityEngine;

// Token: 0x02000CE7 RID: 3303
public class CustomGameSettingListWidget : CustomGameSettingWidget
{
	// Token: 0x060065F9 RID: 26105 RVA: 0x002666EF File Offset: 0x002648EF
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.CycleLeft.onClick += this.DoCycleLeft;
		this.CycleRight.onClick += this.DoCycleRight;
	}

	// Token: 0x060065FA RID: 26106 RVA: 0x00266725 File Offset: 0x00264925
	public void Initialize(ListSettingConfig config, Func<SettingConfig, SettingLevel> getCallback, Func<ListSettingConfig, int, SettingLevel> cycleCallback)
	{
		this.config = config;
		this.Label.text = config.label;
		this.ToolTip.toolTip = config.tooltip;
		this.getCallback = getCallback;
		this.cycleCallback = cycleCallback;
	}

	// Token: 0x060065FB RID: 26107 RVA: 0x00266760 File Offset: 0x00264960
	public override void Refresh()
	{
		base.Refresh();
		SettingLevel settingLevel = this.getCallback(this.config);
		this.ValueLabel.text = settingLevel.label;
		this.ValueToolTip.toolTip = settingLevel.tooltip;
		this.CycleLeft.isInteractable = !this.config.IsFirstLevel(settingLevel.id);
		this.CycleRight.isInteractable = !this.config.IsLastLevel(settingLevel.id);
	}

	// Token: 0x060065FC RID: 26108 RVA: 0x002667E5 File Offset: 0x002649E5
	private void DoCycleLeft()
	{
		this.cycleCallback(this.config, -1);
		base.Notify();
	}

	// Token: 0x060065FD RID: 26109 RVA: 0x00266800 File Offset: 0x00264A00
	private void DoCycleRight()
	{
		this.cycleCallback(this.config, 1);
		base.Notify();
	}

	// Token: 0x04004588 RID: 17800
	[SerializeField]
	private LocText Label;

	// Token: 0x04004589 RID: 17801
	[SerializeField]
	private ToolTip ToolTip;

	// Token: 0x0400458A RID: 17802
	[SerializeField]
	private LocText ValueLabel;

	// Token: 0x0400458B RID: 17803
	[SerializeField]
	private ToolTip ValueToolTip;

	// Token: 0x0400458C RID: 17804
	[SerializeField]
	private KButton CycleLeft;

	// Token: 0x0400458D RID: 17805
	[SerializeField]
	private KButton CycleRight;

	// Token: 0x0400458E RID: 17806
	private ListSettingConfig config;

	// Token: 0x0400458F RID: 17807
	protected Func<ListSettingConfig, int, SettingLevel> cycleCallback;

	// Token: 0x04004590 RID: 17808
	protected Func<SettingConfig, SettingLevel> getCallback;
}
