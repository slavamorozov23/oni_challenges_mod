using System;
using UnityEngine;

// Token: 0x020004B6 RID: 1206
public class ReactEmoteChore : Chore<ReactEmoteChore.StatesInstance>
{
	// Token: 0x06001985 RID: 6533 RVA: 0x0008EA30 File Offset: 0x0008CC30
	public ReactEmoteChore(IStateMachineTarget target, ChoreType chore_type, EmoteReactable reactable, HashedString emote_kanim, HashedString[] emote_anims, KAnim.PlayMode play_mode, Func<StatusItem> get_status_item) : base(chore_type, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.basic, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		this.AddPrecondition(ChorePreconditions.instance.IsMoving, null);
		this.AddPrecondition(ChorePreconditions.instance.IsOffLadder, null);
		this.AddPrecondition(ChorePreconditions.instance.NotInTube, null);
		this.AddPrecondition(ChorePreconditions.instance.IsAwake, null);
		this.getStatusItem = get_status_item;
		base.smi = new ReactEmoteChore.StatesInstance(this, target.gameObject, reactable, emote_kanim, emote_anims, play_mode);
	}

	// Token: 0x06001986 RID: 6534 RVA: 0x0008EABC File Offset: 0x0008CCBC
	protected override StatusItem GetStatusItem()
	{
		if (this.getStatusItem == null)
		{
			return base.GetStatusItem();
		}
		return this.getStatusItem();
	}

	// Token: 0x06001987 RID: 6535 RVA: 0x0008EAD8 File Offset: 0x0008CCD8
	public override string ToString()
	{
		HashedString hashedString;
		if (base.smi.emoteKAnim.IsValid)
		{
			string str = "ReactEmoteChore<";
			hashedString = base.smi.emoteKAnim;
			return str + hashedString.ToString() + ">";
		}
		string str2 = "ReactEmoteChore<";
		hashedString = base.smi.emoteAnims[0];
		return str2 + hashedString.ToString() + ">";
	}

	// Token: 0x04000ED8 RID: 3800
	private Func<StatusItem> getStatusItem;

	// Token: 0x02001306 RID: 4870
	public class StatesInstance : GameStateMachine<ReactEmoteChore.States, ReactEmoteChore.StatesInstance, ReactEmoteChore, object>.GameInstance
	{
		// Token: 0x06008A73 RID: 35443 RVA: 0x00359278 File Offset: 0x00357478
		public StatesInstance(ReactEmoteChore master, GameObject emoter, EmoteReactable reactable, HashedString emote_kanim, HashedString[] emote_anims, KAnim.PlayMode mode) : base(master)
		{
			this.emoteKAnim = emote_kanim;
			this.emoteAnims = emote_anims;
			this.mode = mode;
			base.sm.reactable.Set(reactable, base.smi, false);
			base.sm.emoter.Set(emoter, base.smi, false);
		}

		// Token: 0x040069EF RID: 27119
		public HashedString[] emoteAnims;

		// Token: 0x040069F0 RID: 27120
		public HashedString emoteKAnim;

		// Token: 0x040069F1 RID: 27121
		public KAnim.PlayMode mode = KAnim.PlayMode.Once;
	}

	// Token: 0x02001307 RID: 4871
	public class States : GameStateMachine<ReactEmoteChore.States, ReactEmoteChore.StatesInstance, ReactEmoteChore>
	{
		// Token: 0x06008A74 RID: 35444 RVA: 0x003592E0 File Offset: 0x003574E0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			base.Target(this.emoter);
			this.root.ToggleThought((ReactEmoteChore.StatesInstance smi) => this.reactable.Get(smi).thought).ToggleExpression((ReactEmoteChore.StatesInstance smi) => this.reactable.Get(smi).expression).ToggleAnims((ReactEmoteChore.StatesInstance smi) => smi.emoteKAnim).ToggleThought(Db.Get().Thoughts.Unhappy, null).PlayAnims((ReactEmoteChore.StatesInstance smi) => smi.emoteAnims, (ReactEmoteChore.StatesInstance smi) => smi.mode).OnAnimQueueComplete(null).Enter(delegate(ReactEmoteChore.StatesInstance smi)
			{
				smi.master.GetComponent<Facing>().Face(Grid.CellToPos(this.reactable.Get(smi).sourceCell));
			});
		}

		// Token: 0x040069F2 RID: 27122
		public StateMachine<ReactEmoteChore.States, ReactEmoteChore.StatesInstance, ReactEmoteChore, object>.TargetParameter emoter;

		// Token: 0x040069F3 RID: 27123
		public StateMachine<ReactEmoteChore.States, ReactEmoteChore.StatesInstance, ReactEmoteChore, object>.ObjectParameter<EmoteReactable> reactable;
	}
}
