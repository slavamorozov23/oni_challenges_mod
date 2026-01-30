using System;
using System.Collections.Generic;
using System.Linq;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020007F2 RID: 2034
public class RocketControlStation : StateMachineComponent<RocketControlStation.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x1700037F RID: 895
	// (get) Token: 0x06003676 RID: 13942 RVA: 0x0013368F File Offset: 0x0013188F
	// (set) Token: 0x06003677 RID: 13943 RVA: 0x00133697 File Offset: 0x00131897
	public bool RestrictWhenGrounded
	{
		get
		{
			return this.m_restrictWhenGrounded;
		}
		set
		{
			this.m_restrictWhenGrounded = value;
			base.Trigger(1861523068, null);
		}
	}

	// Token: 0x06003678 RID: 13944 RVA: 0x001336AC File Offset: 0x001318AC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		Components.RocketControlStations.Add(this);
		base.Subscribe<RocketControlStation>(-801688580, RocketControlStation.OnLogicValueChangedDelegate);
		base.Subscribe<RocketControlStation>(1861523068, RocketControlStation.OnRocketRestrictionChanged);
		this.UpdateRestrictionAnimSymbol(null);
	}

	// Token: 0x06003679 RID: 13945 RVA: 0x001336FE File Offset: 0x001318FE
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.RocketControlStations.Remove(this);
	}

	// Token: 0x17000380 RID: 896
	// (get) Token: 0x0600367A RID: 13946 RVA: 0x00133714 File Offset: 0x00131914
	public bool BuildingRestrictionsActive
	{
		get
		{
			if (this.IsLogicInputConnected())
			{
				return this.m_logicUsageRestrictionState;
			}
			base.smi.sm.AquireClustercraft(base.smi, false);
			GameObject gameObject = base.smi.sm.clusterCraft.Get(base.smi);
			return this.RestrictWhenGrounded && gameObject != null && gameObject.gameObject.HasTag(GameTags.RocketOnGround);
		}
	}

	// Token: 0x0600367B RID: 13947 RVA: 0x00133786 File Offset: 0x00131986
	public bool IsLogicInputConnected()
	{
		return this.GetNetwork() != null;
	}

	// Token: 0x0600367C RID: 13948 RVA: 0x00133794 File Offset: 0x00131994
	public void OnLogicValueChanged(object data)
	{
		if (((LogicValueChanged)data).portID == RocketControlStation.PORT_ID)
		{
			LogicCircuitNetwork network = this.GetNetwork();
			int value = (network != null) ? network.OutputValue : 1;
			bool logicUsageRestrictionState = LogicCircuitNetwork.IsBitActive(0, value);
			this.m_logicUsageRestrictionState = logicUsageRestrictionState;
			base.Trigger(1861523068, null);
		}
	}

	// Token: 0x0600367D RID: 13949 RVA: 0x001337E7 File Offset: 0x001319E7
	public void OnTagsChanged(object obj)
	{
		if (((Boxed<TagChangedEventData>)obj).value.tag == GameTags.RocketOnGround)
		{
			base.Trigger(1861523068, null);
		}
	}

	// Token: 0x0600367E RID: 13950 RVA: 0x00133814 File Offset: 0x00131A14
	private LogicCircuitNetwork GetNetwork()
	{
		int portCell = base.GetComponent<LogicPorts>().GetPortCell(RocketControlStation.PORT_ID);
		return Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
	}

	// Token: 0x0600367F RID: 13951 RVA: 0x00133842 File Offset: 0x00131A42
	private void UpdateRestrictionAnimSymbol(object o = null)
	{
		base.GetComponent<KAnimControllerBase>().SetSymbolVisiblity("restriction_sign", this.BuildingRestrictionsActive);
	}

	// Token: 0x06003680 RID: 13952 RVA: 0x00133860 File Offset: 0x00131A60
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		list.Add(new Descriptor(UI.BUILDINGEFFECTS.ROCKETRESTRICTION_HEADER, UI.BUILDINGEFFECTS.TOOLTIPS.ROCKETRESTRICTION_HEADER, Descriptor.DescriptorType.Effect, false));
		string newValue = string.Join(", ", (from t in RocketControlStation.CONTROLLED_BUILDINGS
		select Strings.Get("STRINGS.BUILDINGS.PREFABS." + t.Name.ToUpper() + ".NAME").String).ToArray<string>());
		list.Add(new Descriptor(UI.BUILDINGEFFECTS.ROCKETRESTRICTION_BUILDINGS.text.Replace("{buildinglist}", newValue), UI.BUILDINGEFFECTS.TOOLTIPS.ROCKETRESTRICTION_BUILDINGS.text.Replace("{buildinglist}", newValue), Descriptor.DescriptorType.Effect, false));
		return list;
	}

	// Token: 0x04002111 RID: 8465
	public static List<Tag> CONTROLLED_BUILDINGS = new List<Tag>();

	// Token: 0x04002112 RID: 8466
	private const int UNNETWORKED_VALUE = 1;

	// Token: 0x04002113 RID: 8467
	[Serialize]
	public float TimeRemaining;

	// Token: 0x04002114 RID: 8468
	private bool m_logicUsageRestrictionState;

	// Token: 0x04002115 RID: 8469
	[Serialize]
	private bool m_restrictWhenGrounded;

	// Token: 0x04002116 RID: 8470
	public static readonly HashedString PORT_ID = "LogicUsageRestriction";

	// Token: 0x04002117 RID: 8471
	private static readonly EventSystem.IntraObjectHandler<RocketControlStation> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<RocketControlStation>(delegate(RocketControlStation component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x04002118 RID: 8472
	private static readonly EventSystem.IntraObjectHandler<RocketControlStation> OnRocketRestrictionChanged = new EventSystem.IntraObjectHandler<RocketControlStation>(delegate(RocketControlStation component, object data)
	{
		component.UpdateRestrictionAnimSymbol(data);
	});

	// Token: 0x02001766 RID: 5990
	public class States : GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation>
	{
		// Token: 0x06009B06 RID: 39686 RVA: 0x003933C4 File Offset: 0x003915C4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.ParamsOnly;
			default_state = this.unoperational;
			this.root.Enter("SetTarget", delegate(RocketControlStation.StatesInstance smi)
			{
				this.AquireClustercraft(smi, true);
			}).Target(this.masterTarget).Exit(delegate(RocketControlStation.StatesInstance smi)
			{
				this.SetRocketSpeedModifiers(smi, 0.5f, 1f);
			});
			this.unoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.operational, false);
			this.operational.Enter(delegate(RocketControlStation.StatesInstance smi)
			{
				this.SetRocketSpeedModifiers(smi, 1f, smi.pilotSpeedMult);
			}).PlayAnim("on").TagTransition(GameTags.Operational, this.unoperational, true).Transition(this.ready, new StateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.Transition.ConditionCallback(this.IsInFlight), UpdateRate.SIM_4000ms).Target(this.clusterCraft).EventTransition(GameHashes.RocketRequestLaunch, this.launch, new StateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.Transition.ConditionCallback(this.RocketReadyForLaunch)).EventTransition(GameHashes.LaunchConditionChanged, this.launch, new StateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.Transition.ConditionCallback(this.RocketReadyForLaunch)).Target(this.masterTarget).Exit(delegate(RocketControlStation.StatesInstance smi)
			{
				this.timeRemaining.Set(120f, smi, false);
			});
			this.launch.Enter(delegate(RocketControlStation.StatesInstance smi)
			{
				this.SetRocketSpeedModifiers(smi, 1f, smi.pilotSpeedMult);
			}).ToggleChore(new Func<RocketControlStation.StatesInstance, Chore>(this.CreateLaunchChore), this.operational).Transition(this.launch.fadein, new StateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.Transition.ConditionCallback(this.IsInFlight), UpdateRate.SIM_200ms).Target(this.clusterCraft).EventTransition(GameHashes.RocketRequestLaunch, this.operational, GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.Not(new StateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.Transition.ConditionCallback(this.RocketReadyForLaunch))).EventTransition(GameHashes.LaunchConditionChanged, this.operational, GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.Not(new StateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.Transition.ConditionCallback(this.RocketReadyForLaunch))).Target(this.masterTarget);
			this.launch.fadein.Enter(delegate(RocketControlStation.StatesInstance smi)
			{
				if (CameraController.Instance.cameraActiveCluster == this.clusterCraft.Get(smi).GetComponent<WorldContainer>().id)
				{
					CameraController.Instance.FadeIn(0f, 1f, null);
				}
			});
			this.running.PlayAnim("on").TagTransition(GameTags.Operational, this.unoperational, true).Transition(this.operational, GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.Not(new StateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.Transition.ConditionCallback(this.IsInFlight)), UpdateRate.SIM_200ms).ParamTransition<float>(this.timeRemaining, this.ready, (RocketControlStation.StatesInstance smi, float p) => p <= 0f).Enter(delegate(RocketControlStation.StatesInstance smi)
			{
				this.SetRocketSpeedModifiers(smi, 1f, smi.pilotSpeedMult);
			}).Update("Decrement time", new Action<RocketControlStation.StatesInstance, float>(this.DecrementTime), UpdateRate.SIM_200ms, false).Exit(delegate(RocketControlStation.StatesInstance smi)
			{
				this.timeRemaining.Set(30f, smi, false);
			});
			this.ready.TagTransition(GameTags.Operational, this.unoperational, true).DefaultState(this.ready.idle).ToggleChore(new Func<RocketControlStation.StatesInstance, Chore>(this.CreateChore), this.ready.post, this.ready).Transition(this.operational, GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.Not(new StateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.Transition.ConditionCallback(this.IsInFlight)), UpdateRate.SIM_200ms).OnSignal(this.pilotSuccessful, this.ready.post).Update("Decrement time", new Action<RocketControlStation.StatesInstance, float>(this.DecrementTime), UpdateRate.SIM_200ms, false);
			this.ready.idle.PlayAnim("on", KAnim.PlayMode.Loop).WorkableStartTransition((RocketControlStation.StatesInstance smi) => smi.master.GetComponent<RocketControlStationIdleWorkable>(), this.ready.working).ParamTransition<float>(this.timeRemaining, this.ready.warning, (RocketControlStation.StatesInstance smi, float p) => p <= 15f);
			this.ready.warning.PlayAnim("on_alert", KAnim.PlayMode.Loop).WorkableStartTransition((RocketControlStation.StatesInstance smi) => smi.master.GetComponent<RocketControlStationIdleWorkable>(), this.ready.working).ToggleMainStatusItem(Db.Get().BuildingStatusItems.PilotNeeded, null).ParamTransition<float>(this.timeRemaining, this.ready.autopilot, (RocketControlStation.StatesInstance smi, float p) => p <= 0f);
			this.ready.autopilot.PlayAnim("on_failed", KAnim.PlayMode.Loop).ToggleMainStatusItem(Db.Get().BuildingStatusItems.AutoPilotActive, null).WorkableStartTransition((RocketControlStation.StatesInstance smi) => smi.master.GetComponent<RocketControlStationIdleWorkable>(), this.ready.working).Enter(delegate(RocketControlStation.StatesInstance smi)
			{
				this.SetRocketSpeedModifiers(smi, 0.5f, smi.pilotSpeedMult);
			});
			this.ready.working.PlayAnim("working_pre").QueueAnim("working_loop", true, null).Enter(delegate(RocketControlStation.StatesInstance smi)
			{
				this.SetRocketSpeedModifiers(smi, 1f, smi.pilotSpeedMult);
			}).WorkableStopTransition((RocketControlStation.StatesInstance smi) => smi.master.GetComponent<RocketControlStationIdleWorkable>(), this.ready.idle);
			this.ready.post.PlayAnim("working_pst").OnAnimQueueComplete(this.running).Exit(delegate(RocketControlStation.StatesInstance smi)
			{
				this.timeRemaining.Set(120f, smi, false);
			});
		}

		// Token: 0x06009B07 RID: 39687 RVA: 0x003938F0 File Offset: 0x00391AF0
		public void AquireClustercraft(RocketControlStation.StatesInstance smi, bool force = false)
		{
			if (force || this.clusterCraft.IsNull(smi))
			{
				GameObject rocket = this.GetRocket(smi);
				this.clusterCraft.Set(rocket, smi, false);
				if (rocket != null)
				{
					rocket.Subscribe(-1582839653, new Action<object>(smi.master.OnTagsChanged));
				}
			}
		}

		// Token: 0x06009B08 RID: 39688 RVA: 0x0039394B File Offset: 0x00391B4B
		private void DecrementTime(RocketControlStation.StatesInstance smi, float dt)
		{
			this.timeRemaining.Delta(-dt, smi);
		}

		// Token: 0x06009B09 RID: 39689 RVA: 0x0039395C File Offset: 0x00391B5C
		private bool RocketReadyForLaunch(RocketControlStation.StatesInstance smi)
		{
			Clustercraft component = this.clusterCraft.Get(smi).GetComponent<Clustercraft>();
			return component.LaunchRequested && component.CheckReadyToLaunch();
		}

		// Token: 0x06009B0A RID: 39690 RVA: 0x0039398C File Offset: 0x00391B8C
		private GameObject GetRocket(RocketControlStation.StatesInstance smi)
		{
			WorldContainer world = ClusterManager.Instance.GetWorld(smi.GetMyWorldId());
			if (world == null)
			{
				return null;
			}
			return world.gameObject.GetComponent<Clustercraft>().gameObject;
		}

		// Token: 0x06009B0B RID: 39691 RVA: 0x003939C5 File Offset: 0x00391BC5
		private void SetRocketSpeedModifiers(RocketControlStation.StatesInstance smi, float autoPilotSpeedMultiplier, float pilotSkillMultiplier = 1f)
		{
			this.clusterCraft.Get(smi).GetComponent<Clustercraft>().AutoPilotMultiplier = autoPilotSpeedMultiplier;
			this.clusterCraft.Get(smi).GetComponent<Clustercraft>().PilotSkillMultiplier = pilotSkillMultiplier;
		}

		// Token: 0x06009B0C RID: 39692 RVA: 0x003939F8 File Offset: 0x00391BF8
		private Chore CreateChore(RocketControlStation.StatesInstance smi)
		{
			Workable component = smi.master.GetComponent<RocketControlStationIdleWorkable>();
			WorkChore<RocketControlStationIdleWorkable> workChore = new WorkChore<RocketControlStationIdleWorkable>(Db.Get().ChoreTypes.RocketControl, component, null, true, null, null, null, false, Db.Get().ScheduleBlockTypes.Work, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
			workChore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, Db.Get().SkillPerks.CanUseRocketControlStation);
			workChore.AddPrecondition(ChorePreconditions.instance.IsRocketTravelling, null);
			return workChore;
		}

		// Token: 0x06009B0D RID: 39693 RVA: 0x00393A78 File Offset: 0x00391C78
		private Chore CreateLaunchChore(RocketControlStation.StatesInstance smi)
		{
			Workable component = smi.master.GetComponent<RocketControlStationLaunchWorkable>();
			WorkChore<RocketControlStationLaunchWorkable> workChore = new WorkChore<RocketControlStationLaunchWorkable>(Db.Get().ChoreTypes.RocketControl, component, null, true, null, null, null, true, null, true, true, null, false, true, false, PriorityScreen.PriorityClass.topPriority, 5, false, true);
			workChore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, Db.Get().SkillPerks.CanUseRocketControlStation);
			return workChore;
		}

		// Token: 0x06009B0E RID: 39694 RVA: 0x00393AD6 File Offset: 0x00391CD6
		public void LaunchRocket(RocketControlStation.StatesInstance smi)
		{
			this.clusterCraft.Get(smi).GetComponent<Clustercraft>().Launch(false);
		}

		// Token: 0x06009B0F RID: 39695 RVA: 0x00393AEF File Offset: 0x00391CEF
		public bool IsInFlight(RocketControlStation.StatesInstance smi)
		{
			return this.clusterCraft.Get(smi).GetComponent<Clustercraft>().Status == Clustercraft.CraftStatus.InFlight;
		}

		// Token: 0x06009B10 RID: 39696 RVA: 0x00393B0A File Offset: 0x00391D0A
		public bool IsLaunching(RocketControlStation.StatesInstance smi)
		{
			return this.clusterCraft.Get(smi).GetComponent<Clustercraft>().Status == Clustercraft.CraftStatus.Launching;
		}

		// Token: 0x040077A4 RID: 30628
		public StateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.TargetParameter clusterCraft;

		// Token: 0x040077A5 RID: 30629
		private GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.State unoperational;

		// Token: 0x040077A6 RID: 30630
		private GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.State operational;

		// Token: 0x040077A7 RID: 30631
		private GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.State running;

		// Token: 0x040077A8 RID: 30632
		private RocketControlStation.States.ReadyStates ready;

		// Token: 0x040077A9 RID: 30633
		private RocketControlStation.States.LaunchStates launch;

		// Token: 0x040077AA RID: 30634
		public StateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.Signal pilotSuccessful;

		// Token: 0x040077AB RID: 30635
		public StateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.FloatParameter timeRemaining;

		// Token: 0x02002937 RID: 10551
		public class ReadyStates : GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.State
		{
			// Token: 0x0400B654 RID: 46676
			public GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.State idle;

			// Token: 0x0400B655 RID: 46677
			public GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.State working;

			// Token: 0x0400B656 RID: 46678
			public GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.State post;

			// Token: 0x0400B657 RID: 46679
			public GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.State warning;

			// Token: 0x0400B658 RID: 46680
			public GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.State autopilot;
		}

		// Token: 0x02002938 RID: 10552
		public class LaunchStates : GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.State
		{
			// Token: 0x0400B659 RID: 46681
			public GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.State launch;

			// Token: 0x0400B65A RID: 46682
			public GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.State fadein;
		}
	}

	// Token: 0x02001767 RID: 5991
	public class StatesInstance : GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.GameInstance
	{
		// Token: 0x06009B1D RID: 39709 RVA: 0x00393C26 File Offset: 0x00391E26
		public StatesInstance(RocketControlStation smi) : base(smi)
		{
		}

		// Token: 0x06009B1E RID: 39710 RVA: 0x00393C3A File Offset: 0x00391E3A
		public void LaunchRocket()
		{
			base.sm.LaunchRocket(this);
		}

		// Token: 0x06009B1F RID: 39711 RVA: 0x00393C48 File Offset: 0x00391E48
		public void SetPilotSpeedMult(WorkerBase pilot)
		{
			AttributeConverter pilotingSpeed = Db.Get().AttributeConverters.PilotingSpeed;
			AttributeConverterInstance converter = pilot.GetComponent<AttributeConverters>().GetConverter(pilotingSpeed.Id);
			float a = 1f + converter.Evaluate();
			this.pilotSpeedMult = Mathf.Max(a, 0.1f);
		}

		// Token: 0x040077AC RID: 30636
		public float pilotSpeedMult = 1f;
	}
}
