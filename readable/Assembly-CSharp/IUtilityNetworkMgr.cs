using System;
using System.Collections.Generic;

// Token: 0x02000C0E RID: 3086
public interface IUtilityNetworkMgr
{
	// Token: 0x06005CAD RID: 23725
	bool CanAddConnection(UtilityConnections new_connection, int cell, bool is_physical_building, out string fail_reason);

	// Token: 0x06005CAE RID: 23726
	void AddConnection(UtilityConnections new_connection, int cell, bool is_physical_building);

	// Token: 0x06005CAF RID: 23727
	void StashVisualGrids();

	// Token: 0x06005CB0 RID: 23728
	void UnstashVisualGrids();

	// Token: 0x06005CB1 RID: 23729
	string GetVisualizerString(int cell);

	// Token: 0x06005CB2 RID: 23730
	string GetVisualizerString(UtilityConnections connections);

	// Token: 0x06005CB3 RID: 23731
	UtilityConnections GetConnections(int cell, bool is_physical_building);

	// Token: 0x06005CB4 RID: 23732
	UtilityConnections GetDisplayConnections(int cell);

	// Token: 0x06005CB5 RID: 23733
	void SetConnections(UtilityConnections connections, int cell, bool is_physical_building);

	// Token: 0x06005CB6 RID: 23734
	void ClearCell(int cell, bool is_physical_building);

	// Token: 0x06005CB7 RID: 23735
	void ForceRebuildNetworks();

	// Token: 0x06005CB8 RID: 23736
	void AddToNetworks(int cell, object item, bool is_endpoint);

	// Token: 0x06005CB9 RID: 23737
	void RemoveFromNetworks(int cell, object vent, bool is_endpoint);

	// Token: 0x06005CBA RID: 23738
	object GetEndpoint(int cell);

	// Token: 0x06005CBB RID: 23739
	UtilityNetwork GetNetworkForDirection(int cell, Direction direction);

	// Token: 0x06005CBC RID: 23740
	UtilityNetwork GetNetworkForCell(int cell);

	// Token: 0x06005CBD RID: 23741
	void AddNetworksRebuiltListener(Action<IList<UtilityNetwork>, ICollection<int>> listener);

	// Token: 0x06005CBE RID: 23742
	void RemoveNetworksRebuiltListener(Action<IList<UtilityNetwork>, ICollection<int>> listener);

	// Token: 0x06005CBF RID: 23743
	IList<UtilityNetwork> GetNetworks();
}
