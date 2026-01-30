using System;

// Token: 0x0200090B RID: 2315
internal struct EffectorEntryDecibel
{
	// Token: 0x06004059 RID: 16473 RVA: 0x0016CD05 File Offset: 0x0016AF05
	public EffectorEntryDecibel(string name, float value)
	{
		this.name = name;
		this.value = value;
		this.count = 1;
	}

	// Token: 0x040027F6 RID: 10230
	public string name;

	// Token: 0x040027F7 RID: 10231
	public int count;

	// Token: 0x040027F8 RID: 10232
	public float value;
}
