using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000CFD RID: 3325
public class DetailTabHeader : KMonoBehaviour
{
	// Token: 0x17000780 RID: 1920
	// (get) Token: 0x060066BF RID: 26303 RVA: 0x0026B229 File Offset: 0x00269429
	public TargetPanel ActivePanel
	{
		get
		{
			if (this.tabPanels.ContainsKey(this.selectedTabID))
			{
				return this.tabPanels[this.selectedTabID];
			}
			return null;
		}
	}

	// Token: 0x060066C0 RID: 26304 RVA: 0x0026B254 File Offset: 0x00269454
	public void Init()
	{
		this.detailsScreen = DetailsScreen.Instance;
		this.MakeTab("SIMPLEINFO", UI.DETAILTABS.SIMPLEINFO.NAME, Assets.GetSprite("icon_display_screen_status"), UI.DETAILTABS.SIMPLEINFO.TOOLTIP, this.simpleInfoScreen);
		this.MakeTab("PERSONALITY", UI.DETAILTABS.PERSONALITY.NAME, Assets.GetSprite("icon_display_screen_bio"), UI.DETAILTABS.PERSONALITY.TOOLTIP, this.minionPersonalityPanel);
		this.MakeTab("BUILDINGCHORES", UI.DETAILTABS.BUILDING_CHORES.NAME, Assets.GetSprite("icon_display_screen_errands"), UI.DETAILTABS.BUILDING_CHORES.TOOLTIP, this.buildingInfoPanel);
		this.MakeTab("DETAILS", UI.DETAILTABS.DETAILS.NAME, Assets.GetSprite("icon_display_screen_properties"), UI.DETAILTABS.DETAILS.TOOLTIP, this.additionalDetailsPanel);
		this.ChangeToDefaultTab();
	}

	// Token: 0x060066C1 RID: 26305 RVA: 0x0026B342 File Offset: 0x00269542
	private void MakeTabContents(GameObject panelToActivate)
	{
	}

	// Token: 0x060066C2 RID: 26306 RVA: 0x0026B344 File Offset: 0x00269544
	private void MakeTab(string id, string label, Sprite sprite, string tooltip, GameObject panelToActivate)
	{
		GameObject gameObject = Util.KInstantiateUI(this.tabPrefab, this.tabContainer, true);
		gameObject.name = "tab: " + id;
		gameObject.GetComponent<ToolTip>().SetSimpleTooltip(tooltip);
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("icon").sprite = sprite;
		component.GetReference<LocText>("label").text = label;
		MultiToggle component2 = gameObject.GetComponent<MultiToggle>();
		GameObject gameObject2 = Util.KInstantiateUI(panelToActivate, this.panelContainer.gameObject, true);
		TargetPanel component3 = gameObject2.GetComponent<TargetPanel>();
		component3.SetTarget(this.detailsScreen.target);
		this.tabPanels.Add(id, component3);
		string targetTab = id;
		MultiToggle multiToggle = component2;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			this.ChangeTab(targetTab);
		}));
		this.tabs.Add(id, component2);
		gameObject2.SetActive(false);
	}

	// Token: 0x060066C3 RID: 26307 RVA: 0x0026B430 File Offset: 0x00269630
	private void ChangeTab(string id)
	{
		this.selectedTabID = id;
		foreach (KeyValuePair<string, MultiToggle> keyValuePair in this.tabs)
		{
			keyValuePair.Value.ChangeState((keyValuePair.Key == this.selectedTabID) ? 1 : 0);
		}
		foreach (KeyValuePair<string, TargetPanel> keyValuePair2 in this.tabPanels)
		{
			if (keyValuePair2.Key == id)
			{
				keyValuePair2.Value.gameObject.SetActive(true);
				keyValuePair2.Value.SetTarget(this.detailsScreen.target);
			}
			else
			{
				keyValuePair2.Value.SetTarget(null);
				keyValuePair2.Value.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x060066C4 RID: 26308 RVA: 0x0026B53C File Offset: 0x0026973C
	private void ChangeToDefaultTab()
	{
		this.ChangeTab("SIMPLEINFO");
	}

	// Token: 0x060066C5 RID: 26309 RVA: 0x0026B54C File Offset: 0x0026974C
	public void RefreshTabDisplayForTarget(GameObject target)
	{
		foreach (KeyValuePair<string, TargetPanel> keyValuePair in this.tabPanels)
		{
			this.tabs[keyValuePair.Key].gameObject.SetActive(keyValuePair.Value.IsValidForTarget(target));
		}
		if (this.tabPanels[this.selectedTabID].IsValidForTarget(target))
		{
			this.ChangeTab(this.selectedTabID);
			return;
		}
		this.ChangeToDefaultTab();
	}

	// Token: 0x04004643 RID: 17987
	private Dictionary<string, MultiToggle> tabs = new Dictionary<string, MultiToggle>();

	// Token: 0x04004644 RID: 17988
	private string selectedTabID;

	// Token: 0x04004645 RID: 17989
	[SerializeField]
	private GameObject tabPrefab;

	// Token: 0x04004646 RID: 17990
	[SerializeField]
	private GameObject tabContainer;

	// Token: 0x04004647 RID: 17991
	[SerializeField]
	private GameObject panelContainer;

	// Token: 0x04004648 RID: 17992
	[Header("Screen Prefabs")]
	[SerializeField]
	private GameObject simpleInfoScreen;

	// Token: 0x04004649 RID: 17993
	[SerializeField]
	private GameObject minionPersonalityPanel;

	// Token: 0x0400464A RID: 17994
	[SerializeField]
	private GameObject buildingInfoPanel;

	// Token: 0x0400464B RID: 17995
	[SerializeField]
	private GameObject additionalDetailsPanel;

	// Token: 0x0400464C RID: 17996
	[SerializeField]
	private GameObject cosmeticsPanel;

	// Token: 0x0400464D RID: 17997
	[SerializeField]
	private GameObject materialPanel;

	// Token: 0x0400464E RID: 17998
	private DetailsScreen detailsScreen;

	// Token: 0x0400464F RID: 17999
	private Dictionary<string, TargetPanel> tabPanels = new Dictionary<string, TargetPanel>();
}
