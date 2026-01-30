using System;
using System.Collections.Generic;

// Token: 0x020007C2 RID: 1986
public class MissionControl : GameStateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>
{
	// Token: 0x06003483 RID: 13443 RVA: 0x00129F90 File Offset: 0x00128190
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.Inoperational;
		this.Inoperational.EventTransition(GameHashes.OperationalChanged, this.Operational, new StateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.Transition.ConditionCallback(this.ValidateOperationalTransition)).EventTransition(GameHashes.UpdateRoom, this.Operational, new StateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.Transition.ConditionCallback(this.ValidateOperationalTransition));
		this.Operational.EventTransition(GameHashes.OperationalChanged, this.Inoperational, new StateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.Transition.ConditionCallback(this.ValidateOperationalTransition)).EventTransition(GameHashes.UpdateRoom, this.Operational.WrongRoom, GameStateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.Not(new StateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.Transition.ConditionCallback(this.IsInLabRoom))).Enter(new StateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.State.Callback(this.OnEnterOperational)).DefaultState(this.Operational.NoRockets).Update(delegate(MissionControl.Instance smi, float dt)
		{
			smi.UpdateWorkableRockets(null);
		}, UpdateRate.SIM_1000ms, false);
		this.Operational.WrongRoom.EventTransition(GameHashes.UpdateRoom, this.Operational.NoRockets, new StateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.Transition.ConditionCallback(this.IsInLabRoom));
		this.Operational.NoRockets.ToggleStatusItem(Db.Get().BuildingStatusItems.NoRocketsToMissionControlBoost, null).ParamTransition<bool>(this.WorkableRocketsAreInRange, this.Operational.HasRockets, (MissionControl.Instance smi, bool inRange) => this.WorkableRocketsAreInRange.Get(smi));
		this.Operational.HasRockets.ParamTransition<bool>(this.WorkableRocketsAreInRange, this.Operational.NoRockets, (MissionControl.Instance smi, bool inRange) => !this.WorkableRocketsAreInRange.Get(smi)).ToggleChore(new Func<MissionControl.Instance, Chore>(this.CreateChore), this.Operational);
	}

	// Token: 0x06003484 RID: 13444 RVA: 0x0012A12C File Offset: 0x0012832C
	private Chore CreateChore(MissionControl.Instance smi)
	{
		MissionControlWorkable component = smi.master.gameObject.GetComponent<MissionControlWorkable>();
		Chore result = new WorkChore<MissionControlWorkable>(Db.Get().ChoreTypes.Research, component, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		Spacecraft randomBoostableSpacecraft = smi.GetRandomBoostableSpacecraft();
		component.TargetSpacecraft = randomBoostableSpacecraft;
		return result;
	}

	// Token: 0x06003485 RID: 13445 RVA: 0x0012A17E File Offset: 0x0012837E
	private void OnEnterOperational(MissionControl.Instance smi)
	{
		smi.UpdateWorkableRockets(null);
		if (this.WorkableRocketsAreInRange.Get(smi))
		{
			smi.GoTo(this.Operational.HasRockets);
			return;
		}
		smi.GoTo(this.Operational.NoRockets);
	}

	// Token: 0x06003486 RID: 13446 RVA: 0x0012A1B8 File Offset: 0x001283B8
	private bool ValidateOperationalTransition(MissionControl.Instance smi)
	{
		Operational component = smi.GetComponent<Operational>();
		bool flag = smi.IsInsideState(smi.sm.Operational);
		return component != null && flag != component.IsOperational;
	}

	// Token: 0x06003487 RID: 13447 RVA: 0x0012A1F5 File Offset: 0x001283F5
	private bool IsInLabRoom(MissionControl.Instance smi)
	{
		return smi.roomTracker.IsInCorrectRoom();
	}

	// Token: 0x04001FBE RID: 8126
	public GameStateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.State Inoperational;

	// Token: 0x04001FBF RID: 8127
	public MissionControl.OperationalState Operational;

	// Token: 0x04001FC0 RID: 8128
	public StateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.BoolParameter WorkableRocketsAreInRange;

	// Token: 0x02001706 RID: 5894
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001707 RID: 5895
	public new class Instance : GameStateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.GameInstance
	{
		// Token: 0x060099C3 RID: 39363 RVA: 0x0038EDC6 File Offset: 0x0038CFC6
		public Instance(IStateMachineTarget master, MissionControl.Def def) : base(master, def)
		{
		}

		// Token: 0x060099C4 RID: 39364 RVA: 0x0038EDDC File Offset: 0x0038CFDC
		public void UpdateWorkableRockets(object data)
		{
			this.boostableSpacecraft.Clear();
			for (int i = 0; i < SpacecraftManager.instance.GetSpacecraft().Count; i++)
			{
				if (this.CanBeBoosted(SpacecraftManager.instance.GetSpacecraft()[i]))
				{
					bool flag = false;
					foreach (object obj in Components.MissionControlWorkables)
					{
						MissionControlWorkable missionControlWorkable = (MissionControlWorkable)obj;
						if (!(missionControlWorkable.gameObject == base.gameObject) && missionControlWorkable.TargetSpacecraft == SpacecraftManager.instance.GetSpacecraft()[i])
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						this.boostableSpacecraft.Add(SpacecraftManager.instance.GetSpacecraft()[i]);
					}
				}
			}
			base.sm.WorkableRocketsAreInRange.Set(this.boostableSpacecraft.Count > 0, base.smi, false);
		}

		// Token: 0x060099C5 RID: 39365 RVA: 0x0038EEEC File Offset: 0x0038D0EC
		public Spacecraft GetRandomBoostableSpacecraft()
		{
			return this.boostableSpacecraft.GetRandom<Spacecraft>();
		}

		// Token: 0x060099C6 RID: 39366 RVA: 0x0038EEF9 File Offset: 0x0038D0F9
		private bool CanBeBoosted(Spacecraft spacecraft)
		{
			return spacecraft.controlStationBuffTimeRemaining == 0f && spacecraft.state == Spacecraft.MissionState.Underway;
		}

		// Token: 0x060099C7 RID: 39367 RVA: 0x0038EF16 File Offset: 0x0038D116
		public void ApplyEffect(Spacecraft spacecraft)
		{
			spacecraft.controlStationBuffTimeRemaining = 600f;
		}

		// Token: 0x040076B5 RID: 30389
		private List<Spacecraft> boostableSpacecraft = new List<Spacecraft>();

		// Token: 0x040076B6 RID: 30390
		[MyCmpReq]
		public RoomTracker roomTracker;
	}

	// Token: 0x02001708 RID: 5896
	public class OperationalState : GameStateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.State
	{
		// Token: 0x040076B7 RID: 30391
		public GameStateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.State WrongRoom;

		// Token: 0x040076B8 RID: 30392
		public GameStateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.State NoRockets;

		// Token: 0x040076B9 RID: 30393
		public GameStateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.State HasRockets;
	}
}
