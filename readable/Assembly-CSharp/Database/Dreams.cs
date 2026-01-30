using System;

namespace Database
{
	// Token: 0x02000F38 RID: 3896
	public class Dreams : ResourceSet<Dream>
	{
		// Token: 0x06007C5E RID: 31838 RVA: 0x0030EE39 File Offset: 0x0030D039
		public Dreams(ResourceSet parent) : base("Dreams", parent)
		{
			this.CommonDream = new Dream("CommonDream", this, "dream_tear_swirly_kanim", new string[]
			{
				"dreamIcon_journal"
			});
		}

		// Token: 0x04005963 RID: 22883
		public Dream CommonDream;
	}
}
