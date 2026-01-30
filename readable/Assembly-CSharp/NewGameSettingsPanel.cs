using System;
using System.Collections.Generic;
using Klei.CustomSettings;
using UnityEngine;

// Token: 0x02000DBB RID: 3515
[AddComponentMenu("KMonoBehaviour/scripts/NewGameSettingsPanel")]
public class NewGameSettingsPanel : CustomGameSettingsPanelBase
{
	// Token: 0x06006DC9 RID: 28105 RVA: 0x002999E5 File Offset: 0x00297BE5
	public void SetCloseAction(System.Action onClose)
	{
		if (this.closeButton != null)
		{
			this.closeButton.onClick += onClose;
		}
		if (this.background != null)
		{
			this.background.onClick += onClose;
		}
	}

	// Token: 0x06006DCA RID: 28106 RVA: 0x00299A1C File Offset: 0x00297C1C
	public override void Init()
	{
		CustomGameSettings.Instance.LoadClusters();
		Global.Instance.modManager.Report(base.gameObject);
		this.settings = CustomGameSettings.Instance;
		this.widgets = new List<CustomGameSettingWidget>();
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in this.settings.QualitySettings)
		{
			if (keyValuePair.Value.ShowInUI())
			{
				ListSettingConfig listSettingConfig = keyValuePair.Value as ListSettingConfig;
				if (listSettingConfig != null)
				{
					CustomGameSettingListWidget customGameSettingListWidget = Util.KInstantiateUI<CustomGameSettingListWidget>(this.prefab_cycle_setting, this.content.gameObject, false);
					customGameSettingListWidget.Initialize(listSettingConfig, new Func<SettingConfig, SettingLevel>(CustomGameSettings.Instance.GetCurrentQualitySetting), new Func<ListSettingConfig, int, SettingLevel>(CustomGameSettings.Instance.CycleQualitySettingLevel));
					customGameSettingListWidget.gameObject.SetActive(true);
					base.AddWidget(customGameSettingListWidget);
				}
				else
				{
					ToggleSettingConfig toggleSettingConfig = keyValuePair.Value as ToggleSettingConfig;
					if (toggleSettingConfig != null)
					{
						CustomGameSettingToggleWidget customGameSettingToggleWidget = Util.KInstantiateUI<CustomGameSettingToggleWidget>(this.prefab_checkbox_setting, this.content.gameObject, false);
						customGameSettingToggleWidget.Initialize(toggleSettingConfig, new Func<SettingConfig, SettingLevel>(CustomGameSettings.Instance.GetCurrentQualitySetting), new Func<ToggleSettingConfig, SettingLevel>(CustomGameSettings.Instance.ToggleQualitySettingLevel));
						customGameSettingToggleWidget.gameObject.SetActive(true);
						base.AddWidget(customGameSettingToggleWidget);
					}
					else
					{
						SeedSettingConfig seedSettingConfig = keyValuePair.Value as SeedSettingConfig;
						if (seedSettingConfig != null)
						{
							CustomGameSettingSeed customGameSettingSeed = Util.KInstantiateUI<CustomGameSettingSeed>(this.prefab_seed_input_setting, this.content.gameObject, false);
							customGameSettingSeed.Initialize(seedSettingConfig);
							customGameSettingSeed.gameObject.SetActive(true);
							base.AddWidget(customGameSettingSeed);
						}
					}
				}
			}
		}
		this.Refresh();
	}

	// Token: 0x06006DCB RID: 28107 RVA: 0x00299BE8 File Offset: 0x00297DE8
	public void ConsumeSettingsCode(string code)
	{
		this.settings.ParseAndApplySettingsCode(code);
	}

	// Token: 0x06006DCC RID: 28108 RVA: 0x00299BF6 File Offset: 0x00297DF6
	public void ConsumeStoryTraitsCode(string code)
	{
		this.settings.ParseAndApplyStoryTraitSettingsCode(code);
	}

	// Token: 0x06006DCD RID: 28109 RVA: 0x00299C04 File Offset: 0x00297E04
	public void ConsumeMixingSettingsCode(string code)
	{
		this.settings.ParseAndApplyMixingSettingsCode(code);
	}

	// Token: 0x06006DCE RID: 28110 RVA: 0x00299C12 File Offset: 0x00297E12
	public void SetSetting(SettingConfig setting, string level, bool notify = true)
	{
		this.settings.SetQualitySetting(setting, level, notify);
	}

	// Token: 0x06006DCF RID: 28111 RVA: 0x00299C22 File Offset: 0x00297E22
	public string GetSetting(SettingConfig setting)
	{
		return this.settings.GetCurrentQualitySetting(setting).id;
	}

	// Token: 0x06006DD0 RID: 28112 RVA: 0x00299C35 File Offset: 0x00297E35
	public string GetSetting(string setting)
	{
		return this.settings.GetCurrentQualitySetting(setting).id;
	}

	// Token: 0x06006DD1 RID: 28113 RVA: 0x00299C48 File Offset: 0x00297E48
	public void Cancel()
	{
	}

	// Token: 0x04004AF7 RID: 19191
	[SerializeField]
	private Transform content;

	// Token: 0x04004AF8 RID: 19192
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04004AF9 RID: 19193
	[SerializeField]
	private KButton background;

	// Token: 0x04004AFA RID: 19194
	[Header("Prefab UI Refs")]
	[SerializeField]
	private GameObject prefab_cycle_setting;

	// Token: 0x04004AFB RID: 19195
	[SerializeField]
	private GameObject prefab_slider_setting;

	// Token: 0x04004AFC RID: 19196
	[SerializeField]
	private GameObject prefab_checkbox_setting;

	// Token: 0x04004AFD RID: 19197
	[SerializeField]
	private GameObject prefab_seed_input_setting;

	// Token: 0x04004AFE RID: 19198
	private CustomGameSettings settings;
}
