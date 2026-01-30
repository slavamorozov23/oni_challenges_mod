using System;
using System.Collections.Generic;

// Token: 0x02000C86 RID: 3206
public class UIStringFormatter
{
	// Token: 0x040042CF RID: 17103
	private List<UIStringFormatter.Entry> entries = new List<UIStringFormatter.Entry>();

	// Token: 0x02001EB7 RID: 7863
	private struct Entry
	{
		// Token: 0x04009071 RID: 36977
		public string format;

		// Token: 0x04009072 RID: 36978
		public string key;

		// Token: 0x04009073 RID: 36979
		public string value;

		// Token: 0x04009074 RID: 36980
		public string result;
	}
}
