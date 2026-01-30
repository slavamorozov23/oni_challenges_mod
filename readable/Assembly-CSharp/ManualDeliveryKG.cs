using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020009F7 RID: 2551
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/ManualDeliveryKG")]
public class ManualDeliveryKG : KMonoBehaviour, ISim1000ms
{
	// Token: 0x1700052E RID: 1326
	// (get) Token: 0x06004A7D RID: 19069 RVA: 0x001AF685 File Offset: 0x001AD885
	public bool IsPaused
	{
		get
		{
			return this.paused;
		}
	}

	// Token: 0x1700052F RID: 1327
	// (get) Token: 0x06004A7E RID: 19070 RVA: 0x001AF68D File Offset: 0x001AD88D
	public float Capacity
	{
		get
		{
			return this.capacity;
		}
	}

	// Token: 0x17000530 RID: 1328
	// (get) Token: 0x06004A7F RID: 19071 RVA: 0x001AF695 File Offset: 0x001AD895
	// (set) Token: 0x06004A80 RID: 19072 RVA: 0x001AF69D File Offset: 0x001AD89D
	public Tag RequestedItemTag
	{
		get
		{
			return this.requestedItemTag;
		}
		set
		{
			this.requestedItemTag = value;
			this.AbortDelivery("Requested Item Tag Changed");
		}
	}

	// Token: 0x17000531 RID: 1329
	// (get) Token: 0x06004A81 RID: 19073 RVA: 0x001AF6B1 File Offset: 0x001AD8B1
	// (set) Token: 0x06004A82 RID: 19074 RVA: 0x001AF6B9 File Offset: 0x001AD8B9
	public Tag[] ForbiddenTags
	{
		get
		{
			return this.forbiddenTags;
		}
		set
		{
			this.forbiddenTags = value;
			this.AbortDelivery("Forbidden Tags Changed");
		}
	}

	// Token: 0x17000532 RID: 1330
	// (get) Token: 0x06004A83 RID: 19075 RVA: 0x001AF6CD File Offset: 0x001AD8CD
	public Storage DebugStorage
	{
		get
		{
			return this.storage;
		}
	}

	// Token: 0x17000533 RID: 1331
	// (get) Token: 0x06004A84 RID: 19076 RVA: 0x001AF6D5 File Offset: 0x001AD8D5
	public FetchList2 DebugFetchList
	{
		get
		{
			return this.fetchList;
		}
	}

	// Token: 0x17000534 RID: 1332
	// (get) Token: 0x06004A85 RID: 19077 RVA: 0x001AF6DD File Offset: 0x001AD8DD
	private float MassStored
	{
		get
		{
			return this.storage.GetMassAvailable(this.requestedItemTag);
		}
	}

	// Token: 0x06004A86 RID: 19078 RVA: 0x001AF6F0 File Offset: 0x001AD8F0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		DebugUtil.Assert(this.choreTypeIDHash.IsValid, "ManualDeliveryKG Must have a valid chore type specified!", base.name);
		if (this.allowPause)
		{
			base.Subscribe<ManualDeliveryKG>(493375141, ManualDeliveryKG.OnRefreshUserMenuDelegate);
			base.Subscribe<ManualDeliveryKG>(-111137758, ManualDeliveryKG.OnRefreshUserMenuDelegate);
		}
		base.Subscribe<ManualDeliveryKG>(-592767678, ManualDeliveryKG.OnOperationalChangedDelegate);
		if (this.storage != null)
		{
			this.SetStorage(this.storage);
		}
		if (this.handlePrioritizable)
		{
			Prioritizable.AddRef(base.gameObject);
		}
		if (this.userPaused && this.allowPause)
		{
			this.OnPause();
		}
	}

	// Token: 0x06004A87 RID: 19079 RVA: 0x001AF79C File Offset: 0x001AD99C
	protected override void OnCleanUp()
	{
		this.AbortDelivery("ManualDeliverKG destroyed");
		if (this.handlePrioritizable)
		{
			Prioritizable.RemoveRef(base.gameObject);
		}
		base.OnCleanUp();
	}

	// Token: 0x06004A88 RID: 19080 RVA: 0x001AF7C4 File Offset: 0x001AD9C4
	public void SetStorage(Storage storage)
	{
		if (this.storage != null)
		{
			this.storage.Unsubscribe(this.onStorageChangeSubscription);
			this.onStorageChangeSubscription = -1;
		}
		this.AbortDelivery("storage pointer changed");
		this.storage = storage;
		if (this.storage != null && base.isSpawned)
		{
			global::Debug.Assert(this.onStorageChangeSubscription == -1);
			this.onStorageChangeSubscription = this.storage.Subscribe<ManualDeliveryKG>(-1697596308, ManualDeliveryKG.OnStorageChangedDelegate);
		}
	}

	// Token: 0x06004A89 RID: 19081 RVA: 0x001AF848 File Offset: 0x001ADA48
	public void Pause(bool pause, string reason)
	{
		if (this.paused != pause)
		{
			this.paused = pause;
			if (pause)
			{
				this.AbortDelivery(reason);
			}
		}
	}

	// Token: 0x06004A8A RID: 19082 RVA: 0x001AF864 File Offset: 0x001ADA64
	public void Sim1000ms(float dt)
	{
		this.UpdateDeliveryState();
	}

	// Token: 0x06004A8B RID: 19083 RVA: 0x001AF86C File Offset: 0x001ADA6C
	[ContextMenu("UpdateDeliveryState")]
	public void UpdateDeliveryState()
	{
		if (!this.requestedItemTag.IsValid)
		{
			return;
		}
		if (this.storage == null)
		{
			return;
		}
		this.UpdateFetchList();
	}

	// Token: 0x06004A8C RID: 19084 RVA: 0x001AF894 File Offset: 0x001ADA94
	public void RequestDelivery()
	{
		if (this.fetchList != null)
		{
			return;
		}
		float massStored = this.MassStored;
		if (massStored < this.capacity)
		{
			this.CreateFetchChore(massStored);
		}
	}

	// Token: 0x06004A8D RID: 19085 RVA: 0x001AF8C4 File Offset: 0x001ADAC4
	private void CreateFetchChore(float stored_mass)
	{
		float num = this.capacity - stored_mass;
		num = Mathf.Max(PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT, num);
		if (this.RoundFetchAmountToInt)
		{
			num = (float)((int)num);
			if (num < 0.1f)
			{
				return;
			}
		}
		ChoreType byHash = Db.Get().ChoreTypes.GetByHash(this.choreTypeIDHash);
		this.fetchList = new FetchList2(this.storage, byHash);
		this.fetchList.ShowStatusItem = this.ShowStatusItem;
		this.fetchList.MinimumAmount[this.requestedItemTag] = Mathf.Max(PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT, this.MinimumMass);
		FetchList2 fetchList = this.fetchList;
		Tag tag = this.requestedItemTag;
		float amount = num;
		fetchList.Add(tag, this.forbiddenTags, amount, Operational.State.None);
		this.fetchList.Submit(new System.Action(this.OnFetchComplete), false);
	}

	// Token: 0x06004A8E RID: 19086 RVA: 0x001AF990 File Offset: 0x001ADB90
	private void OnFetchComplete()
	{
		if (this.FillToCapacity && this.storage != null)
		{
			float amountAvailable = this.storage.GetAmountAvailable(this.requestedItemTag);
			if (amountAvailable < this.capacity)
			{
				this.CreateFetchChore(amountAvailable);
			}
		}
	}

	// Token: 0x06004A8F RID: 19087 RVA: 0x001AF9D8 File Offset: 0x001ADBD8
	private void UpdateFetchList()
	{
		if (this.paused)
		{
			return;
		}
		if (this.fetchList != null && this.fetchList.IsComplete)
		{
			this.fetchList = null;
		}
		if (!(this.operational == null) && !this.operational.MeetsRequirements(this.operationalRequirement))
		{
			if (this.fetchList != null)
			{
				this.fetchList.Cancel("Operational requirements");
				this.fetchList = null;
				return;
			}
		}
		else if (this.fetchList == null)
		{
			if (this.MassStored < this.refillMass)
			{
				this.RequestDelivery();
				return;
			}
		}
		else if (this.FillToMinimumMass)
		{
			Dictionary<Tag, float> remaining = this.fetchList.GetRemaining();
			if (remaining.ContainsKey(this.requestedItemTag) && remaining[this.requestedItemTag] < this.MinimumMass)
			{
				this.AbortDelivery("Invalid Mass");
			}
		}
	}

	// Token: 0x06004A90 RID: 19088 RVA: 0x001AFAA9 File Offset: 0x001ADCA9
	public void AbortDelivery(string reason)
	{
		if (this.fetchList != null)
		{
			FetchList2 fetchList = this.fetchList;
			this.fetchList = null;
			fetchList.Cancel(reason);
		}
	}

	// Token: 0x06004A91 RID: 19089 RVA: 0x001AFAC6 File Offset: 0x001ADCC6
	protected void OnStorageChanged(object data)
	{
		this.UpdateDeliveryState();
	}

	// Token: 0x06004A92 RID: 19090 RVA: 0x001AFACE File Offset: 0x001ADCCE
	private void OnPause()
	{
		this.userPaused = true;
		this.Pause(true, "Forbid manual delivery");
	}

	// Token: 0x06004A93 RID: 19091 RVA: 0x001AFAE3 File Offset: 0x001ADCE3
	private void OnResume()
	{
		this.userPaused = false;
		this.Pause(false, "Allow manual delivery");
	}

	// Token: 0x06004A94 RID: 19092 RVA: 0x001AFAF8 File Offset: 0x001ADCF8
	private void OnRefreshUserMenu(object data)
	{
		if (!this.allowPause)
		{
			return;
		}
		KIconButtonMenu.ButtonInfo button = (!this.paused) ? new KIconButtonMenu.ButtonInfo("action_move_to_storage", UI.USERMENUACTIONS.MANUAL_DELIVERY.NAME, new System.Action(this.OnPause), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.MANUAL_DELIVERY.TOOLTIP, true) : new KIconButtonMenu.ButtonInfo("action_move_to_storage", UI.USERMENUACTIONS.MANUAL_DELIVERY.NAME_OFF, new System.Action(this.OnResume), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.MANUAL_DELIVERY.TOOLTIP_OFF, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x06004A95 RID: 19093 RVA: 0x001AFB9A File Offset: 0x001ADD9A
	private void OnOperationalChanged(object _)
	{
		this.UpdateDeliveryState();
	}

	// Token: 0x04003158 RID: 12632
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04003159 RID: 12633
	[SerializeField]
	private Storage storage;

	// Token: 0x0400315A RID: 12634
	[SerializeField]
	private Tag requestedItemTag;

	// Token: 0x0400315B RID: 12635
	private Tag[] forbiddenTags;

	// Token: 0x0400315C RID: 12636
	[SerializeField]
	public float capacity = 100f;

	// Token: 0x0400315D RID: 12637
	[SerializeField]
	public float refillMass = 10f;

	// Token: 0x0400315E RID: 12638
	[SerializeField]
	public float MinimumMass = 10f;

	// Token: 0x0400315F RID: 12639
	[SerializeField]
	public bool RoundFetchAmountToInt;

	// Token: 0x04003160 RID: 12640
	[SerializeField]
	public bool FillToCapacity;

	// Token: 0x04003161 RID: 12641
	[SerializeField]
	public Operational.State operationalRequirement;

	// Token: 0x04003162 RID: 12642
	[SerializeField]
	public bool allowPause;

	// Token: 0x04003163 RID: 12643
	[SerializeField]
	private bool paused;

	// Token: 0x04003164 RID: 12644
	[SerializeField]
	public HashedString choreTypeIDHash;

	// Token: 0x04003165 RID: 12645
	[Serialize]
	private bool userPaused;

	// Token: 0x04003166 RID: 12646
	public bool handlePrioritizable = true;

	// Token: 0x04003167 RID: 12647
	public bool ShowStatusItem = true;

	// Token: 0x04003168 RID: 12648
	public bool FillToMinimumMass;

	// Token: 0x04003169 RID: 12649
	private FetchList2 fetchList;

	// Token: 0x0400316A RID: 12650
	private int onStorageChangeSubscription = -1;

	// Token: 0x0400316B RID: 12651
	private static readonly EventSystem.IntraObjectHandler<ManualDeliveryKG> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<ManualDeliveryKG>(delegate(ManualDeliveryKG component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x0400316C RID: 12652
	private static readonly EventSystem.IntraObjectHandler<ManualDeliveryKG> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<ManualDeliveryKG>(delegate(ManualDeliveryKG component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x0400316D RID: 12653
	private static readonly EventSystem.IntraObjectHandler<ManualDeliveryKG> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<ManualDeliveryKG>(delegate(ManualDeliveryKG component, object data)
	{
		component.OnStorageChanged(data);
	});
}
