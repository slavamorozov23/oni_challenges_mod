using System;
using System.Collections.Generic;

// Token: 0x02000DC4 RID: 3524
public abstract class NotificationDisplayer : KMonoBehaviour
{
	// Token: 0x06006E1A RID: 28186 RVA: 0x0029B4F7 File Offset: 0x002996F7
	protected override void OnSpawn()
	{
		this.displayedNotifications = new List<Notification>();
		NotificationManager.Instance.notificationAdded += this.NotificationAdded;
		NotificationManager.Instance.notificationRemoved += this.NotificationRemoved;
	}

	// Token: 0x06006E1B RID: 28187 RVA: 0x0029B530 File Offset: 0x00299730
	public void NotificationAdded(Notification notification)
	{
		if (this.ShouldDisplayNotification(notification))
		{
			this.displayedNotifications.Add(notification);
			this.OnNotificationAdded(notification);
		}
	}

	// Token: 0x06006E1C RID: 28188
	protected abstract void OnNotificationAdded(Notification notification);

	// Token: 0x06006E1D RID: 28189 RVA: 0x0029B54E File Offset: 0x0029974E
	public void NotificationRemoved(Notification notification)
	{
		if (this.displayedNotifications.Contains(notification))
		{
			this.displayedNotifications.Remove(notification);
			this.OnNotificationRemoved(notification);
		}
	}

	// Token: 0x06006E1E RID: 28190
	protected abstract void OnNotificationRemoved(Notification notification);

	// Token: 0x06006E1F RID: 28191
	protected abstract bool ShouldDisplayNotification(Notification notification);

	// Token: 0x04004B42 RID: 19266
	protected List<Notification> displayedNotifications;
}
