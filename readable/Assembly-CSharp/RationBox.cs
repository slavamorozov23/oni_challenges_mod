using System;
using KSerialization;
using UnityEngine;

// Token: 0x020007E7 RID: 2023
[AddComponentMenu("KMonoBehaviour/scripts/RationBox")]
public class RationBox : KMonoBehaviour, IUserControlledCapacity, IRender1000ms, IRottable
{
	// Token: 0x060035E6 RID: 13798 RVA: 0x001302E4 File Offset: 0x0012E4E4
	protected override void OnPrefabInit()
	{
		this.filteredStorage = new FilteredStorage(this, new Tag[]
		{
			GameTags.Compostable
		}, this, false, Db.Get().ChoreTypes.FoodFetch);
		base.Subscribe<RationBox>(-592767678, RationBox.OnOperationalChangedDelegate);
		base.Subscribe<RationBox>(-905833192, RationBox.OnCopySettingsDelegate);
		DiscoveredResources.Instance.Discover("FieldRation".ToTag(), GameTags.Edible);
	}

	// Token: 0x060035E7 RID: 13799 RVA: 0x0013035B File Offset: 0x0012E55B
	protected override void OnSpawn()
	{
		Operational component = base.GetComponent<Operational>();
		component.SetActive(component.IsOperational, false);
		this.filteredStorage.FilterChanged();
	}

	// Token: 0x060035E8 RID: 13800 RVA: 0x0013037A File Offset: 0x0012E57A
	protected override void OnCleanUp()
	{
		this.filteredStorage.CleanUp();
	}

	// Token: 0x060035E9 RID: 13801 RVA: 0x00130387 File Offset: 0x0012E587
	private void OnOperationalChanged(object _)
	{
		Operational component = base.GetComponent<Operational>();
		component.SetActive(component.IsOperational, false);
	}

	// Token: 0x060035EA RID: 13802 RVA: 0x0013039C File Offset: 0x0012E59C
	private void OnCopySettings(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (gameObject == null)
		{
			return;
		}
		RationBox component = gameObject.GetComponent<RationBox>();
		if (component == null)
		{
			return;
		}
		this.UserMaxCapacity = component.UserMaxCapacity;
	}

	// Token: 0x060035EB RID: 13803 RVA: 0x001303D7 File Offset: 0x0012E5D7
	public void Render1000ms(float dt)
	{
		Rottable.SetStatusItems(this);
	}

	// Token: 0x1700036D RID: 877
	// (get) Token: 0x060035EC RID: 13804 RVA: 0x001303DF File Offset: 0x0012E5DF
	// (set) Token: 0x060035ED RID: 13805 RVA: 0x001303F7 File Offset: 0x0012E5F7
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
		}
	}

	// Token: 0x1700036E RID: 878
	// (get) Token: 0x060035EE RID: 13806 RVA: 0x0013040B File Offset: 0x0012E60B
	public float AmountStored
	{
		get
		{
			return this.storage.MassStored();
		}
	}

	// Token: 0x1700036F RID: 879
	// (get) Token: 0x060035EF RID: 13807 RVA: 0x00130418 File Offset: 0x0012E618
	public float MinCapacity
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000370 RID: 880
	// (get) Token: 0x060035F0 RID: 13808 RVA: 0x0013041F File Offset: 0x0012E61F
	public float MaxCapacity
	{
		get
		{
			return this.storage.capacityKg;
		}
	}

	// Token: 0x17000371 RID: 881
	// (get) Token: 0x060035F1 RID: 13809 RVA: 0x0013042C File Offset: 0x0012E62C
	public bool WholeValues
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000372 RID: 882
	// (get) Token: 0x060035F2 RID: 13810 RVA: 0x0013042F File Offset: 0x0012E62F
	public LocString CapacityUnits
	{
		get
		{
			return GameUtil.GetCurrentMassUnit(false);
		}
	}

	// Token: 0x17000373 RID: 883
	// (get) Token: 0x060035F3 RID: 13811 RVA: 0x00130437 File Offset: 0x0012E637
	public float RotTemperature
	{
		get
		{
			return 277.15f;
		}
	}

	// Token: 0x17000374 RID: 884
	// (get) Token: 0x060035F4 RID: 13812 RVA: 0x0013043E File Offset: 0x0012E63E
	public float PreserveTemperature
	{
		get
		{
			return 255.15f;
		}
	}

	// Token: 0x060035F7 RID: 13815 RVA: 0x0013048E File Offset: 0x0012E68E
	GameObject IRottable.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x040020CE RID: 8398
	[MyCmpReq]
	private Storage storage;

	// Token: 0x040020CF RID: 8399
	[Serialize]
	private float userMaxCapacity = float.PositiveInfinity;

	// Token: 0x040020D0 RID: 8400
	private FilteredStorage filteredStorage;

	// Token: 0x040020D1 RID: 8401
	private static readonly EventSystem.IntraObjectHandler<RationBox> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<RationBox>(delegate(RationBox component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x040020D2 RID: 8402
	private static readonly EventSystem.IntraObjectHandler<RationBox> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<RationBox>(delegate(RationBox component, object data)
	{
		component.OnCopySettings(data);
	});
}
