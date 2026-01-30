using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C09 RID: 3081
public class FlowUtilityNetwork : UtilityNetwork
{
	// Token: 0x170006B0 RID: 1712
	// (get) Token: 0x06005C94 RID: 23700 RVA: 0x002188EE File Offset: 0x00216AEE
	public bool HasSinks
	{
		get
		{
			return this.sinks.Count > 0;
		}
	}

	// Token: 0x06005C95 RID: 23701 RVA: 0x002188FE File Offset: 0x00216AFE
	public int GetActiveCount()
	{
		return this.sinks.Count;
	}

	// Token: 0x06005C96 RID: 23702 RVA: 0x0021890C File Offset: 0x00216B0C
	public override void AddItem(object generic_item)
	{
		FlowUtilityNetwork.IItem item = (FlowUtilityNetwork.IItem)generic_item;
		if (item != null)
		{
			switch (item.EndpointType)
			{
			case Endpoint.Source:
				if (this.sources.Contains(item))
				{
					return;
				}
				this.sources.Add(item);
				item.Network = this;
				return;
			case Endpoint.Sink:
				if (this.sinks.Contains(item))
				{
					return;
				}
				this.sinks.Add(item);
				item.Network = this;
				return;
			case Endpoint.Conduit:
				this.conduitCount++;
				return;
			default:
				item.Network = this;
				break;
			}
		}
	}

	// Token: 0x06005C97 RID: 23703 RVA: 0x0021899C File Offset: 0x00216B9C
	public override void Reset(UtilityNetworkGridNode[] grid)
	{
		for (int i = 0; i < this.sinks.Count; i++)
		{
			FlowUtilityNetwork.IItem item = this.sinks[i];
			item.Network = null;
			UtilityNetworkGridNode utilityNetworkGridNode = grid[item.Cell];
			utilityNetworkGridNode.networkIdx = -1;
			grid[item.Cell] = utilityNetworkGridNode;
		}
		for (int j = 0; j < this.sources.Count; j++)
		{
			FlowUtilityNetwork.IItem item2 = this.sources[j];
			item2.Network = null;
			UtilityNetworkGridNode utilityNetworkGridNode2 = grid[item2.Cell];
			utilityNetworkGridNode2.networkIdx = -1;
			grid[item2.Cell] = utilityNetworkGridNode2;
		}
		this.conduitCount = 0;
		for (int k = 0; k < this.conduits.Count; k++)
		{
			FlowUtilityNetwork.IItem item3 = this.conduits[k];
			item3.Network = null;
			UtilityNetworkGridNode utilityNetworkGridNode3 = grid[item3.Cell];
			utilityNetworkGridNode3.networkIdx = -1;
			grid[item3.Cell] = utilityNetworkGridNode3;
		}
	}

	// Token: 0x04003DAD RID: 15789
	public List<FlowUtilityNetwork.IItem> sources = new List<FlowUtilityNetwork.IItem>();

	// Token: 0x04003DAE RID: 15790
	public List<FlowUtilityNetwork.IItem> sinks = new List<FlowUtilityNetwork.IItem>();

	// Token: 0x04003DAF RID: 15791
	public List<FlowUtilityNetwork.IItem> conduits = new List<FlowUtilityNetwork.IItem>();

	// Token: 0x04003DB0 RID: 15792
	public int conduitCount;

	// Token: 0x02001DA3 RID: 7587
	public interface IItem
	{
		// Token: 0x17000C7C RID: 3196
		// (get) Token: 0x0600B1A4 RID: 45476
		int Cell { get; }

		// Token: 0x17000C7D RID: 3197
		// (set) Token: 0x0600B1A5 RID: 45477
		FlowUtilityNetwork Network { set; }

		// Token: 0x17000C7E RID: 3198
		// (get) Token: 0x0600B1A6 RID: 45478
		Endpoint EndpointType { get; }

		// Token: 0x17000C7F RID: 3199
		// (get) Token: 0x0600B1A7 RID: 45479
		ConduitType ConduitType { get; }

		// Token: 0x17000C80 RID: 3200
		// (get) Token: 0x0600B1A8 RID: 45480
		GameObject GameObject { get; }
	}

	// Token: 0x02001DA4 RID: 7588
	public class NetworkItem : FlowUtilityNetwork.IItem
	{
		// Token: 0x0600B1A9 RID: 45481 RVA: 0x003DDAC1 File Offset: 0x003DBCC1
		public NetworkItem(ConduitType conduit_type, Endpoint endpoint_type, int cell, GameObject parent)
		{
			this.conduitType = conduit_type;
			this.endpointType = endpoint_type;
			this.cell = cell;
			this.parent = parent;
		}

		// Token: 0x17000C81 RID: 3201
		// (get) Token: 0x0600B1AA RID: 45482 RVA: 0x003DDAE6 File Offset: 0x003DBCE6
		public Endpoint EndpointType
		{
			get
			{
				return this.endpointType;
			}
		}

		// Token: 0x17000C82 RID: 3202
		// (get) Token: 0x0600B1AB RID: 45483 RVA: 0x003DDAEE File Offset: 0x003DBCEE
		public ConduitType ConduitType
		{
			get
			{
				return this.conduitType;
			}
		}

		// Token: 0x17000C83 RID: 3203
		// (get) Token: 0x0600B1AC RID: 45484 RVA: 0x003DDAF6 File Offset: 0x003DBCF6
		public int Cell
		{
			get
			{
				return this.cell;
			}
		}

		// Token: 0x17000C84 RID: 3204
		// (get) Token: 0x0600B1AD RID: 45485 RVA: 0x003DDAFE File Offset: 0x003DBCFE
		// (set) Token: 0x0600B1AE RID: 45486 RVA: 0x003DDB06 File Offset: 0x003DBD06
		public FlowUtilityNetwork Network
		{
			get
			{
				return this.network;
			}
			set
			{
				this.network = value;
			}
		}

		// Token: 0x17000C85 RID: 3205
		// (get) Token: 0x0600B1AF RID: 45487 RVA: 0x003DDB0F File Offset: 0x003DBD0F
		public GameObject GameObject
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x04008BD3 RID: 35795
		private int cell;

		// Token: 0x04008BD4 RID: 35796
		private FlowUtilityNetwork network;

		// Token: 0x04008BD5 RID: 35797
		private Endpoint endpointType;

		// Token: 0x04008BD6 RID: 35798
		private ConduitType conduitType;

		// Token: 0x04008BD7 RID: 35799
		private GameObject parent;
	}
}
