using System;

// Token: 0x02000A93 RID: 2707
public struct PlantElementAbsorber
{
	// Token: 0x06004E9C RID: 20124 RVA: 0x001C95B9 File Offset: 0x001C77B9
	public void Clear()
	{
		this.storage = null;
		this.consumedElements = null;
	}

	// Token: 0x0400346F RID: 13423
	public Storage storage;

	// Token: 0x04003470 RID: 13424
	public PlantElementAbsorber.LocalInfo localInfo;

	// Token: 0x04003471 RID: 13425
	public HandleVector<int>.Handle[] accumulators;

	// Token: 0x04003472 RID: 13426
	public PlantElementAbsorber.ConsumeInfo[] consumedElements;

	// Token: 0x02001BAD RID: 7085
	public struct ConsumeInfo
	{
		// Token: 0x0600AABA RID: 43706 RVA: 0x003C515B File Offset: 0x003C335B
		public ConsumeInfo(Tag tag, float mass_consumption_rate)
		{
			this.tag = tag;
			this.massConsumptionRate = mass_consumption_rate;
		}

		// Token: 0x0400857B RID: 34171
		public Tag tag;

		// Token: 0x0400857C RID: 34172
		public float massConsumptionRate;
	}

	// Token: 0x02001BAE RID: 7086
	public struct LocalInfo
	{
		// Token: 0x0400857D RID: 34173
		public Tag tag;

		// Token: 0x0400857E RID: 34174
		public float massConsumptionRate;
	}
}
