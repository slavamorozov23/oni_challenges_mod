using System;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000EB3 RID: 3763
public class TopLeftControlScreen : KScreen
{
	// Token: 0x0600789D RID: 30877 RVA: 0x002E61AF File Offset: 0x002E43AF
	public static void DestroyInstance()
	{
		TopLeftControlScreen.Instance = null;
	}

	// Token: 0x0600789E RID: 30878 RVA: 0x002E61B8 File Offset: 0x002E43B8
	protected override void OnActivate()
	{
		base.OnActivate();
		TopLeftControlScreen.Instance = this;
		this.RefreshName();
		KInputManager.InputChange.AddListener(new UnityAction(this.ResetToolTip));
		this.UpdateSandboxToggleState();
		MultiToggle multiToggle = this.sandboxToggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.OnClickSandboxToggle));
		MultiToggle multiToggle2 = this.kleiItemDropButton;
		multiToggle2.onClick = (System.Action)Delegate.Combine(multiToggle2.onClick, new System.Action(this.OnClickKleiItemDropButton));
		KleiItemsStatusRefresher.AddOrGetListener(this).OnRefreshUI(new System.Action(this.RefreshKleiItemDropButton));
		this.RefreshKleiItemDropButton();
		Game.Instance.Subscribe(-1948169901, delegate(object data)
		{
			this.UpdateSandboxToggleState();
		});
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.secondaryRow);
	}

	// Token: 0x0600789F RID: 30879 RVA: 0x002E6285 File Offset: 0x002E4485
	protected override void OnForcedCleanUp()
	{
		KInputManager.InputChange.RemoveListener(new UnityAction(this.ResetToolTip));
		base.OnForcedCleanUp();
	}

	// Token: 0x060078A0 RID: 30880 RVA: 0x002E62A3 File Offset: 0x002E44A3
	public void RefreshName()
	{
		if (SaveGame.Instance != null)
		{
			this.locText.text = SaveGame.Instance.BaseName;
		}
	}

	// Token: 0x060078A1 RID: 30881 RVA: 0x002E62C8 File Offset: 0x002E44C8
	public void ResetToolTip()
	{
		if (this.CheckSandboxModeLocked())
		{
			this.sandboxToggle.GetComponent<ToolTip>().SetSimpleTooltip(GameUtil.ReplaceHotkeyString(UI.SANDBOX_TOGGLE.TOOLTIP_LOCKED, global::Action.ToggleSandboxTools));
			return;
		}
		this.sandboxToggle.GetComponent<ToolTip>().SetSimpleTooltip(GameUtil.ReplaceHotkeyString(UI.SANDBOX_TOGGLE.TOOLTIP_UNLOCKED, global::Action.ToggleSandboxTools));
	}

	// Token: 0x060078A2 RID: 30882 RVA: 0x002E6328 File Offset: 0x002E4528
	public void UpdateSandboxToggleState()
	{
		if (this.CheckSandboxModeLocked())
		{
			this.sandboxToggle.GetComponent<ToolTip>().SetSimpleTooltip(GameUtil.ReplaceHotkeyString(UI.SANDBOX_TOGGLE.TOOLTIP_LOCKED, global::Action.ToggleSandboxTools));
			this.sandboxToggle.ChangeState(0);
		}
		else
		{
			this.sandboxToggle.GetComponent<ToolTip>().SetSimpleTooltip(GameUtil.ReplaceHotkeyString(UI.SANDBOX_TOGGLE.TOOLTIP_UNLOCKED, global::Action.ToggleSandboxTools));
			this.sandboxToggle.ChangeState(Game.Instance.SandboxModeActive ? 2 : 1);
		}
		this.sandboxToggle.gameObject.SetActive(SaveGame.Instance.sandboxEnabled);
	}

	// Token: 0x060078A3 RID: 30883 RVA: 0x002E63C8 File Offset: 0x002E45C8
	private void OnClickSandboxToggle()
	{
		if (this.CheckSandboxModeLocked())
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
		}
		else
		{
			Game.Instance.SandboxModeActive = !Game.Instance.SandboxModeActive;
			KMonoBehaviour.PlaySound(Game.Instance.SandboxModeActive ? GlobalAssets.GetSound("SandboxTool_Toggle_On", false) : GlobalAssets.GetSound("SandboxTool_Toggle_Off", false));
		}
		this.UpdateSandboxToggleState();
	}

	// Token: 0x060078A4 RID: 30884 RVA: 0x002E6438 File Offset: 0x002E4638
	private void RefreshKleiItemDropButton()
	{
		if (!KleiItemDropScreen.HasItemsToShow())
		{
			this.kleiItemDropButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.ITEM_DROP_SCREEN.IN_GAME_BUTTON.TOOLTIP_ERROR_NO_ITEMS);
			this.kleiItemDropButton.ChangeState(1);
			return;
		}
		this.kleiItemDropButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.ITEM_DROP_SCREEN.IN_GAME_BUTTON.TOOLTIP_ITEMS_AVAILABLE);
		this.kleiItemDropButton.ChangeState(2);
	}

	// Token: 0x060078A5 RID: 30885 RVA: 0x002E6499 File Offset: 0x002E4699
	private void OnClickKleiItemDropButton()
	{
		this.RefreshKleiItemDropButton();
		if (!KleiItemDropScreen.HasItemsToShow())
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
			return;
		}
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click", false));
		UnityEngine.Object.FindObjectOfType<KleiItemDropScreen>(true).Show(true);
	}

	// Token: 0x060078A6 RID: 30886 RVA: 0x002E64D5 File Offset: 0x002E46D5
	private bool CheckSandboxModeLocked()
	{
		return !SaveGame.Instance.sandboxEnabled;
	}

	// Token: 0x04005414 RID: 21524
	public static TopLeftControlScreen Instance;

	// Token: 0x04005415 RID: 21525
	[SerializeField]
	private MultiToggle sandboxToggle;

	// Token: 0x04005416 RID: 21526
	[SerializeField]
	private MultiToggle kleiItemDropButton;

	// Token: 0x04005417 RID: 21527
	[SerializeField]
	private LocText locText;

	// Token: 0x04005418 RID: 21528
	[SerializeField]
	private RectTransform secondaryRow;

	// Token: 0x0200211D RID: 8477
	private enum MultiToggleState
	{
		// Token: 0x04009842 RID: 38978
		Disabled,
		// Token: 0x04009843 RID: 38979
		Off,
		// Token: 0x04009844 RID: 38980
		On
	}
}
