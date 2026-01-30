using System;
using UnityEngine;

// Token: 0x02000B11 RID: 2833
public class RobotElectroBankDeadStates : GameStateMachine<RobotElectroBankDeadStates, RobotElectroBankDeadStates.Instance, IStateMachineTarget, RobotElectroBankDeadStates.Def>
{
	// Token: 0x06005296 RID: 21142 RVA: 0x001E085C File Offset: 0x001DEA5C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.powerdown;
		this.powerdown.DefaultState(this.powerdown.pre).ToggleStatusItem(Db.Get().RobotStatusItems.DeadBatteryFlydo, (RobotElectroBankDeadStates.Instance smi) => smi.gameObject, Db.Get().StatusItemCategories.Main).EventTransition(GameHashes.OnStorageChange, this.powerup.grounded, (RobotElectroBankDeadStates.Instance smi) => RobotElectroBankDeadStates.ElectrobankDelivered(smi)).Exit(delegate(RobotElectroBankDeadStates.Instance smi)
		{
			if (GameComps.Fallers.Has(smi.gameObject))
			{
				GameComps.Fallers.Remove(smi.gameObject);
			}
		});
		this.powerdown.pre.PlayAnim("power_down_pre").OnAnimQueueComplete(this.powerdown.fall);
		this.powerdown.fall.PlayAnim("power_down_loop", KAnim.PlayMode.Loop).Enter(delegate(RobotElectroBankDeadStates.Instance smi)
		{
			if (!GameComps.Fallers.Has(smi.gameObject))
			{
				GameComps.Fallers.Add(smi.gameObject, Vector2.zero);
			}
		}).Update(delegate(RobotElectroBankDeadStates.Instance smi, float dt)
		{
			if (!GameComps.Gravities.Has(smi.gameObject))
			{
				smi.GoTo(this.powerdown.landed);
			}
		}, UpdateRate.SIM_200ms, false).EventTransition(GameHashes.Landed, this.powerdown.landed, null);
		this.powerdown.landed.PlayAnim("power_down_pst").Enter(delegate(RobotElectroBankDeadStates.Instance smi)
		{
			smi.GetComponent<LoopingSounds>().PauseSound(GlobalAssets.GetSound("Flydo_flying_LP", false), true);
		}).OnAnimQueueComplete(this.powerdown.dead);
		this.powerdown.dead.PlayAnim("dead_battery").EventTransition(GameHashes.OnStorageChange, this.powerup.grounded, (RobotElectroBankDeadStates.Instance smi) => RobotElectroBankDeadStates.ElectrobankDelivered(smi));
		this.powerup.Exit(delegate(RobotElectroBankDeadStates.Instance smi)
		{
			smi.GetComponent<LoopingSounds>().PauseSound(GlobalAssets.GetSound("Flydo_flying_LP", false), false);
			smi.Get<Brain>().Resume("power up");
		});
		this.powerup.grounded.PlayAnim("battery_change_dead").OnAnimQueueComplete(this.powerup.takeoff);
		this.powerup.takeoff.PlayAnim("power_up").OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Robots.Behaviours.NoElectroBank, false);
	}

	// Token: 0x06005297 RID: 21143 RVA: 0x001E0AC4 File Offset: 0x001DECC4
	private static bool ElectrobankDelivered(RobotElectroBankDeadStates.Instance smi)
	{
		foreach (Storage storage in smi.gameObject.GetComponents<Storage>())
		{
			if (storage.storageID == GameTags.ChargedPortableBattery)
			{
				return storage.Has(GameTags.ChargedPortableBattery);
			}
		}
		return false;
	}

	// Token: 0x040037C9 RID: 14281
	public RobotElectroBankDeadStates.PowerDown powerdown;

	// Token: 0x040037CA RID: 14282
	public RobotElectroBankDeadStates.PowerUp powerup;

	// Token: 0x040037CB RID: 14283
	public GameStateMachine<RobotElectroBankDeadStates, RobotElectroBankDeadStates.Instance, IStateMachineTarget, RobotElectroBankDeadStates.Def>.State behaviourcomplete;

	// Token: 0x02001C4C RID: 7244
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001C4D RID: 7245
	public class PowerDown : GameStateMachine<RobotElectroBankDeadStates, RobotElectroBankDeadStates.Instance, IStateMachineTarget, RobotElectroBankDeadStates.Def>.State
	{
		// Token: 0x0400879D RID: 34717
		public GameStateMachine<RobotElectroBankDeadStates, RobotElectroBankDeadStates.Instance, IStateMachineTarget, RobotElectroBankDeadStates.Def>.State pre;

		// Token: 0x0400879E RID: 34718
		public GameStateMachine<RobotElectroBankDeadStates, RobotElectroBankDeadStates.Instance, IStateMachineTarget, RobotElectroBankDeadStates.Def>.State fall;

		// Token: 0x0400879F RID: 34719
		public GameStateMachine<RobotElectroBankDeadStates, RobotElectroBankDeadStates.Instance, IStateMachineTarget, RobotElectroBankDeadStates.Def>.State landed;

		// Token: 0x040087A0 RID: 34720
		public GameStateMachine<RobotElectroBankDeadStates, RobotElectroBankDeadStates.Instance, IStateMachineTarget, RobotElectroBankDeadStates.Def>.State dead;
	}

	// Token: 0x02001C4E RID: 7246
	public class PowerUp : GameStateMachine<RobotElectroBankDeadStates, RobotElectroBankDeadStates.Instance, IStateMachineTarget, RobotElectroBankDeadStates.Def>.State
	{
		// Token: 0x040087A1 RID: 34721
		public GameStateMachine<RobotElectroBankDeadStates, RobotElectroBankDeadStates.Instance, IStateMachineTarget, RobotElectroBankDeadStates.Def>.State grounded;

		// Token: 0x040087A2 RID: 34722
		public GameStateMachine<RobotElectroBankDeadStates, RobotElectroBankDeadStates.Instance, IStateMachineTarget, RobotElectroBankDeadStates.Def>.State takeoff;
	}

	// Token: 0x02001C4F RID: 7247
	public new class Instance : GameStateMachine<RobotElectroBankDeadStates, RobotElectroBankDeadStates.Instance, IStateMachineTarget, RobotElectroBankDeadStates.Def>.GameInstance
	{
		// Token: 0x0600AD0D RID: 44301 RVA: 0x003CE6D4 File Offset: 0x003CC8D4
		public Instance(Chore<RobotElectroBankDeadStates.Instance> chore, RobotElectroBankDeadStates.Def def) : base(chore, def)
		{
			chore.choreType.interruptPriority = Db.Get().ChoreTypes.Die.interruptPriority;
			chore.masterPriority.priority_class = PriorityScreen.PriorityClass.compulsory;
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Robots.Behaviours.NoElectroBank);
		}
	}
}
