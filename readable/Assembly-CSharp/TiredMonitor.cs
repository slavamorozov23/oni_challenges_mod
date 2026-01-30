using System;

// Token: 0x02000A53 RID: 2643
public class TiredMonitor : GameStateMachine<TiredMonitor, TiredMonitor.Instance>
{
	// Token: 0x06004CEF RID: 19695 RVA: 0x001BFB3C File Offset: 0x001BDD3C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.EventTransition(GameHashes.SleepFail, this.tired, null);
		this.tired.Enter(delegate(TiredMonitor.Instance smi)
		{
			smi.SetInterruptDay();
		}).EventTransition(GameHashes.NewDay, (TiredMonitor.Instance smi) => GameClock.Instance, this.root, (TiredMonitor.Instance smi) => smi.AllowInterruptClear()).ToggleExpression(Db.Get().Expressions.Tired, null).ToggleAnims("anim_loco_walk_slouch_kanim", 0f).ToggleAnims("anim_idle_slouch_kanim", 0f);
	}

	// Token: 0x04003343 RID: 13123
	public GameStateMachine<TiredMonitor, TiredMonitor.Instance, IStateMachineTarget, object>.State tired;

	// Token: 0x02001B5E RID: 7006
	public new class Instance : GameStateMachine<TiredMonitor, TiredMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600A9A4 RID: 43428 RVA: 0x003C21AE File Offset: 0x003C03AE
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x0600A9A5 RID: 43429 RVA: 0x003C21C5 File Offset: 0x003C03C5
		public void SetInterruptDay()
		{
			this.interruptedDay = GameClock.Instance.GetCycle();
		}

		// Token: 0x0600A9A6 RID: 43430 RVA: 0x003C21D7 File Offset: 0x003C03D7
		public bool AllowInterruptClear()
		{
			bool flag = GameClock.Instance.GetCycle() > this.interruptedDay + 1;
			if (flag)
			{
				this.interruptedDay = -1;
			}
			return flag;
		}

		// Token: 0x040084B6 RID: 33974
		public int disturbedDay = -1;

		// Token: 0x040084B7 RID: 33975
		public int interruptedDay = -1;
	}
}
