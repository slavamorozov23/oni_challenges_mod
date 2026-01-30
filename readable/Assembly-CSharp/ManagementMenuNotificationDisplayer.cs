using System;
using System.Collections.Generic;

// Token: 0x02000DC1 RID: 3521
public class ManagementMenuNotificationDisplayer : NotificationDisplayer
{
	// Token: 0x170007BD RID: 1981
	// (get) Token: 0x06006E06 RID: 28166 RVA: 0x0029B27E File Offset: 0x0029947E
	// (set) Token: 0x06006E07 RID: 28167 RVA: 0x0029B286 File Offset: 0x00299486
	public List<ManagementMenuNotification> displayedManagementMenuNotifications { get; private set; }

	// Token: 0x1400002E RID: 46
	// (add) Token: 0x06006E08 RID: 28168 RVA: 0x0029B290 File Offset: 0x00299490
	// (remove) Token: 0x06006E09 RID: 28169 RVA: 0x0029B2C8 File Offset: 0x002994C8
	public event System.Action onNotificationsChanged;

	// Token: 0x06006E0A RID: 28170 RVA: 0x0029B2FD File Offset: 0x002994FD
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.displayedManagementMenuNotifications = new List<ManagementMenuNotification>();
	}

	// Token: 0x06006E0B RID: 28171 RVA: 0x0029B310 File Offset: 0x00299510
	public void NotificationWasViewed(ManagementMenuNotification notification)
	{
		this.onNotificationsChanged();
	}

	// Token: 0x06006E0C RID: 28172 RVA: 0x0029B31D File Offset: 0x0029951D
	protected override void OnNotificationAdded(Notification notification)
	{
		this.displayedManagementMenuNotifications.Add(notification as ManagementMenuNotification);
		this.onNotificationsChanged();
	}

	// Token: 0x06006E0D RID: 28173 RVA: 0x0029B33B File Offset: 0x0029953B
	protected override void OnNotificationRemoved(Notification notification)
	{
		this.displayedManagementMenuNotifications.Remove(notification as ManagementMenuNotification);
		this.onNotificationsChanged();
	}

	// Token: 0x06006E0E RID: 28174 RVA: 0x0029B35A File Offset: 0x0029955A
	protected override bool ShouldDisplayNotification(Notification notification)
	{
		return notification is ManagementMenuNotification;
	}

	// Token: 0x06006E0F RID: 28175 RVA: 0x0029B368 File Offset: 0x00299568
	public List<ManagementMenuNotification> GetNotificationsForAction(global::Action hotKey)
	{
		List<ManagementMenuNotification> list = new List<ManagementMenuNotification>();
		foreach (ManagementMenuNotification managementMenuNotification in this.displayedManagementMenuNotifications)
		{
			if (managementMenuNotification.targetMenu == hotKey)
			{
				list.Add(managementMenuNotification);
			}
		}
		return list;
	}
}
