using System;
using UnityEngine;

// Token: 0x0200074C RID: 1868
public class EggIncubatorStates : GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance>
{
	// Token: 0x06002F41 RID: 12097 RVA: 0x00110D90 File Offset: 0x0010EF90
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.empty;
		this.empty.PlayAnim("off", KAnim.PlayMode.Loop).EventTransition(GameHashes.OccupantChanged, this.egg, new StateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(EggIncubatorStates.HasEgg)).EventTransition(GameHashes.OccupantChanged, this.baby, new StateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(EggIncubatorStates.HasBaby));
		this.egg.DefaultState(this.egg.unpowered).EventTransition(GameHashes.OccupantChanged, this.empty, GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.Not(new StateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(EggIncubatorStates.HasAny))).EventTransition(GameHashes.OccupantChanged, this.baby, new StateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(EggIncubatorStates.HasBaby)).ToggleStatusItem(Db.Get().BuildingStatusItems.IncubatorProgress, (EggIncubatorStates.Instance smi) => smi.master.GetComponent<EggIncubator>());
		this.egg.lose_power.PlayAnim("no_power_pre").EventTransition(GameHashes.OperationalChanged, this.egg.incubating, new StateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(EggIncubatorStates.IsOperational)).OnAnimQueueComplete(this.egg.unpowered);
		this.egg.unpowered.PlayAnim("no_power_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.egg.incubating, new StateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(EggIncubatorStates.IsOperational));
		this.egg.incubating.PlayAnim("no_power_pst").QueueAnim("working_loop", true, null).EventTransition(GameHashes.OperationalChanged, this.egg.lose_power, GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.Not(new StateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(EggIncubatorStates.IsOperational)));
		this.baby.DefaultState(this.baby.idle).EventTransition(GameHashes.OccupantChanged, this.empty, GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.Not(new StateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(EggIncubatorStates.HasBaby)));
		this.baby.idle.PlayAnim("no_power_pre").QueueAnim("no_power_loop", true, null);
	}

	// Token: 0x06002F42 RID: 12098 RVA: 0x00110F97 File Offset: 0x0010F197
	public static bool IsOperational(EggIncubatorStates.Instance smi)
	{
		return smi.GetComponent<Operational>().IsOperational;
	}

	// Token: 0x06002F43 RID: 12099 RVA: 0x00110FA4 File Offset: 0x0010F1A4
	public static bool HasEgg(EggIncubatorStates.Instance smi)
	{
		GameObject occupant = smi.GetComponent<EggIncubator>().Occupant;
		return occupant && occupant.HasTag(GameTags.Egg);
	}

	// Token: 0x06002F44 RID: 12100 RVA: 0x00110FD4 File Offset: 0x0010F1D4
	public static bool HasBaby(EggIncubatorStates.Instance smi)
	{
		GameObject occupant = smi.GetComponent<EggIncubator>().Occupant;
		return occupant && occupant.HasTag(GameTags.Creature);
	}

	// Token: 0x06002F45 RID: 12101 RVA: 0x00111002 File Offset: 0x0010F202
	public static bool HasAny(EggIncubatorStates.Instance smi)
	{
		return smi.GetComponent<EggIncubator>().Occupant;
	}

	// Token: 0x04001C09 RID: 7177
	public StateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.BoolParameter readyToHatch;

	// Token: 0x04001C0A RID: 7178
	public GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.State empty;

	// Token: 0x04001C0B RID: 7179
	public EggIncubatorStates.EggStates egg;

	// Token: 0x04001C0C RID: 7180
	public EggIncubatorStates.BabyStates baby;

	// Token: 0x0200162B RID: 5675
	public class EggStates : GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040073FC RID: 29692
		public GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.State incubating;

		// Token: 0x040073FD RID: 29693
		public GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.State lose_power;

		// Token: 0x040073FE RID: 29694
		public GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.State unpowered;
	}

	// Token: 0x0200162C RID: 5676
	public class BabyStates : GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040073FF RID: 29695
		public GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.State idle;
	}

	// Token: 0x0200162D RID: 5677
	public new class Instance : GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600963D RID: 38461 RVA: 0x0037ED08 File Offset: 0x0037CF08
		public Instance(IStateMachineTarget master) : base(master)
		{
		}
	}
}
