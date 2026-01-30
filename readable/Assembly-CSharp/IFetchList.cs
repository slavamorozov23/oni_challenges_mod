using System;
using System.Collections.Generic;

// Token: 0x02000951 RID: 2385
public interface IFetchList
{
	// Token: 0x170004BB RID: 1211
	// (get) Token: 0x0600427E RID: 17022
	Storage Destination { get; }

	// Token: 0x0600427F RID: 17023
	float GetMinimumAmount(Tag tag);

	// Token: 0x06004280 RID: 17024
	Dictionary<Tag, float> GetRemaining();

	// Token: 0x06004281 RID: 17025
	Dictionary<Tag, float> GetRemainingMinimum();
}
