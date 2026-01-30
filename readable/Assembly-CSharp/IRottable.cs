using System;
using UnityEngine;

// Token: 0x0200062E RID: 1582
public interface IRottable
{
	// Token: 0x170001AB RID: 427
	// (get) Token: 0x060025C3 RID: 9667
	GameObject gameObject { get; }

	// Token: 0x170001AC RID: 428
	// (get) Token: 0x060025C4 RID: 9668
	float RotTemperature { get; }

	// Token: 0x170001AD RID: 429
	// (get) Token: 0x060025C5 RID: 9669
	float PreserveTemperature { get; }
}
