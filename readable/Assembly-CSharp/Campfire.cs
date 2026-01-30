using System;

// Token: 0x02000718 RID: 1816
public class Campfire : GameStateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>
{
	// Token: 0x06002D54 RID: 11604 RVA: 0x00106C9C File Offset: 0x00104E9C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.noOperational;
		this.noOperational.Enter(new StateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>.State.Callback(Campfire.DisableHeatEmission)).TagTransition(GameTags.Operational, this.operational, false).PlayAnim("off", KAnim.PlayMode.Once);
		this.operational.TagTransition(GameTags.Operational, this.noOperational, true).DefaultState(this.operational.needsFuel);
		this.operational.needsFuel.Enter(new StateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>.State.Callback(Campfire.DisableHeatEmission)).EventTransition(GameHashes.OnStorageChange, this.operational.working, new StateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>.Transition.ConditionCallback(Campfire.HasFuel)).PlayAnim("off", KAnim.PlayMode.Once);
		this.operational.working.Enter(new StateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>.State.Callback(Campfire.EnableHeatEmission)).EventTransition(GameHashes.OnStorageChange, this.operational.needsFuel, GameStateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>.Not(new StateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>.Transition.ConditionCallback(Campfire.HasFuel))).PlayAnim("on", KAnim.PlayMode.Loop).Exit(new StateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>.State.Callback(Campfire.DisableHeatEmission));
	}

	// Token: 0x06002D55 RID: 11605 RVA: 0x00106DBC File Offset: 0x00104FBC
	public static bool HasFuel(Campfire.Instance smi)
	{
		return smi.HasFuel;
	}

	// Token: 0x06002D56 RID: 11606 RVA: 0x00106DC4 File Offset: 0x00104FC4
	public static void EnableHeatEmission(Campfire.Instance smi)
	{
		smi.EnableHeatEmission();
	}

	// Token: 0x06002D57 RID: 11607 RVA: 0x00106DCC File Offset: 0x00104FCC
	public static void DisableHeatEmission(Campfire.Instance smi)
	{
		smi.DisableHeatEmission();
	}

	// Token: 0x04001AF1 RID: 6897
	public const string LIT_ANIM_NAME = "on";

	// Token: 0x04001AF2 RID: 6898
	public const string UNLIT_ANIM_NAME = "off";

	// Token: 0x04001AF3 RID: 6899
	public GameStateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>.State noOperational;

	// Token: 0x04001AF4 RID: 6900
	public Campfire.OperationalStates operational;

	// Token: 0x04001AF5 RID: 6901
	public StateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>.BoolParameter WarmAuraEnabled;

	// Token: 0x020015DD RID: 5597
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040072E7 RID: 29415
		public Tag fuelTag;

		// Token: 0x040072E8 RID: 29416
		public float initialFuelMass;
	}

	// Token: 0x020015DE RID: 5598
	public class OperationalStates : GameStateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>.State
	{
		// Token: 0x040072E9 RID: 29417
		public GameStateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>.State needsFuel;

		// Token: 0x040072EA RID: 29418
		public GameStateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>.State working;
	}

	// Token: 0x020015DF RID: 5599
	public new class Instance : GameStateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>.GameInstance
	{
		// Token: 0x17000A21 RID: 2593
		// (get) Token: 0x060094E4 RID: 38116 RVA: 0x0037A713 File Offset: 0x00378913
		public bool HasFuel
		{
			get
			{
				return this.storage.MassStored() > 0f;
			}
		}

		// Token: 0x17000A22 RID: 2594
		// (get) Token: 0x060094E5 RID: 38117 RVA: 0x0037A727 File Offset: 0x00378927
		public bool IsAuraEnabled
		{
			get
			{
				return base.sm.WarmAuraEnabled.Get(this);
			}
		}

		// Token: 0x060094E6 RID: 38118 RVA: 0x0037A73A File Offset: 0x0037893A
		public Instance(IStateMachineTarget master, Campfire.Def def) : base(master, def)
		{
		}

		// Token: 0x060094E7 RID: 38119 RVA: 0x0037A744 File Offset: 0x00378944
		public void EnableHeatEmission()
		{
			this.operational.SetActive(true, false);
			this.light.enabled = true;
			this.heater.EnableEmission = true;
			this.decorProvider.SetValues(CampfireConfig.DECOR_ON);
			this.decorProvider.Refresh();
		}

		// Token: 0x060094E8 RID: 38120 RVA: 0x0037A794 File Offset: 0x00378994
		public void DisableHeatEmission()
		{
			this.operational.SetActive(false, false);
			this.light.enabled = false;
			this.heater.EnableEmission = false;
			this.decorProvider.SetValues(CampfireConfig.DECOR_OFF);
			this.decorProvider.Refresh();
		}

		// Token: 0x040072EB RID: 29419
		[MyCmpGet]
		public Operational operational;

		// Token: 0x040072EC RID: 29420
		[MyCmpGet]
		public Storage storage;

		// Token: 0x040072ED RID: 29421
		[MyCmpGet]
		public RangeVisualizer rangeVisualizer;

		// Token: 0x040072EE RID: 29422
		[MyCmpGet]
		public Light2D light;

		// Token: 0x040072EF RID: 29423
		[MyCmpGet]
		public DirectVolumeHeater heater;

		// Token: 0x040072F0 RID: 29424
		[MyCmpGet]
		public DecorProvider decorProvider;
	}
}
