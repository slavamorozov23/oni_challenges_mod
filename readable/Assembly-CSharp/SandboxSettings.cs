using System;
using System.Collections.Generic;

// Token: 0x02000DF9 RID: 3577
public class SandboxSettings
{
	// Token: 0x06007118 RID: 28952 RVA: 0x002B2845 File Offset: 0x002B0A45
	public void AddIntSetting(string prefsKey, Action<int> setAction, int defaultValue)
	{
		this.intSettings.Add(new SandboxSettings.Setting<int>(prefsKey, setAction, defaultValue));
	}

	// Token: 0x06007119 RID: 28953 RVA: 0x002B285A File Offset: 0x002B0A5A
	public int GetIntSetting(string prefsKey)
	{
		return KPlayerPrefs.GetInt(prefsKey);
	}

	// Token: 0x0600711A RID: 28954 RVA: 0x002B2864 File Offset: 0x002B0A64
	public void SetIntSetting(string prefsKey, int value)
	{
		SandboxSettings.Setting<int> setting = this.intSettings.Find((SandboxSettings.Setting<int> match) => match.PrefsKey == prefsKey);
		if (setting == null)
		{
			Debug.LogError(string.Concat(new string[]
			{
				"No intSetting named: ",
				prefsKey,
				" could be found amongst ",
				this.intSettings.Count.ToString(),
				" int settings."
			}));
		}
		setting.Value = value;
	}

	// Token: 0x0600711B RID: 28955 RVA: 0x002B28E7 File Offset: 0x002B0AE7
	public void RestoreIntSetting(string prefsKey)
	{
		if (KPlayerPrefs.HasKey(prefsKey))
		{
			this.SetIntSetting(prefsKey, this.GetIntSetting(prefsKey));
			return;
		}
		this.ForceDefaultIntSetting(prefsKey);
	}

	// Token: 0x0600711C RID: 28956 RVA: 0x002B2908 File Offset: 0x002B0B08
	public void ForceDefaultIntSetting(string prefsKey)
	{
		this.SetIntSetting(prefsKey, this.intSettings.Find((SandboxSettings.Setting<int> match) => match.PrefsKey == prefsKey).defaultValue);
	}

	// Token: 0x0600711D RID: 28957 RVA: 0x002B294A File Offset: 0x002B0B4A
	public void AddFloatSetting(string prefsKey, Action<float> setAction, float defaultValue)
	{
		this.floatSettings.Add(new SandboxSettings.Setting<float>(prefsKey, setAction, defaultValue));
	}

	// Token: 0x0600711E RID: 28958 RVA: 0x002B295F File Offset: 0x002B0B5F
	public float GetFloatSetting(string prefsKey)
	{
		return KPlayerPrefs.GetFloat(prefsKey);
	}

	// Token: 0x0600711F RID: 28959 RVA: 0x002B2968 File Offset: 0x002B0B68
	public void SetFloatSetting(string prefsKey, float value)
	{
		SandboxSettings.Setting<float> setting = this.floatSettings.Find((SandboxSettings.Setting<float> match) => match.PrefsKey == prefsKey);
		if (setting == null)
		{
			Debug.LogError(string.Concat(new string[]
			{
				"No KPlayerPrefs float setting named: ",
				prefsKey,
				" could be found amongst ",
				this.floatSettings.Count.ToString(),
				" float settings."
			}));
		}
		setting.Value = value;
	}

	// Token: 0x06007120 RID: 28960 RVA: 0x002B29EB File Offset: 0x002B0BEB
	public void RestoreFloatSetting(string prefsKey)
	{
		if (KPlayerPrefs.HasKey(prefsKey))
		{
			this.SetFloatSetting(prefsKey, this.GetFloatSetting(prefsKey));
			return;
		}
		this.ForceDefaultFloatSetting(prefsKey);
	}

	// Token: 0x06007121 RID: 28961 RVA: 0x002B2A0C File Offset: 0x002B0C0C
	public void ForceDefaultFloatSetting(string prefsKey)
	{
		this.SetFloatSetting(prefsKey, this.floatSettings.Find((SandboxSettings.Setting<float> match) => match.PrefsKey == prefsKey).defaultValue);
	}

	// Token: 0x06007122 RID: 28962 RVA: 0x002B2A4E File Offset: 0x002B0C4E
	public void AddStringSetting(string prefsKey, Action<string> setAction, string defaultValue)
	{
		this.stringSettings.Add(new SandboxSettings.Setting<string>(prefsKey, setAction, defaultValue));
	}

	// Token: 0x06007123 RID: 28963 RVA: 0x002B2A63 File Offset: 0x002B0C63
	public string GetStringSetting(string prefsKey)
	{
		return KPlayerPrefs.GetString(prefsKey);
	}

	// Token: 0x06007124 RID: 28964 RVA: 0x002B2A6C File Offset: 0x002B0C6C
	public void SetStringSetting(string prefsKey, string value)
	{
		SandboxSettings.Setting<string> setting = this.stringSettings.Find((SandboxSettings.Setting<string> match) => match.PrefsKey == prefsKey);
		if (setting == null)
		{
			Debug.LogError(string.Concat(new string[]
			{
				"No KPlayerPrefs string setting named: ",
				prefsKey,
				" could be found amongst ",
				this.stringSettings.Count.ToString(),
				" settings."
			}));
		}
		setting.Value = value;
	}

	// Token: 0x06007125 RID: 28965 RVA: 0x002B2AEF File Offset: 0x002B0CEF
	public void RestoreStringSetting(string prefsKey)
	{
		if (KPlayerPrefs.HasKey(prefsKey))
		{
			this.SetStringSetting(prefsKey, this.GetStringSetting(prefsKey));
			return;
		}
		this.ForceDefaultStringSetting(prefsKey);
	}

	// Token: 0x06007126 RID: 28966 RVA: 0x002B2B10 File Offset: 0x002B0D10
	public void ForceDefaultStringSetting(string prefsKey)
	{
		this.SetStringSetting(prefsKey, this.stringSettings.Find((SandboxSettings.Setting<string> match) => match.PrefsKey == prefsKey).defaultValue);
	}

	// Token: 0x06007127 RID: 28967 RVA: 0x002B2B54 File Offset: 0x002B0D54
	public SandboxSettings()
	{
		this.AddStringSetting("SandboxTools.SelectedEntity", delegate(string data)
		{
			KPlayerPrefs.SetString("SandboxTools.SelectedEntity", data);
			this.OnChangeEntity();
		}, "MushBar");
		this.AddIntSetting("SandboxTools.SelectedElement", delegate(int data)
		{
			KPlayerPrefs.SetInt("SandboxTools.SelectedElement", data);
			this.OnChangeElement(this.hasRestoredElement);
			this.hasRestoredElement = true;
		}, (int)ElementLoader.GetElementIndex(SimHashes.Oxygen));
		this.AddStringSetting("SandboxTools.SelectedDisease", delegate(string data)
		{
			KPlayerPrefs.SetString("SandboxTools.SelectedDisease", data);
			this.OnChangeDisease();
		}, Db.Get().Diseases.FoodGerms.Id);
		this.AddIntSetting("SandboxTools.DiseaseCount", delegate(int val)
		{
			KPlayerPrefs.SetInt("SandboxTools.DiseaseCount", val);
			this.OnChangeDiseaseCount();
		}, 0);
		this.AddStringSetting("SandboxTools.SelectedStory", delegate(string data)
		{
			KPlayerPrefs.SetString("SandboxTools.SelectedStory", data);
			this.OnChangeStory();
		}, Db.Get().Stories.resources[Db.Get().Stories.resources.Count - 1].Id);
		this.AddIntSetting("SandboxTools.BrushSize", delegate(int val)
		{
			KPlayerPrefs.SetInt("SandboxTools.BrushSize", val);
			this.OnChangeBrushSize();
		}, 1);
		this.AddFloatSetting("SandboxTools.NoiseScale", delegate(float val)
		{
			KPlayerPrefs.SetFloat("SandboxTools.NoiseScale", val);
			this.OnChangeNoiseScale();
		}, 1f);
		this.AddFloatSetting("SandboxTools.NoiseDensity", delegate(float val)
		{
			KPlayerPrefs.SetFloat("SandboxTools.NoiseDensity", val);
			this.OnChangeNoiseDensity();
		}, 1f);
		this.AddFloatSetting("SandboxTools.Mass", delegate(float val)
		{
			KPlayerPrefs.SetFloat("SandboxTools.Mass", val);
			this.OnChangeMass();
		}, 1f);
		this.AddFloatSetting("SandbosTools.Temperature", delegate(float val)
		{
			KPlayerPrefs.SetFloat("SandbosTools.Temperature", val);
			this.OnChangeTemperature();
		}, 300f);
		this.AddFloatSetting("SandbosTools.TemperatureAdditive", delegate(float val)
		{
			KPlayerPrefs.SetFloat("SandbosTools.TemperatureAdditive", val);
			this.OnChangeAdditiveTemperature();
		}, 5f);
		this.AddFloatSetting("SandbosTools.StressAdditive", delegate(float val)
		{
			KPlayerPrefs.SetFloat("SandbosTools.StressAdditive", val);
			this.OnChangeAdditiveStress();
		}, 50f);
		this.AddIntSetting("SandbosTools.MoraleAdjustment", delegate(int val)
		{
			KPlayerPrefs.SetInt("SandbosTools.MoraleAdjustment", val);
			this.OnChangeMoraleAdjustment();
		}, 50);
	}

	// Token: 0x06007128 RID: 28968 RVA: 0x002B2D30 File Offset: 0x002B0F30
	public void RestorePrefs()
	{
		foreach (SandboxSettings.Setting<int> setting in this.intSettings)
		{
			this.RestoreIntSetting(setting.PrefsKey);
		}
		foreach (SandboxSettings.Setting<float> setting2 in this.floatSettings)
		{
			this.RestoreFloatSetting(setting2.PrefsKey);
		}
		foreach (SandboxSettings.Setting<string> setting3 in this.stringSettings)
		{
			this.RestoreStringSetting(setting3.PrefsKey);
		}
	}

	// Token: 0x04004E0F RID: 19983
	private List<SandboxSettings.Setting<int>> intSettings = new List<SandboxSettings.Setting<int>>();

	// Token: 0x04004E10 RID: 19984
	private List<SandboxSettings.Setting<float>> floatSettings = new List<SandboxSettings.Setting<float>>();

	// Token: 0x04004E11 RID: 19985
	private List<SandboxSettings.Setting<string>> stringSettings = new List<SandboxSettings.Setting<string>>();

	// Token: 0x04004E12 RID: 19986
	public bool InstantBuild = true;

	// Token: 0x04004E13 RID: 19987
	private bool hasRestoredElement;

	// Token: 0x04004E14 RID: 19988
	public Action<bool> OnChangeElement;

	// Token: 0x04004E15 RID: 19989
	public System.Action OnChangeMass;

	// Token: 0x04004E16 RID: 19990
	public System.Action OnChangeDisease;

	// Token: 0x04004E17 RID: 19991
	public System.Action OnChangeDiseaseCount;

	// Token: 0x04004E18 RID: 19992
	public System.Action OnChangeStory;

	// Token: 0x04004E19 RID: 19993
	public System.Action OnChangeEntity;

	// Token: 0x04004E1A RID: 19994
	public System.Action OnChangeBrushSize;

	// Token: 0x04004E1B RID: 19995
	public System.Action OnChangeNoiseScale;

	// Token: 0x04004E1C RID: 19996
	public System.Action OnChangeNoiseDensity;

	// Token: 0x04004E1D RID: 19997
	public System.Action OnChangeTemperature;

	// Token: 0x04004E1E RID: 19998
	public System.Action OnChangeAdditiveTemperature;

	// Token: 0x04004E1F RID: 19999
	public System.Action OnChangeAdditiveStress;

	// Token: 0x04004E20 RID: 20000
	public System.Action OnChangeMoraleAdjustment;

	// Token: 0x04004E21 RID: 20001
	public const string KEY_SELECTED_ENTITY = "SandboxTools.SelectedEntity";

	// Token: 0x04004E22 RID: 20002
	public const string KEY_SELECTED_ELEMENT = "SandboxTools.SelectedElement";

	// Token: 0x04004E23 RID: 20003
	public const string KEY_SELECTED_DISEASE = "SandboxTools.SelectedDisease";

	// Token: 0x04004E24 RID: 20004
	public const string KEY_DISEASE_COUNT = "SandboxTools.DiseaseCount";

	// Token: 0x04004E25 RID: 20005
	public const string KEY_SELECTED_STORY = "SandboxTools.SelectedStory";

	// Token: 0x04004E26 RID: 20006
	public const string KEY_BRUSH_SIZE = "SandboxTools.BrushSize";

	// Token: 0x04004E27 RID: 20007
	public const string KEY_NOISE_SCALE = "SandboxTools.NoiseScale";

	// Token: 0x04004E28 RID: 20008
	public const string KEY_NOISE_DENSITY = "SandboxTools.NoiseDensity";

	// Token: 0x04004E29 RID: 20009
	public const string KEY_MASS = "SandboxTools.Mass";

	// Token: 0x04004E2A RID: 20010
	public const string KEY_TEMPERATURE = "SandbosTools.Temperature";

	// Token: 0x04004E2B RID: 20011
	public const string KEY_TEMPERATURE_ADDITIVE = "SandbosTools.TemperatureAdditive";

	// Token: 0x04004E2C RID: 20012
	public const string KEY_STRESS_ADDITIVE = "SandbosTools.StressAdditive";

	// Token: 0x04004E2D RID: 20013
	public const string KEY_MORALE_ADJUSTMENT = "SandbosTools.MoraleAdjustment";

	// Token: 0x02002073 RID: 8307
	public class Setting<T>
	{
		// Token: 0x0600B937 RID: 47415 RVA: 0x003F79B1 File Offset: 0x003F5BB1
		public Setting(string prefsKey, Action<T> setAction, T defaultValue)
		{
			this.prefsKey = prefsKey;
			this.SetAction = setAction;
			this.defaultValue = defaultValue;
		}

		// Token: 0x17000D06 RID: 3334
		// (get) Token: 0x0600B938 RID: 47416 RVA: 0x003F79CE File Offset: 0x003F5BCE
		public string PrefsKey
		{
			get
			{
				return this.prefsKey;
			}
		}

		// Token: 0x17000D07 RID: 3335
		// (set) Token: 0x0600B939 RID: 47417 RVA: 0x003F79D6 File Offset: 0x003F5BD6
		public T Value
		{
			set
			{
				this.SetAction(value);
			}
		}

		// Token: 0x04009614 RID: 38420
		private string prefsKey;

		// Token: 0x04009615 RID: 38421
		private Action<T> SetAction;

		// Token: 0x04009616 RID: 38422
		public T defaultValue;
	}
}
