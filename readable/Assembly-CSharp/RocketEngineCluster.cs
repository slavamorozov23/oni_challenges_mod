using System;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000BA2 RID: 2978
[SerializationConfig(MemberSerialization.OptIn)]
public class RocketEngineCluster : StateMachineComponent<RocketEngineCluster.StatesInstance>
{
	// Token: 0x060058F5 RID: 22773 RVA: 0x00204684 File Offset: 0x00202884
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		if (this.mainEngine)
		{
			base.GetComponent<RocketModule>().AddModuleCondition(ProcessCondition.ProcessConditionType.RocketPrep, new RequireAttachedComponent(base.gameObject.GetComponent<AttachableBuilding>(), typeof(IFuelTank), UI.STARMAP.COMPONENT.FUEL_TANK));
			if (this.requireOxidizer)
			{
				base.GetComponent<RocketModule>().AddModuleCondition(ProcessCondition.ProcessConditionType.RocketPrep, new RequireAttachedComponent(base.gameObject.GetComponent<AttachableBuilding>(), typeof(OxidizerTank), UI.STARMAP.COMPONENT.OXIDIZER_TANK));
			}
			base.GetComponent<RocketModule>().AddModuleCondition(ProcessCondition.ProcessConditionType.RocketPrep, new ConditionRocketHeight(this));
		}
	}

	// Token: 0x060058F6 RID: 22774 RVA: 0x00204728 File Offset: 0x00202928
	private void ConfigureFlameLight()
	{
		this.flameLight = base.gameObject.AddOrGet<Light2D>();
		this.flameLight.Color = Color.white;
		this.flameLight.overlayColour = LIGHT2D.LIGHTBUG_OVERLAYCOLOR;
		this.flameLight.Range = 10f;
		this.flameLight.Angle = 0f;
		this.flameLight.Direction = LIGHT2D.LIGHTBUG_DIRECTION;
		this.flameLight.Offset = LIGHT2D.LIGHTBUG_OFFSET;
		this.flameLight.shape = global::LightShape.Circle;
		this.flameLight.drawOverlay = true;
		this.flameLight.Lux = 80000;
		this.flameLight.emitter.RemoveFromGrid();
		base.gameObject.AddOrGet<LightSymbolTracker>().targetSymbol = base.GetComponent<KBatchedAnimController>().CurrentAnim.rootSymbol;
		this.flameLight.enabled = false;
	}

	// Token: 0x060058F7 RID: 22775 RVA: 0x0020480C File Offset: 0x00202A0C
	private void UpdateFlameLight(int cell)
	{
		base.smi.master.flameLight.RefreshShapeAndPosition();
		if (Grid.IsValidCell(cell))
		{
			if (!base.smi.master.flameLight.enabled && base.smi.timeinstate > 3f)
			{
				base.smi.master.flameLight.enabled = true;
				return;
			}
		}
		else
		{
			base.smi.master.flameLight.enabled = false;
		}
	}

	// Token: 0x060058F8 RID: 22776 RVA: 0x0020488D File Offset: 0x00202A8D
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x04003BB3 RID: 15283
	public float exhaustEmitRate = 50f;

	// Token: 0x04003BB4 RID: 15284
	public float exhaustTemperature = 1500f;

	// Token: 0x04003BB5 RID: 15285
	public SpawnFXHashes explosionEffectHash;

	// Token: 0x04003BB6 RID: 15286
	public SimHashes exhaustElement = SimHashes.CarbonDioxide;

	// Token: 0x04003BB7 RID: 15287
	public Tag fuelTag;

	// Token: 0x04003BB8 RID: 15288
	public float efficiency = 1f;

	// Token: 0x04003BB9 RID: 15289
	public bool requireOxidizer = true;

	// Token: 0x04003BBA RID: 15290
	public int maxModules = 32;

	// Token: 0x04003BBB RID: 15291
	public int maxHeight;

	// Token: 0x04003BBC RID: 15292
	public bool mainEngine = true;

	// Token: 0x04003BBD RID: 15293
	public byte exhaustDiseaseIdx = byte.MaxValue;

	// Token: 0x04003BBE RID: 15294
	public int exhaustDiseaseCount;

	// Token: 0x04003BBF RID: 15295
	public bool emitRadiation;

	// Token: 0x04003BC0 RID: 15296
	[MyCmpGet]
	private RadiationEmitter radiationEmitter;

	// Token: 0x04003BC1 RID: 15297
	[MyCmpGet]
	private Generator powerGenerator;

	// Token: 0x04003BC2 RID: 15298
	[MyCmpReq]
	private KBatchedAnimController animController;

	// Token: 0x04003BC3 RID: 15299
	public Light2D flameLight;

	// Token: 0x02001D2E RID: 7470
	public class StatesInstance : GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.GameInstance
	{
		// Token: 0x0600B066 RID: 45158 RVA: 0x003DA8EF File Offset: 0x003D8AEF
		public StatesInstance(RocketEngineCluster smi) : base(smi)
		{
			if (smi.emitRadiation)
			{
				DebugUtil.Assert(smi.radiationEmitter != null, "emitRadiation enabled but no RadiationEmitter component");
				this.radiationEmissionBaseOffset = smi.radiationEmitter.emissionOffset;
			}
		}

		// Token: 0x0600B067 RID: 45159 RVA: 0x003DA928 File Offset: 0x003D8B28
		public void BeginBurn()
		{
			if (base.smi.master.emitRadiation)
			{
				base.smi.master.radiationEmitter.SetEmitting(true);
			}
			LaunchPad currentPad = base.smi.master.GetComponent<RocketModuleCluster>().CraftInterface.CurrentPad;
			if (currentPad != null)
			{
				this.pad_cell = Grid.PosToCell(currentPad.gameObject.transform.GetPosition());
				if (base.smi.master.exhaustDiseaseIdx != 255)
				{
					currentPad.GetComponent<PrimaryElement>().AddDisease(base.smi.master.exhaustDiseaseIdx, base.smi.master.exhaustDiseaseCount, "rocket exhaust");
					return;
				}
			}
			else
			{
				global::Debug.LogWarning("RocketEngineCluster missing LaunchPad for burn.");
				this.pad_cell = Grid.InvalidCell;
			}
		}

		// Token: 0x0600B068 RID: 45160 RVA: 0x003DA9FC File Offset: 0x003D8BFC
		public void DoBurn(float dt)
		{
			int num = Grid.PosToCell(base.smi.master.gameObject.transform.GetPosition() + base.smi.master.animController.Offset);
			if (Grid.AreCellsInSameWorld(num, this.pad_cell))
			{
				SimMessages.EmitMass(num, ElementLoader.GetElementIndex(base.smi.master.exhaustElement), dt * base.smi.master.exhaustEmitRate, base.smi.master.exhaustTemperature, base.smi.master.exhaustDiseaseIdx, base.smi.master.exhaustDiseaseCount, -1);
			}
			if (base.smi.master.emitRadiation)
			{
				Vector3 emissionOffset = base.smi.master.radiationEmitter.emissionOffset;
				base.smi.master.radiationEmitter.emissionOffset = base.smi.radiationEmissionBaseOffset + base.smi.master.animController.Offset;
				if (Grid.AreCellsInSameWorld(base.smi.master.radiationEmitter.GetEmissionCell(), this.pad_cell))
				{
					base.smi.master.radiationEmitter.Refresh();
				}
				else
				{
					base.smi.master.radiationEmitter.emissionOffset = emissionOffset;
					base.smi.master.radiationEmitter.SetEmitting(false);
				}
			}
			int num2 = 10;
			for (int i = 1; i < num2; i++)
			{
				int num3 = Grid.OffsetCell(num, -1, -i);
				int num4 = Grid.OffsetCell(num, 0, -i);
				int num5 = Grid.OffsetCell(num, 1, -i);
				if (Grid.AreCellsInSameWorld(num3, this.pad_cell))
				{
					if (base.smi.master.exhaustDiseaseIdx != 255)
					{
						SimMessages.ModifyDiseaseOnCell(num3, base.smi.master.exhaustDiseaseIdx, (int)((float)base.smi.master.exhaustDiseaseCount / ((float)i + 1f)));
					}
					SimMessages.ModifyEnergy(num3, base.smi.master.exhaustTemperature / (float)(i + 1), 3200f, SimMessages.EnergySourceID.Burner);
				}
				if (Grid.AreCellsInSameWorld(num4, this.pad_cell))
				{
					if (base.smi.master.exhaustDiseaseIdx != 255)
					{
						SimMessages.ModifyDiseaseOnCell(num4, base.smi.master.exhaustDiseaseIdx, (int)((float)base.smi.master.exhaustDiseaseCount / (float)i));
					}
					SimMessages.ModifyEnergy(num4, base.smi.master.exhaustTemperature / (float)i, 3200f, SimMessages.EnergySourceID.Burner);
				}
				if (Grid.AreCellsInSameWorld(num5, this.pad_cell))
				{
					if (base.smi.master.exhaustDiseaseIdx != 255)
					{
						SimMessages.ModifyDiseaseOnCell(num5, base.smi.master.exhaustDiseaseIdx, (int)((float)base.smi.master.exhaustDiseaseCount / ((float)i + 1f)));
					}
					SimMessages.ModifyEnergy(num5, base.smi.master.exhaustTemperature / (float)(i + 1), 3200f, SimMessages.EnergySourceID.Burner);
				}
			}
		}

		// Token: 0x0600B069 RID: 45161 RVA: 0x003DAD24 File Offset: 0x003D8F24
		public void EndBurn()
		{
			if (base.smi.master.emitRadiation)
			{
				base.smi.master.radiationEmitter.emissionOffset = base.smi.radiationEmissionBaseOffset;
				base.smi.master.radiationEmitter.SetEmitting(false);
			}
			this.pad_cell = Grid.InvalidCell;
		}

		// Token: 0x04008A8C RID: 35468
		public Vector3 radiationEmissionBaseOffset;

		// Token: 0x04008A8D RID: 35469
		private int pad_cell;
	}

	// Token: 0x02001D2F RID: 7471
	public class States : GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster>
	{
		// Token: 0x0600B06A RID: 45162 RVA: 0x003DAD84 File Offset: 0x003D8F84
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.initializing.load;
			this.initializing.load.ScheduleGoTo(0f, this.initializing.decide);
			this.initializing.decide.Transition(this.space, new StateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.Transition.ConditionCallback(this.IsRocketInSpace), UpdateRate.SIM_200ms).Transition(this.burning, new StateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.Transition.ConditionCallback(this.IsRocketAirborne), UpdateRate.SIM_200ms).Transition(this.idle, new StateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.Transition.ConditionCallback(this.IsRocketGrounded), UpdateRate.SIM_200ms);
			this.idle.DefaultState(this.idle.grounded).EventTransition(GameHashes.RocketLaunched, this.burning_pre, null);
			this.idle.grounded.EventTransition(GameHashes.LaunchConditionChanged, this.idle.ready, new StateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.Transition.ConditionCallback(this.IsReadyToLaunch)).QueueAnim("grounded", true, null);
			this.idle.ready.EventTransition(GameHashes.LaunchConditionChanged, this.idle.grounded, GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.Not(new StateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.Transition.ConditionCallback(this.IsReadyToLaunch))).PlayAnim("pre_ready_to_launch", KAnim.PlayMode.Once).QueueAnim("ready_to_launch", true, null).Exit(delegate(RocketEngineCluster.StatesInstance smi)
			{
				KAnimControllerBase component = smi.GetComponent<KAnimControllerBase>();
				if (component != null)
				{
					component.Play("pst_ready_to_launch", KAnim.PlayMode.Once, 1f, 0f);
				}
			});
			this.burning_pre.PlayAnim("launch_pre").OnAnimQueueComplete(this.burning);
			this.burning.EventTransition(GameHashes.RocketLanded, this.burnComplete, null).PlayAnim("launch_loop", KAnim.PlayMode.Loop).Enter(delegate(RocketEngineCluster.StatesInstance smi)
			{
				smi.BeginBurn();
			}).Update(delegate(RocketEngineCluster.StatesInstance smi, float dt)
			{
				smi.DoBurn(dt);
			}, UpdateRate.SIM_200ms, false).Exit(delegate(RocketEngineCluster.StatesInstance smi)
			{
				smi.EndBurn();
			}).TagTransition(GameTags.RocketInSpace, this.space, false);
			this.space.EventTransition(GameHashes.DoReturnRocket, this.burning, null);
			this.burnComplete.PlayAnim("launch_pst", KAnim.PlayMode.Loop).GoTo(this.idle);
		}

		// Token: 0x0600B06B RID: 45163 RVA: 0x003DAFD4 File Offset: 0x003D91D4
		private bool IsReadyToLaunch(RocketEngineCluster.StatesInstance smi)
		{
			return smi.GetComponent<RocketModuleCluster>().CraftInterface.CheckPreppedForLaunch();
		}

		// Token: 0x0600B06C RID: 45164 RVA: 0x003DAFE6 File Offset: 0x003D91E6
		public bool IsRocketAirborne(RocketEngineCluster.StatesInstance smi)
		{
			return smi.master.HasTag(GameTags.RocketNotOnGround) && !smi.master.HasTag(GameTags.RocketInSpace);
		}

		// Token: 0x0600B06D RID: 45165 RVA: 0x003DB00F File Offset: 0x003D920F
		public bool IsRocketGrounded(RocketEngineCluster.StatesInstance smi)
		{
			return smi.master.HasTag(GameTags.RocketOnGround);
		}

		// Token: 0x0600B06E RID: 45166 RVA: 0x003DB021 File Offset: 0x003D9221
		public bool IsRocketInSpace(RocketEngineCluster.StatesInstance smi)
		{
			return smi.master.HasTag(GameTags.RocketInSpace);
		}

		// Token: 0x04008A8E RID: 35470
		public RocketEngineCluster.States.InitializingStates initializing;

		// Token: 0x04008A8F RID: 35471
		public RocketEngineCluster.States.IdleStates idle;

		// Token: 0x04008A90 RID: 35472
		public GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.State burning_pre;

		// Token: 0x04008A91 RID: 35473
		public GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.State burning;

		// Token: 0x04008A92 RID: 35474
		public GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.State burnComplete;

		// Token: 0x04008A93 RID: 35475
		public GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.State space;

		// Token: 0x02002A3B RID: 10811
		public class InitializingStates : GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.State
		{
			// Token: 0x0400BAA4 RID: 47780
			public GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.State load;

			// Token: 0x0400BAA5 RID: 47781
			public GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.State decide;
		}

		// Token: 0x02002A3C RID: 10812
		public class IdleStates : GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.State
		{
			// Token: 0x0400BAA6 RID: 47782
			public GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.State grounded;

			// Token: 0x0400BAA7 RID: 47783
			public GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.State ready;
		}
	}
}
