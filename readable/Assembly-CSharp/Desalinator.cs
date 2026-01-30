using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200073B RID: 1851
[SerializationConfig(MemberSerialization.OptIn)]
public class Desalinator : StateMachineComponent<Desalinator.StatesInstance>
{
	// Token: 0x1700027C RID: 636
	// (get) Token: 0x06002E9E RID: 11934 RVA: 0x0010D492 File Offset: 0x0010B692
	// (set) Token: 0x06002E9F RID: 11935 RVA: 0x0010D49A File Offset: 0x0010B69A
	public float SaltStorageLeft
	{
		get
		{
			return this._storageLeft;
		}
		set
		{
			this._storageLeft = value;
			base.smi.sm.saltStorageLeft.Set(value, base.smi, false);
		}
	}

	// Token: 0x06002EA0 RID: 11936 RVA: 0x0010D4C4 File Offset: 0x0010B6C4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.deliveryComponents = base.GetComponents<ManualDeliveryKG>();
		this.OnConduitConnectionChanged(base.GetComponent<ConduitConsumer>().IsConnected);
		base.Subscribe<Desalinator>(-2094018600, Desalinator.OnConduitConnectionChangedDelegate);
		base.smi.StartSM();
	}

	// Token: 0x06002EA1 RID: 11937 RVA: 0x0010D510 File Offset: 0x0010B710
	private void OnConduitConnectionChanged(object data)
	{
		this.OnConduitConnectionChanged(((Boxed<bool>)data).value);
	}

	// Token: 0x06002EA2 RID: 11938 RVA: 0x0010D524 File Offset: 0x0010B724
	private void OnConduitConnectionChanged(bool is_connected)
	{
		foreach (ManualDeliveryKG manualDeliveryKG in this.deliveryComponents)
		{
			Element element = ElementLoader.GetElement(manualDeliveryKG.RequestedItemTag);
			if (element != null && element.IsLiquid)
			{
				manualDeliveryKG.Pause(is_connected, "pipe connected");
			}
		}
	}

	// Token: 0x06002EA3 RID: 11939 RVA: 0x0010D570 File Offset: 0x0010B770
	private void OnRefreshUserMenu(object data)
	{
		if (base.smi.GetCurrentState() == base.smi.sm.full || !base.smi.HasSalt || base.smi.emptyChore != null)
		{
			return;
		}
		Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("status_item_desalinator_needs_emptying", UI.USERMENUACTIONS.EMPTYDESALINATOR.NAME, delegate()
		{
			base.smi.GoTo(base.smi.sm.earlyEmpty);
		}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CLEANTOILET.TOOLTIP, true), 1f);
	}

	// Token: 0x06002EA4 RID: 11940 RVA: 0x0010D604 File Offset: 0x0010B804
	private bool CheckCanConvert()
	{
		if (this.converters == null)
		{
			this.converters = base.GetComponents<ElementConverter>();
		}
		for (int i = 0; i < this.converters.Length; i++)
		{
			if (this.converters[i].CanConvertAtAll())
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002EA5 RID: 11941 RVA: 0x0010D64C File Offset: 0x0010B84C
	private bool CheckEnoughMassToConvert()
	{
		if (this.converters == null)
		{
			this.converters = base.GetComponents<ElementConverter>();
		}
		for (int i = 0; i < this.converters.Length; i++)
		{
			if (this.converters[i].HasEnoughMassToStartConverting(false))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x04001B9D RID: 7069
	[MyCmpAdd]
	private ManuallySetRemoteWorkTargetComponent remoteChore;

	// Token: 0x04001B9E RID: 7070
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04001B9F RID: 7071
	private ManualDeliveryKG[] deliveryComponents;

	// Token: 0x04001BA0 RID: 7072
	[MyCmpReq]
	private Storage storage;

	// Token: 0x04001BA1 RID: 7073
	[Serialize]
	public float maxSalt = 1000f;

	// Token: 0x04001BA2 RID: 7074
	[Serialize]
	private float _storageLeft = 1000f;

	// Token: 0x04001BA3 RID: 7075
	private ElementConverter[] converters;

	// Token: 0x04001BA4 RID: 7076
	private static readonly EventSystem.IntraObjectHandler<Desalinator> OnConduitConnectionChangedDelegate = new EventSystem.IntraObjectHandler<Desalinator>(delegate(Desalinator component, object data)
	{
		component.OnConduitConnectionChanged(data);
	});

	// Token: 0x02001610 RID: 5648
	public class StatesInstance : GameStateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.GameInstance
	{
		// Token: 0x060095E0 RID: 38368 RVA: 0x0037D8BA File Offset: 0x0037BABA
		public StatesInstance(Desalinator smi) : base(smi)
		{
		}

		// Token: 0x17000A2C RID: 2604
		// (get) Token: 0x060095E1 RID: 38369 RVA: 0x0037D8C3 File Offset: 0x0037BAC3
		public bool HasSalt
		{
			get
			{
				return base.master.storage.Has(ElementLoader.FindElementByHash(SimHashes.Salt).tag);
			}
		}

		// Token: 0x060095E2 RID: 38370 RVA: 0x0037D8E4 File Offset: 0x0037BAE4
		public bool IsFull()
		{
			return base.master.SaltStorageLeft <= 0f;
		}

		// Token: 0x060095E3 RID: 38371 RVA: 0x0037D8FB File Offset: 0x0037BAFB
		public bool IsSaltRemoved()
		{
			return !this.HasSalt;
		}

		// Token: 0x060095E4 RID: 38372 RVA: 0x0037D908 File Offset: 0x0037BB08
		public void CreateEmptyChore()
		{
			if (this.emptyChore != null)
			{
				this.emptyChore.Cancel("dupe");
			}
			DesalinatorWorkableEmpty component = base.master.GetComponent<DesalinatorWorkableEmpty>();
			this.emptyChore = new WorkChore<DesalinatorWorkableEmpty>(Db.Get().ChoreTypes.EmptyDesalinator, component, null, true, new Action<Chore>(this.OnEmptyComplete), null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, true, true);
			base.smi.master.remoteChore.SetChore(this.emptyChore);
		}

		// Token: 0x060095E5 RID: 38373 RVA: 0x0037D98B File Offset: 0x0037BB8B
		public void CancelEmptyChore()
		{
			if (this.emptyChore != null)
			{
				this.emptyChore.Cancel("Cancelled");
				this.emptyChore = null;
				base.smi.master.remoteChore.SetChore(null);
			}
		}

		// Token: 0x060095E6 RID: 38374 RVA: 0x0037D9C4 File Offset: 0x0037BBC4
		private void OnEmptyComplete(Chore chore)
		{
			this.emptyChore = null;
			Tag tag = GameTagExtensions.Create(SimHashes.Salt);
			ListPool<GameObject, Desalinator>.PooledList pooledList = ListPool<GameObject, Desalinator>.Allocate();
			base.master.storage.Find(tag, pooledList);
			foreach (GameObject go in pooledList)
			{
				base.master.storage.Drop(go, true);
			}
			pooledList.Recycle();
		}

		// Token: 0x060095E7 RID: 38375 RVA: 0x0037DA50 File Offset: 0x0037BC50
		public void UpdateStorageLeft()
		{
			Tag tag = GameTagExtensions.Create(SimHashes.Salt);
			base.master.SaltStorageLeft = base.master.maxSalt - base.master.storage.GetMassAvailable(tag);
		}

		// Token: 0x040073A7 RID: 29607
		public Chore emptyChore;
	}

	// Token: 0x02001611 RID: 5649
	public class States : GameStateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator>
	{
		// Token: 0x060095E8 RID: 38376 RVA: 0x0037DA90 File Offset: 0x0037BC90
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (Desalinator.StatesInstance smi) => smi.master.operational.IsOperational);
			this.on.PlayAnim("on").EventTransition(GameHashes.OperationalChanged, this.off, (Desalinator.StatesInstance smi) => !smi.master.operational.IsOperational).DefaultState(this.on.waiting);
			this.on.waiting.EventTransition(GameHashes.OnStorageChange, this.on.working_pre, (Desalinator.StatesInstance smi) => smi.master.CheckEnoughMassToConvert());
			this.on.working_pre.PlayAnim("working_pre").OnAnimQueueComplete(this.on.working);
			this.on.working.Enter(delegate(Desalinator.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).QueueAnim("working_loop", true, null).EventTransition(GameHashes.OnStorageChange, this.on.working_pst, (Desalinator.StatesInstance smi) => !smi.master.CheckCanConvert()).ParamTransition<float>(this.saltStorageLeft, this.full, (Desalinator.StatesInstance smi, float p) => smi.IsFull()).EventHandler(GameHashes.OnStorageChange, delegate(Desalinator.StatesInstance smi)
			{
				smi.UpdateStorageLeft();
			}).Exit(delegate(Desalinator.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			});
			this.on.working_pst.PlayAnim("working_pst").OnAnimQueueComplete(this.on.waiting);
			this.earlyEmpty.PlayAnims((Desalinator.StatesInstance smi) => Desalinator.States.FULL_ANIMS, KAnim.PlayMode.Once).OnAnimQueueComplete(this.earlyWaitingForEmpty);
			this.earlyWaitingForEmpty.Enter(delegate(Desalinator.StatesInstance smi)
			{
				smi.CreateEmptyChore();
			}).Exit(delegate(Desalinator.StatesInstance smi)
			{
				smi.CancelEmptyChore();
			}).EventTransition(GameHashes.OnStorageChange, this.empty, (Desalinator.StatesInstance smi) => smi.IsSaltRemoved());
			this.full.PlayAnims((Desalinator.StatesInstance smi) => Desalinator.States.FULL_ANIMS, KAnim.PlayMode.Once).OnAnimQueueComplete(this.fullWaitingForEmpty);
			this.fullWaitingForEmpty.Enter(delegate(Desalinator.StatesInstance smi)
			{
				smi.CreateEmptyChore();
			}).Exit(delegate(Desalinator.StatesInstance smi)
			{
				smi.CancelEmptyChore();
			}).ToggleMainStatusItem(Db.Get().BuildingStatusItems.DesalinatorNeedsEmptying, null).EventTransition(GameHashes.OnStorageChange, this.empty, (Desalinator.StatesInstance smi) => smi.IsSaltRemoved());
			this.empty.PlayAnim("off").Enter("ResetStorage", delegate(Desalinator.StatesInstance smi)
			{
				smi.master.SaltStorageLeft = smi.master.maxSalt;
			}).GoTo(this.on.waiting);
		}

		// Token: 0x040073A8 RID: 29608
		public GameStateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.State off;

		// Token: 0x040073A9 RID: 29609
		public Desalinator.States.OnStates on;

		// Token: 0x040073AA RID: 29610
		public GameStateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.State full;

		// Token: 0x040073AB RID: 29611
		public GameStateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.State fullWaitingForEmpty;

		// Token: 0x040073AC RID: 29612
		public GameStateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.State earlyEmpty;

		// Token: 0x040073AD RID: 29613
		public GameStateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.State earlyWaitingForEmpty;

		// Token: 0x040073AE RID: 29614
		public GameStateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.State empty;

		// Token: 0x040073AF RID: 29615
		private static readonly HashedString[] FULL_ANIMS = new HashedString[]
		{
			"working_pst",
			"off"
		};

		// Token: 0x040073B0 RID: 29616
		public StateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.FloatParameter saltStorageLeft = new StateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.FloatParameter(0f);

		// Token: 0x020028D6 RID: 10454
		public class OnStates : GameStateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.State
		{
			// Token: 0x0400B407 RID: 46087
			public GameStateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.State waiting;

			// Token: 0x0400B408 RID: 46088
			public GameStateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.State working_pre;

			// Token: 0x0400B409 RID: 46089
			public GameStateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.State working;

			// Token: 0x0400B40A RID: 46090
			public GameStateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.State working_pst;
		}
	}
}
