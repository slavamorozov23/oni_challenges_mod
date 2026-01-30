using System;
using System.Collections.Generic;

// Token: 0x02000CBC RID: 3260
public class CategoryEntry : CodexEntry
{
	// Token: 0x17000728 RID: 1832
	// (get) Token: 0x06006421 RID: 25633 RVA: 0x002552CE File Offset: 0x002534CE
	// (set) Token: 0x06006422 RID: 25634 RVA: 0x002552D6 File Offset: 0x002534D6
	public bool largeFormat { get; set; }

	// Token: 0x17000729 RID: 1833
	// (get) Token: 0x06006423 RID: 25635 RVA: 0x002552DF File Offset: 0x002534DF
	// (set) Token: 0x06006424 RID: 25636 RVA: 0x002552E7 File Offset: 0x002534E7
	public bool sort { get; set; }

	// Token: 0x06006425 RID: 25637 RVA: 0x002552F0 File Offset: 0x002534F0
	public CategoryEntry(string category, List<ContentContainer> contentContainers, string name, List<CodexEntry> entriesInCategory, bool largeFormat, bool sort) : base(category, contentContainers, name)
	{
		this.entriesInCategory = entriesInCategory;
		this.largeFormat = largeFormat;
		this.sort = sort;
	}

	// Token: 0x04004415 RID: 17429
	public List<CodexEntry> entriesInCategory = new List<CodexEntry>();
}
