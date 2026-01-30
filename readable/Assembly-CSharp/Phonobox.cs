using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000A91 RID: 2705
[SerializationConfig(MemberSerialization.OptIn)]
public class Phonobox : StateMachineComponent<Phonobox.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x06004E8B RID: 20107 RVA: 0x001C905C File Offset: 0x001C725C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		GameScheduler.Instance.Schedule("Scheduling Tutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Schedule, true);
		}, null, null);
		this.workables = new PhonoboxWorkable[this.choreOffsets.Length];
		this.chores = new Chore[this.choreOffsets.Length];
		for (int i = 0; i < this.workables.Length; i++)
		{
			Vector3 pos = Grid.CellToPosCBC(Grid.OffsetCell(Grid.PosToCell(this), this.choreOffsets[i]), Grid.SceneLayer.Move);
			GameObject go = ChoreHelpers.CreateLocator("PhonoboxWorkable", pos);
			KSelectable kselectable = go.AddOrGet<KSelectable>();
			kselectable.SetName(this.GetProperName());
			kselectable.IsSelectable = false;
			PhonoboxWorkable phonoboxWorkable = go.AddOrGet<PhonoboxWorkable>();
			phonoboxWorkable.owner = this;
			this.workables[i] = phonoboxWorkable;
		}
	}

	// Token: 0x06004E8C RID: 20108 RVA: 0x001C9144 File Offset: 0x001C7344
	protected override void OnCleanUp()
	{
		this.UpdateChores(false);
		for (int i = 0; i < this.workables.Length; i++)
		{
			if (this.workables[i])
			{
				Util.KDestroyGameObject(this.workables[i]);
				this.workables[i] = null;
			}
		}
		base.OnCleanUp();
	}

	// Token: 0x06004E8D RID: 20109 RVA: 0x001C9198 File Offset: 0x001C7398
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
		WorkChore<PhonoboxWorkable> workChore = new WorkChore<PhonoboxWorkable>(relax, target, chore_provider, run_until_complete, on_complete, on_begin, new Action<Chore>(this.OnSocialChoreEnd), false, recreation, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
		workChore.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, workable);
		return workChore;
	}

	// Token: 0x06004E8E RID: 20110 RVA: 0x001C9200 File Offset: 0x001C7400
	private void OnSocialChoreEnd(Chore chore)
	{
		if (base.gameObject.HasTag(GameTags.Operational))
		{
			this.UpdateChores(true);
		}
	}

	// Token: 0x06004E8F RID: 20111 RVA: 0x001C921C File Offset: 0x001C741C
	public void UpdateChores(bool update = true)
	{
		for (int i = 0; i < this.choreOffsets.Length; i++)
		{
			Chore chore = this.chores[i];
			if (update)
			{
				if (chore == null || chore.isComplete)
				{
					this.chores[i] = this.CreateChore(i);
				}
			}
			else if (chore != null)
			{
				chore.Cancel("locator invalidated");
				this.chores[i] = null;
			}
		}
	}

	// Token: 0x06004E90 RID: 20112 RVA: 0x001C927B File Offset: 0x001C747B
	public void AddWorker(WorkerBase player)
	{
		this.players.Add(player);
		base.smi.sm.playerCount.Set(this.players.Count, base.smi, false);
	}

	// Token: 0x06004E91 RID: 20113 RVA: 0x001C92B2 File Offset: 0x001C74B2
	public void RemoveWorker(WorkerBase player)
	{
		this.players.Remove(player);
		base.smi.sm.playerCount.Set(this.players.Count, base.smi, false);
	}

	// Token: 0x06004E92 RID: 20114 RVA: 0x001C92EC File Offset: 0x001C74EC
	List<Descriptor> IGameObjectEffectDescriptor.GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(UI.BUILDINGEFFECTS.RECREATION, UI.BUILDINGEFFECTS.TOOLTIPS.RECREATION, Descriptor.DescriptorType.Effect);
		list.Add(item);
		Effect.AddModifierDescriptions(base.gameObject, list, "Danced", true);
		return list;
	}

	// Token: 0x04003463 RID: 13411
	public const string SPECIFIC_EFFECT = "Danced";

	// Token: 0x04003464 RID: 13412
	public const string TRACKING_EFFECT = "RecentlyDanced";

	// Token: 0x04003465 RID: 13413
	public CellOffset[] choreOffsets = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(-1, 0),
		new CellOffset(1, 0),
		new CellOffset(-2, 0),
		new CellOffset(2, 0)
	};

	// Token: 0x04003466 RID: 13414
	private PhonoboxWorkable[] workables;

	// Token: 0x04003467 RID: 13415
	private Chore[] chores;

	// Token: 0x04003468 RID: 13416
	private HashSet<WorkerBase> players = new HashSet<WorkerBase>();

	// Token: 0x04003469 RID: 13417
	private static string[] building_anims = new string[]
	{
		"working_loop",
		"working_loop2",
		"working_loop3"
	};

	// Token: 0x02001BAA RID: 7082
	public class States : GameStateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox>
	{
		// Token: 0x0600AAB2 RID: 43698 RVA: 0x003C4E20 File Offset: 0x003C3020
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.unoperational;
			this.unoperational.Enter(delegate(Phonobox.StatesInstance smi)
			{
				smi.SetActive(false);
			}).TagTransition(GameTags.Operational, this.operational, false).PlayAnim("off");
			this.operational.TagTransition(GameTags.Operational, this.unoperational, true).Enter("CreateChore", delegate(Phonobox.StatesInstance smi)
			{
				smi.master.UpdateChores(true);
			}).Exit("CancelChore", delegate(Phonobox.StatesInstance smi)
			{
				smi.master.UpdateChores(false);
			}).DefaultState(this.operational.stopped);
			this.operational.stopped.Enter(delegate(Phonobox.StatesInstance smi)
			{
				smi.SetActive(false);
			}).ParamTransition<int>(this.playerCount, this.operational.pre, (Phonobox.StatesInstance smi, int p) => p > 0).PlayAnim("on");
			this.operational.pre.PlayAnim("working_pre").OnAnimQueueComplete(this.operational.playing);
			this.operational.playing.Enter(delegate(Phonobox.StatesInstance smi)
			{
				smi.SetActive(true);
			}).ScheduleGoTo(25f, this.operational.song_end).ParamTransition<int>(this.playerCount, this.operational.post, (Phonobox.StatesInstance smi, int p) => p == 0).PlayAnim(new Func<Phonobox.StatesInstance, string>(Phonobox.States.GetPlayAnim), KAnim.PlayMode.Loop);
			this.operational.song_end.ParamTransition<int>(this.playerCount, this.operational.bridge, (Phonobox.StatesInstance smi, int p) => p > 0).ParamTransition<int>(this.playerCount, this.operational.post, (Phonobox.StatesInstance smi, int p) => p == 0);
			this.operational.bridge.PlayAnim("working_trans").OnAnimQueueComplete(this.operational.playing);
			this.operational.post.PlayAnim("working_pst").OnAnimQueueComplete(this.operational.stopped);
		}

		// Token: 0x0600AAB3 RID: 43699 RVA: 0x003C50D8 File Offset: 0x003C32D8
		public static string GetPlayAnim(Phonobox.StatesInstance smi)
		{
			int num = UnityEngine.Random.Range(0, Phonobox.building_anims.Length);
			return Phonobox.building_anims[num];
		}

		// Token: 0x04008574 RID: 34164
		public StateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.IntParameter playerCount;

		// Token: 0x04008575 RID: 34165
		public GameStateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.State unoperational;

		// Token: 0x04008576 RID: 34166
		public Phonobox.States.OperationalStates operational;

		// Token: 0x020029E9 RID: 10729
		public class OperationalStates : GameStateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.State
		{
			// Token: 0x0400B93A RID: 47418
			public GameStateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.State stopped;

			// Token: 0x0400B93B RID: 47419
			public GameStateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.State pre;

			// Token: 0x0400B93C RID: 47420
			public GameStateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.State bridge;

			// Token: 0x0400B93D RID: 47421
			public GameStateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.State playing;

			// Token: 0x0400B93E RID: 47422
			public GameStateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.State song_end;

			// Token: 0x0400B93F RID: 47423
			public GameStateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.State post;
		}
	}

	// Token: 0x02001BAB RID: 7083
	public class StatesInstance : GameStateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.GameInstance
	{
		// Token: 0x0600AAB5 RID: 43701 RVA: 0x003C5102 File Offset: 0x003C3302
		public StatesInstance(Phonobox smi) : base(smi)
		{
			this.operational = base.master.GetComponent<Operational>();
		}

		// Token: 0x0600AAB6 RID: 43702 RVA: 0x003C511C File Offset: 0x003C331C
		public void SetActive(bool active)
		{
			this.operational.SetActive(this.operational.IsOperational && active, false);
		}

		// Token: 0x04008577 RID: 34167
		private FetchChore chore;

		// Token: 0x04008578 RID: 34168
		private Operational operational;
	}
}
