using System;
using UnityEngine;

// Token: 0x0200072F RID: 1839
[AddComponentMenu("KMonoBehaviour/scripts/ConduitOverflow")]
public class ConduitOverflow : KMonoBehaviour, ISecondaryOutput
{
	// Token: 0x06002E30 RID: 11824 RVA: 0x0010BAB8 File Offset: 0x00109CB8
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
		this.secondaryOutput = new FlowUtilityNetwork.NetworkItem(this.portInfo.conduitType, Endpoint.Sink, cell2, base.gameObject);
		networkManager.AddToNetworks(this.secondaryOutput.Cell, this.secondaryOutput, true);
	}

	// Token: 0x06002E31 RID: 11825 RVA: 0x0010BB7C File Offset: 0x00109D7C
	protected override void OnCleanUp()
	{
		Conduit.GetNetworkManager(this.portInfo.conduitType).RemoveFromNetworks(this.secondaryOutput.Cell, this.secondaryOutput, true);
		Conduit.GetFlowManager(this.portInfo.conduitType).RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
		base.OnCleanUp();
	}

	// Token: 0x06002E32 RID: 11826 RVA: 0x0010BBD8 File Offset: 0x00109DD8
	private void ConduitUpdate(float dt)
	{
		ConduitFlow flowManager = Conduit.GetFlowManager(this.portInfo.conduitType);
		if (!flowManager.HasConduit(this.inputCell))
		{
			return;
		}
		ConduitFlow.ConduitContents contents = flowManager.GetContents(this.inputCell);
		if (contents.mass <= 0f)
		{
			return;
		}
		int cell = this.outputCell;
		ConduitFlow.ConduitContents contents2 = flowManager.GetContents(cell);
		if (contents2.mass > 0f)
		{
			cell = this.secondaryOutput.Cell;
			contents2 = flowManager.GetContents(cell);
		}
		if (contents2.mass <= 0f)
		{
			float num = flowManager.AddElement(cell, contents.element, contents.mass, contents.temperature, contents.diseaseIdx, contents.diseaseCount);
			if (num > 0f)
			{
				flowManager.RemoveElement(this.inputCell, num);
			}
		}
	}

	// Token: 0x06002E33 RID: 11827 RVA: 0x0010BCA0 File Offset: 0x00109EA0
	public bool HasSecondaryConduitType(ConduitType type)
	{
		return this.portInfo.conduitType == type;
	}

	// Token: 0x06002E34 RID: 11828 RVA: 0x0010BCB0 File Offset: 0x00109EB0
	public CellOffset GetSecondaryConduitOffset(ConduitType type)
	{
		return this.portInfo.offset;
	}

	// Token: 0x04001B6F RID: 7023
	[SerializeField]
	public ConduitPortInfo portInfo;

	// Token: 0x04001B70 RID: 7024
	private int inputCell;

	// Token: 0x04001B71 RID: 7025
	private int outputCell;

	// Token: 0x04001B72 RID: 7026
	private FlowUtilityNetwork.NetworkItem secondaryOutput;
}
