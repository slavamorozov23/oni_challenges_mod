using System;

// Token: 0x0200096C RID: 2412
public interface IEnergyProducer
{
	// Token: 0x170004D5 RID: 1237
	// (get) Token: 0x0600448B RID: 17547
	float JoulesAvailable { get; }

	// Token: 0x0600448C RID: 17548
	void ConsumeEnergy(float joules);
}
