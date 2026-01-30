using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020005E1 RID: 1505
public class FixedCapturePoint : GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>
{
	// Token: 0x060022DA RID: 8922 RVA: 0x000CB204 File Offset: 0x000C9404
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.operational;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.unoperational.TagTransition(GameTags.Operational, this.operational, false);
		this.operational.DefaultState(this.operational.manual).TagTransition(GameTags.Operational, this.unoperational, true);
		this.operational.manual.ParamTransition<bool>(this.automated, this.operational.automated, GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.IsTrue);
		this.operational.automated.ParamTransition<bool>(this.automated, this.operational.manual, GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.IsFalse).ToggleChore((FixedCapturePoint.Instance smi) => smi.CreateChore(), this.unoperational, this.unoperational).Update("FindFixedCapturable", delegate(FixedCapturePoint.Instance smi, float dt)
		{
			smi.FindFixedCapturable();
		}, UpdateRate.SIM_1000ms, false);
	}

	// Token: 0x0400146A RID: 5226
	public static readonly Operational.Flag enabledFlag = new Operational.Flag("enabled", Operational.Flag.Type.Requirement);

	// Token: 0x0400146B RID: 5227
	private StateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.BoolParameter automated;

	// Token: 0x0400146C RID: 5228
	public GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.State unoperational;

	// Token: 0x0400146D RID: 5229
	public FixedCapturePoint.OperationalState operational;

	// Token: 0x020014C0 RID: 5312
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006F82 RID: 28546
		public Func<FixedCapturePoint.Instance, FixedCapturableMonitor.Instance, bool> isAmountStoredOverCapacity;

		// Token: 0x04006F83 RID: 28547
		public Func<FixedCapturePoint.Instance, int> getTargetCapturePoint = delegate(FixedCapturePoint.Instance smi)
		{
			int num = Grid.PosToCell(smi);
			Navigator navigator = smi.targetCapturable.Navigator;
			if (Grid.IsValidCell(num - 1) && navigator.CanReach(num - 1))
			{
				return num - 1;
			}
			if (Grid.IsValidCell(num + 1) && navigator.CanReach(num + 1))
			{
				return num + 1;
			}
			return num;
		};

		// Token: 0x04006F84 RID: 28548
		public bool allowBabies;
	}

	// Token: 0x020014C1 RID: 5313
	public class OperationalState : GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.State
	{
		// Token: 0x04006F85 RID: 28549
		public GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.State manual;

		// Token: 0x04006F86 RID: 28550
		public GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.State automated;
	}

	// Token: 0x020014C2 RID: 5314
	[SerializationConfig(MemberSerialization.OptIn)]
	public new class Instance : GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.GameInstance
	{
		// Token: 0x170009EC RID: 2540
		// (get) Token: 0x060090F2 RID: 37106 RVA: 0x0036FE30 File Offset: 0x0036E030
		// (set) Token: 0x060090F3 RID: 37107 RVA: 0x0036FE38 File Offset: 0x0036E038
		public FixedCapturableMonitor.Instance targetCapturable { get; private set; }

		// Token: 0x170009ED RID: 2541
		// (get) Token: 0x060090F4 RID: 37108 RVA: 0x0036FE41 File Offset: 0x0036E041
		// (set) Token: 0x060090F5 RID: 37109 RVA: 0x0036FE49 File Offset: 0x0036E049
		public bool shouldCreatureGoGetCaptured { get; private set; }

		// Token: 0x060090F6 RID: 37110 RVA: 0x0036FE54 File Offset: 0x0036E054
		public Instance(IStateMachineTarget master, FixedCapturePoint.Def def) : base(master, def)
		{
			base.Subscribe(-905833192, new Action<object>(this.OnCopySettings));
			this.captureCell = Grid.PosToCell(base.transform.GetPosition());
			this.critterCapactiy = base.GetComponent<BaggableCritterCapacityTracker>();
			this.operationComp = base.GetComponent<Operational>();
			this.logicPorts = base.GetComponent<LogicPorts>();
			if (this.logicPorts != null)
			{
				base.Subscribe(-801688580, new Action<object>(this.OnLogicEvent));
				this.operationComp.SetFlag(FixedCapturePoint.enabledFlag, !this.logicPorts.IsPortConnected("CritterPickUpInput") || this.logicPorts.GetInputValue("CritterPickUpInput") > 0);
				return;
			}
			this.operationComp.SetFlag(FixedCapturePoint.enabledFlag, true);
		}

		// Token: 0x060090F7 RID: 37111 RVA: 0x0036FF38 File Offset: 0x0036E138
		private void OnLogicEvent(object data)
		{
			LogicValueChanged logicValueChanged = (LogicValueChanged)data;
			if (logicValueChanged.portID == "CritterPickUpInput" && this.logicPorts.IsPortConnected("CritterPickUpInput"))
			{
				this.operationComp.SetFlag(FixedCapturePoint.enabledFlag, logicValueChanged.newValue > 0);
			}
		}

		// Token: 0x060090F8 RID: 37112 RVA: 0x0036FF93 File Offset: 0x0036E193
		public override void StartSM()
		{
			base.StartSM();
			if (base.GetComponent<FixedCapturePoint.AutoWrangleCapture>() == null)
			{
				base.sm.automated.Set(true, this, false);
			}
		}

		// Token: 0x060090F9 RID: 37113 RVA: 0x0036FFC0 File Offset: 0x0036E1C0
		private void OnCopySettings(object data)
		{
			GameObject gameObject = (GameObject)data;
			if (gameObject == null)
			{
				return;
			}
			FixedCapturePoint.Instance smi = gameObject.GetSMI<FixedCapturePoint.Instance>();
			if (smi == null)
			{
				return;
			}
			base.sm.automated.Set(base.sm.automated.Get(smi), this, false);
		}

		// Token: 0x060090FA RID: 37114 RVA: 0x0037000D File Offset: 0x0036E20D
		public bool GetAutomated()
		{
			return base.sm.automated.Get(this);
		}

		// Token: 0x060090FB RID: 37115 RVA: 0x00370020 File Offset: 0x0036E220
		public void SetAutomated(bool automate)
		{
			base.sm.automated.Set(automate, this, false);
		}

		// Token: 0x060090FC RID: 37116 RVA: 0x00370036 File Offset: 0x0036E236
		public Chore CreateChore()
		{
			this.FindFixedCapturable();
			return new FixedCaptureChore(base.GetComponent<KPrefabID>());
		}

		// Token: 0x060090FD RID: 37117 RVA: 0x0037004C File Offset: 0x0036E24C
		public bool IsCreatureAvailableForFixedCapture()
		{
			if (!this.targetCapturable.IsNullOrStopped())
			{
				CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(this.captureCell);
				return FixedCapturePoint.Instance.CanCapturableBeCapturedAtCapturePoint(this.targetCapturable, this, cavityForCell, this.captureCell);
			}
			return false;
		}

		// Token: 0x060090FE RID: 37118 RVA: 0x00370091 File Offset: 0x0036E291
		public void SetRancherIsAvailableForCapturing()
		{
			this.shouldCreatureGoGetCaptured = true;
		}

		// Token: 0x060090FF RID: 37119 RVA: 0x0037009A File Offset: 0x0036E29A
		public void ClearRancherIsAvailableForCapturing()
		{
			this.shouldCreatureGoGetCaptured = false;
		}

		// Token: 0x06009100 RID: 37120 RVA: 0x003700A4 File Offset: 0x0036E2A4
		private static bool CanCapturableBeCapturedAtCapturePoint(FixedCapturableMonitor.Instance capturable, FixedCapturePoint.Instance capture_point, CavityInfo capture_cavity_info, int capture_cell)
		{
			if (!capturable.IsRunning())
			{
				return false;
			}
			if (capturable.targetCapturePoint != capture_point && !capturable.targetCapturePoint.IsNullOrStopped())
			{
				return false;
			}
			int cell = Grid.PosToCell(capturable.transform.GetPosition());
			CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(cell);
			return cavityForCell != null && cavityForCell == capture_cavity_info && !capturable.HasTag(GameTags.Creatures.Bagged) && (!capturable.isBaby || capture_point.def.allowBabies) && capturable.ChoreConsumer.IsChoreEqualOrAboveCurrentChorePriority<FixedCaptureStates>() && capturable.Navigator.GetNavigationCost(capture_cell) != -1 && capture_point.def.isAmountStoredOverCapacity(capture_point, capturable);
		}

		// Token: 0x06009101 RID: 37121 RVA: 0x00370158 File Offset: 0x0036E358
		public void FindFixedCapturable()
		{
			int num = Grid.PosToCell(base.transform.GetPosition());
			CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(num);
			if (cavityForCell == null)
			{
				this.ResetCapturePoint();
				return;
			}
			if (!this.targetCapturable.IsNullOrStopped() && !FixedCapturePoint.Instance.CanCapturableBeCapturedAtCapturePoint(this.targetCapturable, this, cavityForCell, num))
			{
				this.ResetCapturePoint();
			}
			if (this.targetCapturable.IsNullOrStopped())
			{
				foreach (object obj in Components.FixedCapturableMonitors)
				{
					FixedCapturableMonitor.Instance instance = (FixedCapturableMonitor.Instance)obj;
					if (FixedCapturePoint.Instance.CanCapturableBeCapturedAtCapturePoint(instance, this, cavityForCell, num))
					{
						this.targetCapturable = instance;
						if (!this.targetCapturable.IsNullOrStopped())
						{
							this.targetCapturable.targetCapturePoint = this;
							break;
						}
						break;
					}
				}
			}
		}

		// Token: 0x06009102 RID: 37122 RVA: 0x00370238 File Offset: 0x0036E438
		public void ResetCapturePoint()
		{
			base.Trigger(643180843, null);
			if (!this.targetCapturable.IsNullOrStopped())
			{
				this.targetCapturable.targetCapturePoint = null;
				this.targetCapturable.Trigger(1034952693, null);
				this.targetCapturable = null;
			}
		}

		// Token: 0x04006F89 RID: 28553
		public BaggableCritterCapacityTracker critterCapactiy;

		// Token: 0x04006F8A RID: 28554
		private int captureCell;

		// Token: 0x04006F8B RID: 28555
		private Operational operationComp;

		// Token: 0x04006F8C RID: 28556
		private LogicPorts logicPorts;
	}

	// Token: 0x020014C3 RID: 5315
	public class AutoWrangleCapture : KMonoBehaviour, ICheckboxControl
	{
		// Token: 0x06009103 RID: 37123 RVA: 0x00370277 File Offset: 0x0036E477
		protected override void OnSpawn()
		{
			base.OnSpawn();
			this.fcp = this.GetSMI<FixedCapturePoint.Instance>();
		}

		// Token: 0x170009EE RID: 2542
		// (get) Token: 0x06009104 RID: 37124 RVA: 0x0037028B File Offset: 0x0036E48B
		string ICheckboxControl.CheckboxTitleKey
		{
			get
			{
				return UI.UISIDESCREENS.CAPTURE_POINT_SIDE_SCREEN.TITLE.key.String;
			}
		}

		// Token: 0x170009EF RID: 2543
		// (get) Token: 0x06009105 RID: 37125 RVA: 0x0037029C File Offset: 0x0036E49C
		string ICheckboxControl.CheckboxLabel
		{
			get
			{
				return UI.UISIDESCREENS.CAPTURE_POINT_SIDE_SCREEN.AUTOWRANGLE;
			}
		}

		// Token: 0x170009F0 RID: 2544
		// (get) Token: 0x06009106 RID: 37126 RVA: 0x003702A8 File Offset: 0x0036E4A8
		string ICheckboxControl.CheckboxTooltip
		{
			get
			{
				return UI.UISIDESCREENS.CAPTURE_POINT_SIDE_SCREEN.AUTOWRANGLE_TOOLTIP;
			}
		}

		// Token: 0x06009107 RID: 37127 RVA: 0x003702B4 File Offset: 0x0036E4B4
		bool ICheckboxControl.GetCheckboxValue()
		{
			return this.fcp.GetAutomated();
		}

		// Token: 0x06009108 RID: 37128 RVA: 0x003702C1 File Offset: 0x0036E4C1
		void ICheckboxControl.SetCheckboxValue(bool value)
		{
			this.fcp.SetAutomated(value);
		}

		// Token: 0x04006F8D RID: 28557
		private FixedCapturePoint.Instance fcp;
	}
}
