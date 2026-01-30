using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000D36 RID: 3382
public class KButtonMenu : KScreen
{
	// Token: 0x06006892 RID: 26770 RVA: 0x00279AB5 File Offset: 0x00277CB5
	protected override void OnActivate()
	{
		base.ConsumeMouseScroll = this.ShouldConsumeMouseScroll;
		this.RefreshButtons();
	}

	// Token: 0x06006893 RID: 26771 RVA: 0x00279AC9 File Offset: 0x00277CC9
	public void SetButtons(IList<KButtonMenu.ButtonInfo> buttons)
	{
		this.buttons = buttons;
		if (this.activateOnSpawn)
		{
			this.RefreshButtons();
		}
	}

	// Token: 0x06006894 RID: 26772 RVA: 0x00279AE0 File Offset: 0x00277CE0
	public virtual void RefreshButtons()
	{
		if (this.buttonObjects != null)
		{
			for (int i = 0; i < this.buttonObjects.Length; i++)
			{
				UnityEngine.Object.Destroy(this.buttonObjects[i]);
			}
			this.buttonObjects = null;
		}
		if (this.buttons == null)
		{
			return;
		}
		this.buttonObjects = new GameObject[this.buttons.Count];
		for (int j = 0; j < this.buttons.Count; j++)
		{
			KButtonMenu.ButtonInfo binfo = this.buttons[j];
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.buttonPrefab, Vector3.zero, Quaternion.identity);
			this.buttonObjects[j] = gameObject;
			Transform parent = (this.buttonParent != null) ? this.buttonParent : base.transform;
			gameObject.transform.SetParent(parent, false);
			gameObject.SetActive(true);
			gameObject.name = binfo.text + "Button";
			LocText[] componentsInChildren = gameObject.GetComponentsInChildren<LocText>(true);
			if (componentsInChildren != null)
			{
				foreach (LocText locText in componentsInChildren)
				{
					locText.text = ((locText.name == "Hotkey") ? GameUtil.GetActionString(binfo.shortcutKey) : binfo.text);
					locText.color = (binfo.isEnabled ? new Color(1f, 1f, 1f) : new Color(0.5f, 0.5f, 0.5f));
				}
			}
			ToolTip componentInChildren = gameObject.GetComponentInChildren<ToolTip>();
			if (binfo.toolTip != null && binfo.toolTip != "" && componentInChildren != null)
			{
				componentInChildren.toolTip = binfo.toolTip;
			}
			KButtonMenu screen = this;
			KButton button = gameObject.GetComponent<KButton>();
			button.isInteractable = binfo.isEnabled;
			if (binfo.popupOptions == null && binfo.onPopulatePopup == null)
			{
				UnityAction onClick = binfo.onClick;
				System.Action value = delegate()
				{
					onClick();
					if (!this.keepMenuOpen && screen != null)
					{
						screen.Deactivate();
					}
				};
				button.onClick += value;
			}
			else
			{
				button.onClick += delegate()
				{
					this.SetupPopupMenu(binfo, button);
				};
			}
			binfo.uibutton = button;
			KButtonMenu.ButtonInfo.HoverCallback onHover = binfo.onHover;
		}
		this.Update();
	}

	// Token: 0x06006895 RID: 26773 RVA: 0x00279D94 File Offset: 0x00277F94
	protected Button.ButtonClickedEvent SetupPopupMenu(KButtonMenu.ButtonInfo binfo, KButton button)
	{
		Button.ButtonClickedEvent buttonClickedEvent = new Button.ButtonClickedEvent();
		UnityAction unityAction = delegate()
		{
			List<KButtonMenu.ButtonInfo> list = new List<KButtonMenu.ButtonInfo>();
			if (binfo.onPopulatePopup != null)
			{
				binfo.popupOptions = binfo.onPopulatePopup();
			}
			string[] popupOptions = binfo.popupOptions;
			for (int i = 0; i < popupOptions.Length; i++)
			{
				string delegate_str2 = popupOptions[i];
				string delegate_str = delegate_str2;
				list.Add(new KButtonMenu.ButtonInfo(delegate_str, delegate()
				{
					binfo.onPopupClick(delegate_str);
					if (!this.keepMenuOpen)
					{
						this.Deactivate();
					}
				}, global::Action.NumActions, null, null, null, true, null, null, null));
			}
			KButtonMenu component = Util.KInstantiate(ScreenPrefabs.Instance.ButtonGrid.gameObject, null, null).GetComponent<KButtonMenu>();
			component.SetButtons(list.ToArray());
			RootMenu.Instance.AddSubMenu(component);
			Game.Instance.LocalPlayer.ScreenManager.ActivateScreen(component.gameObject, null, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay);
			Vector3 b = default(Vector3);
			if (Util.IsOnLeftSideOfScreen(button.transform.GetPosition()))
			{
				b.x = button.GetComponent<RectTransform>().rect.width * 0.25f;
			}
			else
			{
				b.x = -button.GetComponent<RectTransform>().rect.width * 0.25f;
			}
			component.transform.SetPosition(button.transform.GetPosition() + b);
		};
		binfo.onClick = unityAction;
		buttonClickedEvent.AddListener(unityAction);
		return buttonClickedEvent;
	}

	// Token: 0x06006896 RID: 26774 RVA: 0x00279DE4 File Offset: 0x00277FE4
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.buttons == null)
		{
			return;
		}
		for (int i = 0; i < this.buttons.Count; i++)
		{
			KButtonMenu.ButtonInfo buttonInfo = this.buttons[i];
			if (e.TryConsume(buttonInfo.shortcutKey))
			{
				this.buttonObjects[i].GetComponent<KButton>().PlayPointerDownSound();
				this.buttonObjects[i].GetComponent<KButton>().SignalClick(KKeyCode.Mouse0);
				break;
			}
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06006897 RID: 26775 RVA: 0x00279E5D File Offset: 0x0027805D
	protected override void OnPrefabInit()
	{
		base.Subscribe<KButtonMenu>(315865555, KButtonMenu.OnSetActivatorDelegate);
	}

	// Token: 0x06006898 RID: 26776 RVA: 0x00279E70 File Offset: 0x00278070
	private void OnSetActivator(object data)
	{
		this.go = (GameObject)data;
		this.Update();
	}

	// Token: 0x06006899 RID: 26777 RVA: 0x00279E84 File Offset: 0x00278084
	protected override void OnDeactivate()
	{
	}

	// Token: 0x0600689A RID: 26778 RVA: 0x00279E88 File Offset: 0x00278088
	private void Update()
	{
		if (!this.followGameObject || this.go == null || base.canvas == null)
		{
			return;
		}
		Vector3 vector = Camera.main.WorldToViewportPoint(this.go.transform.GetPosition());
		RectTransform component = base.GetComponent<RectTransform>();
		RectTransform component2 = base.canvas.GetComponent<RectTransform>();
		if (component != null)
		{
			component.anchoredPosition = new Vector2(vector.x * component2.sizeDelta.x - component2.sizeDelta.x * 0.5f, vector.y * component2.sizeDelta.y - component2.sizeDelta.y * 0.5f);
		}
	}

	// Token: 0x040047D6 RID: 18390
	[SerializeField]
	protected bool followGameObject;

	// Token: 0x040047D7 RID: 18391
	[SerializeField]
	protected bool keepMenuOpen;

	// Token: 0x040047D8 RID: 18392
	[SerializeField]
	protected Transform buttonParent;

	// Token: 0x040047D9 RID: 18393
	public GameObject buttonPrefab;

	// Token: 0x040047DA RID: 18394
	public bool ShouldConsumeMouseScroll;

	// Token: 0x040047DB RID: 18395
	[NonSerialized]
	public GameObject[] buttonObjects;

	// Token: 0x040047DC RID: 18396
	protected GameObject go;

	// Token: 0x040047DD RID: 18397
	protected IList<KButtonMenu.ButtonInfo> buttons;

	// Token: 0x040047DE RID: 18398
	private static readonly EventSystem.IntraObjectHandler<KButtonMenu> OnSetActivatorDelegate = new EventSystem.IntraObjectHandler<KButtonMenu>(delegate(KButtonMenu component, object data)
	{
		component.OnSetActivator(data);
	});

	// Token: 0x02001F60 RID: 8032
	public class ButtonInfo
	{
		// Token: 0x0600B62A RID: 46634 RVA: 0x003EF74C File Offset: 0x003ED94C
		public ButtonInfo(string text = null, UnityAction on_click = null, global::Action shortcut_key = global::Action.NumActions, KButtonMenu.ButtonInfo.HoverCallback on_hover = null, string tool_tip = null, GameObject visualizer = null, bool is_enabled = true, string[] popup_options = null, Action<string> on_popup_click = null, Func<string[]> on_populate_popup = null)
		{
			this.text = text;
			this.shortcutKey = shortcut_key;
			this.onClick = on_click;
			this.onHover = on_hover;
			this.visualizer = visualizer;
			this.toolTip = tool_tip;
			this.isEnabled = is_enabled;
			this.uibutton = null;
			this.popupOptions = popup_options;
			this.onPopupClick = on_popup_click;
			this.onPopulatePopup = on_populate_popup;
		}

		// Token: 0x0600B62B RID: 46635 RVA: 0x003EF7BC File Offset: 0x003ED9BC
		public ButtonInfo(string text, global::Action shortcutKey, UnityAction onClick, KButtonMenu.ButtonInfo.HoverCallback onHover = null, object userData = null)
		{
			this.text = text;
			this.shortcutKey = shortcutKey;
			this.onClick = onClick;
			this.onHover = onHover;
			this.userData = userData;
			this.visualizer = null;
			this.uibutton = null;
		}

		// Token: 0x0600B62C RID: 46636 RVA: 0x003EF80C File Offset: 0x003EDA0C
		public ButtonInfo(string text, GameObject visualizer, global::Action shortcutKey, UnityAction onClick, KButtonMenu.ButtonInfo.HoverCallback onHover = null, object userData = null)
		{
			this.text = text;
			this.shortcutKey = shortcutKey;
			this.onClick = onClick;
			this.onHover = onHover;
			this.visualizer = visualizer;
			this.userData = userData;
			this.uibutton = null;
		}

		// Token: 0x040092A1 RID: 37537
		public string text;

		// Token: 0x040092A2 RID: 37538
		public global::Action shortcutKey;

		// Token: 0x040092A3 RID: 37539
		public GameObject visualizer;

		// Token: 0x040092A4 RID: 37540
		public UnityAction onClick;

		// Token: 0x040092A5 RID: 37541
		public KButtonMenu.ButtonInfo.HoverCallback onHover;

		// Token: 0x040092A6 RID: 37542
		public FMODAsset clickSound;

		// Token: 0x040092A7 RID: 37543
		public KButton uibutton;

		// Token: 0x040092A8 RID: 37544
		public string toolTip;

		// Token: 0x040092A9 RID: 37545
		public bool isEnabled = true;

		// Token: 0x040092AA RID: 37546
		public string[] popupOptions;

		// Token: 0x040092AB RID: 37547
		public Action<string> onPopupClick;

		// Token: 0x040092AC RID: 37548
		public Func<string[]> onPopulatePopup;

		// Token: 0x040092AD RID: 37549
		public object userData;

		// Token: 0x02002A78 RID: 10872
		// (Invoke) Token: 0x0600D4C1 RID: 54465
		public delegate void HoverCallback(GameObject hoverTarget);

		// Token: 0x02002A79 RID: 10873
		// (Invoke) Token: 0x0600D4C5 RID: 54469
		public delegate void Callback();
	}
}
