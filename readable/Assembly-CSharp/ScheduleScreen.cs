using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000E01 RID: 3585
public class ScheduleScreen : KScreen
{
	// Token: 0x170007D9 RID: 2009
	// (get) Token: 0x06007197 RID: 29079 RVA: 0x002B63D3 File Offset: 0x002B45D3
	// (set) Token: 0x06007198 RID: 29080 RVA: 0x002B63DB File Offset: 0x002B45DB
	public string SelectedPaint { get; set; }

	// Token: 0x06007199 RID: 29081 RVA: 0x002B63E4 File Offset: 0x002B45E4
	public override float GetSortKey()
	{
		return 50f;
	}

	// Token: 0x0600719A RID: 29082 RVA: 0x002B63EB File Offset: 0x002B45EB
	protected override void OnPrefabInit()
	{
		base.ConsumeMouseScroll = true;
		this.scheduleEntries = new List<ScheduleScreenEntry>();
		ScheduleScreen.Instance = this;
	}

	// Token: 0x0600719B RID: 29083 RVA: 0x002B6408 File Offset: 0x002B4608
	protected override void OnSpawn()
	{
		foreach (Schedule schedule in ScheduleManager.Instance.GetSchedules())
		{
			this.AddScheduleEntry(schedule);
		}
		this.addScheduleButton.onClick += this.OnAddScheduleClick;
		this.closeButton.onClick += delegate()
		{
			ManagementMenu.Instance.CloseAll();
		};
		ScheduleManager.Instance.onSchedulesChanged += this.OnSchedulesChanged;
		Game.Instance.Subscribe(1983128072, new Action<object>(this.RefreshWidgetWorldData));
		this.uiRefreshHandle = base.Subscribe(1980521255, new Action<object>(this.RefreshWidgetWorldData));
	}

	// Token: 0x0600719C RID: 29084 RVA: 0x002B64F0 File Offset: 0x002B46F0
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		ScheduleManager.Instance.onSchedulesChanged -= this.OnSchedulesChanged;
		ScheduleScreen.Instance = null;
		base.Unsubscribe(ref this.uiRefreshHandle);
	}

	// Token: 0x0600719D RID: 29085 RVA: 0x002B6520 File Offset: 0x002B4720
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			base.Activate();
			this.SetScreenHeight();
		}
	}

	// Token: 0x0600719E RID: 29086 RVA: 0x002B6538 File Offset: 0x002B4738
	private void SetScreenHeight()
	{
		bool flag = ScheduleManager.Instance.GetSchedules().Count == 1;
		base.GetComponent<LayoutElement>().preferredHeight = (float)(flag ? 410 : 604);
		this.bottomSpacer.SetActive(flag);
	}

	// Token: 0x0600719F RID: 29087 RVA: 0x002B6580 File Offset: 0x002B4780
	public void RefreshAllPaintButtons()
	{
		foreach (ScheduleScreenEntry scheduleScreenEntry in this.scheduleEntries)
		{
			scheduleScreenEntry.RefreshPaintButtons();
		}
	}

	// Token: 0x060071A0 RID: 29088 RVA: 0x002B65D0 File Offset: 0x002B47D0
	private void OnAddScheduleClick()
	{
		ScheduleManager.Instance.AddDefaultSchedule(false, false);
	}

	// Token: 0x060071A1 RID: 29089 RVA: 0x002B65E0 File Offset: 0x002B47E0
	private void AddScheduleEntry(Schedule schedule)
	{
		ScheduleScreenEntry scheduleScreenEntry = Util.KInstantiateUI<ScheduleScreenEntry>(this.scheduleEntryPrefab.gameObject, this.scheduleEntryContainer, true);
		scheduleScreenEntry.Setup(schedule);
		this.scheduleEntries.Add(scheduleScreenEntry);
		this.SetScreenHeight();
	}

	// Token: 0x060071A2 RID: 29090 RVA: 0x002B6620 File Offset: 0x002B4820
	private void OnSchedulesChanged(List<Schedule> schedules)
	{
		foreach (ScheduleScreenEntry scheduleScreenEntry in this.scheduleEntries)
		{
			scheduleScreenEntry.Deregister();
			Util.KDestroyGameObject(scheduleScreenEntry.gameObject);
		}
		this.scheduleEntries.Clear();
		foreach (Schedule schedule in schedules)
		{
			this.AddScheduleEntry(schedule);
		}
		this.SetScreenHeight();
	}

	// Token: 0x060071A3 RID: 29091 RVA: 0x002B66CC File Offset: 0x002B48CC
	private void RefreshWidgetWorldData(object data = null)
	{
		foreach (ScheduleScreenEntry scheduleScreenEntry in this.scheduleEntries)
		{
			scheduleScreenEntry.RefreshWidgetWorldData();
		}
	}

	// Token: 0x060071A4 RID: 29092 RVA: 0x002B671C File Offset: 0x002B491C
	public void OnChangeCurrentTimetable()
	{
		foreach (ScheduleScreenEntry scheduleScreenEntry in this.scheduleEntries)
		{
			scheduleScreenEntry.RefreshTimeOfDayPositioner();
		}
	}

	// Token: 0x060071A5 RID: 29093 RVA: 0x002B676C File Offset: 0x002B496C
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.CheckBlockedInput())
		{
			if (!e.Consumed)
			{
				e.Consumed = true;
				return;
			}
		}
		else
		{
			base.OnKeyDown(e);
		}
	}

	// Token: 0x060071A6 RID: 29094 RVA: 0x002B6790 File Offset: 0x002B4990
	private bool CheckBlockedInput()
	{
		bool result = false;
		if (UnityEngine.EventSystems.EventSystem.current != null)
		{
			GameObject currentSelectedGameObject = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
			if (currentSelectedGameObject != null)
			{
				foreach (ScheduleScreenEntry scheduleScreenEntry in this.scheduleEntries)
				{
					if (currentSelectedGameObject == scheduleScreenEntry.GetNameInputField())
					{
						result = true;
						break;
					}
				}
			}
		}
		return result;
	}

	// Token: 0x04004E67 RID: 20071
	public static ScheduleScreen Instance;

	// Token: 0x04004E69 RID: 20073
	[SerializeField]
	private ScheduleScreenEntry scheduleEntryPrefab;

	// Token: 0x04004E6A RID: 20074
	[SerializeField]
	private GameObject scheduleEntryContainer;

	// Token: 0x04004E6B RID: 20075
	[SerializeField]
	private KButton addScheduleButton;

	// Token: 0x04004E6C RID: 20076
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04004E6D RID: 20077
	[SerializeField]
	private GameObject bottomSpacer;

	// Token: 0x04004E6E RID: 20078
	private List<ScheduleScreenEntry> scheduleEntries;

	// Token: 0x04004E6F RID: 20079
	private int uiRefreshHandle;
}
