using System;
using UnityEngine;

// Token: 0x020007E2 RID: 2018
[AddComponentMenu("KMonoBehaviour/scripts/Pump")]
public class Pump : KMonoBehaviour, ISim1000ms
{
	// Token: 0x060035B3 RID: 13747 RVA: 0x0012EF9F File Offset: 0x0012D19F
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.consumer.EnableConsumption(false);
		this.consumer.OnElementConsumedStored += this.OnElementConsumedStored;
	}

	// Token: 0x060035B4 RID: 13748 RVA: 0x0012EFCC File Offset: 0x0012D1CC
	private void OnElementConsumedStored(Sim.ConsumedMassInfo elementConsumedInfo)
	{
		if (this.dispenser.conduitType == ConduitType.Liquid)
		{
			Element element = ElementLoader.elements[(int)elementConsumedInfo.removedElemIdx];
			if (this.lastElementConsumed != element.id && element.id != SimHashes.Vacuum)
			{
				Color color = element.substance.colour;
				color.a = 1f;
				this.controller.SetSymbolTint(new KAnimHashedString("water"), color);
			}
			this.lastElementConsumed = element.id;
		}
	}

	// Token: 0x060035B5 RID: 13749 RVA: 0x0012F052 File Offset: 0x0012D252
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.elapsedTime = 0f;
		this.pumpable = this.UpdateOperational();
		this.dispenser.GetConduitManager().AddConduitUpdater(new Action<float>(this.OnConduitUpdate), ConduitFlowPriority.LastPostUpdate);
	}

	// Token: 0x060035B6 RID: 13750 RVA: 0x0012F08F File Offset: 0x0012D28F
	protected override void OnCleanUp()
	{
		this.dispenser.GetConduitManager().RemoveConduitUpdater(new Action<float>(this.OnConduitUpdate));
		base.OnCleanUp();
	}

	// Token: 0x060035B7 RID: 13751 RVA: 0x0012F0B4 File Offset: 0x0012D2B4
	public void Sim1000ms(float dt)
	{
		this.elapsedTime += dt;
		if (this.elapsedTime >= 1f)
		{
			this.pumpable = this.UpdateOperational();
			this.elapsedTime = 0f;
		}
		if (this.operational.IsOperational && this.pumpable)
		{
			this.operational.SetActive(true, false);
			return;
		}
		this.operational.SetActive(false, false);
	}

	// Token: 0x060035B8 RID: 13752 RVA: 0x0012F124 File Offset: 0x0012D324
	private bool UpdateOperational()
	{
		Element.State state = Element.State.Vacuum;
		ConduitType conduitType = this.dispenser.conduitType;
		if (conduitType != ConduitType.Gas)
		{
			if (conduitType == ConduitType.Liquid)
			{
				state = Element.State.Liquid;
			}
		}
		else
		{
			state = Element.State.Gas;
		}
		bool flag = this.IsPumpable(state, (int)this.consumer.consumptionRadius);
		StatusItem status_item = (state == Element.State.Gas) ? Db.Get().BuildingStatusItems.NoGasElementToPump : Db.Get().BuildingStatusItems.NoLiquidElementToPump;
		this.noElementStatusGuid = this.selectable.ToggleStatusItem(status_item, this.noElementStatusGuid, !flag, null);
		this.operational.SetFlag(Pump.PumpableFlag, !this.storage.IsFull() && flag);
		return flag;
	}

	// Token: 0x060035B9 RID: 13753 RVA: 0x0012F1C8 File Offset: 0x0012D3C8
	private bool IsPumpable(Element.State expected_state, int radius)
	{
		int num = Grid.PosToCell(base.transform.GetPosition());
		for (int i = 0; i < (int)this.consumer.consumptionRadius; i++)
		{
			for (int j = 0; j < (int)this.consumer.consumptionRadius; j++)
			{
				int num2 = num + j + Grid.WidthInCells * i;
				if (Grid.Element[num2].IsState(expected_state))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060035BA RID: 13754 RVA: 0x0012F230 File Offset: 0x0012D430
	private void OnConduitUpdate(float dt)
	{
		this.conduitBlockedStatusGuid = this.selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.ConduitBlocked, this.conduitBlockedStatusGuid, this.dispenser.blocked, null);
	}

	// Token: 0x17000366 RID: 870
	// (get) Token: 0x060035BB RID: 13755 RVA: 0x0012F264 File Offset: 0x0012D464
	public ConduitType conduitType
	{
		get
		{
			return this.dispenser.conduitType;
		}
	}

	// Token: 0x04002084 RID: 8324
	public static readonly Operational.Flag PumpableFlag = new Operational.Flag("vent", Operational.Flag.Type.Requirement);

	// Token: 0x04002085 RID: 8325
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04002086 RID: 8326
	[MyCmpGet]
	private KSelectable selectable;

	// Token: 0x04002087 RID: 8327
	[MyCmpGet]
	private ElementConsumer consumer;

	// Token: 0x04002088 RID: 8328
	[MyCmpGet]
	private ConduitDispenser dispenser;

	// Token: 0x04002089 RID: 8329
	[MyCmpGet]
	private Storage storage;

	// Token: 0x0400208A RID: 8330
	[MyCmpGet]
	private KBatchedAnimController controller;

	// Token: 0x0400208B RID: 8331
	private const float OperationalUpdateInterval = 1f;

	// Token: 0x0400208C RID: 8332
	private float elapsedTime;

	// Token: 0x0400208D RID: 8333
	private bool pumpable;

	// Token: 0x0400208E RID: 8334
	private Guid conduitBlockedStatusGuid;

	// Token: 0x0400208F RID: 8335
	private Guid noElementStatusGuid;

	// Token: 0x04002090 RID: 8336
	private SimHashes lastElementConsumed = SimHashes.Vacuum;
}
