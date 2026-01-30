using System;
using System.Collections.Generic;

// Token: 0x02000C85 RID: 3205
public class UIFloatFormatter
{
	// Token: 0x0600623E RID: 25150 RVA: 0x00244D2F File Offset: 0x00242F2F
	public string Format(string format, float value)
	{
		return this.Replace(format, "{0}", value);
	}

	// Token: 0x0600623F RID: 25151 RVA: 0x00244D40 File Offset: 0x00242F40
	private string Replace(string format, string key, float value)
	{
		UIFloatFormatter.Entry entry = default(UIFloatFormatter.Entry);
		if (this.activeStringCount >= this.entries.Count)
		{
			entry.format = format;
			entry.key = key;
			entry.value = value;
			entry.result = entry.format.Replace(key, value.ToString());
			this.entries.Add(entry);
		}
		else
		{
			entry = this.entries[this.activeStringCount];
			if (entry.format != format || entry.key != key || entry.value != value)
			{
				entry.format = format;
				entry.key = key;
				entry.value = value;
				entry.result = entry.format.Replace(key, value.ToString());
				this.entries[this.activeStringCount] = entry;
			}
		}
		this.activeStringCount++;
		return entry.result;
	}

	// Token: 0x06006240 RID: 25152 RVA: 0x00244E37 File Offset: 0x00243037
	public void BeginDrawing()
	{
		this.activeStringCount = 0;
	}

	// Token: 0x06006241 RID: 25153 RVA: 0x00244E40 File Offset: 0x00243040
	public void EndDrawing()
	{
	}

	// Token: 0x040042CD RID: 17101
	private int activeStringCount;

	// Token: 0x040042CE RID: 17102
	private List<UIFloatFormatter.Entry> entries = new List<UIFloatFormatter.Entry>();

	// Token: 0x02001EB6 RID: 7862
	private struct Entry
	{
		// Token: 0x0400906D RID: 36973
		public string format;

		// Token: 0x0400906E RID: 36974
		public string key;

		// Token: 0x0400906F RID: 36975
		public float value;

		// Token: 0x04009070 RID: 36976
		public string result;
	}
}
