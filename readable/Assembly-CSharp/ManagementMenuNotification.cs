using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000614 RID: 1556
public class ManagementMenuNotification : Notification
{
	// Token: 0x17000182 RID: 386
	// (get) Token: 0x0600249A RID: 9370 RVA: 0x000D30D3 File Offset: 0x000D12D3
	// (set) Token: 0x0600249B RID: 9371 RVA: 0x000D30DB File Offset: 0x000D12DB
	public bool hasBeenViewed { get; private set; }

	// Token: 0x17000183 RID: 387
	// (get) Token: 0x0600249C RID: 9372 RVA: 0x000D30E4 File Offset: 0x000D12E4
	// (set) Token: 0x0600249D RID: 9373 RVA: 0x000D30EC File Offset: 0x000D12EC
	public string highlightTarget { get; set; }

	// Token: 0x0600249E RID: 9374 RVA: 0x000D30F8 File Offset: 0x000D12F8
	public ManagementMenuNotification(global::Action targetMenu, NotificationValence valence, string highlightTarget, string title, NotificationType type, Func<List<Notification>, object, string> tooltip = null, object tooltip_data = null, bool expires = true, float delay = 0f, Notification.ClickCallback custom_click_callback = null, object custom_click_data = null, Transform click_focus = null, bool volume_attenuation = true) : base(title, type, tooltip, tooltip_data, expires, delay, custom_click_callback, custom_click_data, click_focus, volume_attenuation, false, false)
	{
		this.targetMenu = targetMenu;
		this.valence = valence;
		this.highlightTarget = highlightTarget;
	}

	// Token: 0x0600249F RID: 9375 RVA: 0x000D3136 File Offset: 0x000D1336
	public void View()
	{
		this.hasBeenViewed = true;
		ManagementMenu.Instance.notificationDisplayer.NotificationWasViewed(this);
	}

	// Token: 0x04001569 RID: 5481
	public global::Action targetMenu;

	// Token: 0x0400156A RID: 5482
	public NotificationValence valence;
}
