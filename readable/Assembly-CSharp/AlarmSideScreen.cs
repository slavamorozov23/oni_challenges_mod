using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E12 RID: 3602
public class AlarmSideScreen : SideScreenContent
{
	// Token: 0x0600721E RID: 29214 RVA: 0x002B9A50 File Offset: 0x002B7C50
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.nameInputField.onEndEdit += this.OnEndEditName;
		this.nameInputField.field.characterLimit = 30;
		this.tooltipInputField.onEndEdit += this.OnEndEditTooltip;
		this.tooltipInputField.field.characterLimit = 90;
		this.pauseToggle.onClick += this.TogglePause;
		this.zoomToggle.onClick += this.ToggleZoom;
		this.InitializeToggles();
	}

	// Token: 0x0600721F RID: 29215 RVA: 0x002B9AE9 File Offset: 0x002B7CE9
	private void OnEndEditName()
	{
		this.targetAlarm.notificationName = this.nameInputField.field.text;
		this.UpdateNotification(true);
	}

	// Token: 0x06007220 RID: 29216 RVA: 0x002B9B0D File Offset: 0x002B7D0D
	private void OnEndEditTooltip()
	{
		this.targetAlarm.notificationTooltip = this.tooltipInputField.field.text;
		this.UpdateNotification(true);
	}

	// Token: 0x06007221 RID: 29217 RVA: 0x002B9B31 File Offset: 0x002B7D31
	private void TogglePause()
	{
		this.targetAlarm.pauseOnNotify = !this.targetAlarm.pauseOnNotify;
		this.pauseCheckmark.enabled = this.targetAlarm.pauseOnNotify;
		this.UpdateNotification(true);
	}

	// Token: 0x06007222 RID: 29218 RVA: 0x002B9B69 File Offset: 0x002B7D69
	private void ToggleZoom()
	{
		this.targetAlarm.zoomOnNotify = !this.targetAlarm.zoomOnNotify;
		this.zoomCheckmark.enabled = this.targetAlarm.zoomOnNotify;
		this.UpdateNotification(true);
	}

	// Token: 0x06007223 RID: 29219 RVA: 0x002B9BA1 File Offset: 0x002B7DA1
	private void SelectType(NotificationType type)
	{
		this.targetAlarm.notificationType = type;
		this.UpdateNotification(true);
		this.RefreshToggles();
	}

	// Token: 0x06007224 RID: 29220 RVA: 0x002B9BBC File Offset: 0x002B7DBC
	private void InitializeToggles()
	{
		if (this.toggles_by_type.Count == 0)
		{
			using (List<NotificationType>.Enumerator enumerator = this.validTypes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					NotificationType type = enumerator.Current;
					GameObject gameObject = Util.KInstantiateUI(this.typeButtonPrefab, this.typeButtonPrefab.transform.parent.gameObject, true);
					gameObject.name = "TypeButton: " + type.ToString();
					HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
					Color notificationBGColour = NotificationScreen.Instance.GetNotificationBGColour(type);
					Color notificationColour = NotificationScreen.Instance.GetNotificationColour(type);
					notificationBGColour.a = 1f;
					notificationColour.a = 1f;
					component.GetReference<KImage>("bg").color = notificationBGColour;
					component.GetReference<KImage>("icon").color = notificationColour;
					component.GetReference<KImage>("icon").sprite = NotificationScreen.Instance.GetNotificationIcon(type);
					ToolTip component2 = gameObject.GetComponent<ToolTip>();
					NotificationType type2 = type;
					if (type2 != NotificationType.Bad)
					{
						if (type2 != NotificationType.Neutral)
						{
							if (type2 == NotificationType.DuplicantThreatening)
							{
								component2.SetSimpleTooltip(UI.UISIDESCREENS.LOGICALARMSIDESCREEN.TOOLTIPS.DUPLICANT_THREATENING);
							}
						}
						else
						{
							component2.SetSimpleTooltip(UI.UISIDESCREENS.LOGICALARMSIDESCREEN.TOOLTIPS.NEUTRAL);
						}
					}
					else
					{
						component2.SetSimpleTooltip(UI.UISIDESCREENS.LOGICALARMSIDESCREEN.TOOLTIPS.BAD);
					}
					if (!this.toggles_by_type.ContainsKey(type))
					{
						this.toggles_by_type.Add(type, gameObject.GetComponent<MultiToggle>());
					}
					this.toggles_by_type[type].onClick = delegate()
					{
						this.SelectType(type);
					};
					for (int i = 0; i < this.toggles_by_type[type].states.Length; i++)
					{
						this.toggles_by_type[type].states[i].on_click_override_sound_path = NotificationScreen.Instance.GetNotificationSound(type);
					}
				}
			}
		}
	}

	// Token: 0x06007225 RID: 29221 RVA: 0x002B9E00 File Offset: 0x002B8000
	private void RefreshToggles()
	{
		this.InitializeToggles();
		foreach (KeyValuePair<NotificationType, MultiToggle> keyValuePair in this.toggles_by_type)
		{
			if (this.targetAlarm.notificationType == keyValuePair.Key)
			{
				keyValuePair.Value.ChangeState(0);
			}
			else
			{
				keyValuePair.Value.ChangeState(1);
			}
		}
	}

	// Token: 0x06007226 RID: 29222 RVA: 0x002B9E84 File Offset: 0x002B8084
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<LogicAlarm>() != null;
	}

	// Token: 0x06007227 RID: 29223 RVA: 0x002B9E92 File Offset: 0x002B8092
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.targetAlarm = target.GetComponent<LogicAlarm>();
		this.RefreshToggles();
		this.UpdateVisuals();
	}

	// Token: 0x06007228 RID: 29224 RVA: 0x002B9EB3 File Offset: 0x002B80B3
	private void UpdateNotification(bool clear)
	{
		this.targetAlarm.UpdateNotification(clear);
	}

	// Token: 0x06007229 RID: 29225 RVA: 0x002B9EC4 File Offset: 0x002B80C4
	private void UpdateVisuals()
	{
		this.nameInputField.SetDisplayValue(this.targetAlarm.notificationName);
		this.tooltipInputField.SetDisplayValue(this.targetAlarm.notificationTooltip);
		this.pauseCheckmark.enabled = this.targetAlarm.pauseOnNotify;
		this.zoomCheckmark.enabled = this.targetAlarm.zoomOnNotify;
	}

	// Token: 0x04004ED1 RID: 20177
	public LogicAlarm targetAlarm;

	// Token: 0x04004ED2 RID: 20178
	[SerializeField]
	private KInputField nameInputField;

	// Token: 0x04004ED3 RID: 20179
	[SerializeField]
	private KInputField tooltipInputField;

	// Token: 0x04004ED4 RID: 20180
	[SerializeField]
	private KToggle pauseToggle;

	// Token: 0x04004ED5 RID: 20181
	[SerializeField]
	private Image pauseCheckmark;

	// Token: 0x04004ED6 RID: 20182
	[SerializeField]
	private KToggle zoomToggle;

	// Token: 0x04004ED7 RID: 20183
	[SerializeField]
	private Image zoomCheckmark;

	// Token: 0x04004ED8 RID: 20184
	[SerializeField]
	private GameObject typeButtonPrefab;

	// Token: 0x04004ED9 RID: 20185
	private List<NotificationType> validTypes = new List<NotificationType>
	{
		NotificationType.Bad,
		NotificationType.Neutral,
		NotificationType.DuplicantThreatening
	};

	// Token: 0x04004EDA RID: 20186
	private Dictionary<NotificationType, MultiToggle> toggles_by_type = new Dictionary<NotificationType, MultiToggle>();
}
