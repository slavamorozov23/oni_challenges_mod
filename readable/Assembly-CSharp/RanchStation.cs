using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

// Token: 0x020007E6 RID: 2022
public class RanchStation : GameStateMachine<RanchStation, RanchStation.Instance, IStateMachineTarget, RanchStation.Def>
{
	// Token: 0x060035E3 RID: 13795 RVA: 0x00130220 File Offset: 0x0012E420
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.Operational;
		this.Unoperational.TagTransition(GameTags.Operational, this.Operational, false);
		this.Operational.TagTransition(GameTags.Operational, this.Unoperational, true).ToggleChore((RanchStation.Instance smi) => smi.CreateChore(), new Action<RanchStation.Instance, Chore>(RanchStation.SetRemoteChore), this.Unoperational, this.Unoperational).Update("FindRanachable", delegate(RanchStation.Instance smi, float dt)
		{
			smi.FindRanchable(null);
		}, UpdateRate.SIM_200ms, false);
	}

	// Token: 0x060035E4 RID: 13796 RVA: 0x001302CC File Offset: 0x0012E4CC
	private static void SetRemoteChore(RanchStation.Instance smi, Chore chore)
	{
		smi.remoteChore.SetChore(chore);
	}

	// Token: 0x040020CB RID: 8395
	public StateMachine<RanchStation, RanchStation.Instance, IStateMachineTarget, RanchStation.Def>.BoolParameter RancherIsReady;

	// Token: 0x040020CC RID: 8396
	public GameStateMachine<RanchStation, RanchStation.Instance, IStateMachineTarget, RanchStation.Def>.State Unoperational;

	// Token: 0x040020CD RID: 8397
	public RanchStation.OperationalState Operational;

	// Token: 0x02001749 RID: 5961
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04007745 RID: 30533
		public Func<GameObject, RanchStation.Instance, bool> IsCritterEligibleToBeRanchedCb;

		// Token: 0x04007746 RID: 30534
		public Action<GameObject, WorkerBase> OnRanchCompleteCb;

		// Token: 0x04007747 RID: 30535
		public Action<RanchedStates.Instance, Workable> OnRanchWorkBegins;

		// Token: 0x04007748 RID: 30536
		public Action<GameObject, float, Workable> OnRanchWorkTick;

		// Token: 0x04007749 RID: 30537
		public HashedString RanchedPreAnim = "idle_loop";

		// Token: 0x0400774A RID: 30538
		public HashedString RanchedLoopAnim = "idle_loop";

		// Token: 0x0400774B RID: 30539
		public HashedString RanchedPstAnim = "idle_loop";

		// Token: 0x0400774C RID: 30540
		public HashedString RanchedAbortAnim = "idle_loop";

		// Token: 0x0400774D RID: 30541
		public HashedString RancherInteractAnim = "anim_interacts_rancherstation_kanim";

		// Token: 0x0400774E RID: 30542
		public bool RancherWipesBrowAnim;

		// Token: 0x0400774F RID: 30543
		public StatusItem RanchingStatusItem = Db.Get().DuplicantStatusItems.Ranching;

		// Token: 0x04007750 RID: 30544
		public StatusItem CreatureRanchingStatusItem = Db.Get().CreatureStatusItems.GettingRanched;

		// Token: 0x04007751 RID: 30545
		public float WorkTime = 12f;

		// Token: 0x04007752 RID: 30546
		public Func<RanchStation.Instance, int> GetTargetRanchCell = (RanchStation.Instance smi) => Grid.PosToCell(smi);
	}

	// Token: 0x0200174A RID: 5962
	public class OperationalState : GameStateMachine<RanchStation, RanchStation.Instance, IStateMachineTarget, RanchStation.Def>.State
	{
	}

	// Token: 0x0200174B RID: 5963
	public new class Instance : GameStateMachine<RanchStation, RanchStation.Instance, IStateMachineTarget, RanchStation.Def>.GameInstance
	{
		// Token: 0x17000A72 RID: 2674
		// (get) Token: 0x06009A97 RID: 39575 RVA: 0x00391BE5 File Offset: 0x0038FDE5
		public RanchedStates.Instance ActiveRanchable
		{
			get
			{
				return this.activeRanchable;
			}
		}

		// Token: 0x17000A73 RID: 2675
		// (get) Token: 0x06009A98 RID: 39576 RVA: 0x00391BED File Offset: 0x0038FDED
		private bool isCritterAvailableForRanching
		{
			get
			{
				return this.targetRanchables.Count > 0;
			}
		}

		// Token: 0x17000A74 RID: 2676
		// (get) Token: 0x06009A99 RID: 39577 RVA: 0x00391BFD File Offset: 0x0038FDFD
		public bool IsCritterAvailableForRanching
		{
			get
			{
				this.ValidateTargetRanchables();
				return this.isCritterAvailableForRanching;
			}
		}

		// Token: 0x17000A75 RID: 2677
		// (get) Token: 0x06009A9A RID: 39578 RVA: 0x00391C0B File Offset: 0x0038FE0B
		public bool HasRancher
		{
			get
			{
				return this.rancher != null;
			}
		}

		// Token: 0x17000A76 RID: 2678
		// (get) Token: 0x06009A9B RID: 39579 RVA: 0x00391C19 File Offset: 0x0038FE19
		public bool IsRancherReady
		{
			get
			{
				return base.sm.RancherIsReady.Get(this);
			}
		}

		// Token: 0x17000A77 RID: 2679
		// (get) Token: 0x06009A9C RID: 39580 RVA: 0x00391C2C File Offset: 0x0038FE2C
		public Extents StationExtents
		{
			get
			{
				return this.station.GetExtents();
			}
		}

		// Token: 0x06009A9D RID: 39581 RVA: 0x00391C39 File Offset: 0x0038FE39
		public int GetRanchNavTarget()
		{
			return base.def.GetTargetRanchCell(this);
		}

		// Token: 0x06009A9E RID: 39582 RVA: 0x00391C4C File Offset: 0x0038FE4C
		public Instance(IStateMachineTarget master, RanchStation.Def def) : base(master, def)
		{
			base.gameObject.AddOrGet<RancherChore.RancherWorkable>();
			this.station = base.GetComponent<BuildingComplete>();
		}

		// Token: 0x06009A9F RID: 39583 RVA: 0x00391C80 File Offset: 0x0038FE80
		public Chore CreateChore()
		{
			RancherChore rancherChore = new RancherChore(base.GetComponent<KPrefabID>());
			StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.TargetParameter targetParameter = rancherChore.smi.sm.rancher;
			StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.Parameter<GameObject>.Context context = targetParameter.GetContext(rancherChore.smi);
			context.onDirty = (Action<RancherChore.RancherChoreStates.Instance>)Delegate.Combine(context.onDirty, new Action<RancherChore.RancherChoreStates.Instance>(this.OnRancherChanged));
			this.rancher = targetParameter.Get<WorkerBase>(rancherChore.smi);
			return rancherChore;
		}

		// Token: 0x06009AA0 RID: 39584 RVA: 0x00391CEA File Offset: 0x0038FEEA
		public int GetTargetRanchCell()
		{
			return base.def.GetTargetRanchCell(this);
		}

		// Token: 0x06009AA1 RID: 39585 RVA: 0x00391D00 File Offset: 0x0038FF00
		public override void StartSM()
		{
			base.StartSM();
			this.onRoomUpdatedHandle = base.Subscribe(144050788, new Action<object>(this.OnRoomUpdated));
			CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(this.GetTargetRanchCell());
			if (cavityForCell != null && cavityForCell.room != null)
			{
				this.OnRoomUpdated(cavityForCell.room);
			}
		}

		// Token: 0x06009AA2 RID: 39586 RVA: 0x00391D5D File Offset: 0x0038FF5D
		public override void StopSM(string reason)
		{
			base.StopSM(reason);
			base.Unsubscribe(ref this.onRoomUpdatedHandle);
		}

		// Token: 0x06009AA3 RID: 39587 RVA: 0x00391D72 File Offset: 0x0038FF72
		private void OnRoomUpdated(object data)
		{
			if (data == null)
			{
				return;
			}
			this.ranch = (data as Room);
			if (this.ranch.roomType != Db.Get().RoomTypes.CreaturePen)
			{
				this.TriggerRanchStationNoLongerAvailable();
				this.ranch = null;
			}
		}

		// Token: 0x06009AA4 RID: 39588 RVA: 0x00391DAD File Offset: 0x0038FFAD
		private void OnRancherChanged(RancherChore.RancherChoreStates.Instance choreInstance)
		{
			this.rancher = choreInstance.sm.rancher.Get<WorkerBase>(choreInstance);
			this.TriggerRanchStationNoLongerAvailable();
		}

		// Token: 0x06009AA5 RID: 39589 RVA: 0x00391DCC File Offset: 0x0038FFCC
		public bool TryGetRanched(RanchedStates.Instance ranchable)
		{
			return this.activeRanchable == null || this.activeRanchable == ranchable;
		}

		// Token: 0x06009AA6 RID: 39590 RVA: 0x00391DE1 File Offset: 0x0038FFE1
		public void MessageCreatureArrived(RanchedStates.Instance critter)
		{
			this.activeRanchable = critter;
			base.sm.RancherIsReady.Set(false, this, false);
			base.Trigger(-1357116271, null);
		}

		// Token: 0x06009AA7 RID: 39591 RVA: 0x00391E0A File Offset: 0x0039000A
		public void MessageRancherReady()
		{
			base.sm.RancherIsReady.Set(true, base.smi, false);
			this.MessageRanchables(GameHashes.RancherReadyAtRanchStation);
		}

		// Token: 0x06009AA8 RID: 39592 RVA: 0x00391E30 File Offset: 0x00390030
		private bool CanRanchableBeRanchedAtRanchStation(RanchableMonitor.Instance ranchable)
		{
			bool flag = !ranchable.IsNullOrStopped();
			if (flag && ranchable.TargetRanchStation != null && ranchable.TargetRanchStation != this)
			{
				flag = (!ranchable.TargetRanchStation.IsRunning() || !ranchable.TargetRanchStation.HasRancher);
			}
			flag = (flag && base.def.IsCritterEligibleToBeRanchedCb(ranchable.gameObject, this));
			flag = (flag && ranchable.ChoreConsumer.IsChoreEqualOrAboveCurrentChorePriority<RanchedStates>());
			if (flag)
			{
				int cell = Grid.PosToCell(ranchable.transform.GetPosition());
				CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(cell);
				if (cavityForCell == null || this.ranch == null || cavityForCell != this.ranch.cavity)
				{
					flag = false;
				}
				else
				{
					int cell2 = this.GetRanchNavTarget();
					if (ranchable.HasTag(GameTags.Creatures.Flyer))
					{
						cell2 = Grid.CellAbove(cell2);
					}
					flag = (ranchable.NavComponent.GetNavigationCost(cell2) != -1);
				}
			}
			return flag;
		}

		// Token: 0x06009AA9 RID: 39593 RVA: 0x00391F1C File Offset: 0x0039011C
		public void ValidateTargetRanchables()
		{
			if (!this.HasRancher)
			{
				return;
			}
			List<RanchableMonitor.Instance> list = CollectionPool<List<RanchableMonitor.Instance>, RanchableMonitor.Instance>.Get();
			list.AddRange(this.targetRanchables);
			foreach (RanchableMonitor.Instance instance in list)
			{
				if (instance.States == null || !this.CanRanchableBeRanchedAtRanchStation(instance))
				{
					this.Abandon(instance);
				}
			}
			CollectionPool<List<RanchableMonitor.Instance>, RanchableMonitor.Instance>.Release(list);
		}

		// Token: 0x06009AAA RID: 39594 RVA: 0x00391F9C File Offset: 0x0039019C
		public void FindRanchable(object _ = null)
		{
			if (this.ranch == null)
			{
				return;
			}
			this.ValidateTargetRanchables();
			if (this.targetRanchables.Count == 2)
			{
				return;
			}
			List<KPrefabID> creatures = this.ranch.cavity.creatures;
			if (this.HasRancher && !this.isCritterAvailableForRanching && creatures.Count == 0)
			{
				this.TryNotifyEmptyRanch();
			}
			for (int i = 0; i < creatures.Count; i++)
			{
				KPrefabID kprefabID = creatures[i];
				if (!(kprefabID == null))
				{
					RanchableMonitor.Instance smi = kprefabID.GetSMI<RanchableMonitor.Instance>();
					if (!this.targetRanchables.Contains(smi) && this.CanRanchableBeRanchedAtRanchStation(smi) && smi != null)
					{
						smi.States.SetRanchStation(this);
						this.targetRanchables.Add(smi);
						return;
					}
				}
			}
		}

		// Token: 0x06009AAB RID: 39595 RVA: 0x00392052 File Offset: 0x00390252
		public Option<CavityInfo> GetCavityInfo()
		{
			if (this.ranch.IsNullOrDestroyed())
			{
				return Option.None;
			}
			return this.ranch.cavity;
		}

		// Token: 0x06009AAC RID: 39596 RVA: 0x0039207C File Offset: 0x0039027C
		public void RanchCreature()
		{
			if (this.activeRanchable.IsNullOrStopped())
			{
				return;
			}
			global::Debug.Assert(this.activeRanchable != null, "targetRanchable was null");
			global::Debug.Assert(this.activeRanchable.GetMaster() != null, "GetMaster was null");
			global::Debug.Assert(base.def != null, "def was null");
			global::Debug.Assert(base.def.OnRanchCompleteCb != null, "onRanchCompleteCb cb was null");
			base.def.OnRanchCompleteCb(this.activeRanchable.gameObject, this.rancher);
			this.targetRanchables.Remove(this.activeRanchable.Monitor);
			this.activeRanchable.Trigger(1827504087, null);
			this.activeRanchable = null;
			this.FindRanchable(null);
		}

		// Token: 0x06009AAD RID: 39597 RVA: 0x00392144 File Offset: 0x00390344
		public void TriggerRanchStationNoLongerAvailable()
		{
			for (int i = this.targetRanchables.Count - 1; i >= 0; i--)
			{
				RanchableMonitor.Instance instance = this.targetRanchables[i];
				if (instance.IsNullOrStopped() || instance.States.IsNullOrStopped())
				{
					this.targetRanchables.RemoveAt(i);
				}
				else
				{
					this.targetRanchables.Remove(instance);
					instance.Trigger(1689625967, null);
				}
			}
			global::Debug.Assert(this.targetRanchables.Count == 0, "targetRanchables is not empty");
			this.activeRanchable = null;
			base.sm.RancherIsReady.Set(false, this, false);
		}

		// Token: 0x06009AAE RID: 39598 RVA: 0x003921E8 File Offset: 0x003903E8
		public void MessageRanchables(GameHashes hash)
		{
			for (int i = 0; i < this.targetRanchables.Count; i++)
			{
				RanchableMonitor.Instance instance = this.targetRanchables[i];
				if (!instance.IsNullOrStopped())
				{
					Game.BrainScheduler.PrioritizeBrain(instance.GetComponent<CreatureBrain>());
					if (!instance.States.IsNullOrStopped())
					{
						instance.Trigger((int)hash, null);
					}
				}
			}
		}

		// Token: 0x06009AAF RID: 39599 RVA: 0x00392248 File Offset: 0x00390448
		public void Abandon(RanchableMonitor.Instance critter)
		{
			if (critter == null)
			{
				global::Debug.LogWarning("Null critter trying to abandon ranch station");
				this.targetRanchables.Remove(critter);
				return;
			}
			critter.TargetRanchStation = null;
			if (this.targetRanchables.Remove(critter))
			{
				if (critter.States == null)
				{
					return;
				}
				bool flag = !this.isCritterAvailableForRanching;
				if (critter.States == this.activeRanchable)
				{
					flag = true;
					this.activeRanchable = null;
				}
				if (flag)
				{
					this.TryNotifyEmptyRanch();
				}
			}
		}

		// Token: 0x06009AB0 RID: 39600 RVA: 0x003922B8 File Offset: 0x003904B8
		private void TryNotifyEmptyRanch()
		{
			if (!this.HasRancher)
			{
				return;
			}
			this.rancher.Trigger(-364750427, null);
		}

		// Token: 0x06009AB1 RID: 39601 RVA: 0x003922D4 File Offset: 0x003904D4
		public bool IsCritterInQueue(RanchableMonitor.Instance critter)
		{
			return this.targetRanchables.Contains(critter);
		}

		// Token: 0x06009AB2 RID: 39602 RVA: 0x003922E2 File Offset: 0x003904E2
		public List<RanchableMonitor.Instance> DEBUG_GetTargetRanchables()
		{
			return this.targetRanchables;
		}

		// Token: 0x04007753 RID: 30547
		[MyCmpAdd]
		public ManuallySetRemoteWorkTargetComponent remoteChore;

		// Token: 0x04007754 RID: 30548
		private const int QUEUE_SIZE = 2;

		// Token: 0x04007755 RID: 30549
		private List<RanchableMonitor.Instance> targetRanchables = new List<RanchableMonitor.Instance>();

		// Token: 0x04007756 RID: 30550
		private RanchedStates.Instance activeRanchable;

		// Token: 0x04007757 RID: 30551
		private Room ranch;

		// Token: 0x04007758 RID: 30552
		private WorkerBase rancher;

		// Token: 0x04007759 RID: 30553
		private BuildingComplete station;

		// Token: 0x0400775A RID: 30554
		private int onRoomUpdatedHandle = -1;
	}
}
