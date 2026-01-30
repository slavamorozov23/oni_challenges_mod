using System;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DBF RID: 3519
public class NotificationScreen : KScreen
{
	// Token: 0x170007BB RID: 1979
	// (get) Token: 0x06006DE4 RID: 28132 RVA: 0x00299FFE File Offset: 0x002981FE
	// (set) Token: 0x06006DE5 RID: 28133 RVA: 0x0029A005 File Offset: 0x00298205
	public static NotificationScreen Instance { get; private set; }

	// Token: 0x06006DE6 RID: 28134 RVA: 0x0029A00D File Offset: 0x0029820D
	public static void DestroyInstance()
	{
		NotificationScreen.Instance = null;
	}

	// Token: 0x06006DE7 RID: 28135 RVA: 0x0029A015 File Offset: 0x00298215
	public void AddPendingNotification(Notification notification)
	{
		this.pendingNotifications.Add(notification);
	}

	// Token: 0x06006DE8 RID: 28136 RVA: 0x0029A023 File Offset: 0x00298223
	public void RemovePendingNotification(Notification notification)
	{
		this.dirty = true;
		this.pendingNotifications.Remove(notification);
		this.RemoveNotification(notification);
	}

	// Token: 0x06006DE9 RID: 28137 RVA: 0x0029A040 File Offset: 0x00298240
	public void RemoveNotification(Notification notification)
	{
		NotificationScreen.Entry entry = null;
		this.entriesByMessage.TryGetValue(notification.titleText, out entry);
		if (entry == null)
		{
			return;
		}
		this.notifications.Remove(notification);
		entry.Remove(notification);
		if (entry.notifications.Count == 0)
		{
			UnityEngine.Object.Destroy(entry.label);
			this.entriesByMessage[notification.titleText] = null;
			this.entries.Remove(entry);
		}
	}

	// Token: 0x06006DEA RID: 28138 RVA: 0x0029A0B4 File Offset: 0x002982B4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		NotificationScreen.Instance = this;
		foreach (NotificationScreen.CustomNotificationPrefabs customNotificationPrefabs in this.customNotificationPrefabs)
		{
			if (customNotificationPrefabs.notificationPrefab != null)
			{
				customNotificationPrefabs.notificationPrefab.SetActive(false);
			}
		}
		this.MessagesPrefab.gameObject.SetActive(false);
		this.LabelPrefab.gameObject.SetActive(false);
		this.InitNotificationSounds();
	}

	// Token: 0x06006DEB RID: 28139 RVA: 0x0029A150 File Offset: 0x00298350
	private void OnNewMessage(object data)
	{
		Message m = (Message)data;
		this.notifier.Add(new MessageNotification(m), "");
	}

	// Token: 0x06006DEC RID: 28140 RVA: 0x0029A17C File Offset: 0x0029837C
	private void ShowMessage(MessageNotification mn)
	{
		mn.message.OnClick();
		if (mn.message.ShowDialog())
		{
			for (int i = 0; i < this.dialogPrefabs.Count; i++)
			{
				if (this.dialogPrefabs[i].CanDisplay(mn.message))
				{
					if (this.messageDialog != null)
					{
						UnityEngine.Object.Destroy(this.messageDialog.gameObject);
						this.messageDialog = null;
					}
					this.messageDialog = global::Util.KInstantiateUI<MessageDialogFrame>(ScreenPrefabs.Instance.MessageDialogFrame.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, false);
					MessageDialog dialog = global::Util.KInstantiateUI<MessageDialog>(this.dialogPrefabs[i].gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, false);
					this.messageDialog.SetMessage(dialog, mn.message);
					this.messageDialog.Show(true);
					break;
				}
			}
		}
		Messenger.Instance.RemoveMessage(mn.message);
		mn.Clear();
	}

	// Token: 0x06006DED RID: 28141 RVA: 0x0029A288 File Offset: 0x00298488
	public void OnClickNextMessage()
	{
		Notification notification2 = this.notifications.Find((Notification notification) => notification.Type == NotificationType.Messages);
		this.ShowMessage((MessageNotification)notification2);
	}

	// Token: 0x06006DEE RID: 28142 RVA: 0x0029A2CC File Offset: 0x002984CC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.initTime = KTime.Instance.UnscaledGameTime;
		LocText[] componentsInChildren = this.LabelPrefab.GetComponentsInChildren<LocText>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].color = GlobalAssets.Instance.colorSet.NotificationNormal;
		}
		componentsInChildren = this.MessagesPrefab.GetComponentsInChildren<LocText>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].color = GlobalAssets.Instance.colorSet.NotificationNormal;
		}
		base.Subscribe(Messenger.Instance.gameObject, 1558809273, new Action<object>(this.OnNewMessage));
		foreach (Message m in Messenger.Instance.Messages)
		{
			Notification notification = new MessageNotification(m);
			notification.playSound = false;
			this.notifier.Add(notification, "");
		}
	}

	// Token: 0x06006DEF RID: 28143 RVA: 0x0029A3D8 File Offset: 0x002985D8
	protected override void OnActivate()
	{
		base.OnActivate();
		this.dirty = true;
	}

	// Token: 0x06006DF0 RID: 28144 RVA: 0x0029A3E8 File Offset: 0x002985E8
	public void AddNotification(Notification notification)
	{
		if (DebugHandler.NotificationsDisabled)
		{
			return;
		}
		this.notifications.Add(notification);
		NotificationScreen.Entry entry;
		this.entriesByMessage.TryGetValue(notification.titleText, out entry);
		if (entry == null)
		{
			HierarchyReferences hierarchyReferences;
			if (notification.Type == NotificationType.Custom)
			{
				NotificationScreen.CustomNotificationPrefabs customNotificationPrefabs = this.customNotificationPrefabs.Find((NotificationScreen.CustomNotificationPrefabs d) => d.ID == notification.customNotificationID);
				global::Debug.Assert(customNotificationPrefabs != null, "Custom notification prefab not found for notification ID: " + notification.customNotificationID);
				hierarchyReferences = global::Util.KInstantiateUI<HierarchyReferences>(customNotificationPrefabs.notificationPrefab, customNotificationPrefabs.parentFolder, false);
			}
			else if (notification.Type == NotificationType.Messages)
			{
				hierarchyReferences = global::Util.KInstantiateUI<HierarchyReferences>(this.MessagesPrefab, this.MessagesFolder, false);
			}
			else
			{
				hierarchyReferences = global::Util.KInstantiateUI<HierarchyReferences>(this.LabelPrefab, this.LabelsFolder, false);
			}
			Button reference = hierarchyReferences.GetReference<Button>("DismissButton");
			reference.gameObject.SetActive(notification.showDismissButton);
			if (notification.showDismissButton)
			{
				reference.onClick.AddListener(delegate()
				{
					NotificationScreen.Entry entry;
					if (!this.entriesByMessage.TryGetValue(notification.titleText, out entry))
					{
						return;
					}
					for (int i = entry.notifications.Count - 1; i >= 0; i--)
					{
						Notification notification2 = entry.notifications[i];
						MessageNotification messageNotification2 = notification2 as MessageNotification;
						if (messageNotification2 != null)
						{
							Messenger.Instance.RemoveMessage(messageNotification2.message);
						}
						notification2.Clear();
					}
				});
			}
			hierarchyReferences.GetReference<NotificationAnimator>("Animator").Begin(true);
			hierarchyReferences.gameObject.SetActive(true);
			if (notification.ToolTip != null)
			{
				ToolTip tooltip = hierarchyReferences.GetReference<ToolTip>("ToolTip");
				tooltip.OnToolTip = delegate()
				{
					tooltip.ClearMultiStringTooltip();
					tooltip.AddMultiStringTooltip(notification.ToolTip(entry.notifications, notification.tooltipData), this.TooltipTextStyle);
					return "";
				};
			}
			KImage reference2 = hierarchyReferences.GetReference<KImage>("Icon");
			LocText reference3 = hierarchyReferences.GetReference<LocText>("Text");
			Button reference4 = hierarchyReferences.GetReference<Button>("MainButton");
			ColorBlock colors = reference4.colors;
			switch (notification.Type)
			{
			case NotificationType.Bad:
			case NotificationType.DuplicantThreatening:
				colors.normalColor = GlobalAssets.Instance.colorSet.NotificationBadBG;
				reference3.color = GlobalAssets.Instance.colorSet.NotificationBad;
				reference2.color = GlobalAssets.Instance.colorSet.NotificationBad;
				reference2.sprite = ((notification.Type == NotificationType.Bad) ? this.icon_bad : this.icon_threatening);
				goto IL_49F;
			case NotificationType.Tutorial:
				colors.normalColor = GlobalAssets.Instance.colorSet.NotificationTutorialBG;
				reference3.color = GlobalAssets.Instance.colorSet.NotificationTutorial;
				reference2.color = GlobalAssets.Instance.colorSet.NotificationTutorial;
				reference2.sprite = this.icon_warning;
				goto IL_49F;
			case NotificationType.Messages:
			{
				colors.normalColor = GlobalAssets.Instance.colorSet.NotificationMessageBG;
				reference3.color = GlobalAssets.Instance.colorSet.NotificationMessage;
				reference2.color = GlobalAssets.Instance.colorSet.NotificationMessage;
				reference2.sprite = this.icon_message;
				MessageNotification messageNotification = notification as MessageNotification;
				if (messageNotification == null)
				{
					goto IL_49F;
				}
				TutorialMessage tutorialMessage = messageNotification.message as TutorialMessage;
				if (tutorialMessage != null && !string.IsNullOrEmpty(tutorialMessage.videoClipId))
				{
					reference2.sprite = this.icon_video;
					goto IL_49F;
				}
				goto IL_49F;
			}
			case NotificationType.Event:
				colors.normalColor = GlobalAssets.Instance.colorSet.NotificationEventBG;
				reference3.color = GlobalAssets.Instance.colorSet.NotificationEvent;
				reference2.color = GlobalAssets.Instance.colorSet.NotificationEvent;
				reference2.sprite = this.icon_event;
				goto IL_49F;
			case NotificationType.MessageImportant:
				colors.normalColor = GlobalAssets.Instance.colorSet.NotificationMessageImportantBG;
				reference3.color = GlobalAssets.Instance.colorSet.NotificationMessageImportant;
				reference2.color = GlobalAssets.Instance.colorSet.NotificationMessageImportant;
				reference2.sprite = this.icon_message_important;
				goto IL_49F;
			case NotificationType.Custom:
				goto IL_49F;
			}
			colors.normalColor = GlobalAssets.Instance.colorSet.NotificationNormalBG;
			reference3.color = GlobalAssets.Instance.colorSet.NotificationNormal;
			reference2.color = GlobalAssets.Instance.colorSet.NotificationNormal;
			reference2.sprite = this.icon_normal;
			IL_49F:
			reference4.colors = colors;
			reference4.onClick.AddListener(delegate()
			{
				this.OnClick(entry);
			});
			string str = "";
			if (KTime.Instance.UnscaledGameTime - this.initTime > 5f && notification.playSound)
			{
				this.PlayDingSound(notification, 0);
			}
			else
			{
				str = "too early";
			}
			if (AudioDebug.Get().debugNotificationSounds)
			{
				global::Debug.Log("Notification(" + notification.titleText + "):" + str);
			}
			entry = new NotificationScreen.Entry(hierarchyReferences.gameObject);
			this.entriesByMessage[notification.titleText] = entry;
			this.entries.Add(entry);
		}
		entry.Add(notification);
		this.dirty = true;
		this.SortNotifications();
	}

	// Token: 0x06006DF1 RID: 28145 RVA: 0x0029A980 File Offset: 0x00298B80
	private void SortNotifications()
	{
		this.notifications.Sort(delegate(Notification n1, Notification n2)
		{
			if (n1.Type == n2.Type)
			{
				return n1.Idx - n2.Idx;
			}
			return n1.Type - n2.Type;
		});
		foreach (Notification notification in this.notifications)
		{
			NotificationScreen.Entry entry = null;
			this.entriesByMessage.TryGetValue(notification.titleText, out entry);
			if (entry != null)
			{
				entry.label.GetComponent<RectTransform>().SetAsLastSibling();
			}
		}
	}

	// Token: 0x06006DF2 RID: 28146 RVA: 0x0029AA20 File Offset: 0x00298C20
	private void PlayDingSound(Notification notification, int count)
	{
		string text;
		if (!this.notificationSounds.TryGetValue(notification.Type, out text))
		{
			text = "Notification";
		}
		float num;
		if (!this.timeOfLastNotification.TryGetValue(text, out num))
		{
			num = 0f;
		}
		float value = notification.volume_attenuation ? ((Time.time - num) / this.soundDecayTime) : 1f;
		this.timeOfLastNotification[text] = Time.time;
		string sound;
		if (count > 1)
		{
			sound = GlobalAssets.GetSound(text + "_AddCount", true);
			if (sound == null)
			{
				sound = GlobalAssets.GetSound(text, false);
			}
		}
		else
		{
			sound = GlobalAssets.GetSound(text, false);
		}
		if (notification.playSound)
		{
			EventInstance instance = KFMOD.BeginOneShot(sound, Vector3.zero, 1f);
			instance.setParameterByName("timeSinceLast", value, false);
			KFMOD.EndOneShot(instance);
		}
	}

	// Token: 0x06006DF3 RID: 28147 RVA: 0x0029AAEC File Offset: 0x00298CEC
	private void Update()
	{
		int i = 0;
		while (i < this.pendingNotifications.Count)
		{
			if (this.pendingNotifications[i].IsReady())
			{
				this.AddNotification(this.pendingNotifications[i]);
				this.pendingNotifications.RemoveAt(i);
			}
			else
			{
				i++;
			}
		}
		int num = 0;
		int num2 = 0;
		for (int j = 0; j < this.notifications.Count; j++)
		{
			Notification notification = this.notifications[j];
			if (notification.Type == NotificationType.Messages)
			{
				num2++;
			}
			else
			{
				num++;
			}
			if (notification.expires && KTime.Instance.UnscaledGameTime - notification.Time > this.lifetime)
			{
				this.dirty = true;
				if (notification.Notifier == null)
				{
					this.RemovePendingNotification(notification);
				}
				else
				{
					notification.Clear();
				}
			}
		}
	}

	// Token: 0x06006DF4 RID: 28148 RVA: 0x0029ABC8 File Offset: 0x00298DC8
	private void OnClick(NotificationScreen.Entry entry)
	{
		Notification nextClickedNotification = entry.NextClickedNotification;
		base.PlaySound3D(GlobalAssets.GetSound("HUD_Click_Open", false));
		if (nextClickedNotification.customClickCallback != null)
		{
			nextClickedNotification.customClickCallback(nextClickedNotification.customClickData);
		}
		else
		{
			if (nextClickedNotification.clickFocus != null)
			{
				Vector3 position = nextClickedNotification.clickFocus.GetPosition();
				position.z = -40f;
				ClusterGridEntity component = nextClickedNotification.clickFocus.GetComponent<ClusterGridEntity>();
				KSelectable component2 = nextClickedNotification.clickFocus.GetComponent<KSelectable>();
				int myWorldId = nextClickedNotification.clickFocus.gameObject.GetMyWorldId();
				if (myWorldId != -1)
				{
					GameUtil.FocusCameraOnWorld(myWorldId, position, 10f, null, true);
				}
				else if (DlcManager.FeatureClusterSpaceEnabled() && component != null && component.IsVisible)
				{
					ManagementMenu.Instance.OpenClusterMap();
					ClusterMapScreen.Instance.SetTargetFocusPosition(component.Location, 0.5f);
				}
				if (component2 != null)
				{
					if (DlcManager.FeatureClusterSpaceEnabled() && component != null && component.IsVisible)
					{
						ClusterMapSelectTool.Instance.Select(component2, false);
					}
					else
					{
						SelectTool.Instance.Select(component2, false);
					}
				}
			}
			else if (nextClickedNotification.Notifier != null)
			{
				SelectTool.Instance.Select(nextClickedNotification.Notifier.GetComponent<KSelectable>(), false);
			}
			if (nextClickedNotification.Type == NotificationType.Messages)
			{
				this.ShowMessage((MessageNotification)nextClickedNotification);
			}
		}
		if (nextClickedNotification.clearOnClick)
		{
			nextClickedNotification.Clear();
		}
	}

	// Token: 0x06006DF5 RID: 28149 RVA: 0x0029AD2F File Offset: 0x00298F2F
	private void PositionLocatorIcon()
	{
	}

	// Token: 0x06006DF6 RID: 28150 RVA: 0x0029AD34 File Offset: 0x00298F34
	private void InitNotificationSounds()
	{
		this.notificationSounds[NotificationType.Good] = "Notification";
		this.notificationSounds[NotificationType.BadMinor] = "Notification";
		this.notificationSounds[NotificationType.Bad] = "Warning";
		this.notificationSounds[NotificationType.Neutral] = "Notification";
		this.notificationSounds[NotificationType.Tutorial] = "Notification";
		this.notificationSounds[NotificationType.Messages] = "Message";
		this.notificationSounds[NotificationType.DuplicantThreatening] = "Warning_DupeThreatening";
		this.notificationSounds[NotificationType.Event] = "Message";
		this.notificationSounds[NotificationType.MessageImportant] = "Message_Important";
	}

	// Token: 0x06006DF7 RID: 28151 RVA: 0x0029ADDC File Offset: 0x00298FDC
	public Sprite GetNotificationIcon(NotificationType type)
	{
		switch (type)
		{
		case NotificationType.Bad:
			return this.icon_bad;
		case NotificationType.Tutorial:
			return this.icon_warning;
		case NotificationType.Messages:
			return this.icon_message;
		case NotificationType.DuplicantThreatening:
			return this.icon_threatening;
		case NotificationType.Event:
			return this.icon_event;
		case NotificationType.MessageImportant:
			return this.icon_message_important;
		}
		return this.icon_normal;
	}

	// Token: 0x06006DF8 RID: 28152 RVA: 0x0029AE48 File Offset: 0x00299048
	public Color GetNotificationColour(NotificationType type)
	{
		switch (type)
		{
		case NotificationType.Bad:
			return GlobalAssets.Instance.colorSet.NotificationBad;
		case NotificationType.Tutorial:
			return GlobalAssets.Instance.colorSet.NotificationTutorial;
		case NotificationType.Messages:
			return GlobalAssets.Instance.colorSet.NotificationMessage;
		case NotificationType.DuplicantThreatening:
			return GlobalAssets.Instance.colorSet.NotificationBad;
		case NotificationType.Event:
			return GlobalAssets.Instance.colorSet.NotificationEvent;
		case NotificationType.MessageImportant:
			return GlobalAssets.Instance.colorSet.NotificationMessageImportant;
		}
		return GlobalAssets.Instance.colorSet.NotificationNormal;
	}

	// Token: 0x06006DF9 RID: 28153 RVA: 0x0029AF18 File Offset: 0x00299118
	public Color GetNotificationBGColour(NotificationType type)
	{
		switch (type)
		{
		case NotificationType.Bad:
			return GlobalAssets.Instance.colorSet.NotificationBadBG;
		case NotificationType.Tutorial:
			return GlobalAssets.Instance.colorSet.NotificationTutorialBG;
		case NotificationType.Messages:
			return GlobalAssets.Instance.colorSet.NotificationMessageBG;
		case NotificationType.DuplicantThreatening:
			return GlobalAssets.Instance.colorSet.NotificationBadBG;
		case NotificationType.Event:
			return GlobalAssets.Instance.colorSet.NotificationEventBG;
		case NotificationType.MessageImportant:
			return GlobalAssets.Instance.colorSet.NotificationMessageImportantBG;
		}
		return GlobalAssets.Instance.colorSet.NotificationNormalBG;
	}

	// Token: 0x06006DFA RID: 28154 RVA: 0x0029AFE5 File Offset: 0x002991E5
	public string GetNotificationSound(NotificationType type)
	{
		return this.notificationSounds[type];
	}

	// Token: 0x04004B08 RID: 19208
	public float lifetime;

	// Token: 0x04004B09 RID: 19209
	public bool dirty;

	// Token: 0x04004B0A RID: 19210
	public GameObject LabelPrefab;

	// Token: 0x04004B0B RID: 19211
	public GameObject LabelsFolder;

	// Token: 0x04004B0C RID: 19212
	public GameObject MessagesPrefab;

	// Token: 0x04004B0D RID: 19213
	public GameObject MessagesFolder;

	// Token: 0x04004B0E RID: 19214
	public List<NotificationScreen.CustomNotificationPrefabs> customNotificationPrefabs;

	// Token: 0x04004B0F RID: 19215
	private MessageDialogFrame messageDialog;

	// Token: 0x04004B10 RID: 19216
	private float initTime;

	// Token: 0x04004B11 RID: 19217
	[MyCmpAdd]
	private Notifier notifier;

	// Token: 0x04004B12 RID: 19218
	[SerializeField]
	private List<MessageDialog> dialogPrefabs = new List<MessageDialog>();

	// Token: 0x04004B13 RID: 19219
	[SerializeField]
	private Color badColorBG;

	// Token: 0x04004B14 RID: 19220
	[SerializeField]
	private Color badColor = Color.red;

	// Token: 0x04004B15 RID: 19221
	[SerializeField]
	private Color normalColorBG;

	// Token: 0x04004B16 RID: 19222
	[SerializeField]
	private Color normalColor = Color.white;

	// Token: 0x04004B17 RID: 19223
	[SerializeField]
	private Color warningColorBG;

	// Token: 0x04004B18 RID: 19224
	[SerializeField]
	private Color warningColor;

	// Token: 0x04004B19 RID: 19225
	[SerializeField]
	private Color messageColorBG;

	// Token: 0x04004B1A RID: 19226
	[SerializeField]
	private Color messageColor;

	// Token: 0x04004B1B RID: 19227
	[SerializeField]
	private Color messageImportantColorBG;

	// Token: 0x04004B1C RID: 19228
	[SerializeField]
	private Color messageImportantColor;

	// Token: 0x04004B1D RID: 19229
	[SerializeField]
	private Color eventColorBG;

	// Token: 0x04004B1E RID: 19230
	[SerializeField]
	private Color eventColor;

	// Token: 0x04004B1F RID: 19231
	public Sprite icon_normal;

	// Token: 0x04004B20 RID: 19232
	public Sprite icon_warning;

	// Token: 0x04004B21 RID: 19233
	public Sprite icon_bad;

	// Token: 0x04004B22 RID: 19234
	public Sprite icon_threatening;

	// Token: 0x04004B23 RID: 19235
	public Sprite icon_message;

	// Token: 0x04004B24 RID: 19236
	public Sprite icon_message_important;

	// Token: 0x04004B25 RID: 19237
	public Sprite icon_video;

	// Token: 0x04004B26 RID: 19238
	public Sprite icon_event;

	// Token: 0x04004B27 RID: 19239
	private List<Notification> pendingNotifications = new List<Notification>();

	// Token: 0x04004B28 RID: 19240
	private List<Notification> notifications = new List<Notification>();

	// Token: 0x04004B29 RID: 19241
	public TextStyleSetting TooltipTextStyle;

	// Token: 0x04004B2A RID: 19242
	private Dictionary<NotificationType, string> notificationSounds = new Dictionary<NotificationType, string>();

	// Token: 0x04004B2B RID: 19243
	private Dictionary<string, float> timeOfLastNotification = new Dictionary<string, float>();

	// Token: 0x04004B2C RID: 19244
	private float soundDecayTime = 10f;

	// Token: 0x04004B2D RID: 19245
	private List<NotificationScreen.Entry> entries = new List<NotificationScreen.Entry>();

	// Token: 0x04004B2E RID: 19246
	private Dictionary<string, NotificationScreen.Entry> entriesByMessage = new Dictionary<string, NotificationScreen.Entry>();

	// Token: 0x0200200F RID: 8207
	[Serializable]
	public class CustomNotificationPrefabs
	{
		// Token: 0x040094B6 RID: 38070
		public string ID;

		// Token: 0x040094B7 RID: 38071
		public GameObject notificationPrefab;

		// Token: 0x040094B8 RID: 38072
		public GameObject parentFolder;
	}

	// Token: 0x02002010 RID: 8208
	private class Entry
	{
		// Token: 0x0600B823 RID: 47139 RVA: 0x003F465A File Offset: 0x003F285A
		public Entry(GameObject label)
		{
			this.label = label;
		}

		// Token: 0x0600B824 RID: 47140 RVA: 0x003F4674 File Offset: 0x003F2874
		public void Add(Notification notification)
		{
			this.notifications.Add(notification);
			this.UpdateMessage(notification, true);
		}

		// Token: 0x0600B825 RID: 47141 RVA: 0x003F468A File Offset: 0x003F288A
		public void Remove(Notification notification)
		{
			this.notifications.Remove(notification);
			this.UpdateMessage(notification, false);
		}

		// Token: 0x0600B826 RID: 47142 RVA: 0x003F46A4 File Offset: 0x003F28A4
		public void UpdateMessage(Notification notification, bool playSound = true)
		{
			if (Game.IsQuitting())
			{
				return;
			}
			this.message = notification.titleText;
			if (this.notifications.Count > 1)
			{
				if (playSound && (notification.Type == NotificationType.Bad || notification.Type == NotificationType.DuplicantThreatening))
				{
					NotificationScreen.Instance.PlayDingSound(notification, this.notifications.Count);
				}
				this.message = this.message + " (" + this.notifications.Count.ToString() + ")";
			}
			if (this.label != null)
			{
				this.label.GetComponent<HierarchyReferences>().GetReference<LocText>("Text").text = this.message;
			}
		}

		// Token: 0x17000CEE RID: 3310
		// (get) Token: 0x0600B827 RID: 47143 RVA: 0x003F475C File Offset: 0x003F295C
		public Notification NextClickedNotification
		{
			get
			{
				List<Notification> list = this.notifications;
				int num = this.clickIdx;
				this.clickIdx = num + 1;
				return list[num % this.notifications.Count];
			}
		}

		// Token: 0x040094B9 RID: 38073
		public string message;

		// Token: 0x040094BA RID: 38074
		public int clickIdx;

		// Token: 0x040094BB RID: 38075
		public GameObject label;

		// Token: 0x040094BC RID: 38076
		public List<Notification> notifications = new List<Notification>();
	}
}
