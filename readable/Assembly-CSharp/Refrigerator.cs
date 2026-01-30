using System;
using KSerialization;
using UnityEngine;

// Token: 0x020007EB RID: 2027
[AddComponentMenu("KMonoBehaviour/scripts/Refrigerator")]
public class Refrigerator : KMonoBehaviour, IUserControlledCapacity
{
	// Token: 0x06003620 RID: 13856 RVA: 0x00131461 File Offset: 0x0012F661
	protected override void OnPrefabInit()
	{
		this.filteredStorage = new FilteredStorage(this, new Tag[]
		{
			GameTags.Compostable
		}, this, true, Db.Get().ChoreTypes.FoodFetch);
	}

	// Token: 0x06003621 RID: 13857 RVA: 0x00131494 File Offset: 0x0012F694
	protected override void OnSpawn()
	{
		base.GetComponent<KAnimControllerBase>().Play("off", KAnim.PlayMode.Once, 1f, 0f);
		FoodStorage component = base.GetComponent<FoodStorage>();
		component.FilteredStorage = this.filteredStorage;
		component.SpicedFoodOnly = component.SpicedFoodOnly;
		this.filteredStorage.FilterChanged();
		this.UpdateLogicCircuit();
		base.Subscribe<Refrigerator>(-905833192, Refrigerator.OnCopySettingsDelegate);
		base.Subscribe<Refrigerator>(-1697596308, Refrigerator.UpdateLogicCircuitCBDelegate);
		base.Subscribe<Refrigerator>(-592767678, Refrigerator.UpdateLogicCircuitCBDelegate);
	}

	// Token: 0x06003622 RID: 13858 RVA: 0x00131522 File Offset: 0x0012F722
	protected override void OnCleanUp()
	{
		this.filteredStorage.CleanUp();
	}

	// Token: 0x06003623 RID: 13859 RVA: 0x0013152F File Offset: 0x0012F72F
	public bool IsActive()
	{
		return this.operational.IsActive;
	}

	// Token: 0x06003624 RID: 13860 RVA: 0x0013153C File Offset: 0x0012F73C
	private void OnCopySettings(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (gameObject == null)
		{
			return;
		}
		Refrigerator component = gameObject.GetComponent<Refrigerator>();
		if (component == null)
		{
			return;
		}
		this.UserMaxCapacity = component.UserMaxCapacity;
	}

	// Token: 0x17000379 RID: 889
	// (get) Token: 0x06003625 RID: 13861 RVA: 0x00131577 File Offset: 0x0012F777
	// (set) Token: 0x06003626 RID: 13862 RVA: 0x0013158F File Offset: 0x0012F78F
	public float UserMaxCapacity
	{
		get
		{
			return Mathf.Min(this.userMaxCapacity, this.storage.capacityKg);
		}
		set
		{
			this.userMaxCapacity = value;
			this.filteredStorage.FilterChanged();
			this.UpdateLogicCircuit();
		}
	}

	// Token: 0x1700037A RID: 890
	// (get) Token: 0x06003627 RID: 13863 RVA: 0x001315A9 File Offset: 0x0012F7A9
	public float AmountStored
	{
		get
		{
			return this.storage.MassStored();
		}
	}

	// Token: 0x1700037B RID: 891
	// (get) Token: 0x06003628 RID: 13864 RVA: 0x001315B6 File Offset: 0x0012F7B6
	public float MinCapacity
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700037C RID: 892
	// (get) Token: 0x06003629 RID: 13865 RVA: 0x001315BD File Offset: 0x0012F7BD
	public float MaxCapacity
	{
		get
		{
			return this.storage.capacityKg;
		}
	}

	// Token: 0x1700037D RID: 893
	// (get) Token: 0x0600362A RID: 13866 RVA: 0x001315CA File Offset: 0x0012F7CA
	public bool WholeValues
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700037E RID: 894
	// (get) Token: 0x0600362B RID: 13867 RVA: 0x001315CD File Offset: 0x0012F7CD
	public LocString CapacityUnits
	{
		get
		{
			return GameUtil.GetCurrentMassUnit(false);
		}
	}

	// Token: 0x0600362C RID: 13868 RVA: 0x001315D5 File Offset: 0x0012F7D5
	private void UpdateLogicCircuitCB(object data)
	{
		this.UpdateLogicCircuit();
	}

	// Token: 0x0600362D RID: 13869 RVA: 0x001315E0 File Offset: 0x0012F7E0
	private void UpdateLogicCircuit()
	{
		bool flag = this.filteredStorage.IsFull();
		bool isOperational = this.operational.IsOperational;
		bool flag2 = flag && isOperational;
		this.ports.SendSignal(FilteredStorage.FULL_PORT_ID, flag2 ? 1 : 0);
		this.filteredStorage.SetLogicMeter(flag2);
	}

	// Token: 0x040020EC RID: 8428
	[MyCmpGet]
	private Storage storage;

	// Token: 0x040020ED RID: 8429
	[MyCmpGet]
	private Operational operational;

	// Token: 0x040020EE RID: 8430
	[MyCmpGet]
	private LogicPorts ports;

	// Token: 0x040020EF RID: 8431
	[Serialize]
	private float userMaxCapacity = float.PositiveInfinity;

	// Token: 0x040020F0 RID: 8432
	private FilteredStorage filteredStorage;

	// Token: 0x040020F1 RID: 8433
	private static readonly EventSystem.IntraObjectHandler<Refrigerator> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<Refrigerator>(delegate(Refrigerator component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x040020F2 RID: 8434
	private static readonly EventSystem.IntraObjectHandler<Refrigerator> UpdateLogicCircuitCBDelegate = new EventSystem.IntraObjectHandler<Refrigerator>(delegate(Refrigerator component, object data)
	{
		component.UpdateLogicCircuitCB(data);
	});
}
