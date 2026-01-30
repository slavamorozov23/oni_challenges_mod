using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E04 RID: 3588
[AddComponentMenu("KMonoBehaviour/scripts/ScheduleScreenEntry")]
public class ScheduleScreenEntry : KMonoBehaviour
{
	// Token: 0x170007DA RID: 2010
	// (get) Token: 0x060071AD RID: 29101 RVA: 0x002B6859 File Offset: 0x002B4A59
	// (set) Token: 0x060071AE RID: 29102 RVA: 0x002B6861 File Offset: 0x002B4A61
	public Schedule schedule { get; private set; }

	// Token: 0x060071AF RID: 29103 RVA: 0x002B686C File Offset: 0x002B4A6C
	public void Setup(Schedule schedule)
	{
		this.schedule = schedule;
		base.gameObject.name = "Schedule_" + schedule.name;
		this.title.SetTitle(schedule.name);
		this.title.OnNameChanged += this.OnNameChanged;
		this.duplicateScheduleButton.onClick += this.DuplicateSchedule;
		this.deleteScheduleButton.onClick += this.DeleteSchedule;
		this.timetableRows = new List<GameObject>();
		this.blockButtonsByTimetableRow = new Dictionary<GameObject, List<ScheduleBlockButton>>();
		int num = Mathf.CeilToInt((float)(schedule.GetBlocks().Count / 24));
		for (int i = 0; i < num; i++)
		{
			this.AddTimetableRow(i * 24);
		}
		this.minionWidgets = new List<ScheduleMinionWidget>();
		this.blankMinionWidget = Util.KInstantiateUI<ScheduleMinionWidget>(this.minionWidgetPrefab.gameObject, this.minionWidgetContainer, false);
		this.blankMinionWidget.SetupBlank(schedule);
		this.RebuildMinionWidgets();
		this.RefreshStatus();
		this.RefreshAlarmButton();
		MultiToggle multiToggle = this.alarmButton;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.OnAlarmClicked));
		schedule.onChanged = (Action<Schedule>)Delegate.Combine(schedule.onChanged, new Action<Schedule>(this.OnScheduleChanged));
		this.ConfigPaintButton(this.PaintButtonBathtime, Db.Get().ScheduleGroups.Hygene, Def.GetUISprite(Assets.GetPrefab(ShowerConfig.ID), "ui", false).first);
		this.ConfigPaintButton(this.PaintButtonWorktime, Db.Get().ScheduleGroups.Worktime, Def.GetUISprite(Assets.GetPrefab("ManualGenerator"), "ui", false).first);
		this.ConfigPaintButton(this.PaintButtonRecreation, Db.Get().ScheduleGroups.Recreation, Def.GetUISprite(Assets.GetPrefab("WaterCooler"), "ui", false).first);
		this.ConfigPaintButton(this.PaintButtonSleep, Db.Get().ScheduleGroups.Sleep, Def.GetUISprite(Assets.GetPrefab("Bed"), "ui", false).first);
		this.RefreshPaintButtons();
		this.RefreshTimeOfDayPositioner();
	}

	// Token: 0x060071B0 RID: 29104 RVA: 0x002B6AB5 File Offset: 0x002B4CB5
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.Deregister();
	}

	// Token: 0x060071B1 RID: 29105 RVA: 0x002B6AC3 File Offset: 0x002B4CC3
	public void Deregister()
	{
		if (this.schedule != null)
		{
			Schedule schedule = this.schedule;
			schedule.onChanged = (Action<Schedule>)Delegate.Remove(schedule.onChanged, new Action<Schedule>(this.OnScheduleChanged));
		}
	}

	// Token: 0x060071B2 RID: 29106 RVA: 0x002B6AF4 File Offset: 0x002B4CF4
	private void DuplicateSchedule()
	{
		ScheduleManager.Instance.DuplicateSchedule(this.schedule);
	}

	// Token: 0x060071B3 RID: 29107 RVA: 0x002B6B07 File Offset: 0x002B4D07
	private void DeleteSchedule()
	{
		ScheduleManager.Instance.DeleteSchedule(this.schedule);
	}

	// Token: 0x060071B4 RID: 29108 RVA: 0x002B6B1C File Offset: 0x002B4D1C
	public void RefreshTimeOfDayPositioner()
	{
		if (this.schedule.ProgressTimetableIdx >= this.timetableRows.Count || this.schedule.ProgressTimetableIdx < 0)
		{
			KCrashReporter.ReportDevNotification("RefreshTimeOfDayPositionerError", Environment.StackTrace, string.Format("DevError: schedule.ProgressTimetableIdx is out of bounds. schedule.name:{0}, schedule.ProgressTimetableIdx:{1}, : timetableRows.Count:{2}", this.schedule.name, this.schedule.ProgressTimetableIdx, this.timetableRows.Count), true, null);
			this.timeOfDayPositioner.SetTargetTimetable(null);
			return;
		}
		GameObject targetTimetable = this.timetableRows[this.schedule.ProgressTimetableIdx];
		this.timeOfDayPositioner.SetTargetTimetable(targetTimetable);
	}

	// Token: 0x060071B5 RID: 29109 RVA: 0x002B6BC8 File Offset: 0x002B4DC8
	private void DuplicateTimetableRow(int sourceTimetableIdx)
	{
		List<ScheduleBlock> range = this.schedule.GetBlocks().GetRange(sourceTimetableIdx * 24, 24);
		List<ScheduleBlock> list = new List<ScheduleBlock>();
		for (int i = 0; i < range.Count; i++)
		{
			list.Add(new ScheduleBlock(range[i].name, range[i].GroupId));
		}
		int num = sourceTimetableIdx + 1;
		this.schedule.InsertTimetable(num, list);
		this.AddTimetableRow(num * 24);
	}

	// Token: 0x060071B6 RID: 29110 RVA: 0x002B6C44 File Offset: 0x002B4E44
	private void AddTimetableRow(int startingBlockIdx)
	{
		GameObject row = Util.KInstantiateUI(this.timetableRowPrefab, this.timetableRowContainer, true);
		int num = startingBlockIdx / 24;
		this.timetableRows.Insert(num, row);
		row.transform.SetSiblingIndex(num);
		HierarchyReferences component = row.GetComponent<HierarchyReferences>();
		List<ScheduleBlockButton> list = new List<ScheduleBlockButton>();
		for (int i = startingBlockIdx; i < startingBlockIdx + 24; i++)
		{
			GameObject gameObject = component.GetReference<RectTransform>("BlockContainer").gameObject;
			ScheduleBlockButton scheduleBlockButton = Util.KInstantiateUI<ScheduleBlockButton>(this.blockButtonPrefab.gameObject, gameObject, true);
			scheduleBlockButton.Setup(i - startingBlockIdx);
			scheduleBlockButton.SetBlockTypes(this.schedule.GetBlock(i).allowed_types);
			list.Add(scheduleBlockButton);
		}
		this.blockButtonsByTimetableRow.Add(row, list);
		component.GetReference<ScheduleBlockPainter>("BlockPainter").SetEntry(this);
		component.GetReference<KButton>("DuplicateButton").onClick += delegate()
		{
			this.DuplicateTimetableRow(this.timetableRows.IndexOf(row));
		};
		component.GetReference<KButton>("DeleteButton").onClick += delegate()
		{
			this.RemoveTimetableRow(row);
		};
		component.GetReference<KButton>("RotateLeftButton").onClick += delegate()
		{
			this.schedule.RotateBlocks(true, this.timetableRows.IndexOf(row));
		};
		component.GetReference<KButton>("RotateRightButton").onClick += delegate()
		{
			this.schedule.RotateBlocks(false, this.timetableRows.IndexOf(row));
		};
		KButton rotateUpButton = component.GetReference<KButton>("ShiftUpButton");
		rotateUpButton.onClick += delegate()
		{
			int timetableToShiftIdx = this.timetableRows.IndexOf(row);
			this.schedule.ShiftTimetable(true, timetableToShiftIdx);
			if (rotateUpButton.soundPlayer.button_widget_sound_events[0].OverrideAssetName == "ScheduleMenu_Shift_up")
			{
				rotateUpButton.soundPlayer.button_widget_sound_events[0].OverrideAssetName = "ScheduleMenu_Shift_up_reset";
				return;
			}
			rotateUpButton.soundPlayer.button_widget_sound_events[0].OverrideAssetName = "ScheduleMenu_Shift_up";
		};
		KButton rotateDownButton = component.GetReference<KButton>("ShiftDownButton");
		rotateDownButton.onClick += delegate()
		{
			int timetableToShiftIdx = this.timetableRows.IndexOf(row);
			this.schedule.ShiftTimetable(false, timetableToShiftIdx);
			if (rotateDownButton.soundPlayer.button_widget_sound_events[0].OverrideAssetName == "ScheduleMenu_Shift_down")
			{
				rotateDownButton.soundPlayer.button_widget_sound_events[0].OverrideAssetName = "ScheduleMenu_Shift_down_reset";
				return;
			}
			rotateDownButton.soundPlayer.button_widget_sound_events[0].OverrideAssetName = "ScheduleMenu_Shift_down";
		};
	}

	// Token: 0x060071B7 RID: 29111 RVA: 0x002B6DFC File Offset: 0x002B4FFC
	private void RemoveTimetableRow(GameObject row)
	{
		if (this.timetableRows.Count == 1)
		{
			return;
		}
		this.timeOfDayPositioner.SetTargetTimetable(null);
		int timetableToRemoveIdx = this.timetableRows.IndexOf(row);
		this.timetableRows.Remove(row);
		this.blockButtonsByTimetableRow.Remove(row);
		UnityEngine.Object.Destroy(row);
		this.schedule.RemoveTimetable(timetableToRemoveIdx);
	}

	// Token: 0x060071B8 RID: 29112 RVA: 0x002B6E5D File Offset: 0x002B505D
	public GameObject GetNameInputField()
	{
		return this.title.inputField.gameObject;
	}

	// Token: 0x060071B9 RID: 29113 RVA: 0x002B6E70 File Offset: 0x002B5070
	private void RebuildMinionWidgets()
	{
		if (this.IsNullOrDestroyed())
		{
			return;
		}
		if (!this.MinionWidgetsNeedRebuild())
		{
			return;
		}
		foreach (ScheduleMinionWidget original in this.minionWidgets)
		{
			Util.KDestroyGameObject(original);
		}
		this.minionWidgets.Clear();
		foreach (Ref<Schedulable> @ref in this.schedule.GetAssigned())
		{
			ScheduleMinionWidget scheduleMinionWidget = Util.KInstantiateUI<ScheduleMinionWidget>(this.minionWidgetPrefab.gameObject, this.minionWidgetContainer, true);
			scheduleMinionWidget.Setup(@ref.Get());
			this.minionWidgets.Add(scheduleMinionWidget);
		}
		if (Components.LiveMinionIdentities.Count > this.schedule.GetAssigned().Count)
		{
			this.blankMinionWidget.transform.SetAsLastSibling();
			this.blankMinionWidget.gameObject.SetActive(true);
			return;
		}
		this.blankMinionWidget.gameObject.SetActive(false);
	}

	// Token: 0x060071BA RID: 29114 RVA: 0x002B6F9C File Offset: 0x002B519C
	private bool MinionWidgetsNeedRebuild()
	{
		List<Ref<Schedulable>> assigned = this.schedule.GetAssigned();
		if (assigned.Count != this.minionWidgets.Count)
		{
			return true;
		}
		if (assigned.Count != Components.LiveMinionIdentities.Count != this.blankMinionWidget.gameObject.activeSelf)
		{
			return true;
		}
		for (int i = 0; i < assigned.Count; i++)
		{
			if (assigned[i].Get() != this.minionWidgets[i].schedulable)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060071BB RID: 29115 RVA: 0x002B702C File Offset: 0x002B522C
	public void RefreshWidgetWorldData()
	{
		foreach (ScheduleMinionWidget scheduleMinionWidget in this.minionWidgets)
		{
			if (!scheduleMinionWidget.IsNullOrDestroyed())
			{
				scheduleMinionWidget.RefreshWidgetWorldData();
			}
		}
	}

	// Token: 0x060071BC RID: 29116 RVA: 0x002B7088 File Offset: 0x002B5288
	private void OnNameChanged(string newName)
	{
		this.schedule.name = newName;
		base.gameObject.name = "Schedule_" + this.schedule.name;
	}

	// Token: 0x060071BD RID: 29117 RVA: 0x002B70B6 File Offset: 0x002B52B6
	private void OnAlarmClicked()
	{
		this.schedule.alarmActivated = !this.schedule.alarmActivated;
		this.RefreshAlarmButton();
	}

	// Token: 0x060071BE RID: 29118 RVA: 0x002B70D8 File Offset: 0x002B52D8
	private void RefreshAlarmButton()
	{
		this.alarmButton.ChangeState(this.schedule.alarmActivated ? 1 : 0);
		ToolTip component = this.alarmButton.GetComponent<ToolTip>();
		component.SetSimpleTooltip(this.schedule.alarmActivated ? UI.SCHEDULESCREEN.ALARM_BUTTON_ON_TOOLTIP : UI.SCHEDULESCREEN.ALARM_BUTTON_OFF_TOOLTIP);
		ToolTipScreen.Instance.MarkTooltipDirty(component);
		this.alarmField.text = (this.schedule.alarmActivated ? UI.SCHEDULESCREEN.ALARM_TITLE_ENABLED : UI.SCHEDULESCREEN.ALARM_TITLE_DISABLED);
	}

	// Token: 0x060071BF RID: 29119 RVA: 0x002B7165 File Offset: 0x002B5365
	private void OnResetClicked()
	{
		this.schedule.SetBlocksToGroupDefaults(Db.Get().ScheduleGroups.allGroups);
	}

	// Token: 0x060071C0 RID: 29120 RVA: 0x002B7181 File Offset: 0x002B5381
	private void OnDeleteClicked()
	{
		ScheduleManager.Instance.DeleteSchedule(this.schedule);
	}

	// Token: 0x060071C1 RID: 29121 RVA: 0x002B7194 File Offset: 0x002B5394
	private void OnScheduleChanged(Schedule changedSchedule)
	{
		foreach (KeyValuePair<GameObject, List<ScheduleBlockButton>> keyValuePair in this.blockButtonsByTimetableRow)
		{
			GameObject key = keyValuePair.Key;
			int num = this.timetableRows.IndexOf(key);
			List<ScheduleBlockButton> value = keyValuePair.Value;
			for (int i = 0; i < value.Count; i++)
			{
				int idx = num * 24 + i;
				value[i].SetBlockTypes(changedSchedule.GetBlock(idx).allowed_types);
			}
		}
		this.RefreshStatus();
		this.RebuildMinionWidgets();
	}

	// Token: 0x060071C2 RID: 29122 RVA: 0x002B7244 File Offset: 0x002B5444
	private void RefreshStatus()
	{
		this.blockTypeCounts.Clear();
		foreach (ScheduleBlockType scheduleBlockType in Db.Get().ScheduleBlockTypes.resources)
		{
			this.blockTypeCounts[scheduleBlockType.Id] = 0;
		}
		foreach (ScheduleBlock scheduleBlock in this.schedule.GetBlocks())
		{
			foreach (ScheduleBlockType scheduleBlockType2 in scheduleBlock.allowed_types)
			{
				Dictionary<string, int> dictionary = this.blockTypeCounts;
				string id = scheduleBlockType2.Id;
				int num = dictionary[id];
				dictionary[id] = num + 1;
			}
		}
		if (this.noteEntryRight == null)
		{
			return;
		}
		int num2 = 0;
		ToolTip component = this.noteEntryRight.GetComponent<ToolTip>();
		component.ClearMultiStringTooltip();
		foreach (KeyValuePair<string, int> keyValuePair in this.blockTypeCounts)
		{
			if (keyValuePair.Value == 0)
			{
				num2++;
				component.AddMultiStringTooltip(string.Format(UI.SCHEDULEGROUPS.NOTIME, Db.Get().ScheduleBlockTypes.Get(keyValuePair.Key).Name), null);
			}
		}
		if (num2 > 0)
		{
			this.noteEntryRight.text = string.Format(UI.SCHEDULEGROUPS.MISSINGBLOCKS, num2);
			return;
		}
		this.noteEntryRight.text = "";
	}

	// Token: 0x060071C3 RID: 29123 RVA: 0x002B7428 File Offset: 0x002B5628
	private void ConfigPaintButton(GameObject button, ScheduleGroup group, Sprite iconSprite)
	{
		string groupID = group.Id;
		button.GetComponent<MultiToggle>().onClick = delegate()
		{
			ScheduleScreen.Instance.SelectedPaint = groupID;
			ScheduleScreen.Instance.RefreshAllPaintButtons();
		};
		this.paintButtons.Add(group.Id, button);
		HierarchyReferences component = button.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("Icon").sprite = iconSprite;
		component.GetReference<LocText>("Label").text = group.Name;
	}

	// Token: 0x060071C4 RID: 29124 RVA: 0x002B749C File Offset: 0x002B569C
	public void RefreshPaintButtons()
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.paintButtons)
		{
			keyValuePair.Value.GetComponent<MultiToggle>().ChangeState((keyValuePair.Key == ScheduleScreen.Instance.SelectedPaint) ? 1 : 0);
		}
	}

	// Token: 0x060071C5 RID: 29125 RVA: 0x002B7518 File Offset: 0x002B5718
	public bool PaintBlock(ScheduleBlockButton blockButton)
	{
		foreach (KeyValuePair<GameObject, List<ScheduleBlockButton>> keyValuePair in this.blockButtonsByTimetableRow)
		{
			GameObject key = keyValuePair.Key;
			int i = 0;
			while (i < keyValuePair.Value.Count)
			{
				if (keyValuePair.Value[i] == blockButton)
				{
					int idx = this.timetableRows.IndexOf(key) * 24 + i;
					ScheduleGroup scheduleGroup = Db.Get().ScheduleGroups.Get(ScheduleScreen.Instance.SelectedPaint);
					if (this.schedule.GetBlock(idx).GroupId != scheduleGroup.Id)
					{
						this.schedule.SetBlockGroup(idx, scheduleGroup);
						return true;
					}
					return false;
				}
				else
				{
					i++;
				}
			}
		}
		return false;
	}

	// Token: 0x04004E73 RID: 20083
	[SerializeField]
	private ScheduleBlockButton blockButtonPrefab;

	// Token: 0x04004E74 RID: 20084
	[SerializeField]
	private ScheduleMinionWidget minionWidgetPrefab;

	// Token: 0x04004E75 RID: 20085
	[SerializeField]
	private GameObject minionWidgetContainer;

	// Token: 0x04004E76 RID: 20086
	private ScheduleMinionWidget blankMinionWidget;

	// Token: 0x04004E77 RID: 20087
	[SerializeField]
	private KButton duplicateScheduleButton;

	// Token: 0x04004E78 RID: 20088
	[SerializeField]
	private KButton deleteScheduleButton;

	// Token: 0x04004E79 RID: 20089
	[SerializeField]
	private EditableTitleBar title;

	// Token: 0x04004E7A RID: 20090
	[SerializeField]
	private LocText alarmField;

	// Token: 0x04004E7B RID: 20091
	[SerializeField]
	private KButton optionsButton;

	// Token: 0x04004E7C RID: 20092
	[SerializeField]
	private LocText noteEntryLeft;

	// Token: 0x04004E7D RID: 20093
	[SerializeField]
	private LocText noteEntryRight;

	// Token: 0x04004E7E RID: 20094
	[SerializeField]
	private MultiToggle alarmButton;

	// Token: 0x04004E7F RID: 20095
	private List<GameObject> timetableRows;

	// Token: 0x04004E80 RID: 20096
	private Dictionary<GameObject, List<ScheduleBlockButton>> blockButtonsByTimetableRow;

	// Token: 0x04004E81 RID: 20097
	private List<ScheduleMinionWidget> minionWidgets;

	// Token: 0x04004E82 RID: 20098
	[SerializeField]
	private GameObject timetableRowPrefab;

	// Token: 0x04004E83 RID: 20099
	[SerializeField]
	private GameObject timetableRowContainer;

	// Token: 0x04004E84 RID: 20100
	private Dictionary<string, GameObject> paintButtons = new Dictionary<string, GameObject>();

	// Token: 0x04004E85 RID: 20101
	[SerializeField]
	private GameObject PaintButtonBathtime;

	// Token: 0x04004E86 RID: 20102
	[SerializeField]
	private GameObject PaintButtonWorktime;

	// Token: 0x04004E87 RID: 20103
	[SerializeField]
	private GameObject PaintButtonRecreation;

	// Token: 0x04004E88 RID: 20104
	[SerializeField]
	private GameObject PaintButtonSleep;

	// Token: 0x04004E89 RID: 20105
	[SerializeField]
	private TimeOfDayPositioner timeOfDayPositioner;

	// Token: 0x04004E8B RID: 20107
	private Dictionary<string, int> blockTypeCounts = new Dictionary<string, int>();
}
