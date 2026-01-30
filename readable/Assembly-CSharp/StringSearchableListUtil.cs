using System;
using System.Linq;

// Token: 0x02000483 RID: 1155
public static class StringSearchableListUtil
{
	// Token: 0x06001868 RID: 6248 RVA: 0x00088484 File Offset: 0x00086684
	public static bool DoAnyTagsMatchFilter(string[] lowercaseTags, in string filter)
	{
		string text = filter.Trim().ToLowerInvariant();
		string[] source = text.Split(' ', StringSplitOptions.None);
		for (int i = 0; i < lowercaseTags.Length; i++)
		{
			string tag = lowercaseTags[i];
			if (StringSearchableListUtil.DoesTagMatchFilter(tag, text))
			{
				return true;
			}
			if ((from f in source
			select StringSearchableListUtil.DoesTagMatchFilter(tag, f)).All((bool result) => result))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001869 RID: 6249 RVA: 0x00088517 File Offset: 0x00086717
	public static bool DoesTagMatchFilter(string lowercaseTag, in string filter)
	{
		return string.IsNullOrWhiteSpace(filter) || lowercaseTag.Contains(filter);
	}

	// Token: 0x0600186A RID: 6250 RVA: 0x00088531 File Offset: 0x00086731
	public static bool ShouldUseFilter(string filter)
	{
		return !string.IsNullOrWhiteSpace(filter);
	}
}
