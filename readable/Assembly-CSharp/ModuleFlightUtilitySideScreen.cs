using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E53 RID: 3667
public class ModuleFlightUtilitySideScreen : SideScreenContent
{
	// Token: 0x17000805 RID: 2053
	// (get) Token: 0x06007444 RID: 29764 RVA: 0x002C60D1 File Offset: 0x002C42D1
	private CraftModuleInterface craftModuleInterface
	{
		get
		{
			return this.targetCraft.GetComponent<CraftModuleInterface>();
		}
	}

	// Token: 0x06007445 RID: 29765 RVA: 0x002C60DE File Offset: 0x002C42DE
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		base.ConsumeMouseScroll = true;
	}

	// Token: 0x06007446 RID: 29766 RVA: 0x002C60EE File Offset: 0x002C42EE
	public override float GetSortKey()
	{
		return 21f;
	}

	// Token: 0x06007447 RID: 29767 RVA: 0x002C60F8 File Offset: 0x002C42F8
	public override bool IsValidForTarget(GameObject target)
	{
		if (target.GetComponent<Clustercraft>() != null && this.HasFlightUtilityModule(target.GetComponent<CraftModuleInterface>()))
		{
			return true;
		}
		RocketControlStation component = target.GetComponent<RocketControlStation>();
		return component != null && this.HasFlightUtilityModule(component.GetMyWorld().GetComponent<Clustercraft>().ModuleInterface);
	}

	// Token: 0x06007448 RID: 29768 RVA: 0x002C614C File Offset: 0x002C434C
	private bool HasFlightUtilityModule(CraftModuleInterface craftModuleInterface)
	{
		using (IEnumerator<Ref<RocketModuleCluster>> enumerator = craftModuleInterface.ClusterModules.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Get().GetSMI<IEmptyableCargo>() != null)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06007449 RID: 29769 RVA: 0x002C61A4 File Offset: 0x002C43A4
	public override void SetTarget(GameObject target)
	{
		if (target != null)
		{
			foreach (int id in this.refreshHandle)
			{
				target.Unsubscribe(id);
			}
			this.refreshHandle.Clear();
		}
		base.SetTarget(target);
		this.targetCraft = target.GetComponent<Clustercraft>();
		if (this.targetCraft == null && target.GetComponent<RocketControlStation>() != null)
		{
			this.targetCraft = target.GetMyWorld().GetComponent<Clustercraft>();
		}
		this.refreshHandle.Add(this.targetCraft.gameObject.Subscribe(-1298331547, new Action<object>(this.RefreshAll)));
		this.refreshHandle.Add(this.targetCraft.gameObject.Subscribe(1792516731, new Action<object>(this.RefreshAll)));
		this.BuildModules();
	}

	// Token: 0x0600744A RID: 29770 RVA: 0x002C62AC File Offset: 0x002C44AC
	private void ClearModules()
	{
		foreach (KeyValuePair<IEmptyableCargo, HierarchyReferences> keyValuePair in this.modulePanels)
		{
			Util.KDestroyGameObject(keyValuePair.Value.gameObject);
		}
		this.modulePanels.Clear();
	}

	// Token: 0x0600744B RID: 29771 RVA: 0x002C6314 File Offset: 0x002C4514
	private void BuildModules()
	{
		this.ClearModules();
		foreach (Ref<RocketModuleCluster> @ref in this.craftModuleInterface.ClusterModules)
		{
			IEmptyableCargo smi = @ref.Get().GetSMI<IEmptyableCargo>();
			if (smi != null)
			{
				HierarchyReferences value = Util.KInstantiateUI<HierarchyReferences>(this.modulePanelPrefab, this.moduleContentContainer, true);
				this.modulePanels.Add(smi, value);
				this.RefreshModulePanel(smi);
			}
		}
		this.scrollRectLayout.preferredHeight = (this.scrollRectLayout.minHeight = Mathf.Min((float)this.modulePanels.Count, 2.5f) * this.modulePanelPrefab.GetComponent<RectTransform>().rect.height);
	}

	// Token: 0x0600744C RID: 29772 RVA: 0x002C63E4 File Offset: 0x002C45E4
	private void RefreshAll(object data = null)
	{
		this.BuildModules();
	}

	// Token: 0x0600744D RID: 29773 RVA: 0x002C63EC File Offset: 0x002C45EC
	private void RefreshModulePanel(IEmptyableCargo module)
	{
		HierarchyReferences hierarchyReferences = this.modulePanels[module];
		hierarchyReferences.GetReference<Image>("icon").sprite = Def.GetUISprite(module.master.gameObject, "ui", false).first;
		hierarchyReferences.GetReference<RectTransform>("targetButtons").gameObject.SetActive(module.CanTargetClusterGridEntities);
		if (module.CanTargetClusterGridEntities)
		{
			KButton reference = hierarchyReferences.GetReference<KButton>("selectTargetButton");
			reference.onClick += delegate()
			{
				ClusterMapScreen.Instance.ShowInSelectDestinationMode(module.master.GetComponent<ClusterDestinationSelector>());
			};
			KButton reference2 = hierarchyReferences.GetReference<KButton>("clearTargetButton");
			reference2.GetComponentInChildren<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.MODULEFLIGHTUTILITYSIDESCREEN.CLEAR_TARGET_BUTTON_TOOLTIP);
			reference2.onClick += delegate()
			{
				module.master.GetComponent<EntityClusterDestinationSelector>().SetDestination(AxialI.INVALID);
				this.RefreshModulePanel(module);
			};
			if (module.master.GetComponent<EntityClusterDestinationSelector>().GetClusterEntityTarget() != null)
			{
				reference.GetComponentInChildren<LocText>().text = (module as StateMachine.Instance).GetMaster().GetComponent<EntityClusterDestinationSelector>().GetClusterEntityTarget().GetProperName();
				reference.isInteractable = false;
			}
			else
			{
				reference.GetComponentInChildren<LocText>().text = UI.UISIDESCREENS.MODULEFLIGHTUTILITYSIDESCREEN.SELECT_TARGET_BUTTON;
				reference.isInteractable = true;
			}
		}
		KButton reference3 = hierarchyReferences.GetReference<KButton>("button");
		reference3.isInteractable = module.CanEmptyCargo();
		reference3.GetComponentInChildren<LocText>().text = module.GetButtonText;
		reference3.GetComponentInChildren<ToolTip>().SetSimpleTooltip(module.GetButtonToolip);
		reference3.ClearOnClick();
		reference3.onClick += module.EmptyCargo;
		KButton reference4 = hierarchyReferences.GetReference<KButton>("repeatButton");
		if (module.CanAutoDeploy)
		{
			this.StyleRepeatButton(module);
			reference4.ClearOnClick();
			reference4.onClick += delegate()
			{
				this.OnRepeatClicked(module);
			};
			reference4.gameObject.SetActive(true);
		}
		else
		{
			reference4.gameObject.SetActive(false);
		}
		DropDown reference5 = hierarchyReferences.GetReference<DropDown>("dropDown");
		reference5.targetDropDownContainer = GameScreenManager.Instance.ssOverlayCanvas;
		reference5.Close();
		CrewPortrait reference6 = hierarchyReferences.GetReference<CrewPortrait>("selectedPortrait");
		WorldContainer component = (module as StateMachine.Instance).GetMaster().GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<WorldContainer>();
		if (component != null && module.ChooseDuplicant)
		{
			if (module.ChosenDuplicant != null && module.ChosenDuplicant.HasTag(GameTags.Dead))
			{
				module.ChosenDuplicant = null;
			}
			int id = component.id;
			reference5.gameObject.SetActive(true);
			reference5.Initialize(Components.LiveMinionIdentities.GetWorldItems(id, false), new Action<IListableOption, object>(this.OnDuplicantEntryClick), null, new Action<DropDownEntry, object>(this.DropDownEntryRefreshAction), true, module);
			reference5.selectedLabel.text = ((module.ChosenDuplicant != null) ? this.GetDuplicantRowName(module.ChosenDuplicant) : UI.UISIDESCREENS.MODULEFLIGHTUTILITYSIDESCREEN.SELECT_DUPLICANT.ToString());
			reference6.gameObject.SetActive(true);
			reference6.SetIdentityObject(module.ChosenDuplicant, false);
			reference5.openButton.isInteractable = !module.ModuleDeployed;
		}
		else
		{
			reference5.gameObject.SetActive(false);
			reference6.gameObject.SetActive(false);
		}
		hierarchyReferences.GetReference<LocText>("label").SetText(module.master.gameObject.GetProperName());
	}

	// Token: 0x0600744E RID: 29774 RVA: 0x002C679C File Offset: 0x002C499C
	private string GetDuplicantRowName(MinionIdentity minion)
	{
		MinionResume component = minion.GetComponent<MinionResume>();
		if (component != null && component.HasPerk(Db.Get().SkillPerks.CanUseRocketControlStation))
		{
			return string.Format(UI.UISIDESCREENS.MODULEFLIGHTUTILITYSIDESCREEN.PILOT_FMT, minion.GetProperName());
		}
		return minion.GetProperName();
	}

	// Token: 0x0600744F RID: 29775 RVA: 0x002C67EC File Offset: 0x002C49EC
	private void OnRepeatClicked(IEmptyableCargo module)
	{
		module.AutoDeploy = !module.AutoDeploy;
		this.StyleRepeatButton(module);
	}

	// Token: 0x06007450 RID: 29776 RVA: 0x002C6804 File Offset: 0x002C4A04
	private void OnDuplicantEntryClick(IListableOption option, object data)
	{
		MinionIdentity chosenDuplicant = (MinionIdentity)option;
		IEmptyableCargo emptyableCargo = (IEmptyableCargo)data;
		emptyableCargo.ChosenDuplicant = chosenDuplicant;
		HierarchyReferences hierarchyReferences = this.modulePanels[emptyableCargo];
		hierarchyReferences.GetReference<DropDown>("dropDown").selectedLabel.text = ((emptyableCargo.ChosenDuplicant != null) ? this.GetDuplicantRowName(emptyableCargo.ChosenDuplicant) : UI.UISIDESCREENS.MODULEFLIGHTUTILITYSIDESCREEN.SELECT_DUPLICANT.ToString());
		hierarchyReferences.GetReference<CrewPortrait>("selectedPortrait").SetIdentityObject(emptyableCargo.ChosenDuplicant, false);
		this.RefreshAll(null);
	}

	// Token: 0x06007451 RID: 29777 RVA: 0x002C688C File Offset: 0x002C4A8C
	private void DropDownEntryRefreshAction(DropDownEntry entry, object targetData)
	{
		MinionIdentity minionIdentity = (MinionIdentity)entry.entryData;
		entry.label.text = this.GetDuplicantRowName(minionIdentity);
		entry.portrait.SetIdentityObject(minionIdentity, false);
		bool flag = false;
		foreach (Ref<RocketModuleCluster> @ref in this.targetCraft.ModuleInterface.ClusterModules)
		{
			RocketModuleCluster rocketModuleCluster = @ref.Get();
			if (!(rocketModuleCluster == null))
			{
				IEmptyableCargo smi = rocketModuleCluster.GetSMI<IEmptyableCargo>();
				if (smi != null && !(((IEmptyableCargo)targetData).ChosenDuplicant == minionIdentity))
				{
					flag = (flag || smi.ChosenDuplicant == minionIdentity);
				}
			}
		}
		entry.button.isInteractable = !flag;
	}

	// Token: 0x06007452 RID: 29778 RVA: 0x002C695C File Offset: 0x002C4B5C
	private void StyleRepeatButton(IEmptyableCargo module)
	{
		KButton reference = this.modulePanels[module].GetReference<KButton>("repeatButton");
		reference.bgImage.colorStyleSetting = (module.AutoDeploy ? this.repeatOn : this.repeatOff);
		reference.bgImage.ApplyColorStyleSetting();
	}

	// Token: 0x04005068 RID: 20584
	private Clustercraft targetCraft;

	// Token: 0x04005069 RID: 20585
	public GameObject moduleContentContainer;

	// Token: 0x0400506A RID: 20586
	public GameObject modulePanelPrefab;

	// Token: 0x0400506B RID: 20587
	public ColorStyleSetting repeatOff;

	// Token: 0x0400506C RID: 20588
	public ColorStyleSetting repeatOn;

	// Token: 0x0400506D RID: 20589
	private Dictionary<IEmptyableCargo, HierarchyReferences> modulePanels = new Dictionary<IEmptyableCargo, HierarchyReferences>();

	// Token: 0x0400506E RID: 20590
	[SerializeField]
	private LayoutElement scrollRectLayout;

	// Token: 0x0400506F RID: 20591
	private List<int> refreshHandle = new List<int>();
}
