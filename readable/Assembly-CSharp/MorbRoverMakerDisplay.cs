using System;

// Token: 0x02000361 RID: 865
public class MorbRoverMakerDisplay : GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>
{
	// Token: 0x06001210 RID: 4624 RVA: 0x000696E8 File Offset: 0x000678E8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.Never;
		default_state = this.off.idle;
		this.root.Target(this.monitor);
		this.off.DefaultState(this.off.idle);
		this.off.entering.PlayAnim("display_off").OnAnimQueueComplete(this.off.idle);
		this.off.idle.Target(this.masterTarget).EventTransition(GameHashes.TagsChanged, this.off.exiting, new StateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.Transition.ConditionCallback(MorbRoverMakerDisplay.ShouldBeOn)).Target(this.monitor).PlayAnim("display_off_idle", KAnim.PlayMode.Loop);
		this.off.exiting.PlayAnim("display_on").OnAnimQueueComplete(this.on);
		this.on.Target(this.masterTarget).TagTransition(GameTags.Operational, this.off.entering, true).Target(this.monitor).DefaultState(this.on.idle);
		this.on.idle.Transition(this.on.germ, new StateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.Transition.ConditionCallback(MorbRoverMakerDisplay.HasGermsAddedAndGermsAreNeeded), UpdateRate.SIM_200ms).Transition(this.on.noGerm, new StateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.Transition.ConditionCallback(MorbRoverMakerDisplay.NoGermsAddedAndGermsAreNeeded), UpdateRate.SIM_200ms).PlayAnim("display_idle", KAnim.PlayMode.Loop);
		this.on.noGerm.Transition(this.on.idle, new StateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.Transition.ConditionCallback(MorbRoverMakerDisplay.GermsNoLongerNeeded), UpdateRate.SIM_200ms).Transition(this.on.germ, new StateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.Transition.ConditionCallback(MorbRoverMakerDisplay.HasGermsAddedAndGermsAreNeeded), UpdateRate.SIM_200ms).PlayAnim("display_no_germ", KAnim.PlayMode.Loop);
		this.on.germ.Transition(this.on.idle, new StateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.Transition.ConditionCallback(MorbRoverMakerDisplay.GermsNoLongerNeeded), UpdateRate.SIM_200ms).Transition(this.on.noGerm, new StateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.Transition.ConditionCallback(MorbRoverMakerDisplay.NoGermsAddedAndGermsAreNeeded), UpdateRate.SIM_200ms).PlayAnim("display_germ", KAnim.PlayMode.Loop);
	}

	// Token: 0x06001211 RID: 4625 RVA: 0x000698FD File Offset: 0x00067AFD
	public static bool NoGermsAddedAndGermsAreNeeded(MorbRoverMakerDisplay.Instance smi)
	{
		return smi.GermsAreNeeded && !smi.HasRecentlyConsumedGerms;
	}

	// Token: 0x06001212 RID: 4626 RVA: 0x00069912 File Offset: 0x00067B12
	public static bool HasGermsAddedAndGermsAreNeeded(MorbRoverMakerDisplay.Instance smi)
	{
		return smi.GermsAreNeeded && smi.HasRecentlyConsumedGerms;
	}

	// Token: 0x06001213 RID: 4627 RVA: 0x00069924 File Offset: 0x00067B24
	public static bool ShouldBeOn(MorbRoverMakerDisplay.Instance smi)
	{
		return smi.ShouldBeOn();
	}

	// Token: 0x06001214 RID: 4628 RVA: 0x0006992C File Offset: 0x00067B2C
	public static bool GermsNoLongerNeeded(MorbRoverMakerDisplay.Instance smi)
	{
		return !smi.GermsAreNeeded;
	}

	// Token: 0x04000B5A RID: 2906
	public const string METER_TARGET_NAME = "meter_display_target";

	// Token: 0x04000B5B RID: 2907
	public const string OFF_IDLE_ANIM_NAME = "display_off_idle";

	// Token: 0x04000B5C RID: 2908
	public const string OFF_ENTERING_ANIM_NAME = "display_off";

	// Token: 0x04000B5D RID: 2909
	public const string OFF_EXITING_ANIM_NAME = "display_on";

	// Token: 0x04000B5E RID: 2910
	public const string GERM_ICON_ANIM_NAME = "display_germ";

	// Token: 0x04000B5F RID: 2911
	public const string NO_GERM_ANIM_NAME = "display_no_germ";

	// Token: 0x04000B60 RID: 2912
	public const string ON_IDLE_ANIM_NAME = "display_idle";

	// Token: 0x04000B61 RID: 2913
	public StateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.TargetParameter monitor;

	// Token: 0x04000B62 RID: 2914
	public MorbRoverMakerDisplay.OffStates off;

	// Token: 0x04000B63 RID: 2915
	public MorbRoverMakerDisplay.OnStates on;

	// Token: 0x02001245 RID: 4677
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400676B RID: 26475
		public float Timeout = 1f;
	}

	// Token: 0x02001246 RID: 4678
	public class OffStates : GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.State
	{
		// Token: 0x0400676C RID: 26476
		public GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.State entering;

		// Token: 0x0400676D RID: 26477
		public GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.State idle;

		// Token: 0x0400676E RID: 26478
		public GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.State exiting;
	}

	// Token: 0x02001247 RID: 4679
	public class OnStates : GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.State
	{
		// Token: 0x0400676F RID: 26479
		public GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.State idle;

		// Token: 0x04006770 RID: 26480
		public GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.State shake;

		// Token: 0x04006771 RID: 26481
		public GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.State noGerm;

		// Token: 0x04006772 RID: 26482
		public GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.State germ;

		// Token: 0x04006773 RID: 26483
		public GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.State checkmark;
	}

	// Token: 0x02001248 RID: 4680
	public new class Instance : GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.GameInstance
	{
		// Token: 0x17000973 RID: 2419
		// (get) Token: 0x06008790 RID: 34704 RVA: 0x0034BD85 File Offset: 0x00349F85
		public bool HasRecentlyConsumedGerms
		{
			get
			{
				return GameClock.Instance.GetTime() - this.lastTimeGermsConsumed < base.def.Timeout;
			}
		}

		// Token: 0x17000974 RID: 2420
		// (get) Token: 0x06008791 RID: 34705 RVA: 0x0034BDA5 File Offset: 0x00349FA5
		public bool GermsAreNeeded
		{
			get
			{
				return this.morbRoverMaker.MorbDevelopment_Progress < 1f;
			}
		}

		// Token: 0x06008792 RID: 34706 RVA: 0x0034BDBC File Offset: 0x00349FBC
		public Instance(IStateMachineTarget master, MorbRoverMakerDisplay.Def def) : base(master, def)
		{
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			this.meter = new MeterController(component, "meter_display_target", "display_off_idle", Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingFront, Array.Empty<string>());
			base.sm.monitor.Set(this.meter.gameObject, base.smi, false);
		}

		// Token: 0x06008793 RID: 34707 RVA: 0x0034BE24 File Offset: 0x0034A024
		public override void StartSM()
		{
			this.morbRoverMaker = base.gameObject.GetSMI<MorbRoverMaker.Instance>();
			MorbRoverMaker.Instance instance = this.morbRoverMaker;
			instance.GermsAdded = (Action<long>)Delegate.Combine(instance.GermsAdded, new Action<long>(this.OnGermsAdded));
			MorbRoverMaker.Instance instance2 = this.morbRoverMaker;
			instance2.OnUncovered = (System.Action)Delegate.Combine(instance2.OnUncovered, new System.Action(this.OnUncovered));
			base.StartSM();
		}

		// Token: 0x06008794 RID: 34708 RVA: 0x0034BE96 File Offset: 0x0034A096
		private void OnGermsAdded(long amount)
		{
			this.lastTimeGermsConsumed = GameClock.Instance.GetTime();
		}

		// Token: 0x06008795 RID: 34709 RVA: 0x0034BEA8 File Offset: 0x0034A0A8
		public bool ShouldBeOn()
		{
			return this.morbRoverMaker.HasBeenRevealed && this.operational.IsOperational;
		}

		// Token: 0x06008796 RID: 34710 RVA: 0x0034BEC4 File Offset: 0x0034A0C4
		private void OnUncovered()
		{
			if (base.IsInsideState(base.sm.off.idle))
			{
				this.GoTo(base.sm.off.exiting);
			}
		}

		// Token: 0x04006774 RID: 26484
		private float lastTimeGermsConsumed = -1f;

		// Token: 0x04006775 RID: 26485
		[MyCmpReq]
		private Operational operational;

		// Token: 0x04006776 RID: 26486
		private MorbRoverMaker.Instance morbRoverMaker;

		// Token: 0x04006777 RID: 26487
		private MeterController meter;
	}
}
