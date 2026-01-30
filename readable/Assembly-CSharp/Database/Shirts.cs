using System;

namespace Database
{
	// Token: 0x02000F56 RID: 3926
	public class Shirts : ResourceSet<Shirt>
	{
		// Token: 0x06007CC5 RID: 31941 RVA: 0x00317988 File Offset: 0x00315B88
		public Shirts()
		{
			this.Hot00 = base.Add(new Shirt("body_shirt_hot_shearling"));
			this.Decor00 = base.Add(new Shirt("body_shirt_decor01"));
		}

		// Token: 0x04005B63 RID: 23395
		public Shirt Hot00;

		// Token: 0x04005B64 RID: 23396
		public Shirt Decor00;
	}
}
