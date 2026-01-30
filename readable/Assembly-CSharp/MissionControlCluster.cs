using System;
using System.Collections.Generic;

// Token: 0x020007C3 RID: 1987
public class MissionControlCluster : GameStateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>
{
	// Token: 0x0600348B RID: 13451 RVA: 0x0012A22C File Offset: 0x0012842C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.Inoperational;
		this.Inoperational.EventTransition(GameHashes.OperationalChanged, this.Operational, new StateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.Transition.ConditionCallback(this.ValidateOperationalTransition)).EventTransition(GameHashes.UpdateRoom, this.Operational, new StateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.Transition.ConditionCallback(this.ValidateOperationalTransition));
		this.Operational.EventTransition(GameHashes.OperationalChanged, this.Inoperational, new StateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.Transition.ConditionCallback(this.ValidateOperationalTransition)).EventTransition(GameHashes.UpdateRoom, this.Operational.WrongRoom, GameStateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.Not(new StateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.Transition.ConditionCallback(this.IsInLabRoom))).Enter(new StateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.State.Callback(this.OnEnterOperational)).DefaultState(this.Operational.NoRockets).Update(delegate(MissionControlCluster.Instance smi, float dt)
		{
			smi.UpdateWorkableRocketsInRange(null);
		}, UpdateRate.SIM_1000ms, false);
		this.Operational.WrongRoom.EventTransition(GameHashes.UpdateRoom, this.Operational.NoRockets, new StateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.Transition.ConditionCallback(this.IsInLabRoom));
		this.Operational.NoRockets.ToggleStatusItem(Db.Get().BuildingStatusItems.NoRocketsToMissionControlClusterBoost, null).ParamTransition<bool>(this.WorkableRocketsAreInRange, this.Operational.HasRockets, (MissionControlCluster.Instance smi, bool inRange) => this.WorkableRocketsAreInRange.Get(smi));
		this.Operational.HasRockets.ParamTransition<bool>(this.WorkableRocketsAreInRange, this.Operational.NoRockets, (MissionControlCluster.Instance smi, bool inRange) => !this.WorkableRocketsAreInRange.Get(smi)).ToggleChore(new Func<MissionControlCluster.Instance, Chore>(this.CreateChore), this.Operational);
	}

	// Token: 0x0600348C RID: 13452 RVA: 0x0012A3C8 File Offset: 0x001285C8
	private Chore CreateChore(MissionControlCluster.Instance smi)
	{
		MissionControlClusterWorkable component = smi.master.gameObject.GetComponent<MissionControlClusterWorkable>();
		Chore result = new WorkChore<MissionControlClusterWorkable>(Db.Get().ChoreTypes.Research, component, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		Clustercraft randomBoostableClustercraft = smi.GetRandomBoostableClustercraft();
		component.TargetClustercraft = randomBoostableClustercraft;
		return result;
	}

	// Token: 0x0600348D RID: 13453 RVA: 0x0012A41A File Offset: 0x0012861A
	private void OnEnterOperational(MissionControlCluster.Instance smi)
	{
		smi.UpdateWorkableRocketsInRange(null);
		if (this.WorkableRocketsAreInRange.Get(smi))
		{
			smi.GoTo(this.Operational.HasRockets);
			return;
		}
		smi.GoTo(this.Operational.NoRockets);
	}

	// Token: 0x0600348E RID: 13454 RVA: 0x0012A454 File Offset: 0x00128654
	private bool ValidateOperationalTransition(MissionControlCluster.Instance smi)
	{
		Operational component = smi.GetComponent<Operational>();
		bool flag = smi.IsInsideState(smi.sm.Operational);
		return component != null && flag != component.IsOperational;
	}

	// Token: 0x0600348F RID: 13455 RVA: 0x0012A491 File Offset: 0x00128691
	private bool IsInLabRoom(MissionControlCluster.Instance smi)
	{
		return smi.roomTracker.IsInCorrectRoom();
	}

	// Token: 0x04001FC1 RID: 8129
	public GameStateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.State Inoperational;

	// Token: 0x04001FC2 RID: 8130
	public MissionControlCluster.OperationalState Operational;

	// Token: 0x04001FC3 RID: 8131
	public StateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.BoolParameter WorkableRocketsAreInRange;

	// Token: 0x0200170A RID: 5898
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200170B RID: 5899
	public new class Instance : GameStateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.GameInstance
	{
		// Token: 0x060099CD RID: 39373 RVA: 0x0038EF50 File Offset: 0x0038D150
		public Instance(IStateMachineTarget master, MissionControlCluster.Def def) : base(master, def)
		{
		}

		// Token: 0x060099CE RID: 39374 RVA: 0x0038EF6C File Offset: 0x0038D16C
		public override void StartSM()
		{
			base.StartSM();
			this.clusterUpdatedHandle = Game.Instance.Subscribe(-1298331547, new Action<object>(this.UpdateWorkableRocketsInRange));
		}

		// Token: 0x060099CF RID: 39375 RVA: 0x0038EF95 File Offset: 0x0038D195
		public override void StopSM(string reason)
		{
			base.StopSM(reason);
			Game.Instance.Unsubscribe(this.clusterUpdatedHandle);
		}

		// Token: 0x060099D0 RID: 39376 RVA: 0x0038EFB0 File Offset: 0x0038D1B0
		public void UpdateWorkableRocketsInRange(object data)
		{
			this.boostableClustercraft.Clear();
			AxialI myWorldLocation = base.gameObject.GetMyWorldLocation();
			for (int i = 0; i < Components.Clustercrafts.Count; i++)
			{
				if (ClusterGrid.Instance.IsInRange(Components.Clustercrafts[i].Location, myWorldLocation, 2) && !this.IsOwnWorld(Components.Clustercrafts[i]) && this.CanBeBoosted(Components.Clustercrafts[i]))
				{
					bool flag = false;
					foreach (object obj in Components.MissionControlClusterWorkables)
					{
						MissionControlClusterWorkable missionControlClusterWorkable = (MissionControlClusterWorkable)obj;
						if (!(missionControlClusterWorkable.gameObject == base.gameObject) && missionControlClusterWorkable.TargetClustercraft == Components.Clustercrafts[i])
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						this.boostableClustercraft.Add(Components.Clustercrafts[i]);
					}
				}
			}
			base.sm.WorkableRocketsAreInRange.Set(this.boostableClustercraft.Count > 0, base.smi, false);
		}

		// Token: 0x060099D1 RID: 39377 RVA: 0x0038F0F8 File Offset: 0x0038D2F8
		public Clustercraft GetRandomBoostableClustercraft()
		{
			return this.boostableClustercraft.GetRandom<Clustercraft>();
		}

		// Token: 0x060099D2 RID: 39378 RVA: 0x0038F105 File Offset: 0x0038D305
		private bool CanBeBoosted(Clustercraft clustercraft)
		{
			return clustercraft.controlStationBuffTimeRemaining == 0f && clustercraft.HasResourcesToMove(1, Clustercraft.CombustionResource.All) && clustercraft.IsFlightInProgress();
		}

		// Token: 0x060099D3 RID: 39379 RVA: 0x0038F12C File Offset: 0x0038D32C
		private bool IsOwnWorld(Clustercraft candidateClustercraft)
		{
			int myWorldId = base.gameObject.GetMyWorldId();
			WorldContainer interiorWorld = candidateClustercraft.ModuleInterface.GetInteriorWorld();
			return !(interiorWorld == null) && myWorldId == interiorWorld.id;
		}

		// Token: 0x060099D4 RID: 39380 RVA: 0x0038F165 File Offset: 0x0038D365
		public void ApplyEffect(Clustercraft clustercraft)
		{
			clustercraft.controlStationBuffTimeRemaining = 600f;
		}

		// Token: 0x040076BC RID: 30396
		private int clusterUpdatedHandle = -1;

		// Token: 0x040076BD RID: 30397
		private List<Clustercraft> boostableClustercraft = new List<Clustercraft>();

		// Token: 0x040076BE RID: 30398
		[MyCmpReq]
		public RoomTracker roomTracker;
	}

	// Token: 0x0200170C RID: 5900
	public class OperationalState : GameStateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.State
	{
		// Token: 0x040076BF RID: 30399
		public GameStateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.State WrongRoom;

		// Token: 0x040076C0 RID: 30400
		public GameStateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.State NoRockets;

		// Token: 0x040076C1 RID: 30401
		public GameStateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.State HasRockets;
	}
}
