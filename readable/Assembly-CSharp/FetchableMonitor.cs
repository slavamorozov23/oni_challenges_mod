using System;

// Token: 0x02000A26 RID: 2598
public class FetchableMonitor : GameStateMachine<FetchableMonitor, FetchableMonitor.Instance>
{
	// Token: 0x06004BF5 RID: 19445 RVA: 0x001B9700 File Offset: 0x001B7900
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.unfetchable;
		base.serializable = StateMachine.SerializeType.Never;
		this.fetchable.Enter("RegisterFetchable", delegate(FetchableMonitor.Instance smi)
		{
			smi.RegisterFetchable();
		}).Exit("UnregisterFetchable", delegate(FetchableMonitor.Instance smi)
		{
			smi.UnregisterFetchable();
		}).EventTransition(GameHashes.ReachableChanged, this.unfetchable, GameStateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Not(new StateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(this.IsFetchable))).EventTransition(GameHashes.AssigneeChanged, this.unfetchable, GameStateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Not(new StateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(this.IsFetchable))).EventTransition(GameHashes.EntombedChanged, this.unfetchable, GameStateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Not(new StateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(this.IsFetchable))).EventTransition(GameHashes.TagsChanged, this.unfetchable, GameStateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Not(new StateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(this.IsFetchable))).EventHandler(GameHashes.OnStore, new GameStateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.GameEvent.Callback(this.UpdateStorage)).EventHandler(GameHashes.StoragePriorityChanged, new GameStateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.GameEvent.Callback(this.UpdateStorage)).EventHandler(GameHashes.TagsChanged, new GameStateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.GameEvent.Callback(this.UpdateTags)).ParamTransition<bool>(this.forceUnfetchable, this.unfetchable, (FetchableMonitor.Instance smi, bool p) => !smi.IsFetchable());
		this.unfetchable.EventTransition(GameHashes.ReachableChanged, this.fetchable, new StateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(this.IsFetchable)).EventTransition(GameHashes.AssigneeChanged, this.fetchable, new StateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(this.IsFetchable)).EventTransition(GameHashes.EntombedChanged, this.fetchable, new StateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(this.IsFetchable)).EventTransition(GameHashes.TagsChanged, this.fetchable, new StateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(this.IsFetchable)).ParamTransition<bool>(this.forceUnfetchable, this.fetchable, new StateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Parameter<bool>.Callback(this.IsFetchable));
	}

	// Token: 0x06004BF6 RID: 19446 RVA: 0x001B98FF File Offset: 0x001B7AFF
	private bool IsFetchable(FetchableMonitor.Instance smi, bool param)
	{
		return this.IsFetchable(smi);
	}

	// Token: 0x06004BF7 RID: 19447 RVA: 0x001B9908 File Offset: 0x001B7B08
	private bool IsFetchable(FetchableMonitor.Instance smi)
	{
		return smi.IsFetchable();
	}

	// Token: 0x06004BF8 RID: 19448 RVA: 0x001B9910 File Offset: 0x001B7B10
	private void UpdateStorage(FetchableMonitor.Instance smi, object data)
	{
		Game.Instance.fetchManager.UpdateStorage(smi.pickupable.KPrefabID.PrefabID(), smi.fetchable, data as Storage);
	}

	// Token: 0x06004BF9 RID: 19449 RVA: 0x001B993D File Offset: 0x001B7B3D
	private void UpdateTags(FetchableMonitor.Instance smi, object data)
	{
		Game.Instance.fetchManager.UpdateTags(smi.pickupable.KPrefabID.PrefabID(), smi.fetchable);
	}

	// Token: 0x04003266 RID: 12902
	public GameStateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.State fetchable;

	// Token: 0x04003267 RID: 12903
	public GameStateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.State unfetchable;

	// Token: 0x04003268 RID: 12904
	public StateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.BoolParameter forceUnfetchable = new StateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.BoolParameter(false);

	// Token: 0x02001ADE RID: 6878
	public new class Instance : GameStateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600A76C RID: 42860 RVA: 0x003BC403 File Offset: 0x003BA603
		public Instance(Pickupable pickupable) : base(pickupable)
		{
			this.pickupable = pickupable;
			this.equippable = base.GetComponent<Equippable>();
		}

		// Token: 0x0600A76D RID: 42861 RVA: 0x003BC41F File Offset: 0x003BA61F
		public void RegisterFetchable()
		{
			this.fetchable = Game.Instance.fetchManager.Add(this.pickupable);
			Game.Instance.Trigger(-1588644844, base.gameObject);
		}

		// Token: 0x0600A76E RID: 42862 RVA: 0x003BC454 File Offset: 0x003BA654
		public void UnregisterFetchable()
		{
			Game.Instance.fetchManager.Remove(base.smi.pickupable.KPrefabID.PrefabID(), this.fetchable);
			Game.Instance.Trigger(-1491270284, base.gameObject);
		}

		// Token: 0x0600A76F RID: 42863 RVA: 0x003BC4A0 File Offset: 0x003BA6A0
		public void SetForceUnfetchable(bool is_unfetchable)
		{
			base.sm.forceUnfetchable.Set(is_unfetchable, base.smi, false);
		}

		// Token: 0x0600A770 RID: 42864 RVA: 0x003BC4BC File Offset: 0x003BA6BC
		public bool IsFetchable()
		{
			return !base.sm.forceUnfetchable.Get(this) && !this.pickupable.IsEntombed && this.pickupable.IsReachable() && (!(this.equippable != null) || !this.equippable.isEquipped) && !this.pickupable.KPrefabID.HasTag(GameTags.StoredPrivate) && !this.pickupable.KPrefabID.HasTag(GameTags.Creatures.ReservedByCreature) && (!this.pickupable.KPrefabID.HasTag(GameTags.Creature) || this.pickupable.KPrefabID.HasTag(GameTags.Creatures.Deliverable));
		}

		// Token: 0x04008300 RID: 33536
		public Pickupable pickupable;

		// Token: 0x04008301 RID: 33537
		private Equippable equippable;

		// Token: 0x04008302 RID: 33538
		public HandleVector<int>.Handle fetchable;
	}
}
