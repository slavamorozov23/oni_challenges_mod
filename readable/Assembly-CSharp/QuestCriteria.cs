using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000AB9 RID: 2745
public class QuestCriteria
{
	// Token: 0x17000575 RID: 1397
	// (get) Token: 0x06004FE9 RID: 20457 RVA: 0x001D0D20 File Offset: 0x001CEF20
	// (set) Token: 0x06004FEA RID: 20458 RVA: 0x001D0D28 File Offset: 0x001CEF28
	public string Text { get; private set; }

	// Token: 0x17000576 RID: 1398
	// (get) Token: 0x06004FEB RID: 20459 RVA: 0x001D0D31 File Offset: 0x001CEF31
	// (set) Token: 0x06004FEC RID: 20460 RVA: 0x001D0D39 File Offset: 0x001CEF39
	public string Tooltip { get; private set; }

	// Token: 0x06004FED RID: 20461 RVA: 0x001D0D44 File Offset: 0x001CEF44
	public QuestCriteria(Tag id, float[] targetValues = null, int requiredCount = 1, HashSet<Tag> acceptedTags = null, QuestCriteria.BehaviorFlags flags = QuestCriteria.BehaviorFlags.None)
	{
		global::Debug.Assert(targetValues == null || (targetValues.Length != 0 && targetValues.Length <= 32));
		this.CriteriaId = id;
		this.EvaluationBehaviors = flags;
		this.TargetValues = targetValues;
		this.AcceptedTags = acceptedTags;
		this.RequiredCount = requiredCount;
	}

	// Token: 0x06004FEE RID: 20462 RVA: 0x001D0DA0 File Offset: 0x001CEFA0
	public bool ValueSatisfies(float value, int valueHandle)
	{
		if (float.IsNaN(value))
		{
			return false;
		}
		float target = (this.TargetValues == null) ? 0f : this.TargetValues[valueHandle];
		return this.ValueSatisfies_Internal(value, target);
	}

	// Token: 0x06004FEF RID: 20463 RVA: 0x001D0DD7 File Offset: 0x001CEFD7
	protected virtual bool ValueSatisfies_Internal(float current, float target)
	{
		return true;
	}

	// Token: 0x06004FF0 RID: 20464 RVA: 0x001D0DDA File Offset: 0x001CEFDA
	public bool IsSatisfied(uint satisfactionState, uint satisfactionMask)
	{
		return (satisfactionState & satisfactionMask) == satisfactionMask;
	}

	// Token: 0x06004FF1 RID: 20465 RVA: 0x001D0DE4 File Offset: 0x001CEFE4
	public void PopulateStrings(string prefix)
	{
		string str = this.CriteriaId.Name.ToUpperInvariant();
		StringEntry stringEntry;
		if (Strings.TryGet(prefix + "CRITERIA." + str + ".NAME", out stringEntry))
		{
			this.Text = stringEntry.String;
		}
		if (Strings.TryGet(prefix + "CRITERIA." + str + ".TOOLTIP", out stringEntry))
		{
			this.Tooltip = stringEntry.String;
		}
	}

	// Token: 0x06004FF2 RID: 20466 RVA: 0x001D0E51 File Offset: 0x001CF051
	public uint GetSatisfactionMask()
	{
		if (this.TargetValues == null)
		{
			return 1U;
		}
		return (uint)Mathf.Pow(2f, (float)(this.TargetValues.Length - 1));
	}

	// Token: 0x06004FF3 RID: 20467 RVA: 0x001D0E73 File Offset: 0x001CF073
	public uint GetValueMask(int valueHandle)
	{
		if (this.TargetValues == null)
		{
			return 1U;
		}
		if (!QuestCriteria.HasBehavior(this.EvaluationBehaviors, QuestCriteria.BehaviorFlags.TrackArea))
		{
			valueHandle %= this.TargetValues.Length;
		}
		return 1U << valueHandle;
	}

	// Token: 0x06004FF4 RID: 20468 RVA: 0x001D0E9F File Offset: 0x001CF09F
	public static bool HasBehavior(QuestCriteria.BehaviorFlags flags, QuestCriteria.BehaviorFlags behavior)
	{
		return (flags & behavior) == behavior;
	}

	// Token: 0x0400355F RID: 13663
	public const int MAX_VALUES = 32;

	// Token: 0x04003560 RID: 13664
	public const int INVALID_VALUE = -1;

	// Token: 0x04003561 RID: 13665
	public readonly Tag CriteriaId;

	// Token: 0x04003562 RID: 13666
	public readonly QuestCriteria.BehaviorFlags EvaluationBehaviors;

	// Token: 0x04003563 RID: 13667
	public readonly float[] TargetValues;

	// Token: 0x04003564 RID: 13668
	public readonly int RequiredCount = 1;

	// Token: 0x04003565 RID: 13669
	public readonly HashSet<Tag> AcceptedTags;

	// Token: 0x02001C05 RID: 7173
	public enum BehaviorFlags
	{
		// Token: 0x040086BD RID: 34493
		None,
		// Token: 0x040086BE RID: 34494
		TrackArea,
		// Token: 0x040086BF RID: 34495
		AllowsRegression,
		// Token: 0x040086C0 RID: 34496
		TrackValues = 4,
		// Token: 0x040086C1 RID: 34497
		TrackItems = 8,
		// Token: 0x040086C2 RID: 34498
		UniqueItems = 24
	}
}
