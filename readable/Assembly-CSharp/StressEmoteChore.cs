using System;
using UnityEngine;

// Token: 0x020004C1 RID: 1217
public class StressEmoteChore : Chore<StressEmoteChore.StatesInstance>
{
	// Token: 0x060019AE RID: 6574 RVA: 0x0008F9F0 File Offset: 0x0008DBF0
	public StressEmoteChore(IStateMachineTarget target, ChoreType chore_type, HashedString emote_kanim, HashedString[] emote_anims, KAnim.PlayMode play_mode, Func<StatusItem> get_status_item) : base(chore_type, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		this.AddPrecondition(ChorePreconditions.instance.IsMoving, null);
		this.AddPrecondition(ChorePreconditions.instance.IsOffLadder, null);
		this.AddPrecondition(ChorePreconditions.instance.NotInTube, null);
		this.AddPrecondition(ChorePreconditions.instance.IsAwake, null);
		this.getStatusItem = get_status_item;
		base.smi = new StressEmoteChore.StatesInstance(this, target.gameObject, emote_kanim, emote_anims, play_mode);
	}

	// Token: 0x060019AF RID: 6575 RVA: 0x0008FA7A File Offset: 0x0008DC7A
	protected override StatusItem GetStatusItem()
	{
		if (this.getStatusItem == null)
		{
			return base.GetStatusItem();
		}
		return this.getStatusItem();
	}

	// Token: 0x060019B0 RID: 6576 RVA: 0x0008FA98 File Offset: 0x0008DC98
	public override string ToString()
	{
		HashedString hashedString;
		if (base.smi.emoteKAnim.IsValid)
		{
			string str = "StressEmoteChore<";
			hashedString = base.smi.emoteKAnim;
			return str + hashedString.ToString() + ">";
		}
		string str2 = "StressEmoteChore<";
		hashedString = base.smi.emoteAnims[0];
		return str2 + hashedString.ToString() + ">";
	}

	// Token: 0x04000EE0 RID: 3808
	private Func<StatusItem> getStatusItem;

	// Token: 0x02001323 RID: 4899
	public class StatesInstance : GameStateMachine<StressEmoteChore.States, StressEmoteChore.StatesInstance, StressEmoteChore, object>.GameInstance
	{
		// Token: 0x06008B08 RID: 35592 RVA: 0x0035C6E7 File Offset: 0x0035A8E7
		public StatesInstance(StressEmoteChore master, GameObject emoter, HashedString emote_kanim, HashedString[] emote_anims, KAnim.PlayMode mode) : base(master)
		{
			this.emoteKAnim = emote_kanim;
			this.emoteAnims = emote_anims;
			this.mode = mode;
			base.sm.emoter.Set(emoter, base.smi, false);
		}

		// Token: 0x04006A61 RID: 27233
		public HashedString[] emoteAnims;

		// Token: 0x04006A62 RID: 27234
		public HashedString emoteKAnim;

		// Token: 0x04006A63 RID: 27235
		public KAnim.PlayMode mode = KAnim.PlayMode.Once;
	}

	// Token: 0x02001324 RID: 4900
	public class States : GameStateMachine<StressEmoteChore.States, StressEmoteChore.StatesInstance, StressEmoteChore>
	{
		// Token: 0x06008B09 RID: 35593 RVA: 0x0035C728 File Offset: 0x0035A928
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			base.Target(this.emoter);
			this.root.ToggleAnims((StressEmoteChore.StatesInstance smi) => smi.emoteKAnim).ToggleThought(Db.Get().Thoughts.Unhappy, null).PlayAnims((StressEmoteChore.StatesInstance smi) => smi.emoteAnims, (StressEmoteChore.StatesInstance smi) => smi.mode).OnAnimQueueComplete(null);
		}

		// Token: 0x04006A64 RID: 27236
		public StateMachine<StressEmoteChore.States, StressEmoteChore.StatesInstance, StressEmoteChore, object>.TargetParameter emoter;
	}
}
