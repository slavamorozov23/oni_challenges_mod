using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using FMOD.Studio;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000B35 RID: 2869
[AddComponentMenu("KMonoBehaviour/scripts/ScheduleManager")]
public class ScheduleManager : KMonoBehaviour, ISim33ms
{
	// Token: 0x14000020 RID: 32
	// (add) Token: 0x0600548C RID: 21644 RVA: 0x001EDE88 File Offset: 0x001EC088
	// (remove) Token: 0x0600548D RID: 21645 RVA: 0x001EDEC0 File Offset: 0x001EC0C0
	public event Action<List<Schedule>> onSchedulesChanged;

	// Token: 0x0600548E RID: 21646 RVA: 0x001EDEF5 File Offset: 0x001EC0F5
	public static void DestroyInstance()
	{
		ScheduleManager.Instance = null;
	}

	// Token: 0x0600548F RID: 21647 RVA: 0x001EDEFD File Offset: 0x001EC0FD
	public Schedule GetDefaultBionicSchedule()
	{
		return this.schedules.Find((Schedule match) => match.isDefaultForBionics);
	}

	// Token: 0x06005490 RID: 21648 RVA: 0x001EDF29 File Offset: 0x001EC129
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.schedules.Count == 0)
		{
			this.AddDefaultSchedule(true, true);
		}
	}

	// Token: 0x06005491 RID: 21649 RVA: 0x001EDF40 File Offset: 0x001EC140
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.schedules = new List<Schedule>();
		ScheduleManager.Instance = this;
	}

	// Token: 0x06005492 RID: 21650 RVA: 0x001EDF5C File Offset: 0x001EC15C
	protected override void OnSpawn()
	{
		if (this.schedules.Count == 0)
		{
			this.AddDefaultSchedule(true, true);
		}
		foreach (Schedule schedule in this.schedules)
		{
			schedule.ClearNullReferences();
		}
		List<ScheduleBlock> scheduleBlocksFromGroupDefaults = Schedule.GetScheduleBlocksFromGroupDefaults(Db.Get().ScheduleGroups.allGroups);
		foreach (Schedule schedule2 in this.schedules)
		{
			List<ScheduleBlock> blocks = schedule2.GetBlocks();
			for (int i = 0; i < blocks.Count; i++)
			{
				ScheduleBlock scheduleBlock = blocks[i];
				if (Db.Get().ScheduleGroups.FindGroupForScheduleTypes(scheduleBlock.allowed_types) == null)
				{
					ScheduleGroup group = Db.Get().ScheduleGroups.FindGroupForScheduleTypes(scheduleBlocksFromGroupDefaults[i].allowed_types);
					schedule2.SetBlockGroup(i, group);
				}
			}
		}
		foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
		{
			Schedulable component = minionIdentity.GetComponent<Schedulable>();
			if (this.GetSchedule(component) == null)
			{
				this.schedules[0].Assign(component);
			}
		}
		Components.LiveMinionIdentities.OnAdd += this.OnAddDupe;
		Components.LiveMinionIdentities.OnRemove += this.OnRemoveDupe;
	}

	// Token: 0x06005493 RID: 21651 RVA: 0x001EE108 File Offset: 0x001EC308
	private void OnAddDupe(MinionIdentity minion)
	{
		Schedulable component = minion.GetComponent<Schedulable>();
		if (component.GetSchedule() != null)
		{
			return;
		}
		Schedule schedule = this.schedules[0];
		if (minion.model == GameTags.Minions.Models.Bionic)
		{
			if (this.GetDefaultBionicSchedule() == null)
			{
				if (!this.hasDeletedDefaultBionicSchedule)
				{
					Schedule schedule2 = this.AddSchedule(Db.Get().ScheduleGroups.allGroups, UI.SCHEDULESCREEN.SCHEDULE_NAME_DEFAULT_BIONIC, true);
					schedule2.AddTimetable(Schedule.GetScheduleBlocksFromGroupDefaults(Db.Get().ScheduleGroups.allGroups));
					schedule2.AddTimetable(Schedule.GetScheduleBlocksFromGroupDefaults(Db.Get().ScheduleGroups.allGroups));
					for (int i = 0; i < schedule2.GetBlocks().Count; i++)
					{
						schedule2.SetBlockGroup(i, Db.Get().ScheduleGroups.Worktime);
					}
					for (int j = 1; j <= 6; j++)
					{
						schedule2.SetBlockGroup(schedule2.GetBlocks().Count - j, Db.Get().ScheduleGroups.Sleep);
					}
					for (int k = 7; k <= 12; k++)
					{
						schedule2.SetBlockGroup(schedule2.GetBlocks().Count - k, Db.Get().ScheduleGroups.Recreation);
					}
					schedule = schedule2;
					schedule2.isDefaultForBionics = true;
					if (this.onSchedulesChanged != null)
					{
						this.onSchedulesChanged(this.schedules);
					}
				}
			}
			else
			{
				schedule = this.GetDefaultBionicSchedule();
			}
		}
		else if (this.GetSchedule(component) != null)
		{
			schedule = this.GetSchedule(component);
		}
		schedule.Assign(component);
	}

	// Token: 0x06005494 RID: 21652 RVA: 0x001EE28C File Offset: 0x001EC48C
	private void OnRemoveDupe(MinionIdentity minion)
	{
		Schedulable component = minion.GetComponent<Schedulable>();
		Schedule schedule = this.GetSchedule(component);
		if (schedule != null)
		{
			schedule.Unassign(component);
		}
	}

	// Token: 0x06005495 RID: 21653 RVA: 0x001EE2B4 File Offset: 0x001EC4B4
	public void OnStoredDupeDestroyed(StoredMinionIdentity dupe)
	{
		foreach (Schedule schedule in this.schedules)
		{
			schedule.Unassign(dupe.gameObject.GetComponent<Schedulable>());
		}
	}

	// Token: 0x06005496 RID: 21654 RVA: 0x001EE310 File Offset: 0x001EC510
	public void AddDefaultSchedule(bool alarmOn, bool useDefaultName = true)
	{
		Schedule schedule = this.AddSchedule(Db.Get().ScheduleGroups.allGroups, useDefaultName ? UI.SCHEDULESCREEN.SCHEDULE_NAME_DEFAULT : UI.SCHEDULESCREEN.SCHEDULE_NAME_NEW, alarmOn);
		if (Game.Instance.FastWorkersModeActive)
		{
			for (int i = 0; i < 21; i++)
			{
				schedule.SetBlockGroup(i, Db.Get().ScheduleGroups.Worktime);
			}
			schedule.SetBlockGroup(21, Db.Get().ScheduleGroups.Recreation);
			schedule.SetBlockGroup(22, Db.Get().ScheduleGroups.Recreation);
			schedule.SetBlockGroup(23, Db.Get().ScheduleGroups.Sleep);
		}
	}

	// Token: 0x06005497 RID: 21655 RVA: 0x001EE3BC File Offset: 0x001EC5BC
	public Schedule AddSchedule(List<ScheduleGroup> groups, string name = null, bool alarmOn = false)
	{
		if (name == null)
		{
			this.scheduleNameIncrementor++;
			name = string.Format(UI.SCHEDULESCREEN.SCHEDULE_NAME_FORMAT, this.scheduleNameIncrementor.ToString());
		}
		Schedule schedule = new Schedule(name, groups, alarmOn);
		this.schedules.Add(schedule);
		if (this.onSchedulesChanged != null)
		{
			this.onSchedulesChanged(this.schedules);
		}
		return schedule;
	}

	// Token: 0x06005498 RID: 21656 RVA: 0x001EE428 File Offset: 0x001EC628
	public Schedule DuplicateSchedule(Schedule source)
	{
		if (base.name == null)
		{
			this.scheduleNameIncrementor++;
			base.name = string.Format(UI.SCHEDULESCREEN.SCHEDULE_NAME_FORMAT, this.scheduleNameIncrementor.ToString());
		}
		Schedule schedule = new Schedule("copy of " + source.name, source.GetBlocks(), source.alarmActivated);
		schedule.ProgressTimetableIdx = source.ProgressTimetableIdx;
		this.schedules.Add(schedule);
		if (this.onSchedulesChanged != null)
		{
			this.onSchedulesChanged(this.schedules);
		}
		return schedule;
	}

	// Token: 0x06005499 RID: 21657 RVA: 0x001EE4C0 File Offset: 0x001EC6C0
	public void DeleteSchedule(Schedule schedule)
	{
		if (this.schedules.Count == 1)
		{
			return;
		}
		List<Ref<Schedulable>> assigned = schedule.GetAssigned();
		if (schedule.isDefaultForBionics)
		{
			this.hasDeletedDefaultBionicSchedule = true;
		}
		this.schedules.Remove(schedule);
		foreach (Ref<Schedulable> @ref in assigned)
		{
			this.schedules[0].Assign(@ref.Get());
		}
		if (this.onSchedulesChanged != null)
		{
			this.onSchedulesChanged(this.schedules);
		}
	}

	// Token: 0x0600549A RID: 21658 RVA: 0x001EE568 File Offset: 0x001EC768
	public Schedule GetSchedule(Schedulable schedulable)
	{
		foreach (Schedule schedule in this.schedules)
		{
			if (schedule.IsAssigned(schedulable))
			{
				return schedule;
			}
		}
		return null;
	}

	// Token: 0x0600549B RID: 21659 RVA: 0x001EE5C4 File Offset: 0x001EC7C4
	public List<Schedule> GetSchedules()
	{
		return this.schedules;
	}

	// Token: 0x0600549C RID: 21660 RVA: 0x001EE5CC File Offset: 0x001EC7CC
	public bool IsAllowed(Schedulable schedulable, ScheduleBlockType schedule_block_type)
	{
		Schedule schedule = this.GetSchedule(schedulable);
		return schedule != null && schedule.GetCurrentScheduleBlock().IsAllowed(schedule_block_type);
	}

	// Token: 0x0600549D RID: 21661 RVA: 0x001EE5F2 File Offset: 0x001EC7F2
	public static int GetCurrentHour()
	{
		return Math.Min((int)(GameClock.Instance.GetCurrentCycleAsPercentage() * 24f), 23);
	}

	// Token: 0x0600549E RID: 21662 RVA: 0x001EE60C File Offset: 0x001EC80C
	public void Sim33ms(float dt)
	{
		int currentHour = ScheduleManager.GetCurrentHour();
		if (ScheduleManager.GetCurrentHour() != this.lastHour)
		{
			foreach (Schedule schedule in this.schedules)
			{
				schedule.Tick();
			}
			this.lastHour = currentHour;
		}
	}

	// Token: 0x0600549F RID: 21663 RVA: 0x001EE678 File Offset: 0x001EC878
	public void PlayScheduleAlarm(Schedule schedule, ScheduleBlock block, bool forwards)
	{
		Notification notification = new Notification(string.Format(MISC.NOTIFICATIONS.SCHEDULE_CHANGED.NAME, schedule.name, block.name), NotificationType.Good, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.SCHEDULE_CHANGED.TOOLTIP.Replace("{0}", schedule.name).Replace("{1}", block.name).Replace("{2}", Db.Get().ScheduleGroups.Get(block.GroupId).notificationTooltip), null, true, 0f, null, null, null, true, false, false);
		base.GetComponent<Notifier>().Add(notification, "");
		base.StartCoroutine(this.PlayScheduleTone(schedule, forwards));
	}

	// Token: 0x060054A0 RID: 21664 RVA: 0x001EE703 File Offset: 0x001EC903
	private IEnumerator PlayScheduleTone(Schedule schedule, bool forwards)
	{
		int[] tones = schedule.GetTones();
		int num2;
		for (int i = 0; i < tones.Length; i = num2 + 1)
		{
			int num = forwards ? i : (tones.Length - 1 - i);
			this.PlayTone(tones[num], forwards);
			yield return SequenceUtil.WaitForSeconds(TuningData<ScheduleManager.Tuning>.Get().toneSpacingSeconds);
			num2 = i;
		}
		yield break;
	}

	// Token: 0x060054A1 RID: 21665 RVA: 0x001EE720 File Offset: 0x001EC920
	private void PlayTone(int pitch, bool forwards)
	{
		EventInstance instance = KFMOD.BeginOneShot(GlobalAssets.GetSound("WorkChime_tone", false), Vector3.zero, 1f);
		instance.setParameterByName("WorkChime_pitch", (float)pitch, false);
		instance.setParameterByName("WorkChime_start", (float)(forwards ? 1 : 0), false);
		KFMOD.EndOneShot(instance);
	}

	// Token: 0x04003911 RID: 14609
	[Serialize]
	private List<Schedule> schedules;

	// Token: 0x04003912 RID: 14610
	[Serialize]
	private int lastHour;

	// Token: 0x04003913 RID: 14611
	[Serialize]
	private int scheduleNameIncrementor;

	// Token: 0x04003915 RID: 14613
	public static ScheduleManager Instance;

	// Token: 0x04003916 RID: 14614
	[Serialize]
	private bool hasDeletedDefaultBionicSchedule;

	// Token: 0x02001C9D RID: 7325
	public class Tuning : TuningData<ScheduleManager.Tuning>
	{
		// Token: 0x0400889B RID: 34971
		public float toneSpacingSeconds;

		// Token: 0x0400889C RID: 34972
		public int minToneIndex;

		// Token: 0x0400889D RID: 34973
		public int maxToneIndex;

		// Token: 0x0400889E RID: 34974
		public int firstLastToneSpacing;
	}
}
