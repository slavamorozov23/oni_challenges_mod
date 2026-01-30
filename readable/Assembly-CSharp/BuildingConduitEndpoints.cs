using System;
using UnityEngine;

// Token: 0x020006F9 RID: 1785
[AddComponentMenu("KMonoBehaviour/scripts/BuildingConduitEndpoints")]
public class BuildingConduitEndpoints : KMonoBehaviour
{
	// Token: 0x06002C30 RID: 11312 RVA: 0x0010194F File Offset: 0x000FFB4F
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.AddEndpoint();
	}

	// Token: 0x06002C31 RID: 11313 RVA: 0x0010195D File Offset: 0x000FFB5D
	protected override void OnCleanUp()
	{
		this.RemoveEndPoint();
		base.OnCleanUp();
	}

	// Token: 0x06002C32 RID: 11314 RVA: 0x0010196C File Offset: 0x000FFB6C
	public void RemoveEndPoint()
	{
		if (this.itemInput != null)
		{
			if (this.itemInput.ConduitType == ConduitType.Solid)
			{
				Game.Instance.solidConduitSystem.RemoveFromNetworks(this.itemInput.Cell, this.itemInput, true);
			}
			else
			{
				Conduit.GetNetworkManager(this.itemInput.ConduitType).RemoveFromNetworks(this.itemInput.Cell, this.itemInput, true);
			}
			this.itemInput = null;
		}
		if (this.itemOutput != null)
		{
			if (this.itemOutput.ConduitType == ConduitType.Solid)
			{
				Game.Instance.solidConduitSystem.RemoveFromNetworks(this.itemOutput.Cell, this.itemOutput, true);
			}
			else
			{
				Conduit.GetNetworkManager(this.itemOutput.ConduitType).RemoveFromNetworks(this.itemOutput.Cell, this.itemOutput, true);
			}
			this.itemOutput = null;
		}
	}

	// Token: 0x06002C33 RID: 11315 RVA: 0x00101A48 File Offset: 0x000FFC48
	public void AddEndpoint()
	{
		Building component = base.GetComponent<Building>();
		BuildingDef def = component.Def;
		this.RemoveEndPoint();
		if (def.InputConduitType != ConduitType.None)
		{
			int utilityInputCell = component.GetUtilityInputCell();
			this.itemInput = new FlowUtilityNetwork.NetworkItem(def.InputConduitType, Endpoint.Sink, utilityInputCell, base.gameObject);
			if (def.InputConduitType == ConduitType.Solid)
			{
				Game.Instance.solidConduitSystem.AddToNetworks(utilityInputCell, this.itemInput, true);
			}
			else
			{
				Conduit.GetNetworkManager(def.InputConduitType).AddToNetworks(utilityInputCell, this.itemInput, true);
			}
		}
		if (def.OutputConduitType != ConduitType.None)
		{
			int utilityOutputCell = component.GetUtilityOutputCell();
			this.itemOutput = new FlowUtilityNetwork.NetworkItem(def.OutputConduitType, Endpoint.Source, utilityOutputCell, base.gameObject);
			if (def.OutputConduitType == ConduitType.Solid)
			{
				Game.Instance.solidConduitSystem.AddToNetworks(utilityOutputCell, this.itemOutput, true);
				return;
			}
			Conduit.GetNetworkManager(def.OutputConduitType).AddToNetworks(utilityOutputCell, this.itemOutput, true);
		}
	}

	// Token: 0x04001A34 RID: 6708
	private FlowUtilityNetwork.NetworkItem itemInput;

	// Token: 0x04001A35 RID: 6709
	private FlowUtilityNetwork.NetworkItem itemOutput;
}
