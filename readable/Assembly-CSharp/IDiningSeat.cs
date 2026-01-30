using System;

// Token: 0x020007B8 RID: 1976
public interface IDiningSeat
{
	// Token: 0x1700033D RID: 829
	// (get) Token: 0x06003439 RID: 13369
	bool HasSalt { get; }

	// Token: 0x1700033E RID: 830
	// (get) Token: 0x0600343A RID: 13370
	HashedString EatAnim { get; }

	// Token: 0x1700033F RID: 831
	// (get) Token: 0x0600343B RID: 13371
	HashedString ReloadElectrobankAnim { get; }

	// Token: 0x0600343C RID: 13372
	Storage FindStorage();

	// Token: 0x0600343D RID: 13373
	Operational FindOperational();

	// Token: 0x17000340 RID: 832
	// (get) Token: 0x0600343E RID: 13374
	// (set) Token: 0x0600343F RID: 13375
	KPrefabID Diner { get; set; }
}
