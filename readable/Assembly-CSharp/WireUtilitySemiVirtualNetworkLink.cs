using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C22 RID: 3106
public class WireUtilitySemiVirtualNetworkLink : UtilityNetworkLink, IHaveUtilityNetworkMgr, ICircuitConnected
{
	// Token: 0x06005D6D RID: 23917 RVA: 0x0021CC94 File Offset: 0x0021AE94
	public Wire.WattageRating GetMaxWattageRating()
	{
		return this.maxWattageRating;
	}

	// Token: 0x06005D6E RID: 23918 RVA: 0x0021CC9C File Offset: 0x0021AE9C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06005D6F RID: 23919 RVA: 0x0021CCA4 File Offset: 0x0021AEA4
	protected override void OnSpawn()
	{
		RocketModuleCluster component = base.GetComponent<RocketModuleCluster>();
		if (component != null)
		{
			this.VirtualCircuitKey = component.CraftInterface;
		}
		else
		{
			CraftModuleInterface component2 = this.GetMyWorld().GetComponent<CraftModuleInterface>();
			if (component2 != null)
			{
				this.VirtualCircuitKey = component2;
			}
		}
		Game.Instance.electricalConduitSystem.AddToVirtualNetworks(this.VirtualCircuitKey, this, true);
		base.OnSpawn();
	}

	// Token: 0x06005D70 RID: 23920 RVA: 0x0021CD08 File Offset: 0x0021AF08
	public void SetLinkConnected(bool connect)
	{
		if (connect && this.visualizeOnly)
		{
			this.visualizeOnly = false;
			if (base.isSpawned)
			{
				base.Connect();
				return;
			}
		}
		else if (!connect && !this.visualizeOnly)
		{
			if (base.isSpawned)
			{
				base.Disconnect();
			}
			this.visualizeOnly = true;
		}
	}

	// Token: 0x06005D71 RID: 23921 RVA: 0x0021CD56 File Offset: 0x0021AF56
	protected override void OnDisconnect(int cell1, int cell2)
	{
		Game.Instance.electricalConduitSystem.RemoveSemiVirtualLink(cell1, this.VirtualCircuitKey);
	}

	// Token: 0x06005D72 RID: 23922 RVA: 0x0021CD6E File Offset: 0x0021AF6E
	protected override void OnConnect(int cell1, int cell2)
	{
		Game.Instance.electricalConduitSystem.AddSemiVirtualLink(cell1, this.VirtualCircuitKey);
	}

	// Token: 0x06005D73 RID: 23923 RVA: 0x0021CD86 File Offset: 0x0021AF86
	public IUtilityNetworkMgr GetNetworkManager()
	{
		return Game.Instance.electricalConduitSystem;
	}

	// Token: 0x170006B9 RID: 1721
	// (get) Token: 0x06005D74 RID: 23924 RVA: 0x0021CD92 File Offset: 0x0021AF92
	// (set) Token: 0x06005D75 RID: 23925 RVA: 0x0021CD9A File Offset: 0x0021AF9A
	public bool IsVirtual { get; private set; }

	// Token: 0x170006BA RID: 1722
	// (get) Token: 0x06005D76 RID: 23926 RVA: 0x0021CDA3 File Offset: 0x0021AFA3
	public int PowerCell
	{
		get
		{
			return base.GetNetworkCell();
		}
	}

	// Token: 0x170006BB RID: 1723
	// (get) Token: 0x06005D77 RID: 23927 RVA: 0x0021CDAB File Offset: 0x0021AFAB
	// (set) Token: 0x06005D78 RID: 23928 RVA: 0x0021CDB3 File Offset: 0x0021AFB3
	public object VirtualCircuitKey { get; private set; }

	// Token: 0x06005D79 RID: 23929 RVA: 0x0021CDBC File Offset: 0x0021AFBC
	public void AddNetworks(ICollection<UtilityNetwork> networks)
	{
		int networkCell = base.GetNetworkCell();
		UtilityNetwork networkForCell = this.GetNetworkManager().GetNetworkForCell(networkCell);
		if (networkForCell != null)
		{
			networks.Add(networkForCell);
		}
	}

	// Token: 0x06005D7A RID: 23930 RVA: 0x0021CDE8 File Offset: 0x0021AFE8
	public bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks)
	{
		int networkCell = base.GetNetworkCell();
		UtilityNetwork networkForCell = this.GetNetworkManager().GetNetworkForCell(networkCell);
		return networks.Contains(networkForCell);
	}

	// Token: 0x04003E39 RID: 15929
	[SerializeField]
	public Wire.WattageRating maxWattageRating;
}
