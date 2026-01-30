using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000824 RID: 2084
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/Valve")]
public class Valve : Workable, ISaveLoadable
{
	// Token: 0x170003CF RID: 975
	// (get) Token: 0x060038C1 RID: 14529 RVA: 0x0013D9D9 File Offset: 0x0013BBD9
	public float QueuedMaxFlow
	{
		get
		{
			if (this.chore == null)
			{
				return -1f;
			}
			return this.desiredFlow;
		}
	}

	// Token: 0x170003D0 RID: 976
	// (get) Token: 0x060038C2 RID: 14530 RVA: 0x0013D9EF File Offset: 0x0013BBEF
	public float DesiredFlow
	{
		get
		{
			return this.desiredFlow;
		}
	}

	// Token: 0x170003D1 RID: 977
	// (get) Token: 0x060038C3 RID: 14531 RVA: 0x0013D9F7 File Offset: 0x0013BBF7
	public float MaxFlow
	{
		get
		{
			return this.valveBase.MaxFlow;
		}
	}

	// Token: 0x060038C4 RID: 14532 RVA: 0x0013DA04 File Offset: 0x0013BC04
	private void OnCopySettings(object data)
	{
		Valve component = ((GameObject)data).GetComponent<Valve>();
		if (component != null)
		{
			this.ChangeFlow(component.desiredFlow);
		}
	}

	// Token: 0x060038C5 RID: 14533 RVA: 0x0013DA34 File Offset: 0x0013BC34
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
		this.synchronizeAnims = false;
		this.valveBase.CurrentFlow = this.valveBase.MaxFlow;
		this.desiredFlow = this.valveBase.MaxFlow;
		base.Subscribe<Valve>(-905833192, Valve.OnCopySettingsDelegate);
	}

	// Token: 0x060038C6 RID: 14534 RVA: 0x0013DA91 File Offset: 0x0013BC91
	protected override void OnSpawn()
	{
		this.ChangeFlow(this.desiredFlow);
		base.OnSpawn();
		Prioritizable.AddRef(base.gameObject);
	}

	// Token: 0x060038C7 RID: 14535 RVA: 0x0013DAB0 File Offset: 0x0013BCB0
	protected override void OnCleanUp()
	{
		Prioritizable.RemoveRef(base.gameObject);
		base.OnCleanUp();
	}

	// Token: 0x060038C8 RID: 14536 RVA: 0x0013DAC4 File Offset: 0x0013BCC4
	public void ChangeFlow(float amount)
	{
		this.desiredFlow = Mathf.Clamp(amount, 0f, this.valveBase.MaxFlow);
		KSelectable component = base.GetComponent<KSelectable>();
		component.ToggleStatusItem(Db.Get().BuildingStatusItems.PumpingLiquidOrGas, this.desiredFlow >= 0f, this.valveBase.AccumulatorHandle);
		if (DebugHandler.InstantBuildMode)
		{
			this.UpdateFlow();
			return;
		}
		if (this.desiredFlow == this.valveBase.CurrentFlow)
		{
			if (this.chore != null)
			{
				this.chore.Cancel("desiredFlow == currentFlow");
				this.chore = null;
			}
			component.RemoveStatusItem(Db.Get().BuildingStatusItems.ValveRequest, false);
			component.RemoveStatusItem(Db.Get().BuildingStatusItems.PendingWork, false);
			return;
		}
		if (this.chore == null)
		{
			component.AddStatusItem(Db.Get().BuildingStatusItems.ValveRequest, this);
			component.AddStatusItem(Db.Get().BuildingStatusItems.PendingWork, this);
			this.chore = new WorkChore<Valve>(Db.Get().ChoreTypes.Toggle, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			return;
		}
	}

	// Token: 0x060038C9 RID: 14537 RVA: 0x0013DBFF File Offset: 0x0013BDFF
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		this.UpdateFlow();
	}

	// Token: 0x060038CA RID: 14538 RVA: 0x0013DC10 File Offset: 0x0013BE10
	public void UpdateFlow()
	{
		this.valveBase.CurrentFlow = this.desiredFlow;
		this.valveBase.UpdateAnim();
		if (this.chore != null)
		{
			this.chore.Cancel("forced complete");
		}
		this.chore = null;
		KSelectable component = base.GetComponent<KSelectable>();
		component.RemoveStatusItem(Db.Get().BuildingStatusItems.ValveRequest, false);
		component.RemoveStatusItem(Db.Get().BuildingStatusItems.PendingWork, false);
	}

	// Token: 0x040022A0 RID: 8864
	[MyCmpReq]
	private ValveBase valveBase;

	// Token: 0x040022A1 RID: 8865
	[Serialize]
	private float desiredFlow = 0.5f;

	// Token: 0x040022A2 RID: 8866
	private Chore chore;

	// Token: 0x040022A3 RID: 8867
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x040022A4 RID: 8868
	private static readonly EventSystem.IntraObjectHandler<Valve> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<Valve>(delegate(Valve component, object data)
	{
		component.OnCopySettings(data);
	});
}
