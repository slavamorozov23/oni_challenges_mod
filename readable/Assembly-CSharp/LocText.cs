using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000D62 RID: 3426
public class LocText : TextMeshProUGUI
{
	// Token: 0x06006A0D RID: 27149 RVA: 0x002819A4 File Offset: 0x0027FBA4
	protected override void OnEnable()
	{
		base.OnEnable();
	}

	// Token: 0x17000799 RID: 1945
	// (get) Token: 0x06006A0E RID: 27150 RVA: 0x002819AC File Offset: 0x0027FBAC
	// (set) Token: 0x06006A0F RID: 27151 RVA: 0x002819B4 File Offset: 0x0027FBB4
	public bool AllowLinks
	{
		get
		{
			return this.allowLinksInternal;
		}
		set
		{
			this.allowLinksInternal = value;
			this.RefreshLinkHandler();
			this.raycastTarget = (this.raycastTarget || this.allowLinksInternal);
		}
	}

	// Token: 0x06006A10 RID: 27152 RVA: 0x002819DC File Offset: 0x0027FBDC
	[ContextMenu("Apply Settings")]
	public void ApplySettings()
	{
		if (this.key != "" && Application.isPlaying)
		{
			StringKey key = new StringKey(this.key);
			this.text = Strings.Get(key);
		}
		if (this.textStyleSetting != null)
		{
			SetTextStyleSetting.ApplyStyle(this, this.textStyleSetting);
		}
	}

	// Token: 0x06006A11 RID: 27153 RVA: 0x00281A3C File Offset: 0x0027FC3C
	private new void Awake()
	{
		base.Awake();
		if (!Application.isPlaying)
		{
			return;
		}
		if (this.key != "")
		{
			StringEntry stringEntry = Strings.Get(new StringKey(this.key));
			this.text = stringEntry.String;
		}
		this.text = Localization.Fixup(this.text);
		base.isRightToLeftText = Localization.IsRightToLeft;
		KInputManager.InputChange.AddListener(new UnityAction(this.RefreshText));
		SetTextStyleSetting setTextStyleSetting = base.gameObject.GetComponent<SetTextStyleSetting>();
		if (setTextStyleSetting == null)
		{
			setTextStyleSetting = base.gameObject.AddComponent<SetTextStyleSetting>();
		}
		if (!this.allowOverride)
		{
			setTextStyleSetting.SetStyle(this.textStyleSetting);
		}
		this.textLinkHandler = base.GetComponent<TextLinkHandler>();
	}

	// Token: 0x06006A12 RID: 27154 RVA: 0x00281AF9 File Offset: 0x0027FCF9
	private new void Start()
	{
		base.Start();
		this.RefreshLinkHandler();
	}

	// Token: 0x06006A13 RID: 27155 RVA: 0x00281B07 File Offset: 0x0027FD07
	private new void OnDestroy()
	{
		KInputManager.InputChange.RemoveListener(new UnityAction(this.RefreshText));
		base.OnDestroy();
	}

	// Token: 0x06006A14 RID: 27156 RVA: 0x00281B25 File Offset: 0x0027FD25
	public override void SetLayoutDirty()
	{
		if (this.staticLayout)
		{
			return;
		}
		base.SetLayoutDirty();
	}

	// Token: 0x06006A15 RID: 27157 RVA: 0x00281B36 File Offset: 0x0027FD36
	public void SetLinkOverrideAction(Func<string, bool> action)
	{
		this.RefreshLinkHandler();
		if (this.textLinkHandler != null)
		{
			this.textLinkHandler.overrideLinkAction = action;
		}
	}

	// Token: 0x1700079A RID: 1946
	// (get) Token: 0x06006A16 RID: 27158 RVA: 0x00281B58 File Offset: 0x0027FD58
	// (set) Token: 0x06006A17 RID: 27159 RVA: 0x00281B60 File Offset: 0x0027FD60
	public override string text
	{
		get
		{
			return base.text;
		}
		set
		{
			base.text = this.FilterInput(value);
		}
	}

	// Token: 0x06006A18 RID: 27160 RVA: 0x00281B6F File Offset: 0x0027FD6F
	public override void SetText(string text)
	{
		text = this.FilterInput(text);
		base.SetText(text);
	}

	// Token: 0x06006A19 RID: 27161 RVA: 0x00281B81 File Offset: 0x0027FD81
	private string FilterInput(string input)
	{
		if (input != null)
		{
			string text = LocText.ParseText(input);
			if (text != input)
			{
				this.originalString = input;
			}
			else
			{
				this.originalString = string.Empty;
			}
			input = text;
		}
		if (this.AllowLinks)
		{
			return LocText.ModifyLinkStrings(input);
		}
		return input;
	}

	// Token: 0x06006A1A RID: 27162 RVA: 0x00281BBC File Offset: 0x0027FDBC
	public static string ParseText(string input)
	{
		string pattern = "\\{Hotkey/(\\w+)\\}";
		string input2 = Regex.Replace(input, pattern, delegate(Match m)
		{
			string value = m.Groups[1].Value;
			global::Action action;
			if (LocText.ActionLookup.TryGetValue(value, out action))
			{
				return GameUtil.GetHotkeyString(action);
			}
			return m.Value;
		});
		pattern = "\\(ClickType/(\\w+)\\)";
		return Regex.Replace(input2, pattern, delegate(Match m)
		{
			string value = m.Groups[1].Value;
			Pair<LocString, LocString> pair;
			if (!LocText.ClickLookup.TryGetValue(value, out pair))
			{
				return m.Value;
			}
			if (KInputManager.currentControllerIsGamepad)
			{
				return pair.first.ToString();
			}
			return pair.second.ToString();
		});
	}

	// Token: 0x06006A1B RID: 27163 RVA: 0x00281C20 File Offset: 0x0027FE20
	private void RefreshText()
	{
		if (this.originalString != string.Empty)
		{
			this.SetText(this.originalString);
		}
	}

	// Token: 0x06006A1C RID: 27164 RVA: 0x00281C40 File Offset: 0x0027FE40
	protected override void GenerateTextMesh()
	{
		base.GenerateTextMesh();
	}

	// Token: 0x06006A1D RID: 27165 RVA: 0x00281C48 File Offset: 0x0027FE48
	internal void SwapFont(TMP_FontAsset font, bool isRightToLeft)
	{
		base.font = font;
		if (this.key != "")
		{
			StringEntry stringEntry = Strings.Get(new StringKey(this.key));
			this.text = stringEntry.String;
		}
		this.text = Localization.Fixup(this.text);
		base.isRightToLeftText = isRightToLeft;
	}

	// Token: 0x06006A1E RID: 27166 RVA: 0x00281CA4 File Offset: 0x0027FEA4
	private static string ModifyLinkStrings(string input)
	{
		if (input == null || input.IndexOf("<b><style=\"KLink\">") != -1)
		{
			return input;
		}
		StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
		stringBuilder.Append(input);
		stringBuilder.Replace("<link=\"", LocText.combinedPrefix);
		stringBuilder.Replace("</link>", LocText.combinedSuffix);
		return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
	}

	// Token: 0x06006A1F RID: 27167 RVA: 0x00281CF8 File Offset: 0x0027FEF8
	private void RefreshLinkHandler()
	{
		if (this.textLinkHandler == null && this.allowLinksInternal)
		{
			this.textLinkHandler = base.GetComponent<TextLinkHandler>();
			if (this.textLinkHandler == null)
			{
				this.textLinkHandler = base.gameObject.AddComponent<TextLinkHandler>();
			}
		}
		else if (!this.allowLinksInternal && this.textLinkHandler != null)
		{
			UnityEngine.Object.Destroy(this.textLinkHandler);
			this.textLinkHandler = null;
		}
		if (this.textLinkHandler != null)
		{
			this.textLinkHandler.CheckMouseOver();
		}
	}

	// Token: 0x040048DC RID: 18652
	public string key;

	// Token: 0x040048DD RID: 18653
	public TextStyleSetting textStyleSetting;

	// Token: 0x040048DE RID: 18654
	public bool allowOverride;

	// Token: 0x040048DF RID: 18655
	public bool staticLayout;

	// Token: 0x040048E0 RID: 18656
	private TextLinkHandler textLinkHandler;

	// Token: 0x040048E1 RID: 18657
	private string originalString = string.Empty;

	// Token: 0x040048E2 RID: 18658
	[SerializeField]
	private bool allowLinksInternal;

	// Token: 0x040048E3 RID: 18659
	private static readonly Dictionary<string, global::Action> ActionLookup = Enum.GetNames(typeof(global::Action)).ToDictionary((string x) => x, (string x) => (global::Action)Enum.Parse(typeof(global::Action), x), StringComparer.OrdinalIgnoreCase);

	// Token: 0x040048E4 RID: 18660
	private static readonly Dictionary<string, Pair<LocString, LocString>> ClickLookup = new Dictionary<string, Pair<LocString, LocString>>
	{
		{
			UI.ClickType.Click.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESS, UI.CONTROLS.CLICK)
		},
		{
			UI.ClickType.Clickable.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSABLE, UI.CONTROLS.CLICKABLE)
		},
		{
			UI.ClickType.Clicked.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSED, UI.CONTROLS.CLICKED)
		},
		{
			UI.ClickType.Clicking.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSING, UI.CONTROLS.CLICKING)
		},
		{
			UI.ClickType.Clicks.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSES, UI.CONTROLS.CLICKS)
		},
		{
			UI.ClickType.click.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSLOWER, UI.CONTROLS.CLICKLOWER)
		},
		{
			UI.ClickType.clickable.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSABLELOWER, UI.CONTROLS.CLICKABLELOWER)
		},
		{
			UI.ClickType.clicked.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSEDLOWER, UI.CONTROLS.CLICKEDLOWER)
		},
		{
			UI.ClickType.clicking.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSINGLOWER, UI.CONTROLS.CLICKINGLOWER)
		},
		{
			UI.ClickType.clicks.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSESLOWER, UI.CONTROLS.CLICKSLOWER)
		},
		{
			UI.ClickType.CLICK.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSUPPER, UI.CONTROLS.CLICKUPPER)
		},
		{
			UI.ClickType.CLICKABLE.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSABLEUPPER, UI.CONTROLS.CLICKABLEUPPER)
		},
		{
			UI.ClickType.CLICKED.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSEDUPPER, UI.CONTROLS.CLICKEDUPPER)
		},
		{
			UI.ClickType.CLICKING.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSINGUPPER, UI.CONTROLS.CLICKINGUPPER)
		},
		{
			UI.ClickType.CLICKS.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSESUPPER, UI.CONTROLS.CLICKSUPPER)
		}
	};

	// Token: 0x040048E5 RID: 18661
	private const string linkPrefix_open = "<link=\"";

	// Token: 0x040048E6 RID: 18662
	private const string linkSuffix = "</link>";

	// Token: 0x040048E7 RID: 18663
	private const string linkColorPrefix = "<b><style=\"KLink\">";

	// Token: 0x040048E8 RID: 18664
	private const string linkColorSuffix = "</style></b>";

	// Token: 0x040048E9 RID: 18665
	private static readonly string combinedPrefix = "<b><style=\"KLink\"><link=\"";

	// Token: 0x040048EA RID: 18666
	private static readonly string combinedSuffix = "</style></b></link>";
}
