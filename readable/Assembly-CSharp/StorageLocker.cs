using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000806 RID: 2054
[AddComponentMenu("KMonoBehaviour/scripts/StorageLocker")]
public class StorageLocker : KMonoBehaviour, IUserControlledCapacity
{
	// Token: 0x0600374B RID: 14155 RVA: 0x001373DB File Offset: 0x001355DB
	protected override void OnPrefabInit()
	{
		this.Initialize(false);
	}

	// Token: 0x0600374C RID: 14156 RVA: 0x001373E4 File Offset: 0x001355E4
	protected void Initialize(bool use_logic_meter)
	{
		base.OnPrefabInit();
		this.log = new LoggerFS("StorageLocker", 35);
		ChoreType fetch_chore_type = Db.Get().ChoreTypes.Get(this.choreTypeID);
		this.filteredStorage = new FilteredStorage(this, null, this, use_logic_meter, fetch_chore_type);
		base.Subscribe<StorageLocker>(-905833192, StorageLocker.OnCopySettingsDelegate);
	}

	// Token: 0x0600374D RID: 14157 RVA: 0x00137440 File Offset: 0x00135640
	protected override void OnSpawn()
	{
		this.filteredStorage.FilterChanged();
		if (this.nameable != null && !this.lockerName.IsNullOrWhiteSpace())
		{
			this.nameable.SetName(this.lockerName);
		}
		base.Trigger(-1683615038, null);
	}

	// Token: 0x0600374E RID: 14158 RVA: 0x00137490 File Offset: 0x00135690
	protected override void OnCleanUp()
	{
		this.filteredStorage.CleanUp();
	}

	// Token: 0x0600374F RID: 14159 RVA: 0x001374A0 File Offset: 0x001356A0
	private void OnCopySettings(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (gameObject == null)
		{
			return;
		}
		StorageLocker component = gameObject.GetComponent<StorageLocker>();
		if (component == null)
		{
			return;
		}
		this.UserMaxCapacity = component.UserMaxCapacity;
	}

	// Token: 0x06003750 RID: 14160 RVA: 0x001374DB File Offset: 0x001356DB
	public void UpdateForbiddenTag(Tag game_tag, bool forbidden)
	{
		if (forbidden)
		{
			this.filteredStorage.RemoveForbiddenTag(game_tag);
			return;
		}
		this.filteredStorage.AddForbiddenTag(game_tag);
	}

	// Token: 0x17000399 RID: 921
	// (get) Token: 0x06003751 RID: 14161 RVA: 0x001374F9 File Offset: 0x001356F9
	// (set) Token: 0x06003752 RID: 14162 RVA: 0x00137511 File Offset: 0x00135711
	public virtual float UserMaxCapacity
	{
		get
		{
			return Mathf.Min(this.userMaxCapacity, base.GetComponent<Storage>().capacityKg);
		}
		set
		{
			this.userMaxCapacity = value;
			this.filteredStorage.FilterChanged();
		}
	}

	// Token: 0x1700039A RID: 922
	// (get) Token: 0x06003753 RID: 14163 RVA: 0x00137525 File Offset: 0x00135725
	public float AmountStored
	{
		get
		{
			return base.GetComponent<Storage>().MassStored();
		}
	}

	// Token: 0x1700039B RID: 923
	// (get) Token: 0x06003754 RID: 14164 RVA: 0x00137532 File Offset: 0x00135732
	public float MinCapacity
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700039C RID: 924
	// (get) Token: 0x06003755 RID: 14165 RVA: 0x00137539 File Offset: 0x00135739
	public float MaxCapacity
	{
		get
		{
			return base.GetComponent<Storage>().capacityKg;
		}
	}

	// Token: 0x1700039D RID: 925
	// (get) Token: 0x06003756 RID: 14166 RVA: 0x00137546 File Offset: 0x00135746
	public bool WholeValues
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700039E RID: 926
	// (get) Token: 0x06003757 RID: 14167 RVA: 0x00137549 File Offset: 0x00135749
	public LocString CapacityUnits
	{
		get
		{
			return GameUtil.GetCurrentMassUnit(false);
		}
	}

	// Token: 0x040021AF RID: 8623
	private LoggerFS log;

	// Token: 0x040021B0 RID: 8624
	[Serialize]
	private float userMaxCapacity = float.PositiveInfinity;

	// Token: 0x040021B1 RID: 8625
	[Serialize]
	public string lockerName = "";

	// Token: 0x040021B2 RID: 8626
	protected FilteredStorage filteredStorage;

	// Token: 0x040021B3 RID: 8627
	[MyCmpGet]
	private UserNameable nameable;

	// Token: 0x040021B4 RID: 8628
	public string choreTypeID = Db.Get().ChoreTypes.StorageFetch.Id;

	// Token: 0x040021B5 RID: 8629
	private static readonly EventSystem.IntraObjectHandler<StorageLocker> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<StorageLocker>(delegate(StorageLocker component, object data)
	{
		component.OnCopySettings(data);
	});
}
