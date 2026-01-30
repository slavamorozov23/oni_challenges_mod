using System;
using System.Collections.Generic;

// Token: 0x02000BF0 RID: 3056
public class TagCollection : IReadonlyTags
{
	// Token: 0x06005BBA RID: 23482 RVA: 0x00212C49 File Offset: 0x00210E49
	public TagCollection()
	{
	}

	// Token: 0x06005BBB RID: 23483 RVA: 0x00212C5C File Offset: 0x00210E5C
	public TagCollection(int[] initialTags)
	{
		for (int i = 0; i < initialTags.Length; i++)
		{
			this.tags.Add(initialTags[i]);
		}
	}

	// Token: 0x06005BBC RID: 23484 RVA: 0x00212C98 File Offset: 0x00210E98
	public TagCollection(string[] initialTags)
	{
		for (int i = 0; i < initialTags.Length; i++)
		{
			this.tags.Add(Hash.SDBMLower(initialTags[i]));
		}
	}

	// Token: 0x06005BBD RID: 23485 RVA: 0x00212CD8 File Offset: 0x00210ED8
	public TagCollection(TagCollection initialTags)
	{
		if (initialTags != null && initialTags.tags != null)
		{
			this.tags.UnionWith(initialTags.tags);
		}
	}

	// Token: 0x06005BBE RID: 23486 RVA: 0x00212D08 File Offset: 0x00210F08
	public TagCollection Append(TagCollection others)
	{
		foreach (int item in others.tags)
		{
			this.tags.Add(item);
		}
		return this;
	}

	// Token: 0x06005BBF RID: 23487 RVA: 0x00212D64 File Offset: 0x00210F64
	public void AddTag(string tag)
	{
		this.tags.Add(Hash.SDBMLower(tag));
	}

	// Token: 0x06005BC0 RID: 23488 RVA: 0x00212D78 File Offset: 0x00210F78
	public void AddTag(int tag)
	{
		this.tags.Add(tag);
	}

	// Token: 0x06005BC1 RID: 23489 RVA: 0x00212D87 File Offset: 0x00210F87
	public void RemoveTag(string tag)
	{
		this.tags.Remove(Hash.SDBMLower(tag));
	}

	// Token: 0x06005BC2 RID: 23490 RVA: 0x00212D9B File Offset: 0x00210F9B
	public void RemoveTag(int tag)
	{
		this.tags.Remove(tag);
	}

	// Token: 0x06005BC3 RID: 23491 RVA: 0x00212DAA File Offset: 0x00210FAA
	public bool HasTag(string tag)
	{
		return this.tags.Contains(Hash.SDBMLower(tag));
	}

	// Token: 0x06005BC4 RID: 23492 RVA: 0x00212DBD File Offset: 0x00210FBD
	public bool HasTag(int tag)
	{
		return this.tags.Contains(tag);
	}

	// Token: 0x06005BC5 RID: 23493 RVA: 0x00212DCC File Offset: 0x00210FCC
	public bool HasTags(int[] searchTags)
	{
		for (int i = 0; i < searchTags.Length; i++)
		{
			if (!this.tags.Contains(searchTags[i]))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x04003D0F RID: 15631
	private HashSet<int> tags = new HashSet<int>();
}
