using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200072D RID: 1837
[AddComponentMenu("KMonoBehaviour/scripts/ConduitBridge")]
public class ConduitBridge : ConduitBridgeBase, IBridgedNetworkItem
{
	// Token: 0x06002E26 RID: 11814 RVA: 0x0010B7F9 File Offset: 0x001099F9
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.accumulator = Game.Instance.accumulators.Add("Flow", this);
	}

	// Token: 0x06002E27 RID: 11815 RVA: 0x0010B81C File Offset: 0x00109A1C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Building component = base.GetComponent<Building>();
		this.inputCell = component.GetUtilityInputCell();
		this.outputCell = component.GetUtilityOutputCell();
		Conduit.GetFlowManager(this.type).AddConduitUpdater(new Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);
	}

	// Token: 0x06002E28 RID: 11816 RVA: 0x0010B86B File Offset: 0x00109A6B
	protected override void OnCleanUp()
	{
		Conduit.GetFlowManager(this.type).RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
		Game.Instance.accumulators.Remove(this.accumulator);
		base.OnCleanUp();
	}

	// Token: 0x06002E29 RID: 11817 RVA: 0x0010B8A8 File Offset: 0x00109AA8
	private void ConduitUpdate(float dt)
	{
		ConduitFlow flowManager = Conduit.GetFlowManager(this.type);
		if (!flowManager.HasConduit(this.inputCell) || !flowManager.HasConduit(this.outputCell))
		{
			base.SendEmptyOnMassTransfer();
			return;
		}
		ConduitFlow.ConduitContents contents = flowManager.GetContents(this.inputCell);
		float num = contents.mass;
		if (this.desiredMassTransfer != null)
		{
			num = this.desiredMassTransfer(dt, contents.element, contents.mass, contents.temperature, contents.diseaseIdx, contents.diseaseCount, null);
		}
		if (num > 0f)
		{
			int disease_count = (int)(num / contents.mass * (float)contents.diseaseCount);
			float num2 = flowManager.AddElement(this.outputCell, contents.element, num, contents.temperature, contents.diseaseIdx, disease_count);
			if (num2 <= 0f)
			{
				base.SendEmptyOnMassTransfer();
				return;
			}
			flowManager.RemoveElement(this.inputCell, num2);
			Game.Instance.accumulators.Accumulate(this.accumulator, contents.mass);
			if (this.OnMassTransfer != null)
			{
				this.OnMassTransfer(contents.element, num2, contents.temperature, contents.diseaseIdx, disease_count, null);
				return;
			}
		}
		else
		{
			base.SendEmptyOnMassTransfer();
		}
	}

	// Token: 0x06002E2A RID: 11818 RVA: 0x0010B9DC File Offset: 0x00109BDC
	public void AddNetworks(ICollection<UtilityNetwork> networks)
	{
		IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(this.type);
		UtilityNetwork networkForCell = networkManager.GetNetworkForCell(this.inputCell);
		if (networkForCell != null)
		{
			networks.Add(networkForCell);
		}
		networkForCell = networkManager.GetNetworkForCell(this.outputCell);
		if (networkForCell != null)
		{
			networks.Add(networkForCell);
		}
	}

	// Token: 0x06002E2B RID: 11819 RVA: 0x0010BA24 File Offset: 0x00109C24
	public bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks)
	{
		bool flag = false;
		IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(this.type);
		return flag || networks.Contains(networkManager.GetNetworkForCell(this.inputCell)) || networks.Contains(networkManager.GetNetworkForCell(this.outputCell));
	}

	// Token: 0x06002E2C RID: 11820 RVA: 0x0010BA6B File Offset: 0x00109C6B
	public int GetNetworkCell()
	{
		return this.inputCell;
	}

	// Token: 0x04001B69 RID: 7017
	[SerializeField]
	public ConduitType type;

	// Token: 0x04001B6A RID: 7018
	private int inputCell;

	// Token: 0x04001B6B RID: 7019
	private int outputCell;

	// Token: 0x04001B6C RID: 7020
	private HandleVector<int>.Handle accumulator = HandleVector<int>.InvalidHandle;
}
