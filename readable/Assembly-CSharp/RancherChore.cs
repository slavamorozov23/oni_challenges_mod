using System;
using Klei.AI;
using STRINGS;
using TUNING;

// Token: 0x020004B5 RID: 1205
public class RancherChore : Chore<RancherChore.RancherChoreStates.Instance>
{
	// Token: 0x06001981 RID: 6529 RVA: 0x0008E88C File Offset: 0x0008CA8C
	public RancherChore(KPrefabID rancher_station) : base(Db.Get().ChoreTypes.Ranch, rancher_station, null, false, null, null, null, PriorityScreen.PriorityClass.basic, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		this.AddPrecondition(RancherChore.IsOpenForRanching, rancher_station.GetSMI<RanchStation.Instance>());
		SkillPerkMissingComplainer component = base.GetComponent<SkillPerkMissingComplainer>();
		this.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, component.requiredSkillPerk);
		this.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, Db.Get().ScheduleBlockTypes.Work);
		this.AddPrecondition(ChorePreconditions.instance.CanMoveTo, rancher_station.GetComponent<Building>());
		Operational component2 = rancher_station.GetComponent<Operational>();
		this.AddPrecondition(ChorePreconditions.instance.IsOperational, component2);
		Deconstructable component3 = rancher_station.GetComponent<Deconstructable>();
		this.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDeconstruction, component3);
		BuildingEnabledButton component4 = rancher_station.GetComponent<BuildingEnabledButton>();
		this.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDisable, component4);
		base.smi = new RancherChore.RancherChoreStates.Instance(rancher_station);
		base.SetPrioritizable(rancher_station.GetComponent<Prioritizable>());
	}

	// Token: 0x06001982 RID: 6530 RVA: 0x0008E97F File Offset: 0x0008CB7F
	public override void Begin(Chore.Precondition.Context context)
	{
		base.smi.sm.rancher.Set(context.consumerState.gameObject, base.smi, false);
		base.Begin(context);
	}

	// Token: 0x06001983 RID: 6531 RVA: 0x0008E9B0 File Offset: 0x0008CBB0
	protected override void End(string reason)
	{
		base.End(reason);
		base.smi.sm.rancher.Set(null, base.smi);
	}

	// Token: 0x04000ED7 RID: 3799
	public static Chore.Precondition IsOpenForRanching = new Chore.Precondition
	{
		id = "IsCreatureAvailableForRanching",
		description = DUPLICANTS.CHORES.PRECONDITIONS.IS_CREATURE_AVAILABLE_FOR_RANCHING,
		sortOrder = -3,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			RanchStation.Instance instance = data as RanchStation.Instance;
			return !instance.HasRancher && instance.IsCritterAvailableForRanching;
		}
	};

	// Token: 0x02001303 RID: 4867
	public class RancherChoreStates : GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance>
	{
		// Token: 0x06008A63 RID: 35427 RVA: 0x00358D4C File Offset: 0x00356F4C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.moveToRanch;
			base.Target(this.rancher);
			this.root.Exit("TriggerRanchStationNoLongerAvailable", delegate(RancherChore.RancherChoreStates.Instance smi)
			{
				smi.ranchStation.TriggerRanchStationNoLongerAvailable();
			});
			this.moveToRanch.MoveTo((RancherChore.RancherChoreStates.Instance smi) => Grid.PosToCell(smi.transform.GetPosition()), this.waitForAvailableRanchable, null, false);
			this.waitForAvailableRanchable.Enter("FindRanchable", delegate(RancherChore.RancherChoreStates.Instance smi)
			{
				smi.WaitForAvailableRanchable(0f);
			}).Update("FindRanchable", delegate(RancherChore.RancherChoreStates.Instance smi, float dt)
			{
				smi.WaitForAvailableRanchable(dt);
			}, UpdateRate.SIM_200ms, false);
			this.ranchCritter.ScheduleGoTo(0.5f, this.ranchCritter.callForCritter).EventTransition(GameHashes.CreatureAbandonedRanchStation, this.waitForAvailableRanchable, null);
			this.ranchCritter.callForCritter.ToggleAnims("anim_interacts_rancherstation_kanim", 0f).PlayAnim("calling_loop", KAnim.PlayMode.Loop).ScheduleActionNextFrame("TellCreatureRancherIsReady", delegate(RancherChore.RancherChoreStates.Instance smi)
			{
				smi.ranchStation.MessageRancherReady();
			}).Target(this.masterTarget).EventTransition(GameHashes.CreatureArrivedAtRanchStation, this.ranchCritter.working, null);
			this.ranchCritter.working.ToggleWork<RancherChore.RancherWorkable>(this.masterTarget, this.ranchCritter.pst, this.waitForAvailableRanchable, null);
			this.ranchCritter.pst.Enter(delegate(RancherChore.RancherChoreStates.Instance smi)
			{
				if (!RancherChore.RancherChoreStates.HasWipeBrowAnim(smi))
				{
					smi.GoTo(this.waitForAvailableRanchable);
				}
			}).ToggleAnims(new Func<RancherChore.RancherChoreStates.Instance, HashedString>(RancherChore.RancherChoreStates.GetRancherInteractAnim)).QueueAnim("wipe_brow", false, null).OnAnimQueueComplete(this.waitForAvailableRanchable);
		}

		// Token: 0x06008A64 RID: 35428 RVA: 0x00358F36 File Offset: 0x00357136
		private static HashedString GetRancherInteractAnim(RancherChore.RancherChoreStates.Instance smi)
		{
			return smi.ranchStation.def.RancherInteractAnim;
		}

		// Token: 0x06008A65 RID: 35429 RVA: 0x00358F48 File Offset: 0x00357148
		private static bool HasWipeBrowAnim(RancherChore.RancherChoreStates.Instance smi)
		{
			return smi.ranchStation.def.RancherWipesBrowAnim;
		}

		// Token: 0x06008A66 RID: 35430 RVA: 0x00358F5C File Offset: 0x0035715C
		public static bool TryRanchCreature(RancherChore.RancherChoreStates.Instance smi)
		{
			Debug.Assert(smi.ranchStation != null, "smi.ranchStation was null");
			RanchedStates.Instance activeRanchable = smi.ranchStation.ActiveRanchable;
			if (activeRanchable.IsNullOrStopped())
			{
				return false;
			}
			KPrefabID component = activeRanchable.GetComponent<KPrefabID>();
			smi.sm.rancher.Get(smi).Trigger(937885943, component.PrefabTag.Name);
			smi.ranchStation.RanchCreature();
			return true;
		}

		// Token: 0x040069E8 RID: 27112
		public StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.TargetParameter rancher;

		// Token: 0x040069E9 RID: 27113
		private GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State moveToRanch;

		// Token: 0x040069EA RID: 27114
		private RancherChore.RancherChoreStates.RanchState ranchCritter;

		// Token: 0x040069EB RID: 27115
		private GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State waitForAvailableRanchable;

		// Token: 0x020027C9 RID: 10185
		private class RanchState : GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State
		{
			// Token: 0x0400B068 RID: 45160
			public GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State callForCritter;

			// Token: 0x0400B069 RID: 45161
			public GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State working;

			// Token: 0x0400B06A RID: 45162
			public GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State pst;
		}

		// Token: 0x020027CA RID: 10186
		public new class Instance : GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.GameInstance
		{
			// Token: 0x0600CA01 RID: 51713 RVA: 0x0042A89A File Offset: 0x00428A9A
			public Instance(KPrefabID rancher_station) : base(rancher_station)
			{
				this.ranchStation = rancher_station.GetSMI<RanchStation.Instance>();
			}

			// Token: 0x0600CA02 RID: 51714 RVA: 0x0042A8B0 File Offset: 0x00428AB0
			public void WaitForAvailableRanchable(float dt)
			{
				this.waitTime += dt;
				GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State state = this.ranchStation.IsCritterAvailableForRanching ? base.sm.ranchCritter : null;
				if (state != null || this.waitTime >= 2f)
				{
					this.waitTime = 0f;
					this.GoTo(state);
				}
			}

			// Token: 0x0400B06B RID: 45163
			private const float WAIT_FOR_RANCHABLE_TIMEOUT = 2f;

			// Token: 0x0400B06C RID: 45164
			public RanchStation.Instance ranchStation;

			// Token: 0x0400B06D RID: 45165
			private float waitTime;
		}
	}

	// Token: 0x02001304 RID: 4868
	public class RancherWorkable : Workable
	{
		// Token: 0x06008A69 RID: 35433 RVA: 0x00358FEC File Offset: 0x003571EC
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.ranch = base.gameObject.GetSMI<RanchStation.Instance>();
			this.overrideAnims = new KAnimFile[]
			{
				Assets.GetAnim(this.ranch.def.RancherInteractAnim)
			};
			base.SetWorkTime(this.ranch.def.WorkTime);
			base.SetWorkerStatusItem(this.ranch.def.RanchingStatusItem);
			this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
			this.skillExperienceSkillGroup = Db.Get().SkillGroups.Ranching.Id;
			this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
			this.lightEfficiencyBonus = false;
		}

		// Token: 0x06008A6A RID: 35434 RVA: 0x00359097 File Offset: 0x00357297
		public override Klei.AI.Attribute GetWorkAttribute()
		{
			return Db.Get().Attributes.Ranching;
		}

		// Token: 0x06008A6B RID: 35435 RVA: 0x003590A8 File Offset: 0x003572A8
		protected override void OnStartWork(WorkerBase worker)
		{
			if (this.ranch == null)
			{
				return;
			}
			if (this.ranch.def.OnRanchWorkBegins != null)
			{
				this.ranch.def.OnRanchWorkBegins(this.ranch.ActiveRanchable, this);
			}
			this.critterAnimController = this.ranch.ActiveRanchable.AnimController;
			this.critterAnimController.Play(this.ranch.def.RanchedPreAnim, KAnim.PlayMode.Once, 1f, 0f);
			this.critterAnimController.Queue(this.ranch.def.RanchedLoopAnim, KAnim.PlayMode.Loop, 1f, 0f);
		}

		// Token: 0x06008A6C RID: 35436 RVA: 0x00359154 File Offset: 0x00357354
		protected override bool OnWorkTick(WorkerBase worker, float dt)
		{
			if (this.ranch.def.OnRanchWorkTick != null)
			{
				this.ranch.def.OnRanchWorkTick(this.ranch.ActiveRanchable.gameObject, dt, this);
			}
			return base.OnWorkTick(worker, dt);
		}

		// Token: 0x06008A6D RID: 35437 RVA: 0x003591A4 File Offset: 0x003573A4
		public override void OnPendingCompleteWork(WorkerBase work)
		{
			RancherChore.RancherChoreStates.Instance smi = base.gameObject.GetSMI<RancherChore.RancherChoreStates.Instance>();
			if (this.ranch == null || smi == null)
			{
				return;
			}
			if (RancherChore.RancherChoreStates.TryRanchCreature(smi))
			{
				this.critterAnimController.Play(this.ranch.def.RanchedPstAnim, KAnim.PlayMode.Once, 1f, 0f);
			}
		}

		// Token: 0x06008A6E RID: 35438 RVA: 0x003591F7 File Offset: 0x003573F7
		protected override void OnAbortWork(WorkerBase worker)
		{
			if (this.ranch == null || this.critterAnimController == null)
			{
				return;
			}
			this.critterAnimController.Play(this.ranch.def.RanchedAbortAnim, KAnim.PlayMode.Once, 1f, 0f);
		}

		// Token: 0x040069EC RID: 27116
		private RanchStation.Instance ranch;

		// Token: 0x040069ED RID: 27117
		private KBatchedAnimController critterAnimController;
	}
}
