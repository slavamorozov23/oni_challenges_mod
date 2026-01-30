using System;

// Token: 0x02000921 RID: 2337
public class EntityConfigOrder : Attribute
{
	// Token: 0x06004173 RID: 16755 RVA: 0x00171C02 File Offset: 0x0016FE02
	public EntityConfigOrder(int sort_order)
	{
		this.sortOrder = sort_order;
	}

	// Token: 0x040028E8 RID: 10472
	public int sortOrder;
}
