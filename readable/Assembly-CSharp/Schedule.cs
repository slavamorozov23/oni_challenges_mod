using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000B34 RID: 2868
[SerializationConfig(MemberSerialization.OptIn)]
public class Schedule : ISaveLoadable, IListableOption
{
	// Token: 0x170005EB RID: 1515
	// (get) Token: 0x0600546F RID: 21615 RVA: 0x001ED53C File Offset: 0x001EB73C
	// (set) Token: 0x06005470 RID: 21616 RVA: 0x001ED544 File Offset: 0x001EB744
	public int ProgressTimetableIdx
	{
		get
		{
			return this.progressTimetableIdx;
		}
		set
		{
			this.progressTimetableIdx = value;
		}
	}

	// Token: 0x06005471 RID: 21617 RVA: 0x001ED54D File Offset: 0x001EB74D
	public ScheduleBlock GetCurrentScheduleBlock()
	{
		return this.GetBlock(this.GetCurrentBlockIdx());
	}

	// Token: 0x06005472 RID: 21618 RVA: 0x001ED55B File Offset: 0x001EB75B
	public int GetCurrentBlockIdx()
	{
		return Math.Min((int)(GameClock.Instance.GetCurrentCycleAsPercentage() * 24f), 23) + this.progressTimetableIdx * 24;
	}

	// Token: 0x06005473 RID: 21619 RVA: 0x001ED57F File Offset: 0x001EB77F
	public ScheduleBlock GetPreviousScheduleBlock()
	{
		return this.GetBlock(this.GetPreviousBlockIdx());
	}

	// Token: 0x06005474 RID: 21620 RVA: 0x001ED590 File Offset: 0x001EB790
	public int GetPreviousBlockIdx()
	{
		int num = this.GetCurrentBlockIdx() - 1;
		if (num == -1)
		{
			num = this.blocks.Count - 1;
		}
		return num;
	}

	// Token: 0x06005475 RID: 21621 RVA: 0x001ED5B9 File Offset: 0x001EB7B9
	public void ClearNullReferences()
	{
		this.assigned.RemoveAll((Ref<Schedulable> x) => x.Get() == null);
	}

	// Token: 0x06005476 RID: 21622 RVA: 0x001ED5E8 File Offset: 0x001EB7E8
	public Schedule(string name, List<ScheduleGroup> defaultGroups, bool alarmActivated)
	{
		this.name = name;
		this.alarmActivated = alarmActivated;
		this.blocks = new List<ScheduleBlock>(defaultGroups.Count);
		this.assigned = new List<Ref<Schedulable>>();
		this.tones = this.GenerateTones();
		this.SetBlocksToGroupDefaults(defaultGroups);
	}

	// Token: 0x06005477 RID: 21623 RVA: 0x001ED640 File Offset: 0x001EB840
	public Schedule(string name, List<ScheduleBlock> sourceBlocks, bool alarmActivated)
	{
		this.name = name;
		this.alarmActivated = alarmActivated;
		this.blocks = new List<ScheduleBlock>();
		for (int i = 0; i < sourceBlocks.Count; i++)
		{
			this.blocks.Add(new ScheduleBlock(sourceBlocks[i].name, sourceBlocks[i].GroupId));
		}
		this.assigned = new List<Ref<Schedulable>>();
		this.tones = this.GenerateTones();
		this.Changed();
	}

	// Token: 0x06005478 RID: 21624 RVA: 0x001ED6C9 File Offset: 0x001EB8C9
	public void SetBlocksToGroupDefaults(List<ScheduleGroup> defaultGroups)
	{
		this.blocks = Schedule.GetScheduleBlocksFromGroupDefaults(defaultGroups);
		global::Debug.Assert(this.blocks.Count == 24);
		this.Changed();
	}

	// Token: 0x06005479 RID: 21625 RVA: 0x001ED6F4 File Offset: 0x001EB8F4
	public static List<ScheduleBlock> GetScheduleBlocksFromGroupDefaults(List<ScheduleGroup> defaultGroups)
	{
		List<ScheduleBlock> list = new List<ScheduleBlock>();
		for (int i = 0; i < defaultGroups.Count; i++)
		{
			ScheduleGroup scheduleGroup = defaultGroups[i];
			for (int j = 0; j < scheduleGroup.defaultSegments; j++)
			{
				list.Add(new ScheduleBlock(scheduleGroup.Name, scheduleGroup.Id));
			}
		}
		return list;
	}

	// Token: 0x0600547A RID: 21626 RVA: 0x001ED74C File Offset: 0x001EB94C
	public void Tick()
	{
		ScheduleBlock currentScheduleBlock = this.GetCurrentScheduleBlock();
		ScheduleBlock block = this.GetBlock(this.GetPreviousBlockIdx());
		global::Debug.Assert(block != currentScheduleBlock);
		if (this.GetCurrentBlockIdx() % 24 == 0)
		{
			this.progressTimetableIdx++;
			if (this.progressTimetableIdx >= this.blocks.Count / 24)
			{
				this.progressTimetableIdx = 0;
			}
			if (ScheduleScreen.Instance != null)
			{
				ScheduleScreen.Instance.OnChangeCurrentTimetable();
			}
		}
		if (!Schedule.AreScheduleTypesIdentical(currentScheduleBlock.allowed_types, block.allowed_types))
		{
			ScheduleGroup scheduleGroup = Db.Get().ScheduleGroups.FindGroupForScheduleTypes(currentScheduleBlock.allowed_types);
			ScheduleGroup scheduleGroup2 = Db.Get().ScheduleGroups.FindGroupForScheduleTypes(block.allowed_types);
			if (this.alarmActivated && scheduleGroup2.alarm != scheduleGroup.alarm)
			{
				ScheduleManager.Instance.PlayScheduleAlarm(this, currentScheduleBlock, scheduleGroup.alarm);
			}
			foreach (Ref<Schedulable> @ref in this.GetAssigned())
			{
				@ref.Get().OnScheduleBlocksChanged(this);
			}
		}
		foreach (Ref<Schedulable> ref2 in this.GetAssigned())
		{
			ref2.Get().OnScheduleBlocksTick(this);
		}
	}

	// Token: 0x0600547B RID: 21627 RVA: 0x001ED8C0 File Offset: 0x001EBAC0
	string IListableOption.GetProperName()
	{
		return this.name;
	}

	// Token: 0x0600547C RID: 21628 RVA: 0x001ED8C8 File Offset: 0x001EBAC8
	public int[] GenerateTones()
	{
		int minToneIndex = TuningData<ScheduleManager.Tuning>.Get().minToneIndex;
		int maxToneIndex = TuningData<ScheduleManager.Tuning>.Get().maxToneIndex;
		int firstLastToneSpacing = TuningData<ScheduleManager.Tuning>.Get().firstLastToneSpacing;
		int[] array = new int[4];
		array[0] = UnityEngine.Random.Range(minToneIndex, maxToneIndex - firstLastToneSpacing + 1);
		array[1] = UnityEngine.Random.Range(minToneIndex, maxToneIndex + 1);
		array[2] = UnityEngine.Random.Range(minToneIndex, maxToneIndex + 1);
		array[3] = UnityEngine.Random.Range(array[0] + firstLastToneSpacing, maxToneIndex + 1);
		return array;
	}

	// Token: 0x0600547D RID: 21629 RVA: 0x001ED934 File Offset: 0x001EBB34
	public List<Ref<Schedulable>> GetAssigned()
	{
		if (this.assigned == null)
		{
			this.assigned = new List<Ref<Schedulable>>();
		}
		return this.assigned;
	}

	// Token: 0x0600547E RID: 21630 RVA: 0x001ED94F File Offset: 0x001EBB4F
	public int[] GetTones()
	{
		if (this.tones == null)
		{
			this.tones = this.GenerateTones();
		}
		return this.tones;
	}

	// Token: 0x0600547F RID: 21631 RVA: 0x001ED96B File Offset: 0x001EBB6B
	public void SetBlockGroup(int idx, ScheduleGroup group)
	{
		if (0 <= idx && idx < this.blocks.Count)
		{
			this.blocks[idx] = new ScheduleBlock(group.Name, group.Id);
			this.Changed();
		}
	}

	// Token: 0x06005480 RID: 21632 RVA: 0x001ED9A4 File Offset: 0x001EBBA4
	private void Changed()
	{
		foreach (Ref<Schedulable> @ref in this.GetAssigned())
		{
			@ref.Get().OnScheduleChanged(this);
		}
		if (this.onChanged != null)
		{
			this.onChanged(this);
		}
	}

	// Token: 0x06005481 RID: 21633 RVA: 0x001EDA10 File Offset: 0x001EBC10
	public List<ScheduleBlock> GetBlocks()
	{
		return this.blocks;
	}

	// Token: 0x06005482 RID: 21634 RVA: 0x001EDA18 File Offset: 0x001EBC18
	public ScheduleBlock GetBlock(int idx)
	{
		return this.blocks[idx];
	}

	// Token: 0x06005483 RID: 21635 RVA: 0x001EDA26 File Offset: 0x001EBC26
	public void InsertTimetable(int timetableIdx, List<ScheduleBlock> newBlocks)
	{
		this.blocks.InsertRange(timetableIdx * 24, newBlocks);
		if (timetableIdx <= this.progressTimetableIdx)
		{
			this.progressTimetableIdx++;
		}
	}

	// Token: 0x06005484 RID: 21636 RVA: 0x001EDA4F File Offset: 0x001EBC4F
	public void AddTimetable(List<ScheduleBlock> newBlocks)
	{
		this.blocks.AddRange(newBlocks);
	}

	// Token: 0x06005485 RID: 21637 RVA: 0x001EDA60 File Offset: 0x001EBC60
	public void RemoveTimetable(int TimetableToRemoveIdx)
	{
		int index = TimetableToRemoveIdx * 24;
		int num = this.blocks.Count / 24;
		this.blocks.RemoveRange(index, 24);
		bool flag = TimetableToRemoveIdx == this.progressTimetableIdx;
		bool flag2 = this.progressTimetableIdx == num - 1;
		if (TimetableToRemoveIdx < this.progressTimetableIdx || (flag && flag2))
		{
			this.progressTimetableIdx--;
		}
		ScheduleScreen.Instance.OnChangeCurrentTimetable();
	}

	// Token: 0x06005486 RID: 21638 RVA: 0x001EDACB File Offset: 0x001EBCCB
	public void Assign(Schedulable schedulable)
	{
		if (!this.IsAssigned(schedulable))
		{
			this.GetAssigned().Add(new Ref<Schedulable>(schedulable));
		}
		this.Changed();
	}

	// Token: 0x06005487 RID: 21639 RVA: 0x001EDAF0 File Offset: 0x001EBCF0
	public void Unassign(Schedulable schedulable)
	{
		for (int i = 0; i < this.GetAssigned().Count; i++)
		{
			if (this.GetAssigned()[i].Get() == schedulable)
			{
				this.GetAssigned().RemoveAt(i);
				break;
			}
		}
		this.Changed();
	}

	// Token: 0x06005488 RID: 21640 RVA: 0x001EDB40 File Offset: 0x001EBD40
	public bool IsAssigned(Schedulable schedulable)
	{
		using (List<Ref<Schedulable>>.Enumerator enumerator = this.GetAssigned().GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Get() == schedulable)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06005489 RID: 21641 RVA: 0x001EDBA0 File Offset: 0x001EBDA0
	public static bool AreScheduleTypesIdentical(List<ScheduleBlockType> a, List<ScheduleBlockType> b)
	{
		if (a.Count != b.Count)
		{
			return false;
		}
		foreach (ScheduleBlockType scheduleBlockType in a)
		{
			bool flag = false;
			foreach (ScheduleBlockType scheduleBlockType2 in b)
			{
				if (scheduleBlockType.IdHash == scheduleBlockType2.IdHash)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600548A RID: 21642 RVA: 0x001EDC54 File Offset: 0x001EBE54
	public bool ShiftTimetable(bool up, int timetableToShiftIdx = 0)
	{
		if (timetableToShiftIdx == 0 && up)
		{
			return false;
		}
		if (timetableToShiftIdx == this.blocks.Count / 24 - 1 && !up)
		{
			return false;
		}
		int num = timetableToShiftIdx * 24;
		List<ScheduleBlock> collection = new List<ScheduleBlock>();
		List<ScheduleBlock> collection2 = new List<ScheduleBlock>();
		if (up)
		{
			collection = this.blocks.GetRange(num, 24);
			collection2 = this.blocks.GetRange(num - 24, 24);
			this.blocks.RemoveRange(num - 24, 48);
			this.blocks.InsertRange(num - 24, collection2);
			this.blocks.InsertRange(num - 24, collection);
		}
		else
		{
			collection = this.blocks.GetRange(num, 24);
			collection2 = this.blocks.GetRange(num + 24, 24);
			this.blocks.RemoveRange(num, 48);
			this.blocks.InsertRange(num, collection);
			this.blocks.InsertRange(num, collection2);
		}
		this.Changed();
		return true;
	}

	// Token: 0x0600548B RID: 21643 RVA: 0x001EDD3C File Offset: 0x001EBF3C
	public void RotateBlocks(bool directionLeft, int timetableToRotateIdx = 0)
	{
		List<ScheduleBlock> list = new List<ScheduleBlock>();
		int index = timetableToRotateIdx * 24;
		list = this.blocks.GetRange(index, 24);
		if (!directionLeft)
		{
			ScheduleGroup scheduleGroup = Db.Get().ScheduleGroups.Get(list[list.Count - 1].GroupId);
			for (int i = list.Count - 1; i >= 1; i--)
			{
				ScheduleGroup scheduleGroup2 = Db.Get().ScheduleGroups.Get(list[i - 1].GroupId);
				list[i].GroupId = scheduleGroup2.Id;
			}
			list[0].GroupId = scheduleGroup.Id;
		}
		else
		{
			ScheduleGroup scheduleGroup3 = Db.Get().ScheduleGroups.Get(list[0].GroupId);
			for (int j = 0; j < list.Count - 1; j++)
			{
				ScheduleGroup scheduleGroup4 = Db.Get().ScheduleGroups.Get(list[j + 1].GroupId);
				list[j].GroupId = scheduleGroup4.Id;
			}
			list[list.Count - 1].GroupId = scheduleGroup3.Id;
		}
		this.blocks.RemoveRange(index, 24);
		this.blocks.InsertRange(index, list);
		this.Changed();
	}

	// Token: 0x04003909 RID: 14601
	[Serialize]
	private List<ScheduleBlock> blocks;

	// Token: 0x0400390A RID: 14602
	[Serialize]
	private List<Ref<Schedulable>> assigned;

	// Token: 0x0400390B RID: 14603
	[Serialize]
	public string name;

	// Token: 0x0400390C RID: 14604
	[Serialize]
	public bool alarmActivated = true;

	// Token: 0x0400390D RID: 14605
	[Serialize]
	private int[] tones;

	// Token: 0x0400390E RID: 14606
	[Serialize]
	public bool isDefaultForBionics;

	// Token: 0x0400390F RID: 14607
	[Serialize]
	private int progressTimetableIdx;

	// Token: 0x04003910 RID: 14608
	public Action<Schedule> onChanged;
}
