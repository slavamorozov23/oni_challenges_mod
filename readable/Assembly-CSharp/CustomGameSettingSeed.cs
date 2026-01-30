using System;
using Klei.CustomSettings;
using ProcGen;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000CE8 RID: 3304
public class CustomGameSettingSeed : CustomGameSettingWidget
{
	// Token: 0x060065FF RID: 26111 RVA: 0x00266824 File Offset: 0x00264A24
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.Input.onEndEdit.AddListener(new UnityAction<string>(this.OnEndEdit));
		this.Input.onValueChanged.AddListener(new UnityAction<string>(this.OnValueChanged));
		this.RandomizeButton.onClick += this.GetNewRandomSeed;
	}

	// Token: 0x06006600 RID: 26112 RVA: 0x00266886 File Offset: 0x00264A86
	public void Initialize(SeedSettingConfig config)
	{
		this.config = config;
		this.Label.text = config.label;
		this.ToolTip.toolTip = config.tooltip;
		this.GetNewRandomSeed();
	}

	// Token: 0x06006601 RID: 26113 RVA: 0x002668B8 File Offset: 0x00264AB8
	public override void Refresh()
	{
		base.Refresh();
		string currentQualitySettingLevelId = CustomGameSettings.Instance.GetCurrentQualitySettingLevelId(this.config);
		ClusterLayout currentClusterLayout = CustomGameSettings.Instance.GetCurrentClusterLayout();
		this.allowChange = (currentClusterLayout.fixedCoordinate == -1);
		this.Input.interactable = this.allowChange;
		this.RandomizeButton.isInteractable = this.allowChange;
		if (this.allowChange)
		{
			this.InputToolTip.enabled = false;
			this.RandomizeButtonToolTip.enabled = false;
		}
		else
		{
			this.InputToolTip.enabled = true;
			this.RandomizeButtonToolTip.enabled = true;
			this.InputToolTip.SetSimpleTooltip(UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.WORLDGEN_SEED.FIXEDSEED);
			this.RandomizeButtonToolTip.SetSimpleTooltip(UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.WORLDGEN_SEED.FIXEDSEED);
		}
		this.Input.text = currentQualitySettingLevelId;
	}

	// Token: 0x06006602 RID: 26114 RVA: 0x00266988 File Offset: 0x00264B88
	private char ValidateInput(string text, int charIndex, char addedChar)
	{
		if ('0' > addedChar || addedChar > '9')
		{
			return '\0';
		}
		return addedChar;
	}

	// Token: 0x06006603 RID: 26115 RVA: 0x00266998 File Offset: 0x00264B98
	private void OnEndEdit(string text)
	{
		int seed;
		try
		{
			seed = Convert.ToInt32(text);
		}
		catch
		{
			seed = 0;
		}
		this.SetSeed(seed);
	}

	// Token: 0x06006604 RID: 26116 RVA: 0x002669CC File Offset: 0x00264BCC
	public void SetSeed(int seed)
	{
		seed = Mathf.Min(seed, int.MaxValue);
		CustomGameSettings.Instance.SetQualitySetting(this.config, seed.ToString());
		this.Refresh();
	}

	// Token: 0x06006605 RID: 26117 RVA: 0x002669F8 File Offset: 0x00264BF8
	private void OnValueChanged(string text)
	{
		int num = 0;
		try
		{
			num = Convert.ToInt32(text);
		}
		catch
		{
			if (text.Length > 0)
			{
				this.Input.text = text.Substring(0, text.Length - 1);
			}
			else
			{
				this.Input.text = "";
			}
		}
		if (num > 2147483647)
		{
			this.Input.text = text.Substring(0, text.Length - 1);
		}
	}

	// Token: 0x06006606 RID: 26118 RVA: 0x00266A7C File Offset: 0x00264C7C
	private void GetNewRandomSeed()
	{
		int seed = UnityEngine.Random.Range(0, int.MaxValue);
		this.SetSeed(seed);
	}

	// Token: 0x04004591 RID: 17809
	[SerializeField]
	private LocText Label;

	// Token: 0x04004592 RID: 17810
	[SerializeField]
	private ToolTip ToolTip;

	// Token: 0x04004593 RID: 17811
	[SerializeField]
	private KInputTextField Input;

	// Token: 0x04004594 RID: 17812
	[SerializeField]
	private KButton RandomizeButton;

	// Token: 0x04004595 RID: 17813
	[SerializeField]
	private ToolTip InputToolTip;

	// Token: 0x04004596 RID: 17814
	[SerializeField]
	private ToolTip RandomizeButtonToolTip;

	// Token: 0x04004597 RID: 17815
	private const int MAX_VALID_SEED = 2147483647;

	// Token: 0x04004598 RID: 17816
	private SeedSettingConfig config;

	// Token: 0x04004599 RID: 17817
	private bool allowChange = true;
}
