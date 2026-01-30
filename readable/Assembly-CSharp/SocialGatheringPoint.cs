using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000B56 RID: 2902
[SerializationConfig(MemberSerialization.OptIn)]
public class SocialGatheringPoint : StateMachineComponent<SocialGatheringPoint.StatesInstance>
{
	// Token: 0x060055BA RID: 21946 RVA: 0x001F3F70 File Offset: 0x001F2170
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.workables = new SocialGatheringPointWorkable[this.choreOffsets.Length];
		for (int i = 0; i < this.workables.Length; i++)
		{
			Vector3 pos = Grid.CellToPosCBC(Grid.OffsetCell(Grid.PosToCell(this), this.choreOffsets[i]), Grid.SceneLayer.Move);
			SocialGatheringPointWorkable socialGatheringPointWorkable = ChoreHelpers.CreateLocator("SocialGatheringPointWorkable", pos).AddOrGet<SocialGatheringPointWorkable>();
			socialGatheringPointWorkable.basePriority = this.basePriority;
			socialGatheringPointWorkable.specificEffect = this.socialEffect;
			socialGatheringPointWorkable.OnWorkableEventCB = new Action<Workable, Workable.WorkableEvent>(this.OnWorkableEvent);
			socialGatheringPointWorkable.SetWorkTime(this.workTime);
			this.workables[i] = socialGatheringPointWorkable;
		}
		this.tracker = new SocialChoreTracker(base.gameObject, this.choreOffsets);
		this.tracker.choreCount = this.choreCount;
		this.tracker.CreateChoreCB = new Func<int, Chore>(this.CreateChore);
		base.smi.StartSM();
		Components.SocialGatheringPoints.Add((int)Grid.WorldIdx[Grid.PosToCell(this)], this);
	}

	// Token: 0x060055BB RID: 21947 RVA: 0x001F407C File Offset: 0x001F227C
	protected override void OnCleanUp()
	{
		if (this.tracker != null)
		{
			this.tracker.Clear();
			this.tracker = null;
		}
		if (this.workables != null)
		{
			for (int i = 0; i < this.workables.Length; i++)
			{
				if (this.workables[i])
				{
					Util.KDestroyGameObject(this.workables[i]);
					this.workables[i] = null;
				}
			}
		}
		Components.SocialGatheringPoints.Remove((int)Grid.WorldIdx[Grid.PosToCell(this)], this);
		base.OnCleanUp();
	}

	// Token: 0x060055BC RID: 21948 RVA: 0x001F4100 File Offset: 0x001F2300
	private Chore CreateChore(int i)
	{
		Workable workable = this.workables[i];
		ChoreType relax = Db.Get().ChoreTypes.Relax;
		IStateMachineTarget target = workable;
		ChoreProvider chore_provider = null;
		bool run_until_complete = true;
		Action<Chore> on_complete = null;
		Action<Chore> on_begin = null;
		ScheduleBlockType recreation = Db.Get().ScheduleBlockTypes.Recreation;
		WorkChore<SocialGatheringPointWorkable> workChore = new WorkChore<SocialGatheringPointWorkable>(relax, target, chore_provider, run_until_complete, on_complete, on_begin, new Action<Chore>(this.OnSocialChoreEnd), false, recreation, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, false);
		workChore.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		workChore.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, workable);
		workChore.AddPrecondition(ChorePreconditions.instance.IsNotARobot, null);
		return workChore;
	}

	// Token: 0x060055BD RID: 21949 RVA: 0x001F418A File Offset: 0x001F238A
	private void OnSocialChoreEnd(Chore chore)
	{
		if (base.smi.IsInsideState(base.smi.sm.on))
		{
			this.tracker.Update(true);
		}
	}

	// Token: 0x060055BE RID: 21950 RVA: 0x001F41B5 File Offset: 0x001F23B5
	private void OnWorkableEvent(Workable workable, Workable.WorkableEvent workable_event)
	{
		if (workable_event == Workable.WorkableEvent.WorkStarted)
		{
			if (this.OnSocializeBeginCB != null)
			{
				this.OnSocializeBeginCB();
				return;
			}
		}
		else if (workable_event == Workable.WorkableEvent.WorkStopped && this.OnSocializeEndCB != null)
		{
			this.OnSocializeEndCB();
		}
	}

	// Token: 0x040039DC RID: 14812
	public CellOffset[] choreOffsets = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(1, 0)
	};

	// Token: 0x040039DD RID: 14813
	public int choreCount = 2;

	// Token: 0x040039DE RID: 14814
	public int basePriority;

	// Token: 0x040039DF RID: 14815
	public string socialEffect;

	// Token: 0x040039E0 RID: 14816
	public float workTime = 15f;

	// Token: 0x040039E1 RID: 14817
	public System.Action OnSocializeBeginCB;

	// Token: 0x040039E2 RID: 14818
	public System.Action OnSocializeEndCB;

	// Token: 0x040039E3 RID: 14819
	private SocialChoreTracker tracker;

	// Token: 0x040039E4 RID: 14820
	private SocialGatheringPointWorkable[] workables;

	// Token: 0x02001CBD RID: 7357
	public class States : GameStateMachine<SocialGatheringPoint.States, SocialGatheringPoint.StatesInstance, SocialGatheringPoint>
	{
		// Token: 0x0600AE75 RID: 44661 RVA: 0x003D3854 File Offset: 0x003D1A54
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			this.root.DoNothing();
			this.off.TagTransition(GameTags.Operational, this.on, false);
			this.on.TagTransition(GameTags.Operational, this.off, true).Enter("CreateChore", delegate(SocialGatheringPoint.StatesInstance smi)
			{
				smi.master.tracker.Update(true);
			}).Exit("CancelChore", delegate(SocialGatheringPoint.StatesInstance smi)
			{
				smi.master.tracker.Update(false);
			});
		}

		// Token: 0x04008912 RID: 35090
		public GameStateMachine<SocialGatheringPoint.States, SocialGatheringPoint.StatesInstance, SocialGatheringPoint, object>.State off;

		// Token: 0x04008913 RID: 35091
		public GameStateMachine<SocialGatheringPoint.States, SocialGatheringPoint.StatesInstance, SocialGatheringPoint, object>.State on;
	}

	// Token: 0x02001CBE RID: 7358
	public class StatesInstance : GameStateMachine<SocialGatheringPoint.States, SocialGatheringPoint.StatesInstance, SocialGatheringPoint, object>.GameInstance
	{
		// Token: 0x0600AE77 RID: 44663 RVA: 0x003D38FF File Offset: 0x003D1AFF
		public StatesInstance(SocialGatheringPoint smi) : base(smi)
		{
		}
	}
}
