using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000B95 RID: 2965
[SerializationConfig(MemberSerialization.OptIn)]
public class LaunchableRocketCluster : StateMachineComponent<LaunchableRocketCluster.StatesInstance>, ILaunchableRocket
{
	// Token: 0x17000666 RID: 1638
	// (get) Token: 0x06005882 RID: 22658 RVA: 0x00201F4C File Offset: 0x0020014C
	public IList<Ref<RocketModuleCluster>> parts
	{
		get
		{
			return base.GetComponent<RocketModuleCluster>().CraftInterface.ClusterModules;
		}
	}

	// Token: 0x17000667 RID: 1639
	// (get) Token: 0x06005883 RID: 22659 RVA: 0x00201F5E File Offset: 0x0020015E
	// (set) Token: 0x06005884 RID: 22660 RVA: 0x00201F66 File Offset: 0x00200166
	public bool isLanding { get; private set; }

	// Token: 0x17000668 RID: 1640
	// (get) Token: 0x06005885 RID: 22661 RVA: 0x00201F6F File Offset: 0x0020016F
	// (set) Token: 0x06005886 RID: 22662 RVA: 0x00201F77 File Offset: 0x00200177
	public float rocketSpeed { get; private set; }

	// Token: 0x17000669 RID: 1641
	// (get) Token: 0x06005887 RID: 22663 RVA: 0x00201F80 File Offset: 0x00200180
	public LaunchableRocketRegisterType registerType
	{
		get
		{
			return LaunchableRocketRegisterType.Clustercraft;
		}
	}

	// Token: 0x1700066A RID: 1642
	// (get) Token: 0x06005888 RID: 22664 RVA: 0x00201F83 File Offset: 0x00200183
	public GameObject LaunchableGameObject
	{
		get
		{
			return base.gameObject;
		}
	}

	// Token: 0x06005889 RID: 22665 RVA: 0x00201F8B File Offset: 0x0020018B
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x0600588A RID: 22666 RVA: 0x00201FA0 File Offset: 0x002001A0
	public List<GameObject> GetEngines()
	{
		List<GameObject> list = new List<GameObject>();
		foreach (Ref<RocketModuleCluster> @ref in this.parts)
		{
			if (@ref.Get().GetComponent<RocketEngineCluster>())
			{
				list.Add(@ref.Get().gameObject);
			}
		}
		return list;
	}

	// Token: 0x0600588B RID: 22667 RVA: 0x00202010 File Offset: 0x00200210
	private int GetRocketHeight()
	{
		int num = 0;
		foreach (Ref<RocketModuleCluster> @ref in this.parts)
		{
			num += @ref.Get().GetComponent<Building>().Def.HeightInCells;
		}
		return num;
	}

	// Token: 0x0600588C RID: 22668 RVA: 0x00202074 File Offset: 0x00200274
	private float InitialFlightAnimOffsetForLanding()
	{
		int num = Grid.PosToCell(base.gameObject);
		return ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[num]).maximumBounds.y - base.gameObject.transform.GetPosition().y + (float)this.GetRocketHeight() + 100f;
	}

	// Token: 0x04003B67 RID: 15207
	[Serialize]
	private int takeOffLocation;

	// Token: 0x04003B6A RID: 15210
	private GameObject soundSpeakerObject;

	// Token: 0x02001D1B RID: 7451
	public class StatesInstance : GameStateMachine<LaunchableRocketCluster.States, LaunchableRocketCluster.StatesInstance, LaunchableRocketCluster, object>.GameInstance
	{
		// Token: 0x17000C70 RID: 3184
		// (get) Token: 0x0600B004 RID: 45060 RVA: 0x003D8C70 File Offset: 0x003D6E70
		private float heightLaunchSpeedRatio
		{
			get
			{
				return Mathf.Pow((float)base.master.GetRocketHeight(), TuningData<LaunchableRocketCluster.StatesInstance.Tuning>.Get().heightSpeedPower) * TuningData<LaunchableRocketCluster.StatesInstance.Tuning>.Get().heightSpeedFactor;
			}
		}

		// Token: 0x17000C71 RID: 3185
		// (get) Token: 0x0600B005 RID: 45061 RVA: 0x003D8C98 File Offset: 0x003D6E98
		// (set) Token: 0x0600B006 RID: 45062 RVA: 0x003D8CAB File Offset: 0x003D6EAB
		public float DistanceAboveGround
		{
			get
			{
				return base.sm.distanceAboveGround.Get(this);
			}
			set
			{
				base.sm.distanceAboveGround.Set(value, this, false);
			}
		}

		// Token: 0x0600B007 RID: 45063 RVA: 0x003D8CC1 File Offset: 0x003D6EC1
		public StatesInstance(LaunchableRocketCluster master) : base(master)
		{
			this.takeoffAccelPowerInv = 1f / TuningData<LaunchableRocketCluster.StatesInstance.Tuning>.Get().takeoffAccelPower;
		}

		// Token: 0x0600B008 RID: 45064 RVA: 0x003D8CE0 File Offset: 0x003D6EE0
		public void SetMissionState(Spacecraft.MissionState state)
		{
			global::Debug.Assert(!DlcManager.FeatureClusterSpaceEnabled());
			SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(base.master.GetComponent<LaunchConditionManager>()).SetState(state);
		}

		// Token: 0x0600B009 RID: 45065 RVA: 0x003D8D0A File Offset: 0x003D6F0A
		public Spacecraft.MissionState GetMissionState()
		{
			global::Debug.Assert(!DlcManager.FeatureClusterSpaceEnabled());
			return SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(base.master.GetComponent<LaunchConditionManager>()).state;
		}

		// Token: 0x0600B00A RID: 45066 RVA: 0x003D8D33 File Offset: 0x003D6F33
		public bool IsGrounded()
		{
			return base.smi.master.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>().Status == Clustercraft.CraftStatus.Grounded;
		}

		// Token: 0x0600B00B RID: 45067 RVA: 0x003D8D58 File Offset: 0x003D6F58
		public bool IsNotSpaceBound()
		{
			Clustercraft component = base.smi.master.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>();
			return component.Status == Clustercraft.CraftStatus.Grounded || component.Status == Clustercraft.CraftStatus.Landing;
		}

		// Token: 0x0600B00C RID: 45068 RVA: 0x003D8D94 File Offset: 0x003D6F94
		public bool IsNotGroundBound()
		{
			Clustercraft component = base.smi.master.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>();
			return component.Status == Clustercraft.CraftStatus.Launching || component.Status == Clustercraft.CraftStatus.InFlight;
		}

		// Token: 0x0600B00D RID: 45069 RVA: 0x003D8DD0 File Offset: 0x003D6FD0
		public void SetupLaunch()
		{
			base.master.isLanding = false;
			base.master.rocketSpeed = 0f;
			base.sm.warmupTimeRemaining.Set(5f, this, false);
			base.sm.distanceAboveGround.Set(0f, this, false);
			if (base.master.soundSpeakerObject == null)
			{
				base.master.soundSpeakerObject = new GameObject("rocketSpeaker");
				base.master.soundSpeakerObject.transform.SetParent(base.master.gameObject.transform);
			}
			foreach (Ref<RocketModuleCluster> @ref in base.master.parts)
			{
				if (@ref != null)
				{
					base.master.takeOffLocation = Grid.PosToCell(base.master.gameObject);
					@ref.Get().Trigger(-1277991738, base.master.gameObject);
				}
			}
			CraftModuleInterface craftInterface = base.master.GetComponent<RocketModuleCluster>().CraftInterface;
			if (craftInterface != null)
			{
				craftInterface.Trigger(-1277991738, base.master.gameObject);
				WorldContainer component = craftInterface.GetComponent<WorldContainer>();
				if (component != null)
				{
					List<MinionIdentity> worldItems = Components.MinionIdentities.GetWorldItems(component.id, false);
					MinionMigrationEventArgs minionMigrationEventArgs = new MinionMigrationEventArgs
					{
						prevWorldId = component.id,
						targetWorldId = component.id
					};
					foreach (MinionIdentity minionId in worldItems)
					{
						minionMigrationEventArgs.minionId = minionId;
						Game.Instance.Trigger(586301400, minionMigrationEventArgs);
					}
				}
			}
			Game.Instance.Trigger(-1277991738, base.gameObject);
			this.constantVelocityPhase_maxSpeed = 0f;
		}

		// Token: 0x0600B00E RID: 45070 RVA: 0x003D8FD4 File Offset: 0x003D71D4
		public void LaunchLoop(float dt)
		{
			base.master.isLanding = false;
			if (this.constantVelocityPhase_maxSpeed == 0f)
			{
				float num = Mathf.Pow((Mathf.Pow(TuningData<LaunchableRocketCluster.StatesInstance.Tuning>.Get().maxAccelerationDistance, this.takeoffAccelPowerInv) * this.heightLaunchSpeedRatio - 0.033f) / this.heightLaunchSpeedRatio, TuningData<LaunchableRocketCluster.StatesInstance.Tuning>.Get().takeoffAccelPower);
				this.constantVelocityPhase_maxSpeed = (TuningData<LaunchableRocketCluster.StatesInstance.Tuning>.Get().maxAccelerationDistance - num) / 0.033f;
			}
			if (base.sm.warmupTimeRemaining.Get(this) > 0f)
			{
				base.sm.warmupTimeRemaining.Delta(-dt, this);
			}
			else if (this.DistanceAboveGround < TuningData<LaunchableRocketCluster.StatesInstance.Tuning>.Get().maxAccelerationDistance)
			{
				float num2 = Mathf.Pow(this.DistanceAboveGround, this.takeoffAccelPowerInv) * this.heightLaunchSpeedRatio;
				num2 += dt;
				this.DistanceAboveGround = Mathf.Pow(num2 / this.heightLaunchSpeedRatio, TuningData<LaunchableRocketCluster.StatesInstance.Tuning>.Get().takeoffAccelPower);
				float num3 = Mathf.Pow((num2 - 0.033f) / this.heightLaunchSpeedRatio, TuningData<LaunchableRocketCluster.StatesInstance.Tuning>.Get().takeoffAccelPower);
				base.master.rocketSpeed = (this.DistanceAboveGround - num3) / 0.033f;
			}
			else
			{
				base.master.rocketSpeed = this.constantVelocityPhase_maxSpeed;
				this.DistanceAboveGround += base.master.rocketSpeed * dt;
			}
			this.UpdateSoundSpeakerObject();
			if (this.UpdatePartsAnimPositionsAndDamage(true) == 0)
			{
				base.smi.GoTo(base.sm.not_grounded.space);
			}
		}

		// Token: 0x0600B00F RID: 45071 RVA: 0x003D9158 File Offset: 0x003D7358
		public void FinalizeLaunch()
		{
			base.master.rocketSpeed = 0f;
			this.DistanceAboveGround = base.sm.distanceToSpace.Get(base.smi);
			foreach (Ref<RocketModuleCluster> @ref in base.master.parts)
			{
				if (@ref != null && !(@ref.Get() == null))
				{
					RocketModuleCluster rocketModuleCluster = @ref.Get();
					rocketModuleCluster.GetComponent<KBatchedAnimController>().Offset = Vector3.up * this.DistanceAboveGround;
					rocketModuleCluster.GetComponent<KBatchedAnimController>().enabled = false;
					rocketModuleCluster.GetComponent<RocketModule>().MoveToSpace();
				}
			}
			base.smi.master.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>().SetCraftStatus(Clustercraft.CraftStatus.InFlight);
		}

		// Token: 0x0600B010 RID: 45072 RVA: 0x003D9238 File Offset: 0x003D7438
		public void SetupLanding()
		{
			float distanceAboveGround = base.master.InitialFlightAnimOffsetForLanding();
			this.DistanceAboveGround = distanceAboveGround;
			base.sm.warmupTimeRemaining.Set(2f, this, false);
			base.master.isLanding = true;
			base.master.rocketSpeed = 0f;
			this.constantVelocityPhase_maxSpeed = 0f;
		}

		// Token: 0x0600B011 RID: 45073 RVA: 0x003D9298 File Offset: 0x003D7498
		public void LandingLoop(float dt)
		{
			base.master.isLanding = true;
			if (this.constantVelocityPhase_maxSpeed == 0f)
			{
				float num = Mathf.Pow((Mathf.Pow(TuningData<LaunchableRocketCluster.StatesInstance.Tuning>.Get().maxAccelerationDistance, this.takeoffAccelPowerInv) * this.heightLaunchSpeedRatio - 0.033f) / this.heightLaunchSpeedRatio, TuningData<LaunchableRocketCluster.StatesInstance.Tuning>.Get().takeoffAccelPower);
				this.constantVelocityPhase_maxSpeed = (num - TuningData<LaunchableRocketCluster.StatesInstance.Tuning>.Get().maxAccelerationDistance) / 0.033f;
			}
			if (this.DistanceAboveGround > TuningData<LaunchableRocketCluster.StatesInstance.Tuning>.Get().maxAccelerationDistance)
			{
				base.master.rocketSpeed = this.constantVelocityPhase_maxSpeed;
				this.DistanceAboveGround += base.master.rocketSpeed * dt;
			}
			else if (this.DistanceAboveGround > 0.0025f)
			{
				float num2 = Mathf.Pow(this.DistanceAboveGround, this.takeoffAccelPowerInv) * this.heightLaunchSpeedRatio;
				num2 -= dt;
				this.DistanceAboveGround = Mathf.Pow(num2 / this.heightLaunchSpeedRatio, TuningData<LaunchableRocketCluster.StatesInstance.Tuning>.Get().takeoffAccelPower);
				float num3 = Mathf.Pow((num2 - 0.033f) / this.heightLaunchSpeedRatio, TuningData<LaunchableRocketCluster.StatesInstance.Tuning>.Get().takeoffAccelPower);
				base.master.rocketSpeed = (this.DistanceAboveGround - num3) / 0.033f;
			}
			else if (base.sm.warmupTimeRemaining.Get(this) > 0f)
			{
				base.sm.warmupTimeRemaining.Delta(-dt, this);
				this.DistanceAboveGround = 0f;
			}
			this.UpdateSoundSpeakerObject();
			this.UpdatePartsAnimPositionsAndDamage(true);
		}

		// Token: 0x0600B012 RID: 45074 RVA: 0x003D9418 File Offset: 0x003D7618
		public void FinalizeLanding()
		{
			base.GetComponent<KSelectable>().IsSelectable = true;
			base.master.rocketSpeed = 0f;
			this.DistanceAboveGround = 0f;
			foreach (Ref<RocketModuleCluster> @ref in base.smi.master.parts)
			{
				if (@ref != null && !(@ref.Get() == null))
				{
					@ref.Get().GetComponent<KBatchedAnimController>().Offset = Vector3.zero;
				}
			}
			base.smi.master.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>().SetCraftStatus(Clustercraft.CraftStatus.Grounded);
		}

		// Token: 0x0600B013 RID: 45075 RVA: 0x003D94D8 File Offset: 0x003D76D8
		private void UpdateSoundSpeakerObject()
		{
			if (base.master.soundSpeakerObject == null)
			{
				base.master.soundSpeakerObject = new GameObject("rocketSpeaker");
				base.master.soundSpeakerObject.transform.SetParent(base.gameObject.transform);
			}
			base.master.soundSpeakerObject.transform.SetLocalPosition(this.DistanceAboveGround * Vector3.up);
		}

		// Token: 0x0600B014 RID: 45076 RVA: 0x003D9554 File Offset: 0x003D7754
		public int UpdatePartsAnimPositionsAndDamage(bool doDamage = true)
		{
			int myWorldId = base.gameObject.GetMyWorldId();
			if (myWorldId == -1)
			{
				return 0;
			}
			LaunchPad currentPad = base.smi.master.GetComponent<RocketModuleCluster>().CraftInterface.CurrentPad;
			if (currentPad != null)
			{
				myWorldId = currentPad.GetMyWorldId();
			}
			int num = 0;
			foreach (Ref<RocketModuleCluster> @ref in base.master.parts)
			{
				if (@ref != null)
				{
					RocketModuleCluster rocketModuleCluster = @ref.Get();
					KBatchedAnimController component = rocketModuleCluster.GetComponent<KBatchedAnimController>();
					component.Offset = Vector3.up * this.DistanceAboveGround;
					Vector3 positionIncludingOffset = component.PositionIncludingOffset;
					int num2 = Grid.PosToCell(component.transform.GetPosition());
					bool flag = Grid.IsValidCell(num2);
					bool flag2 = flag && (int)Grid.WorldIdx[num2] == myWorldId;
					if (component.enabled != flag2)
					{
						component.enabled = flag2;
					}
					if (doDamage && flag)
					{
						num++;
						LaunchableRocketCluster.States.DoWorldDamage(rocketModuleCluster.gameObject, positionIncludingOffset, myWorldId);
					}
				}
			}
			return num;
		}

		// Token: 0x04008A59 RID: 35417
		private float takeoffAccelPowerInv;

		// Token: 0x04008A5A RID: 35418
		private float constantVelocityPhase_maxSpeed;

		// Token: 0x02002A36 RID: 10806
		public class Tuning : TuningData<LaunchableRocketCluster.StatesInstance.Tuning>
		{
			// Token: 0x0400BA88 RID: 47752
			public float takeoffAccelPower = 4f;

			// Token: 0x0400BA89 RID: 47753
			public float maxAccelerationDistance = 25f;

			// Token: 0x0400BA8A RID: 47754
			public float warmupTime = 5f;

			// Token: 0x0400BA8B RID: 47755
			public float heightSpeedPower = 0.5f;

			// Token: 0x0400BA8C RID: 47756
			public float heightSpeedFactor = 4f;

			// Token: 0x0400BA8D RID: 47757
			public int maxAccelHeight = 40;
		}
	}

	// Token: 0x02001D1C RID: 7452
	public class States : GameStateMachine<LaunchableRocketCluster.States, LaunchableRocketCluster.StatesInstance, LaunchableRocketCluster>
	{
		// Token: 0x0600B015 RID: 45077 RVA: 0x003D967C File Offset: 0x003D787C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.grounded;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.grounded.EventTransition(GameHashes.DoLaunchRocket, this.not_grounded.launch_setup, null).EnterTransition(this.not_grounded.launch_loop, (LaunchableRocketCluster.StatesInstance smi) => smi.IsNotGroundBound()).Enter(delegate(LaunchableRocketCluster.StatesInstance smi)
			{
				smi.FinalizeLanding();
			});
			this.not_grounded.launch_setup.Enter(delegate(LaunchableRocketCluster.StatesInstance smi)
			{
				smi.SetupLaunch();
				this.distanceToSpace.Set((float)ConditionFlightPathIsClear.PadTopEdgeDistanceToOutOfScreenEdge(smi.master.gameObject.GetComponent<RocketModuleCluster>().CraftInterface.CurrentPad.gameObject), smi, false);
				smi.GoTo(this.not_grounded.launch_loop);
			});
			this.not_grounded.launch_loop.EventTransition(GameHashes.DoReturnRocket, this.not_grounded.landing_setup, null).Enter(delegate(LaunchableRocketCluster.StatesInstance smi)
			{
				smi.UpdatePartsAnimPositionsAndDamage(false);
			}).Update(delegate(LaunchableRocketCluster.StatesInstance smi, float dt)
			{
				smi.LaunchLoop(dt);
			}, UpdateRate.SIM_EVERY_TICK, false).ParamTransition<float>(this.distanceAboveGround, this.not_grounded.launch_pst, (LaunchableRocketCluster.StatesInstance smi, float p) => p >= this.distanceToSpace.Get(smi)).TriggerOnEnter(GameHashes.StartRocketLaunch, null).Exit(delegate(LaunchableRocketCluster.StatesInstance smi)
			{
				WorldContainer myWorld = smi.gameObject.GetMyWorld();
				if (myWorld != null)
				{
					myWorld.RevealSurface();
				}
			});
			this.not_grounded.launch_pst.ScheduleGoTo(0f, this.not_grounded.space);
			this.not_grounded.space.EnterTransition(this.not_grounded.landing_setup, (LaunchableRocketCluster.StatesInstance smi) => smi.IsNotSpaceBound()).EventTransition(GameHashes.DoReturnRocket, this.not_grounded.landing_setup, null).Enter(delegate(LaunchableRocketCluster.StatesInstance smi)
			{
				smi.FinalizeLaunch();
			});
			this.not_grounded.landing_setup.Enter(delegate(LaunchableRocketCluster.StatesInstance smi)
			{
				smi.SetupLanding();
				smi.GoTo(this.not_grounded.landing_loop);
			});
			this.not_grounded.landing_loop.Enter(delegate(LaunchableRocketCluster.StatesInstance smi)
			{
				smi.UpdatePartsAnimPositionsAndDamage(false);
			}).Update(delegate(LaunchableRocketCluster.StatesInstance smi, float dt)
			{
				smi.LandingLoop(dt);
			}, UpdateRate.SIM_EVERY_TICK, false).ParamTransition<float>(this.distanceAboveGround, this.not_grounded.land, new StateMachine<LaunchableRocketCluster.States, LaunchableRocketCluster.StatesInstance, LaunchableRocketCluster, object>.Parameter<float>.Callback(this.IsFullyLanded<float>)).ParamTransition<float>(this.warmupTimeRemaining, this.not_grounded.land, new StateMachine<LaunchableRocketCluster.States, LaunchableRocketCluster.StatesInstance, LaunchableRocketCluster, object>.Parameter<float>.Callback(this.IsFullyLanded<float>));
			this.not_grounded.land.TriggerOnEnter(GameHashes.RocketTouchDown, null).Enter(delegate(LaunchableRocketCluster.StatesInstance smi)
			{
				foreach (Ref<RocketModuleCluster> @ref in smi.master.parts)
				{
					if (@ref != null && !(@ref.Get() == null))
					{
						@ref.Get().Trigger(-887025858, smi.gameObject);
					}
				}
				CraftModuleInterface craftInterface = smi.master.GetComponent<RocketModuleCluster>().CraftInterface;
				if (craftInterface != null)
				{
					craftInterface.Trigger(-887025858, smi.gameObject);
					WorldContainer component = craftInterface.GetComponent<WorldContainer>();
					if (component != null)
					{
						List<MinionIdentity> worldItems = Components.MinionIdentities.GetWorldItems(component.id, false);
						MinionMigrationEventArgs minionMigrationEventArgs = new MinionMigrationEventArgs
						{
							prevWorldId = component.id,
							targetWorldId = component.id
						};
						foreach (MinionIdentity minionId in worldItems)
						{
							minionMigrationEventArgs.minionId = minionId;
							Game.Instance.Trigger(586301400, minionMigrationEventArgs);
						}
					}
				}
				Game.Instance.Trigger(-887025858, smi.gameObject);
				if (craftInterface != null)
				{
					PassengerRocketModule passengerModule = craftInterface.GetPassengerModule();
					if (passengerModule != null)
					{
						passengerModule.RemovePassengersOnOtherWorlds();
					}
				}
				smi.GoTo(this.grounded);
			});
		}

		// Token: 0x0600B016 RID: 45078 RVA: 0x003D9957 File Offset: 0x003D7B57
		public bool IsFullyLanded<T>(LaunchableRocketCluster.StatesInstance smi, T p)
		{
			return this.distanceAboveGround.Get(smi) <= 0.0025f && this.warmupTimeRemaining.Get(smi) <= 0f;
		}

		// Token: 0x0600B017 RID: 45079 RVA: 0x003D9984 File Offset: 0x003D7B84
		public static void DoWorldDamage(GameObject part, Vector3 apparentPosition, int actualWorld)
		{
			OccupyArea component = part.GetComponent<OccupyArea>();
			component.UpdateOccupiedArea();
			foreach (CellOffset offset in component.OccupiedCellsOffsets)
			{
				int num = Grid.OffsetCell(Grid.PosToCell(apparentPosition), offset);
				if (Grid.IsValidCell(num) && Grid.WorldIdx[num] == Grid.WorldIdx[actualWorld])
				{
					if (Grid.Solid[num])
					{
						WorldDamage.Instance.ApplyDamage(num, 10000f, num, BUILDINGS.DAMAGESOURCES.ROCKET, UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.ROCKET);
					}
					else if (Grid.FakeFloor[num])
					{
						GameObject gameObject = Grid.Objects[num, 39];
						if (gameObject != null && gameObject.HasTag(GameTags.GantryExtended))
						{
							BuildingHP component2 = gameObject.GetComponent<BuildingHP>();
							if (component2 != null)
							{
								gameObject.BoxingTrigger(-794517298, new BuildingHP.DamageSourceInfo
								{
									damage = component2.MaxHitPoints,
									source = BUILDINGS.DAMAGESOURCES.ROCKET,
									popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.ROCKET
								});
							}
						}
					}
				}
			}
		}

		// Token: 0x04008A5B RID: 35419
		public StateMachine<LaunchableRocketCluster.States, LaunchableRocketCluster.StatesInstance, LaunchableRocketCluster, object>.FloatParameter warmupTimeRemaining;

		// Token: 0x04008A5C RID: 35420
		public StateMachine<LaunchableRocketCluster.States, LaunchableRocketCluster.StatesInstance, LaunchableRocketCluster, object>.FloatParameter distanceAboveGround;

		// Token: 0x04008A5D RID: 35421
		public StateMachine<LaunchableRocketCluster.States, LaunchableRocketCluster.StatesInstance, LaunchableRocketCluster, object>.FloatParameter distanceToSpace;

		// Token: 0x04008A5E RID: 35422
		public GameStateMachine<LaunchableRocketCluster.States, LaunchableRocketCluster.StatesInstance, LaunchableRocketCluster, object>.State grounded;

		// Token: 0x04008A5F RID: 35423
		public LaunchableRocketCluster.States.NotGroundedStates not_grounded;

		// Token: 0x02002A37 RID: 10807
		public class NotGroundedStates : GameStateMachine<LaunchableRocketCluster.States, LaunchableRocketCluster.StatesInstance, LaunchableRocketCluster, object>.State
		{
			// Token: 0x0400BA8E RID: 47758
			public GameStateMachine<LaunchableRocketCluster.States, LaunchableRocketCluster.StatesInstance, LaunchableRocketCluster, object>.State launch_setup;

			// Token: 0x0400BA8F RID: 47759
			public GameStateMachine<LaunchableRocketCluster.States, LaunchableRocketCluster.StatesInstance, LaunchableRocketCluster, object>.State launch_loop;

			// Token: 0x0400BA90 RID: 47760
			public GameStateMachine<LaunchableRocketCluster.States, LaunchableRocketCluster.StatesInstance, LaunchableRocketCluster, object>.State launch_pst;

			// Token: 0x0400BA91 RID: 47761
			public GameStateMachine<LaunchableRocketCluster.States, LaunchableRocketCluster.StatesInstance, LaunchableRocketCluster, object>.State space;

			// Token: 0x0400BA92 RID: 47762
			public GameStateMachine<LaunchableRocketCluster.States, LaunchableRocketCluster.StatesInstance, LaunchableRocketCluster, object>.State landing_setup;

			// Token: 0x0400BA93 RID: 47763
			public GameStateMachine<LaunchableRocketCluster.States, LaunchableRocketCluster.StatesInstance, LaunchableRocketCluster, object>.State landing_loop;

			// Token: 0x0400BA94 RID: 47764
			public GameStateMachine<LaunchableRocketCluster.States, LaunchableRocketCluster.StatesInstance, LaunchableRocketCluster, object>.State land;
		}
	}
}
