using System;
using STRINGS;

// Token: 0x02000764 RID: 1892
public class Gantry : Switch
{
	// Token: 0x06002FDC RID: 12252 RVA: 0x00114614 File Offset: 0x00112814
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (Gantry.infoStatusItem == null)
		{
			Gantry.infoStatusItem = new StatusItem("GantryAutomationInfo", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			Gantry.infoStatusItem.resolveStringCallback = new Func<string, object, string>(Gantry.ResolveInfoStatusItemString);
		}
		base.GetComponent<KAnimControllerBase>().PlaySpeedMultiplier = 0.5f;
		this.smi = new Gantry.Instance(this, base.IsSwitchedOn);
		this.smi.StartSM();
		base.GetComponent<KSelectable>().ToggleStatusItem(Gantry.infoStatusItem, true, this.smi);
	}

	// Token: 0x06002FDD RID: 12253 RVA: 0x001146B1 File Offset: 0x001128B1
	protected override void OnCleanUp()
	{
		if (this.smi != null)
		{
			this.smi.StopSM("cleanup");
		}
		base.OnCleanUp();
	}

	// Token: 0x06002FDE RID: 12254 RVA: 0x001146D1 File Offset: 0x001128D1
	public void SetWalkable(bool active)
	{
		this.fakeFloorAdder.SetFloor(active);
	}

	// Token: 0x06002FDF RID: 12255 RVA: 0x001146DF File Offset: 0x001128DF
	protected override void Toggle()
	{
		base.Toggle();
		this.smi.SetSwitchState(this.switchedOn);
	}

	// Token: 0x06002FE0 RID: 12256 RVA: 0x001146F8 File Offset: 0x001128F8
	protected override void OnRefreshUserMenu(object data)
	{
		if (!this.smi.IsAutomated())
		{
			base.OnRefreshUserMenu(data);
		}
	}

	// Token: 0x06002FE1 RID: 12257 RVA: 0x0011470E File Offset: 0x0011290E
	protected override void UpdateSwitchStatus()
	{
	}

	// Token: 0x06002FE2 RID: 12258 RVA: 0x00114710 File Offset: 0x00112910
	private static string ResolveInfoStatusItemString(string format_str, object data)
	{
		Gantry.Instance instance = (Gantry.Instance)data;
		string format = instance.IsAutomated() ? BUILDING.STATUSITEMS.GANTRY.AUTOMATION_CONTROL : BUILDING.STATUSITEMS.GANTRY.MANUAL_CONTROL;
		string arg = instance.IsExtended() ? BUILDING.STATUSITEMS.GANTRY.EXTENDED : BUILDING.STATUSITEMS.GANTRY.RETRACTED;
		return string.Format(format, arg);
	}

	// Token: 0x04001C82 RID: 7298
	public static readonly HashedString PORT_ID = "Gantry";

	// Token: 0x04001C83 RID: 7299
	[MyCmpReq]
	private Building building;

	// Token: 0x04001C84 RID: 7300
	[MyCmpReq]
	private FakeFloorAdder fakeFloorAdder;

	// Token: 0x04001C85 RID: 7301
	private Gantry.Instance smi;

	// Token: 0x04001C86 RID: 7302
	private static StatusItem infoStatusItem;

	// Token: 0x02001650 RID: 5712
	public class States : GameStateMachine<Gantry.States, Gantry.Instance, Gantry>
	{
		// Token: 0x060096C7 RID: 38599 RVA: 0x00380F3C File Offset: 0x0037F13C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.extended;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.retracted_pre.Enter(delegate(Gantry.Instance smi)
			{
				smi.SetActive(true);
			}).Exit(delegate(Gantry.Instance smi)
			{
				smi.SetActive(false);
			}).PlayAnim("off_pre").OnAnimQueueComplete(this.retracted);
			this.retracted.PlayAnim("off").ParamTransition<bool>(this.should_extend, this.extended_pre, GameStateMachine<Gantry.States, Gantry.Instance, Gantry, object>.IsTrue);
			this.extended_pre.Enter(delegate(Gantry.Instance smi)
			{
				smi.SetActive(true);
			}).Exit(delegate(Gantry.Instance smi)
			{
				smi.SetActive(false);
			}).PlayAnim("on_pre").OnAnimQueueComplete(this.extended);
			this.extended.Enter(delegate(Gantry.Instance smi)
			{
				smi.master.SetWalkable(true);
			}).Exit(delegate(Gantry.Instance smi)
			{
				smi.master.SetWalkable(false);
			}).PlayAnim("on").ParamTransition<bool>(this.should_extend, this.retracted_pre, GameStateMachine<Gantry.States, Gantry.Instance, Gantry, object>.IsFalse).ToggleTag(GameTags.GantryExtended);
		}

		// Token: 0x04007471 RID: 29809
		public GameStateMachine<Gantry.States, Gantry.Instance, Gantry, object>.State retracted_pre;

		// Token: 0x04007472 RID: 29810
		public GameStateMachine<Gantry.States, Gantry.Instance, Gantry, object>.State retracted;

		// Token: 0x04007473 RID: 29811
		public GameStateMachine<Gantry.States, Gantry.Instance, Gantry, object>.State extended_pre;

		// Token: 0x04007474 RID: 29812
		public GameStateMachine<Gantry.States, Gantry.Instance, Gantry, object>.State extended;

		// Token: 0x04007475 RID: 29813
		public StateMachine<Gantry.States, Gantry.Instance, Gantry, object>.BoolParameter should_extend;
	}

	// Token: 0x02001651 RID: 5713
	public class Instance : GameStateMachine<Gantry.States, Gantry.Instance, Gantry, object>.GameInstance
	{
		// Token: 0x060096C9 RID: 38601 RVA: 0x003810C8 File Offset: 0x0037F2C8
		public Instance(Gantry master, bool manual_start_state) : base(master)
		{
			this.manual_on = manual_start_state;
			this.operational = base.GetComponent<Operational>();
			this.logic = base.GetComponent<LogicPorts>();
			base.Subscribe(-592767678, new Action<object>(this.OnOperationalChanged));
			base.Subscribe(-801688580, new Action<object>(this.OnLogicValueChanged));
			base.smi.sm.should_extend.Set(true, base.smi, false);
		}

		// Token: 0x060096CA RID: 38602 RVA: 0x00381150 File Offset: 0x0037F350
		public bool IsAutomated()
		{
			return this.logic.IsPortConnected(Gantry.PORT_ID);
		}

		// Token: 0x060096CB RID: 38603 RVA: 0x00381162 File Offset: 0x0037F362
		public bool IsExtended()
		{
			if (!this.IsAutomated())
			{
				return this.manual_on;
			}
			return this.logic_on;
		}

		// Token: 0x060096CC RID: 38604 RVA: 0x00381179 File Offset: 0x0037F379
		public void SetSwitchState(bool on)
		{
			this.manual_on = on;
			this.UpdateShouldExtend();
		}

		// Token: 0x060096CD RID: 38605 RVA: 0x00381188 File Offset: 0x0037F388
		public void SetActive(bool active)
		{
			this.operational.SetActive(this.operational.IsOperational && active, false);
		}

		// Token: 0x060096CE RID: 38606 RVA: 0x003811A3 File Offset: 0x0037F3A3
		private void OnOperationalChanged(object _)
		{
			this.UpdateShouldExtend();
		}

		// Token: 0x060096CF RID: 38607 RVA: 0x003811AC File Offset: 0x0037F3AC
		private void OnLogicValueChanged(object data)
		{
			LogicValueChanged logicValueChanged = (LogicValueChanged)data;
			if (logicValueChanged.portID != Gantry.PORT_ID)
			{
				return;
			}
			this.logic_on = LogicCircuitNetwork.IsBitActive(0, logicValueChanged.newValue);
			this.UpdateShouldExtend();
		}

		// Token: 0x060096D0 RID: 38608 RVA: 0x003811EC File Offset: 0x0037F3EC
		private void UpdateShouldExtend()
		{
			if (!this.operational.IsOperational)
			{
				return;
			}
			if (this.IsAutomated())
			{
				base.smi.sm.should_extend.Set(this.logic_on, base.smi, false);
				return;
			}
			base.smi.sm.should_extend.Set(this.manual_on, base.smi, false);
		}

		// Token: 0x04007476 RID: 29814
		private Operational operational;

		// Token: 0x04007477 RID: 29815
		public LogicPorts logic;

		// Token: 0x04007478 RID: 29816
		public bool logic_on = true;

		// Token: 0x04007479 RID: 29817
		private bool manual_on;
	}
}
