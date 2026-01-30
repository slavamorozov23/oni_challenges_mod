using System;
using System.Collections;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E6E RID: 3694
public class RocketModuleSideScreen : SideScreenContent
{
	// Token: 0x0600755D RID: 30045 RVA: 0x002CCD4A File Offset: 0x002CAF4A
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		RocketModuleSideScreen.instance = this;
	}

	// Token: 0x0600755E RID: 30046 RVA: 0x002CCD58 File Offset: 0x002CAF58
	protected override void OnForcedCleanUp()
	{
		RocketModuleSideScreen.instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x0600755F RID: 30047 RVA: 0x002CCD66 File Offset: 0x002CAF66
	public override int GetSideScreenSortOrder()
	{
		return 104;
	}

	// Token: 0x06007560 RID: 30048 RVA: 0x002CCD6C File Offset: 0x002CAF6C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.addNewModuleButton.onClick += delegate()
		{
			Vector2 vector = Vector2.zero;
			if (SelectModuleSideScreen.Instance != null)
			{
				vector = SelectModuleSideScreen.Instance.mainContents.GetComponent<KScrollRect>().content.rectTransform().anchoredPosition;
			}
			this.ClickAddNew(vector.y, null);
		};
		this.removeModuleButton.onClick += this.ClickRemove;
		this.moveModuleUpButton.onClick += this.ClickSwapUp;
		this.moveModuleDownButton.onClick += this.ClickSwapDown;
		this.changeModuleButton.onClick += delegate()
		{
			Vector2 vector = Vector2.zero;
			if (SelectModuleSideScreen.Instance != null)
			{
				vector = SelectModuleSideScreen.Instance.mainContents.GetComponent<KScrollRect>().content.rectTransform().anchoredPosition;
			}
			this.ClickChangeModule(vector.y);
		};
		this.moduleNameLabel.textStyleSetting = this.nameSetting;
		this.moduleDescriptionLabel.textStyleSetting = this.descriptionSetting;
		this.moduleNameLabel.ApplySettings();
		this.moduleDescriptionLabel.ApplySettings();
	}

	// Token: 0x06007561 RID: 30049 RVA: 0x002CCE2A File Offset: 0x002CB02A
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		DetailsScreen.Instance.ClearSecondarySideScreen();
	}

	// Token: 0x06007562 RID: 30050 RVA: 0x002CCE3C File Offset: 0x002CB03C
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06007563 RID: 30051 RVA: 0x002CCE44 File Offset: 0x002CB044
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<ReorderableBuilding>() != null;
	}

	// Token: 0x06007564 RID: 30052 RVA: 0x002CCE54 File Offset: 0x002CB054
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.reorderable = new_target.GetComponent<ReorderableBuilding>();
		this.moduleIcon.sprite = Def.GetUISprite(this.reorderable.gameObject, "ui", false).first;
		this.moduleNameLabel.SetText(this.reorderable.GetProperName());
		this.moduleDescriptionLabel.SetText(this.reorderable.GetComponent<Building>().Desc);
		this.UpdateButtonStates();
	}

	// Token: 0x06007565 RID: 30053 RVA: 0x002CCEE0 File Offset: 0x002CB0E0
	public void UpdateButtonStates()
	{
		this.changeModuleButton.isInteractable = this.reorderable.CanChangeModule();
		this.changeModuleButton.GetComponent<ToolTip>().SetSimpleTooltip(this.changeModuleButton.isInteractable ? UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONCHANGEMODULE.DESC.text : UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONCHANGEMODULE.INVALID.text);
		this.addNewModuleButton.isInteractable = true;
		this.addNewModuleButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.ADDMODULE.DESC.text);
		Deconstructable component = this.reorderable.GetComponent<Deconstructable>();
		bool flag = component != null && component.IsMarkedForDeconstruction();
		this.removeModuleButton.isInteractable = (component != null && this.reorderable.CanRemoveModule());
		this.removeModuleButton.GetComponent<ToolTip>().SetSimpleTooltip(this.removeModuleButton.isInteractable ? (flag ? UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONREMOVEMODULE.DESC_CANCEL.text : UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONREMOVEMODULE.DESC.text) : UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONREMOVEMODULE.INVALID.text);
		this.removeButtonLabel.SetText(flag ? UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONREMOVEMODULE.LABEL_CANCEL : UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONREMOVEMODULE.LABEL);
		this.moveModuleDownButton.isInteractable = this.reorderable.CanSwapDown(true);
		this.moveModuleDownButton.GetComponent<ToolTip>().SetSimpleTooltip(this.moveModuleDownButton.isInteractable ? UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONSWAPMODULEDOWN.DESC.text : UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONSWAPMODULEDOWN.INVALID.text);
		this.moveModuleUpButton.isInteractable = this.reorderable.CanSwapUp(true);
		this.moveModuleUpButton.GetComponent<ToolTip>().SetSimpleTooltip(this.moveModuleUpButton.isInteractable ? UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONSWAPMODULEUP.DESC.text : UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONSWAPMODULEUP.INVALID.text);
	}

	// Token: 0x06007566 RID: 30054 RVA: 0x002CD094 File Offset: 0x002CB294
	public void ClickAddNew(float scrollViewPosition, BuildingDef autoSelectDef = null)
	{
		SelectModuleSideScreen selectModuleSideScreen = (SelectModuleSideScreen)DetailsScreen.Instance.SetSecondarySideScreen(this.changeModuleSideScreen, UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.CHANGEMODULEPANEL);
		selectModuleSideScreen.addingNewModule = true;
		selectModuleSideScreen.SetTarget(this.reorderable.gameObject);
		if (autoSelectDef != null)
		{
			selectModuleSideScreen.SelectModule(autoSelectDef);
		}
		this.ScrollToTargetPoint(scrollViewPosition);
	}

	// Token: 0x06007567 RID: 30055 RVA: 0x002CD0F0 File Offset: 0x002CB2F0
	private void ScrollToTargetPoint(float scrollViewPosition)
	{
		if (SelectModuleSideScreen.Instance != null)
		{
			SelectModuleSideScreen.Instance.mainContents.GetComponent<KScrollRect>().content.anchoredPosition = new Vector2(0f, scrollViewPosition);
			if (base.gameObject.activeInHierarchy)
			{
				base.StartCoroutine(this.DelayedScrollToTargetPoint(scrollViewPosition));
			}
		}
	}

	// Token: 0x06007568 RID: 30056 RVA: 0x002CD149 File Offset: 0x002CB349
	private IEnumerator DelayedScrollToTargetPoint(float scrollViewPosition)
	{
		if (SelectModuleSideScreen.Instance != null)
		{
			yield return SequenceUtil.WaitForEndOfFrame;
			SelectModuleSideScreen.Instance.mainContents.GetComponent<KScrollRect>().content.anchoredPosition = new Vector2(0f, scrollViewPosition);
		}
		yield break;
	}

	// Token: 0x06007569 RID: 30057 RVA: 0x002CD158 File Offset: 0x002CB358
	private void ClickRemove()
	{
		Deconstructable component = this.reorderable.GetComponent<Deconstructable>();
		if (component == null)
		{
			return;
		}
		if (component.IsMarkedForDeconstruction())
		{
			component.CancelDeconstruction();
		}
		else
		{
			this.reorderable.Trigger(-790448070, null);
		}
		this.UpdateButtonStates();
		component.Trigger(1980521255, null);
	}

	// Token: 0x0600756A RID: 30058 RVA: 0x002CD1AE File Offset: 0x002CB3AE
	private void ClickSwapUp()
	{
		this.reorderable.SwapWithAbove(true);
		this.UpdateButtonStates();
	}

	// Token: 0x0600756B RID: 30059 RVA: 0x002CD1C2 File Offset: 0x002CB3C2
	private void ClickSwapDown()
	{
		this.reorderable.SwapWithBelow(true);
		this.UpdateButtonStates();
	}

	// Token: 0x0600756C RID: 30060 RVA: 0x002CD1D6 File Offset: 0x002CB3D6
	private void ClickChangeModule(float scrollViewPosition)
	{
		SelectModuleSideScreen selectModuleSideScreen = (SelectModuleSideScreen)DetailsScreen.Instance.SetSecondarySideScreen(this.changeModuleSideScreen, UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.CHANGEMODULEPANEL);
		selectModuleSideScreen.addingNewModule = false;
		selectModuleSideScreen.SetTarget(this.reorderable.gameObject);
		this.ScrollToTargetPoint(scrollViewPosition);
	}

	// Token: 0x04005137 RID: 20791
	public static RocketModuleSideScreen instance;

	// Token: 0x04005138 RID: 20792
	private ReorderableBuilding reorderable;

	// Token: 0x04005139 RID: 20793
	public KScreen changeModuleSideScreen;

	// Token: 0x0400513A RID: 20794
	public Image moduleIcon;

	// Token: 0x0400513B RID: 20795
	[Header("Buttons")]
	public KButton addNewModuleButton;

	// Token: 0x0400513C RID: 20796
	public KButton removeModuleButton;

	// Token: 0x0400513D RID: 20797
	public KButton changeModuleButton;

	// Token: 0x0400513E RID: 20798
	public KButton moveModuleUpButton;

	// Token: 0x0400513F RID: 20799
	public KButton moveModuleDownButton;

	// Token: 0x04005140 RID: 20800
	[Header("Labels")]
	public LocText removeButtonLabel;

	// Token: 0x04005141 RID: 20801
	public LocText moduleNameLabel;

	// Token: 0x04005142 RID: 20802
	public LocText moduleDescriptionLabel;

	// Token: 0x04005143 RID: 20803
	public TextStyleSetting nameSetting;

	// Token: 0x04005144 RID: 20804
	public TextStyleSetting descriptionSetting;
}
