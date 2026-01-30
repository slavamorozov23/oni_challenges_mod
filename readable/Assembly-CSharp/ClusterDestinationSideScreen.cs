using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E26 RID: 3622
public class ClusterDestinationSideScreen : SideScreenContent
{
	// Token: 0x170007F4 RID: 2036
	// (get) Token: 0x060072E9 RID: 29417 RVA: 0x002BDC3F File Offset: 0x002BBE3F
	// (set) Token: 0x060072EA RID: 29418 RVA: 0x002BDC47 File Offset: 0x002BBE47
	private ClusterDestinationSelector targetSelector { get; set; }

	// Token: 0x170007F5 RID: 2037
	// (get) Token: 0x060072EB RID: 29419 RVA: 0x002BDC50 File Offset: 0x002BBE50
	// (set) Token: 0x060072EC RID: 29420 RVA: 0x002BDC58 File Offset: 0x002BBE58
	private RocketClusterDestinationSelector targetRocketSelector { get; set; }

	// Token: 0x060072ED RID: 29421 RVA: 0x002BDC61 File Offset: 0x002BBE61
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.CheckShouldShowTopTitle = (() => false);
	}

	// Token: 0x060072EE RID: 29422 RVA: 0x002BDC90 File Offset: 0x002BBE90
	protected override void OnSpawn()
	{
		this.changeDestinationButton.onClick += this.OnClickChangeDestination;
		this.clearDestinationButton.onClick += this.OnClickClearDestination;
		this.launchPadDropDown.targetDropDownContainer = GameScreenManager.Instance.ssOverlayCanvas;
		this.launchPadDropDown.CustomizeEmptyRow(UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.FIRSTAVAILABLE, null);
		this.repeatButton.onClick += this.OnRepeatClicked;
	}

	// Token: 0x060072EF RID: 29423 RVA: 0x002BDD0D File Offset: 0x002BBF0D
	public override int GetSideScreenSortOrder()
	{
		return 103;
	}

	// Token: 0x060072F0 RID: 29424 RVA: 0x002BDD14 File Offset: 0x002BBF14
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			this.Refresh(null);
			this.m_refreshHandle = this.targetSelector.Subscribe(543433792, delegate(object data)
			{
				this.Refresh(null);
			});
			this.m_refreshOnCancelHandle = this.targetSelector.Subscribe(94158097, delegate(object data)
			{
				this.Refresh(null);
			});
			return;
		}
		if (this.m_refreshHandle != -1)
		{
			this.targetSelector.Unsubscribe(this.m_refreshHandle);
			this.m_refreshHandle = -1;
			this.launchPadDropDown.Close();
		}
		if (this.m_refreshOnCancelHandle != -1)
		{
			this.targetSelector.Unsubscribe(this.m_refreshOnCancelHandle);
			this.m_refreshOnCancelHandle = -1;
			this.launchPadDropDown.Close();
		}
	}

	// Token: 0x060072F1 RID: 29425 RVA: 0x002BDDD0 File Offset: 0x002BBFD0
	public override bool IsValidForTarget(GameObject target)
	{
		ClusterDestinationSelector component = target.GetComponent<ClusterDestinationSelector>();
		bool flag = component != null && component.assignable;
		bool flag2 = target.GetComponent<RocketModuleCluster>() != null && target.GetComponent<RocketModuleCluster>().GetComponent<PassengerRocketModule>() != null;
		bool flag3 = target.GetComponent<RocketModuleCluster>() != null && target.GetComponent<RocketModuleCluster>().GetComponent<RoboPilotModule>() != null;
		if (flag2 || flag3)
		{
			return true;
		}
		bool flag4 = target.GetComponent<RocketControlStation>() != null && target.GetComponent<RocketControlStation>().GetMyWorld().GetComponent<Clustercraft>().Status != Clustercraft.CraftStatus.Launching;
		return flag || flag4;
	}

	// Token: 0x060072F2 RID: 29426 RVA: 0x002BDE74 File Offset: 0x002BC074
	public override void SetTarget(GameObject target)
	{
		this.targetSelector = target.GetComponent<ClusterDestinationSelector>();
		if (this.targetSelector == null)
		{
			if (target.GetComponent<RocketModuleCluster>() != null)
			{
				this.targetSelector = target.GetComponent<RocketModuleCluster>().CraftInterface.GetClusterDestinationSelector();
			}
			else if (target.GetComponent<RocketControlStation>() != null)
			{
				this.targetSelector = target.GetMyWorld().GetComponent<Clustercraft>().ModuleInterface.GetClusterDestinationSelector();
			}
		}
		this.targetRocketSelector = (this.targetSelector as RocketClusterDestinationSelector);
		this.changeDestinationButtonTooltip.SetSimpleTooltip(this.targetSelector.changeTargetButtonTooltipString);
		this.clearDestinationButton.GetComponent<ToolTip>().SetSimpleTooltip(this.targetSelector.clearTargetButtonTooltipString);
	}

	// Token: 0x060072F3 RID: 29427 RVA: 0x002BDF2C File Offset: 0x002BC12C
	private void Refresh(object data = null)
	{
		EntityLayer entityLayer = EntityLayer.None;
		bool flag = ClusterMapScreen.Instance.GetMode() == ClusterMapScreen.Mode.SelectDestination;
		if (!this.targetSelector.IsAtDestination())
		{
			ClusterGridEntity clusterEntityTarget = this.targetSelector.GetClusterEntityTarget();
			if (clusterEntityTarget != null)
			{
				this.destinationImage.sprite = clusterEntityTarget.GetUISprite();
				this.destinationInfoLabel.text = GameUtil.SafeStringFormat(UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.DESTINATION_LABEL, new object[]
				{
					clusterEntityTarget.GetProperName()
				});
			}
			else
			{
				Sprite sprite;
				string text;
				string text2;
				ClusterGrid.Instance.GetLocationDescription(this.targetSelector.GetDestination(), out sprite, out text, out text2, out entityLayer);
				this.destinationImage.sprite = sprite;
				this.destinationInfoLabel.text = GameUtil.SafeStringFormat(UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.DESTINATION_LABEL, new object[]
				{
					text
				});
			}
			this.clearDestinationButton.isInteractable = !flag;
		}
		else
		{
			this.destinationImage.sprite = Assets.GetSprite("hex_unknown");
			this.destinationInfoLabel.text = GameUtil.SafeStringFormat(UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.DESTINATION_LABEL, new object[]
			{
				UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.DESTINATION_LABEL_INVALID
			});
			this.clearDestinationButton.isInteractable = false;
		}
		this.changeDestinationButtonTooltip.SetSimpleTooltip(flag ? UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.CHANGE_DESTINATION_BUTTON_SELECTING_TOOLTIP : this.targetSelector.changeTargetButtonTooltipString);
		this.changeDestinationButton.isInteractable = !flag;
		if (flag)
		{
			this.destinationInfoLabel.text = UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.DESTINATION_LABEL_SELECTING;
		}
		if (this.targetRocketSelector != null)
		{
			List<LaunchPad> launchPadsForDestination = LaunchPad.GetLaunchPadsForDestination(this.targetRocketSelector.GetDestination());
			this.landingPlatformSection.gameObject.SetActive(true);
			this.roundtripSection.gameObject.SetActive(true);
			this.launchPadDropDown.Initialize(launchPadsForDestination, new Action<IListableOption, object>(this.OnLaunchPadEntryClick), new Func<IListableOption, IListableOption, object, int>(this.PadDropDownSort), new Action<DropDownEntry, object>(this.PadDropDownEntryRefreshAction), true, this.targetRocketSelector);
			if (!this.targetRocketSelector.IsAtDestination() && launchPadsForDestination.Count > 0)
			{
				this.launchPadDropDown.openButton.isInteractable = true;
				LaunchPad destinationPad = this.targetRocketSelector.GetDestinationPad();
				if (destinationPad != null)
				{
					this.launchPadDropDown.selectedLabel.text = destinationPad.GetProperName();
					this.landingPlatformInfoLabel.SetText(GameUtil.SafeStringFormat(UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.LANDING_PLATFORM_LABEL, new object[]
					{
						destinationPad.GetProperName()
					}));
				}
				else
				{
					this.launchPadDropDown.selectedLabel.text = UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.FIRSTAVAILABLE;
					this.landingPlatformInfoLabel.SetText(GameUtil.SafeStringFormat(UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.LANDING_PLATFORM_LABEL, new object[]
					{
						UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.FIRSTAVAILABLE
					}));
				}
			}
			else
			{
				this.launchPadDropDown.selectedLabel.text = UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.FIRSTAVAILABLE;
				this.landingPlatformInfoLabel.SetText(GameUtil.SafeStringFormat(UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.LANDING_PLATFORM_LABEL, new object[]
				{
					UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.FIRSTAVAILABLE
				}));
				this.launchPadDropDown.openButton.isInteractable = false;
			}
			this.RefreshRepeatButtonLabels();
		}
		else
		{
			this.landingPlatformSection.gameObject.SetActive(false);
			this.roundtripSection.gameObject.SetActive(false);
		}
		this.hexEmptyBG.gameObject.SetActive(entityLayer == EntityLayer.POI);
	}

	// Token: 0x060072F4 RID: 29428 RVA: 0x002BE284 File Offset: 0x002BC484
	private void OnClickChangeDestination()
	{
		if (this.targetSelector.assignable)
		{
			ClusterMapScreen.Instance.ShowInSelectDestinationMode(this.targetSelector);
			AxialI myWorldLocation = this.targetSelector.GetMyWorldLocation();
			AxialI destination = this.targetSelector.GetDestination();
			AxialI randomVisibleAdjacentCellLocation = ClusterGrid.Instance.GetRandomVisibleAdjacentCellLocation(myWorldLocation, destination);
			if (randomVisibleAdjacentCellLocation != AxialI.INVALID)
			{
				ClusterMapScreen.Instance.OnHoverHex(ClusterMapScreen.Instance.GetClusterMapHexAtLocation(randomVisibleAdjacentCellLocation));
			}
		}
		this.Refresh(null);
		if (this.changeDestinationButtonTooltip.isHovering)
		{
			ToolTipScreen.Instance.ClearToolTip(this.changeDestinationButtonTooltip);
			ToolTipScreen.Instance.SetToolTip(this.changeDestinationButtonTooltip);
		}
	}

	// Token: 0x060072F5 RID: 29429 RVA: 0x002BE329 File Offset: 0x002BC529
	private void OnClickClearDestination()
	{
		this.targetSelector.SetDestination(this.targetSelector.GetMyWorldLocation());
	}

	// Token: 0x060072F6 RID: 29430 RVA: 0x002BE344 File Offset: 0x002BC544
	private void OnLaunchPadEntryClick(IListableOption option, object data)
	{
		LaunchPad destinationPad = (LaunchPad)option;
		this.targetRocketSelector.SetDestinationPad(destinationPad);
	}

	// Token: 0x060072F7 RID: 29431 RVA: 0x002BE364 File Offset: 0x002BC564
	private void PadDropDownEntryRefreshAction(DropDownEntry entry, object targetData)
	{
		LaunchPad launchPad = (LaunchPad)entry.entryData;
		Clustercraft component = this.targetRocketSelector.GetComponent<Clustercraft>();
		if (!(launchPad != null))
		{
			entry.button.isInteractable = true;
			entry.image.sprite = Assets.GetBuildingDef("LaunchPad").GetUISprite("ui", false);
			entry.tooltip.SetSimpleTooltip(UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.DROPDOWN_TOOLTIP_FIRST_AVAILABLE);
			return;
		}
		string simpleTooltip;
		if (component.CanLandAtPad(launchPad, out simpleTooltip) == Clustercraft.PadLandingStatus.CanNeverLand)
		{
			entry.button.isInteractable = false;
			entry.image.sprite = Assets.GetSprite("iconWarning");
			entry.tooltip.SetSimpleTooltip(simpleTooltip);
			return;
		}
		entry.button.isInteractable = true;
		entry.image.sprite = launchPad.GetComponent<Building>().Def.GetUISprite("ui", false);
		entry.tooltip.SetSimpleTooltip(string.Format(UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.DROPDOWN_TOOLTIP_VALID_SITE, launchPad.GetProperName()));
	}

	// Token: 0x060072F8 RID: 29432 RVA: 0x002BE463 File Offset: 0x002BC663
	private int PadDropDownSort(IListableOption a, IListableOption b, object targetData)
	{
		return 0;
	}

	// Token: 0x060072F9 RID: 29433 RVA: 0x002BE466 File Offset: 0x002BC666
	private void OnRepeatClicked()
	{
		this.targetRocketSelector.Repeat = !this.targetRocketSelector.Repeat;
		this.RefreshRepeatButtonLabels();
	}

	// Token: 0x060072FA RID: 29434 RVA: 0x002BE488 File Offset: 0x002BC688
	private void RefreshRepeatButtonLabels()
	{
		this.roundTripInfoLabel.SetText(this.targetRocketSelector.Repeat ? UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.ROUNDTRIP_LABEL_ROUNDTRIP : UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.ROUNDTRIP_LABEL_ONE_WAY);
		this.roundTripButtonLabel.SetText(this.targetRocketSelector.Repeat ? UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.ROUNDTRIP_BUTTON_ONE_WAY : UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.ROUNDTRIP_BUTTON_ROUNDTRIP);
		this.roundtripButtonTooltip.SetSimpleTooltip(this.targetRocketSelector.Repeat ? UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.ROUNDTRIP_BUTTON_TOOLTIP_ONE_WAY : UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.ROUNDTRIP_BUTTON_TOOLTIP_ROUNDTRIP);
	}

	// Token: 0x04004F66 RID: 20326
	public Image hexEmptyBG;

	// Token: 0x04004F67 RID: 20327
	public Image destinationImage;

	// Token: 0x04004F68 RID: 20328
	[Header("Destination selection Section")]
	public RectTransform destinationSection;

	// Token: 0x04004F69 RID: 20329
	public LocText destinationInfoLabel;

	// Token: 0x04004F6A RID: 20330
	public KButton changeDestinationButton;

	// Token: 0x04004F6B RID: 20331
	public ToolTip changeDestinationButtonTooltip;

	// Token: 0x04004F6C RID: 20332
	public KButton clearDestinationButton;

	// Token: 0x04004F6D RID: 20333
	[Header("Landing Platform Section")]
	public RectTransform landingPlatformSection;

	// Token: 0x04004F6E RID: 20334
	public LocText landingPlatformInfoLabel;

	// Token: 0x04004F6F RID: 20335
	public DropDown launchPadDropDown;

	// Token: 0x04004F70 RID: 20336
	[Header("Round Trip Section")]
	public RectTransform roundtripSection;

	// Token: 0x04004F71 RID: 20337
	public LocText roundTripInfoLabel;

	// Token: 0x04004F72 RID: 20338
	public LocText roundTripButtonLabel;

	// Token: 0x04004F73 RID: 20339
	public KButton repeatButton;

	// Token: 0x04004F74 RID: 20340
	public ToolTip roundtripButtonTooltip;

	// Token: 0x04004F75 RID: 20341
	[Space]
	public ColorStyleSetting defaultButton;

	// Token: 0x04004F76 RID: 20342
	public ColorStyleSetting highlightButton;

	// Token: 0x04004F79 RID: 20345
	private int m_refreshHandle = -1;

	// Token: 0x04004F7A RID: 20346
	private int m_refreshOnCancelHandle = -1;
}
