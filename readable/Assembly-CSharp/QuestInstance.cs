using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000AB8 RID: 2744
[SerializationConfig(MemberSerialization.OptIn)]
public class QuestInstance : ISaveLoadable
{
	// Token: 0x1700056D RID: 1389
	// (get) Token: 0x06004FCC RID: 20428 RVA: 0x001D0102 File Offset: 0x001CE302
	public HashedString Id
	{
		get
		{
			return this.quest.IdHash;
		}
	}

	// Token: 0x1700056E RID: 1390
	// (get) Token: 0x06004FCD RID: 20429 RVA: 0x001D010F File Offset: 0x001CE30F
	public int CriteriaCount
	{
		get
		{
			return this.quest.Criteria.Length;
		}
	}

	// Token: 0x1700056F RID: 1391
	// (get) Token: 0x06004FCE RID: 20430 RVA: 0x001D011E File Offset: 0x001CE31E
	public string Name
	{
		get
		{
			return this.quest.Name;
		}
	}

	// Token: 0x17000570 RID: 1392
	// (get) Token: 0x06004FCF RID: 20431 RVA: 0x001D012B File Offset: 0x001CE32B
	public string CompletionText
	{
		get
		{
			return this.quest.CompletionText;
		}
	}

	// Token: 0x17000571 RID: 1393
	// (get) Token: 0x06004FD0 RID: 20432 RVA: 0x001D0138 File Offset: 0x001CE338
	public bool IsStarted
	{
		get
		{
			return this.currentState > Quest.State.NotStarted;
		}
	}

	// Token: 0x17000572 RID: 1394
	// (get) Token: 0x06004FD1 RID: 20433 RVA: 0x001D0143 File Offset: 0x001CE343
	public bool IsComplete
	{
		get
		{
			return this.currentState == Quest.State.Completed;
		}
	}

	// Token: 0x17000573 RID: 1395
	// (get) Token: 0x06004FD2 RID: 20434 RVA: 0x001D014E File Offset: 0x001CE34E
	// (set) Token: 0x06004FD3 RID: 20435 RVA: 0x001D0156 File Offset: 0x001CE356
	public float CurrentProgress { get; private set; }

	// Token: 0x17000574 RID: 1396
	// (get) Token: 0x06004FD4 RID: 20436 RVA: 0x001D015F File Offset: 0x001CE35F
	public Quest.State CurrentState
	{
		get
		{
			return this.currentState;
		}
	}

	// Token: 0x06004FD5 RID: 20437 RVA: 0x001D0168 File Offset: 0x001CE368
	public QuestInstance(Quest quest)
	{
		this.quest = quest;
		this.criteriaStates = new Dictionary<int, QuestInstance.CriteriaState>(quest.Criteria.Length);
		for (int i = 0; i < quest.Criteria.Length; i++)
		{
			QuestCriteria questCriteria = quest.Criteria[i];
			QuestInstance.CriteriaState value = new QuestInstance.CriteriaState
			{
				Handle = i
			};
			if (questCriteria.TargetValues != null)
			{
				if ((questCriteria.EvaluationBehaviors & QuestCriteria.BehaviorFlags.TrackItems) == QuestCriteria.BehaviorFlags.TrackItems)
				{
					value.SatisfyingItems = new Tag[questCriteria.TargetValues.Length * questCriteria.RequiredCount];
				}
				if ((questCriteria.EvaluationBehaviors & QuestCriteria.BehaviorFlags.TrackValues) == QuestCriteria.BehaviorFlags.TrackValues)
				{
					value.CurrentValues = new float[questCriteria.TargetValues.Length * questCriteria.RequiredCount];
				}
			}
			this.criteriaStates[questCriteria.CriteriaId.GetHash()] = value;
		}
	}

	// Token: 0x06004FD6 RID: 20438 RVA: 0x001D0238 File Offset: 0x001CE438
	public void Initialize(Quest quest)
	{
		this.quest = quest;
		this.ValidateCriteriasOnLoad();
		this.UpdateQuestProgress(false);
	}

	// Token: 0x06004FD7 RID: 20439 RVA: 0x001D024E File Offset: 0x001CE44E
	public bool HasCriteria(HashedString criteriaId)
	{
		return this.criteriaStates.ContainsKey(criteriaId.HashValue);
	}

	// Token: 0x06004FD8 RID: 20440 RVA: 0x001D0264 File Offset: 0x001CE464
	public bool HasBehavior(QuestCriteria.BehaviorFlags behavior)
	{
		bool flag = false;
		int num = 0;
		while (!flag && num < this.quest.Criteria.Length)
		{
			flag = ((this.quest.Criteria[num].EvaluationBehaviors & behavior) > QuestCriteria.BehaviorFlags.None);
			num++;
		}
		return flag;
	}

	// Token: 0x06004FD9 RID: 20441 RVA: 0x001D02A8 File Offset: 0x001CE4A8
	public int GetTargetCount(HashedString criteriaId)
	{
		QuestInstance.CriteriaState criteriaState;
		if (!this.criteriaStates.TryGetValue(criteriaId.HashValue, out criteriaState))
		{
			return 0;
		}
		return this.quest.Criteria[criteriaState.Handle].RequiredCount;
	}

	// Token: 0x06004FDA RID: 20442 RVA: 0x001D02E4 File Offset: 0x001CE4E4
	public int GetCurrentCount(HashedString criteriaId)
	{
		QuestInstance.CriteriaState criteriaState;
		if (!this.criteriaStates.TryGetValue(criteriaId.HashValue, out criteriaState))
		{
			return 0;
		}
		return criteriaState.CurrentCount;
	}

	// Token: 0x06004FDB RID: 20443 RVA: 0x001D0310 File Offset: 0x001CE510
	public float GetCurrentValue(HashedString criteriaId, int valueHandle = 0)
	{
		QuestInstance.CriteriaState criteriaState;
		if (!this.criteriaStates.TryGetValue(criteriaId.HashValue, out criteriaState) || criteriaState.CurrentValues == null)
		{
			return float.NaN;
		}
		return criteriaState.CurrentValues[valueHandle];
	}

	// Token: 0x06004FDC RID: 20444 RVA: 0x001D034C File Offset: 0x001CE54C
	public float GetTargetValue(HashedString criteriaId, int valueHandle = 0)
	{
		QuestInstance.CriteriaState criteriaState;
		if (!this.criteriaStates.TryGetValue(criteriaId.HashValue, out criteriaState))
		{
			return float.NaN;
		}
		if (this.quest.Criteria[criteriaState.Handle].TargetValues == null)
		{
			return float.NaN;
		}
		return this.quest.Criteria[criteriaState.Handle].TargetValues[valueHandle];
	}

	// Token: 0x06004FDD RID: 20445 RVA: 0x001D03B0 File Offset: 0x001CE5B0
	public Tag GetSatisfyingItem(HashedString criteriaId, int valueHandle = 0)
	{
		QuestInstance.CriteriaState criteriaState;
		if (!this.criteriaStates.TryGetValue(criteriaId.HashValue, out criteriaState) || criteriaState.SatisfyingItems == null)
		{
			return default(Tag);
		}
		return criteriaState.SatisfyingItems[valueHandle];
	}

	// Token: 0x06004FDE RID: 20446 RVA: 0x001D03F4 File Offset: 0x001CE5F4
	public float GetAreaAverage(HashedString criteriaId)
	{
		QuestInstance.CriteriaState criteriaState;
		if (!this.criteriaStates.TryGetValue(criteriaId.HashValue, out criteriaState))
		{
			return float.NaN;
		}
		if (!QuestCriteria.HasBehavior(this.quest.Criteria[criteriaState.Handle].EvaluationBehaviors, (QuestCriteria.BehaviorFlags)5))
		{
			return float.NaN;
		}
		float num = 0f;
		for (int i = 0; i < criteriaState.CurrentValues.Length; i++)
		{
			num += criteriaState.CurrentValues[i];
		}
		return num / (float)criteriaState.CurrentValues.Length;
	}

	// Token: 0x06004FDF RID: 20447 RVA: 0x001D0474 File Offset: 0x001CE674
	public bool IsItemRedundant(HashedString criteriaId, Tag item)
	{
		QuestInstance.CriteriaState criteriaState;
		if (!this.criteriaStates.TryGetValue(criteriaId.HashValue, out criteriaState) || criteriaState.SatisfyingItems == null)
		{
			return false;
		}
		bool flag = false;
		int num = 0;
		while (!flag && num < criteriaState.SatisfyingItems.Length)
		{
			flag = (criteriaState.SatisfyingItems[num] == item);
			num++;
		}
		return flag;
	}

	// Token: 0x06004FE0 RID: 20448 RVA: 0x001D04D0 File Offset: 0x001CE6D0
	public bool IsCriteriaSatisfied(HashedString id)
	{
		QuestInstance.CriteriaState criteriaState;
		return this.criteriaStates.TryGetValue(id.HashValue, out criteriaState) && this.quest.Criteria[criteriaState.Handle].IsSatisfied(criteriaState.SatisfactionState, this.GetSatisfactionMask(criteriaState));
	}

	// Token: 0x06004FE1 RID: 20449 RVA: 0x001D051C File Offset: 0x001CE71C
	public bool IsCriteriaSatisfied(Tag id)
	{
		QuestInstance.CriteriaState criteriaState;
		return this.criteriaStates.TryGetValue(id.GetHash(), out criteriaState) && this.quest.Criteria[criteriaState.Handle].IsSatisfied(criteriaState.SatisfactionState, this.GetSatisfactionMask(criteriaState));
	}

	// Token: 0x06004FE2 RID: 20450 RVA: 0x001D0568 File Offset: 0x001CE768
	public void TrackAreaForCriteria(HashedString criteriaId, Extents area)
	{
		QuestInstance.CriteriaState criteriaState;
		if (!this.criteriaStates.TryGetValue(criteriaId.HashValue, out criteriaState))
		{
			return;
		}
		int num = area.width * area.height;
		QuestCriteria questCriteria = this.quest.Criteria[criteriaState.Handle];
		global::Debug.Assert(num <= 32);
		if (QuestCriteria.HasBehavior(questCriteria.EvaluationBehaviors, QuestCriteria.BehaviorFlags.TrackValues))
		{
			criteriaState.CurrentValues = new float[num];
		}
		if (QuestCriteria.HasBehavior(questCriteria.EvaluationBehaviors, QuestCriteria.BehaviorFlags.TrackItems))
		{
			criteriaState.SatisfyingItems = new Tag[num];
		}
		this.criteriaStates[criteriaId.HashValue] = criteriaState;
	}

	// Token: 0x06004FE3 RID: 20451 RVA: 0x001D0604 File Offset: 0x001CE804
	private uint GetSatisfactionMask(QuestInstance.CriteriaState state)
	{
		QuestCriteria questCriteria = this.quest.Criteria[state.Handle];
		if (QuestCriteria.HasBehavior(questCriteria.EvaluationBehaviors, QuestCriteria.BehaviorFlags.TrackArea))
		{
			int num = 0;
			if (state.SatisfyingItems != null)
			{
				num = state.SatisfyingItems.Length;
			}
			else if (state.CurrentValues != null)
			{
				num = state.CurrentValues.Length;
			}
			return (uint)(Mathf.Pow(2f, (float)num) - 1f);
		}
		return questCriteria.GetSatisfactionMask();
	}

	// Token: 0x06004FE4 RID: 20452 RVA: 0x001D0674 File Offset: 0x001CE874
	public int TrackProgress(Quest.ItemData data, out bool dataSatisfies, out bool itemIsRedundant)
	{
		dataSatisfies = false;
		itemIsRedundant = false;
		QuestInstance.CriteriaState criteriaState;
		if (!this.criteriaStates.TryGetValue(data.CriteriaId.HashValue, out criteriaState))
		{
			return -1;
		}
		int valueHandle = data.ValueHandle;
		QuestCriteria questCriteria = this.quest.Criteria[criteriaState.Handle];
		dataSatisfies = this.DataSatisfiesCriteria(data, ref valueHandle);
		if (valueHandle == -1)
		{
			return valueHandle;
		}
		bool flag = QuestCriteria.HasBehavior(questCriteria.EvaluationBehaviors, QuestCriteria.BehaviorFlags.AllowsRegression);
		bool flag2 = QuestCriteria.HasBehavior(questCriteria.EvaluationBehaviors, QuestCriteria.BehaviorFlags.TrackItems);
		Tag tag = flag2 ? criteriaState.SatisfyingItems[valueHandle] : default(Tag);
		if (dataSatisfies)
		{
			itemIsRedundant = (QuestCriteria.HasBehavior(questCriteria.EvaluationBehaviors, QuestCriteria.BehaviorFlags.UniqueItems) && this.IsItemRedundant(data.CriteriaId, data.SatisfyingItem));
			if (itemIsRedundant)
			{
				return valueHandle;
			}
			tag = data.SatisfyingItem;
			criteriaState.SatisfactionState |= questCriteria.GetValueMask(valueHandle);
		}
		else if (flag)
		{
			criteriaState.SatisfactionState &= ~questCriteria.GetValueMask(valueHandle);
		}
		if (QuestCriteria.HasBehavior(questCriteria.EvaluationBehaviors, QuestCriteria.BehaviorFlags.TrackValues))
		{
			criteriaState.CurrentValues[valueHandle] = data.CurrentValue;
		}
		if (flag2)
		{
			criteriaState.SatisfyingItems[valueHandle] = tag;
		}
		bool flag3 = this.IsCriteriaSatisfied(data.CriteriaId);
		bool flag4 = questCriteria.IsSatisfied(criteriaState.SatisfactionState, this.GetSatisfactionMask(criteriaState));
		if (flag3 != flag4)
		{
			criteriaState.CurrentCount += (flag3 ? -1 : 1);
			if (flag4 && criteriaState.CurrentCount < questCriteria.RequiredCount)
			{
				criteriaState.SatisfactionState = 0U;
			}
		}
		this.criteriaStates[data.CriteriaId.HashValue] = criteriaState;
		this.UpdateQuestProgress(true);
		return valueHandle;
	}

	// Token: 0x06004FE5 RID: 20453 RVA: 0x001D0810 File Offset: 0x001CEA10
	public bool DataSatisfiesCriteria(Quest.ItemData data, ref int valueHandle)
	{
		QuestInstance.CriteriaState criteriaState;
		if (!this.criteriaStates.TryGetValue(data.CriteriaId.HashValue, out criteriaState))
		{
			return false;
		}
		QuestCriteria questCriteria = this.quest.Criteria[criteriaState.Handle];
		bool flag = questCriteria.AcceptedTags == null || (data.QualifyingTag.IsValid && questCriteria.AcceptedTags.Contains(data.QualifyingTag));
		if (flag && questCriteria.TargetValues == null)
		{
			valueHandle = 0;
		}
		if (!flag || valueHandle != -1)
		{
			return flag && questCriteria.ValueSatisfies(data.CurrentValue, valueHandle);
		}
		if (QuestCriteria.HasBehavior(questCriteria.EvaluationBehaviors, QuestCriteria.BehaviorFlags.TrackArea))
		{
			valueHandle = data.LocalCellId;
		}
		int num = -1;
		bool flag2 = QuestCriteria.HasBehavior(questCriteria.EvaluationBehaviors, QuestCriteria.BehaviorFlags.TrackValues);
		bool flag3 = false;
		int num2 = 0;
		while (!flag3 && num2 < questCriteria.TargetValues.Length)
		{
			if (questCriteria.ValueSatisfies(data.CurrentValue, num2))
			{
				flag3 = true;
				num = num2;
				break;
			}
			if (flag2 && (num == -1 || criteriaState.CurrentValues[num] > criteriaState.CurrentValues[num2]))
			{
				num = num2;
			}
			num2++;
		}
		if (valueHandle == -1 && num != -1)
		{
			valueHandle = questCriteria.RequiredCount * num + Mathf.Min(criteriaState.CurrentCount, questCriteria.RequiredCount - 1);
		}
		return flag3;
	}

	// Token: 0x06004FE6 RID: 20454 RVA: 0x001D0948 File Offset: 0x001CEB48
	private void UpdateQuestProgress(bool startQuest = false)
	{
		if (!this.IsStarted && !startQuest)
		{
			return;
		}
		float currentProgress = this.CurrentProgress;
		Quest.State state = this.currentState;
		this.currentState = Quest.State.InProgress;
		this.CurrentProgress = 0f;
		float num = 0f;
		for (int i = 0; i < this.quest.Criteria.Length; i++)
		{
			QuestCriteria questCriteria = this.quest.Criteria[i];
			QuestInstance.CriteriaState criteriaState = this.criteriaStates[questCriteria.CriteriaId.GetHash()];
			float num2 = (float)((questCriteria.TargetValues != null) ? questCriteria.TargetValues.Length : 1);
			num += (float)questCriteria.RequiredCount;
			this.CurrentProgress += (float)criteriaState.CurrentCount;
			if (!this.IsCriteriaSatisfied(questCriteria.CriteriaId))
			{
				float num3 = 0f;
				int num4 = 0;
				while (questCriteria.TargetValues != null && (float)num4 < num2)
				{
					if ((criteriaState.SatisfactionState & questCriteria.GetValueMask(num4)) == 0U)
					{
						if (QuestCriteria.HasBehavior(questCriteria.EvaluationBehaviors, QuestCriteria.BehaviorFlags.TrackValues))
						{
							int num5 = questCriteria.RequiredCount * num4 + Mathf.Min(criteriaState.CurrentCount, questCriteria.RequiredCount - 1);
							num3 += Mathf.Max(0f, criteriaState.CurrentValues[num5] / questCriteria.TargetValues[num4]);
						}
					}
					else
					{
						num3 += 1f;
					}
					num4++;
				}
				this.CurrentProgress += num3 / num2;
			}
		}
		this.CurrentProgress = Mathf.Clamp01(this.CurrentProgress / num);
		if (this.CurrentProgress == 1f)
		{
			this.currentState = Quest.State.Completed;
		}
		float num6 = this.CurrentProgress - currentProgress;
		if (state != this.currentState || Mathf.Abs(num6) > Mathf.Epsilon)
		{
			Action<QuestInstance, Quest.State, float> questProgressChanged = this.QuestProgressChanged;
			if (questProgressChanged == null)
			{
				return;
			}
			questProgressChanged(this, state, num6);
		}
	}

	// Token: 0x06004FE7 RID: 20455 RVA: 0x001D0B24 File Offset: 0x001CED24
	public ICheckboxListGroupControl.CheckboxItem[] GetCheckBoxData(Func<int, string, QuestInstance, string> resolveToolTip = null)
	{
		ICheckboxListGroupControl.CheckboxItem[] array = new ICheckboxListGroupControl.CheckboxItem[this.quest.Criteria.Length];
		for (int i = 0; i < this.quest.Criteria.Length; i++)
		{
			QuestCriteria c = this.quest.Criteria[i];
			array[i] = new ICheckboxListGroupControl.CheckboxItem
			{
				text = c.Text,
				isOn = this.IsCriteriaSatisfied(c.CriteriaId),
				tooltip = c.Tooltip
			};
			if (resolveToolTip != null)
			{
				array[i].resolveTooltipCallback = ((string tooltip, object owner) => resolveToolTip(c.CriteriaId.GetHash(), c.Tooltip, this));
			}
		}
		return array;
	}

	// Token: 0x06004FE8 RID: 20456 RVA: 0x001D0C0C File Offset: 0x001CEE0C
	public void ValidateCriteriasOnLoad()
	{
		if (this.criteriaStates.Count != this.quest.Criteria.Length)
		{
			Dictionary<int, QuestInstance.CriteriaState> dictionary = new Dictionary<int, QuestInstance.CriteriaState>(this.quest.Criteria.Length);
			for (int i = 0; i < this.quest.Criteria.Length; i++)
			{
				QuestCriteria questCriteria = this.quest.Criteria[i];
				int hash = questCriteria.CriteriaId.GetHash();
				if (this.criteriaStates.ContainsKey(hash))
				{
					dictionary[hash] = this.criteriaStates[hash];
				}
				else
				{
					QuestInstance.CriteriaState value = new QuestInstance.CriteriaState
					{
						Handle = i
					};
					if (questCriteria.TargetValues != null)
					{
						if ((questCriteria.EvaluationBehaviors & QuestCriteria.BehaviorFlags.TrackItems) == QuestCriteria.BehaviorFlags.TrackItems)
						{
							value.SatisfyingItems = new Tag[questCriteria.TargetValues.Length * questCriteria.RequiredCount];
						}
						if ((questCriteria.EvaluationBehaviors & QuestCriteria.BehaviorFlags.TrackValues) == QuestCriteria.BehaviorFlags.TrackValues)
						{
							value.CurrentValues = new float[questCriteria.TargetValues.Length * questCriteria.RequiredCount];
						}
					}
					dictionary[hash] = value;
				}
			}
			this.criteriaStates = dictionary;
		}
	}

	// Token: 0x0400355A RID: 13658
	public Action<QuestInstance, Quest.State, float> QuestProgressChanged;

	// Token: 0x0400355C RID: 13660
	private Quest quest;

	// Token: 0x0400355D RID: 13661
	[Serialize]
	private Dictionary<int, QuestInstance.CriteriaState> criteriaStates;

	// Token: 0x0400355E RID: 13662
	[Serialize]
	private Quest.State currentState;

	// Token: 0x02001C02 RID: 7170
	private struct CriteriaState
	{
		// Token: 0x0600AC3B RID: 44091 RVA: 0x003CB7D0 File Offset: 0x003C99D0
		public static bool ItemAlreadySatisfying(QuestInstance.CriteriaState state, Tag item)
		{
			bool result = false;
			int num = 0;
			while (state.SatisfyingItems != null && num < state.SatisfyingItems.Length)
			{
				if (state.SatisfyingItems[num] == item)
				{
					result = true;
					break;
				}
				num++;
			}
			return result;
		}

		// Token: 0x040086B3 RID: 34483
		public int Handle;

		// Token: 0x040086B4 RID: 34484
		public int CurrentCount;

		// Token: 0x040086B5 RID: 34485
		public uint SatisfactionState;

		// Token: 0x040086B6 RID: 34486
		public Tag[] SatisfyingItems;

		// Token: 0x040086B7 RID: 34487
		public float[] CurrentValues;
	}
}
