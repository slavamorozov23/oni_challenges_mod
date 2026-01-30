using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x02000615 RID: 1557
public class Notification
{
	// Token: 0x17000184 RID: 388
	// (get) Token: 0x060024A0 RID: 9376 RVA: 0x000D314F File Offset: 0x000D134F
	// (set) Token: 0x060024A1 RID: 9377 RVA: 0x000D3157 File Offset: 0x000D1357
	public NotificationType Type { get; set; }

	// Token: 0x17000185 RID: 389
	// (get) Token: 0x060024A2 RID: 9378 RVA: 0x000D3160 File Offset: 0x000D1360
	// (set) Token: 0x060024A3 RID: 9379 RVA: 0x000D3168 File Offset: 0x000D1368
	public Notifier Notifier { get; set; }

	// Token: 0x17000186 RID: 390
	// (get) Token: 0x060024A4 RID: 9380 RVA: 0x000D3171 File Offset: 0x000D1371
	// (set) Token: 0x060024A5 RID: 9381 RVA: 0x000D3179 File Offset: 0x000D1379
	public Transform clickFocus { get; set; }

	// Token: 0x17000187 RID: 391
	// (get) Token: 0x060024A6 RID: 9382 RVA: 0x000D3182 File Offset: 0x000D1382
	// (set) Token: 0x060024A7 RID: 9383 RVA: 0x000D318A File Offset: 0x000D138A
	public float Time { get; set; }

	// Token: 0x17000188 RID: 392
	// (get) Token: 0x060024A8 RID: 9384 RVA: 0x000D3193 File Offset: 0x000D1393
	// (set) Token: 0x060024A9 RID: 9385 RVA: 0x000D319B File Offset: 0x000D139B
	public float GameTime { get; set; }

	// Token: 0x17000189 RID: 393
	// (get) Token: 0x060024AA RID: 9386 RVA: 0x000D31A4 File Offset: 0x000D13A4
	// (set) Token: 0x060024AB RID: 9387 RVA: 0x000D31AC File Offset: 0x000D13AC
	public float Delay { get; set; }

	// Token: 0x1700018A RID: 394
	// (get) Token: 0x060024AC RID: 9388 RVA: 0x000D31B5 File Offset: 0x000D13B5
	// (set) Token: 0x060024AD RID: 9389 RVA: 0x000D31BD File Offset: 0x000D13BD
	public int Idx { get; set; }

	// Token: 0x1700018B RID: 395
	// (get) Token: 0x060024AE RID: 9390 RVA: 0x000D31C6 File Offset: 0x000D13C6
	// (set) Token: 0x060024AF RID: 9391 RVA: 0x000D31CE File Offset: 0x000D13CE
	public Func<List<Notification>, object, string> ToolTip { get; set; }

	// Token: 0x060024B0 RID: 9392 RVA: 0x000D31D7 File Offset: 0x000D13D7
	public bool IsReady()
	{
		return UnityEngine.Time.time >= this.GameTime + this.Delay;
	}

	// Token: 0x1700018C RID: 396
	// (get) Token: 0x060024B1 RID: 9393 RVA: 0x000D31F0 File Offset: 0x000D13F0
	// (set) Token: 0x060024B2 RID: 9394 RVA: 0x000D31F8 File Offset: 0x000D13F8
	public string titleText { get; private set; }

	// Token: 0x1700018D RID: 397
	// (get) Token: 0x060024B3 RID: 9395 RVA: 0x000D3201 File Offset: 0x000D1401
	// (set) Token: 0x060024B4 RID: 9396 RVA: 0x000D3209 File Offset: 0x000D1409
	public string NotifierName
	{
		get
		{
			return this.notifierName;
		}
		set
		{
			this.notifierName = value;
			this.titleText = this.ReplaceTags(this.titleText);
		}
	}

	// Token: 0x060024B5 RID: 9397 RVA: 0x000D3224 File Offset: 0x000D1424
	public Notification(string title, NotificationType type, Func<List<Notification>, object, string> tooltip = null, object tooltip_data = null, bool expires = true, float delay = 0f, Notification.ClickCallback custom_click_callback = null, object custom_click_data = null, Transform click_focus = null, bool volume_attenuation = true, bool clear_on_click = false, bool show_dismiss_button = false)
	{
		this.titleText = title;
		this.Type = type;
		this.ToolTip = tooltip;
		this.tooltipData = tooltip_data;
		this.expires = expires;
		this.Delay = delay;
		this.customClickCallback = custom_click_callback;
		this.customClickData = custom_click_data;
		this.clickFocus = click_focus;
		this.volume_attenuation = volume_attenuation;
		this.clearOnClick = clear_on_click;
		this.showDismissButton = show_dismiss_button;
		int num = this.notificationIncrement;
		this.notificationIncrement = num + 1;
		this.Idx = num;
	}

	// Token: 0x060024B6 RID: 9398 RVA: 0x000D32C0 File Offset: 0x000D14C0
	public void Clear()
	{
		if (this.Notifier != null)
		{
			this.Notifier.Remove(this);
			return;
		}
		NotificationManager.Instance.RemoveNotification(this);
	}

	// Token: 0x060024B7 RID: 9399 RVA: 0x000D32E8 File Offset: 0x000D14E8
	private string ReplaceTags(string text)
	{
		DebugUtil.Assert(text != null);
		int num = text.IndexOf('{');
		int num2 = text.IndexOf('}');
		if (0 <= num && num < num2)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num3 = 0;
			while (0 <= num)
			{
				string value = text.Substring(num3, num - num3);
				stringBuilder.Append(value);
				num2 = text.IndexOf('}', num);
				if (num >= num2)
				{
					break;
				}
				string tag = text.Substring(num + 1, num2 - num - 1);
				string tagDescription = this.GetTagDescription(tag);
				stringBuilder.Append(tagDescription);
				num3 = num2 + 1;
				num = text.IndexOf('{', num2);
			}
			stringBuilder.Append(text.Substring(num3, text.Length - num3));
			return stringBuilder.ToString();
		}
		return text;
	}

	// Token: 0x060024B8 RID: 9400 RVA: 0x000D339C File Offset: 0x000D159C
	private string GetTagDescription(string tag)
	{
		string result;
		if (tag == "NotifierName")
		{
			result = this.notifierName;
		}
		else
		{
			result = "UNKNOWN TAG: " + tag;
		}
		return result;
	}

	// Token: 0x04001575 RID: 5493
	public object tooltipData;

	// Token: 0x04001576 RID: 5494
	public bool expires = true;

	// Token: 0x04001577 RID: 5495
	public bool playSound = true;

	// Token: 0x04001578 RID: 5496
	public bool volume_attenuation = true;

	// Token: 0x04001579 RID: 5497
	public Notification.ClickCallback customClickCallback;

	// Token: 0x0400157A RID: 5498
	public bool clearOnClick;

	// Token: 0x0400157B RID: 5499
	public bool showDismissButton;

	// Token: 0x0400157C RID: 5500
	public object customClickData;

	// Token: 0x0400157D RID: 5501
	public string customNotificationID;

	// Token: 0x0400157E RID: 5502
	private int notificationIncrement;

	// Token: 0x04001580 RID: 5504
	private string notifierName;

	// Token: 0x020014E7 RID: 5351
	// (Invoke) Token: 0x0600917F RID: 37247
	public delegate void ClickCallback(object data);
}
