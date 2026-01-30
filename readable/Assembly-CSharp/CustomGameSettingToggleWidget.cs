using System;
using Klei.CustomSettings;
using UnityEngine;

// Token: 0x02000CE9 RID: 3305
public class CustomGameSettingToggleWidget : CustomGameSettingWidget
{
	// Token: 0x06006608 RID: 26120 RVA: 0x00266AAB File Offset: 0x00264CAB
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MultiToggle toggle = this.Toggle;
		toggle.onClick = (System.Action)Delegate.Combine(toggle.onClick, new System.Action(this.ToggleSetting));
	}

	// Token: 0x06006609 RID: 26121 RVA: 0x00266ADA File Offset: 0x00264CDA
	public void Initialize(ToggleSettingConfig config, Func<SettingConfig, SettingLevel> getCurrentSettingCallback, Func<ToggleSettingConfig, SettingLevel> toggleCallback)
	{
		this.config = config;
		this.Label.text = config.label;
		this.ToolTip.toolTip = config.tooltip;
		this.getCurrentSettingCallback = getCurrentSettingCallback;
		this.toggleCallback = toggleCallback;
	}

	// Token: 0x0600660A RID: 26122 RVA: 0x00266B14 File Offset: 0x00264D14
	public override void Refresh()
	{
		base.Refresh();
		SettingLevel settingLevel = this.getCurrentSettingCallback(this.config);
		this.Toggle.ChangeState(this.config.IsOnLevel(settingLevel.id) ? 1 : 0);
		this.ToggleToolTip.toolTip = settingLevel.tooltip;
	}

	// Token: 0x0600660B RID: 26123 RVA: 0x00266B6C File Offset: 0x00264D6C
	public void ToggleSetting()
	{
		this.toggleCallback(this.config);
		base.Notify();
	}

	// Token: 0x0400459A RID: 17818
	[SerializeField]
	private LocText Label;

	// Token: 0x0400459B RID: 17819
	[SerializeField]
	private ToolTip ToolTip;

	// Token: 0x0400459C RID: 17820
	[SerializeField]
	private MultiToggle Toggle;

	// Token: 0x0400459D RID: 17821
	[SerializeField]
	private ToolTip ToggleToolTip;

	// Token: 0x0400459E RID: 17822
	private ToggleSettingConfig config;

	// Token: 0x0400459F RID: 17823
	protected Func<SettingConfig, SettingLevel> getCurrentSettingCallback;

	// Token: 0x040045A0 RID: 17824
	protected Func<ToggleSettingConfig, SettingLevel> toggleCallback;
}
