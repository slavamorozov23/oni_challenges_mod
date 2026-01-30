using System;
using System.Collections.Generic;

// Token: 0x02000728 RID: 1832
public interface IBridgedNetworkItem
{
	// Token: 0x06002DFF RID: 11775
	void AddNetworks(ICollection<UtilityNetwork> networks);

	// Token: 0x06002E00 RID: 11776
	bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks);

	// Token: 0x06002E01 RID: 11777
	int GetNetworkCell();
}
