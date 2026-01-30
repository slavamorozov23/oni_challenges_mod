using System;
using System.Collections.Generic;

namespace Database
{
	// Token: 0x02000F17 RID: 3863
	public class MooSongModifiers : ResourceSet<MooSongModifier>
	{
		// Token: 0x06007BF8 RID: 31736 RVA: 0x003012C4 File Offset: 0x002FF4C4
		public List<MooSongModifier> GetForTag(Tag searchTag)
		{
			List<MooSongModifier> list = new List<MooSongModifier>();
			foreach (MooSongModifier mooSongModifier in this.resources)
			{
				if (mooSongModifier.TargetTag == searchTag)
				{
					list.Add(mooSongModifier);
				}
			}
			return list;
		}
	}
}
