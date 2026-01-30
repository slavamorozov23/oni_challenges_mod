using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000BDE RID: 3038
public class StatusItem : Resource
{
	// Token: 0x06005AF0 RID: 23280 RVA: 0x0020F1C0 File Offset: 0x0020D3C0
	private StatusItem(string id, string composed_prefix) : base(id, Strings.Get(composed_prefix + ".NAME"))
	{
		this.composedPrefix = composed_prefix;
		this.tooltipText = Strings.Get(composed_prefix + ".TOOLTIP");
	}

	// Token: 0x06005AF1 RID: 23281 RVA: 0x0020F214 File Offset: 0x0020D414
	private void SetIcon(string icon, StatusItem.IconType icon_type, NotificationType notification_type, bool allow_multiples, HashedString render_overlay, bool show_world_icon = true, int status_overlays = 129022, Func<string, object, string> resolve_string_callback = null)
	{
		switch (icon_type)
		{
		case StatusItem.IconType.Info:
			icon = "dash";
			break;
		case StatusItem.IconType.Exclamation:
			icon = "status_item_exclamation";
			break;
		}
		this.iconName = icon;
		this.notificationType = notification_type;
		this.sprite = Assets.GetTintedSprite(icon);
		if (this.sprite == null)
		{
			this.sprite = new TintedSprite();
			this.sprite.sprite = Assets.GetSprite(icon);
			this.sprite.color = new Color(0f, 0f, 0f, 255f);
		}
		this.iconType = icon_type;
		this.allowMultiples = allow_multiples;
		this.render_overlay = render_overlay;
		this.showShowWorldIcon = show_world_icon;
		this.status_overlays = status_overlays;
		this.resolveStringCallback = resolve_string_callback;
		if (this.sprite == null)
		{
			global::Debug.LogWarning("Status item '" + this.Id + "' references a missing icon: " + icon);
		}
	}

	// Token: 0x06005AF2 RID: 23282 RVA: 0x0020F300 File Offset: 0x0020D500
	public StatusItem(string id, string prefix, string icon, StatusItem.IconType icon_type, NotificationType notification_type, bool allow_multiples, HashedString render_overlay, bool showWorldIcon = true, int status_overlays = 129022, Func<string, object, string> resolve_string_callback = null) : this(id, "STRINGS." + prefix + ".STATUSITEMS." + id.ToUpper())
	{
		this.SetIcon(icon, icon_type, notification_type, allow_multiples, render_overlay, showWorldIcon, status_overlays, resolve_string_callback);
	}

	// Token: 0x06005AF3 RID: 23283 RVA: 0x0020F340 File Offset: 0x0020D540
	public StatusItem(string id, string name, string tooltip, string icon, StatusItem.IconType icon_type, NotificationType notification_type, bool allow_multiples, HashedString render_overlay, int status_overlays = 129022, bool showWorldIcon = true, Func<string, object, string> resolve_string_callback = null) : base(id, name)
	{
		this.tooltipText = tooltip;
		this.SetIcon(icon, icon_type, notification_type, allow_multiples, render_overlay, showWorldIcon, status_overlays, resolve_string_callback);
	}

	// Token: 0x06005AF4 RID: 23284 RVA: 0x0020F37C File Offset: 0x0020D57C
	public void AddNotification(string sound_path = null, string notification_text = null, string notification_tooltip = null)
	{
		this.shouldNotify = true;
		if (sound_path == null)
		{
			if (this.notificationType == NotificationType.Bad)
			{
				this.soundPath = "Warning";
			}
			else
			{
				this.soundPath = "Notification";
			}
		}
		else
		{
			this.soundPath = sound_path;
		}
		if (notification_text != null)
		{
			this.notificationText = notification_text;
		}
		else
		{
			DebugUtil.Assert(this.composedPrefix != null, "When adding a notification, either set the status prefix or specify strings!");
			this.notificationText = Strings.Get(this.composedPrefix + ".NOTIFICATION_NAME");
		}
		if (notification_tooltip != null)
		{
			this.notificationTooltipText = notification_tooltip;
			return;
		}
		DebugUtil.Assert(this.composedPrefix != null, "When adding a notification, either set the status prefix or specify strings!");
		this.notificationTooltipText = Strings.Get(this.composedPrefix + ".NOTIFICATION_TOOLTIP");
	}

	// Token: 0x06005AF5 RID: 23285 RVA: 0x0020F43A File Offset: 0x0020D63A
	public virtual string GetName(object data)
	{
		return this.ResolveString(this.Name, data);
	}

	// Token: 0x06005AF6 RID: 23286 RVA: 0x0020F449 File Offset: 0x0020D649
	public virtual string GetTooltip(object data)
	{
		return this.ResolveTooltip(this.tooltipText, data);
	}

	// Token: 0x06005AF7 RID: 23287 RVA: 0x0020F458 File Offset: 0x0020D658
	private string ResolveString(string str, object data)
	{
		if (this.resolveStringCallback != null && (data != null || this.resolveStringCallback_shouldStillCallIfDataIsNull))
		{
			return this.resolveStringCallback(str, data);
		}
		return str;
	}

	// Token: 0x06005AF8 RID: 23288 RVA: 0x0020F47C File Offset: 0x0020D67C
	private string ResolveTooltip(string str, object data)
	{
		if (data != null)
		{
			if (this.resolveTooltipCallback != null)
			{
				return this.resolveTooltipCallback(str, data);
			}
			if (this.resolveStringCallback != null)
			{
				return this.resolveStringCallback(str, data);
			}
		}
		else
		{
			if (this.resolveStringCallback_shouldStillCallIfDataIsNull && this.resolveStringCallback != null)
			{
				return this.resolveStringCallback(str, data);
			}
			if (this.resolveTooltipCallback_shouldStillCallIfDataIsNull && this.resolveTooltipCallback != null)
			{
				return this.resolveTooltipCallback(str, data);
			}
		}
		return str;
	}

	// Token: 0x06005AF9 RID: 23289 RVA: 0x0020F4F5 File Offset: 0x0020D6F5
	public bool ShouldShowIcon()
	{
		return this.iconType == StatusItem.IconType.Custom && this.showShowWorldIcon;
	}

	// Token: 0x06005AFA RID: 23290 RVA: 0x0020F508 File Offset: 0x0020D708
	public virtual void ShowToolTip(ToolTip tooltip_widget, object data, TextStyleSetting property_style)
	{
		tooltip_widget.ClearMultiStringTooltip();
		string tooltip = this.GetTooltip(data);
		tooltip_widget.AddMultiStringTooltip(tooltip, property_style);
	}

	// Token: 0x06005AFB RID: 23291 RVA: 0x0020F52B File Offset: 0x0020D72B
	public void SetIcon(Image image, object data)
	{
		if (this.sprite == null)
		{
			return;
		}
		image.color = this.sprite.color;
		image.sprite = this.sprite.sprite;
	}

	// Token: 0x06005AFC RID: 23292 RVA: 0x0020F558 File Offset: 0x0020D758
	public bool UseConditionalCallback(HashedString overlay, Transform transform)
	{
		return overlay != OverlayModes.None.ID && this.conditionalOverlayCallback != null && this.conditionalOverlayCallback(overlay, transform);
	}

	// Token: 0x06005AFD RID: 23293 RVA: 0x0020F57E File Offset: 0x0020D77E
	public StatusItem SetResolveStringCallback(Func<string, object, string> cb)
	{
		this.resolveStringCallback = cb;
		return this;
	}

	// Token: 0x06005AFE RID: 23294 RVA: 0x0020F588 File Offset: 0x0020D788
	public void OnClick(object data)
	{
		if (this.statusItemClickCallback != null)
		{
			this.statusItemClickCallback(data);
		}
	}

	// Token: 0x06005AFF RID: 23295 RVA: 0x0020F5A0 File Offset: 0x0020D7A0
	public static StatusItem.StatusItemOverlays GetStatusItemOverlayBySimViewMode(HashedString mode)
	{
		StatusItem.StatusItemOverlays result;
		if (!StatusItem.overlayBitfieldMap.TryGetValue(mode, out result))
		{
			string str = "ViewMode ";
			HashedString hashedString = mode;
			global::Debug.LogWarning(str + hashedString.ToString() + " has no StatusItemOverlay value");
			result = StatusItem.StatusItemOverlays.None;
		}
		return result;
	}

	// Token: 0x04003C94 RID: 15508
	public string tooltipText;

	// Token: 0x04003C95 RID: 15509
	public string notificationText;

	// Token: 0x04003C96 RID: 15510
	public string notificationTooltipText;

	// Token: 0x04003C97 RID: 15511
	public string soundPath;

	// Token: 0x04003C98 RID: 15512
	public string iconName;

	// Token: 0x04003C99 RID: 15513
	public bool unique;

	// Token: 0x04003C9A RID: 15514
	public TintedSprite sprite;

	// Token: 0x04003C9B RID: 15515
	public bool shouldNotify;

	// Token: 0x04003C9C RID: 15516
	public StatusItem.IconType iconType;

	// Token: 0x04003C9D RID: 15517
	public NotificationType notificationType;

	// Token: 0x04003C9E RID: 15518
	public Notification.ClickCallback notificationClickCallback;

	// Token: 0x04003C9F RID: 15519
	public Func<string, object, string> resolveStringCallback;

	// Token: 0x04003CA0 RID: 15520
	public Func<string, object, string> resolveTooltipCallback;

	// Token: 0x04003CA1 RID: 15521
	public bool resolveStringCallback_shouldStillCallIfDataIsNull;

	// Token: 0x04003CA2 RID: 15522
	public bool resolveTooltipCallback_shouldStillCallIfDataIsNull;

	// Token: 0x04003CA3 RID: 15523
	public bool allowMultiples;

	// Token: 0x04003CA4 RID: 15524
	public Func<HashedString, object, bool> conditionalOverlayCallback;

	// Token: 0x04003CA5 RID: 15525
	public HashedString render_overlay;

	// Token: 0x04003CA6 RID: 15526
	public int status_overlays;

	// Token: 0x04003CA7 RID: 15527
	public Action<object> statusItemClickCallback;

	// Token: 0x04003CA8 RID: 15528
	private string composedPrefix;

	// Token: 0x04003CA9 RID: 15529
	private bool showShowWorldIcon = true;

	// Token: 0x04003CAA RID: 15530
	public bool showInHoverCardOnly;

	// Token: 0x04003CAB RID: 15531
	public const int ALL_OVERLAYS = 129022;

	// Token: 0x04003CAC RID: 15532
	private static Dictionary<HashedString, StatusItem.StatusItemOverlays> overlayBitfieldMap = new Dictionary<HashedString, StatusItem.StatusItemOverlays>
	{
		{
			OverlayModes.None.ID,
			StatusItem.StatusItemOverlays.None
		},
		{
			OverlayModes.Power.ID,
			StatusItem.StatusItemOverlays.PowerMap
		},
		{
			OverlayModes.Temperature.ID,
			StatusItem.StatusItemOverlays.Temperature
		},
		{
			OverlayModes.ThermalConductivity.ID,
			StatusItem.StatusItemOverlays.ThermalComfort
		},
		{
			OverlayModes.Light.ID,
			StatusItem.StatusItemOverlays.Light
		},
		{
			OverlayModes.LiquidConduits.ID,
			StatusItem.StatusItemOverlays.LiquidPlumbing
		},
		{
			OverlayModes.GasConduits.ID,
			StatusItem.StatusItemOverlays.GasPlumbing
		},
		{
			OverlayModes.SolidConveyor.ID,
			StatusItem.StatusItemOverlays.Conveyor
		},
		{
			OverlayModes.Decor.ID,
			StatusItem.StatusItemOverlays.Decor
		},
		{
			OverlayModes.Disease.ID,
			StatusItem.StatusItemOverlays.Pathogens
		},
		{
			OverlayModes.Crop.ID,
			StatusItem.StatusItemOverlays.Farming
		},
		{
			OverlayModes.Rooms.ID,
			StatusItem.StatusItemOverlays.Rooms
		},
		{
			OverlayModes.Suit.ID,
			StatusItem.StatusItemOverlays.Suits
		},
		{
			OverlayModes.Logic.ID,
			StatusItem.StatusItemOverlays.Logic
		},
		{
			OverlayModes.Oxygen.ID,
			StatusItem.StatusItemOverlays.None
		},
		{
			OverlayModes.TileMode.ID,
			StatusItem.StatusItemOverlays.None
		},
		{
			OverlayModes.Radiation.ID,
			StatusItem.StatusItemOverlays.Radiation
		}
	};

	// Token: 0x02001D6F RID: 7535
	public enum IconType
	{
		// Token: 0x04008B3E RID: 35646
		Info,
		// Token: 0x04008B3F RID: 35647
		Exclamation,
		// Token: 0x04008B40 RID: 35648
		Custom
	}

	// Token: 0x02001D70 RID: 7536
	[Flags]
	public enum StatusItemOverlays
	{
		// Token: 0x04008B42 RID: 35650
		None = 2,
		// Token: 0x04008B43 RID: 35651
		PowerMap = 4,
		// Token: 0x04008B44 RID: 35652
		Temperature = 8,
		// Token: 0x04008B45 RID: 35653
		ThermalComfort = 16,
		// Token: 0x04008B46 RID: 35654
		Light = 32,
		// Token: 0x04008B47 RID: 35655
		LiquidPlumbing = 64,
		// Token: 0x04008B48 RID: 35656
		GasPlumbing = 128,
		// Token: 0x04008B49 RID: 35657
		Decor = 256,
		// Token: 0x04008B4A RID: 35658
		Pathogens = 512,
		// Token: 0x04008B4B RID: 35659
		Farming = 1024,
		// Token: 0x04008B4C RID: 35660
		Rooms = 4096,
		// Token: 0x04008B4D RID: 35661
		Suits = 8192,
		// Token: 0x04008B4E RID: 35662
		Logic = 16384,
		// Token: 0x04008B4F RID: 35663
		Conveyor = 32768,
		// Token: 0x04008B50 RID: 35664
		Radiation = 65536
	}
}
