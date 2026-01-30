using System;
using UnityEngine;

// Token: 0x0200048E RID: 1166
public class RationalAi : GameStateMachine<RationalAi, RationalAi.Instance>
{
	// Token: 0x060018C9 RID: 6345 RVA: 0x00089924 File Offset: 0x00087B24
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleStateMachine((RationalAi.Instance smi) => new DeathMonitor.Instance(smi.master, new DeathMonitor.Def())).Enter(delegate(RationalAi.Instance smi)
		{
			if (smi.HasTag(GameTags.Dead))
			{
				smi.GoTo(this.dead);
				return;
			}
			smi.GoTo(this.alive);
		});
		this.alive.TagTransition(GameTags.Dead, this.dead, false).Exit(new StateMachine<RationalAi, RationalAi.Instance, IStateMachineTarget, object>.State.Callback(RationalAi.IncreaseDeathCounterIfDying)).ToggleStateMachineList(new Func<RationalAi.Instance, Func<RationalAi.Instance, StateMachine.Instance>[]>(RationalAi.GetStateMachinesToRunWhenAlive));
		this.dead.ToggleStateMachine((RationalAi.Instance smi) => new FallWhenDeadMonitor.Instance(smi.master)).ToggleBrain("dead").Enter("RefreshUserMenu", delegate(RationalAi.Instance smi)
		{
			smi.RefreshUserMenu();
		}).Enter("DropStorage", delegate(RationalAi.Instance smi)
		{
			smi.GetComponent<Storage>().DropAll(false, false, default(Vector3), true, null);
		});
	}

	// Token: 0x060018CA RID: 6346 RVA: 0x00089A36 File Offset: 0x00087C36
	public static Func<RationalAi.Instance, StateMachine.Instance>[] GetStateMachinesToRunWhenAlive(RationalAi.Instance smi)
	{
		return smi.stateMachinesToRunWhenAlive;
	}

	// Token: 0x060018CB RID: 6347 RVA: 0x00089A3E File Offset: 0x00087C3E
	private static void IncreaseDeathCounterIfDying(RationalAi.Instance smi)
	{
		if (smi.HasTag(GameTags.Dead))
		{
			SaveGame.Instance.ColonyAchievementTracker.deadDupeCounter++;
		}
	}

	// Token: 0x04000E55 RID: 3669
	public GameStateMachine<RationalAi, RationalAi.Instance, IStateMachineTarget, object>.State alive;

	// Token: 0x04000E56 RID: 3670
	public GameStateMachine<RationalAi, RationalAi.Instance, IStateMachineTarget, object>.State dead;

	// Token: 0x020012AD RID: 4781
	public new class Instance : GameStateMachine<RationalAi, RationalAi.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008903 RID: 35075 RVA: 0x0034FBC0 File Offset: 0x0034DDC0
		public Instance(IStateMachineTarget master, Tag minionModel) : base(master)
		{
			this.MinionModel = minionModel;
			ChoreConsumer component = base.GetComponent<ChoreConsumer>();
			component.AddUrge(Db.Get().Urges.EmoteHighPriority);
			component.AddUrge(Db.Get().Urges.EmoteIdle);
			component.prioritizeBrainIfNoChore = true;
		}

		// Token: 0x06008904 RID: 35076 RVA: 0x0034FC11 File Offset: 0x0034DE11
		public void RefreshUserMenu()
		{
			Game.Instance.userMenu.Refresh(base.master.gameObject);
		}

		// Token: 0x04006882 RID: 26754
		public Tag MinionModel;

		// Token: 0x04006883 RID: 26755
		public Func<RationalAi.Instance, StateMachine.Instance>[] stateMachinesToRunWhenAlive;
	}
}
