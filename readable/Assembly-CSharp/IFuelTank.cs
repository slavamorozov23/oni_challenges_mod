using System;

// Token: 0x02000993 RID: 2451
public interface IFuelTank
{
	// Token: 0x17000506 RID: 1286
	// (get) Token: 0x06004678 RID: 18040
	IStorage Storage { get; }

	// Token: 0x17000507 RID: 1287
	// (get) Token: 0x06004679 RID: 18041
	bool ConsumeFuelOnLand { get; }

	// Token: 0x0600467A RID: 18042
	void DEBUG_FillTank();
}
