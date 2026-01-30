using System;

// Token: 0x02000807 RID: 2055
public class StorageLockerSmart : StorageLocker
{
	// Token: 0x0600375A RID: 14170 RVA: 0x001375A5 File Offset: 0x001357A5
	protected override void OnPrefabInit()
	{
		base.Initialize(true);
	}

	// Token: 0x0600375B RID: 14171 RVA: 0x001375B0 File Offset: 0x001357B0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.ports = base.gameObject.GetComponent<LogicPorts>();
		base.Subscribe<StorageLockerSmart>(-1697596308, StorageLockerSmart.UpdateLogicCircuitCBDelegate);
		base.Subscribe<StorageLockerSmart>(-592767678, StorageLockerSmart.UpdateLogicCircuitCBDelegate);
		this.UpdateLogicAndActiveState();
	}

	// Token: 0x0600375C RID: 14172 RVA: 0x001375FC File Offset: 0x001357FC
	private void UpdateLogicCircuitCB(object _)
	{
		this.UpdateLogicAndActiveState();
	}

	// Token: 0x0600375D RID: 14173 RVA: 0x00137604 File Offset: 0x00135804
	private void UpdateLogicAndActiveState()
	{
		bool flag = this.filteredStorage.IsFull();
		bool isOperational = this.operational.IsOperational;
		bool flag2 = flag && isOperational;
		this.ports.SendSignal(FilteredStorage.FULL_PORT_ID, flag2 ? 1 : 0);
		this.filteredStorage.SetLogicMeter(flag2);
		this.operational.SetActive(isOperational, false);
	}

	// Token: 0x1700039F RID: 927
	// (get) Token: 0x0600375E RID: 14174 RVA: 0x0013765B File Offset: 0x0013585B
	// (set) Token: 0x0600375F RID: 14175 RVA: 0x00137663 File Offset: 0x00135863
	public override float UserMaxCapacity
	{
		get
		{
			return base.UserMaxCapacity;
		}
		set
		{
			base.UserMaxCapacity = value;
			this.UpdateLogicAndActiveState();
		}
	}

	// Token: 0x040021B6 RID: 8630
	[MyCmpGet]
	private LogicPorts ports;

	// Token: 0x040021B7 RID: 8631
	[MyCmpGet]
	private Operational operational;

	// Token: 0x040021B8 RID: 8632
	private static readonly EventSystem.IntraObjectHandler<StorageLockerSmart> UpdateLogicCircuitCBDelegate = new EventSystem.IntraObjectHandler<StorageLockerSmart>(delegate(StorageLockerSmart component, object data)
	{
		component.UpdateLogicCircuitCB(data);
	});
}
