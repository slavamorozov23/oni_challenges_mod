using System;
using System.Collections.Generic;
using KSerialization;

// Token: 0x0200071E RID: 1822
public class ClusterCometDetector : GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>
{
	// Token: 0x06002D8B RID: 11659 RVA: 0x00107B5C File Offset: 0x00105D5C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.root.Enter(delegate(ClusterCometDetector.Instance smi)
		{
			smi.UpdateDetectionState(this.lastIsTargetDetected.Get(smi), true);
			smi.remainingSecondsToFreezeLogicSignal = 3f;
		}).Update(delegate(ClusterCometDetector.Instance smi, float deltaSeconds)
		{
			smi.remainingSecondsToFreezeLogicSignal -= deltaSeconds;
			if (smi.remainingSecondsToFreezeLogicSignal < 0f)
			{
				smi.remainingSecondsToFreezeLogicSignal = 0f;
				return;
			}
			smi.SetLogicSignal(this.lastIsTargetDetected.Get(smi));
		}, UpdateRate.SIM_200ms, false);
		this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (ClusterCometDetector.Instance smi) => smi.GetComponent<Operational>().IsOperational);
		this.on.DefaultState(this.on.pre).ToggleStatusItem(Db.Get().BuildingStatusItems.DetectorScanning, null).Enter("ToggleActive", delegate(ClusterCometDetector.Instance smi)
		{
			smi.GetComponent<Operational>().SetActive(true, false);
		}).Exit("ToggleActive", delegate(ClusterCometDetector.Instance smi)
		{
			smi.GetComponent<Operational>().SetActive(false, false);
		});
		this.on.pre.PlayAnim("on_pre").OnAnimQueueComplete(this.on.loop);
		this.on.loop.PlayAnim("on", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.on.pst, (ClusterCometDetector.Instance smi) => !smi.GetComponent<Operational>().IsOperational).TagTransition(GameTags.Detecting, this.on.working, false).Enter("UpdateLogic", delegate(ClusterCometDetector.Instance smi)
		{
			smi.UpdateDetectionState(smi.HasTag(GameTags.Detecting), false);
		}).Update("Scan Sky", delegate(ClusterCometDetector.Instance smi, float dt)
		{
			smi.ScanSky(false);
		}, UpdateRate.SIM_200ms, false);
		this.on.pst.PlayAnim("on_pst").OnAnimQueueComplete(this.off);
		this.on.working.DefaultState(this.on.working.pre).ToggleStatusItem(Db.Get().BuildingStatusItems.IncomingMeteors, null).Enter("UpdateLogic", delegate(ClusterCometDetector.Instance smi)
		{
			smi.SetLogicSignal(true);
		}).Exit("UpdateLogic", delegate(ClusterCometDetector.Instance smi)
		{
			smi.SetLogicSignal(false);
		}).Update("Scan Sky", delegate(ClusterCometDetector.Instance smi, float dt)
		{
			smi.ScanSky(true);
		}, UpdateRate.SIM_200ms, false);
		this.on.working.pre.PlayAnim("detect_pre").OnAnimQueueComplete(this.on.working.loop);
		this.on.working.loop.PlayAnim("detect_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.on.working.pst, (ClusterCometDetector.Instance smi) => !smi.GetComponent<Operational>().IsOperational).EventTransition(GameHashes.ActiveChanged, this.on.working.pst, (ClusterCometDetector.Instance smi) => !smi.GetComponent<Operational>().IsActive).TagTransition(GameTags.Detecting, this.on.working.pst, true);
		this.on.working.pst.PlayAnim("detect_pst").OnAnimQueueComplete(this.on.loop);
	}

	// Token: 0x04001B11 RID: 6929
	public GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State off;

	// Token: 0x04001B12 RID: 6930
	public ClusterCometDetector.OnStates on;

	// Token: 0x04001B13 RID: 6931
	public StateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.BoolParameter lastIsTargetDetected;

	// Token: 0x020015EB RID: 5611
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020015EC RID: 5612
	public class OnStates : GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State
	{
		// Token: 0x04007315 RID: 29461
		public GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State pre;

		// Token: 0x04007316 RID: 29462
		public GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State loop;

		// Token: 0x04007317 RID: 29463
		public ClusterCometDetector.WorkingStates working;

		// Token: 0x04007318 RID: 29464
		public GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State pst;
	}

	// Token: 0x020015ED RID: 5613
	public class WorkingStates : GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State
	{
		// Token: 0x04007319 RID: 29465
		public GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State pre;

		// Token: 0x0400731A RID: 29466
		public GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State loop;

		// Token: 0x0400731B RID: 29467
		public GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State pst;
	}

	// Token: 0x020015EE RID: 5614
	public new class Instance : GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.GameInstance
	{
		// Token: 0x0600950C RID: 38156 RVA: 0x0037AF6E File Offset: 0x0037916E
		public Instance(IStateMachineTarget master, ClusterCometDetector.Def def) : base(master, def)
		{
			this.detectorNetworkDef = new DetectorNetwork.Def();
		}

		// Token: 0x0600950D RID: 38157 RVA: 0x0037AF8E File Offset: 0x0037918E
		public override void StartSM()
		{
			if (this.detectorNetwork == null)
			{
				this.detectorNetwork = (DetectorNetwork.Instance)this.detectorNetworkDef.CreateSMI(base.master);
			}
			this.detectorNetwork.StartSM();
			base.StartSM();
		}

		// Token: 0x0600950E RID: 38158 RVA: 0x0037AFC5 File Offset: 0x003791C5
		public override void StopSM(string reason)
		{
			base.StopSM(reason);
			this.detectorNetwork.StopSM(reason);
		}

		// Token: 0x0600950F RID: 38159 RVA: 0x0037AFDC File Offset: 0x003791DC
		public void UpdateDetectionState(bool currentDetection, bool expectedDetectionForState)
		{
			KPrefabID component = base.GetComponent<KPrefabID>();
			if (currentDetection)
			{
				component.AddTag(GameTags.Detecting, false);
			}
			else
			{
				component.RemoveTag(GameTags.Detecting);
			}
			if (currentDetection == expectedDetectionForState)
			{
				this.SetLogicSignal(currentDetection);
			}
		}

		// Token: 0x06009510 RID: 38160 RVA: 0x0037B018 File Offset: 0x00379218
		public void ScanSky(bool expectedDetectionForState)
		{
			Option<SpaceScannerTarget> option;
			switch (this.GetDetectorState())
			{
			case ClusterCometDetector.Instance.ClusterCometDetectorState.MeteorShower:
				option = SpaceScannerTarget.MeteorShower();
				break;
			case ClusterCometDetector.Instance.ClusterCometDetectorState.BallisticObject:
				option = SpaceScannerTarget.BallisticObject();
				break;
			case ClusterCometDetector.Instance.ClusterCometDetectorState.Rocket:
				if (this.targetCraft != null && this.targetCraft.Get() != null)
				{
					option = SpaceScannerTarget.RocketDlc1(this.targetCraft.Get());
				}
				else
				{
					option = Option.None;
				}
				break;
			default:
				throw new NotImplementedException();
			}
			bool flag = option.IsSome() && Game.Instance.spaceScannerNetworkManager.IsTargetDetectedOnWorld(this.GetMyWorldId(), option.Unwrap());
			base.smi.sm.lastIsTargetDetected.Set(flag, this, false);
			this.UpdateDetectionState(flag, expectedDetectionForState);
		}

		// Token: 0x06009511 RID: 38161 RVA: 0x0037B0EB File Offset: 0x003792EB
		public void SetLogicSignal(bool on)
		{
			base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, on ? 1 : 0);
		}

		// Token: 0x06009512 RID: 38162 RVA: 0x0037B104 File Offset: 0x00379304
		public void SetDetectorState(ClusterCometDetector.Instance.ClusterCometDetectorState newState)
		{
			this.detectorState = newState;
		}

		// Token: 0x06009513 RID: 38163 RVA: 0x0037B10D File Offset: 0x0037930D
		public ClusterCometDetector.Instance.ClusterCometDetectorState GetDetectorState()
		{
			return this.detectorState;
		}

		// Token: 0x06009514 RID: 38164 RVA: 0x0037B115 File Offset: 0x00379315
		public void SetClustercraftTarget(Clustercraft target)
		{
			if (target)
			{
				this.targetCraft = new Ref<Clustercraft>(target);
				return;
			}
			this.targetCraft = null;
		}

		// Token: 0x06009515 RID: 38165 RVA: 0x0037B133 File Offset: 0x00379333
		public Clustercraft GetClustercraftTarget()
		{
			if (this.targetCraft == null)
			{
				return null;
			}
			return this.targetCraft.Get();
		}

		// Token: 0x0400731C RID: 29468
		public bool ShowWorkingStatus;

		// Token: 0x0400731D RID: 29469
		[Serialize]
		private ClusterCometDetector.Instance.ClusterCometDetectorState detectorState;

		// Token: 0x0400731E RID: 29470
		[Serialize]
		private Ref<Clustercraft> targetCraft;

		// Token: 0x0400731F RID: 29471
		[NonSerialized]
		public float remainingSecondsToFreezeLogicSignal;

		// Token: 0x04007320 RID: 29472
		private DetectorNetwork.Def detectorNetworkDef;

		// Token: 0x04007321 RID: 29473
		private DetectorNetwork.Instance detectorNetwork;

		// Token: 0x04007322 RID: 29474
		private List<GameplayEventInstance> meteorShowers = new List<GameplayEventInstance>();

		// Token: 0x020028CF RID: 10447
		public enum ClusterCometDetectorState
		{
			// Token: 0x0400B3D0 RID: 46032
			MeteorShower,
			// Token: 0x0400B3D1 RID: 46033
			BallisticObject,
			// Token: 0x0400B3D2 RID: 46034
			Rocket
		}
	}
}
