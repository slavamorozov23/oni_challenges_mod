using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D39 RID: 3385
public class KIconToggleMenu : KScreen
{
	// Token: 0x1400002A RID: 42
	// (add) Token: 0x060068B1 RID: 26801 RVA: 0x0027A920 File Offset: 0x00278B20
	// (remove) Token: 0x060068B2 RID: 26802 RVA: 0x0027A958 File Offset: 0x00278B58
	public event KIconToggleMenu.OnSelect onSelect;

	// Token: 0x060068B3 RID: 26803 RVA: 0x0027A98D File Offset: 0x00278B8D
	public void Setup(IList<KIconToggleMenu.ToggleInfo> toggleInfo)
	{
		this.toggleInfo = toggleInfo;
		this.RefreshButtons();
	}

	// Token: 0x060068B4 RID: 26804 RVA: 0x0027A99C File Offset: 0x00278B9C
	protected void Setup()
	{
		this.RefreshButtons();
	}

	// Token: 0x060068B5 RID: 26805 RVA: 0x0027A9A4 File Offset: 0x00278BA4
	protected virtual void RefreshButtons()
	{
		foreach (KToggle ktoggle in this.toggles)
		{
			if (ktoggle != null)
			{
				if (!this.dontDestroyToggles.Contains(ktoggle))
				{
					UnityEngine.Object.Destroy(ktoggle.gameObject);
				}
				else
				{
					ktoggle.ClearOnClick();
				}
			}
		}
		this.toggles.Clear();
		this.dontDestroyToggles.Clear();
		if (this.toggleInfo == null)
		{
			return;
		}
		Transform transform = (this.toggleParent != null) ? this.toggleParent : base.transform;
		for (int i = 0; i < this.toggleInfo.Count; i++)
		{
			int idx = i;
			KIconToggleMenu.ToggleInfo toggleInfo = this.toggleInfo[i];
			KToggle ktoggle2;
			if (toggleInfo.instanceOverride != null)
			{
				ktoggle2 = toggleInfo.instanceOverride;
				this.dontDestroyToggles.Add(ktoggle2);
			}
			else if (toggleInfo.prefabOverride)
			{
				ktoggle2 = Util.KInstantiateUI<KToggle>(toggleInfo.prefabOverride.gameObject, transform.gameObject, true);
			}
			else
			{
				ktoggle2 = Util.KInstantiateUI<KToggle>(this.prefab.gameObject, transform.gameObject, true);
			}
			ktoggle2.Deselect();
			ktoggle2.gameObject.name = "Toggle:" + toggleInfo.text;
			ktoggle2.group = this.group;
			ktoggle2.onClick += delegate()
			{
				this.OnClick(idx);
			};
			LocText componentInChildren = ktoggle2.transform.GetComponentInChildren<LocText>();
			if (componentInChildren != null)
			{
				componentInChildren.SetText(toggleInfo.text);
			}
			if (toggleInfo.getSpriteCB != null)
			{
				ktoggle2.fgImage.sprite = toggleInfo.getSpriteCB();
			}
			else if (toggleInfo.icon != null)
			{
				ktoggle2.fgImage.sprite = Assets.GetSprite(toggleInfo.icon);
			}
			toggleInfo.SetToggle(ktoggle2);
			this.toggles.Add(ktoggle2);
		}
	}

	// Token: 0x060068B6 RID: 26806 RVA: 0x0027ABCC File Offset: 0x00278DCC
	public Sprite GetIcon(string name)
	{
		foreach (Sprite sprite in this.icons)
		{
			if (sprite.name == name)
			{
				return sprite;
			}
		}
		return null;
	}

	// Token: 0x060068B7 RID: 26807 RVA: 0x0027AC04 File Offset: 0x00278E04
	public virtual void ClearSelection()
	{
		if (this.toggles == null)
		{
			return;
		}
		foreach (KToggle ktoggle in this.toggles)
		{
			ktoggle.Deselect();
			ktoggle.ClearAnimState();
		}
		this.selected = -1;
	}

	// Token: 0x060068B8 RID: 26808 RVA: 0x0027AC6C File Offset: 0x00278E6C
	private void OnClick(int i)
	{
		if (this.onSelect == null)
		{
			return;
		}
		this.selected = i;
		this.onSelect(this.toggleInfo[i]);
		if (!this.toggles[i].isOn)
		{
			this.selected = -1;
		}
		for (int j = 0; j < this.toggles.Count; j++)
		{
			if (j != this.selected)
			{
				this.toggles[j].isOn = false;
			}
		}
	}

	// Token: 0x060068B9 RID: 26809 RVA: 0x0027ACEC File Offset: 0x00278EEC
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.toggles == null)
		{
			return;
		}
		if (this.toggleInfo == null)
		{
			return;
		}
		for (int i = 0; i < this.toggleInfo.Count; i++)
		{
			if (this.toggles[i].isActiveAndEnabled)
			{
				global::Action hotKey = this.toggleInfo[i].hotKey;
				if (hotKey != global::Action.NumActions && e.TryConsume(hotKey))
				{
					if (this.selected != i || this.repeatKeyDownToggles)
					{
						this.toggles[i].Click();
						if (this.selected == i)
						{
							this.toggles[i].Deselect();
						}
						this.selected = i;
						return;
					}
					break;
				}
			}
		}
	}

	// Token: 0x060068BA RID: 26810 RVA: 0x0027AD9E File Offset: 0x00278F9E
	public virtual void Close()
	{
		this.ClearSelection();
		this.Show(false);
	}

	// Token: 0x040047F1 RID: 18417
	[SerializeField]
	private Transform toggleParent;

	// Token: 0x040047F2 RID: 18418
	[SerializeField]
	private KToggle prefab;

	// Token: 0x040047F3 RID: 18419
	[SerializeField]
	private ToggleGroup group;

	// Token: 0x040047F4 RID: 18420
	[SerializeField]
	private Sprite[] icons;

	// Token: 0x040047F5 RID: 18421
	[SerializeField]
	public TextStyleSetting ToggleToolTipTextStyleSetting;

	// Token: 0x040047F6 RID: 18422
	[SerializeField]
	public TextStyleSetting ToggleToolTipHeaderTextStyleSetting;

	// Token: 0x040047F7 RID: 18423
	[SerializeField]
	protected bool repeatKeyDownToggles = true;

	// Token: 0x040047F8 RID: 18424
	protected KToggle currentlySelectedToggle;

	// Token: 0x040047FA RID: 18426
	protected IList<KIconToggleMenu.ToggleInfo> toggleInfo;

	// Token: 0x040047FB RID: 18427
	protected List<KToggle> toggles = new List<KToggle>();

	// Token: 0x040047FC RID: 18428
	private List<KToggle> dontDestroyToggles = new List<KToggle>();

	// Token: 0x040047FD RID: 18429
	protected int selected = -1;

	// Token: 0x02001F6A RID: 8042
	// (Invoke) Token: 0x0600B641 RID: 46657
	public delegate void OnSelect(KIconToggleMenu.ToggleInfo toggleInfo);

	// Token: 0x02001F6B RID: 8043
	public class ToggleInfo
	{
		// Token: 0x0600B644 RID: 46660 RVA: 0x003EFC08 File Offset: 0x003EDE08
		public ToggleInfo(string text, string icon, object user_data = null, global::Action hotkey = global::Action.NumActions, string tooltip = "", string tooltip_header = "")
		{
			this.text = text;
			this.userData = user_data;
			this.icon = icon;
			this.hotKey = hotkey;
			this.tooltip = tooltip;
			this.tooltipHeader = tooltip_header;
			this.getTooltipText = new ToolTip.ComplexTooltipDelegate(this.DefaultGetTooltipText);
		}

		// Token: 0x0600B645 RID: 46661 RVA: 0x003EFC5B File Offset: 0x003EDE5B
		public ToggleInfo(string text, object user_data, global::Action hotkey, Func<Sprite> get_sprite_cb)
		{
			this.text = text;
			this.userData = user_data;
			this.hotKey = hotkey;
			this.getSpriteCB = get_sprite_cb;
		}

		// Token: 0x0600B646 RID: 46662 RVA: 0x003EFC80 File Offset: 0x003EDE80
		public virtual void SetToggle(KToggle toggle)
		{
			this.toggle = toggle;
			toggle.GetComponent<ToolTip>().OnComplexToolTip = this.getTooltipText;
		}

		// Token: 0x0600B647 RID: 46663 RVA: 0x003EFC9C File Offset: 0x003EDE9C
		protected virtual List<global::Tuple<string, TextStyleSetting>> DefaultGetTooltipText()
		{
			List<global::Tuple<string, TextStyleSetting>> list = new List<global::Tuple<string, TextStyleSetting>>();
			if (this.tooltipHeader != null)
			{
				list.Add(new global::Tuple<string, TextStyleSetting>(this.tooltipHeader, ToolTipScreen.Instance.defaultTooltipHeaderStyle));
			}
			list.Add(new global::Tuple<string, TextStyleSetting>(this.tooltip, ToolTipScreen.Instance.defaultTooltipBodyStyle));
			return list;
		}

		// Token: 0x040092CD RID: 37581
		public string text;

		// Token: 0x040092CE RID: 37582
		public object userData;

		// Token: 0x040092CF RID: 37583
		public string icon;

		// Token: 0x040092D0 RID: 37584
		public string tooltip;

		// Token: 0x040092D1 RID: 37585
		public string tooltipHeader;

		// Token: 0x040092D2 RID: 37586
		public KToggle toggle;

		// Token: 0x040092D3 RID: 37587
		public global::Action hotKey;

		// Token: 0x040092D4 RID: 37588
		public ToolTip.ComplexTooltipDelegate getTooltipText;

		// Token: 0x040092D5 RID: 37589
		public Func<Sprite> getSpriteCB;

		// Token: 0x040092D6 RID: 37590
		public KToggle prefabOverride;

		// Token: 0x040092D7 RID: 37591
		public KToggle instanceOverride;
	}
}
