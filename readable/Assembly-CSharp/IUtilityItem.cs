using System;

// Token: 0x02000C06 RID: 3078
public interface IUtilityItem
{
	// Token: 0x170006AF RID: 1711
	// (get) Token: 0x06005C82 RID: 23682
	// (set) Token: 0x06005C83 RID: 23683
	UtilityConnections Connections { get; set; }

	// Token: 0x06005C84 RID: 23684
	void UpdateConnections(UtilityConnections Connections);

	// Token: 0x06005C85 RID: 23685
	int GetNetworkID();

	// Token: 0x06005C86 RID: 23686
	UtilityNetwork GetNetworkForDirection(Direction d);
}
