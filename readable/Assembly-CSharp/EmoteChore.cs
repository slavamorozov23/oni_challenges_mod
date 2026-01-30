using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020004A1 RID: 1185
public class EmoteChore : Chore<EmoteChore.StatesInstance>
{
	// Token: 0x0600191D RID: 6429 RVA: 0x0008C264 File Offset: 0x0008A464
	public EmoteChore(IStateMachineTarget target, ChoreType chore_type, Emote emote, int emoteIterations = 1, Func<StatusItem> get_status_item = null) : base(chore_type, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new EmoteChore.StatesInstance(this, target.gameObject, emote, KAnim.PlayMode.Once, emoteIterations, false);
		this.getStatusItem = get_status_item;
	}

	// Token: 0x0600191E RID: 6430 RVA: 0x0008C2AC File Offset: 0x0008A4AC
	public EmoteChore(IStateMachineTarget target, ChoreType chore_type, Emote emote, KAnim.PlayMode play_mode, int emoteIterations = 1, bool flip_x = false) : base(chore_type, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new EmoteChore.StatesInstance(this, target.gameObject, emote, play_mode, emoteIterations, flip_x);
	}

	// Token: 0x0600191F RID: 6431 RVA: 0x0008C2EC File Offset: 0x0008A4EC
	public EmoteChore(IStateMachineTarget target, ChoreType chore_type, HashedString animFile, HashedString[] anims, Func<StatusItem> get_status_item = null) : base(chore_type, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new EmoteChore.StatesInstance(this, target.gameObject, animFile, anims, KAnim.PlayMode.Once, false);
		this.getStatusItem = get_status_item;
	}

	// Token: 0x06001920 RID: 6432 RVA: 0x0008C334 File Offset: 0x0008A534
	public EmoteChore(IStateMachineTarget target, ChoreType chore_type, HashedString animFile, HashedString[] anims, KAnim.PlayMode play_mode, bool flip_x = false) : base(chore_type, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new EmoteChore.StatesInstance(this, target.gameObject, animFile, anims, play_mode, flip_x);
	}

	// Token: 0x06001921 RID: 6433 RVA: 0x0008C374 File Offset: 0x0008A574
	protected override StatusItem GetStatusItem()
	{
		if (this.getStatusItem == null)
		{
			return base.GetStatusItem();
		}
		return this.getStatusItem();
	}

	// Token: 0x06001922 RID: 6434 RVA: 0x0008C390 File Offset: 0x0008A590
	public override string ToString()
	{
		if (base.smi.animFile != null)
		{
			return "EmoteChore<" + base.smi.animFile.GetData().name + ">";
		}
		string str = "EmoteChore<";
		HashedString hashedString = base.smi.emoteAnims[0];
		return str + hashedString.ToString() + ">";
	}

	// Token: 0x06001923 RID: 6435 RVA: 0x0008C403 File Offset: 0x0008A603
	public void PairReactable(SelfEmoteReactable reactable)
	{
		this.reactable = reactable;
	}

	// Token: 0x06001924 RID: 6436 RVA: 0x0008C40C File Offset: 0x0008A60C
	protected new virtual void End(string reason)
	{
		if (this.reactable != null)
		{
			this.reactable.PairEmote(null);
			this.reactable.Cleanup();
			this.reactable = null;
		}
		base.End(reason);
	}

	// Token: 0x04000EAC RID: 3756
	private Func<StatusItem> getStatusItem;

	// Token: 0x04000EAD RID: 3757
	private SelfEmoteReactable reactable;

	// Token: 0x020012D3 RID: 4819
	public class StatesInstance : GameStateMachine<EmoteChore.States, EmoteChore.StatesInstance, EmoteChore, object>.GameInstance
	{
		// Token: 0x060089C9 RID: 35273 RVA: 0x00354914 File Offset: 0x00352B14
		public StatesInstance(EmoteChore master, GameObject emoter, Emote emote, KAnim.PlayMode mode, int emoteIterations, bool flip_x) : base(master)
		{
			this.mode = mode;
			this.animFile = emote.AnimSet;
			emote.CollectStepAnims(out this.emoteAnims, emoteIterations);
			base.sm.emoter.Set(emoter, base.smi, false);
		}

		// Token: 0x060089CA RID: 35274 RVA: 0x0035496C File Offset: 0x00352B6C
		public StatesInstance(EmoteChore master, GameObject emoter, HashedString animFile, HashedString[] anims, KAnim.PlayMode mode, bool flip_x) : base(master)
		{
			this.mode = mode;
			this.animFile = Assets.GetAnim(animFile);
			this.emoteAnims = anims;
			base.sm.emoter.Set(emoter, base.smi, false);
		}

		// Token: 0x04006945 RID: 26949
		public KAnimFile animFile;

		// Token: 0x04006946 RID: 26950
		public HashedString[] emoteAnims;

		// Token: 0x04006947 RID: 26951
		public KAnim.PlayMode mode = KAnim.PlayMode.Once;
	}

	// Token: 0x020012D4 RID: 4820
	public class States : GameStateMachine<EmoteChore.States, EmoteChore.StatesInstance, EmoteChore>
	{
		// Token: 0x060089CB RID: 35275 RVA: 0x003549BC File Offset: 0x00352BBC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			base.Target(this.emoter);
			this.root.ToggleAnims((EmoteChore.StatesInstance smi) => smi.animFile).PlayAnims((EmoteChore.StatesInstance smi) => smi.emoteAnims, (EmoteChore.StatesInstance smi) => smi.mode).ScheduleGoTo(10f, this.finish).OnAnimQueueComplete(this.finish);
			this.finish.ReturnSuccess();
		}

		// Token: 0x04006948 RID: 26952
		public StateMachine<EmoteChore.States, EmoteChore.StatesInstance, EmoteChore, object>.TargetParameter emoter;

		// Token: 0x04006949 RID: 26953
		public GameStateMachine<EmoteChore.States, EmoteChore.StatesInstance, EmoteChore, object>.State finish;
	}
}
