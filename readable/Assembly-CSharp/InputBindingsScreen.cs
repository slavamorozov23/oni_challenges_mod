using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D31 RID: 3377
public class InputBindingsScreen : KModalScreen
{
	// Token: 0x06006848 RID: 26696 RVA: 0x002752CB File Offset: 0x002734CB
	public override bool IsModal()
	{
		return true;
	}

	// Token: 0x06006849 RID: 26697 RVA: 0x002752CE File Offset: 0x002734CE
	private bool IsKeyDown(KeyCode key_code)
	{
		return Input.GetKey(key_code) || Input.GetKeyDown(key_code);
	}

	// Token: 0x0600684A RID: 26698 RVA: 0x002752E0 File Offset: 0x002734E0
	private string GetModifierString(Modifier modifiers)
	{
		string text = "";
		foreach (object obj in Enum.GetValues(typeof(Modifier)))
		{
			Modifier modifier = (Modifier)obj;
			if ((modifiers & modifier) != Modifier.None)
			{
				text = text + " + " + modifier.ToString();
			}
		}
		return text;
	}

	// Token: 0x0600684B RID: 26699 RVA: 0x00275360 File Offset: 0x00273560
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.entryPrefab.SetActive(false);
		this.prevScreenButton.onClick += this.OnPrevScreen;
		this.nextScreenButton.onClick += this.OnNextScreen;
	}

	// Token: 0x0600684C RID: 26700 RVA: 0x002753B0 File Offset: 0x002735B0
	protected override void OnActivate()
	{
		this.CollectScreens();
		string text = this.screens[this.activeScreen];
		string key = "STRINGS.INPUT_BINDINGS." + text.ToUpper() + ".NAME";
		this.screenTitle.text = Strings.Get(key);
		this.closeButton.onClick += this.OnBack;
		this.backButton.onClick += this.OnBack;
		this.resetButton.onClick += this.OnReset;
		this.BuildDisplay();
	}

	// Token: 0x0600684D RID: 26701 RVA: 0x0027544C File Offset: 0x0027364C
	private void CollectScreens()
	{
		this.screens.Clear();
		for (int i = 0; i < GameInputMapping.KeyBindings.Length; i++)
		{
			BindingEntry bindingEntry = GameInputMapping.KeyBindings[i];
			if (bindingEntry.mGroup != null && bindingEntry.mRebindable && !this.screens.Contains(bindingEntry.mGroup) && DlcManager.IsCorrectDlcSubscribed(bindingEntry.dlcIds, null))
			{
				if (bindingEntry.mGroup == "Root")
				{
					this.activeScreen = this.screens.Count;
				}
				this.screens.Add(bindingEntry.mGroup);
			}
		}
	}

	// Token: 0x0600684E RID: 26702 RVA: 0x002754E7 File Offset: 0x002736E7
	protected override void OnDeactivate()
	{
		GameInputMapping.SaveBindings();
		this.DestroyDisplay();
	}

	// Token: 0x0600684F RID: 26703 RVA: 0x002754F4 File Offset: 0x002736F4
	private LocString GetActionString(global::Action action)
	{
		return null;
	}

	// Token: 0x06006850 RID: 26704 RVA: 0x002754F8 File Offset: 0x002736F8
	private string GetBindingText(BindingEntry binding)
	{
		string text = GameUtil.GetKeycodeLocalized(binding.mKeyCode);
		if (binding.mKeyCode != KKeyCode.LeftAlt && binding.mKeyCode != KKeyCode.RightAlt && binding.mKeyCode != KKeyCode.LeftControl && binding.mKeyCode != KKeyCode.RightControl && binding.mKeyCode != KKeyCode.LeftShift && binding.mKeyCode != KKeyCode.RightShift)
		{
			text += this.GetModifierString(binding.mModifier);
		}
		return text;
	}

	// Token: 0x06006851 RID: 26705 RVA: 0x00275574 File Offset: 0x00273774
	private void BuildDisplay()
	{
		string text = this.screens[this.activeScreen];
		string key = "STRINGS.INPUT_BINDINGS." + text.ToUpper() + ".NAME";
		this.screenTitle.text = Strings.Get(key);
		if (this.entryPool == null)
		{
			this.entryPool = new UIPool<HorizontalLayoutGroup>(this.entryPrefab.GetComponent<HorizontalLayoutGroup>());
		}
		this.DestroyDisplay();
		int num = 0;
		for (int i = 0; i < GameInputMapping.KeyBindings.Length; i++)
		{
			BindingEntry binding = GameInputMapping.KeyBindings[i];
			if (binding.mGroup == this.screens[this.activeScreen] && binding.mRebindable && DlcManager.IsCorrectDlcSubscribed(binding.dlcIds, null))
			{
				GameObject gameObject = this.entryPool.GetFreeElement(this.parent, true).gameObject;
				TMP_Text componentInChildren = gameObject.transform.GetChild(0).GetComponentInChildren<LocText>();
				string key2 = "STRINGS.INPUT_BINDINGS." + binding.mGroup.ToUpper() + "." + binding.mAction.ToString().ToUpper();
				componentInChildren.text = Strings.Get(key2);
				LocText key_label = gameObject.transform.GetChild(1).GetComponentInChildren<LocText>();
				key_label.text = this.GetBindingText(binding);
				KButton button = gameObject.GetComponentInChildren<KButton>();
				button.onClick += delegate()
				{
					this.waitingForKeyPress = true;
					this.actionToRebind = binding.mAction;
					this.ignoreRootConflicts = binding.mIgnoreRootConflics;
					this.activeButton = button;
					key_label.text = UI.FRONTEND.INPUT_BINDINGS_SCREEN.WAITING_FOR_INPUT;
				};
				gameObject.transform.SetSiblingIndex(num);
				num++;
			}
		}
	}

	// Token: 0x06006852 RID: 26706 RVA: 0x00275751 File Offset: 0x00273951
	private void DestroyDisplay()
	{
		this.entryPool.ClearAll();
	}

	// Token: 0x06006853 RID: 26707 RVA: 0x00275760 File Offset: 0x00273960
	private void Update()
	{
		if (this.waitingForKeyPress)
		{
			Modifier modifier = Modifier.None;
			modifier |= ((this.IsKeyDown(KeyCode.LeftAlt) || this.IsKeyDown(KeyCode.RightAlt)) ? Modifier.Alt : Modifier.None);
			modifier |= ((this.IsKeyDown(KeyCode.LeftControl) || this.IsKeyDown(KeyCode.RightControl)) ? Modifier.Ctrl : Modifier.None);
			modifier |= ((this.IsKeyDown(KeyCode.LeftShift) || this.IsKeyDown(KeyCode.RightShift)) ? Modifier.Shift : Modifier.None);
			modifier |= (this.IsKeyDown(KeyCode.CapsLock) ? Modifier.CapsLock : Modifier.None);
			modifier |= (this.IsKeyDown(KeyCode.BackQuote) ? Modifier.Backtick : Modifier.None);
			bool flag = false;
			for (int i = 0; i < InputBindingsScreen.validKeys.Length; i++)
			{
				KeyCode keyCode = InputBindingsScreen.validKeys[i];
				if (Input.GetKeyDown(keyCode))
				{
					KKeyCode kkey_code = (KKeyCode)keyCode;
					this.Bind(kkey_code, modifier);
					flag = true;
				}
			}
			if (!flag)
			{
				float axis = Input.GetAxis("Mouse ScrollWheel");
				KKeyCode kkeyCode = KKeyCode.None;
				if (axis < 0f)
				{
					kkeyCode = KKeyCode.MouseScrollDown;
				}
				else if (axis > 0f)
				{
					kkeyCode = KKeyCode.MouseScrollUp;
				}
				if (kkeyCode != KKeyCode.None)
				{
					this.Bind(kkeyCode, modifier);
				}
			}
		}
	}

	// Token: 0x06006854 RID: 26708 RVA: 0x00275878 File Offset: 0x00273A78
	private BindingEntry GetDuplicatedBinding(string activeScreen, BindingEntry new_binding)
	{
		BindingEntry result = default(BindingEntry);
		for (int i = 0; i < GameInputMapping.KeyBindings.Length; i++)
		{
			BindingEntry bindingEntry = GameInputMapping.KeyBindings[i];
			if (new_binding.IsBindingEqual(bindingEntry) && (bindingEntry.mGroup == null || bindingEntry.mGroup == activeScreen || bindingEntry.mGroup == "Root" || activeScreen == "Root") && (!(activeScreen == "Root") || !bindingEntry.mIgnoreRootConflics) && (!(bindingEntry.mGroup == "Root") || !new_binding.mIgnoreRootConflics))
			{
				result = bindingEntry;
				break;
			}
		}
		return result;
	}

	// Token: 0x06006855 RID: 26709 RVA: 0x00275924 File Offset: 0x00273B24
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.waitingForKeyPress)
		{
			e.Consumed = true;
			return;
		}
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.Deactivate();
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06006856 RID: 26710 RVA: 0x00275956 File Offset: 0x00273B56
	public override void OnKeyUp(KButtonEvent e)
	{
		e.Consumed = true;
	}

	// Token: 0x06006857 RID: 26711 RVA: 0x00275960 File Offset: 0x00273B60
	private void OnBack()
	{
		int num = this.NumUnboundActions();
		if (num == 0)
		{
			this.Deactivate();
			return;
		}
		string text;
		if (num == 1)
		{
			BindingEntry firstUnbound = this.GetFirstUnbound();
			text = string.Format(UI.FRONTEND.INPUT_BINDINGS_SCREEN.UNBOUND_ACTION, firstUnbound.mAction.ToString());
		}
		else
		{
			text = UI.FRONTEND.INPUT_BINDINGS_SCREEN.MULTIPLE_UNBOUND_ACTIONS;
		}
		this.confirmDialog = Util.KInstantiateUI(this.confirmPrefab.gameObject, base.transform.gameObject, false).GetComponent<ConfirmDialogScreen>();
		this.confirmDialog.PopupConfirmDialog(text, delegate
		{
			this.Deactivate();
		}, delegate
		{
			this.confirmDialog.Deactivate();
		}, null, null, null, null, null, null);
		this.confirmDialog.gameObject.SetActive(true);
	}

	// Token: 0x06006858 RID: 26712 RVA: 0x00275A1C File Offset: 0x00273C1C
	private int NumUnboundActions()
	{
		int num = 0;
		for (int i = 0; i < GameInputMapping.KeyBindings.Length; i++)
		{
			BindingEntry bindingEntry = GameInputMapping.KeyBindings[i];
			if (bindingEntry.mKeyCode == KKeyCode.None && bindingEntry.mRebindable && (BuildMenu.UseHotkeyBuildMenu() || !bindingEntry.mIgnoreRootConflics))
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06006859 RID: 26713 RVA: 0x00275A70 File Offset: 0x00273C70
	private BindingEntry GetFirstUnbound()
	{
		BindingEntry result = default(BindingEntry);
		for (int i = 0; i < GameInputMapping.KeyBindings.Length; i++)
		{
			BindingEntry bindingEntry = GameInputMapping.KeyBindings[i];
			if (bindingEntry.mKeyCode == KKeyCode.None)
			{
				result = bindingEntry;
				break;
			}
		}
		return result;
	}

	// Token: 0x0600685A RID: 26714 RVA: 0x00275AB0 File Offset: 0x00273CB0
	private void OnReset()
	{
		GameInputMapping.KeyBindings = (BindingEntry[])GameInputMapping.DefaultBindings.Clone();
		Global.GetInputManager().RebindControls();
		this.BuildDisplay();
	}

	// Token: 0x0600685B RID: 26715 RVA: 0x00275AD6 File Offset: 0x00273CD6
	public void OnPrevScreen()
	{
		if (this.activeScreen > 0)
		{
			this.activeScreen--;
		}
		else
		{
			this.activeScreen = this.screens.Count - 1;
		}
		this.BuildDisplay();
	}

	// Token: 0x0600685C RID: 26716 RVA: 0x00275B0A File Offset: 0x00273D0A
	public void OnNextScreen()
	{
		if (this.activeScreen < this.screens.Count - 1)
		{
			this.activeScreen++;
		}
		else
		{
			this.activeScreen = 0;
		}
		this.BuildDisplay();
	}

	// Token: 0x0600685D RID: 26717 RVA: 0x00275B40 File Offset: 0x00273D40
	private void Bind(KKeyCode kkey_code, Modifier modifier)
	{
		BindingEntry bindingEntry = new BindingEntry(this.screens[this.activeScreen], GamepadButton.NumButtons, kkey_code, modifier, this.actionToRebind, true, this.ignoreRootConflicts);
		for (int i = 0; i < GameInputMapping.KeyBindings.Length; i++)
		{
			BindingEntry bindingEntry2 = GameInputMapping.KeyBindings[i];
			if (bindingEntry2.mRebindable && bindingEntry2.mAction == this.actionToRebind)
			{
				BindingEntry duplicatedBinding = this.GetDuplicatedBinding(this.screens[this.activeScreen], bindingEntry);
				bindingEntry.mButton = GameInputMapping.KeyBindings[i].mButton;
				GameInputMapping.KeyBindings[i] = bindingEntry;
				this.activeButton.GetComponentInChildren<LocText>().text = this.GetBindingText(bindingEntry);
				if (duplicatedBinding.mAction != global::Action.Invalid && duplicatedBinding.mAction != this.actionToRebind)
				{
					this.confirmDialog = Util.KInstantiateUI(this.confirmPrefab.gameObject, base.transform.gameObject, false).GetComponent<ConfirmDialogScreen>();
					string arg = Strings.Get("STRINGS.INPUT_BINDINGS." + duplicatedBinding.mGroup.ToUpper() + "." + duplicatedBinding.mAction.ToString().ToUpper());
					string bindingText = this.GetBindingText(duplicatedBinding);
					string text = string.Format(UI.FRONTEND.INPUT_BINDINGS_SCREEN.DUPLICATE, arg, bindingText);
					this.Unbind(duplicatedBinding.mAction);
					this.confirmDialog.PopupConfirmDialog(text, null, null, null, null, null, null, null, null);
					this.confirmDialog.gameObject.SetActive(true);
				}
				Global.GetInputManager().RebindControls();
				this.waitingForKeyPress = false;
				this.actionToRebind = global::Action.NumActions;
				this.activeButton = null;
				this.BuildDisplay();
				return;
			}
		}
	}

	// Token: 0x0600685E RID: 26718 RVA: 0x00275D04 File Offset: 0x00273F04
	private void Unbind(global::Action action)
	{
		for (int i = 0; i < GameInputMapping.KeyBindings.Length; i++)
		{
			BindingEntry bindingEntry = GameInputMapping.KeyBindings[i];
			if (bindingEntry.mAction == action)
			{
				bindingEntry.mKeyCode = KKeyCode.None;
				bindingEntry.mModifier = Modifier.None;
				GameInputMapping.KeyBindings[i] = bindingEntry;
			}
		}
	}

	// Token: 0x06006860 RID: 26720 RVA: 0x00275D79 File Offset: 0x00273F79
	// Note: this type is marked as 'beforefieldinit'.
	static InputBindingsScreen()
	{
		KeyCode[] array = new KeyCode[111];
		RuntimeHelpers.InitializeArray(array, fieldof(<PrivateImplementationDetails>.4522A529DBF1D30936B6BCC06D2E607CD76E3B0FB1C18D9DA2635843A2840CD7).FieldHandle);
		InputBindingsScreen.validKeys = array;
	}

	// Token: 0x040047A0 RID: 18336
	private const string ROOT_KEY = "STRINGS.INPUT_BINDINGS.";

	// Token: 0x040047A1 RID: 18337
	[SerializeField]
	private OptionsMenuScreen optionsScreen;

	// Token: 0x040047A2 RID: 18338
	[SerializeField]
	private ConfirmDialogScreen confirmPrefab;

	// Token: 0x040047A3 RID: 18339
	public KButton backButton;

	// Token: 0x040047A4 RID: 18340
	public KButton resetButton;

	// Token: 0x040047A5 RID: 18341
	public KButton closeButton;

	// Token: 0x040047A6 RID: 18342
	public KButton prevScreenButton;

	// Token: 0x040047A7 RID: 18343
	public KButton nextScreenButton;

	// Token: 0x040047A8 RID: 18344
	private bool waitingForKeyPress;

	// Token: 0x040047A9 RID: 18345
	private global::Action actionToRebind = global::Action.NumActions;

	// Token: 0x040047AA RID: 18346
	private bool ignoreRootConflicts;

	// Token: 0x040047AB RID: 18347
	private KButton activeButton;

	// Token: 0x040047AC RID: 18348
	[SerializeField]
	private LocText screenTitle;

	// Token: 0x040047AD RID: 18349
	[SerializeField]
	private GameObject parent;

	// Token: 0x040047AE RID: 18350
	[SerializeField]
	private GameObject entryPrefab;

	// Token: 0x040047AF RID: 18351
	private ConfirmDialogScreen confirmDialog;

	// Token: 0x040047B0 RID: 18352
	private int activeScreen = -1;

	// Token: 0x040047B1 RID: 18353
	private List<string> screens = new List<string>();

	// Token: 0x040047B2 RID: 18354
	private UIPool<HorizontalLayoutGroup> entryPool;

	// Token: 0x040047B3 RID: 18355
	private static readonly KeyCode[] validKeys;
}
