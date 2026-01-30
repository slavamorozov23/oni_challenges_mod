using System;
using System.Collections.Generic;

// Token: 0x020009F2 RID: 2546
public class LogicUtilityNetworkLink : UtilityNetworkLink, IHaveUtilityNetworkMgr, IBridgedNetworkItem
{
	// Token: 0x06004A55 RID: 19029 RVA: 0x001AEE8E File Offset: 0x001AD08E
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06004A56 RID: 19030 RVA: 0x001AEE96 File Offset: 0x001AD096
	protected override void OnConnect(int cell1, int cell2)
	{
		this.cell_one = cell1;
		this.cell_two = cell2;
		Game.Instance.logicCircuitSystem.AddLink(cell1, cell2);
		Game.Instance.logicCircuitManager.Connect(this);
	}

	// Token: 0x06004A57 RID: 19031 RVA: 0x001AEEC7 File Offset: 0x001AD0C7
	protected override void OnDisconnect(int cell1, int cell2)
	{
		Game.Instance.logicCircuitSystem.RemoveLink(cell1, cell2);
		Game.Instance.logicCircuitManager.Disconnect(this);
	}

	// Token: 0x06004A58 RID: 19032 RVA: 0x001AEEEA File Offset: 0x001AD0EA
	public IUtilityNetworkMgr GetNetworkManager()
	{
		return Game.Instance.logicCircuitSystem;
	}

	// Token: 0x06004A59 RID: 19033 RVA: 0x001AEEF8 File Offset: 0x001AD0F8
	public void AddNetworks(ICollection<UtilityNetwork> networks)
	{
		int networkCell = base.GetNetworkCell();
		UtilityNetwork networkForCell = this.GetNetworkManager().GetNetworkForCell(networkCell);
		if (networkForCell != null)
		{
			networks.Add(networkForCell);
		}
	}

	// Token: 0x06004A5A RID: 19034 RVA: 0x001AEF24 File Offset: 0x001AD124
	public bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks)
	{
		int networkCell = base.GetNetworkCell();
		UtilityNetwork networkForCell = this.GetNetworkManager().GetNetworkForCell(networkCell);
		return networks.Contains(networkForCell);
	}

	// Token: 0x04003150 RID: 12624
	public LogicWire.BitDepth bitDepth;

	// Token: 0x04003151 RID: 12625
	public int cell_one;

	// Token: 0x04003152 RID: 12626
	public int cell_two;
}
