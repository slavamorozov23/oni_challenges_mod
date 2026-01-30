using System;

// Token: 0x02000AB7 RID: 2743
public class Quest : Resource
{
	// Token: 0x06004FCB RID: 20427 RVA: 0x001D0068 File Offset: 0x001CE268
	public Quest(string id, QuestCriteria[] criteria) : base(id, id)
	{
		Debug.Assert(criteria.Length != 0);
		this.Criteria = criteria;
		string str = "STRINGS.CODEX.QUESTS." + id.ToUpperInvariant();
		StringEntry stringEntry;
		if (Strings.TryGet(str + ".NAME", out stringEntry))
		{
			this.Title = stringEntry.String;
		}
		if (Strings.TryGet(str + ".COMPLETE", out stringEntry))
		{
			this.CompletionText = stringEntry.String;
		}
		for (int i = 0; i < this.Criteria.Length; i++)
		{
			this.Criteria[i].PopulateStrings("STRINGS.CODEX.QUESTS.");
		}
	}

	// Token: 0x04003556 RID: 13654
	public const string STRINGS_PREFIX = "STRINGS.CODEX.QUESTS.";

	// Token: 0x04003557 RID: 13655
	public readonly QuestCriteria[] Criteria;

	// Token: 0x04003558 RID: 13656
	public readonly string Title;

	// Token: 0x04003559 RID: 13657
	public readonly string CompletionText;

	// Token: 0x02001C00 RID: 7168
	public struct ItemData
	{
		// Token: 0x17000C2E RID: 3118
		// (get) Token: 0x0600AC39 RID: 44089 RVA: 0x003CB7B9 File Offset: 0x003C99B9
		// (set) Token: 0x0600AC3A RID: 44090 RVA: 0x003CB7C3 File Offset: 0x003C99C3
		public int ValueHandle
		{
			get
			{
				return this.valueHandle - 1;
			}
			set
			{
				this.valueHandle = value + 1;
			}
		}

		// Token: 0x040086A9 RID: 34473
		public int LocalCellId;

		// Token: 0x040086AA RID: 34474
		public float CurrentValue;

		// Token: 0x040086AB RID: 34475
		public Tag SatisfyingItem;

		// Token: 0x040086AC RID: 34476
		public Tag QualifyingTag;

		// Token: 0x040086AD RID: 34477
		public HashedString CriteriaId;

		// Token: 0x040086AE RID: 34478
		private int valueHandle;
	}

	// Token: 0x02001C01 RID: 7169
	public enum State
	{
		// Token: 0x040086B0 RID: 34480
		NotStarted,
		// Token: 0x040086B1 RID: 34481
		InProgress,
		// Token: 0x040086B2 RID: 34482
		Completed
	}
}
