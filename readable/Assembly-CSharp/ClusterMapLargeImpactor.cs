using System;
using KSerialization;

// Token: 0x02000B6D RID: 2925
public class ClusterMapLargeImpactor : GameStateMachine<ClusterMapLargeImpactor, ClusterMapLargeImpactor.Instance, IStateMachineTarget, ClusterMapLargeImpactor.Def>
{
	// Token: 0x0600569A RID: 22170 RVA: 0x001F85D8 File Offset: 0x001F67D8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.traveling;
		this.traveling.DefaultState(this.traveling.unidentified).EventTransition(GameHashes.ClusterDestinationReached, this.arrived, null);
		this.traveling.unidentified.ParamTransition<bool>(this.IsIdentified, this.traveling.identified, GameStateMachine<ClusterMapLargeImpactor, ClusterMapLargeImpactor.Instance, IStateMachineTarget, ClusterMapLargeImpactor.Def>.IsTrue);
		this.traveling.identified.ParamTransition<bool>(this.IsIdentified, this.traveling.unidentified, GameStateMachine<ClusterMapLargeImpactor, ClusterMapLargeImpactor.Instance, IStateMachineTarget, ClusterMapLargeImpactor.Def>.IsFalse).ToggleStatusItem(Db.Get().MiscStatusItems.ClusterMeteorRemainingTravelTime, null);
		this.arrived.DoNothing();
	}

	// Token: 0x04003A77 RID: 14967
	public StateMachine<ClusterMapLargeImpactor, ClusterMapLargeImpactor.Instance, IStateMachineTarget, ClusterMapLargeImpactor.Def>.BoolParameter IsIdentified;

	// Token: 0x04003A78 RID: 14968
	public ClusterMapLargeImpactor.TravelingState traveling;

	// Token: 0x04003A79 RID: 14969
	public GameStateMachine<ClusterMapLargeImpactor, ClusterMapLargeImpactor.Instance, IStateMachineTarget, ClusterMapLargeImpactor.Def>.State arrived;

	// Token: 0x02001CE4 RID: 7396
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400898C RID: 35212
		public string name;

		// Token: 0x0400898D RID: 35213
		public string description;

		// Token: 0x0400898E RID: 35214
		public string eventID;

		// Token: 0x0400898F RID: 35215
		public int destinationWorldID;

		// Token: 0x04008990 RID: 35216
		public float arrivalTime;
	}

	// Token: 0x02001CE5 RID: 7397
	public class TravelingState : GameStateMachine<ClusterMapLargeImpactor, ClusterMapLargeImpactor.Instance, IStateMachineTarget, ClusterMapLargeImpactor.Def>.State
	{
		// Token: 0x04008991 RID: 35217
		public GameStateMachine<ClusterMapLargeImpactor, ClusterMapLargeImpactor.Instance, IStateMachineTarget, ClusterMapLargeImpactor.Def>.State unidentified;

		// Token: 0x04008992 RID: 35218
		public GameStateMachine<ClusterMapLargeImpactor, ClusterMapLargeImpactor.Instance, IStateMachineTarget, ClusterMapLargeImpactor.Def>.State identified;
	}

	// Token: 0x02001CE6 RID: 7398
	public new class Instance : GameStateMachine<ClusterMapLargeImpactor, ClusterMapLargeImpactor.Instance, IStateMachineTarget, ClusterMapLargeImpactor.Def>.GameInstance
	{
		// Token: 0x17000C50 RID: 3152
		// (get) Token: 0x0600AF12 RID: 44818 RVA: 0x003D52E0 File Offset: 0x003D34E0
		public WorldContainer World_Destination
		{
			get
			{
				return ClusterManager.Instance.GetWorld(this.DestinationWorldID);
			}
		}

		// Token: 0x0600AF13 RID: 44819 RVA: 0x003D52F2 File Offset: 0x003D34F2
		public AxialI ClusterGridPosition()
		{
			return this.visualizer.Location;
		}

		// Token: 0x0600AF14 RID: 44820 RVA: 0x003D52FF File Offset: 0x003D34FF
		public Instance(IStateMachineTarget master, ClusterMapLargeImpactor.Def def) : base(master, def)
		{
			this.traveler.getSpeedCB = new Func<float>(this.GetSpeed);
			this.traveler.onTravelCB = new System.Action(this.OnTravellerMoved);
		}

		// Token: 0x0600AF15 RID: 44821 RVA: 0x003D533E File Offset: 0x003D353E
		private void OnTravellerMoved()
		{
			Game.Instance.Trigger(-1975776133, this);
		}

		// Token: 0x0600AF16 RID: 44822 RVA: 0x003D5350 File Offset: 0x003D3550
		protected override void OnCleanUp()
		{
			Components.LongRangeMissileTargetables.Remove(base.gameObject.GetComponent<ClusterGridEntity>());
			this.visualizer.Deselect();
			base.OnCleanUp();
		}

		// Token: 0x0600AF17 RID: 44823 RVA: 0x003D5378 File Offset: 0x003D3578
		public override void StartSM()
		{
			base.StartSM();
			if (this.DestinationWorldID < 0)
			{
				this.Setup(base.def.destinationWorldID, base.def.arrivalTime);
			}
			Components.LongRangeMissileTargetables.Add(base.gameObject.GetComponent<ClusterGridEntity>());
			this.RefreshVisuals(false);
		}

		// Token: 0x0600AF18 RID: 44824 RVA: 0x003D53CC File Offset: 0x003D35CC
		public void RefreshVisuals(bool playIdentifyAnimationIfVisible = false)
		{
			this.selectable.SetName(base.def.name);
			this.descriptor.description = base.def.description;
			this.visualizer.PlayRevealAnimation(playIdentifyAnimationIfVisible);
			base.Trigger(1980521255, null);
		}

		// Token: 0x0600AF19 RID: 44825 RVA: 0x003D5420 File Offset: 0x003D3620
		public void Setup(int destinationWorldID, float arrivalTime)
		{
			this.DestinationWorldID = destinationWorldID;
			this.ArrivalTime = arrivalTime;
			AxialI location = this.World_Destination.GetComponent<ClusterGridEntity>().Location;
			this.destinationSelector.SetDestination(location);
			this.traveler.RevalidatePath(false);
			ClusterFogOfWarManager.Instance smi = SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>();
			foreach (AxialI location2 in this.traveler.CurrentPath)
			{
				smi.RevealLocation(location2, 0, 0);
			}
			int count = this.traveler.CurrentPath.Count;
			float num = arrivalTime - GameUtil.GetCurrentTimeInCycles() * 600f;
			this.Speed = (float)count / num * 600f;
		}

		// Token: 0x0600AF1A RID: 44826 RVA: 0x003D54F0 File Offset: 0x003D36F0
		public float GetSpeed()
		{
			return this.Speed;
		}

		// Token: 0x04008993 RID: 35219
		[Serialize]
		public int DestinationWorldID = -1;

		// Token: 0x04008994 RID: 35220
		[Serialize]
		public float ArrivalTime;

		// Token: 0x04008995 RID: 35221
		[Serialize]
		private float Speed;

		// Token: 0x04008996 RID: 35222
		[MyCmpGet]
		private InfoDescription descriptor;

		// Token: 0x04008997 RID: 35223
		[MyCmpGet]
		private KSelectable selectable;

		// Token: 0x04008998 RID: 35224
		[MyCmpGet]
		private ClusterMapMeteorShowerVisualizer visualizer;

		// Token: 0x04008999 RID: 35225
		[MyCmpGet]
		private ClusterTraveler traveler;

		// Token: 0x0400899A RID: 35226
		[MyCmpGet]
		private ClusterDestinationSelector destinationSelector;
	}
}
