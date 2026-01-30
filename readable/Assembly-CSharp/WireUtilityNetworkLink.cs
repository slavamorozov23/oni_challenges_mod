using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C21 RID: 3105
public class WireUtilityNetworkLink : UtilityNetworkLink, IWattageRating, IHaveUtilityNetworkMgr, IBridgedNetworkItem, ICircuitConnected
{
	// Token: 0x06005D60 RID: 23904 RVA: 0x0021CBAC File Offset: 0x0021ADAC
	public Wire.WattageRating GetMaxWattageRating()
	{
		return this.maxWattageRating;
	}

	// Token: 0x06005D61 RID: 23905 RVA: 0x0021CBB4 File Offset: 0x0021ADB4
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06005D62 RID: 23906 RVA: 0x0021CBBC File Offset: 0x0021ADBC
	protected override void OnDisconnect(int cell1, int cell2)
	{
		Game.Instance.electricalConduitSystem.RemoveLink(cell1, cell2);
		Game.Instance.circuitManager.Disconnect(this);
	}

	// Token: 0x06005D63 RID: 23907 RVA: 0x0021CBDF File Offset: 0x0021ADDF
	protected override void OnConnect(int cell1, int cell2)
	{
		Game.Instance.electricalConduitSystem.AddLink(cell1, cell2);
		Game.Instance.circuitManager.Connect(this);
	}

	// Token: 0x06005D64 RID: 23908 RVA: 0x0021CC02 File Offset: 0x0021AE02
	public IUtilityNetworkMgr GetNetworkManager()
	{
		return Game.Instance.electricalConduitSystem;
	}

	// Token: 0x170006B6 RID: 1718
	// (get) Token: 0x06005D65 RID: 23909 RVA: 0x0021CC0E File Offset: 0x0021AE0E
	// (set) Token: 0x06005D66 RID: 23910 RVA: 0x0021CC16 File Offset: 0x0021AE16
	public bool IsVirtual { get; private set; }

	// Token: 0x170006B7 RID: 1719
	// (get) Token: 0x06005D67 RID: 23911 RVA: 0x0021CC1F File Offset: 0x0021AE1F
	public int PowerCell
	{
		get
		{
			return base.GetNetworkCell();
		}
	}

	// Token: 0x170006B8 RID: 1720
	// (get) Token: 0x06005D68 RID: 23912 RVA: 0x0021CC27 File Offset: 0x0021AE27
	// (set) Token: 0x06005D69 RID: 23913 RVA: 0x0021CC2F File Offset: 0x0021AE2F
	public object VirtualCircuitKey { get; private set; }

	// Token: 0x06005D6A RID: 23914 RVA: 0x0021CC38 File Offset: 0x0021AE38
	public void AddNetworks(ICollection<UtilityNetwork> networks)
	{
		int networkCell = base.GetNetworkCell();
		UtilityNetwork networkForCell = this.GetNetworkManager().GetNetworkForCell(networkCell);
		if (networkForCell != null)
		{
			networks.Add(networkForCell);
		}
	}

	// Token: 0x06005D6B RID: 23915 RVA: 0x0021CC64 File Offset: 0x0021AE64
	public bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks)
	{
		int networkCell = base.GetNetworkCell();
		UtilityNetwork networkForCell = this.GetNetworkManager().GetNetworkForCell(networkCell);
		return networks.Contains(networkForCell);
	}

	// Token: 0x04003E36 RID: 15926
	[SerializeField]
	public Wire.WattageRating maxWattageRating;
}
