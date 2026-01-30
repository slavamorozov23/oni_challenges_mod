using System;
using System.Collections.Generic;
using ProcGen;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000CB3 RID: 3251
public class ClusterCategorySelectionScreen : NewGameFlowScreen
{
	// Token: 0x06006392 RID: 25490 RVA: 0x00251218 File Offset: 0x0024F418
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.closeButton.onClick += base.NavigateBackward;
		int num = 0;
		using (Dictionary<string, ClusterLayout>.ValueCollection.Enumerator enumerator = SettingsCache.clusterLayouts.clusterCache.Values.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.clusterCategory == ClusterLayout.ClusterCategory.Special)
				{
					num++;
				}
			}
		}
		if (num > 0)
		{
			this.eventStyle.button.gameObject.SetActive(true);
			this.eventStyle.Init(this.descriptionArea, UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.EVENT_DESC, UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.EVENT_TITLE);
			MultiToggle button = this.eventStyle.button;
			button.onClick = (System.Action)Delegate.Combine(button.onClick, new System.Action(delegate()
			{
				this.OnClickOption(ClusterLayout.ClusterCategory.Special);
			}));
		}
		if (DlcManager.IsExpansion1Active())
		{
			this.classicStyle.button.gameObject.SetActive(true);
			this.classicStyle.Init(this.descriptionArea, UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.CLASSIC_DESC, UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.CLASSIC_TITLE);
			MultiToggle button2 = this.classicStyle.button;
			button2.onClick = (System.Action)Delegate.Combine(button2.onClick, new System.Action(delegate()
			{
				this.OnClickOption(ClusterLayout.ClusterCategory.SpacedOutVanillaStyle);
			}));
			this.spacedOutStyle.button.gameObject.SetActive(true);
			this.spacedOutStyle.Init(this.descriptionArea, UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.SPACEDOUT_DESC, UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.SPACEDOUT_TITLE);
			MultiToggle button3 = this.spacedOutStyle.button;
			button3.onClick = (System.Action)Delegate.Combine(button3.onClick, new System.Action(delegate()
			{
				this.OnClickOption(ClusterLayout.ClusterCategory.SpacedOutStyle);
			}));
			this.panel.sizeDelta = ((num > 0) ? new Vector2(622f, this.panel.sizeDelta.y) : new Vector2(480f, this.panel.sizeDelta.y));
			return;
		}
		this.vanillaStyle.button.gameObject.SetActive(true);
		this.vanillaStyle.Init(this.descriptionArea, UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.VANILLA_DESC, UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.VANILLA_TITLE);
		MultiToggle button4 = this.vanillaStyle.button;
		button4.onClick = (System.Action)Delegate.Combine(button4.onClick, new System.Action(delegate()
		{
			this.OnClickOption(ClusterLayout.ClusterCategory.Vanilla);
		}));
		this.panel.sizeDelta = new Vector2(480f, this.panel.sizeDelta.y);
		this.eventStyle.kanim.Play("lab_asteroid_standard", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x06006393 RID: 25491 RVA: 0x002514D4 File Offset: 0x0024F6D4
	private void OnClickOption(ClusterLayout.ClusterCategory clusterCategory)
	{
		this.Deactivate();
		DestinationSelectPanel.ChosenClusterCategorySetting = (int)clusterCategory;
		base.NavigateForward();
	}

	// Token: 0x040043AB RID: 17323
	public ClusterCategorySelectionScreen.ButtonConfig vanillaStyle;

	// Token: 0x040043AC RID: 17324
	public ClusterCategorySelectionScreen.ButtonConfig classicStyle;

	// Token: 0x040043AD RID: 17325
	public ClusterCategorySelectionScreen.ButtonConfig spacedOutStyle;

	// Token: 0x040043AE RID: 17326
	public ClusterCategorySelectionScreen.ButtonConfig eventStyle;

	// Token: 0x040043AF RID: 17327
	[SerializeField]
	private LocText descriptionArea;

	// Token: 0x040043B0 RID: 17328
	[SerializeField]
	private KButton closeButton;

	// Token: 0x040043B1 RID: 17329
	[SerializeField]
	private RectTransform panel;

	// Token: 0x02001EDC RID: 7900
	[Serializable]
	public class ButtonConfig
	{
		// Token: 0x0600B4DA RID: 46298 RVA: 0x003EC9EC File Offset: 0x003EABEC
		public void Init(LocText descriptionArea, string hoverDescriptionText, string headerText)
		{
			this.descriptionArea = descriptionArea;
			this.hoverDescriptionText = hoverDescriptionText;
			this.headerLabel.SetText(headerText);
			MultiToggle multiToggle = this.button;
			multiToggle.onEnter = (System.Action)Delegate.Combine(multiToggle.onEnter, new System.Action(this.OnHoverEnter));
			MultiToggle multiToggle2 = this.button;
			multiToggle2.onExit = (System.Action)Delegate.Combine(multiToggle2.onExit, new System.Action(this.OnHoverExit));
			HierarchyReferences component = this.button.GetComponent<HierarchyReferences>();
			this.headerImage = component.GetReference<RectTransform>("HeaderBackground").GetComponent<Image>();
			this.selectionFrame = component.GetReference<RectTransform>("SelectionFrame").GetComponent<Image>();
		}

		// Token: 0x0600B4DB RID: 46299 RVA: 0x003ECA9C File Offset: 0x003EAC9C
		private void OnHoverEnter()
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Mouseover", false));
			this.selectionFrame.SetAlpha(1f);
			this.headerImage.color = new Color(0.7019608f, 0.3647059f, 0.53333336f, 1f);
			this.descriptionArea.text = this.hoverDescriptionText;
		}

		// Token: 0x0600B4DC RID: 46300 RVA: 0x003ECB00 File Offset: 0x003EAD00
		private void OnHoverExit()
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Mouseover", false));
			this.selectionFrame.SetAlpha(0f);
			this.headerImage.color = new Color(0.30980393f, 0.34117648f, 0.38431373f, 1f);
			this.descriptionArea.text = UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.BLANK_DESC;
		}

		// Token: 0x040090DC RID: 37084
		public MultiToggle button;

		// Token: 0x040090DD RID: 37085
		public Image headerImage;

		// Token: 0x040090DE RID: 37086
		public LocText headerLabel;

		// Token: 0x040090DF RID: 37087
		public Image selectionFrame;

		// Token: 0x040090E0 RID: 37088
		public KAnimControllerBase kanim;

		// Token: 0x040090E1 RID: 37089
		private string hoverDescriptionText;

		// Token: 0x040090E2 RID: 37090
		private LocText descriptionArea;
	}
}
