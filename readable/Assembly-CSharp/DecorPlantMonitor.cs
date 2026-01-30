using System;

// Token: 0x02000896 RID: 2198
public class DecorPlantMonitor : GameStateMachine<DecorPlantMonitor, DecorPlantMonitor.Instance, IStateMachineTarget, DecorPlantMonitor.Def>
{
	// Token: 0x06003C81 RID: 15489 RVA: 0x00152634 File Offset: 0x00150834
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.wildPlanted;
		this.wildPlanted.EventTransition(GameHashes.ReceptacleMonitorChange, this.domestic, new StateMachine<DecorPlantMonitor, DecorPlantMonitor.Instance, IStateMachineTarget, DecorPlantMonitor.Def>.Transition.ConditionCallback(DecorPlantMonitor.IsDomestic));
		this.domestic.EventTransition(GameHashes.ReceptacleMonitorChange, this.wildPlanted, GameStateMachine<DecorPlantMonitor, DecorPlantMonitor.Instance, IStateMachineTarget, DecorPlantMonitor.Def>.Not(new StateMachine<DecorPlantMonitor, DecorPlantMonitor.Instance, IStateMachineTarget, DecorPlantMonitor.Def>.Transition.ConditionCallback(DecorPlantMonitor.IsDomestic))).DefaultState(this.domestic.wilted);
		this.domestic.wilted.EventTransition(GameHashes.WiltRecover, this.domestic.healthy, GameStateMachine<DecorPlantMonitor, DecorPlantMonitor.Instance, IStateMachineTarget, DecorPlantMonitor.Def>.Not(new StateMachine<DecorPlantMonitor, DecorPlantMonitor.Instance, IStateMachineTarget, DecorPlantMonitor.Def>.Transition.ConditionCallback(DecorPlantMonitor.IsWilted))).Enter(new StateMachine<DecorPlantMonitor, DecorPlantMonitor.Instance, IStateMachineTarget, DecorPlantMonitor.Def>.State.Callback(DecorPlantMonitor.TriggerRoomRefresh));
		this.domestic.healthy.EventTransition(GameHashes.Wilt, this.domestic.wilted, new StateMachine<DecorPlantMonitor, DecorPlantMonitor.Instance, IStateMachineTarget, DecorPlantMonitor.Def>.Transition.ConditionCallback(DecorPlantMonitor.IsWilted)).ToggleTag(GameTags.Decoration).Enter(new StateMachine<DecorPlantMonitor, DecorPlantMonitor.Instance, IStateMachineTarget, DecorPlantMonitor.Def>.State.Callback(DecorPlantMonitor.TriggerRoomRefresh));
	}

	// Token: 0x06003C82 RID: 15490 RVA: 0x00152736 File Offset: 0x00150936
	public static bool IsDomestic(DecorPlantMonitor.Instance smi)
	{
		return smi.receptacleMonitor != null && smi.receptacleMonitor.ReceptacleObject != null;
	}

	// Token: 0x06003C83 RID: 15491 RVA: 0x00152753 File Offset: 0x00150953
	public static bool IsWilted(DecorPlantMonitor.Instance smi)
	{
		return smi.IsWilted;
	}

	// Token: 0x06003C84 RID: 15492 RVA: 0x0015275C File Offset: 0x0015095C
	public static void TriggerRoomRefresh(DecorPlantMonitor.Instance smi)
	{
		int cell = Grid.PosToCell(smi);
		Game.Instance.roomProber.TriggerBuildingChangedEvent(cell, smi.gameObject);
	}

	// Token: 0x0400254D RID: 9549
	public GameStateMachine<DecorPlantMonitor, DecorPlantMonitor.Instance, IStateMachineTarget, DecorPlantMonitor.Def>.State wildPlanted;

	// Token: 0x0400254E RID: 9550
	public DecorPlantMonitor.DomesticStates domestic;

	// Token: 0x02001872 RID: 6258
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001873 RID: 6259
	public class DomesticStates : GameStateMachine<DecorPlantMonitor, DecorPlantMonitor.Instance, IStateMachineTarget, DecorPlantMonitor.Def>.State
	{
		// Token: 0x04007AEF RID: 31471
		public GameStateMachine<DecorPlantMonitor, DecorPlantMonitor.Instance, IStateMachineTarget, DecorPlantMonitor.Def>.State healthy;

		// Token: 0x04007AF0 RID: 31472
		public GameStateMachine<DecorPlantMonitor, DecorPlantMonitor.Instance, IStateMachineTarget, DecorPlantMonitor.Def>.State wilted;
	}

	// Token: 0x02001874 RID: 6260
	public new class Instance : GameStateMachine<DecorPlantMonitor, DecorPlantMonitor.Instance, IStateMachineTarget, DecorPlantMonitor.Def>.GameInstance
	{
		// Token: 0x17000ACA RID: 2762
		// (get) Token: 0x06009EEC RID: 40684 RVA: 0x003A466D File Offset: 0x003A286D
		public bool IsWilted
		{
			get
			{
				return this.wiltCondition.IsWilting();
			}
		}

		// Token: 0x17000ACB RID: 2763
		// (get) Token: 0x06009EED RID: 40685 RVA: 0x003A467A File Offset: 0x003A287A
		public ReceptacleMonitor.StatesInstance receptacleMonitor
		{
			get
			{
				if (this._receptacleMonitor == null)
				{
					this._receptacleMonitor = base.gameObject.GetSMI<ReceptacleMonitor.StatesInstance>();
				}
				return this._receptacleMonitor;
			}
		}

		// Token: 0x06009EEE RID: 40686 RVA: 0x003A469B File Offset: 0x003A289B
		public Instance(IStateMachineTarget master, DecorPlantMonitor.Def def) : base(master, def)
		{
			this.wiltCondition = base.GetComponent<WiltCondition>();
		}

		// Token: 0x04007AF1 RID: 31473
		private WiltCondition wiltCondition;

		// Token: 0x04007AF2 RID: 31474
		private ReceptacleMonitor.StatesInstance _receptacleMonitor;
	}
}
