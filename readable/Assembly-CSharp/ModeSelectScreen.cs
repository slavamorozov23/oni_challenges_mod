using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DB2 RID: 3506
public class ModeSelectScreen : NewGameFlowScreen
{
	// Token: 0x06006D6D RID: 28013 RVA: 0x002974DF File Offset: 0x002956DF
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.LoadWorldAndClusterData();
	}

	// Token: 0x06006D6E RID: 28014 RVA: 0x002974F0 File Offset: 0x002956F0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		HierarchyReferences component = this.survivalButton.GetComponent<HierarchyReferences>();
		this.survivalButtonHeader = component.GetReference<RectTransform>("HeaderBackground").GetComponent<Image>();
		this.survivalButtonSelectionFrame = component.GetReference<RectTransform>("SelectionFrame").GetComponent<Image>();
		MultiToggle multiToggle = this.survivalButton;
		multiToggle.onEnter = (System.Action)Delegate.Combine(multiToggle.onEnter, new System.Action(this.OnHoverEnterSurvival));
		MultiToggle multiToggle2 = this.survivalButton;
		multiToggle2.onExit = (System.Action)Delegate.Combine(multiToggle2.onExit, new System.Action(this.OnHoverExitSurvival));
		MultiToggle multiToggle3 = this.survivalButton;
		multiToggle3.onClick = (System.Action)Delegate.Combine(multiToggle3.onClick, new System.Action(this.OnClickSurvival));
		HierarchyReferences component2 = this.nosweatButton.GetComponent<HierarchyReferences>();
		this.nosweatButtonHeader = component2.GetReference<RectTransform>("HeaderBackground").GetComponent<Image>();
		this.nosweatButtonSelectionFrame = component2.GetReference<RectTransform>("SelectionFrame").GetComponent<Image>();
		MultiToggle multiToggle4 = this.nosweatButton;
		multiToggle4.onEnter = (System.Action)Delegate.Combine(multiToggle4.onEnter, new System.Action(this.OnHoverEnterNosweat));
		MultiToggle multiToggle5 = this.nosweatButton;
		multiToggle5.onExit = (System.Action)Delegate.Combine(multiToggle5.onExit, new System.Action(this.OnHoverExitNosweat));
		MultiToggle multiToggle6 = this.nosweatButton;
		multiToggle6.onClick = (System.Action)Delegate.Combine(multiToggle6.onClick, new System.Action(this.OnClickNosweat));
		this.closeButton.onClick += base.NavigateBackward;
	}

	// Token: 0x06006D6F RID: 28015 RVA: 0x00297674 File Offset: 0x00295874
	private void OnHoverEnterSurvival()
	{
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Mouseover", false));
		this.survivalButtonSelectionFrame.SetAlpha(1f);
		this.survivalButtonHeader.color = new Color(0.7019608f, 0.3647059f, 0.53333336f, 1f);
		this.descriptionArea.text = UI.FRONTEND.MODESELECTSCREEN.SURVIVAL_DESC;
	}

	// Token: 0x06006D70 RID: 28016 RVA: 0x002976DC File Offset: 0x002958DC
	private void OnHoverExitSurvival()
	{
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Mouseover", false));
		this.survivalButtonSelectionFrame.SetAlpha(0f);
		this.survivalButtonHeader.color = new Color(0.30980393f, 0.34117648f, 0.38431373f, 1f);
		this.descriptionArea.text = UI.FRONTEND.MODESELECTSCREEN.BLANK_DESC;
	}

	// Token: 0x06006D71 RID: 28017 RVA: 0x00297742 File Offset: 0x00295942
	private void OnClickSurvival()
	{
		this.Deactivate();
		CustomGameSettings.Instance.SetSurvivalDefaults();
		base.NavigateForward();
	}

	// Token: 0x06006D72 RID: 28018 RVA: 0x0029775A File Offset: 0x0029595A
	private void LoadWorldAndClusterData()
	{
		if (ModeSelectScreen.dataLoaded)
		{
			return;
		}
		CustomGameSettings.Instance.LoadClusters();
		Global.Instance.modManager.Report(base.gameObject);
		ModeSelectScreen.dataLoaded = true;
	}

	// Token: 0x06006D73 RID: 28019 RVA: 0x0029778C File Offset: 0x0029598C
	private void OnHoverEnterNosweat()
	{
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Mouseover", false));
		this.nosweatButtonSelectionFrame.SetAlpha(1f);
		this.nosweatButtonHeader.color = new Color(0.7019608f, 0.3647059f, 0.53333336f, 1f);
		this.descriptionArea.text = UI.FRONTEND.MODESELECTSCREEN.NOSWEAT_DESC;
	}

	// Token: 0x06006D74 RID: 28020 RVA: 0x002977F4 File Offset: 0x002959F4
	private void OnHoverExitNosweat()
	{
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Mouseover", false));
		this.nosweatButtonSelectionFrame.SetAlpha(0f);
		this.nosweatButtonHeader.color = new Color(0.30980393f, 0.34117648f, 0.38431373f, 1f);
		this.descriptionArea.text = UI.FRONTEND.MODESELECTSCREEN.BLANK_DESC;
	}

	// Token: 0x06006D75 RID: 28021 RVA: 0x0029785A File Offset: 0x00295A5A
	private void OnClickNosweat()
	{
		this.Deactivate();
		CustomGameSettings.Instance.SetNosweatDefaults();
		base.NavigateForward();
	}

	// Token: 0x04004ABA RID: 19130
	[SerializeField]
	private MultiToggle nosweatButton;

	// Token: 0x04004ABB RID: 19131
	private Image nosweatButtonHeader;

	// Token: 0x04004ABC RID: 19132
	private Image nosweatButtonSelectionFrame;

	// Token: 0x04004ABD RID: 19133
	[SerializeField]
	private MultiToggle survivalButton;

	// Token: 0x04004ABE RID: 19134
	private Image survivalButtonHeader;

	// Token: 0x04004ABF RID: 19135
	private Image survivalButtonSelectionFrame;

	// Token: 0x04004AC0 RID: 19136
	[SerializeField]
	private LocText descriptionArea;

	// Token: 0x04004AC1 RID: 19137
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04004AC2 RID: 19138
	[SerializeField]
	private KBatchedAnimController nosweatAnim;

	// Token: 0x04004AC3 RID: 19139
	[SerializeField]
	private KBatchedAnimController survivalAnim;

	// Token: 0x04004AC4 RID: 19140
	private static bool dataLoaded;
}
