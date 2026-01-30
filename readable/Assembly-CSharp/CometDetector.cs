using System;
using System.Collections.Generic;
using KSerialization;

// Token: 0x02000720 RID: 1824
public class CometDetector : GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>
{
	// Token: 0x06002D91 RID: 11665 RVA: 0x00108478 File Offset: 0x00106678
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.root.Enter(delegate(CometDetector.Instance smi)
		{
			smi.UpdateDetectionState(this.lastIsTargetDetected.Get(smi), true);
			smi.remainingSecondsToFreezeLogicSignal = 3f;
		}).Update(delegate(CometDetector.Instance smi, float deltaSeconds)
		{
			smi.remainingSecondsToFreezeLogicSignal -= deltaSeconds;
			if (smi.remainingSecondsToFreezeLogicSignal < 0f)
			{
				smi.remainingSecondsToFreezeLogicSignal = 0f;
				return;
			}
			smi.SetLogicSignal(this.lastIsTargetDetected.Get(smi));
		}, UpdateRate.SIM_200ms, false);
		this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (CometDetector.Instance smi) => smi.GetComponent<Operational>().IsOperational);
		this.on.DefaultState(this.on.pre).ToggleStatusItem(Db.Get().BuildingStatusItems.DetectorScanning, null).Enter("ToggleActive", delegate(CometDetector.Instance smi)
		{
			smi.GetComponent<Operational>().SetActive(true, false);
		}).Exit("ToggleActive", delegate(CometDetector.Instance smi)
		{
			smi.GetComponent<Operational>().SetActive(false, false);
		});
		this.on.pre.PlayAnim("on_pre").OnAnimQueueComplete(this.on.loop);
		this.on.loop.PlayAnim("on", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.on.pst, (CometDetector.Instance smi) => !smi.GetComponent<Operational>().IsOperational).TagTransition(GameTags.Detecting, this.on.working, false).Enter("UpdateLogic", delegate(CometDetector.Instance smi)
		{
			smi.UpdateDetectionState(smi.HasTag(GameTags.Detecting), false);
		}).Update("Scan Sky", delegate(CometDetector.Instance smi, float dt)
		{
			smi.ScanSky(false);
		}, UpdateRate.SIM_200ms, false);
		this.on.pst.PlayAnim("on_pst").OnAnimQueueComplete(this.off);
		this.on.working.DefaultState(this.on.working.pre).ToggleStatusItem(Db.Get().BuildingStatusItems.IncomingMeteors, null).Enter("UpdateLogic", delegate(CometDetector.Instance smi)
		{
			smi.SetLogicSignal(true);
		}).Exit("UpdateLogic", delegate(CometDetector.Instance smi)
		{
			smi.SetLogicSignal(false);
		}).Update("Scan Sky", delegate(CometDetector.Instance smi, float dt)
		{
			smi.ScanSky(true);
		}, UpdateRate.SIM_200ms, false);
		this.on.working.pre.PlayAnim("detect_pre").OnAnimQueueComplete(this.on.working.loop);
		this.on.working.loop.PlayAnim("detect_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.on.working.pst, (CometDetector.Instance smi) => !smi.GetComponent<Operational>().IsOperational).EventTransition(GameHashes.ActiveChanged, this.on.working.pst, (CometDetector.Instance smi) => !smi.GetComponent<Operational>().IsActive).TagTransition(GameTags.Detecting, this.on.working.pst, true);
		this.on.working.pst.PlayAnim("detect_pst").OnAnimQueueComplete(this.on.loop);
	}

	// Token: 0x04001B18 RID: 6936
	public GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State off;

	// Token: 0x04001B19 RID: 6937
	public CometDetector.OnStates on;

	// Token: 0x04001B1A RID: 6938
	public StateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.BoolParameter lastIsTargetDetected;

	// Token: 0x020015F7 RID: 5623
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020015F8 RID: 5624
	public class OnStates : GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State
	{
		// Token: 0x04007369 RID: 29545
		public GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State pre;

		// Token: 0x0400736A RID: 29546
		public GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State loop;

		// Token: 0x0400736B RID: 29547
		public CometDetector.WorkingStates working;

		// Token: 0x0400736C RID: 29548
		public GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State pst;
	}

	// Token: 0x020015F9 RID: 5625
	public class WorkingStates : GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State
	{
		// Token: 0x0400736D RID: 29549
		public GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State pre;

		// Token: 0x0400736E RID: 29550
		public GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State loop;

		// Token: 0x0400736F RID: 29551
		public GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State pst;
	}

	// Token: 0x020015FA RID: 5626
	public new class Instance : GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.GameInstance
	{
		// Token: 0x06009579 RID: 38265 RVA: 0x0037C3C1 File Offset: 0x0037A5C1
		public Instance(IStateMachineTarget master, CometDetector.Def def) : base(master, def)
		{
			this.detectorNetworkDef = new DetectorNetwork.Def();
			this.targetCraft = new Ref<LaunchConditionManager>();
		}

		// Token: 0x0600957A RID: 38266 RVA: 0x0037C3EC File Offset: 0x0037A5EC
		public override void StartSM()
		{
			if (this.detectorNetwork == null)
			{
				this.detectorNetwork = (DetectorNetwork.Instance)this.detectorNetworkDef.CreateSMI(base.master);
			}
			this.detectorNetwork.StartSM();
			base.StartSM();
		}

		// Token: 0x0600957B RID: 38267 RVA: 0x0037C423 File Offset: 0x0037A623
		public override void StopSM(string reason)
		{
			base.StopSM(reason);
			this.detectorNetwork.StopSM(reason);
		}

		// Token: 0x0600957C RID: 38268 RVA: 0x0037C438 File Offset: 0x0037A638
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

		// Token: 0x0600957D RID: 38269 RVA: 0x0037C474 File Offset: 0x0037A674
		public void ScanSky(bool expectedDetectionForState)
		{
			LaunchConditionManager launchConditionManager = this.targetCraft.Get();
			Option<SpaceScannerTarget> option;
			if (launchConditionManager == null)
			{
				option = SpaceScannerTarget.MeteorShower();
			}
			else if (SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this.targetCraft.Get()).state == Spacecraft.MissionState.Destroyed)
			{
				option = Option.None;
			}
			else
			{
				option = SpaceScannerTarget.RocketBaseGame(launchConditionManager);
			}
			bool flag = option.IsSome() && Game.Instance.spaceScannerNetworkManager.IsTargetDetectedOnWorld(this.GetMyWorldId(), option.Unwrap());
			base.smi.sm.lastIsTargetDetected.Set(flag, this, false);
			this.UpdateDetectionState(flag, expectedDetectionForState);
		}

		// Token: 0x0600957E RID: 38270 RVA: 0x0037C524 File Offset: 0x0037A724
		public void SetLogicSignal(bool on)
		{
			base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, on ? 1 : 0);
		}

		// Token: 0x0600957F RID: 38271 RVA: 0x0037C53D File Offset: 0x0037A73D
		public void SetTargetCraft(LaunchConditionManager target)
		{
			this.targetCraft.Set(target);
		}

		// Token: 0x06009580 RID: 38272 RVA: 0x0037C54B File Offset: 0x0037A74B
		public LaunchConditionManager GetTargetCraft()
		{
			return this.targetCraft.Get();
		}

		// Token: 0x04007370 RID: 29552
		public bool ShowWorkingStatus;

		// Token: 0x04007371 RID: 29553
		[Serialize]
		private Ref<LaunchConditionManager> targetCraft;

		// Token: 0x04007372 RID: 29554
		[NonSerialized]
		public float remainingSecondsToFreezeLogicSignal;

		// Token: 0x04007373 RID: 29555
		private DetectorNetwork.Def detectorNetworkDef;

		// Token: 0x04007374 RID: 29556
		private DetectorNetwork.Instance detectorNetwork;

		// Token: 0x04007375 RID: 29557
		private List<GameplayEventInstance> meteorShowers = new List<GameplayEventInstance>();
	}
}
