using System;
using Klei.AI;

// Token: 0x02000C8D RID: 3213
public interface IAmountDisplayer
{
	// Token: 0x060062A0 RID: 25248
	string GetValueString(Amount master, AmountInstance instance);

	// Token: 0x060062A1 RID: 25249
	string GetDescription(Amount master, AmountInstance instance);

	// Token: 0x060062A2 RID: 25250
	string GetTooltip(Amount master, AmountInstance instance);

	// Token: 0x1700070D RID: 1805
	// (get) Token: 0x060062A3 RID: 25251
	IAttributeFormatter Formatter { get; }
}
