using System;
using Klei.CustomSettings;
using ProcGen;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000CDC RID: 3292
public class ColonyDestinationSelectScreen : NewGameFlowScreen
{
	// Token: 0x060065A3 RID: 26019 RVA: 0x0026434C File Offset: 0x0026254C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.backButton.onClick += this.BackClicked;
		this.customizeButton.onClick += this.CustomizeClicked;
		this.launchButton.onClick += this.LaunchClicked;
		this.shuffleButton.onClick += this.ShuffleClicked;
		this.storyTraitShuffleButton.onClick += this.StoryTraitShuffleClicked;
		this.storyTraitShuffleButton.gameObject.SetActive(Db.Get().Stories.Count > 5);
		this.destinationMapPanel.OnAsteroidClicked += this.OnAsteroidClicked;
		KInputTextField kinputTextField = this.coordinate;
		kinputTextField.onFocus = (System.Action)Delegate.Combine(kinputTextField.onFocus, new System.Action(this.CoordinateEditStarted));
		this.coordinate.onEndEdit.AddListener(new UnityAction<string>(this.CoordinateEditFinished));
		if (this.locationIcons != null)
		{
			bool cloudSavesAvailable = SaveLoader.GetCloudSavesAvailable();
			this.locationIcons.gameObject.SetActive(cloudSavesAvailable);
		}
		this.random = new KRandom();
	}

	// Token: 0x060065A4 RID: 26020 RVA: 0x00264480 File Offset: 0x00262680
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.RefreshCloudSavePref();
		this.RefreshCloudLocalIcon();
		this.newGameSettingsPanel.Init();
		this.newGameSettingsPanel.SetCloseAction(new System.Action(this.CustomizeClose));
		this.destinationMapPanel.Init();
		this.mixingPanel.Init();
		this.ShuffleClicked();
		this.RefreshMenuTabs();
		for (int i = 0; i < this.menuTabs.Length; i++)
		{
			int target = i;
			this.menuTabs[i].onClick = delegate()
			{
				this.selectedMenuTabIdx = target;
				this.RefreshMenuTabs();
			};
		}
		this.ResizeLayout();
		this.storyContentPanel.Init();
		this.storyContentPanel.SelectRandomStories(5, 5, true);
		this.storyContentPanel.SelectDefault();
		this.RefreshStoryLabel();
		this.RefreshRowsAndDescriptions();
		CustomGameSettings.Instance.OnQualitySettingChanged += this.QualitySettingChanged;
		CustomGameSettings.Instance.OnStorySettingChanged += this.QualitySettingChanged;
		CustomGameSettings.Instance.OnMixingSettingChanged += this.QualitySettingChanged;
		this.coordinate.text = CustomGameSettings.Instance.GetSettingsCoordinate();
	}

	// Token: 0x060065A5 RID: 26021 RVA: 0x002645B0 File Offset: 0x002627B0
	private void ResizeLayout()
	{
		Vector2 sizeDelta = this.destinationProperties.clusterDetailsButton.rectTransform().sizeDelta;
		this.destinationProperties.clusterDetailsButton.rectTransform().sizeDelta = new Vector2(sizeDelta.x, (float)(DlcManager.FeatureClusterSpaceEnabled() ? 164 : 76));
		Vector2 sizeDelta2 = this.worldsScrollPanel.rectTransform().sizeDelta;
		Vector2 anchoredPosition = this.worldsScrollPanel.rectTransform().anchoredPosition;
		if (!DlcManager.FeatureClusterSpaceEnabled())
		{
			this.worldsScrollPanel.rectTransform().anchoredPosition = new Vector2(anchoredPosition.x, anchoredPosition.y + 88f);
		}
		float num = (float)(DlcManager.FeatureClusterSpaceEnabled() ? 436 : 524);
		LayoutRebuilder.ForceRebuildLayoutImmediate(base.gameObject.rectTransform());
		num = Mathf.Min(num, this.destinationInfoPanel.sizeDelta.y - (float)(DlcManager.FeatureClusterSpaceEnabled() ? 164 : 76) - 22f);
		this.worldsScrollPanel.rectTransform().sizeDelta = new Vector2(sizeDelta2.x, num);
		this.storyScrollPanel.rectTransform().sizeDelta = new Vector2(sizeDelta2.x, num);
		this.mixingScrollPanel.rectTransform().sizeDelta = new Vector2(sizeDelta2.x, num);
		this.gameSettingsScrollPanel.rectTransform().sizeDelta = new Vector2(sizeDelta2.x, num);
	}

	// Token: 0x060065A6 RID: 26022 RVA: 0x00264718 File Offset: 0x00262918
	protected override void OnCleanUp()
	{
		CustomGameSettings.Instance.OnQualitySettingChanged -= this.QualitySettingChanged;
		CustomGameSettings.Instance.OnStorySettingChanged -= this.QualitySettingChanged;
		this.newGameSettingsPanel.Uninit();
		this.destinationMapPanel.Uninit();
		this.mixingPanel.Uninit();
		this.storyContentPanel.Cleanup();
		base.OnCleanUp();
	}

	// Token: 0x060065A7 RID: 26023 RVA: 0x00264784 File Offset: 0x00262984
	private void RefreshCloudLocalIcon()
	{
		if (this.locationIcons == null)
		{
			return;
		}
		if (!SaveLoader.GetCloudSavesAvailable())
		{
			return;
		}
		HierarchyReferences component = this.locationIcons.GetComponent<HierarchyReferences>();
		LocText component2 = component.GetReference<RectTransform>("LocationText").GetComponent<LocText>();
		KButton component3 = component.GetReference<RectTransform>("CloudButton").GetComponent<KButton>();
		KButton component4 = component.GetReference<RectTransform>("LocalButton").GetComponent<KButton>();
		ToolTip component5 = component3.GetComponent<ToolTip>();
		ToolTip component6 = component4.GetComponent<ToolTip>();
		component5.toolTip = string.Format("{0}\n{1}", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SAVETOCLOUD.TOOLTIP, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SAVETOCLOUD.TOOLTIP_EXTRA);
		component6.toolTip = string.Format("{0}\n{1}", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SAVETOCLOUD.TOOLTIP_LOCAL, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SAVETOCLOUD.TOOLTIP_EXTRA);
		bool flag = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.SaveToCloud).id == "Enabled";
		component2.text = (flag ? UI.FRONTEND.LOADSCREEN.CLOUD_SAVE : UI.FRONTEND.LOADSCREEN.LOCAL_SAVE);
		component3.gameObject.SetActive(flag);
		component3.ClearOnClick();
		if (flag)
		{
			component3.onClick += delegate()
			{
				CustomGameSettings.Instance.SetQualitySetting(CustomGameSettingConfigs.SaveToCloud, "Disabled");
				this.RefreshCloudLocalIcon();
			};
		}
		component4.gameObject.SetActive(!flag);
		component4.ClearOnClick();
		if (!flag)
		{
			component4.onClick += delegate()
			{
				CustomGameSettings.Instance.SetQualitySetting(CustomGameSettingConfigs.SaveToCloud, "Enabled");
				this.RefreshCloudLocalIcon();
			};
		}
	}

	// Token: 0x060065A8 RID: 26024 RVA: 0x002648B8 File Offset: 0x00262AB8
	private void RefreshCloudSavePref()
	{
		if (!SaveLoader.GetCloudSavesAvailable())
		{
			return;
		}
		string cloudSavesDefaultPref = SaveLoader.GetCloudSavesDefaultPref();
		CustomGameSettings.Instance.SetQualitySetting(CustomGameSettingConfigs.SaveToCloud, cloudSavesDefaultPref);
	}

	// Token: 0x060065A9 RID: 26025 RVA: 0x002648E3 File Offset: 0x00262AE3
	private void BackClicked()
	{
		this.newGameSettingsPanel.Cancel();
		base.NavigateBackward();
	}

	// Token: 0x060065AA RID: 26026 RVA: 0x002648F6 File Offset: 0x00262AF6
	private void CustomizeClicked()
	{
		this.newGameSettingsPanel.Refresh();
		this.customSettings.SetActive(true);
	}

	// Token: 0x060065AB RID: 26027 RVA: 0x0026490F File Offset: 0x00262B0F
	private void CustomizeClose()
	{
		this.customSettings.SetActive(false);
	}

	// Token: 0x060065AC RID: 26028 RVA: 0x0026491D File Offset: 0x00262B1D
	private void LaunchClicked()
	{
		CustomGameSettings.Instance.RemoveInvalidMixingSettings();
		base.NavigateForward();
	}

	// Token: 0x060065AD RID: 26029 RVA: 0x00264930 File Offset: 0x00262B30
	private void RefreshMenuTabs()
	{
		for (int i = 0; i < this.menuTabs.Length; i++)
		{
			this.menuTabs[i].ChangeState((i == this.selectedMenuTabIdx) ? 1 : 0);
			LocText componentInChildren = this.menuTabs[i].GetComponentInChildren<LocText>();
			HierarchyReferences component = this.menuTabs[i].GetComponent<HierarchyReferences>();
			if (componentInChildren != null)
			{
				componentInChildren.color = ((i == this.selectedMenuTabIdx) ? Color.white : Color.grey);
			}
			if (component != null)
			{
				Image reference = component.GetReference<Image>("Icon");
				if (reference != null)
				{
					reference.color = ((i == this.selectedMenuTabIdx) ? Color.white : Color.grey);
				}
			}
		}
		this.destinationInfoPanel.gameObject.SetActive(this.selectedMenuTabIdx == 1);
		this.storyInfoPanel.gameObject.SetActive(this.selectedMenuTabIdx == 2);
		this.mixingSettingsPanel.gameObject.SetActive(this.selectedMenuTabIdx == 3);
		this.gameSettingsPanel.gameObject.SetActive(this.selectedMenuTabIdx == 4);
		int num = this.selectedMenuTabIdx;
		if (num != 1)
		{
			if (num == 2)
			{
				this.destinationDetailsHeader.SetParent(this.destinationDetailsParent_Story);
			}
		}
		else
		{
			this.destinationDetailsHeader.SetParent(this.destinationDetailsParent_Asteroid);
		}
		this.destinationDetailsHeader.SetAsFirstSibling();
	}

	// Token: 0x060065AE RID: 26030 RVA: 0x00264A90 File Offset: 0x00262C90
	private void ShuffleClicked()
	{
		ClusterLayout currentClusterLayout = CustomGameSettings.Instance.GetCurrentClusterLayout();
		int num = this.random.Next();
		if (currentClusterLayout != null && currentClusterLayout.fixedCoordinate != -1)
		{
			num = currentClusterLayout.fixedCoordinate;
		}
		this.newGameSettingsPanel.SetSetting(CustomGameSettingConfigs.WorldgenSeed, num.ToString(), true);
	}

	// Token: 0x060065AF RID: 26031 RVA: 0x00264ADF File Offset: 0x00262CDF
	private void StoryTraitShuffleClicked()
	{
		this.storyContentPanel.SelectRandomStories(5, 5, false);
	}

	// Token: 0x060065B0 RID: 26032 RVA: 0x00264AF0 File Offset: 0x00262CF0
	private void CoordinateChanged(string text)
	{
		string[] array = CustomGameSettings.ParseSettingCoordinate(text);
		if (array.Length < 4 || array.Length > 6)
		{
			return;
		}
		int num;
		if (!int.TryParse(array[2], out num))
		{
			return;
		}
		ClusterLayout clusterLayout = null;
		foreach (string name in SettingsCache.GetClusterNames())
		{
			ClusterLayout clusterData = SettingsCache.clusterLayouts.GetClusterData(name);
			if (clusterData.coordinatePrefix == array[1])
			{
				clusterLayout = clusterData;
			}
		}
		if (clusterLayout != null)
		{
			this.newGameSettingsPanel.SetSetting(CustomGameSettingConfigs.ClusterLayout, clusterLayout.filePath, true);
		}
		this.newGameSettingsPanel.SetSetting(CustomGameSettingConfigs.WorldgenSeed, array[2], true);
		this.newGameSettingsPanel.ConsumeSettingsCode(array[3]);
		string code = (array.Length >= 5) ? array[4] : "0";
		this.newGameSettingsPanel.ConsumeStoryTraitsCode(code);
		string code2 = (array.Length >= 6) ? array[5] : "0";
		this.newGameSettingsPanel.ConsumeMixingSettingsCode(code2);
	}

	// Token: 0x060065B1 RID: 26033 RVA: 0x00264BF8 File Offset: 0x00262DF8
	private void CoordinateEditStarted()
	{
		this.isEditingCoordinate = true;
	}

	// Token: 0x060065B2 RID: 26034 RVA: 0x00264C01 File Offset: 0x00262E01
	private void CoordinateEditFinished(string text)
	{
		this.CoordinateChanged(text);
		this.isEditingCoordinate = false;
		this.coordinate.text = CustomGameSettings.Instance.GetSettingsCoordinate();
	}

	// Token: 0x060065B3 RID: 26035 RVA: 0x00264C28 File Offset: 0x00262E28
	private void QualitySettingChanged(SettingConfig config, SettingLevel level)
	{
		if (config == CustomGameSettingConfigs.SaveToCloud)
		{
			this.RefreshCloudLocalIcon();
		}
		if (!this.destinationDetailsHeader.IsNullOrDestroyed())
		{
			if (!this.isEditingCoordinate && !this.coordinate.IsNullOrDestroyed())
			{
				this.coordinate.text = CustomGameSettings.Instance.GetSettingsCoordinate();
			}
			this.RefreshRowsAndDescriptions();
		}
	}

	// Token: 0x060065B4 RID: 26036 RVA: 0x00264C80 File Offset: 0x00262E80
	public void RefreshRowsAndDescriptions()
	{
		string setting = this.newGameSettingsPanel.GetSetting(CustomGameSettingConfigs.ClusterLayout);
		int seed;
		int.TryParse(this.newGameSettingsPanel.GetSetting(CustomGameSettingConfigs.WorldgenSeed), out seed);
		int fixedCoordinate = CustomGameSettings.Instance.GetCurrentClusterLayout().fixedCoordinate;
		if (fixedCoordinate != -1)
		{
			this.newGameSettingsPanel.SetSetting(CustomGameSettingConfigs.WorldgenSeed, fixedCoordinate.ToString(), false);
			seed = fixedCoordinate;
			this.shuffleButton.isInteractable = false;
			this.shuffleButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.FRONTEND.COLONYDESTINATIONSCREEN.SHUFFLETOOLTIP_DISABLED);
		}
		else
		{
			this.coordinate.interactable = true;
			this.shuffleButton.isInteractable = true;
			this.shuffleButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.FRONTEND.COLONYDESTINATIONSCREEN.SHUFFLETOOLTIP);
		}
		ColonyDestinationAsteroidBeltData cluster;
		try
		{
			cluster = this.destinationMapPanel.SelectCluster(setting, seed);
		}
		catch
		{
			string defaultAsteroid = this.destinationMapPanel.GetDefaultAsteroid();
			this.newGameSettingsPanel.SetSetting(CustomGameSettingConfigs.ClusterLayout, defaultAsteroid, true);
			cluster = this.destinationMapPanel.SelectCluster(defaultAsteroid, seed);
		}
		if (DlcManager.IsContentSubscribed("EXPANSION1_ID"))
		{
			this.destinationProperties.EnableClusterLocationLabels(true);
			this.destinationProperties.RefreshAsteroidLines(cluster, this.selectedLocationProperties, this.storyContentPanel.GetActiveStories());
			this.destinationProperties.EnableClusterDetails(true);
			this.destinationProperties.SetClusterDetailLabels(cluster);
			this.selectedLocationProperties.headerLabel.SetText(UI.FRONTEND.COLONYDESTINATIONSCREEN.SELECTED_CLUSTER_TRAITS_HEADER);
			this.destinationProperties.clusterDetailsButton.onClick = delegate()
			{
				this.destinationProperties.SelectWholeClusterDetails(cluster, this.selectedLocationProperties, this.storyContentPanel.GetActiveStories());
			};
		}
		else
		{
			this.destinationProperties.EnableClusterDetails(false);
			this.destinationProperties.EnableClusterLocationLabels(false);
			this.destinationProperties.SetParameterDescriptors(cluster.GetParamDescriptors());
			this.selectedLocationProperties.SetTraitDescriptors(cluster.GetTraitDescriptors(), this.storyContentPanel.GetActiveStories(), true);
		}
		this.RefreshStoryLabel();
	}

	// Token: 0x060065B5 RID: 26037 RVA: 0x00264E90 File Offset: 0x00263090
	public void RefreshStoryLabel()
	{
		this.storyTraitsDestinationDetailsLabel.SetText(this.storyContentPanel.GetTraitsString(false));
		this.storyTraitsDestinationDetailsLabel.GetComponent<ToolTip>().SetSimpleTooltip(this.storyContentPanel.GetTraitsString(true));
	}

	// Token: 0x060065B6 RID: 26038 RVA: 0x00264EC5 File Offset: 0x002630C5
	private void OnAsteroidClicked(ColonyDestinationAsteroidBeltData cluster)
	{
		this.newGameSettingsPanel.SetSetting(CustomGameSettingConfigs.ClusterLayout, cluster.beltPath, true);
		this.ShuffleClicked();
	}

	// Token: 0x060065B7 RID: 26039 RVA: 0x00264EE4 File Offset: 0x002630E4
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.isEditingCoordinate)
		{
			return;
		}
		if (!e.Consumed && e.TryConsume(global::Action.PanLeft))
		{
			this.destinationMapPanel.ScrollLeft();
		}
		else if (!e.Consumed && e.TryConsume(global::Action.PanRight))
		{
			this.destinationMapPanel.ScrollRight();
		}
		else if (this.customSettings.activeSelf && !e.Consumed && (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight)))
		{
			this.CustomizeClose();
		}
		base.OnKeyDown(e);
	}

	// Token: 0x040044DD RID: 17629
	[SerializeField]
	private GameObject destinationMap;

	// Token: 0x040044DE RID: 17630
	[SerializeField]
	private GameObject customSettings;

	// Token: 0x040044DF RID: 17631
	[Header("Menu")]
	[SerializeField]
	private MultiToggle[] menuTabs;

	// Token: 0x040044E0 RID: 17632
	private int selectedMenuTabIdx = 1;

	// Token: 0x040044E1 RID: 17633
	[Header("Buttons")]
	[SerializeField]
	private KButton backButton;

	// Token: 0x040044E2 RID: 17634
	[SerializeField]
	private KButton customizeButton;

	// Token: 0x040044E3 RID: 17635
	[SerializeField]
	private KButton launchButton;

	// Token: 0x040044E4 RID: 17636
	[SerializeField]
	private KButton shuffleButton;

	// Token: 0x040044E5 RID: 17637
	[SerializeField]
	private KButton storyTraitShuffleButton;

	// Token: 0x040044E6 RID: 17638
	[Header("Scroll Panels")]
	[SerializeField]
	private RectTransform worldsScrollPanel;

	// Token: 0x040044E7 RID: 17639
	[SerializeField]
	private RectTransform storyScrollPanel;

	// Token: 0x040044E8 RID: 17640
	[SerializeField]
	private RectTransform mixingScrollPanel;

	// Token: 0x040044E9 RID: 17641
	[SerializeField]
	private RectTransform gameSettingsScrollPanel;

	// Token: 0x040044EA RID: 17642
	[Header("Panels")]
	[SerializeField]
	private RectTransform destinationDetailsHeader;

	// Token: 0x040044EB RID: 17643
	[SerializeField]
	private RectTransform destinationInfoPanel;

	// Token: 0x040044EC RID: 17644
	[SerializeField]
	private RectTransform storyInfoPanel;

	// Token: 0x040044ED RID: 17645
	[SerializeField]
	private RectTransform mixingSettingsPanel;

	// Token: 0x040044EE RID: 17646
	[SerializeField]
	private RectTransform gameSettingsPanel;

	// Token: 0x040044EF RID: 17647
	[Header("References")]
	[SerializeField]
	private RectTransform destinationDetailsParent_Asteroid;

	// Token: 0x040044F0 RID: 17648
	[SerializeField]
	private RectTransform destinationDetailsParent_Story;

	// Token: 0x040044F1 RID: 17649
	[SerializeField]
	private LocText storyTraitsDestinationDetailsLabel;

	// Token: 0x040044F2 RID: 17650
	[SerializeField]
	private HierarchyReferences locationIcons;

	// Token: 0x040044F3 RID: 17651
	[SerializeField]
	private KInputTextField coordinate;

	// Token: 0x040044F4 RID: 17652
	[SerializeField]
	private StoryContentPanel storyContentPanel;

	// Token: 0x040044F5 RID: 17653
	[SerializeField]
	private AsteroidDescriptorPanel destinationProperties;

	// Token: 0x040044F6 RID: 17654
	[SerializeField]
	private AsteroidDescriptorPanel selectedLocationProperties;

	// Token: 0x040044F7 RID: 17655
	private const int DESTINATION_HEADER_BUTTON_HEIGHT_CLUSTER = 164;

	// Token: 0x040044F8 RID: 17656
	private const int DESTINATION_HEADER_BUTTON_HEIGHT_BASE = 76;

	// Token: 0x040044F9 RID: 17657
	private const int WORLDS_SCROLL_PANEL_HEIGHT_CLUSTER = 436;

	// Token: 0x040044FA RID: 17658
	private const int WORLDS_SCROLL_PANEL_HEIGHT_BASE = 524;

	// Token: 0x040044FB RID: 17659
	[SerializeField]
	private NewGameSettingsPanel newGameSettingsPanel;

	// Token: 0x040044FC RID: 17660
	[MyCmpReq]
	private DestinationSelectPanel destinationMapPanel;

	// Token: 0x040044FD RID: 17661
	[SerializeField]
	private MixingContentPanel mixingPanel;

	// Token: 0x040044FE RID: 17662
	private KRandom random;

	// Token: 0x040044FF RID: 17663
	private bool isEditingCoordinate;
}
