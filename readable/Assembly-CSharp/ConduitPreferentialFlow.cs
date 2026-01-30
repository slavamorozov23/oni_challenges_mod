using System;
using UnityEngine;

// Token: 0x02000730 RID: 1840
[AddComponentMenu("KMonoBehaviour/scripts/ConduitPreferentialFlow")]
public class ConduitPreferentialFlow : KMonoBehaviour, ISecondaryInput
{
	// Token: 0x06002E36 RID: 11830 RVA: 0x0010BCC8 File Offset: 0x00109EC8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Building component = base.GetComponent<Building>();
		this.inputCell = component.GetUtilityInputCell();
		this.outputCell = component.GetUtilityOutputCell();
		int cell = Grid.PosToCell(base.transform.GetPosition());
		CellOffset rotatedOffset = component.GetRotatedOffset(this.portInfo.offset);
		int cell2 = Grid.OffsetCell(cell, rotatedOffset);
		Conduit.GetFlowManager(this.portInfo.conduitType).AddConduitUpdater(new Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);
		IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(this.portInfo.conduitType);
		this.secondaryInput = new FlowUtilityNetwork.NetworkItem(this.portInfo.conduitType, Endpoint.Sink, cell2, base.gameObject);
		networkManager.AddToNetworks(this.secondaryInput.Cell, this.secondaryInput, true);
	}

	// Token: 0x06002E37 RID: 11831 RVA: 0x0010BD8C File Offset: 0x00109F8C
	protected override void OnCleanUp()
	{
		Conduit.GetNetworkManager(this.portInfo.conduitType).RemoveFromNetworks(this.secondaryInput.Cell, this.secondaryInput, true);
		Conduit.GetFlowManager(this.portInfo.conduitType).RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
		base.OnCleanUp();
	}

	// Token: 0x06002E38 RID: 11832 RVA: 0x0010BDE8 File Offset: 0x00109FE8
	private void ConduitUpdate(float dt)
	{
		ConduitFlow flowManager = Conduit.GetFlowManager(this.portInfo.conduitType);
		if (!flowManager.HasConduit(this.outputCell))
		{
			return;
		}
		int cell = this.inputCell;
		ConduitFlow.ConduitContents contents = flowManager.GetContents(cell);
		if (contents.mass <= 0f)
		{
			cell = this.secondaryInput.Cell;
			contents = flowManager.GetContents(cell);
		}
		if (contents.mass > 0f)
		{
			float num = flowManager.AddElement(this.outputCell, contents.element, contents.mass, contents.temperature, contents.diseaseIdx, contents.diseaseCount);
			if (num > 0f)
			{
				flowManager.RemoveElement(cell, num);
			}
		}
	}

	// Token: 0x06002E39 RID: 11833 RVA: 0x0010BE91 File Offset: 0x0010A091
	public bool HasSecondaryConduitType(ConduitType type)
	{
		return this.portInfo.conduitType == type;
	}

	// Token: 0x06002E3A RID: 11834 RVA: 0x0010BEA1 File Offset: 0x0010A0A1
	public CellOffset GetSecondaryConduitOffset(ConduitType type)
	{
		if (this.portInfo.conduitType == type)
		{
			return this.portInfo.offset;
		}
		return CellOffset.none;
	}

	// Token: 0x04001B73 RID: 7027
	[SerializeField]
	public ConduitPortInfo portInfo;

	// Token: 0x04001B74 RID: 7028
	private int inputCell;

	// Token: 0x04001B75 RID: 7029
	private int outputCell;

	// Token: 0x04001B76 RID: 7030
	private FlowUtilityNetwork.NetworkItem secondaryInput;
}
