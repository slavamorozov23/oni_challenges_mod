using System;
using UnityEngine;

// Token: 0x020003E8 RID: 1000
public class ReturnToChargeStationStates : GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>
{
	// Token: 0x06001489 RID: 5257 RVA: 0x00074B08 File Offset: 0x00072D08
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.emote;
		this.emote.ToggleStatusItem(Db.Get().RobotStatusItems.MovingToChargeStation, (ReturnToChargeStationStates.Instance smi) => smi.gameObject, Db.Get().StatusItemCategories.Main).PlayAnim("react_lobatt", KAnim.PlayMode.Once).OnAnimQueueComplete(this.movingToChargingStation);
		this.idle.ToggleStatusItem(Db.Get().RobotStatusItems.MovingToChargeStation, (ReturnToChargeStationStates.Instance smi) => smi.gameObject, Db.Get().StatusItemCategories.Main).ScheduleGoTo(1f, this.movingToChargingStation);
		this.movingToChargingStation.ToggleStatusItem(Db.Get().RobotStatusItems.MovingToChargeStation, (ReturnToChargeStationStates.Instance smi) => smi.gameObject, Db.Get().StatusItemCategories.Main).MoveTo(delegate(ReturnToChargeStationStates.Instance smi)
		{
			Storage sweepLocker = this.GetSweepLocker(smi);
			if (!(sweepLocker == null))
			{
				return Grid.PosToCell(sweepLocker);
			}
			return Grid.InvalidCell;
		}, this.chargingstates.waitingForCharging, this.idle, false);
		this.chargingstates.Enter(delegate(ReturnToChargeStationStates.Instance smi)
		{
			Storage sweepLocker = this.GetSweepLocker(smi);
			if (sweepLocker != null)
			{
				smi.master.GetComponent<Facing>().Face(sweepLocker.gameObject.transform.position + Vector3.right);
				Vector3 position = smi.transform.GetPosition();
				position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingUse);
				smi.transform.SetPosition(position);
				KBatchedAnimController component = smi.GetComponent<KBatchedAnimController>();
				component.enabled = false;
				component.enabled = true;
			}
		}).Exit(delegate(ReturnToChargeStationStates.Instance smi)
		{
			Vector3 position = smi.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Creatures);
			smi.transform.SetPosition(position);
			KBatchedAnimController component = smi.GetComponent<KBatchedAnimController>();
			component.enabled = false;
			component.enabled = true;
		}).Enter(delegate(ReturnToChargeStationStates.Instance smi)
		{
			this.Station_DockRobot(smi, true);
		}).Exit(delegate(ReturnToChargeStationStates.Instance smi)
		{
			this.Station_DockRobot(smi, false);
		});
		this.chargingstates.waitingForCharging.PlayAnim("react_base", KAnim.PlayMode.Loop).TagTransition(GameTags.Robots.Behaviours.RechargeBehaviour, this.chargingstates.completed, true).Transition(this.chargingstates.charging, (ReturnToChargeStationStates.Instance smi) => smi.StationReadyToCharge(), UpdateRate.SIM_200ms);
		this.chargingstates.charging.TagTransition(GameTags.Robots.Behaviours.RechargeBehaviour, this.chargingstates.completed, true).Transition(this.chargingstates.interupted, (ReturnToChargeStationStates.Instance smi) => !smi.StationReadyToCharge(), UpdateRate.SIM_200ms).ToggleEffect("Charging").PlayAnim("sleep_pre").QueueAnim("sleep_idle", true, null).Enter(new StateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State.Callback(this.Station_StartCharging)).Exit(new StateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State.Callback(this.Station_StopCharging));
		this.chargingstates.interupted.PlayAnim("sleep_pst").TagTransition(GameTags.Robots.Behaviours.RechargeBehaviour, this.chargingstates.completed, true).OnAnimQueueComplete(this.chargingstates.waitingForCharging);
		this.chargingstates.completed.PlayAnim("sleep_pst").OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Robots.Behaviours.RechargeBehaviour, false);
	}

	// Token: 0x0600148A RID: 5258 RVA: 0x00074E00 File Offset: 0x00073000
	public Storage GetSweepLocker(ReturnToChargeStationStates.Instance smi)
	{
		StorageUnloadMonitor.Instance smi2 = smi.master.gameObject.GetSMI<StorageUnloadMonitor.Instance>();
		if (smi2 == null)
		{
			return null;
		}
		return smi2.sm.sweepLocker.Get(smi2);
	}

	// Token: 0x0600148B RID: 5259 RVA: 0x00074E34 File Offset: 0x00073034
	public void Station_StartCharging(ReturnToChargeStationStates.Instance smi)
	{
		Storage sweepLocker = this.GetSweepLocker(smi);
		if (sweepLocker != null)
		{
			sweepLocker.GetComponent<SweepBotStation>().StartCharging();
		}
	}

	// Token: 0x0600148C RID: 5260 RVA: 0x00074E60 File Offset: 0x00073060
	public void Station_StopCharging(ReturnToChargeStationStates.Instance smi)
	{
		Storage sweepLocker = this.GetSweepLocker(smi);
		if (sweepLocker != null)
		{
			sweepLocker.GetComponent<SweepBotStation>().StopCharging();
		}
	}

	// Token: 0x0600148D RID: 5261 RVA: 0x00074E8C File Offset: 0x0007308C
	public void Station_DockRobot(ReturnToChargeStationStates.Instance smi, bool dockState)
	{
		Storage sweepLocker = this.GetSweepLocker(smi);
		if (sweepLocker != null)
		{
			sweepLocker.GetComponent<SweepBotStation>().DockRobot(dockState);
		}
	}

	// Token: 0x04000C6C RID: 3180
	public GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State emote;

	// Token: 0x04000C6D RID: 3181
	public GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State idle;

	// Token: 0x04000C6E RID: 3182
	public GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State movingToChargingStation;

	// Token: 0x04000C6F RID: 3183
	public GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State behaviourcomplete;

	// Token: 0x04000C70 RID: 3184
	public ReturnToChargeStationStates.ChargingStates chargingstates;

	// Token: 0x0200125B RID: 4699
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200125C RID: 4700
	public new class Instance : GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.GameInstance
	{
		// Token: 0x060087C9 RID: 34761 RVA: 0x0034C7FB File Offset: 0x0034A9FB
		public Instance(Chore<ReturnToChargeStationStates.Instance> chore, ReturnToChargeStationStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Robots.Behaviours.RechargeBehaviour);
		}

		// Token: 0x060087CA RID: 34762 RVA: 0x0034C820 File Offset: 0x0034AA20
		public bool ChargeAborted()
		{
			return base.smi.sm.GetSweepLocker(base.smi) == null || !base.smi.sm.GetSweepLocker(base.smi).GetComponent<Operational>().IsActive;
		}

		// Token: 0x060087CB RID: 34763 RVA: 0x0034C870 File Offset: 0x0034AA70
		public bool StationReadyToCharge()
		{
			return base.smi.sm.GetSweepLocker(base.smi) != null && base.smi.sm.GetSweepLocker(base.smi).GetComponent<Operational>().IsActive;
		}
	}

	// Token: 0x0200125D RID: 4701
	public class ChargingStates : GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State
	{
		// Token: 0x0400679A RID: 26522
		public GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State waitingForCharging;

		// Token: 0x0400679B RID: 26523
		public GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State charging;

		// Token: 0x0400679C RID: 26524
		public GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State interupted;

		// Token: 0x0400679D RID: 26525
		public GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State completed;
	}
}
