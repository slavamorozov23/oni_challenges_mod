using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using Database;
using Klei.CustomSettings;
using KSerialization;
using ProcGen;
using UnityEngine;

// Token: 0x020008C7 RID: 2247
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/CustomGameSettings")]
public class CustomGameSettings : KMonoBehaviour
{
	// Token: 0x1700044A RID: 1098
	// (get) Token: 0x06003E09 RID: 15881 RVA: 0x00159F95 File Offset: 0x00158195
	public static CustomGameSettings Instance
	{
		get
		{
			return CustomGameSettings.instance;
		}
	}

	// Token: 0x1700044B RID: 1099
	// (get) Token: 0x06003E0A RID: 15882 RVA: 0x00159F9C File Offset: 0x0015819C
	public IReadOnlyDictionary<string, string> CurrentStoryLevelsBySetting
	{
		get
		{
			return this.currentStoryLevelsBySetting;
		}
	}

	// Token: 0x14000016 RID: 22
	// (add) Token: 0x06003E0B RID: 15883 RVA: 0x00159FA4 File Offset: 0x001581A4
	// (remove) Token: 0x06003E0C RID: 15884 RVA: 0x00159FDC File Offset: 0x001581DC
	public event Action<SettingConfig, SettingLevel> OnQualitySettingChanged;

	// Token: 0x14000017 RID: 23
	// (add) Token: 0x06003E0D RID: 15885 RVA: 0x0015A014 File Offset: 0x00158214
	// (remove) Token: 0x06003E0E RID: 15886 RVA: 0x0015A04C File Offset: 0x0015824C
	public event Action<SettingConfig, SettingLevel> OnStorySettingChanged;

	// Token: 0x14000018 RID: 24
	// (add) Token: 0x06003E0F RID: 15887 RVA: 0x0015A084 File Offset: 0x00158284
	// (remove) Token: 0x06003E10 RID: 15888 RVA: 0x0015A0BC File Offset: 0x001582BC
	public event Action<SettingConfig, SettingLevel> OnMixingSettingChanged;

	// Token: 0x06003E11 RID: 15889 RVA: 0x0015A0F4 File Offset: 0x001582F4
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 6))
		{
			this.customGameMode = (this.is_custom_game ? CustomGameSettings.CustomGameMode.Custom : CustomGameSettings.CustomGameMode.Survival);
		}
		if (this.CurrentQualityLevelsBySetting.ContainsKey("CarePackages "))
		{
			if (!this.CurrentQualityLevelsBySetting.ContainsKey(CustomGameSettingConfigs.CarePackages.id))
			{
				this.CurrentQualityLevelsBySetting.Add(CustomGameSettingConfigs.CarePackages.id, this.CurrentQualityLevelsBySetting["CarePackages "]);
			}
			this.CurrentQualityLevelsBySetting.Remove("CarePackages ");
		}
		this.CurrentQualityLevelsBySetting.Remove("Expansion1Active");
		string clusterDefaultName;
		this.CurrentQualityLevelsBySetting.TryGetValue(CustomGameSettingConfigs.ClusterLayout.id, out clusterDefaultName);
		if (clusterDefaultName.IsNullOrWhiteSpace())
		{
			if (!DlcManager.IsExpansion1Active())
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					"Deserializing CustomGameSettings.ClusterLayout: ClusterLayout is blank, using default cluster instead"
				});
			}
			clusterDefaultName = WorldGenSettings.ClusterDefaultName;
			this.SetQualitySetting(CustomGameSettingConfigs.ClusterLayout, clusterDefaultName);
		}
		if (!SettingsCache.clusterLayouts.clusterCache.ContainsKey(clusterDefaultName))
		{
			global::Debug.Log("Deserializing CustomGameSettings.ClusterLayout: '" + clusterDefaultName + "' doesn't exist in the clusterCache, trying to rewrite path to scoped path.");
			string text = SettingsCache.GetScope("EXPANSION1_ID") + clusterDefaultName;
			if (SettingsCache.clusterLayouts.clusterCache.ContainsKey(text))
			{
				global::Debug.Log(string.Concat(new string[]
				{
					"Deserializing CustomGameSettings.ClusterLayout: Success in rewriting ClusterLayout '",
					clusterDefaultName,
					"' to '",
					text,
					"'"
				}));
				this.SetQualitySetting(CustomGameSettingConfigs.ClusterLayout, text);
			}
			else
			{
				global::Debug.LogWarning("Deserializing CustomGameSettings.ClusterLayout: Failed to find cluster '" + clusterDefaultName + "' including the scoped path, setting to default cluster name.");
				global::Debug.Log("ClusterCache: " + string.Join(",", SettingsCache.clusterLayouts.clusterCache.Keys));
				this.SetQualitySetting(CustomGameSettingConfigs.ClusterLayout, WorldGenSettings.ClusterDefaultName);
			}
		}
		this.CheckCustomGameMode();
	}

	// Token: 0x06003E12 RID: 15890 RVA: 0x0015A2C8 File Offset: 0x001584C8
	private void AddMissingQualitySettings()
	{
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in this.QualitySettings)
		{
			SettingConfig value = keyValuePair.Value;
			if (Game.IsCorrectDlcActiveForCurrentSave(value) && !this.CurrentQualityLevelsBySetting.ContainsKey(value.id))
			{
				if (value.missing_content_default != "")
				{
					DebugUtil.LogArgs(new object[]
					{
						string.Concat(new string[]
						{
							"QualitySetting '",
							value.id,
							"' is missing, setting it to missing_content_default '",
							value.missing_content_default,
							"'."
						})
					});
					this.SetQualitySetting(value, value.missing_content_default);
				}
				else
				{
					DebugUtil.DevLogError("QualitySetting '" + value.id + "' is missing in this save. Either provide a missing_content_default or handle it in OnDeserialized.");
				}
			}
		}
	}

	// Token: 0x06003E13 RID: 15891 RVA: 0x0015A3C0 File Offset: 0x001585C0
	protected override void OnPrefabInit()
	{
		DlcManager.IsExpansion1Active();
		Action<SettingConfig> action = delegate(SettingConfig setting)
		{
			this.AddQualitySettingConfig(setting);
			if (setting.coordinate_range >= 0L)
			{
				this.CoordinatedQualitySettings.Add(setting.id);
			}
		};
		Action<SettingConfig> action2 = delegate(SettingConfig setting)
		{
			this.AddStorySettingConfig(setting);
			if (setting.coordinate_range >= 0L)
			{
				this.CoordinatedStorySettings.Add(setting.id);
			}
		};
		Action<SettingConfig> action3 = delegate(SettingConfig setting)
		{
			this.AddMixingSettingsConfig(setting);
			if (setting.coordinate_range >= 0L)
			{
				this.CoordinatedMixingSettings.Add(setting.id);
			}
		};
		CustomGameSettings.instance = this;
		action(CustomGameSettingConfigs.ClusterLayout);
		action(CustomGameSettingConfigs.WorldgenSeed);
		action(CustomGameSettingConfigs.ImmuneSystem);
		action(CustomGameSettingConfigs.CalorieBurn);
		action(CustomGameSettingConfigs.Morale);
		action(CustomGameSettingConfigs.Durability);
		action(CustomGameSettingConfigs.MeteorShowers);
		action(CustomGameSettingConfigs.Radiation);
		action(CustomGameSettingConfigs.Stress);
		action(CustomGameSettingConfigs.StressBreaks);
		action(CustomGameSettingConfigs.CarePackages);
		action(CustomGameSettingConfigs.SandboxMode);
		action(CustomGameSettingConfigs.FastWorkersMode);
		action(CustomGameSettingConfigs.SaveToCloud);
		action(CustomGameSettingConfigs.Teleporters);
		action(CustomGameSettingConfigs.BionicWattage);
		action(CustomGameSettingConfigs.DemoliorDifficulty);
		action3(CustomMixingSettingsConfigs.DLC2Mixing);
		action3(CustomMixingSettingsConfigs.IceCavesMixing);
		action3(CustomMixingSettingsConfigs.CarrotQuarryMixing);
		action3(CustomMixingSettingsConfigs.SugarWoodsMixing);
		action3(CustomMixingSettingsConfigs.CeresAsteroidMixing);
		action3(CustomMixingSettingsConfigs.DLC3Mixing);
		action3(CustomMixingSettingsConfigs.DLC4Mixing);
		action3(CustomMixingSettingsConfigs.GardenMixing);
		action3(CustomMixingSettingsConfigs.RaptorMixing);
		action3(CustomMixingSettingsConfigs.WetlandsMixing);
		action3(CustomMixingSettingsConfigs.PrehistoricAsteroidMixing);
		foreach (Story story in Db.Get().Stories.GetStoriesSortedByCoordinateOrder())
		{
			int num = (story.kleiUseOnlyCoordinateOrder == -1) ? -1 : 3;
			SettingConfig obj = new ListSettingConfig(story.Id, "", "", new List<SettingLevel>
			{
				new SettingLevel("Disabled", "", "", 0L, null),
				new SettingLevel("Guaranteed", "", "", 1L, null)
			}, "Disabled", "Disabled", (long)num, false, false, null, "", false);
			action2(obj);
		}
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in this.MixingSettings)
		{
			DlcMixingSettingConfig dlcMixingSettingConfig = keyValuePair.Value as DlcMixingSettingConfig;
			if (dlcMixingSettingConfig != null && DlcManager.IsContentSubscribed(dlcMixingSettingConfig.id))
			{
				this.SetMixingSetting(dlcMixingSettingConfig, "Enabled");
			}
		}
		this.VerifySettingCoordinates();
	}

	// Token: 0x06003E14 RID: 15892 RVA: 0x0015A670 File Offset: 0x00158870
	public void DisableAllStories()
	{
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in this.StorySettings)
		{
			this.SetStorySetting(keyValuePair.Value, false);
		}
	}

	// Token: 0x06003E15 RID: 15893 RVA: 0x0015A6CC File Offset: 0x001588CC
	public void SetSurvivalDefaults()
	{
		this.customGameMode = CustomGameSettings.CustomGameMode.Survival;
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in this.QualitySettings)
		{
			this.SetQualitySetting(keyValuePair.Value, keyValuePair.Value.GetDefaultLevelId());
		}
	}

	// Token: 0x06003E16 RID: 15894 RVA: 0x0015A738 File Offset: 0x00158938
	public void SetNosweatDefaults()
	{
		this.customGameMode = CustomGameSettings.CustomGameMode.Nosweat;
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in this.QualitySettings)
		{
			this.SetQualitySetting(keyValuePair.Value, keyValuePair.Value.GetNoSweatDefaultLevelId());
		}
	}

	// Token: 0x06003E17 RID: 15895 RVA: 0x0015A7A4 File Offset: 0x001589A4
	public SettingLevel CycleQualitySettingLevel(ListSettingConfig config, int direction)
	{
		this.SetQualitySetting(config, config.CycleSettingLevelID(this.CurrentQualityLevelsBySetting[config.id], direction));
		return config.GetLevel(this.CurrentQualityLevelsBySetting[config.id]);
	}

	// Token: 0x06003E18 RID: 15896 RVA: 0x0015A7DC File Offset: 0x001589DC
	public SettingLevel ToggleQualitySettingLevel(ToggleSettingConfig config)
	{
		this.SetQualitySetting(config, config.ToggleSettingLevelID(this.CurrentQualityLevelsBySetting[config.id]));
		return config.GetLevel(this.CurrentQualityLevelsBySetting[config.id]);
	}

	// Token: 0x06003E19 RID: 15897 RVA: 0x0015A814 File Offset: 0x00158A14
	private void CheckCustomGameMode()
	{
		bool flag = true;
		bool flag2 = true;
		foreach (KeyValuePair<string, string> keyValuePair in this.CurrentQualityLevelsBySetting)
		{
			if (!this.QualitySettings.ContainsKey(keyValuePair.Key))
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					"Quality settings missing " + keyValuePair.Key
				});
			}
			else if (this.QualitySettings[keyValuePair.Key].triggers_custom_game)
			{
				if (keyValuePair.Value != this.QualitySettings[keyValuePair.Key].GetDefaultLevelId())
				{
					flag = false;
				}
				if (keyValuePair.Value != this.QualitySettings[keyValuePair.Key].GetNoSweatDefaultLevelId())
				{
					flag2 = false;
				}
				if (!flag && !flag2)
				{
					break;
				}
			}
		}
		CustomGameSettings.CustomGameMode customGameMode;
		if (flag)
		{
			customGameMode = CustomGameSettings.CustomGameMode.Survival;
		}
		else if (flag2)
		{
			customGameMode = CustomGameSettings.CustomGameMode.Nosweat;
		}
		else
		{
			customGameMode = CustomGameSettings.CustomGameMode.Custom;
		}
		if (customGameMode != this.customGameMode)
		{
			DebugUtil.LogArgs(new object[]
			{
				"Game mode changed from",
				this.customGameMode,
				"to",
				customGameMode
			});
			this.customGameMode = customGameMode;
		}
	}

	// Token: 0x06003E1A RID: 15898 RVA: 0x0015A968 File Offset: 0x00158B68
	public void SetQualitySetting(SettingConfig config, string value)
	{
		this.SetQualitySetting(config, value, true);
	}

	// Token: 0x06003E1B RID: 15899 RVA: 0x0015A973 File Offset: 0x00158B73
	public void SetQualitySetting(SettingConfig config, string value, bool notify)
	{
		this.CurrentQualityLevelsBySetting[config.id] = value;
		this.CheckCustomGameMode();
		if (notify && this.OnQualitySettingChanged != null)
		{
			this.OnQualitySettingChanged(config, this.GetCurrentQualitySetting(config));
		}
	}

	// Token: 0x06003E1C RID: 15900 RVA: 0x0015A9AB File Offset: 0x00158BAB
	public SettingLevel GetCurrentQualitySetting(SettingConfig setting)
	{
		return this.GetCurrentQualitySetting(setting.id);
	}

	// Token: 0x06003E1D RID: 15901 RVA: 0x0015A9BC File Offset: 0x00158BBC
	public SettingLevel GetCurrentQualitySetting(string setting_id)
	{
		SettingConfig settingConfig = this.QualitySettings[setting_id];
		if (this.customGameMode == CustomGameSettings.CustomGameMode.Survival && settingConfig.triggers_custom_game)
		{
			return settingConfig.GetLevel(settingConfig.GetDefaultLevelId());
		}
		if (this.customGameMode == CustomGameSettings.CustomGameMode.Nosweat && settingConfig.triggers_custom_game)
		{
			return settingConfig.GetLevel(settingConfig.GetNoSweatDefaultLevelId());
		}
		if (!this.CurrentQualityLevelsBySetting.ContainsKey(setting_id))
		{
			this.CurrentQualityLevelsBySetting[setting_id] = this.QualitySettings[setting_id].GetDefaultLevelId();
		}
		string level_id = DlcManager.IsAllContentSubscribed(settingConfig.required_content) ? this.CurrentQualityLevelsBySetting[setting_id] : settingConfig.GetDefaultLevelId();
		return this.QualitySettings[setting_id].GetLevel(level_id);
	}

	// Token: 0x06003E1E RID: 15902 RVA: 0x0015AA70 File Offset: 0x00158C70
	public string GetCurrentQualitySettingLevelId(SettingConfig config)
	{
		return this.CurrentQualityLevelsBySetting[config.id];
	}

	// Token: 0x06003E1F RID: 15903 RVA: 0x0015AA84 File Offset: 0x00158C84
	public string GetSettingLevelLabel(string setting_id, string level_id)
	{
		SettingConfig settingConfig = this.QualitySettings[setting_id];
		if (settingConfig != null)
		{
			SettingLevel level = settingConfig.GetLevel(level_id);
			if (level != null)
			{
				return level.label;
			}
		}
		global::Debug.LogWarning("No label string for setting: " + setting_id + " level: " + level_id);
		return "";
	}

	// Token: 0x06003E20 RID: 15904 RVA: 0x0015AAD0 File Offset: 0x00158CD0
	public string GetQualitySettingLevelTooltip(string setting_id, string level_id)
	{
		SettingConfig settingConfig = this.QualitySettings[setting_id];
		if (settingConfig != null)
		{
			SettingLevel level = settingConfig.GetLevel(level_id);
			if (level != null)
			{
				return level.tooltip;
			}
		}
		global::Debug.LogWarning("No tooltip string for setting: " + setting_id + " level: " + level_id);
		return "";
	}

	// Token: 0x06003E21 RID: 15905 RVA: 0x0015AB1C File Offset: 0x00158D1C
	public void AddQualitySettingConfig(SettingConfig config)
	{
		this.QualitySettings.Add(config.id, config);
		if (!this.CurrentQualityLevelsBySetting.ContainsKey(config.id) || string.IsNullOrEmpty(this.CurrentQualityLevelsBySetting[config.id]))
		{
			this.CurrentQualityLevelsBySetting[config.id] = config.GetDefaultLevelId();
		}
	}

	// Token: 0x06003E22 RID: 15906 RVA: 0x0015AB80 File Offset: 0x00158D80
	public void AddStorySettingConfig(SettingConfig config)
	{
		this.StorySettings.Add(config.id, config);
		if (!this.currentStoryLevelsBySetting.ContainsKey(config.id) || string.IsNullOrEmpty(this.currentStoryLevelsBySetting[config.id]))
		{
			this.currentStoryLevelsBySetting[config.id] = config.GetDefaultLevelId();
		}
	}

	// Token: 0x06003E23 RID: 15907 RVA: 0x0015ABE1 File Offset: 0x00158DE1
	public void SetStorySetting(SettingConfig config, string value)
	{
		this.SetStorySetting(config, value == "Guaranteed");
	}

	// Token: 0x06003E24 RID: 15908 RVA: 0x0015ABF5 File Offset: 0x00158DF5
	public void SetStorySetting(SettingConfig config, bool value)
	{
		this.currentStoryLevelsBySetting[config.id] = (value ? "Guaranteed" : "Disabled");
		if (this.OnStorySettingChanged != null)
		{
			this.OnStorySettingChanged(config, this.GetCurrentStoryTraitSetting(config));
		}
	}

	// Token: 0x06003E25 RID: 15909 RVA: 0x0015AC34 File Offset: 0x00158E34
	public void ParseAndApplyStoryTraitSettingsCode(string code)
	{
		BigInteger dividend = this.Base36toBinary(code);
		Dictionary<SettingConfig, string> dictionary = new Dictionary<SettingConfig, string>();
		foreach (object obj in global::Util.Reverse(this.CoordinatedStorySettings))
		{
			string key = (string)obj;
			SettingConfig settingConfig = this.StorySettings[key];
			if (settingConfig.coordinate_range != -1L)
			{
				long num = (long)(dividend % settingConfig.coordinate_range);
				dividend /= settingConfig.coordinate_range;
				foreach (SettingLevel settingLevel in settingConfig.GetLevels())
				{
					if (settingLevel.coordinate_value == num)
					{
						dictionary[settingConfig] = settingLevel.id;
						break;
					}
				}
			}
		}
		foreach (KeyValuePair<SettingConfig, string> keyValuePair in dictionary)
		{
			this.SetStorySetting(keyValuePair.Key, keyValuePair.Value);
		}
	}

	// Token: 0x06003E26 RID: 15910 RVA: 0x0015AD90 File Offset: 0x00158F90
	private string GetStoryTraitSettingsCode()
	{
		BigInteger bigInteger = 0;
		foreach (string key in this.CoordinatedStorySettings)
		{
			SettingConfig settingConfig = this.StorySettings[key];
			bigInteger *= settingConfig.coordinate_range;
			bigInteger += settingConfig.GetLevel(this.currentStoryLevelsBySetting[key]).coordinate_value;
		}
		return this.BinarytoBase36(bigInteger);
	}

	// Token: 0x06003E27 RID: 15911 RVA: 0x0015AE2C File Offset: 0x0015902C
	public SettingLevel GetCurrentStoryTraitSetting(SettingConfig setting)
	{
		return this.GetCurrentStoryTraitSetting(setting.id);
	}

	// Token: 0x06003E28 RID: 15912 RVA: 0x0015AE3C File Offset: 0x0015903C
	public SettingLevel GetCurrentStoryTraitSetting(string settingId)
	{
		SettingConfig settingConfig = this.StorySettings[settingId];
		if (this.customGameMode == CustomGameSettings.CustomGameMode.Survival && settingConfig.triggers_custom_game)
		{
			return settingConfig.GetLevel(settingConfig.GetDefaultLevelId());
		}
		if (this.customGameMode == CustomGameSettings.CustomGameMode.Nosweat && settingConfig.triggers_custom_game)
		{
			return settingConfig.GetLevel(settingConfig.GetNoSweatDefaultLevelId());
		}
		if (!this.currentStoryLevelsBySetting.ContainsKey(settingId))
		{
			this.currentStoryLevelsBySetting[settingId] = this.StorySettings[settingId].GetDefaultLevelId();
		}
		string level_id = DlcManager.IsAllContentSubscribed(settingConfig.required_content) ? this.currentStoryLevelsBySetting[settingId] : settingConfig.GetDefaultLevelId();
		return this.StorySettings[settingId].GetLevel(level_id);
	}

	// Token: 0x06003E29 RID: 15913 RVA: 0x0015AEF0 File Offset: 0x001590F0
	public List<string> GetCurrentStories()
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, string> keyValuePair in this.currentStoryLevelsBySetting)
		{
			if (this.IsStoryActive(keyValuePair.Key, keyValuePair.Value))
			{
				list.Add(keyValuePair.Key);
			}
		}
		return list;
	}

	// Token: 0x06003E2A RID: 15914 RVA: 0x0015AF68 File Offset: 0x00159168
	public bool IsStoryActive(string id, string level)
	{
		SettingConfig settingConfig;
		return this.StorySettings.TryGetValue(id, out settingConfig) && settingConfig != null && level == "Guaranteed";
	}

	// Token: 0x06003E2B RID: 15915 RVA: 0x0015AF97 File Offset: 0x00159197
	public void SetMixingSetting(SettingConfig config, string value)
	{
		this.SetMixingSetting(config, value, true);
	}

	// Token: 0x06003E2C RID: 15916 RVA: 0x0015AFA2 File Offset: 0x001591A2
	public void SetMixingSetting(SettingConfig config, string value, bool notify)
	{
		this.CurrentMixingLevelsBySetting[config.id] = value;
		if (notify && this.OnMixingSettingChanged != null)
		{
			this.OnMixingSettingChanged(config, this.GetCurrentMixingSettingLevel(config));
		}
	}

	// Token: 0x06003E2D RID: 15917 RVA: 0x0015AFD4 File Offset: 0x001591D4
	public void AddMixingSettingsConfig(SettingConfig config)
	{
		this.MixingSettings.Add(config.id, config);
		if (!this.CurrentMixingLevelsBySetting.ContainsKey(config.id) || string.IsNullOrEmpty(this.CurrentMixingLevelsBySetting[config.id]))
		{
			this.CurrentMixingLevelsBySetting[config.id] = config.GetDefaultLevelId();
		}
	}

	// Token: 0x06003E2E RID: 15918 RVA: 0x0015B035 File Offset: 0x00159235
	public SettingLevel GetCurrentMixingSettingLevel(SettingConfig setting)
	{
		return this.GetCurrentMixingSettingLevel(setting.id);
	}

	// Token: 0x06003E2F RID: 15919 RVA: 0x0015B044 File Offset: 0x00159244
	public SettingConfig GetWorldMixingSettingForWorldgenFile(string file)
	{
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in this.MixingSettings)
		{
			WorldMixingSettingConfig worldMixingSettingConfig = keyValuePair.Value as WorldMixingSettingConfig;
			if (worldMixingSettingConfig != null && worldMixingSettingConfig.worldgenPath == file)
			{
				return keyValuePair.Value;
			}
		}
		return null;
	}

	// Token: 0x06003E30 RID: 15920 RVA: 0x0015B0BC File Offset: 0x001592BC
	public SettingConfig GetSubworldMixingSettingForWorldgenFile(string file)
	{
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in this.MixingSettings)
		{
			SubworldMixingSettingConfig subworldMixingSettingConfig = keyValuePair.Value as SubworldMixingSettingConfig;
			if (subworldMixingSettingConfig != null && subworldMixingSettingConfig.worldgenPath == file)
			{
				return keyValuePair.Value;
			}
		}
		return null;
	}

	// Token: 0x06003E31 RID: 15921 RVA: 0x0015B134 File Offset: 0x00159334
	public void DisableAllMixing()
	{
		foreach (SettingConfig settingConfig in this.MixingSettings.Values)
		{
			this.SetMixingSetting(settingConfig, settingConfig.GetDefaultLevelId());
		}
	}

	// Token: 0x06003E32 RID: 15922 RVA: 0x0015B194 File Offset: 0x00159394
	public List<SubworldMixingSettingConfig> GetActiveSubworldMixingSettings()
	{
		List<SubworldMixingSettingConfig> list = new List<SubworldMixingSettingConfig>();
		foreach (SettingConfig settingConfig in this.MixingSettings.Values)
		{
			SubworldMixingSettingConfig subworldMixingSettingConfig = settingConfig as SubworldMixingSettingConfig;
			if (subworldMixingSettingConfig != null && this.GetCurrentMixingSettingLevel(settingConfig).id != "Disabled")
			{
				list.Add(subworldMixingSettingConfig);
			}
		}
		return list;
	}

	// Token: 0x06003E33 RID: 15923 RVA: 0x0015B218 File Offset: 0x00159418
	public List<WorldMixingSettingConfig> GetActiveWorldMixingSettings()
	{
		List<WorldMixingSettingConfig> list = new List<WorldMixingSettingConfig>();
		foreach (SettingConfig settingConfig in this.MixingSettings.Values)
		{
			WorldMixingSettingConfig worldMixingSettingConfig = settingConfig as WorldMixingSettingConfig;
			if (worldMixingSettingConfig != null && this.GetCurrentMixingSettingLevel(settingConfig).id != "Disabled")
			{
				list.Add(worldMixingSettingConfig);
			}
		}
		return list;
	}

	// Token: 0x06003E34 RID: 15924 RVA: 0x0015B29C File Offset: 0x0015949C
	public SettingLevel CycleMixingSettingLevel(ListSettingConfig config, int direction)
	{
		this.SetMixingSetting(config, config.CycleSettingLevelID(this.CurrentMixingLevelsBySetting[config.id], direction));
		return config.GetLevel(this.CurrentMixingLevelsBySetting[config.id]);
	}

	// Token: 0x06003E35 RID: 15925 RVA: 0x0015B2D4 File Offset: 0x001594D4
	public SettingLevel ToggleMixingSettingLevel(ToggleSettingConfig config)
	{
		this.SetMixingSetting(config, config.ToggleSettingLevelID(this.CurrentMixingLevelsBySetting[config.id]));
		return config.GetLevel(this.CurrentMixingLevelsBySetting[config.id]);
	}

	// Token: 0x06003E36 RID: 15926 RVA: 0x0015B30C File Offset: 0x0015950C
	public SettingLevel GetCurrentMixingSettingLevel(string settingId)
	{
		SettingConfig settingConfig = this.MixingSettings[settingId];
		if (!this.CurrentMixingLevelsBySetting.ContainsKey(settingId))
		{
			this.CurrentMixingLevelsBySetting[settingId] = this.MixingSettings[settingId].GetDefaultLevelId();
		}
		string level_id = DlcManager.IsAllContentSubscribed(settingConfig.required_content) ? this.CurrentMixingLevelsBySetting[settingId] : settingConfig.GetDefaultLevelId();
		return this.MixingSettings[settingId].GetLevel(level_id);
	}

	// Token: 0x06003E37 RID: 15927 RVA: 0x0015B388 File Offset: 0x00159588
	public List<string> GetCurrentDlcMixingIds()
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in this.MixingSettings)
		{
			DlcMixingSettingConfig dlcMixingSettingConfig = keyValuePair.Value as DlcMixingSettingConfig;
			if (dlcMixingSettingConfig != null && dlcMixingSettingConfig.IsOnLevel(this.GetCurrentMixingSettingLevel(dlcMixingSettingConfig.id).id))
			{
				list.Add(dlcMixingSettingConfig.id);
			}
		}
		return list;
	}

	// Token: 0x06003E38 RID: 15928 RVA: 0x0015B410 File Offset: 0x00159610
	public void ParseAndApplyMixingSettingsCode(string code)
	{
		BigInteger dividend = this.Base36toBinary(code);
		Dictionary<SettingConfig, string> dictionary = new Dictionary<SettingConfig, string>();
		foreach (object obj in global::Util.Reverse(this.CoordinatedMixingSettings))
		{
			string key = (string)obj;
			SettingConfig settingConfig = this.MixingSettings[key];
			if (settingConfig.coordinate_range != -1L)
			{
				long num = (long)(dividend % settingConfig.coordinate_range);
				dividend /= settingConfig.coordinate_range;
				foreach (SettingLevel settingLevel in settingConfig.GetLevels())
				{
					if (settingLevel.coordinate_value == num)
					{
						dictionary[settingConfig] = settingLevel.id;
						break;
					}
				}
			}
		}
		foreach (KeyValuePair<SettingConfig, string> keyValuePair in dictionary)
		{
			this.SetMixingSetting(keyValuePair.Key, keyValuePair.Value);
		}
	}

	// Token: 0x06003E39 RID: 15929 RVA: 0x0015B56C File Offset: 0x0015976C
	private string GetMixingSettingsCode()
	{
		BigInteger bigInteger = 0;
		foreach (string key in this.CoordinatedMixingSettings)
		{
			SettingConfig settingConfig = this.MixingSettings[key];
			bigInteger *= settingConfig.coordinate_range;
			bigInteger += settingConfig.GetLevel(this.GetCurrentMixingSettingLevel(settingConfig).id).coordinate_value;
		}
		return this.BinarytoBase36(bigInteger);
	}

	// Token: 0x06003E3A RID: 15930 RVA: 0x0015B608 File Offset: 0x00159808
	public void RemoveInvalidMixingSettings()
	{
		ClusterLayout currentClusterLayout = this.GetCurrentClusterLayout();
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in this.MixingSettings)
		{
			DlcMixingSettingConfig dlcMixingSettingConfig = keyValuePair.Value as DlcMixingSettingConfig;
			if (dlcMixingSettingConfig != null && currentClusterLayout.requiredDlcIds.Contains(dlcMixingSettingConfig.id))
			{
				this.SetMixingSetting(keyValuePair.Value, "Disabled");
			}
		}
		CustomGameSettings.<>c__DisplayClass71_0 CS$<>8__locals1;
		CS$<>8__locals1.availableDlcs = this.GetCurrentDlcMixingIds();
		CS$<>8__locals1.availableDlcs.AddRange(currentClusterLayout.requiredDlcIds);
		foreach (KeyValuePair<string, SettingConfig> keyValuePair2 in this.MixingSettings)
		{
			SettingConfig value = keyValuePair2.Value;
			WorldMixingSettingConfig worldMixingSettingConfig = value as WorldMixingSettingConfig;
			if (worldMixingSettingConfig == null)
			{
				SubworldMixingSettingConfig subworldMixingSettingConfig = value as SubworldMixingSettingConfig;
				if (subworldMixingSettingConfig != null)
				{
					if (!CustomGameSettings.<RemoveInvalidMixingSettings>g__HasRequiredContent|71_0(subworldMixingSettingConfig.required_content, ref CS$<>8__locals1) || currentClusterLayout.HasAnyTags(subworldMixingSettingConfig.forbiddenClusterTags))
					{
						this.SetMixingSetting(keyValuePair2.Value, "Disabled");
					}
				}
			}
			else if (!CustomGameSettings.<RemoveInvalidMixingSettings>g__HasRequiredContent|71_0(worldMixingSettingConfig.required_content, ref CS$<>8__locals1) || currentClusterLayout.HasAnyTags(worldMixingSettingConfig.forbiddenClusterTags))
			{
				this.SetMixingSetting(keyValuePair2.Value, "Disabled");
			}
		}
	}

	// Token: 0x06003E3B RID: 15931 RVA: 0x0015B77C File Offset: 0x0015997C
	public ClusterLayout GetCurrentClusterLayout()
	{
		SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.ClusterLayout);
		if (currentQualitySetting == null)
		{
			return null;
		}
		return SettingsCache.clusterLayouts.GetClusterData(currentQualitySetting.id);
	}

	// Token: 0x06003E3C RID: 15932 RVA: 0x0015B7B0 File Offset: 0x001599B0
	public int GetCurrentWorldgenSeed()
	{
		SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.WorldgenSeed);
		if (currentQualitySetting == null)
		{
			return 0;
		}
		return int.Parse(currentQualitySetting.id);
	}

	// Token: 0x06003E3D RID: 15933 RVA: 0x0015B7E0 File Offset: 0x001599E0
	public void LoadClusters()
	{
		Dictionary<string, ClusterLayout> clusterCache = SettingsCache.clusterLayouts.clusterCache;
		List<SettingLevel> list = new List<SettingLevel>(clusterCache.Count);
		foreach (KeyValuePair<string, ClusterLayout> keyValuePair in clusterCache)
		{
			StringEntry stringEntry;
			string label = Strings.TryGet(new StringKey(keyValuePair.Value.name), out stringEntry) ? stringEntry.ToString() : keyValuePair.Value.name;
			string tooltip = Strings.TryGet(new StringKey(keyValuePair.Value.description), out stringEntry) ? stringEntry.ToString() : keyValuePair.Value.description;
			list.Add(new SettingLevel(keyValuePair.Key, label, tooltip, 0L, null));
		}
		CustomGameSettingConfigs.ClusterLayout.StompLevels(list, WorldGenSettings.ClusterDefaultName, WorldGenSettings.ClusterDefaultName);
	}

	// Token: 0x06003E3E RID: 15934 RVA: 0x0015B8D0 File Offset: 0x00159AD0
	public void Print()
	{
		string text = "Custom Settings: ";
		foreach (KeyValuePair<string, string> keyValuePair in this.CurrentQualityLevelsBySetting)
		{
			text = string.Concat(new string[]
			{
				text,
				keyValuePair.Key,
				"=",
				keyValuePair.Value,
				","
			});
		}
		global::Debug.Log(text);
		text = "Story Settings: ";
		foreach (KeyValuePair<string, string> keyValuePair2 in this.currentStoryLevelsBySetting)
		{
			text = string.Concat(new string[]
			{
				text,
				keyValuePair2.Key,
				"=",
				keyValuePair2.Value,
				","
			});
		}
		global::Debug.Log(text);
		text = "Mixing Settings: ";
		foreach (KeyValuePair<string, string> keyValuePair3 in this.CurrentMixingLevelsBySetting)
		{
			text = string.Concat(new string[]
			{
				text,
				keyValuePair3.Key,
				"=",
				keyValuePair3.Value,
				","
			});
		}
		global::Debug.Log(text);
	}

	// Token: 0x06003E3F RID: 15935 RVA: 0x0015BA54 File Offset: 0x00159C54
	private bool AllValuesMatch(Dictionary<string, string> data, CustomGameSettings.CustomGameMode mode)
	{
		bool result = true;
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in this.QualitySettings)
		{
			if (!(keyValuePair.Key == CustomGameSettingConfigs.WorldgenSeed.id))
			{
				string b = null;
				if (mode != CustomGameSettings.CustomGameMode.Survival)
				{
					if (mode == CustomGameSettings.CustomGameMode.Nosweat)
					{
						b = keyValuePair.Value.GetNoSweatDefaultLevelId();
					}
				}
				else
				{
					b = keyValuePair.Value.GetDefaultLevelId();
				}
				if (data.ContainsKey(keyValuePair.Key) && data[keyValuePair.Key] != b)
				{
					result = false;
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x06003E40 RID: 15936 RVA: 0x0015BB08 File Offset: 0x00159D08
	public List<CustomGameSettings.MetricSettingsData> GetSettingsForMetrics()
	{
		List<CustomGameSettings.MetricSettingsData> list = new List<CustomGameSettings.MetricSettingsData>();
		list.Add(new CustomGameSettings.MetricSettingsData
		{
			Name = "CustomGameMode",
			Value = this.customGameMode.ToString()
		});
		foreach (KeyValuePair<string, string> keyValuePair in this.CurrentQualityLevelsBySetting)
		{
			list.Add(new CustomGameSettings.MetricSettingsData
			{
				Name = keyValuePair.Key,
				Value = keyValuePair.Value
			});
		}
		CustomGameSettings.MetricSettingsData item = new CustomGameSettings.MetricSettingsData
		{
			Name = "CustomGameModeActual",
			Value = CustomGameSettings.CustomGameMode.Custom.ToString()
		};
		foreach (object obj in Enum.GetValues(typeof(CustomGameSettings.CustomGameMode)))
		{
			CustomGameSettings.CustomGameMode customGameMode = (CustomGameSettings.CustomGameMode)obj;
			if (customGameMode != CustomGameSettings.CustomGameMode.Custom && this.AllValuesMatch(this.CurrentQualityLevelsBySetting, customGameMode))
			{
				item.Value = customGameMode.ToString();
				break;
			}
		}
		list.Add(item);
		return list;
	}

	// Token: 0x06003E41 RID: 15937 RVA: 0x0015BC74 File Offset: 0x00159E74
	public List<CustomGameSettings.MetricSettingsData> GetSettingsForMixingMetrics()
	{
		List<CustomGameSettings.MetricSettingsData> list = new List<CustomGameSettings.MetricSettingsData>();
		foreach (KeyValuePair<string, string> keyValuePair in this.CurrentMixingLevelsBySetting)
		{
			if (DlcManager.IsAllContentSubscribed(this.MixingSettings[keyValuePair.Key].required_content))
			{
				list.Add(new CustomGameSettings.MetricSettingsData
				{
					Name = keyValuePair.Key,
					Value = keyValuePair.Value
				});
			}
		}
		return list;
	}

	// Token: 0x06003E42 RID: 15938 RVA: 0x0015BD10 File Offset: 0x00159F10
	public bool VerifySettingCoordinates()
	{
		bool flag = this.VerifySettingsDictionary(this.QualitySettings);
		bool flag2 = this.VerifySettingsDictionary(this.StorySettings);
		return flag || flag2;
	}

	// Token: 0x06003E43 RID: 15939 RVA: 0x0015BD38 File Offset: 0x00159F38
	private bool VerifySettingsDictionary(Dictionary<string, SettingConfig> configs)
	{
		bool result = false;
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in configs)
		{
			if (keyValuePair.Value.coordinate_range >= 0L)
			{
				List<SettingLevel> levels = keyValuePair.Value.GetLevels();
				if (keyValuePair.Value.coordinate_range < (long)levels.Count)
				{
					result = true;
					global::Debug.Assert(false, string.Concat(new string[]
					{
						keyValuePair.Value.id,
						": Range between coordinate min and max insufficient for all levels (",
						keyValuePair.Value.coordinate_range.ToString(),
						"<",
						levels.Count.ToString(),
						")"
					}));
				}
				foreach (SettingLevel settingLevel in levels)
				{
					Dictionary<long, string> dictionary = new Dictionary<long, string>();
					string text = keyValuePair.Value.id + " > " + settingLevel.id;
					if (keyValuePair.Value.coordinate_range <= settingLevel.coordinate_value)
					{
						result = true;
						global::Debug.Assert(false, string.Format("%s: Level coordinate value (%u) exceedes range (%u)", text, settingLevel.coordinate_value, keyValuePair.Value.coordinate_range));
					}
					if (settingLevel.coordinate_value < 0L)
					{
						result = true;
						global::Debug.Assert(false, text + ": Level coordinate value must be >= 0");
					}
					else if (settingLevel.coordinate_value == 0L)
					{
						if (settingLevel.id != keyValuePair.Value.GetDefaultLevelId())
						{
							result = true;
							global::Debug.Assert(false, text + ": Only the default level should have a coordinate value of 0");
						}
					}
					else
					{
						string str;
						bool flag = !dictionary.TryGetValue(settingLevel.coordinate_value, out str);
						dictionary[settingLevel.coordinate_value] = text;
						if (settingLevel.id == keyValuePair.Value.GetDefaultLevelId())
						{
							result = true;
							global::Debug.Assert(false, text + ": Default level must be a coordinate value of 0");
						}
						if (!flag)
						{
							result = true;
							global::Debug.Assert(false, text + ": Combined coordinate conflicts with another coordinate (" + str + "). Ensure this SettingConfig's min and max don't overlap with another SettingConfig's");
						}
					}
				}
			}
		}
		return result;
	}

	// Token: 0x06003E44 RID: 15940 RVA: 0x0015BFB8 File Offset: 0x0015A1B8
	public static string[] ParseSettingCoordinate(string coord)
	{
		Match match = new Regex("(.*)-(\\d*)-(.*)-(.*)-(.*)").Match(coord);
		for (int i = 1; i <= 2; i++)
		{
			if (match.Groups.Count == 1)
			{
				match = new Regex("(.*)-(\\d*)-(.*)-(.*)-(.*)".Remove("(.*)-(\\d*)-(.*)-(.*)-(.*)".Length - i * 5)).Match(coord);
			}
		}
		string[] array = new string[match.Groups.Count];
		for (int j = 0; j < match.Groups.Count; j++)
		{
			array[j] = match.Groups[j].Value;
		}
		return array;
	}

	// Token: 0x06003E45 RID: 15941 RVA: 0x0015C050 File Offset: 0x0015A250
	public string GetSettingsCoordinate()
	{
		SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.ClusterLayout);
		if (currentQualitySetting == null)
		{
			DebugUtil.DevLogError("GetSettingsCoordinate: clusterLayoutSetting is null, returning '0' coordinate");
			CustomGameSettings.Instance.Print();
			global::Debug.Log("ClusterCache: " + string.Join(",", SettingsCache.clusterLayouts.clusterCache.Keys));
			return "0-0-0-0-0";
		}
		ClusterLayout clusterData = SettingsCache.clusterLayouts.GetClusterData(currentQualitySetting.id);
		SettingLevel currentQualitySetting2 = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.WorldgenSeed);
		string otherSettingsCode = this.GetOtherSettingsCode();
		string storyTraitSettingsCode = this.GetStoryTraitSettingsCode();
		string mixingSettingsCode = this.GetMixingSettingsCode();
		return string.Format("{0}-{1}-{2}-{3}-{4}", new object[]
		{
			clusterData.GetCoordinatePrefix(),
			currentQualitySetting2.id,
			otherSettingsCode,
			storyTraitSettingsCode,
			mixingSettingsCode
		});
	}

	// Token: 0x06003E46 RID: 15942 RVA: 0x0015C11C File Offset: 0x0015A31C
	public void ParseAndApplySettingsCode(string code)
	{
		BigInteger dividend = this.Base36toBinary(code);
		Dictionary<SettingConfig, string> dictionary = new Dictionary<SettingConfig, string>();
		foreach (object obj in global::Util.Reverse(this.CoordinatedQualitySettings))
		{
			string key = (string)obj;
			if (this.QualitySettings.ContainsKey(key))
			{
				SettingConfig settingConfig = this.QualitySettings[key];
				if (settingConfig.coordinate_range != -1L)
				{
					long num = (long)(dividend % settingConfig.coordinate_range);
					dividend /= settingConfig.coordinate_range;
					foreach (SettingLevel settingLevel in settingConfig.GetLevels())
					{
						if (settingLevel.coordinate_value == num)
						{
							dictionary[settingConfig] = settingLevel.id;
							break;
						}
					}
				}
			}
		}
		foreach (KeyValuePair<SettingConfig, string> keyValuePair in dictionary)
		{
			this.SetQualitySetting(keyValuePair.Key, keyValuePair.Value);
		}
	}

	// Token: 0x06003E47 RID: 15943 RVA: 0x0015C288 File Offset: 0x0015A488
	private string GetOtherSettingsCode()
	{
		BigInteger bigInteger = 0;
		foreach (string text in this.CoordinatedQualitySettings)
		{
			SettingConfig settingConfig = this.QualitySettings[text];
			bigInteger *= settingConfig.coordinate_range;
			bigInteger += settingConfig.GetLevel(this.GetCurrentQualitySetting(text).id).coordinate_value;
		}
		return this.BinarytoBase36(bigInteger);
	}

	// Token: 0x06003E48 RID: 15944 RVA: 0x0015C324 File Offset: 0x0015A524
	private BigInteger Base36toBinary(string input)
	{
		if (input == "0")
		{
			return 0;
		}
		BigInteger bigInteger = 0;
		for (int i = input.Length - 1; i >= 0; i--)
		{
			bigInteger *= 36;
			long value = (long)this.hexChars.IndexOf(input[i]);
			bigInteger += value;
		}
		DebugUtil.LogArgs(new object[]
		{
			"tried converting",
			input,
			", got",
			bigInteger,
			"and returns to",
			this.BinarytoBase36(bigInteger)
		});
		return bigInteger;
	}

	// Token: 0x06003E49 RID: 15945 RVA: 0x0015C3CC File Offset: 0x0015A5CC
	private string BinarytoBase36(BigInteger input)
	{
		if (input == 0L)
		{
			return "0";
		}
		BigInteger bigInteger = input;
		string text = "";
		while (bigInteger > 0L)
		{
			text += this.hexChars[(int)(bigInteger % 36)].ToString();
			bigInteger /= 36;
		}
		return text;
	}

	// Token: 0x06003E4E RID: 15950 RVA: 0x0015C528 File Offset: 0x0015A728
	[CompilerGenerated]
	internal static bool <RemoveInvalidMixingSettings>g__HasRequiredContent|71_0(string[] requiredContent, ref CustomGameSettings.<>c__DisplayClass71_0 A_1)
	{
		foreach (string text in requiredContent)
		{
			if (!(text == "") && !A_1.availableDlcs.Contains(text))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x04002655 RID: 9813
	private static CustomGameSettings instance;

	// Token: 0x04002656 RID: 9814
	public const long NO_COORDINATE_RANGE = -1L;

	// Token: 0x04002657 RID: 9815
	private const int NUM_STORY_LEVELS = 3;

	// Token: 0x04002658 RID: 9816
	public const string STORY_DISABLED_LEVEL = "Disabled";

	// Token: 0x04002659 RID: 9817
	public const string STORY_GUARANTEED_LEVEL = "Guaranteed";

	// Token: 0x0400265A RID: 9818
	[Serialize]
	public bool is_custom_game;

	// Token: 0x0400265B RID: 9819
	[Serialize]
	public CustomGameSettings.CustomGameMode customGameMode;

	// Token: 0x0400265C RID: 9820
	[Serialize]
	private Dictionary<string, string> CurrentQualityLevelsBySetting = new Dictionary<string, string>();

	// Token: 0x0400265D RID: 9821
	[Serialize]
	private Dictionary<string, string> CurrentMixingLevelsBySetting = new Dictionary<string, string>();

	// Token: 0x0400265E RID: 9822
	private Dictionary<string, string> currentStoryLevelsBySetting = new Dictionary<string, string>();

	// Token: 0x0400265F RID: 9823
	public List<string> CoordinatedQualitySettings = new List<string>();

	// Token: 0x04002660 RID: 9824
	public Dictionary<string, SettingConfig> QualitySettings = new Dictionary<string, SettingConfig>();

	// Token: 0x04002661 RID: 9825
	public List<string> CoordinatedStorySettings = new List<string>();

	// Token: 0x04002662 RID: 9826
	public Dictionary<string, SettingConfig> StorySettings = new Dictionary<string, SettingConfig>();

	// Token: 0x04002663 RID: 9827
	public List<string> CoordinatedMixingSettings = new List<string>();

	// Token: 0x04002664 RID: 9828
	public Dictionary<string, SettingConfig> MixingSettings = new Dictionary<string, SettingConfig>();

	// Token: 0x04002668 RID: 9832
	private const string coordinatePatern = "(.*)-(\\d*)-(.*)-(.*)-(.*)";

	// Token: 0x04002669 RID: 9833
	private string hexChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

	// Token: 0x020018E3 RID: 6371
	public enum CustomGameMode
	{
		// Token: 0x04007C5D RID: 31837
		Survival,
		// Token: 0x04007C5E RID: 31838
		Nosweat,
		// Token: 0x04007C5F RID: 31839
		Custom = 255
	}

	// Token: 0x020018E4 RID: 6372
	public struct MetricSettingsData
	{
		// Token: 0x04007C60 RID: 31840
		public string Name;

		// Token: 0x04007C61 RID: 31841
		public string Value;
	}
}
