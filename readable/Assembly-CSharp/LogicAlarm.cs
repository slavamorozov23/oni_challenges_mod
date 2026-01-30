using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200078D RID: 1933
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/LogicAlarm")]
public class LogicAlarm : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x0600316E RID: 12654 RVA: 0x0011D8C4 File Offset: 0x0011BAC4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicAlarm>(-905833192, LogicAlarm.OnCopySettingsDelegate);
	}

	// Token: 0x0600316F RID: 12655 RVA: 0x0011D8E0 File Offset: 0x0011BAE0
	private void OnCopySettings(object data)
	{
		LogicAlarm component = ((GameObject)data).GetComponent<LogicAlarm>();
		if (component != null)
		{
			this.notificationName = component.notificationName;
			this.notificationType = component.notificationType;
			this.pauseOnNotify = component.pauseOnNotify;
			this.zoomOnNotify = component.zoomOnNotify;
			this.cooldown = component.cooldown;
			this.notificationTooltip = component.notificationTooltip;
		}
	}

	// Token: 0x06003170 RID: 12656 RVA: 0x0011D94C File Offset: 0x0011BB4C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.notifier = base.gameObject.AddComponent<Notifier>();
		base.Subscribe<LogicAlarm>(-801688580, LogicAlarm.OnLogicValueChangedDelegate);
		if (string.IsNullOrEmpty(this.notificationName))
		{
			this.notificationName = UI.UISIDESCREENS.LOGICALARMSIDESCREEN.NAME_DEFAULT;
		}
		if (string.IsNullOrEmpty(this.notificationTooltip))
		{
			this.notificationTooltip = UI.UISIDESCREENS.LOGICALARMSIDESCREEN.TOOLTIP_DEFAULT;
		}
		this.UpdateVisualState();
		this.UpdateNotification(false);
	}

	// Token: 0x06003171 RID: 12657 RVA: 0x0011D9C8 File Offset: 0x0011BBC8
	private void UpdateVisualState()
	{
		base.GetComponent<KBatchedAnimController>().Play(this.wasOn ? LogicAlarm.ON_ANIMS : LogicAlarm.OFF_ANIMS, KAnim.PlayMode.Once);
	}

	// Token: 0x06003172 RID: 12658 RVA: 0x0011D9EC File Offset: 0x0011BBEC
	public void OnLogicValueChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID != LogicAlarm.INPUT_PORT_ID)
		{
			return;
		}
		int newValue = logicValueChanged.newValue;
		if (LogicCircuitNetwork.IsBitActive(0, newValue))
		{
			if (!this.wasOn)
			{
				this.PushNotification();
				this.wasOn = true;
				if (this.pauseOnNotify && !SpeedControlScreen.Instance.IsPaused)
				{
					SpeedControlScreen.Instance.Pause(false, false);
				}
				if (this.zoomOnNotify)
				{
					GameUtil.FocusCameraOnWorld(base.gameObject.GetMyWorldId(), base.transform.GetPosition(), 8f, null, true);
				}
				this.UpdateVisualState();
				return;
			}
		}
		else if (this.wasOn)
		{
			this.wasOn = false;
			this.UpdateVisualState();
		}
	}

	// Token: 0x06003173 RID: 12659 RVA: 0x0011DA9E File Offset: 0x0011BC9E
	private void PushNotification()
	{
		this.notification.Clear();
		this.notifier.Add(this.notification, "");
	}

	// Token: 0x06003174 RID: 12660 RVA: 0x0011DAC4 File Offset: 0x0011BCC4
	public void UpdateNotification(bool clear)
	{
		if (this.notification != null && clear)
		{
			this.notification.Clear();
			this.lastNotificationCreated = null;
		}
		if (this.notification != this.lastNotificationCreated || this.lastNotificationCreated == null)
		{
			this.notification = this.CreateNotification();
		}
	}

	// Token: 0x06003175 RID: 12661 RVA: 0x0011DB14 File Offset: 0x0011BD14
	public Notification CreateNotification()
	{
		base.GetComponent<KSelectable>();
		Notification result = new Notification(this.notificationName, this.notificationType, (List<Notification> n, object d) => this.notificationTooltip, null, true, 0f, null, null, null, false, false, false);
		this.lastNotificationCreated = result;
		return result;
	}

	// Token: 0x04001DB4 RID: 7604
	[Serialize]
	public string notificationName;

	// Token: 0x04001DB5 RID: 7605
	[Serialize]
	public string notificationTooltip;

	// Token: 0x04001DB6 RID: 7606
	[Serialize]
	public NotificationType notificationType;

	// Token: 0x04001DB7 RID: 7607
	[Serialize]
	public bool pauseOnNotify;

	// Token: 0x04001DB8 RID: 7608
	[Serialize]
	public bool zoomOnNotify;

	// Token: 0x04001DB9 RID: 7609
	[Serialize]
	public float cooldown;

	// Token: 0x04001DBA RID: 7610
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001DBB RID: 7611
	private bool wasOn;

	// Token: 0x04001DBC RID: 7612
	private Notifier notifier;

	// Token: 0x04001DBD RID: 7613
	private Notification notification;

	// Token: 0x04001DBE RID: 7614
	private Notification lastNotificationCreated;

	// Token: 0x04001DBF RID: 7615
	private static readonly EventSystem.IntraObjectHandler<LogicAlarm> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicAlarm>(delegate(LogicAlarm component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04001DC0 RID: 7616
	private static readonly EventSystem.IntraObjectHandler<LogicAlarm> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<LogicAlarm>(delegate(LogicAlarm component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x04001DC1 RID: 7617
	public static readonly HashedString INPUT_PORT_ID = new HashedString("LogicAlarmInput");

	// Token: 0x04001DC2 RID: 7618
	protected static readonly HashedString[] ON_ANIMS = new HashedString[]
	{
		"on_pre",
		"on_loop"
	};

	// Token: 0x04001DC3 RID: 7619
	protected static readonly HashedString[] OFF_ANIMS = new HashedString[]
	{
		"on_pst",
		"off"
	};
}
