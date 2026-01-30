using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020007CD RID: 1997
public class ObjectDispenser : Switch, IUserControlledCapacity
{
	// Token: 0x17000349 RID: 841
	// (get) Token: 0x060034E6 RID: 13542 RVA: 0x0012BDC8 File Offset: 0x00129FC8
	// (set) Token: 0x060034E7 RID: 13543 RVA: 0x0012BDE0 File Offset: 0x00129FE0
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

	// Token: 0x1700034A RID: 842
	// (get) Token: 0x060034E8 RID: 13544 RVA: 0x0012BDF4 File Offset: 0x00129FF4
	public float AmountStored
	{
		get
		{
			return base.GetComponent<Storage>().MassStored();
		}
	}

	// Token: 0x1700034B RID: 843
	// (get) Token: 0x060034E9 RID: 13545 RVA: 0x0012BE01 File Offset: 0x0012A001
	public float MinCapacity
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700034C RID: 844
	// (get) Token: 0x060034EA RID: 13546 RVA: 0x0012BE08 File Offset: 0x0012A008
	public float MaxCapacity
	{
		get
		{
			return base.GetComponent<Storage>().capacityKg;
		}
	}

	// Token: 0x1700034D RID: 845
	// (get) Token: 0x060034EB RID: 13547 RVA: 0x0012BE15 File Offset: 0x0012A015
	public bool WholeValues
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700034E RID: 846
	// (get) Token: 0x060034EC RID: 13548 RVA: 0x0012BE18 File Offset: 0x0012A018
	public LocString CapacityUnits
	{
		get
		{
			return GameUtil.GetCurrentMassUnit(false);
		}
	}

	// Token: 0x060034ED RID: 13549 RVA: 0x0012BE20 File Offset: 0x0012A020
	protected override void OnPrefabInit()
	{
		this.Initialize();
	}

	// Token: 0x060034EE RID: 13550 RVA: 0x0012BE28 File Offset: 0x0012A028
	protected void Initialize()
	{
		base.OnPrefabInit();
		this.log = new LoggerFS("ObjectDispenser", 35);
		this.filteredStorage = new FilteredStorage(this, null, this, false, Db.Get().ChoreTypes.StorageFetch);
		base.Subscribe<ObjectDispenser>(-905833192, ObjectDispenser.OnCopySettingsDelegate);
	}

	// Token: 0x060034EF RID: 13551 RVA: 0x0012BE7C File Offset: 0x0012A07C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.smi = new ObjectDispenser.Instance(this, base.IsSwitchedOn);
		this.smi.StartSM();
		if (ObjectDispenser.infoStatusItem == null)
		{
			ObjectDispenser.infoStatusItem = new StatusItem("ObjectDispenserAutomationInfo", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			ObjectDispenser.infoStatusItem.resolveStringCallback = new Func<string, object, string>(ObjectDispenser.ResolveInfoStatusItemString);
		}
		this.filteredStorage.FilterChanged();
		base.GetComponent<KSelectable>().ToggleStatusItem(ObjectDispenser.infoStatusItem, true, this.smi);
	}

	// Token: 0x060034F0 RID: 13552 RVA: 0x0012BF14 File Offset: 0x0012A114
	protected override void OnCleanUp()
	{
		this.filteredStorage.CleanUp();
		base.OnCleanUp();
	}

	// Token: 0x060034F1 RID: 13553 RVA: 0x0012BF28 File Offset: 0x0012A128
	private void OnCopySettings(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (gameObject == null)
		{
			return;
		}
		ObjectDispenser component = gameObject.GetComponent<ObjectDispenser>();
		if (component == null)
		{
			return;
		}
		this.UserMaxCapacity = component.UserMaxCapacity;
	}

	// Token: 0x060034F2 RID: 13554 RVA: 0x0012BF64 File Offset: 0x0012A164
	public void DropHeldItems()
	{
		while (this.storage.Count > 0)
		{
			GameObject gameObject = this.storage.Drop(this.storage.items[0], true);
			if (this.rotatable != null)
			{
				gameObject.transform.SetPosition(base.transform.GetPosition() + this.rotatable.GetRotatedCellOffset(this.dropOffset).ToVector3());
			}
			else
			{
				gameObject.transform.SetPosition(base.transform.GetPosition() + this.dropOffset.ToVector3());
			}
		}
		this.smi.GetMaster().GetComponent<Storage>().DropAll(false, false, default(Vector3), true, null);
	}

	// Token: 0x060034F3 RID: 13555 RVA: 0x0012C033 File Offset: 0x0012A233
	protected override void Toggle()
	{
		base.Toggle();
	}

	// Token: 0x060034F4 RID: 13556 RVA: 0x0012C03B File Offset: 0x0012A23B
	protected override void OnRefreshUserMenu(object data)
	{
		if (!this.smi.IsAutomated())
		{
			base.OnRefreshUserMenu(data);
		}
	}

	// Token: 0x060034F5 RID: 13557 RVA: 0x0012C054 File Offset: 0x0012A254
	private static string ResolveInfoStatusItemString(string format_str, object data)
	{
		ObjectDispenser.Instance instance = (ObjectDispenser.Instance)data;
		string format = instance.IsAutomated() ? BUILDING.STATUSITEMS.OBJECTDISPENSER.AUTOMATION_CONTROL : BUILDING.STATUSITEMS.OBJECTDISPENSER.MANUAL_CONTROL;
		string arg = instance.IsOpened ? BUILDING.STATUSITEMS.OBJECTDISPENSER.OPENED : BUILDING.STATUSITEMS.OBJECTDISPENSER.CLOSED;
		return string.Format(format, arg);
	}

	// Token: 0x04001FFE RID: 8190
	public static readonly HashedString PORT_ID = "ObjectDispenser";

	// Token: 0x04001FFF RID: 8191
	private LoggerFS log;

	// Token: 0x04002000 RID: 8192
	public CellOffset dropOffset;

	// Token: 0x04002001 RID: 8193
	[MyCmpReq]
	private Building building;

	// Token: 0x04002002 RID: 8194
	[MyCmpReq]
	private Storage storage;

	// Token: 0x04002003 RID: 8195
	[MyCmpGet]
	private Rotatable rotatable;

	// Token: 0x04002004 RID: 8196
	private ObjectDispenser.Instance smi;

	// Token: 0x04002005 RID: 8197
	private static StatusItem infoStatusItem;

	// Token: 0x04002006 RID: 8198
	[Serialize]
	private float userMaxCapacity = float.PositiveInfinity;

	// Token: 0x04002007 RID: 8199
	protected FilteredStorage filteredStorage;

	// Token: 0x04002008 RID: 8200
	private static readonly EventSystem.IntraObjectHandler<ObjectDispenser> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<ObjectDispenser>(delegate(ObjectDispenser component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x02001718 RID: 5912
	public class States : GameStateMachine<ObjectDispenser.States, ObjectDispenser.Instance, ObjectDispenser>
	{
		// Token: 0x060099F3 RID: 39411 RVA: 0x0038F31C File Offset: 0x0038D51C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.idle.PlayAnim("on").EventHandler(GameHashes.OnStorageChange, delegate(ObjectDispenser.Instance smi)
			{
				smi.UpdateState();
			}).ParamTransition<bool>(this.should_open, this.drop_item, (ObjectDispenser.Instance smi, bool p) => p && !smi.master.GetComponent<Storage>().IsEmpty());
			this.load_item.PlayAnim("working_load").OnAnimQueueComplete(this.load_item_pst);
			this.load_item_pst.ParamTransition<bool>(this.should_open, this.idle, (ObjectDispenser.Instance smi, bool p) => !p).ParamTransition<bool>(this.should_open, this.drop_item, (ObjectDispenser.Instance smi, bool p) => p);
			this.drop_item.PlayAnim("working_dispense").OnAnimQueueComplete(this.idle).Exit(delegate(ObjectDispenser.Instance smi)
			{
				smi.master.DropHeldItems();
			});
		}

		// Token: 0x040076DF RID: 30431
		public GameStateMachine<ObjectDispenser.States, ObjectDispenser.Instance, ObjectDispenser, object>.State load_item;

		// Token: 0x040076E0 RID: 30432
		public GameStateMachine<ObjectDispenser.States, ObjectDispenser.Instance, ObjectDispenser, object>.State load_item_pst;

		// Token: 0x040076E1 RID: 30433
		public GameStateMachine<ObjectDispenser.States, ObjectDispenser.Instance, ObjectDispenser, object>.State drop_item;

		// Token: 0x040076E2 RID: 30434
		public GameStateMachine<ObjectDispenser.States, ObjectDispenser.Instance, ObjectDispenser, object>.State idle;

		// Token: 0x040076E3 RID: 30435
		public StateMachine<ObjectDispenser.States, ObjectDispenser.Instance, ObjectDispenser, object>.BoolParameter should_open;
	}

	// Token: 0x02001719 RID: 5913
	public class Instance : GameStateMachine<ObjectDispenser.States, ObjectDispenser.Instance, ObjectDispenser, object>.GameInstance
	{
		// Token: 0x060099F5 RID: 39413 RVA: 0x0038F470 File Offset: 0x0038D670
		public Instance(ObjectDispenser master, bool manual_start_state) : base(master)
		{
			this.manual_on = manual_start_state;
			this.operational = base.GetComponent<Operational>();
			this.logic = base.GetComponent<LogicPorts>();
			base.Subscribe(-592767678, new Action<object>(this.OnOperationalChanged));
			base.Subscribe(-801688580, new Action<object>(this.OnLogicValueChanged));
			base.smi.sm.should_open.Set(true, base.smi, false);
		}

		// Token: 0x060099F6 RID: 39414 RVA: 0x0038F4F8 File Offset: 0x0038D6F8
		public void UpdateState()
		{
			base.smi.GoTo(base.sm.load_item);
		}

		// Token: 0x060099F7 RID: 39415 RVA: 0x0038F510 File Offset: 0x0038D710
		public bool IsAutomated()
		{
			return this.logic.IsPortConnected(ObjectDispenser.PORT_ID);
		}

		// Token: 0x17000A6E RID: 2670
		// (get) Token: 0x060099F8 RID: 39416 RVA: 0x0038F522 File Offset: 0x0038D722
		public bool IsOpened
		{
			get
			{
				if (!this.IsAutomated())
				{
					return this.manual_on;
				}
				return this.logic_on;
			}
		}

		// Token: 0x060099F9 RID: 39417 RVA: 0x0038F539 File Offset: 0x0038D739
		public void SetSwitchState(bool on)
		{
			this.manual_on = on;
			this.UpdateShouldOpen();
		}

		// Token: 0x060099FA RID: 39418 RVA: 0x0038F548 File Offset: 0x0038D748
		public void SetActive(bool active)
		{
			this.operational.SetActive(active, false);
		}

		// Token: 0x060099FB RID: 39419 RVA: 0x0038F557 File Offset: 0x0038D757
		private void OnOperationalChanged(object _)
		{
			this.UpdateShouldOpen();
		}

		// Token: 0x060099FC RID: 39420 RVA: 0x0038F560 File Offset: 0x0038D760
		private void OnLogicValueChanged(object data)
		{
			LogicValueChanged logicValueChanged = (LogicValueChanged)data;
			if (logicValueChanged.portID != ObjectDispenser.PORT_ID)
			{
				return;
			}
			this.logic_on = LogicCircuitNetwork.IsBitActive(0, logicValueChanged.newValue);
			this.UpdateShouldOpen();
		}

		// Token: 0x060099FD RID: 39421 RVA: 0x0038F5A0 File Offset: 0x0038D7A0
		private void UpdateShouldOpen()
		{
			this.SetActive(this.operational.IsOperational);
			if (!this.operational.IsOperational)
			{
				return;
			}
			if (this.IsAutomated())
			{
				base.smi.sm.should_open.Set(this.logic_on, base.smi, false);
				return;
			}
			base.smi.sm.should_open.Set(this.manual_on, base.smi, false);
		}

		// Token: 0x040076E4 RID: 30436
		private Operational operational;

		// Token: 0x040076E5 RID: 30437
		public LogicPorts logic;

		// Token: 0x040076E6 RID: 30438
		public bool logic_on = true;

		// Token: 0x040076E7 RID: 30439
		private bool manual_on;
	}
}
