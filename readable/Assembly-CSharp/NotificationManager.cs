using System;
using System.Collections.Generic;

// Token: 0x02000DC7 RID: 3527
public class NotificationManager : KMonoBehaviour
{
	// Token: 0x170007BE RID: 1982
	// (get) Token: 0x06006E30 RID: 28208 RVA: 0x0029B9A3 File Offset: 0x00299BA3
	// (set) Token: 0x06006E31 RID: 28209 RVA: 0x0029B9AA File Offset: 0x00299BAA
	public static NotificationManager Instance { get; private set; }

	// Token: 0x1400002F RID: 47
	// (add) Token: 0x06006E32 RID: 28210 RVA: 0x0029B9B4 File Offset: 0x00299BB4
	// (remove) Token: 0x06006E33 RID: 28211 RVA: 0x0029B9EC File Offset: 0x00299BEC
	public event Action<Notification> notificationAdded;

	// Token: 0x14000030 RID: 48
	// (add) Token: 0x06006E34 RID: 28212 RVA: 0x0029BA24 File Offset: 0x00299C24
	// (remove) Token: 0x06006E35 RID: 28213 RVA: 0x0029BA5C File Offset: 0x00299C5C
	public event Action<Notification> notificationRemoved;

	// Token: 0x06006E36 RID: 28214 RVA: 0x0029BA91 File Offset: 0x00299C91
	protected override void OnPrefabInit()
	{
		Debug.Assert(NotificationManager.Instance == null);
		NotificationManager.Instance = this;
	}

	// Token: 0x06006E37 RID: 28215 RVA: 0x0029BAA9 File Offset: 0x00299CA9
	protected override void OnForcedCleanUp()
	{
		NotificationManager.Instance = null;
	}

	// Token: 0x06006E38 RID: 28216 RVA: 0x0029BAB1 File Offset: 0x00299CB1
	public void AddNotification(Notification notification)
	{
		this.pendingNotifications.Add(notification);
		if (NotificationScreen.Instance != null)
		{
			NotificationScreen.Instance.AddPendingNotification(notification);
		}
	}

	// Token: 0x06006E39 RID: 28217 RVA: 0x0029BAD8 File Offset: 0x00299CD8
	public void RemoveNotification(Notification notification)
	{
		this.pendingNotifications.Remove(notification);
		if (NotificationScreen.Instance != null)
		{
			NotificationScreen.Instance.RemovePendingNotification(notification);
		}
		if (this.notifications.Remove(notification))
		{
			this.notificationRemoved(notification);
		}
	}

	// Token: 0x06006E3A RID: 28218 RVA: 0x0029BB24 File Offset: 0x00299D24
	private void Update()
	{
		int i = 0;
		while (i < this.pendingNotifications.Count)
		{
			if (this.pendingNotifications[i].IsReady())
			{
				this.DoAddNotification(this.pendingNotifications[i]);
				this.pendingNotifications.RemoveAt(i);
			}
			else
			{
				i++;
			}
		}
	}

	// Token: 0x06006E3B RID: 28219 RVA: 0x0029BB7A File Offset: 0x00299D7A
	private void DoAddNotification(Notification notification)
	{
		this.notifications.Add(notification);
		if (this.notificationAdded != null)
		{
			this.notificationAdded(notification);
		}
	}

	// Token: 0x04004B4C RID: 19276
	private List<Notification> pendingNotifications = new List<Notification>();

	// Token: 0x04004B4D RID: 19277
	private List<Notification> notifications = new List<Notification>();
}
