using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000D38 RID: 3384
public class KIconButtonMenu : KScreen
{
	// Token: 0x060068A5 RID: 26789 RVA: 0x0027A214 File Offset: 0x00278414
	protected override void OnActivate()
	{
		base.OnActivate();
		this.RefreshButtons();
	}

	// Token: 0x060068A6 RID: 26790 RVA: 0x0027A222 File Offset: 0x00278422
	public void SetButtons(IList<KIconButtonMenu.ButtonInfo> buttons)
	{
		this.buttons = buttons;
		if (this.activateOnSpawn)
		{
			this.RefreshButtons();
		}
	}

	// Token: 0x060068A7 RID: 26791 RVA: 0x0027A23C File Offset: 0x0027843C
	public void RefreshButtonTooltip()
	{
		for (int i = 0; i < this.buttons.Count; i++)
		{
			KIconButtonMenu.ButtonInfo buttonInfo = this.buttons[i];
			if (buttonInfo.buttonGo == null || buttonInfo == null)
			{
				return;
			}
			ToolTip componentInChildren = buttonInfo.buttonGo.GetComponentInChildren<ToolTip>();
			if (buttonInfo.text != null && buttonInfo.text != "" && componentInChildren != null)
			{
				componentInChildren.toolTip = buttonInfo.GetTooltipText();
				LocText componentInChildren2 = buttonInfo.buttonGo.GetComponentInChildren<LocText>();
				if (componentInChildren2 != null)
				{
					componentInChildren2.text = buttonInfo.text;
				}
			}
		}
	}

	// Token: 0x060068A8 RID: 26792 RVA: 0x0027A2E0 File Offset: 0x002784E0
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
		if (this.buttons == null || this.buttons.Count == 0)
		{
			return;
		}
		this.buttonObjects = new GameObject[this.buttons.Count];
		for (int j = 0; j < this.buttons.Count; j++)
		{
			KIconButtonMenu.ButtonInfo buttonInfo = this.buttons[j];
			if (buttonInfo != null)
			{
				GameObject binstance = UnityEngine.Object.Instantiate<GameObject>(this.buttonPrefab, Vector3.zero, Quaternion.identity);
				buttonInfo.buttonGo = binstance;
				this.buttonObjects[j] = binstance;
				Transform parent = (this.buttonParent != null) ? this.buttonParent : base.transform;
				binstance.transform.SetParent(parent, false);
				binstance.SetActive(true);
				binstance.name = buttonInfo.text + "Button";
				KButton component = binstance.GetComponent<KButton>();
				if (component != null && buttonInfo.onClick != null)
				{
					component.onClick += buttonInfo.onClick;
				}
				Image image = null;
				if (component)
				{
					image = component.fgImage;
				}
				if (image != null)
				{
					image.gameObject.SetActive(false);
					foreach (Sprite sprite in this.icons)
					{
						if (sprite != null && sprite.name == buttonInfo.iconName)
						{
							image.sprite = sprite;
							image.gameObject.SetActive(true);
							break;
						}
					}
				}
				if (buttonInfo.texture != null)
				{
					RawImage componentInChildren = binstance.GetComponentInChildren<RawImage>();
					if (componentInChildren != null)
					{
						componentInChildren.gameObject.SetActive(true);
						componentInChildren.texture = buttonInfo.texture;
					}
				}
				ToolTip componentInChildren2 = binstance.GetComponentInChildren<ToolTip>();
				if (buttonInfo.text != null && buttonInfo.text != "" && componentInChildren2 != null)
				{
					componentInChildren2.toolTip = buttonInfo.GetTooltipText();
					LocText componentInChildren3 = binstance.GetComponentInChildren<LocText>();
					if (componentInChildren3 != null)
					{
						componentInChildren3.text = buttonInfo.text;
					}
				}
				if (buttonInfo.onToolTip != null)
				{
					componentInChildren2.OnToolTip = buttonInfo.onToolTip;
				}
				KIconButtonMenu screen = this;
				System.Action onClick = buttonInfo.onClick;
				System.Action value = delegate()
				{
					onClick.Signal();
					if (!this.keepMenuOpen && screen != null)
					{
						screen.Deactivate();
					}
					if (binstance != null)
					{
						KToggle component3 = binstance.GetComponent<KToggle>();
						if (component3 != null)
						{
							this.SelectToggle(component3);
						}
					}
				};
				KToggle componentInChildren4 = binstance.GetComponentInChildren<KToggle>();
				if (componentInChildren4 != null)
				{
					ToggleGroup component2 = base.GetComponent<ToggleGroup>();
					if (component2 == null)
					{
						component2 = this.externalToggleGroup;
					}
					componentInChildren4.group = component2;
					componentInChildren4.onClick += value;
					Navigation navigation = componentInChildren4.navigation;
					navigation.mode = (this.automaticNavigation ? Navigation.Mode.Automatic : Navigation.Mode.None);
					componentInChildren4.navigation = navigation;
				}
				else
				{
					KBasicToggle componentInChildren5 = binstance.GetComponentInChildren<KBasicToggle>();
					if (componentInChildren5 != null)
					{
						componentInChildren5.onClick += value;
					}
				}
				if (component != null)
				{
					component.isInteractable = buttonInfo.isInteractable;
				}
				buttonInfo.onCreate.Signal(buttonInfo);
			}
		}
		this.Update();
	}

	// Token: 0x060068A9 RID: 26793 RVA: 0x0027A650 File Offset: 0x00278850
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.buttons == null)
		{
			return;
		}
		if (!base.gameObject.activeSelf || !base.enabled)
		{
			return;
		}
		for (int i = 0; i < this.buttons.Count; i++)
		{
			KIconButtonMenu.ButtonInfo buttonInfo = this.buttons[i];
			if (e.TryConsume(buttonInfo.shortcutKey))
			{
				this.buttonObjects[i].GetComponent<KButton>().PlayPointerDownSound();
				this.buttonObjects[i].GetComponent<KButton>().SignalClick(KKeyCode.Mouse0);
				break;
			}
		}
		base.OnKeyDown(e);
	}

	// Token: 0x060068AA RID: 26794 RVA: 0x0027A6DF File Offset: 0x002788DF
	protected override void OnPrefabInit()
	{
		base.Subscribe<KIconButtonMenu>(315865555, KIconButtonMenu.OnSetActivatorDelegate);
	}

	// Token: 0x060068AB RID: 26795 RVA: 0x0027A6F2 File Offset: 0x002788F2
	private void OnSetActivator(object data)
	{
		this.go = (GameObject)data;
		this.Update();
	}

	// Token: 0x060068AC RID: 26796 RVA: 0x0027A708 File Offset: 0x00278908
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

	// Token: 0x060068AD RID: 26797 RVA: 0x0027A7C4 File Offset: 0x002789C4
	protected void SelectToggle(KToggle selectedToggle)
	{
		if (UnityEngine.EventSystems.EventSystem.current == null || !UnityEngine.EventSystems.EventSystem.current.enabled)
		{
			return;
		}
		if (this.currentlySelectedToggle == selectedToggle)
		{
			this.currentlySelectedToggle = null;
		}
		else
		{
			this.currentlySelectedToggle = selectedToggle;
		}
		GameObject[] array = this.buttonObjects;
		for (int i = 0; i < array.Length; i++)
		{
			KToggle component = array[i].GetComponent<KToggle>();
			if (component != null)
			{
				if (component == this.currentlySelectedToggle)
				{
					component.Select();
					component.isOn = true;
				}
				else
				{
					component.Deselect();
					component.isOn = false;
				}
			}
		}
	}

	// Token: 0x060068AE RID: 26798 RVA: 0x0027A85C File Offset: 0x00278A5C
	public void ClearSelection()
	{
		foreach (GameObject gameObject in this.buttonObjects)
		{
			KToggle component = gameObject.GetComponent<KToggle>();
			if (component != null)
			{
				component.Deselect();
				component.isOn = false;
			}
			else
			{
				KBasicToggle component2 = gameObject.GetComponent<KBasicToggle>();
				if (component2 != null)
				{
					component2.isOn = false;
				}
			}
			ImageToggleState component3 = gameObject.GetComponent<ImageToggleState>();
			if (component3.GetIsActive())
			{
				component3.SetInactive();
			}
		}
		ToggleGroup component4 = base.GetComponent<ToggleGroup>();
		if (component4 != null)
		{
			component4.SetAllTogglesOff(true);
		}
		this.SelectToggle(null);
	}

	// Token: 0x040047E3 RID: 18403
	[SerializeField]
	protected bool followGameObject;

	// Token: 0x040047E4 RID: 18404
	[SerializeField]
	protected bool keepMenuOpen;

	// Token: 0x040047E5 RID: 18405
	[SerializeField]
	protected bool automaticNavigation = true;

	// Token: 0x040047E6 RID: 18406
	[SerializeField]
	protected Transform buttonParent;

	// Token: 0x040047E7 RID: 18407
	[SerializeField]
	private GameObject buttonPrefab;

	// Token: 0x040047E8 RID: 18408
	[SerializeField]
	protected Sprite[] icons;

	// Token: 0x040047E9 RID: 18409
	[SerializeField]
	private ToggleGroup externalToggleGroup;

	// Token: 0x040047EA RID: 18410
	protected KToggle currentlySelectedToggle;

	// Token: 0x040047EB RID: 18411
	[NonSerialized]
	public GameObject[] buttonObjects;

	// Token: 0x040047EC RID: 18412
	[SerializeField]
	public TextStyleSetting ToggleToolTipTextStyleSetting;

	// Token: 0x040047ED RID: 18413
	private UnityAction inputChangeReceiver;

	// Token: 0x040047EE RID: 18414
	protected GameObject go;

	// Token: 0x040047EF RID: 18415
	protected IList<KIconButtonMenu.ButtonInfo> buttons;

	// Token: 0x040047F0 RID: 18416
	private static readonly global::EventSystem.IntraObjectHandler<KIconButtonMenu> OnSetActivatorDelegate = new global::EventSystem.IntraObjectHandler<KIconButtonMenu>(delegate(KIconButtonMenu component, object data)
	{
		component.OnSetActivator(data);
	});

	// Token: 0x02001F67 RID: 8039
	public class ButtonInfo
	{
		// Token: 0x0600B639 RID: 46649 RVA: 0x003EFAD8 File Offset: 0x003EDCD8
		public ButtonInfo(string iconName = "", string text = "", System.Action on_click = null, global::Action shortcutKey = global::Action.NumActions, Action<GameObject> on_refresh = null, Action<KIconButtonMenu.ButtonInfo> on_create = null, Texture texture = null, string tooltipText = "", bool is_interactable = true)
		{
			this.iconName = iconName;
			this.text = text;
			this.shortcutKey = shortcutKey;
			this.onClick = on_click;
			this.onCreate = on_create;
			this.texture = texture;
			this.tooltipText = tooltipText;
			this.isInteractable = is_interactable;
		}

		// Token: 0x0600B63A RID: 46650 RVA: 0x003EFB28 File Offset: 0x003EDD28
		public string GetTooltipText()
		{
			string text = (this.tooltipText == "") ? this.text : this.tooltipText;
			if (this.shortcutKey != global::Action.NumActions)
			{
				text = GameUtil.ReplaceHotkeyString(text, this.shortcutKey);
			}
			return text;
		}

		// Token: 0x040092BC RID: 37564
		public string iconName;

		// Token: 0x040092BD RID: 37565
		public string text;

		// Token: 0x040092BE RID: 37566
		public string tooltipText;

		// Token: 0x040092BF RID: 37567
		public string[] multiText;

		// Token: 0x040092C0 RID: 37568
		public global::Action shortcutKey;

		// Token: 0x040092C1 RID: 37569
		public bool isInteractable;

		// Token: 0x040092C2 RID: 37570
		public Action<KIconButtonMenu.ButtonInfo> onCreate;

		// Token: 0x040092C3 RID: 37571
		public System.Action onClick;

		// Token: 0x040092C4 RID: 37572
		public Func<string> onToolTip;

		// Token: 0x040092C5 RID: 37573
		public GameObject buttonGo;

		// Token: 0x040092C6 RID: 37574
		public object userData;

		// Token: 0x040092C7 RID: 37575
		public Texture texture;

		// Token: 0x02002A7A RID: 10874
		// (Invoke) Token: 0x0600D4C9 RID: 54473
		public delegate void Callback();
	}
}
