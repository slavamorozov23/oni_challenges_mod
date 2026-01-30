using System;
using System.Collections.Generic;

// Token: 0x02000DC3 RID: 3523
public class NotificationAlertBar : KMonoBehaviour
{
	// Token: 0x06006E16 RID: 28182 RVA: 0x0029B420 File Offset: 0x00299620
	public void Init(ManagementMenuNotification notification)
	{
		this.notification = notification;
		this.thisButton.onClick += this.OnThisButtonClicked;
		this.background.colorStyleSetting = this.alertColorStyle[(int)notification.valence];
		this.background.ApplyColorStyleSetting();
		this.text.text = notification.titleText;
		this.tooltip.SetSimpleTooltip(notification.ToolTip(null, notification.tooltipData));
		this.muteButton.onClick += this.OnMuteButtonClicked;
	}

	// Token: 0x06006E17 RID: 28183 RVA: 0x0029B4B8 File Offset: 0x002996B8
	private void OnThisButtonClicked()
	{
		NotificationHighlightController componentInParent = base.GetComponentInParent<NotificationHighlightController>();
		if (componentInParent != null)
		{
			componentInParent.SetActiveTarget(this.notification);
			return;
		}
		this.notification.View();
	}

	// Token: 0x06006E18 RID: 28184 RVA: 0x0029B4ED File Offset: 0x002996ED
	private void OnMuteButtonClicked()
	{
	}

	// Token: 0x04004B3B RID: 19259
	public ManagementMenuNotification notification;

	// Token: 0x04004B3C RID: 19260
	public KButton thisButton;

	// Token: 0x04004B3D RID: 19261
	public KImage background;

	// Token: 0x04004B3E RID: 19262
	public LocText text;

	// Token: 0x04004B3F RID: 19263
	public ToolTip tooltip;

	// Token: 0x04004B40 RID: 19264
	public KButton muteButton;

	// Token: 0x04004B41 RID: 19265
	public List<ColorStyleSetting> alertColorStyle;
}
