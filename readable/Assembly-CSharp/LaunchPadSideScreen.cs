using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000E48 RID: 3656
public class LaunchPadSideScreen : SideScreenContent
{
	// Token: 0x060073E1 RID: 29665 RVA: 0x002C3F39 File Offset: 0x002C2139
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.startNewRocketbutton.onClick += this.ClickStartNewRocket;
		this.devAutoRocketButton.onClick += this.ClickAutoRocket;
	}

	// Token: 0x060073E2 RID: 29666 RVA: 0x002C3F6F File Offset: 0x002C216F
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (!show)
		{
			DetailsScreen.Instance.ClearSecondarySideScreen();
		}
	}

	// Token: 0x060073E3 RID: 29667 RVA: 0x002C3F85 File Offset: 0x002C2185
	public override int GetSideScreenSortOrder()
	{
		return 100;
	}

	// Token: 0x060073E4 RID: 29668 RVA: 0x002C3F89 File Offset: 0x002C2189
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<LaunchPad>() != null;
	}

	// Token: 0x060073E5 RID: 29669 RVA: 0x002C3F98 File Offset: 0x002C2198
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		if (this.refreshEventHandle != -1)
		{
			this.selectedPad.Unsubscribe(this.refreshEventHandle);
		}
		this.selectedPad = new_target.GetComponent<LaunchPad>();
		if (this.selectedPad == null)
		{
			global::Debug.LogError("The gameObject received does not contain a LaunchPad component");
			return;
		}
		this.refreshEventHandle = this.selectedPad.Subscribe(-887025858, new Action<object>(this.RefreshWaitingToLandList));
		this.RefreshRocketButton();
		this.RefreshWaitingToLandList(null);
	}

	// Token: 0x060073E6 RID: 29670 RVA: 0x002C4028 File Offset: 0x002C2228
	private void RefreshWaitingToLandList(object data = null)
	{
		for (int i = this.waitingToLandRows.Count - 1; i >= 0; i--)
		{
			Util.KDestroyGameObject(this.waitingToLandRows[i]);
		}
		this.waitingToLandRows.Clear();
		this.nothingWaitingRow.SetActive(true);
		AxialI myWorldLocation = this.selectedPad.GetMyWorldLocation();
		foreach (ClusterGridEntity clusterGridEntity in ClusterGrid.Instance.GetEntitiesInRange(myWorldLocation, 1))
		{
			Clustercraft craft = clusterGridEntity as Clustercraft;
			if (!(craft == null) && craft.Status == Clustercraft.CraftStatus.InFlight && (!craft.IsFlightInProgress() || !(craft.Destination != myWorldLocation)))
			{
				GameObject gameObject = Util.KInstantiateUI(this.landableRocketRowPrefab, this.landableRowContainer, true);
				gameObject.GetComponentInChildren<LocText>().text = craft.Name;
				this.waitingToLandRows.Add(gameObject);
				KButton componentInChildren = gameObject.GetComponentInChildren<KButton>();
				componentInChildren.GetComponentInChildren<LocText>().SetText((craft.ModuleInterface.GetClusterDestinationSelector().GetDestinationPad() == this.selectedPad) ? UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.CANCEL_LAND_BUTTON : UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAND_BUTTON);
				string simpleTooltip;
				componentInChildren.isInteractable = (craft.CanLandAtPad(this.selectedPad, out simpleTooltip) != Clustercraft.PadLandingStatus.CanNeverLand);
				if (!componentInChildren.isInteractable)
				{
					componentInChildren.GetComponent<ToolTip>().SetSimpleTooltip(simpleTooltip);
				}
				else
				{
					componentInChildren.GetComponent<ToolTip>().ClearMultiStringTooltip();
				}
				componentInChildren.onClick += delegate()
				{
					if (craft.ModuleInterface.GetClusterDestinationSelector().GetDestinationPad() == this.selectedPad)
					{
						craft.GetComponent<ClusterDestinationSelector>().SetDestination(craft.Location);
					}
					else
					{
						craft.LandAtPad(this.selectedPad);
					}
					this.RefreshWaitingToLandList(null);
				};
				this.nothingWaitingRow.SetActive(false);
			}
		}
	}

	// Token: 0x060073E7 RID: 29671 RVA: 0x002C4228 File Offset: 0x002C2428
	private void ClickStartNewRocket()
	{
		((SelectModuleSideScreen)DetailsScreen.Instance.SetSecondarySideScreen(this.changeModuleSideScreen, UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.CHANGEMODULEPANEL)).SetLaunchPad(this.selectedPad);
	}

	// Token: 0x060073E8 RID: 29672 RVA: 0x002C4254 File Offset: 0x002C2454
	private void RefreshRocketButton()
	{
		bool isOperational = this.selectedPad.GetComponent<Operational>().IsOperational;
		this.startNewRocketbutton.isInteractable = (this.selectedPad.LandedRocket == null && isOperational);
		if (!isOperational)
		{
			this.startNewRocketbutton.GetComponent<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.DROPDOWN_TOOLTIP_PAD_DISABLED);
		}
		else
		{
			this.startNewRocketbutton.GetComponent<ToolTip>().ClearMultiStringTooltip();
		}
		this.devAutoRocketButton.isInteractable = (this.selectedPad.LandedRocket == null);
		this.devAutoRocketButton.gameObject.SetActive(DebugHandler.InstantBuildMode);
	}

	// Token: 0x060073E9 RID: 29673 RVA: 0x002C42F0 File Offset: 0x002C24F0
	private void ClickAutoRocket()
	{
		AutoRocketUtility.StartAutoRocket(this.selectedPad);
	}

	// Token: 0x04005021 RID: 20513
	public GameObject content;

	// Token: 0x04005022 RID: 20514
	private LaunchPad selectedPad;

	// Token: 0x04005023 RID: 20515
	public LocText DescriptionText;

	// Token: 0x04005024 RID: 20516
	public GameObject landableRocketRowPrefab;

	// Token: 0x04005025 RID: 20517
	public GameObject newRocketPanel;

	// Token: 0x04005026 RID: 20518
	public KButton startNewRocketbutton;

	// Token: 0x04005027 RID: 20519
	public KButton devAutoRocketButton;

	// Token: 0x04005028 RID: 20520
	public GameObject landableRowContainer;

	// Token: 0x04005029 RID: 20521
	public GameObject nothingWaitingRow;

	// Token: 0x0400502A RID: 20522
	public KScreen changeModuleSideScreen;

	// Token: 0x0400502B RID: 20523
	private int refreshEventHandle = -1;

	// Token: 0x0400502C RID: 20524
	public List<GameObject> waitingToLandRows = new List<GameObject>();
}
