using System;
using UnityEngine;

// Token: 0x02000617 RID: 1559
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Notifier")]
public class Notifier : KMonoBehaviour
{
	// Token: 0x060024BA RID: 9402 RVA: 0x000D34EC File Offset: 0x000D16EC
	protected override void OnPrefabInit()
	{
		Components.Notifiers.Add(this);
	}

	// Token: 0x060024BB RID: 9403 RVA: 0x000D34F9 File Offset: 0x000D16F9
	protected override void OnCleanUp()
	{
		Components.Notifiers.Remove(this);
	}

	// Token: 0x060024BC RID: 9404 RVA: 0x000D3508 File Offset: 0x000D1708
	public void Add(Notification notification, string suffix = "")
	{
		if (KScreenManager.Instance == null)
		{
			return;
		}
		if (this.DisableNotifications)
		{
			return;
		}
		if (DebugHandler.NotificationsDisabled)
		{
			return;
		}
		DebugUtil.DevAssert(notification != null, "Trying to add null notification. It's safe to continue playing, the notification won't be displayed.", null);
		if (notification == null)
		{
			return;
		}
		if (notification.Notifier == null)
		{
			if (this.Selectable != null)
			{
				notification.NotifierName = "• " + this.Selectable.GetName() + suffix;
			}
			else
			{
				notification.NotifierName = "• " + base.name + suffix;
			}
			notification.Notifier = this;
			if (this.AutoClickFocus && notification.clickFocus == null)
			{
				notification.clickFocus = base.transform;
			}
			NotificationManager.Instance.AddNotification(notification);
			notification.GameTime = Time.time;
		}
		else
		{
			DebugUtil.Assert(notification.Notifier == this);
		}
		notification.Time = KTime.Instance.UnscaledGameTime;
	}

	// Token: 0x060024BD RID: 9405 RVA: 0x000D35FD File Offset: 0x000D17FD
	public void Remove(Notification notification)
	{
		if (notification == null)
		{
			return;
		}
		if (notification.Notifier != null)
		{
			notification.Notifier = null;
		}
		if (NotificationManager.Instance != null)
		{
			NotificationManager.Instance.RemoveNotification(notification);
		}
	}

	// Token: 0x04001581 RID: 5505
	[MyCmpGet]
	private KSelectable Selectable;

	// Token: 0x04001582 RID: 5506
	public bool DisableNotifications;

	// Token: 0x04001583 RID: 5507
	public bool AutoClickFocus = true;
}
