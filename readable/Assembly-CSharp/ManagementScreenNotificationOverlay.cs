using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000DC2 RID: 3522
public class ManagementScreenNotificationOverlay : KMonoBehaviour
{
	// Token: 0x06006E11 RID: 28177 RVA: 0x0029B3D4 File Offset: 0x002995D4
	protected void OnEnable()
	{
	}

	// Token: 0x06006E12 RID: 28178 RVA: 0x0029B3D6 File Offset: 0x002995D6
	protected override void OnDisable()
	{
	}

	// Token: 0x06006E13 RID: 28179 RVA: 0x0029B3D8 File Offset: 0x002995D8
	private NotificationAlertBar CreateAlertBar(ManagementMenuNotification notification)
	{
		NotificationAlertBar notificationAlertBar = Util.KInstantiateUI<NotificationAlertBar>(this.alertBarPrefab.gameObject, this.alertContainer.gameObject, false);
		notificationAlertBar.Init(notification);
		notificationAlertBar.gameObject.SetActive(true);
		return notificationAlertBar;
	}

	// Token: 0x06006E14 RID: 28180 RVA: 0x0029B409 File Offset: 0x00299609
	private void NotificationsChanged()
	{
	}

	// Token: 0x04004B37 RID: 19255
	public global::Action currentMenu;

	// Token: 0x04004B38 RID: 19256
	public NotificationAlertBar alertBarPrefab;

	// Token: 0x04004B39 RID: 19257
	public RectTransform alertContainer;

	// Token: 0x04004B3A RID: 19258
	private List<NotificationAlertBar> alertBars = new List<NotificationAlertBar>();
}
